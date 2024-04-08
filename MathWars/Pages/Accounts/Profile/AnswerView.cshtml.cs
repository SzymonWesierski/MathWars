using MathWars.Entities;
using MathWars.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MathWars.Pages.Accounts.Profile
{
    [Authorize]
    [BindProperties]
    public class AnswerViewModel : PageModel
    {
        private readonly IUnitOfWork _uow;

        public AnswerViewModel(IUnitOfWork uow) 
        {
            _uow = uow;
        }

        public UserAnswer Answer { get; set; }
        public string Uid { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {
            Answer = await _uow.UserAnswersRepository.GetUserAnswersAndTaskAsync(id);

			if (Answer == null)
            {
                return NotFound();
            }

            Uid = Answer.UserId;

            return Page();  
        }
    }
}
