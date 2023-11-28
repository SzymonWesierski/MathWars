using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MathWars.Models;

public class ApplicationUser : IdentityUser
{
    public ICollection<Answers> Answers { get; set; }
    public int Level { get; set; } = 1;
    public int Experience { get; set; } = 0;
    public int ExpToReachNewLvl { get; set; } = 20;
}


