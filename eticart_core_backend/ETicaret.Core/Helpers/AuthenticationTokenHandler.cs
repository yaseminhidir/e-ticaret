using ETicaret.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.Core.Helpers
{
    public class AuthenticationTokenHandler : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Request.Headers["authentication"].ToString(); // headerdan tokenı çektik
            if (string.IsNullOrWhiteSpace(token))
            {
                context.Result = new JsonResult(new GenericResponse("Giriş başarısız", false));
                return;
            }
            //axios.defaults.headers["Authentication"] = "Bearer " + state.userToken -> burdaki bearerdan kurtulmak için.
            token = token.Split(' ')[1];  
            var db = (eticaret_coreContext)context.HttpContext.RequestServices.GetService(typeof(eticaret_coreContext));
            var customerToken = db.CustomerTokens.Where(x => x.Token == token).Include(x => x.Customer).FirstOrDefault();
            if (customerToken == null)
            {
                context.Result = new JsonResult(new GenericResponse("Giriş başarısız", false));
                return;
            }
            else
            {
                // request contexti getirir.

             context.HttpContext.Items["Customer"] = customerToken.Customer;
                context.HttpContext.Items["CustomerToken"] = customerToken;


            }
            base.OnActionExecuting(context);
        }
    }
}
