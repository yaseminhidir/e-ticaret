using ETicaret.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.Core.Controllers
{
    public class HomeController : Controller
    {
        private eticaret_coreContext db;

        public HomeController(eticaret_coreContext db)
        {
            this.db = db;
        }

        /*
        public ActionResult GetPrdoucts()
        {
            var query = db.Products
                .Include(x => x.ProductImages)
                .ToList();
            return Json(query);
            // var product = await axios.post("http://....",{ category:this.category,subcategory:this.subcategory }/* suslu parantez body );*/
        // product.name;
        // for( image of product.productImages){
        //
        // }
        // <div v-for="var image product.productImages">
        //}

        [HttpPost]
        public ActionResult CategorySubCategory([FromBody]CategorySubCategoryRequest teststatsa)
        {


            return Content("");
        }



        [HttpPost]
        public ActionResult SaveProduct([FromBody] Product urun)
        {
            if (urun.Id == 0)
            {
                db.Products.Add(urun);
            }
            else
            {
                db.Products.Attach(urun);
                db.Entry(urun).State = EntityState.Modified;
            }

            db.SaveChanges();
            return Json(urun);
        }

    }
   
public class CategorySubCategoryRequest
    {
        public string category { get; set; }
        public string subcategory { get; set; }
    }

}
