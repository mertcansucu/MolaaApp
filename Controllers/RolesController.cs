using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MolaaApp.Models;

namespace MolaaApp.Controllers
{
    [Authorize(Roles ="admin")]
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

        public async Task<IActionResult> Edit(string id){
            var role = await _roleManager.FindByIdAsync(id);//id ile eşleşen role bilgisini aldım

            if (role != null && role.Name != null)
            {
                ViewBag.Users = await _userManager.GetUsersInRoleAsync(role.Name);//Viewbag ile userları aldım ve userslardan o user rolune ait kayıtları getirdim
                return View(role);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AppRole model){
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);//modelin içindeki id ile eşleşen role bilgisini aldım

                if (role !=null)
                {
                    role.Name = model.Name;

                    var result =await _roleManager.UpdateAsync(role);//burda ben role ismini güncelledim

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError("",err.Description);
                    }

                    if(role.Name != null){
                        ViewBag.Users = await _userManager.GetUsersInRoleAsync(role.Name);//Viewbag ile userları aldım ve userslardan o user rolune ait kayıtları getirdim
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id){
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return RedirectToAction("Index");
        } 
    }
}