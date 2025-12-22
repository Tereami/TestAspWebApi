using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestAspWebApi.Models
{
    public class LoginViewModel
    {
        [Required]
        [DisplayName("Имя")]
        public string UserName { get; set; }

        [Required]
        [DisplayName("Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [HiddenInput(DisplayValue = false)]
        public string? ReturnUrl { get; set; }
    }
}
