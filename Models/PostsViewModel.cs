using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entity;

namespace MolaaApp.Models
{
    public class PostsViewModel
    {
        public List<Post> Posts { get; set; } = new();
    }
}