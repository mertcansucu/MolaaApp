using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MolaaApp.Data.Abstract;
using MolaaApp.Entity;
using MolaaApp.Models;

namespace MolaaApp.Data.Concrete.EfCore
{
    public class EfLikeRepository : ILikeRepository
    {
        private IdentityContext _context;
        public EfLikeRepository(IdentityContext context)
        {
            _context = context;
        }

        public IQueryable<Like> likes => _context.likes;
        public bool UserLikedPost(string userId, int postId)
        {
            return _context.likes.Any(l => l.UserId == userId && l.PostId == postId);
        }

        public void AddLike(Like like)
        {
            _context.likes.Add(like);
            _context.SaveChanges();
        }

        public void RemoveLike(Like like)
        {
            _context.likes.Remove(like);
            _context.SaveChanges();
        }
    }
}