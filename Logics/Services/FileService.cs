using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logics.Interfaces;

namespace Logics.Services
{
    public class FileService : IFileService
    {
        public async Task<string> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return string.Empty;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                throw new InvalidOperationException("Можно загружать только изображения (.jpg, .png, .gif, .webp)");

            if (!file.ContentType.StartsWith("image/"))
                throw new InvalidOperationException("Неверный тип файла. Ожидается изображение.");

            var fileName = Guid.NewGuid().ToString() + extension;

            var rootPath = Directory.GetCurrentDirectory();
            var uploadDir = Path.Combine(rootPath, "wwwroot", "img");

            Directory.CreateDirectory(uploadDir);

            var filePath = Path.Combine(uploadDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/img/{fileName}";
        }
    }
}

