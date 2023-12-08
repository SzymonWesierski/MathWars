using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MathWars.Pages.TaskPages.TypeOfAnswer;
[Authorize]
[BindProperties]
public class EditAnswerTypeModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public AnswerTypes AnswerType { get; set; }

    public EditAnswerTypeModel(ApplicationDbContext db)
    {
        _db = db;
    }
    public void OnGet(int id)
    {
        AnswerType = _db.AnswerTypes.Find(id);
    }

    public async Task<IActionResult> OnPost()
    {
        if (TaskCategoryValidation())
        {
            _db.AnswerTypes.Update(AnswerType);
            await _db.SaveChangesAsync();
            return RedirectToPage("ViewAnswerType");
        }
        return Page();
    }

    private bool TaskCategoryValidation()
    {
        bool result = true;
        // Category validation
        return result;
    }
}
