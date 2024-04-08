using System.ComponentModel.DataAnnotations;

namespace MathWars.Models
{
	public class UsersReportsModel
	{
		public int Id { get; set; }
		public DateTime Created { get; set; } = DateTime.Now;
		[Required(ErrorMessage = "Proszę podać tytuł"), Display(Name = "Tytuł:")]
		public string Title { get; set; }
		[Required(ErrorMessage = "Proszę podać opis problemu"), Display(Name = "Opis:")]
		public string Description { get; set; }
		[Display(Name = "Możesz dodać zdjęcie:")]
		public string ImageUrl { get; set; } = string.Empty;
		public string PublicImageId { get; set; } = string.Empty;

		public string UserId { get; set; }
		public int TaskId { get; set; } = 0;
	}
}
