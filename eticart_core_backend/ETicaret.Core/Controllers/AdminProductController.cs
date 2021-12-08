using ETicaret.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.Core.Controllers
{
    public class AdminProductController : Controller
    {
        private eticaret_coreContext db;
        public AdminProductController(eticaret_coreContext db)
        {
            this.db = db;
        }
        [HttpPost]
        public ActionResult GetStockStatus()
        {
            var StockStatus = db.StockStatuses.ToList();
            return Json(StockStatus);
        }
        [HttpPost]
        public IActionResult UploadImage(List<IFormFile> files)
        {
            if (!Directory.Exists("uploads"))
            {
                Directory.CreateDirectory("uploads");
            }
            List<string> Urls = new List<string>();

            foreach (var file in files)
            {
              var filePath = Path.Join("uploads", file.FileName);

                using (FileStream fs = System.IO.File.Open(filePath,FileMode.OpenOrCreate))
                {
                    file.CopyTo(fs);
                    // url oluşturma
                    Urls.Add("/uploads/" + file.FileName); 

                }
                
            }
            return Json(Urls);
        }
        [HttpPost]
       
        public ActionResult GetProducts([FromBody] ProductListFilteredRequest request)
        {
            db.ChangeTracker.LazyLoadingEnabled = false;
            // (isDeleted == false) && (kategori id si içinde olanlar)
            var query = db.Products.Where(x => !x.IsDeleted);

            if (request.CategoryIds.Count > 0)
            {
                //birden fazla filtre
                query = query.Where(product =>
                    product.ProductCategories.Any(product_category =>
                        request.CategoryIds.Contains(product_category.CategoryId ?? 0)));
            }

            if (request.ManufacturerIds.Count > 0)
            {
                query = query.Where(product => request.ManufacturerIds.Contains(product.ManufacturerId ?? 0));
            }
            if (request.stockMax >= 0)
            {
                query = query.Where(x => x.Quantity <= request.stockMax);
            }
            if (request.stockMin >= 0)
            {
                query = query.Where(x => x.Quantity >= request.stockMin);

            }
            query = query.Include(x => x.ProductImages)
               .Include(x => x.StockStatu)
               ;

            return Json(query.ToList());
        }
        /*
        [HttpPost]
        public ActionResult GetProductsFromManufacturerId([FromBody] List<int> SelectedManufacturers)
        {
            List<Product> products = new List<Product>();


            foreach (var manufacturerId in SelectedManufacturers)
            {
                var query = db.Products.Where(x => x.ManufacturerId == manufacturerId && !x.IsDeleted).FirstOrDefault();
                products.Add(query);
            }
            return Json(products);
        }
        
        [HttpPost]
        public ActionResult GetProductsFromCategoryId([FromBody] List<int> SelectedCategories)
        {
            // <Product>= Product.cs (Bu arrayin türü Product classı, Yani propertyleri Db deki sütun isimleriyle aynı)
            //yeni bir değişken olduğu için boş
            //yeni bir liste olsun, listemin türü Product olsun
            // js deki karşılığı: var Products=[];
            List<Product> products = new List<Product>();
            foreach (var categoryId in SelectedCategories)

            {
                //ProductCategories tablosunda ProductId foreign key olduğu için IsDeleted sütunu bu sorguda Product.Isdeleted şeklinde filtrelenebilir.
                //Tolist dediğimizde IsDeleted true olsa bile ürün null olarak gelmez.
                var query = db.ProductCategories.Where(x => x.CategoryId == categoryId && !x.Product.IsDeleted).ToList();
                foreach (var product in query)
                {
                    //IsDeleted bu sorguda filtrelendiğinde; first or default olduğu için true ise null döner ve frontendde hataya seebep olur. 
                    var query2 = db.Products.Where(x => x.Id == product.ProductId).Include(x => x.ProductImages).Include(x => x.StockStatu).Include(x => x.ProductCategories).ThenInclude(y => y.Category).FirstOrDefault();
                    products.Add(query2);
                }
            }
            return Json(products);
        }
        */

        [HttpPost]

        public ActionResult AddManufacturer([FromBody] Manufacturer Manufacturer)
        {
            if (Manufacturer.Id == 0)
            {
                if (string.IsNullOrWhiteSpace(Manufacturer.Name))

                {
                    return Json(new { message = "Manufacturer name boş olamaz", success = false });
                }
                else
                {
                    Manufacturer.DateAdded = DateTime.Now;
                    db.Manufacturers.Add(Manufacturer);
                    db.SaveChanges();
                    return Json(new { message = "eklendi" });
                }


            }

            if (Manufacturer.Id != 0)
            {
                Manufacturer.DateModified = DateTime.Now;
                db.Manufacturers.Attach(Manufacturer);
                db.Entry(Manufacturer).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { message = "Başarıyla güncellendi" });

            }
            return NotFound();
        }
        [HttpPost]
        public ActionResult DeleteManufacturers([FromBody] List<int> DeleteManufacturers)
        {
            foreach (var manufacturerId in DeleteManufacturers)
            {
                var manufacturer = db.Manufacturers.Where(x => x.Id == manufacturerId).FirstOrDefault();
                manufacturer.IsDeleted = true;
            }
            db.SaveChanges();
            return Json(new { message = "Başarıyla Silindi!" });
        }
        [HttpPost]
        public ActionResult GetManufacturers()
        {
            var manufacturers = db.Manufacturers.Where(x => !x.IsDeleted).ToList();
            return Json(manufacturers);
        }
        [HttpPost]
        public ActionResult GetManufacturerFromId([FromBody] DetailsWithIdRequest req)
        {
            var manufacturer = db.Manufacturers.Where(x => x.Id == req.Id).FirstOrDefault();
            return Json(manufacturer);
        }
        [HttpPost]
        public ActionResult GetCategories()
        {
            var Categories = db.Categories.ToList();

            foreach (var cat in Categories)
            {
                cat.FullName = cat.Name;
                if (cat.ParentId != null)
                {
                    //lastParentId 'in içerisinde değer olduğu sürece devam edecek.
                    int? lastParentId = cat.ParentId;
                    while (lastParentId != null)
                    {
                        // Id'si Parent ıd ye eşit olanı filtreleme
                        var parent = Categories.Where(x => x.Id == lastParentId && !x.IsDeleted).FirstOrDefault();

                        cat.FullName = parent?.Name + " > " + cat.FullName;

                        lastParentId = parent?.ParentId;
                    }
                }
            }

            return Json(Categories);
        }
        [HttpPost]
        public ActionResult AddProduct([FromBody] Product Product)
        {
            if (Product.Id == 0)
            {
                Product.DateAdded = DateTime.Now;
              
                db.Products.Add(Product);
                db.SaveChanges();
                return Json(new { message = "Ürün başarıyla eklendi", id = Product.Id });

            }
            else
            {
                Product.DateModified = DateTime.Now;
                foreach (var category in Product.ProductCategories)
                {
                    category.Category = null;
                }
                Product.Manufacturer = null;
                db.Products.Attach(Product);
                db.Entry(Product).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { message = "Ürün başarıyla güncellendi", id = Product.Id });

            }

        }

        [HttpPost]
        public ActionResult AddImage([FromBody] ProductImageRequest req)
        {
            var images = db.ProductImages.Where(x => x.ProductId == req.ProductId).ToList();
            foreach (var image in images)
            {
                db.ProductImages.Remove(image);
                db.SaveChanges();
            }
            foreach (var ProductImage in req.ProductImages)
            {
                db.ProductImages.Add(new ProductImage()
                {
                    ProductId = req.ProductId,
                    Url = ProductImage.Url,
                });
            }
            db.SaveChanges();
            return Json(req);
        }

        [HttpPost]
        //ürünü düzenlerken kategori güncellemek için
        public ActionResult ProductsCategory([FromBody] ProductCategoryRequest req)
        {
            var categories = db.ProductCategories.Where(x => x.ProductId == req.ProductId).ToList();
            foreach (var category in categories)
            {
                db.ProductCategories.Remove(category);
                db.SaveChanges();
            }

            foreach (var categoryId in req.ProductsCategory)
            {
                db.ProductCategories.Add(new ProductCategory()
                {
                    ProductId = req.ProductId,
                    CategoryId = categoryId
                });
            }
            db.SaveChanges();
            return Json("ne return edilcek????");
        }
        [HttpPost]
        public ActionResult GetProductFromId([FromBody] DetailsWithIdRequest req)
        {
            var Product = db.Products.Where(x => x.Id == req.Id)
                .Include(x => x.Manufacturer)
                .Include(x => x.ProductImages)
                .Include(x => x.StockStatu)
                .Include(x => x.ProductCategories)
                    .ThenInclude(y => y.Category).FirstOrDefault();
            return Json(Product);
        }
        [HttpPost]
        public ActionResult GetCategoryFromId([FromBody] DetailsWithIdRequest req)
        {
            var Category = db.Categories.Where(x => x.Id == req.Id).FirstOrDefault();
            return Json(Category);
        }
        [HttpPost]
        public ActionResult DeleteProduct([FromBody] List<int> DeleteProducts)
        {
            foreach (var productId in DeleteProducts)
            {
                var product = db.Products.Where(x => x.Id == productId).FirstOrDefault();
                product.IsDeleted = true;

            }
            db.SaveChanges();
            return Json(new { message = "Başarıyla Silindi!" });
        }
        [HttpPost]
        public ActionResult DeleteCategory([FromBody] List<int> DeleteCategories)
        {
            foreach (var categoryID in DeleteCategories)
            {
                var category = db.Categories.Where(x => x.Id == categoryID).FirstOrDefault();
                category.IsDeleted = true;
            }
            db.SaveChanges();
            return Json(new { mesaj = "Başarıyla Silindi!" });
        }

        [HttpPost]
        public ActionResult AddCategory([FromBody] Category category)
        {
            if (category.Id == 0)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return Json(new { mesaj = "Eklendi" });

            }
            else
            {
                db.Categories.Attach(category);
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { mesaj = "Güncellendi" });

            }


        }
    }
    public class ProductListFilteredRequest
    {
        public List<int> ManufacturerIds { get; set; }
        public List<int> CategoryIds { get; set; }
        public int? stockMin { get; set; }
        public int? stockMax { get; set; }
    }
    public class ProductCategoryRequest
    {
        public List<int> ProductsCategory { get; set; }
        public int ProductId { get; set; }
    }
    public class ProductImageRequest
    {
        public List<ProductImage> ProductImages { get; set; }
        public int ProductId { get; set; }
    }
}
