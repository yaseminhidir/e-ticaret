using ETicaret.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.Core.Controllers
{
    public class AdminOrderController : Controller
    {
        private eticaret_coreContext db;
        public AdminOrderController(eticaret_coreContext db)
        {
            this.db = db;
        }
        [HttpPost]
        public IActionResult GetOrders()
        {
            var query = db.Orders.
            Include(x => x.Address).
            Include(x => x.OrderStatu).
            Include(x => x.OrderDetails).
                ThenInclude(y => y.Product).
                 ThenInclude(i => i.ProductImages).
            Include(x => x.Customer).ToList();

            return Json(query);
        }
        [HttpPost]
        public IActionResult GetOrderStatus()
        {
            var status = db.OrderStatuses.ToList();

            return Json(status);
        }
        [HttpPost]
        public IActionResult GetOrderById([FromBody] DetailsWithIdRequest req)
        {
            var query = db.Orders.Where(x => x.Id == req.Id).
                    Include(x => x.Address).
                    Include(x => x.OrderStatu).
                    Include(x => x.OrderDetails).
                         ThenInclude(y => y.Product).
                               ThenInclude(z => z.ProductImages).FirstOrDefault();
            return Json(query);
        }
        [HttpPost]
        public IActionResult UpdateStatu([FromBody] UpdateOrderStatuRequest req)
        {
            var query = db.Orders.Where(x => x.Id == req.Id).FirstOrDefault();
            query.OrderStatuId = req.StatuId;
            db.SaveChanges();
            return Json(new { message = "Başarıyla güncellendi" });
        }
        [HttpPost]
        public IActionResult EditAddress([FromBody] Address address)
        {
            db.Addresses.Attach(address);
            db.Entry(address).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { message = "Başarıyla güncellendi" });
        }
      [HttpPost]
        public IActionResult CancelProduct([FromBody] OrderDetail orderDetail)
        {
            var query = db.OrderDetails.Where(x => x.Id == orderDetail.Id).FirstOrDefault();
            query.IsCanceled = true;
            var product = db.Products.Where(x => x.Id == orderDetail.ProductId).FirstOrDefault();
            product.Quantity = product.Quantity + orderDetail.Quantity;
            var order = db.Orders.Where(x => x.Id == orderDetail.OrderId).FirstOrDefault();
            order.TotalPrice = order.TotalPrice - (orderDetail.ProductPrice * orderDetail.Quantity);
            
            db.SaveChanges();
            return Json(new { message = "Başarıyla silindi" });
        }
     
        public class UpdateOrderStatuRequest
        {
            public int StatuId { get; set; }
            public int Id { get; set; }
        }

        public class CancelProductRequest
        {
            public Product Product { get; set; }
            public int Id { get; set; }
        }
    }
}
