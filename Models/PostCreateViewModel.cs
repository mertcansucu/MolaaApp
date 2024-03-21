using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MolaaApp.Models
{
    public class PostCreateViewModel
    {
        public int PostId { get; set; }

        [Required]//zorunlu alan
        [Display(Name = "Başlık")]
        public string? Title { get; set; }

        [Required]
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "İçerik")]
        public string? Content { get; set; }

        [Required]
        [Display(Name = "Url")]
        public string? Url { get; set; }

        [Display(Name ="Resim")]
        public string? Image{ get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}