using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MathWars.Pages.TaskPages.TypeOfAnswer;
[Authorize(Roles = "admin,taskManager")]
[BindProperties]
public class CreateAnswerTypeModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public AnswerTypesViewModel AnswerType { get; set; }

    public CreateAnswerTypeModel(ApplicationDbContext db)
    {
        _db = db;
        AnswerType = new AnswerTypesViewModel();
    }
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (AnswerType == null) 
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var answerType = new AnswerTypes
            {
                Name = AnswerType.Name,
                FormatExplanation = AnswerType.FormatExplanation,
                HowManyCorrectAnswers = AnswerType.HowManyCorrectAnswers,
                Created = AnswerType.Created
            };
            await _db.AnswerTypes.AddAsync(answerType);
            await _db.SaveChangesAsync();
            return RedirectToPage("ViewAnswerType");
        }
        return Page();
    }
}
