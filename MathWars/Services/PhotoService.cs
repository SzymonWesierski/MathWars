using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using MathWars.Interfaces;
using MathWars.Helpers;

namespace MathWars.Services
{
    public readonly struct ImageDirectoriesCloudinary
    {
        public static readonly string Tasks = "Tasks";
        public static readonly string TaskAnswers = "TaskAnswers";
        public static readonly string Reports = "Reports";
        public static readonly string Profiles = "ProfilePictures";
    }

    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public PhotoService(IOptions<CloudinarySettingsModel> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.APIKey,
                config.Value.ApiSecret
                );

            _cloudinary = new Cloudinary(acc);
        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file, string folderName)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                    Folder = folderName
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deletParams = new DeletionParams(publicId);

            return await _cloudinary.DestroyAsync(deletParams);
        }
    }
}
