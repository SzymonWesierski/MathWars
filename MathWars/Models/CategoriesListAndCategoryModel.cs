using MathWars.Entities;
using MathWars.Helpers;

namespace MathWars.Models
{
    public class CategoriesListAndCategoryModel
    {
        public TaskCategoryModel Category { get; set; }
        public PaginatedList<TasksCategory> Categories { get; set; }
		public List<int> SelectedCategoryIds { get; set; } = new();
        public Dictionary<int, List<int>> CategoriesIds { get; set; }
		public int CurrentPageIndex { get; set; }
        public int NextPageIndex { get; set; } = 1;
	}
}
