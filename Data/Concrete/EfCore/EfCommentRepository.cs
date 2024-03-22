using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MolaaApp.Data.Abstract;
using MolaaApp.Entity;
using MolaaApp.Models;

namespace MolaaApp.Data.Concrete
{
    public class EfCommentRepository : ICommentRepository
    {
        private IdentityContext _context;
        public EfCommentRepository(IdentityContext context)
        {
            _context = context;
        }
        
        public IQueryable<Comment> Comments => _context.Comments;

        public void CreateComment(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }
    }
}