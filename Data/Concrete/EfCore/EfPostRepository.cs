using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MolaaApp.Entity;
using MolaaApp.Data.Abstract;
using MolaaApp.Models;

namespace MolaaApp.Data.Concrete
{
    public class EfPostRepository : IPostRepository
    {//IPostRepository interface i Iplement edecek olan concrete versiyonunu oluşturdum
    //alttaki hali implement hali olarak gelen hali ctrl+. ile ekledim sonra veritabanı bağlantısı sağlayıp o verileri çektim
    //bunu böyle yapmamın nedeni istediğim yerde çağırıp kullanabilmek diğer şekilde her yerde ayrı ayrı oluşturmam gerekiyordu

        private IdentityContext _context;
        public EfPostRepository(IdentityContext context)
        {
            _context = context;
        }
        
        public IQueryable<Post> Posts => _context.Posts;

        public void CreatePost(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        public void EditPost(Post post)
        {
            var entity = _context.Posts.FirstOrDefault(i => i.PostId == post.PostId);//Include(i => i.Tags) ifadesini kullanmazsam, Post nesnesi yüklenirken Tags nesneleri yüklenmez. Bu durumda, posta ait etiketler hafızada olmadığı için, bu etiketleri güncellemeye çalıştığımda da hata alırım bunu ekleyerek hatayı engelleyip güncellemeyi yaptım

            if (entity != null)
            {
                entity.Title = post.Title;
                entity.Description = post.Description;
                entity.Content = post.Content;
                entity.Url = post.Url;
                entity.Image = post.Image;
                entity.IsActive = post.IsActive;

                

                _context.SaveChanges();//oluşturduğum bu metodu postcont içinde kullanıcam ve güncellemeyi yapıcam

            }
        }

        public void DeletePost(Post post)
        {
            _context.Posts.Remove(post);
            _context.SaveChanges();
        }
    }
}