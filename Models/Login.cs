using System.ComponentModel.DataAnnotations;

namespace Ctulhu.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Логин обязателен")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Пароль обязателен")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
