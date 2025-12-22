using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestAspWebApi.Models
{
    public class RegisterViewModel
    {
        [Required]
        [DisplayName("Имя")]
        public string UserName { get; set; }

        [Required]
        [DisplayName("Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DisplayName("Повторите пароль")]
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
