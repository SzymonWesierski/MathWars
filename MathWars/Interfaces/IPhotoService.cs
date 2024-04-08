using CloudinaryDotNet.Actions;

namespace MathWars.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file, string folderName);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
