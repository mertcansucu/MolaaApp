using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MolaaApp.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MolaaApp.Data.Abstract;
using MolaaApp.Models;
using Microsoft.EntityFrameworkCore;
using MolaaApp.Data.Concrete;

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

        private ICommentRepository _commentRepository;

        private ILikeRepository _likeRepository;

        public PostsController(IPostRepository postRepository,UserManager<AppUser> userManager,ICommentRepository commentRepository,ILikeRepository likeRepository){
            _postRepository = postRepository;
            _userManager = userManager;
            _commentRepository = commentRepository;
            _likeRepository = likeRepository;
            
        }
        public async Task<IActionResult> Index()
        {
            var posts = _postRepository.Posts.Where(i => i.IsActive);//Isactive ekleyerek sadece true olanları göster dedim
            // IQueryable bir bilgi yani veri tabanından bilgileri şuan almıyorum sadece bağlantıyı sağladım
            
            return View(new PostsViewModel
            {// index.cshtml sayfasına bilgileri gönderdim
                Posts = await posts.ToListAsync()//burda diyorum ki ya bütün bilgileri gönder ya da if ile koşul sağlanırsa ordaki istediğim bilgileri sadece bana gönder
        
            }); 
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
                        Description = model.Description,
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

        //önceden url de id bilgisi vardı ben bunu url ile değiştirdim
        public async Task<IActionResult> Details(string url){
            return View(await _postRepository
            .Posts
            .Include(x => x.User)//postu yayınlayan kişinin bilgisini almak için ekledim
            //doğrudan ulaştıklarım include olur ama theninclude farklı bir yere geçip ordaki bilgiyi almak istediğimde öyle yaparım
            .Include(x => x.Comments)//o postla ilgili yorumları ekledim join ile
            .ThenInclude(x => x.User)//bunu böyle yapmamın nedeni commentinde user bilgisini alıp onun içindeki resmi çekmek için
            .FirstOrDefaultAsync(p => p.Url == url));
        }

        [HttpPost]
        public JsonResult AddComment(int PostId, string Text){
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
        var fullName = user != null ? user.FullName : string.Empty;
        var avatarImg = user != null ? user.Image : string.Empty;

        var entity = new Comment{
            PostId = PostId,
            Text = Text,
            PublishedOn = DateTime.Now,
            UserId = userId ?? ""
        };
        _commentRepository.CreateComment(entity);
    
        return Json(new{
            fullName, // username yerine fullName'i döndür
            Text,
            entity.PublishedOn,
            avatarImg
        });
        }   

        [HttpGet]
        public JsonResult GetLikeCount(int postId)
        {
            var likeCount = _likeRepository.likes.Count(l => l.PostId == postId);
            return Json(new { likeCount = likeCount });
        }


        [HttpPost]
        public JsonResult AddLike(int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingLike = _likeRepository.likes.FirstOrDefault(l => l.UserId == userId && l.PostId == postId);


            if (existingLike != null)
            {
                // Kullanıcı daha önce bu gönderiyi beğenmişse like'ı kaldır
                _likeRepository.RemoveLike(existingLike);
                return Json(new { liked = false });
            }
            else
            {
                // Kullanıcı daha önce bu gönderiyi beğenmemişse like'ı ekleyerek işlemi gerçekleştir
                _likeRepository.AddLike(new Like { UserId = userId ?? "", PostId = postId });
                return Json(new { liked = true });
            }
        }

        [Authorize] // kullanıcı giriş yapmadan post ekleme yapmasını engellemek için bunu ekledim
        public async Task<IActionResult> List(){//bu listenin amacı admin kişisi tüm postları görebiliyor
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);

            var posts = _postRepository.Posts;//tüm post bilgilerimni aldım ama veritabnı ile çağırmadım

            if (string.IsNullOrEmpty(role))//burda user rolu olmayanları bul dedim ve onlara sadece kendi paylaştığı postları görsün dedim
            {
                posts = posts.Where(i => i.UserId == userId);
            }
            return View(await posts.ToListAsync());//veri tabanıyla bağlantı sağlayıp postları çağırdım
        }

        [Authorize] // kullanıcı giriş yapmadan post ekleme yapmasını engellemek için bunu ekledim
        public IActionResult Edit(int? id){
            if (id == null)
            {
                return NotFound();
            }

            var post = _postRepository.Posts.FirstOrDefault(i => i.PostId == id);//sayfa yüklenirken kullanıcın önceden seçtiği tagler varsa onlarda gelsin diye
            if (post == null)
            {
                return NotFound();
            }


            return View(new PostCreateViewModel {
                PostId = post.PostId,
                Title = post.Title,
                Description = post.Description,
                Content = post.Content,
                Url = post.Url,
                Image = post.Image,
                IsActive = post.IsActive,
                
            });
        }

        

        [Authorize] // kullanıcı giriş yapmadan post ekleme yapmasını engellemek için bunu ekledim
        [HttpPost]
        public async Task<IActionResult> Edit(PostCreateViewModel model, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    var extension = Path.GetExtension(imageFile.FileName);
                    var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    model.Image = randomFileName;
                }

                var entityToUpdate = new Post
                {
                    PostId = model.PostId,
                    Title = model.Title,
                    Description = model.Description,
                    Content = model.Content,
                    Url = model.Url,
                    Image = model.Image
                };

                if (User.FindFirstValue(ClaimTypes.Role) == "admin")
                {
                    entityToUpdate.IsActive = model.IsActive;
                }

                _postRepository.EditPost(entityToUpdate);
                return RedirectToAction("List");
            }

        
            return View(model);
        }

        
    }
}