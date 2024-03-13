using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entity;
using Microsoft.AspNetCore.Identity;
using MolaaApp.Entity;

namespace MolaaApp.Models
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string? Image { get; set; }
        public List<Post> Posts { get; set; } = new List<Post>();//bir kullanıcı birden fazla post yayınlayacağı için liste şeklinde bağlantıyı sağladım
        public List<Comment> Comments { get; set; } = new List<Comment>();//Bir kullanıı bir posta birden fazla yorum yapabildiği için liste şeklinde bağladım
    }
}