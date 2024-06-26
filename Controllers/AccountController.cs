using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MolaaApp.Models;
using MolaaApp.ViewModels;

namespace MolaaApp.Controllers
{
    public class AccountController:Controller
    {
        private UserManager<AppUser> _userManager;//usertablosundaki verileri veritabnından getirdim

        private RoleManager<AppRole> _roleManager;//roletablosundaki verileri veritabnından getirdim

        private SignInManager<AppUser> _signInManager;

        private IEmailSender _emailSender;

        public AccountController(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            SignInManager<AppUser> signInManager,IEmailSender emailSender)
        {
            _userManager = userManager;

            _roleManager = roleManager;

            _signInManager = signInManager;//login işlemi olunca kullanıcıya cookie bilgisi göndermek için ekledim

            _emailSender = emailSender;
        }

        public IActionResult Login(){
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model){

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    await _signInManager.SignOutAsync();//başlangıçta kullanıcı daha önce giriş yapmışsa cookie sini sıfırladım

                    //IsEmailConfirmedAsync hesap email onayı getirdim bu şekilde kullanıcı emailini onaylarsa ture olacak ve giriş yapabilecek
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError("","Hesabınızı email üzerinden onaylayınız");
                        return View(model);
                    }

                    //yeni cookie oluşturdum
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe,true);
                    /*
                    bu kodda:var result = await _signInManager.PasswordSignInAsync(user, model.Password, 
                    model.RememberMe,=> burda eğer bu şeçili olursa tarayıcı kapandığında bile tekrar uygulamayı açınca kullanıcı girişi olmuş şekilde gelir beni hatırla yani
                    true)=> bu kod ise programcs de ben 5 hak vermiştim kullanıcıya hatalı giriş için ben bunu geçerli kıldım bunu false yaparsam kullanıcı istediği kadar hatalı girebilir
                    */

