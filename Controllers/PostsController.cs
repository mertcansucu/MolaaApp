using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MolaaApp.Controllers
{
    public class PostsController:Controller
    {
        public IActionResult Index(){
            return View();
        }
    }
}