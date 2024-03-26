using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MolaaApp.Data.Abstract;
using MolaaApp.Models;

namespace MolaaApp.ViewComponents
{
    public class NewPosts : ViewComponent
    {
        private IPostRepository _postRepository;
        public NewPosts(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var posts = await _postRepository.Posts
                .Where(p => p.IsActive) // Sadece etkin gönderileri filtrele
                .OrderByDescending(p => p.PubilshedOn)
                .Take(5)
                .ToListAsync();

            return View(posts);
        }
    }
}
