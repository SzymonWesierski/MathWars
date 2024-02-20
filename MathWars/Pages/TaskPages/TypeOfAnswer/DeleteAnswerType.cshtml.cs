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
public class DeleteAnswerTypeModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public AnswerTypes AnswerType { get; set; } = new AnswerTypes();

    public DeleteAnswerTypeModel(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> OnGet(int id)
    {
        AnswerType = await _db.AnswerTypes
            .FirstOrDefaultAsync(c => c.Id == id) ?? new AnswerTypes();

        if (AnswerType == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (AnswerType == null)
        {
            return NotFound();
        }

        var answerTypeFromDb = _db.AnswerTypes.Find(AnswerType.Id);
        if (answerTypeFromDb != null)
        {
            _db.AnswerTypes.Remove(answerTypeFromDb);
            await _db.SaveChangesAsync();
            return RedirectToPage("ViewAnswerType");
        }
        return Page();

    }
}
