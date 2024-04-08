using System.ComponentModel.DataAnnotations;

namespace MathWars.Models
{
    public class RegisterUserModel
    {
        [Required(ErrorMessage = "Podaj email"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Podaj nazwę użytkownika")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Podaj hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Podaj ponownie hasło")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Hasło i ponownie podane hasło nie pasują do siebie")]
        public string ConfirmPassword { get; set; }
    }
}
