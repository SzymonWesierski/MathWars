using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MathWars.Models;

public class ApplicationUser : IdentityUser
{
    public ICollection<Answers> Answers { get; set; }
}


