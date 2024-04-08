using MathWars.Entities;
using System.ComponentModel.DataAnnotations;

namespace MathWars.Models
{
    public class UserWithRoleModel
    {
        public ApplicationUser user { get; set; }
        public string RoleName { get; set; }
    }
}
