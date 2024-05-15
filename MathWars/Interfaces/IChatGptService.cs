namespace MathWars.Interfaces
{
    public interface IChatGptService
    {
        Task<string> GetResponseAsync(string prompt);
    }
}
