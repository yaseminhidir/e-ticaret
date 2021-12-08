using ETicaret.Core.Helpers;
using ETicaret.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.Core.Controllers
{
    public class testController : Controller
    {
        [AuthenticationTokenHandler]
        public IActionResult Index()
        {
            var user = (CustomerToken)HttpContext.Items["user"];
            return Json(user);
        }
    }
}
