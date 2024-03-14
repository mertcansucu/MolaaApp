using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MolaaApp.Models;

namespace MolaaApp.Controllers
{
    public class UsersController:Controller
    {
        private UserManager<AppUser> _userManager;//usertablosundaki verileri veritabnından getirdim

        private RoleManager<AppRole> _roleManager;//roletablosundaki verileri veritabnından getirdim

        public UsersController(UserManager<AppUser> userManager,RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;

            _roleManager = roleManager;
        }

        public IActionResult Index(){
            return View(_userManager.Users);
        }
    }
}