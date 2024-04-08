namespace MathWars.Entities
{
	public class AnswersToTask
	{
		public int id {  get; set; }
		public string Content { get; set; }
		public string PhotoUrl { get; set; }
        public string PublicPhotoId { get; set; }
        public bool IsCorrect { get; set; }
		public int TaskId { get; set; }
		public Tasks Task { get; set; }
	}
}
