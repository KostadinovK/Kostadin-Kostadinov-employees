using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Services
{
    public class UploadService : IUploadService
    {
        public async Task<string> UploadCSVFileAsync(IFormFile file)
        {
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "TempFiles");

            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }

            var fileName = "data.csv";

            var fullPath = Path.Combine(pathToSave, fileName);

            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return fullPath;
        }

        public void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }
    }
}
