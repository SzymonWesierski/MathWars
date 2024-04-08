namespace MathWars.Helpers
{
	public class TaskToSolveParams : PaginationParams
	{
		public int DifficultyLevel { get; set; } = 0;
		public int CategoryId { get; set; } = 0;
		public bool OnlyNotSolved { get; set; } = false;
	}
}
