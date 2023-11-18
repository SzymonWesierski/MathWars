using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathWars.Data;
using MathWars.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace MathWars.Pages
{
    
    public class IndexGuestModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _db;
        public IEnumerable<Tasks> Tasks { get; set; }

        public IndexGuestModel(ApplicationDbContext db, ILogger<IndexModel> logger)
        {
            _db = db;
            _logger = logger;
        }

        public IActionResult OnPost(string returnUrl = null) 
        {
            if (returnUrl == null || returnUrl == "/")
            {
                return RedirectToPage("/IndexGuest");
            }
            else
            {
                return RedirectToPage(returnUrl);
            }
        }


        public void OnGet()
        {
            Tasks = _db.Tasks;
        }
    }
}