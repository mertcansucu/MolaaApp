using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MolaaApp.ViewModels
{
    public class ResetPasswordModel
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage = "Parola eşleşmiyor!")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}