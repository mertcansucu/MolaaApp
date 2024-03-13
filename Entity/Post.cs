using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MolaaApp.Entity;
using MolaaApp.Models;

namespace BlogApp.Entity
{
    public class Post
    {
        public int PostId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public string? Url { get; set; }
        public string? Image {get; set;}
        public DateTime PubilshedOn { get; set; }
        public bool IsActive { get; set; }//active lik durumu admin onayı olan postlar sadece yayınlanacak
        public string UserId { get; set; } = null!;
        public AppUser User { get; set; } = null!;//navigation prop,joinleme,null!=>bu kısmın boş kalmayacağını söylüyorum,bir postu bir user yayınlayacağı için böyle yaptım,birden fazla olasaydı liste şeklinde yapacaktım
        public List<Tag> Tags { get; set; } = new List<Tag>();//Bir posta birden fazla etiket olabieceği için liste şeklinde bağlantıyı sağladım
        public List<Comment> Comments { get; set; } = new List<Comment>();//Her bir posta birden fazla yorum olabilir
    }
}