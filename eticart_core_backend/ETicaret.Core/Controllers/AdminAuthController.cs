using ETicaret.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.Core.Controllers
{
    public class AdminAuthController:Controller
    {
        private eticaret_coreContext db;
        public AdminAuthController(eticaret_coreContext db)
        {
            this.db = db;
        }
        
    }
}


