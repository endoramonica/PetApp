using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Pet.Core.Application.Infrastructure;


namespace Pet.Core.Application.Services
{
    public class PhotoUploadService : IPhotoUploadService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public PhotoUploadService(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public async Task<(string PhotoPath, string PhotoUrl)> UploadPhotoAsync(IFormFile photo, params string[] folderPaths)
        {
            if (photo.Length > 5 * 1024 * 1024) // Giới hạn 5MB
                throw new Exception("File size exceeds limit.");

            var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
            if (!allowedExtensions.Contains(Path.GetExtension(photo.FileName).ToLower()))
                throw new Exception("Invalid file format.");

            var targetFolderPath = Path.Combine(new[] { _webHostEnvironment.WebRootPath }.Concat(folderPaths).ToArray());

            if (!Directory.Exists(targetFolderPath))
            {
                Directory.CreateDirectory(targetFolderPath);
            }

            var extension = Path.GetExtension(photo.FileName);
            var newPhotoName = $"{Guid.NewGuid()}_{DateTime.UtcNow.Ticks}{extension}";
            var fullPhotoPath = Path.Combine(targetFolderPath, newPhotoName);

            using (FileStream fs = File.Create(fullPhotoPath))
            {
                await photo.CopyToAsync(fs);
            }

            var domainUrl = _configuration.GetValue<string>("Domain")?.TrimEnd('/');
            var photoUrl = $"{domainUrl}/{string.Join('/', folderPaths)}/{newPhotoName}";

            return (fullPhotoPath, photoUrl);
        }

        public async Task DeletePhotoAsync(string photoPath)
        {
            if (File.Exists(photoPath))
            {
                await Task.Run(() => File.Delete(photoPath));
            }
        }
    }
}