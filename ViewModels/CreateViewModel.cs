using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MolaaApp.ViewModels
{
    public class CreateViewModel
    {
        [Required]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; } = string.Empty;
        

        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; } = string.Empty;
        

        [Required]
        [EmailAddress]
        [Display(Name = "Eposta")]
        public string Email { get; set; } = string.Empty;
        
        
        [Display(Name ="Resim")]
        public string? Image{ get; set; } = string.Empty;

        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string Password { get; set; } = string.Empty;
        
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage = "Parolanız eşleşmiyor!")]
        [Display(Name = "Parola Tekrar")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}