using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlogApp.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MolaaApp.Data.Abstract;
using MolaaApp.Models;

namespace MolaaApp.Controllers
{
    public class PostsController:Controller
    {
        // private readonly BlogContext _context;//BlogContext ten nesne ürettim
        // //veri tabanı bilgilerini çekmek için
        // public PostsController(BlogContext context){
        //     _context = context;
        // }bu injection yöntemi bunun yerine interface aracılığı ile yapıp bağımlılıkları ortadan kaldırıp ayrıca yazdığım kodları tek bir yerden kontrol etmiş oldum

        //**Postun içinde tag bilgileri olduğu için onuda ekledim
        //ancak iki tablodan veri göndereceğim için bunları ortak bir models(PostsViewModel) içinde çağırıp index sayfasına ekleyerek yaptım

        //bu yöntemden sonra ben component kullanarak veri çektim onun için bu bilgileri sidim
        private UserManager<AppUser> _userManager;//usertablosundaki verileri veritabnından getirdim
        private IPostRepository _postRepository;

        public PostsController(IPostRepository postRepository,UserManager<AppUser> userManager){
            _postRepository = postRepository;
            _userManager = userManager;
            
        }
        public IActionResult Index(){
            var model = new PostsViewModel{
                Posts = _postRepository.Posts.ToList()
                };
                return View(model);
            }


        [Authorize] // kullanıvı giriş yapmadan post ekleme yapmasını engellemek için bunu ekledim
        public IActionResult Create(){
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(PostCreateViewModel model, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

                var extension = Path.GetExtension(imageFile.FileName);//dosyanın uzantısını alır ; mesela burda imageFile.FileName buna bakar abc.jpg ise "jpg" kısmını alır
                var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");//burda random isim oluşturup(Guid.NewGuid()) üste dosyadan aldığım uzatıyı ekliyorum direk
                var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img", randomFileName);

                //resim eklenmesinde sorun olmazsa burda stremi oluşturuyorum çünkü kapsamdan çıktığında bellekten silinsin diye
                using(var stream = new FileStream(path, FileMode.Create)){
                await imageFile.CopyToAsync(stream);//ilgili dizine kopyaladım resmi ve çalışması için Task<IActionResult> yaptım
                }
                model.Image = randomFileName;

                _postRepository.CreatePost(
                    new Post{
                        Title = model.Title,
                        Content = model.Content,
                        Url = model.Url,
                        UserId = userId ?? "",
                        PubilshedOn = DateTime.Now,
                        Image = model.Image,
                        IsActive = false 
                    }
                );
                return RedirectToAction("Index");
            }
            return View(model);
        }

    }
}