using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MolaaApp.Entity;
using Microsoft.AspNetCore.Identity;

namespace MolaaApp.Models
{
    public class AppUser:IdentityUser
    {
        public string? FullName { get; set; }
        public string? Image { get; set; }
        public string? OrganizerId { get; set; } // Organizatörün kimliği

        public List<Post> Posts { get; set; } = new List<Post>();//bir kullanıcı birden fazla post yayınlayacağı için liste şeklinde bağlantıyı sağladım
        public List<Comment> Comments { get; set; } = new List<Comment>();//Bir kullanıı bir posta birden fazla yorum yapabildiği için liste şeklinde bağladım
    }
}