using MathWars.Models;
using System.ComponentModel.DataAnnotations;

namespace MathWars.ViewModels
{
    public class UserWithRole
    {
        public ApplicationUser user {  get; set; }
        public String RoleName {  get; set; }
    }
}
