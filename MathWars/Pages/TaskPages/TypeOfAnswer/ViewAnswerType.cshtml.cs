using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;

namespace MathWars.Pages.TaskPages.TypeOfAnswer;
[Authorize]
public class ViewAnswerTypeModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public IEnumerable<AnswerTypes> AnswerType { get; set; }

    public ViewAnswerTypeModel(ApplicationDbContext db)
    {
        _db = db;
        AnswerType = new List<AnswerTypes>();
    }

    public void OnGet()
    {
        AnswerType = _db.AnswerTypes;
    }
}