using System.ComponentModel.DataAnnotations;

namespace MathWars.Models
{
    public class TaskCategoryModel
    {
        [Required(ErrorMessage = "Podaj nazwę kategorii")]
        public string CategoryName { get; set; }

        public DateTime? Created {  get; set; }
    }
}
