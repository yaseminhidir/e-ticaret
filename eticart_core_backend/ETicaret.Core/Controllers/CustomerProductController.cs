using ETicaret.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.Core.Controllers
{
    public class CustomerProductController : Controller
    {
        private eticaret_coreContext db;
        public CustomerProductController(eticaret_coreContext db)
        {
            this.db = db;
        }

        [HttpPost]
        public IActionResult GetProducts([FromBody] CustomerProductListFilteredRequest req)
        {
            var category = db.Categories.Where(x => x.Id == req.CategoryId).FirstOrDefault();
            var query = db.Products.Where(x => !x.IsDeleted && x.ProductCategories.Any(y => y.CategoryId == req.CategoryId));
            /*
                        if (req.SelectedStatus.Count > 0)
                        {
                            // 1 , 2 , 3 
                            foreach (var statusId in req.SelectedStatus)
                            {
                                query = query.Where(x => x.StockStatuId == statusId);

                                // ilk dongude sorgu = products where StockStatuId = 1
                                // ikinci "     "    = products where StockStatuId = 1 and StockStatuId == 2
                                // ücüncü "     "    = products where StockStatuId = 1 and StockStatuId == 2 and StockStatuId == 3
                            }
                            Bir ürünün yalnızca bir stockstatu id si olabilir. Aynı anda 1,2,3 olamaz.
                            // query = query.Where(product => req.SelectedStatus.Contains(product.StockStatuId ?? 0));

                        }
            */
            if(req.MaxPrice !=0||req.MinPrice != 0)
            {
                query = query.Where(x => x.Price >= req.MinPrice && x.Price <= req.MaxPrice);
            }
            if (req.SelectedStatus.Count > 0)
            {

                // where StockStatusId in (1,2,3)
                query = query.Where(product => req.SelectedStatus.Contains(product.StockStatuId ?? 0));

            }
            
            if (req.SelectedManufacturersId.Count > 0)
            {
                query = query.Where(product => req.SelectedManufacturersId.Contains(product.ManufacturerId ?? 0));
            }
            var products = query
                .Include(x => x.Manufacturer)
                .Include(x => x.ProductImages)
                .Include(x => x.StockStatu);
            // query var mı?
            var exists = query.Any();

            return Json(new CategoryResponse
            {
                Products = products.ToList(),
                //tüm priceları karşılaştırır, sadece en büyüğünü döndürür.
                /*
                 * if(exists) {
                 *  return query.Max(x => x.Price ?? 0) 
                 * }
                 * else {
                 *  return 0
                 * }
                 * */
                MaxPrice = exists ? query.Max(x => x.Price ?? 0) : 0,
                //tüm priceları karşılaştırır, sadece en küçüğünü döndürür.
                MinPrice = exists ? query.Min(x => x.Price ?? 0) : 0,

                Category = category,
            });
        }
        
        [HttpPost]
        public IActionResult GetMostSellers()
        {
            var query = db.Products.Where(x => !x.IsDeleted).OrderByDescending(x => x.Favs.Count()).Take(4)
                .Include(x=>x.ProductImages)
                .ToList();
            return Json(query);
        }
        [HttpPost]
        public IActionResult GetProductByID([FromBody] DetailsWithIdRequest req) 
        {
            var categories = db.Categories.ToList();

            foreach (var category in categories)
            {
                category.FullName = category.Name;
                if (category.ParentId != null)
                {
                    int? lastParentId = category.ParentId;
                    while (lastParentId != null)
                    {
                        var parent = categories.Where(x => x.Id == lastParentId && !x.IsDeleted).FirstOrDefault();

                        category.FullName = parent?.Name + " > " + category.FullName;

                        lastParentId = parent.ParentId;
                    }
                }
            }
            var product = db.Products.Where(x => x.Id == req.Id)
                .Include(x => x.Manufacturer)
                .Include(x => x.ProductImages)
                .Include(x => x.StockStatu)
                .Include(x => x.ProductCategories)
                    .ThenInclude(y => y.Category).FirstOrDefault();
                        
            return Json(product);
        }
        [HttpPost]
        public IActionResult GetCategoriesTopMenu()
        {
            var categories = db.Categories.Where(x => x.ParentId == null)
                .Include(x => x.InverseParent).ToList();
            return Json(categories);
        }
        [HttpPost]
        public IActionResult GetCategories()
        {
            //childları da recursive olarak listeliyo.
            var categories = db.Categories.ToList();
            //kök parentları filtreliyo, kök parentın içinde childlar da dahil.
            return Json(categories.Where(x => x.ParentId == null));
        }
        [HttpPost]
        public IActionResult GetStockStatus()
        {
            var stockstatus = db.StockStatuses.ToList();
            return Json(stockstatus);
        }
        [HttpPost]
        public IActionResult GetManufacturers()
        {
            var manufacturers = db.Manufacturers.Where(x => !x.IsDeleted).ToList();
            return Json(manufacturers);
        }

    }
    public class CategoryResponse
    {
        public List<Product> Products { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public Category Category { get; set; }

    }
    public class CustomerProductListFilteredRequest
    {
        public int CategoryId { get; set; }
        public List<int> SelectedStatus { get; set; }
        public List<int> SelectedManufacturersId { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }

    }
}



