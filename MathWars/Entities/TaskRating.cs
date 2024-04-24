namespace MathWars.Entities
{
	public class TaskRating
	{
		public int Id { get; set; }
		public int Value { get; set; }
		public int TaskId { get; set; }
		public Tasks Task { get; set; }
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
	}
}
