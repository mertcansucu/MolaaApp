using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MolaaApp.Entity;

namespace MolaaApp.Entity
{
    public class Tag
    {
        public int TagId { get; set; }
        public string? Text { get; set; }
        public string? Url { get; set; }
        public List<Post> Posts { get; set; } = new List<Post>();//Her etiket birden fazla postla ilgili olabileceği için liste şeklinde bağladım
    }
}