using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MolaaApp.Data.Abstract;

namespace MolaaApp.ViewComponents
{
    public class NewPosts : ViewComponent
    {
        private IPostRepository _postRepository;
        public NewPosts(IPostRepository postRepository){
            _postRepository = postRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(){
            return View( await _postRepository
                                .Posts
                                .OrderByDescending(p => p.PubilshedOn)//veri tabanındaki bilgileri azalan şekilde sıralattım
                                .Take(5)//son 5 veriyi aldım
                                .ToListAsync());
        }
    }
}