using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MolaaApp.Models;
using MolaaApp.ViewModels;

namespace MolaaApp.Controllers
{
    //[Authorize(Roles ="admin")]ben burda diyorum ki rolü admin olan kişiler sadece burdaki linklere erişebilsin, yani normal bir kullanıcı girip de linke users yapıp o sayfaya gidebiliyordu onu engelledim
    //[AllowAnonymous]//**bunu dersem diyorum ki admin rolündeki bir kişi burdaki linklere gidebiliyordu ama ben bunu diyerek diyorum ki bu sayfaya bütün rollerdeki kullanıcılar gitsin diğerler linklere ise gidemsin diyorum yani sadece bir linki diğerlerinden farklı yapmış oluyorum
    //ya da şöyle yaparım ben buraya [Authorize(Roles ="admin")] bunu diyerek sadece buraya admin rolündeki sadece gitsin ama diğer linklere herkes gidebilir diyebilirim, veya [Authorize(Roles ="admin,customer")] bu roldeki kişiler girebilir diyebilirim
    // if (!User.IsInRole("admin"))
    //         {
    //             return RedirectToAction("Login","Account");
    //         }
    public class UsersController:Controller
    {
        private UserManager<AppUser> _userManager;//usertablosundaki verileri veritabnından getirdim

        private RoleManager<AppRole> _roleManager;//roletablosundaki verileri veritabnından getirdim

        public UsersController(UserManager<AppUser> userManager,RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;

            _roleManager = roleManager;
        }

        [Authorize(Roles ="admin")]
        public IActionResult Index(){
            return View(_userManager.Users);
        }

       
       [Authorize]
        public async Task<IActionResult> Edit(string id){
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || currentUser.Id != id && !User.IsInRole("admin"))
            {
                return Forbid();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                ViewBag.Roles = _roleManager.Roles.Select(i => i.Name).ToList();//viewbag üzerinden ben role tablosundan sadece role isimlerini aldım ve kullanıcı seçsin diye view sayfasında burdan aldığım bilgileri getirip seçtiricem

                return View(new EditViewModel{
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Image = user.Image,
                    SelectedRoles = await _userManager.GetRolesAsync(user)//bu komutla kullanıcıya atanmış rolleri ben alabilicem veritabanından ve sayfada göstericem
                });
            }
            return RedirectToAction("Index","Posts");
        }

        [Authorize]
        [HttpPost]//kullanıcı güncellemesi 
        public async Task<IActionResult> Edit(string id, EditViewModel model,IFormFile? imageFile){//IFormFile? resim seçilme zorunluluğunu kaldırdım
            if (id != model.Id)
            {
                return RedirectToAction("Index");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || currentUser.Id != id && !User.IsInRole("admin"))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);//Id ile o kullanıcıyı aldım

                if (user != null)
                {
                    if (imageFile != null)
                    {
                        var extension = Path.GetExtension(imageFile.FileName);//dosyanın uzantısını alır ; mesela burda imageFile.FileName buna bakar abc.jpg ise "jpg" kısmını alır
                        var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");//burda random isim oluşturup(Guid.NewGuid()) üste dosyadan aldığım uzatıyı ekliyorum direk
                        var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img", randomFileName);

                        //resim eklenmesinde sorun olmazsa burda stremi oluşturuyorum çünkü kapsamdan çıktığında bellekten silinsin diye
                        using(var stream = new FileStream(path, FileMode.Create)){
                        await imageFile.CopyToAsync(stream);//ilgili dizine kopyaladım resmi ve çalışması için Task<IActionResult> yaptım
                        }
                        model.Image = randomFileName;
                    }
                    
                    user.FullName = model.FullName;//kullanıcı tablosundaki bilgiyi modeldeki değerle güncelledim
                    user.Email = model.Email;//kullanıcı tablosundaki bilgiyi modeldeki değerle güncelledim
                    user.Image = model.Image;

                    var result = await _userManager.UpdateAsync(user);
                    

                    //şifre güncellemesini zorunlu yapmadım ama admin istediği zaman kullanıcı şifresini değiştirebilir onuda:
                    if (result.Succeeded && !string.IsNullOrEmpty(model.Password))//result.Succeeded && !string.IsNullOrEmpty(model.Password) burda diyorum ki durum başarılı ve model view de password alanı boş değilse bu koşulun içine gir ve kullanıcı da kayıtlı olan şifreyi sil ve model view de yazdığım şifreyi userın yeni şifresi yap
                    {
                        await _userManager.RemovePasswordAsync(user);
                        await _userManager.AddPasswordAsync(user, model.Password);

                        
                    }
                    if (result.Succeeded)
                    {
                        await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));//ben her editleme işleminde o kullanıcıya ait rol bilgilerini başta sildim

                        if (User.IsInRole("admin") && model.SelectedRoles != null)//eğer ben editte bir rol seçtiysem ekleme yapıcam
                        {
                            await _userManager.AddToRolesAsync(user, model.SelectedRoles);
                        }
                        return RedirectToAction("Index","Posts");
                    }
                    
                    foreach (IdentityError err in result.Errors)
                    {
                        ModelState.AddModelError("",err.Description);
                    }
                    return RedirectToAction("Index","Posts");
                }
            }

            return View(model);
        }

        [Authorize(Roles ="admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(string id){
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            
            return RedirectToAction("Index");
        } 
        
    }
}