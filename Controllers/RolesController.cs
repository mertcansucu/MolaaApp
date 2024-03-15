using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MolaaApp.Models;

namespace MolaaApp.Controllers
{
    public class RolesController:Controller
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RolesController(RoleManager<AppRole> roleManager,UserManager<AppUser> userManager){
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index(){
            return View(_roleManager.Roles);//_roleManager.Roles =>IQueryable bir nesnedir yani ben bunu filtreleyip veri tabanına sorgu yazıp istediğim şekilde çekebilirim
        }

        public IActionResult Create(){
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(AppRole model){

            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(model);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("",err.Description);
                }
            }

            return View(model);
        }
    }
}