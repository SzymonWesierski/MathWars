using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;

namespace MathWars.Pages.TaskPages.TypeOfAnswer;
[Authorize]
public class ViewAnswerTypeModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ApplicationDbContext _db;
    public IEnumerable<AnswerTypes> AnswerType { get; set; }

    public ViewAnswerTypeModel(ApplicationDbContext db, ILogger<IndexModel> logger)
    {
        _db = db;
        _logger = logger;
    }

    public void OnGet()
    {
        AnswerType = _db.AnswerTypes;
    }
}