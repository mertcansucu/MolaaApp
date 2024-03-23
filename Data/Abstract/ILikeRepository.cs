using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MolaaApp.Entity;

namespace MolaaApp.Data.Abstract
{
    public interface ILikeRepository
    {
        IQueryable<Like> likes { get; }
        bool UserLikedPost(string userId, int postId);
        void AddLike(Like like);
        void RemoveLike(Like like);
    }
}