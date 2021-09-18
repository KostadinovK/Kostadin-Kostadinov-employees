using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Services
{
    public interface IUploadService
    {
        Task<string> UploadCSVFileAsync(IFormFile file);

        void DeleteFile(string filePath);
    }
}
