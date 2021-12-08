using ETicaret.Core.Helpers;
using ETicaret.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ETicaret.Core.Controllers
{
    public class AuthCustomerController : Controller
    {
        private eticaret_coreContext db;
        public AuthCustomerController(eticaret_coreContext db)
        {
            this.db = db;
        }
        [HttpPost]
        public IActionResult Register([FromBody] Customer customer)
        {
            GenericResponse bad = new GenericResponse("Kayıt başarısız", false);

            var query = db.Customers.Where(x => x.Email == customer.Email);
            if (query.Count()>0 && customer.Id==0)
            {
                return Json(new GenericResponse("Bu e-mail ile başka bir hesap mevcut", false));
            }
            if(string.IsNullOrEmpty(customer.Email)||
                string.IsNullOrWhiteSpace(customer.FirstName)||
                string.IsNullOrWhiteSpace(customer.LastName)||
                string.IsNullOrWhiteSpace(customer.Password)||
                string.IsNullOrWhiteSpace(customer.Telephone))
            {
                return Json(new GenericResponse("Tüm alanları doldurunuz", false));
            }
          
            if (!customer.Email.Contains('@'))
            {
                return base.Json(bad);
            }
            if (customer.Telephone.Length != 11)
            {
                return base.Json(bad);
            }
          
            if (customer.Password.Length <= 9)
            {
                return base.Json(bad);
            }
            // Minimum eight and maximum 10 characters,
            // at least one uppercase letter,
            // one lowercase letter,
            // one number and one special character:
            if (!Regex.IsMatch(customer.Password, @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$"))
            {
                return base.Json(bad);
            }
            if (customer.Id == 0)
            {
                var digest = SHA256Helper.HashWithNewSalt(customer.Password, 64);
                customer.Password = digest.Digest;
                customer.Salt = digest.Salt;
                customer.DateAdded = DateTime.Now;
                db.Customers.Add(customer);
                db.SaveChanges();
                return Json(new GenericResponse("Kayıt başarılı", true, customer));
            }
            if (customer.Id != 0)
            {
                
                db.Customers.Attach(customer);
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
               
            }
            return Json(new GenericResponse("Güncelleme başarılı", true, customer));
            /*  var digest = SHA256Helper.HashWithNewSalt(customer.Password, 64);
              customer.Password = digest.Digest;
              customer.Salt = digest.Salt;
              customer.DateAdded = DateTime.Now;
              db.Customers.Add(customer);
              db.SaveChanges();
              return Json(new GenericResponse("Kayıt başarılı", true, customer));*/
        }
        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest req)
        {
            GenericResponse bad = new GenericResponse("Giriş başarısız", false);

            if (string.IsNullOrWhiteSpace(req.email))
            {
                return base.Json(bad);
            }
            if (!req.email.Contains('@'))
            {
                return base.Json(bad);
            }

            if (string.IsNullOrWhiteSpace(req.password))
            {
                return base.Json(bad);
            }

            if (req.password.Length < 8)
            {
                return base.Json(bad);
            }

            var query = db.Customers.Where(x => x.Email == req.email);

            if (query.Count() == 0)
            {
                return base.Json(bad);
            }


            var customer = query.FirstOrDefault();
            var digest = SHA256Helper.HashWithKnownSalt(req.password, customer.Salt); //forma girilen şifreyi tekrar hashledik.

            if (digest.Digest != customer.Password)
            {
                return base.Json(bad);
            }

            var token = new CustomerToken(); //tokenın satırını oluşturduk
            token.Token = Guid.NewGuid().ToString("N"); //32 karakterlik rastgele
            token.CustomerId = customer.Id;
            db.CustomerTokens.Add(token);
            db.SaveChanges();

            return Json(new GenericResponse("Giriş başarılı", true, token));

        }
        [HttpPost]
        [AuthenticationTokenHandler]
        public IActionResult LogOut()
        {
          
            var customer = (Customer)HttpContext.Items["Customer"];
            var token = (CustomerToken)HttpContext.Items["CustomerToken"];
            var freshToken = db.CustomerTokens.Where(x => x.Token == token.Token).FirstOrDefault();
            db.CustomerTokens.Remove(freshToken);
            db.SaveChanges();
            return Json(freshToken);
        }
    }

    public class LoginRequest
    {
        public string email { get; set; }
        public string password { get; set; }
    }

    public class LogOutRequest
    {
        public object customer { get; set; }
        public string token { get; set; }
    }

}
