using MathWars.Interfaces;

namespace MathWars.Helpers
{
    public readonly struct ImageDirectoriesOnServer
    {
        public static readonly string Task = "forTasks";
        public static readonly string Report = "reportPictures";
        public static readonly string Profile = "profilePictures";
    }


    public class ImageOnServerService : IImageOnServerService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ImageOnServerService> _logger;

        public ImageOnServerService(IWebHostEnvironment webHostEnvironment, IConfiguration configuration, ILogger<ImageOnServerService> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _logger = logger;
        }

        public string SaveImage(string directory, IFormFile image)
        {
            try
            {
                // generating new unique file name 
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;

                var appDirectory = _webHostEnvironment.WebRootPath;

                var imageDirectory = _configuration.GetSection("ImagesDirectorys").GetValue<string>(directory);

                if (imageDirectory == null || uniqueFileName == null)
                {
                    _logger.LogError("Can't find image directory or create unique file name");
                    return null;
                }
                else
                {
                    //create directory if doesn't exists
                    var directoryPath = appDirectory + imageDirectory;
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    // Save image on server
                    var filePath = Path.Combine(imageDirectory, uniqueFileName);

                    var physicalFilePath = appDirectory + filePath;

                    using (var fileStream = new FileStream(physicalFilePath, FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }
                    return filePath;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while saving image");
                return null;
            }
        }

        public void DeleteImage(string imagePath)
        {
            try
            {
                var physicalFilePath = _webHostEnvironment.WebRootPath + imagePath;
                if (File.Exists(physicalFilePath))
                {
                    File.Delete(physicalFilePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting image");
            }
        }
    }
}
