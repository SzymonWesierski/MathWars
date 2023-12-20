using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MathWars.Pages.TaskPages.TypeOfAnswer;
[Authorize]
[BindProperties]
public class EditAnswerTypeModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public AnswerTypes AnswerType { get; set; } = new AnswerTypes();

    public EditAnswerTypeModel(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task<IActionResult> OnGetAsync(int id)
    {
        AnswerType = await _db.AnswerTypes
            .FirstOrDefaultAsync(at => at.Id == id) ?? new AnswerTypes();

        if (AnswerType == null) 
        {
            return NotFound(); 
        }
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if(AnswerType != null) 
        {
            if (TaskCategoryValidation(AnswerType) && ModelState.IsValid)
            {
                _db.AnswerTypes.Update(AnswerType);
                await _db.SaveChangesAsync();
                return RedirectToPage("ViewAnswerType");
            }
        }
        return Page();
    }

    private bool TaskCategoryValidation(AnswerTypes AnswerType)
    {
        bool result = true;

        if (AnswerType.Name == null)
        {
            ModelState.AddModelError("AnswerType.Name", "Pole nazwy jest wymagane");
            result = false;
        }
        if (AnswerType.FormatExplanation == null)
        {
            ModelState.AddModelError("AnswerType.FormatExplanation", "Pole wyjaœnienia typu odpowiedzi jest wymagane");
            result = false;
        }
        if (AnswerType.HowManyCorrectAnswers == 0)
        {
            ModelState.AddModelError("AnswerType.HowManyCorrectAnswers", "Pole iloœci prawid³owych odpowiedzi musi wynosiæ wiêcej ni¿ 0");
            result = false;
        }

        return result;
    }
}
