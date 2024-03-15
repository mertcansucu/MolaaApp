using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MolaaApp.ViewModels
{
    public class EditViewModel
    {
        public string? Id { get; set; } //güncellemek istediğim userın idsi lazım ben bu idye göre güncelliyicem

        public string? FullName { get; set; } 
        //Username e gerek görmediğim için kaldırdım fullname i alıcam direk
       
        [EmailAddress]
        public string? Email { get; set; } 

        [Display(Name ="Resim")]
        public string? Image{ get; set; } = string.Empty;
        
        
        [DataType(DataType.Password)]
        public string? Password { get; set; } 
        
        
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Parola eşleşmiyor!")]
        public string? ConfirmPassword { get; set; }
    }
}