using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MolaaApp.Controllers
{
    public class AccountController:Controller
    {
        public IActionResult Login(){
            return View();
        }
    }
}