using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace MathWars.Pages.TaskPages.TypeOfAnswer;
[Authorize]
public class ViewAnswerTypeModel : PageModel
{
    private readonly ApplicationDbContext _db;
	private readonly IConfiguration _configuration;
	public ViewAnswerTypeModel(ApplicationDbContext db, IConfiguration configuration)
    {
        _db = db;
		_configuration = configuration;
    }

	public PaginatedList<AnswerTypes> AnswerType { get; set; }

	public async Task OnGetAsync(int? pageIndex)
	{
		int pageSize = _configuration.GetSection("NumberOfElementsInList").GetValue<int>("AnswerType");
        IQueryable<AnswerTypes> answerTypeQuery = _db.AnswerTypes;

		AnswerType = await PaginatedList<AnswerTypes>.CreateAsync(answerTypeQuery, pageIndex ?? 1, pageSize);
	}
}