                    if (result.Succeeded)//giriş başarılı durumu
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);//ben burda eğer kullanıcı giriş yaptıysa başta dediğim 5 kere hatalı girme bilgisine getirdim yani 3 kere yanlış girdi ve giriş yaptıysa tekrar giriş yaptığında 5 kere hatalı girme hakkı verdim
                        await _userManager.SetLockoutEndDateAsync(user,null);//bekleme süresini de sıfırladım


                        return RedirectToAction("Index","Posts");
                    }else if (result.IsLockedOut)//kullanıcı girişi başarısızsa
                    {
                        var LockoutDate = await _userManager.GetLockoutEndDateAsync(user);
                        var timeleft = LockoutDate.Value - DateTime.UtcNow;//ben burdaki kodla şunu aldım eğer kullanıcı 5 kere hatalı girerse 5 dakida bekle demiştim ya kullanıcıya bu süreyi göstermek için aldım burda kaç dakikası kaldığını göstericem ve ona söyliyicem

                        ModelState.AddModelError("",$"Hesabınız kilitlendi, lütfen {timeleft.Minutes} dakika sonra tekrar deneyiniz!");
                    }else
                    {
                        ModelState.AddModelError("","Parolanız hatalı!");
                    }

                    
                }else{
                    ModelState.AddModelError("","Bu Email adresiyle bir hesap bulunamadı!");
                }
            }


            return View(model);
        }

        //kullanıcı ekleme kısmını userdan buraya taşıdım çünkü mail onayda ekliyicem
         public IActionResult Create(){
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel model,IFormFile imageFile){
            if (ModelState.IsValid)
            {
                var extension = Path.GetExtension(imageFile.FileName);//dosyanın uzantısını alır ; mesela burda imageFile.FileName buna bakar abc.jpg ise "jpg" kısmını alır
                var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");//burda random isim oluşturup(Guid.NewGuid()) üste dosyadan aldığım uzatıyı ekliyorum direk
                var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img", randomFileName);

                //resim eklenmesinde sorun olmazsa burda stremi oluşturuyorum çünkü kapsamdan çıktığında bellekten silinsin diye
                using(var stream = new FileStream(path, FileMode.Create)){
                await imageFile.CopyToAsync(stream);//ilgili dizine kopyaladım resmi ve çalışması için Task<IActionResult> yaptım
                }
                model.Image = randomFileName;

                
                var user = new AppUser{
                    UserName = model.UserName, 
                    Email = model.Email,
                    FullName = model.FullName,
                    Image = model.Image};

                IdentityResult result = await _userManager.CreateAsync(user,model.Password);
                //await _userManager.CreateAsync(user); şifresizde kayıt yapabilirim ama kullancı girişi olduğu için uygulamamda böyle yaptım

                if (result.Succeeded)
                {
                    //email token oluşturdum,bunu yapmakmiçin programcs de AddDefaultTokenProviders() bunu ekledim
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var url = Url.Action("ConfirmEmail","Account",new{user.Id, token});//url oluşturken altta yeni bir metod tanımladım ve orda email onaylama sayfası olacak ve onun için de gerekli kısımları oraya yazdım

                    // email
                    //http://localhost:5284/ http://localhost:5258/
                    await _emailSender.SendEmailAsync(user.Email, "Hesap Onayı", $"Lütfen email hesabınızı onaylamak için linke <a href='http://localhost:5258{url}'>tıklayınız.</a>");

                    TempData["message"] = "Email Hesabınızdaki onay mailini tıklayınız";
                    return RedirectToAction("Login","Account");
                }

                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("",err.Description);
                }

            }
            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string Id, string token){
            if (Id == null || token == null)
            {
                TempData["message"] = "Geçersiz token bilgisi";
                return View();
            }

            var user =await _userManager.FindByIdAsync(Id);

            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user,token);//ben burda veritabında email onayını 0 dan 1 yaptım

                if (result.Succeeded)
                {
                    TempData["message"] = "Hesabınız onaylandı";
                    return RedirectToAction("Login","Account");
                }
            }

            TempData["message"] = "Kullanıcı bulunamadı";
            return View();
        }

         public async Task<IActionResult> Logout(){
            await _signInManager.SignOutAsync();//network altında oluşan cookie yi sildi
            return RedirectToAction("Login");
        }

        public IActionResult ForgotPassword(){
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email){
            if (string.IsNullOrEmpty(Email))//eğer email alanı boşsa sayfayı tekrar gönder dedim
            {
                TempData["message"] = "Email adresini giriniz !";
                return View();
            }

            var user = await _userManager.FindByEmailAsync(Email);//burda yazdığım mail adresiyle uyumlu olan bir user var mı diye baktım 
            if (user == null)//eğer eşleşen bir kullanıcı yoksa sayfayı bir daha gönderdim 
            {
                TempData["message"] = "Bu Email adresiyle eşleşen kullanıcı bulunmamaktadır. Lütfen tekrar giriniz!";
                return View();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);//şifre sıfırlama için o kullanıcı için bir token oluşturdum
            var url = Url.Action("ResetPassword","Account",new{user.Id, token,Email});//url oluşturken altta yeni bir metod tanımladım

            // email, buda kullanıcıya mail ile diyorum ki bu linke tıklayıp şifreni sıfırla
            //http://localhost:5258/
            await _emailSender.SendEmailAsync(Email, "Parola Sıfırlama", $"Parolanızı yenilemek için linke <a href='http://localhost:5258{url}'>tıklayınız.</a>");

            TempData["message"] = "Email adresinize gönderilen link ile şifrenizi sıfırlayabilirsiniz.";

            return View();
            
        }

        public IActionResult ResetPassword(string Id,string token,string Email){
            //var url = Url.Action("ResetPassword","Account",new{user.Id, token}yukarıda zaten bu bilgileri alıyorum ben bu sayfaya bu bilgileri gönderip yeni şifre oluşturup, şifreyi güncelliyicem

            if (Id == null || token == null)
            {
                return RedirectToAction("Login");
            }

            var model = new ResetPasswordModel {Token = token};
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model){
            if (ModelState.IsValid)
            {
                
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    TempData["message"] = "Bu mail adresiyle eşleşen kullanıcı yok";
                    return RedirectToAction("Login");
                }
                var result = await _userManager.ResetPasswordAsync(user, model.Token,model.Password);

                if (result.Succeeded)
                {
                    TempData["message"] = "Şifreniz değiştirildi";
                    return RedirectToAction("Login");
                }

                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("",err.Description);
                }
            }

            return View(model);
        }

        public IActionResult Profile(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return NotFound();
            }

            var user = _userManager
                       .Users
                       .Include(x => x.Posts.Where(p => p.IsActive))
                       .Include(x => x.Comments)
                       .ThenInclude(x => x.Post)//üstekiler users tablosundan çektim ama burda commentsin içindeki postu çektim [@comment.Post.Title] profile.cshtml
                       .FirstOrDefault(x => x.UserName == username);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
    }
}