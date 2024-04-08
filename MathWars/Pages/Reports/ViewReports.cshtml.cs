using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using MathWars.Helpers;
using MathWars.Entities;
using MathWars.Interfaces;

namespace MathWars.Pages.Reports
{
    [Authorize(Policy = "RequireAdminOrManagerRole")]
    public class ViewReportsModel : PageModel
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;

        public ViewReportsModel(IUnitOfWork uow, IConfiguration configuration)
        {
            _uow = uow;
            _configuration = configuration;
        }

        public PaginatedList<UsersReports> Reports { get; set; }

        public async Task OnGetAsync(int? pageIndex)
        {
            int pageSize = _configuration.GetSection("NumberOfElementsInList")
                .GetValue<int>("Reports");

            Reports = await _uow.UserReportsRepository
                .GetReportsAsync(pageIndex, pageSize);
        }
    }
}
