using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entity;
using MolaaApp.Models;

namespace MolaaApp.Entity
{
    public class Like
    {
        public int LikeId { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public AppUser User { get; set; } = null!;
    }
}