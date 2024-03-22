using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MolaaApp.Entity;
using MolaaApp.Models;

namespace MolaaApp.Entity
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string? Text { get; set; }
        public DateTime PublishedOn { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; } = null!;//Her bir yorum bir postu ilgilendirdiği için böyle bağlantıyı sağladım
        public string UserId { get; set; } = null!;
        public AppUser User { get; set; } = null!;//o yorumu yapanda bir kişi olduğu için böyle bağlantıyı sağladım
    }
}