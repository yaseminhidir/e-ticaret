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
    [AuthenticationTokenHandler]
    public class CustomerProfileController : Controller
    {
        private eticaret_coreContext db;
        public CustomerProfileController(eticaret_coreContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public IActionResult AddAdresses([FromBody] List<Address> addresses)
        {
            GenericResponse bad = new GenericResponse("Kayıt başarısız", false);
            foreach (var address in addresses)
            {
                if (string.IsNullOrWhiteSpace(address.City) ||
                    string.IsNullOrWhiteSpace(address.Country) ||
                    string.IsNullOrWhiteSpace(address.Zone) ||
                    string.IsNullOrWhiteSpace(address.Address1))
                {
                    return Json(bad);
                }
            }
            var customer = (Customer)HttpContext.Items["Customer"];


            foreach (var newAddress in addresses)
            {
                if (newAddress.Id == 0)
                {
                    newAddress.Id = 0;
                    newAddress.CustomerId = customer.Id;
                    db.Addresses.Add(
                        newAddress
                    );
                }
                else
                {
                   db.Addresses.Attach(newAddress);
                    db.Entry(newAddress).State = EntityState.Modified;
                }
            }
            db.SaveChanges();

            return Json(new GenericResponse("Güncellendi", true));
        }
        [HttpPost]
        public IActionResult DeleteAddress([FromBody] Address address)
        {
            var query = db.Addresses.Where(x => x.Id == address.Id).FirstOrDefault();
            query.IsDeleted = true;
            db.SaveChanges();
            return Json(new GenericResponse("Silindi", true,query));
        }
        [HttpPost]
        public IActionResult GetProfileDetails()
        {

            var customer = (Customer)HttpContext.Items["Customer"];
            var freshcustomer = db.Customers.Where(x => x.Id == customer.Id).Include(x => x.Addresses.Where(a => !a.IsDeleted)).FirstOrDefault();
            return Json(freshcustomer);

        }


    }
}
