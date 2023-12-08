using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MathWars.Pages.TaskPages.TypeOfAnswer;
[Authorize]
[BindProperties]
public class CreateAnswerTypeModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public AnswerTypes AnswerType { get; set; }

    public CreateAnswerTypeModel(ApplicationDbContext db)
    {
        _db = db;
    }
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (TaskCategoryValidation())
        {
            await _db.AnswerTypes.AddAsync(AnswerType);
            await _db.SaveChangesAsync();
            return RedirectToPage("ViewAnswerType");
        }
        return Page();
    }

    private bool TaskCategoryValidation()
    {
        bool result = true;
        // Add validation
        return result;
    }
}
