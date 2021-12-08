using ETicaret.Core.Helpers;
using ETicaret.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.Core.Controllers
{
    public class CustomerCardController : Controller
    {
        private eticaret_coreContext db;
        public CustomerCardController(eticaret_coreContext db)
        {
            this.db = db;
        }

        [HttpPost]
        public IActionResult AddToCart([FromBody] AddToCartRequest req)
        {

            if(ViewData.ModelState.ErrorCount > 0)
            {
                var modelstateErrors = ViewData.ModelState.ToList();
                
            }
            var query = db.CartItems.Where((x) => x.CustomerIdentify == req.CustomerIdentify && x.ProductId == req.Product.Id);
            var product = db.Products.Where(x => x.Id == req.Product.Id).FirstOrDefault();
            CartItem item = null;

            if (query.Count() > 0)
            {
                item = query.First(); //

                item.Quantity += req.Quantity;
            }

            else
            {
                item = new CartItem
                {
                    ProductId = req.Product.Id,
                    CustomerIdentify = req.CustomerIdentify,
                    Quantity = req.Quantity,
                };
                db.CartItems.Add(item);
            }

            if (item.Quantity <= 0)
            {
                db.CartItems.Remove(item);
            }
            if (item.Quantity > product.Quantity)
            {
                item.Quantity = product.Quantity;
            }
            db.SaveChanges();
            return Json(req);
        }
        [HttpPost]
        public IActionResult GetCartItems([FromBody] GetCartRequest req)
        {
            var query = db.CartItems.Where(x => x.CustomerIdentify == req.CustomerIdentify)
                .Include(x => x.Product)
                    .ThenInclude(y => y.ProductImages).
                ToList();
            return Json(new GetCartResult { CartItems = query });
        }
        [AuthenticationTokenHandler]
        [HttpPost]
        public IActionResult PlaceOrder([FromBody] PlaceOrderRequest req)
        {
            var customer = (Customer)HttpContext.Items["Customer"];

            if (req.AddressId == null)
            {
                return Json(new GenericResponse("Adres Seçiniz", false));
            }
            var dbCartItems = db.CartItems.Where(x => x.CustomerIdentify == req.CustomerIdentify)
                .Include(x => x.Product).ToList();

            var cartResult = new GetCartResult();
            cartResult.CartItems = dbCartItems;


            var order = new Order();
            order.AddressId = req.AddressId;
            order.CustomerId = customer.Id;
            order.OrderStatuId = 1;
            order.TotalPrice = cartResult.CartTotalPrice;
            order.Date = DateTime.Now;

            foreach (var item in dbCartItems)
            {
                var orderDetail = new OrderDetail();
                orderDetail.ProductId = item.ProductId;
                orderDetail.ProductPrice = item.TotalPrice;
                orderDetail.Quantity = item.Quantity;

                order.OrderDetails.Add(orderDetail);
                db.CartItems.Remove(item);
                item.Product.Quantity -= item.Quantity;
                if (item.Product.Quantity < 0)
                {
                    return Json(new GenericResponse("Stok bulunmamaktadır", false));
                }
                if (item.Product.Quantity == 0)
                {
                    item.Product.StockStatuId = 3;
                }
            }


            db.Orders.Add(order);

            db.SaveChanges();
            return Json(new GenericResponse("Siparişiniz alınmıştır", true, order));

        }


        [AuthenticationTokenHandler]
        [HttpPost]
        public IActionResult GetOrders()
        {
            var customer = (Customer)HttpContext.Items["Customer"];
            var query = db.Orders.Where(x => x.CustomerId == customer.Id).
                  Include(x => x.Address).
            Include(x => x.OrderDetails).
                    ThenInclude(y => y.Product).
                         ThenInclude(i => i.ProductImages).ToList();

            return Json(query);
        }
        [AuthenticationTokenHandler]
        [HttpPost]
        public IActionResult GetOrderById([FromBody] DetailsWithIdRequest req)
        {
            var query = db.Orders.Where(x => x.Id == req.Id).
                    Include(x => x.Address).
                    Include(x => x.OrderDetails).
                         ThenInclude(y => y.Product).
                               ThenInclude(z => z.ProductImages).FirstOrDefault();
            return Json(query);
        }
        public class AddToCartRequest
        {
            public Product Product { get; set; }

            public int Quantity { get; set; }
            public string CustomerIdentify { get; set; }
        }
        public class GetCartRequest
        {

            public string CustomerIdentify { get; set; }
        }
        public class GetCartResult
        {
            public List<CartItem> CartItems { get; set; }
            public decimal? CartTotalPrice
            {
                get
                {
                    decimal? toplam = 0;
                    foreach (var cartItem in CartItems)
                    {
                        toplam += cartItem.TotalPrice;
                    }
                    return toplam;
                }

            }
        }
        public class PlaceOrderRequest
        {
            public int? AddressId { get; set; }
            public string CustomerIdentify { get; set; }
        }

    }
}
