namespace MathWars.Interfaces
{
    public interface IImageOnServerService
    {
        string SaveImage(string directory, IFormFile image);
        void DeleteImage(string imagePath);
    }
}
