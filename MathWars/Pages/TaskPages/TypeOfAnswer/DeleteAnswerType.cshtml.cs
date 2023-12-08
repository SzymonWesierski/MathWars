using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MathWars.Pages.TaskPages.TypeOfAnswer;
[Authorize]
[BindProperties]
public class DeleteAnswerTypeModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public AnswerTypes AnswerType { get; set; }

    public DeleteAnswerTypeModel(ApplicationDbContext db)
    {
        _db = db;
    }
    public void OnGet(int id)
    {
        AnswerType = _db.AnswerTypes.Find(id);
    }

    public async Task<IActionResult> OnPost()
    {
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
