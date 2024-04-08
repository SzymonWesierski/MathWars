using System.ComponentModel.DataAnnotations;

namespace MathWars.Models
{
    public class LoginUserModel
    {
        [Required(ErrorMessage = "Podaj nazwę użytkownika")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Podaj hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
