using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MathWars.Pages.TaskPages.TypeOfAnswer;
[Authorize(Roles = "admin,taskManager")]
[BindProperties]
public class EditAnswerTypeModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public AnswerTypesViewModel AnswerType { get; set; } = new AnswerTypesViewModel();

    public EditAnswerTypeModel(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task<IActionResult> OnGetAsync(int id)
    {
        var AnswerTypeFromDB = await _db.AnswerTypes
            .FirstOrDefaultAsync(at => at.Id == id) ?? new AnswerTypes();

        if (AnswerTypeFromDB == null) 
        {
            return NotFound(); 
        }

        AnswerType = new AnswerTypesViewModel()
        {
            Id = id,
            Created = AnswerTypeFromDB.Created,
            Name = AnswerTypeFromDB.Name,
            FormatExplanation = AnswerTypeFromDB.FormatExplanation,
            HowManyCorrectAnswers = AnswerTypeFromDB.HowManyCorrectAnswers
        };
        
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if(AnswerType != null) 
        {
            if (ModelState.IsValid)
            {
                var AnswerTypeToDB = new AnswerTypes
                {
                    Id = AnswerType.Id,
                    Created = AnswerType.Created,
                    Name = AnswerType.Name,
                    FormatExplanation = AnswerType.FormatExplanation,
                    HowManyCorrectAnswers = AnswerType.HowManyCorrectAnswers,
                };

                _db.AnswerTypes.Update(AnswerTypeToDB);
                await _db.SaveChangesAsync();
                return RedirectToPage("ViewAnswerType");
            }
        }
        return Page();
    }
}
