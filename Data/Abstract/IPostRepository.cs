using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MolaaApp.Entity;

namespace MolaaApp.Data.Abstract
{
    public interface IPostRepository
    {//IPostRepository =>burada bazı kullanacağım metotları yazıcam ama bunlar sanal olacak, yani interfacede kullanacağım metot tanımlamaları bunları da concrete içinde gerçek bir metoda dönüştürücem
        IQueryable<Post> Posts { get; }//IQueryable=>Contex üzerinden Post verilerini aldığımda extra filtreleme yapabilmek için

        void CreatePost(Post post);
        
        void EditPost(Post post);

        void DeletePost(Post post);
    }
}