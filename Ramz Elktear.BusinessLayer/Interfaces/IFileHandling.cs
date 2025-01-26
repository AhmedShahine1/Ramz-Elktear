using Microsoft.AspNetCore.Http;
using Ramz_Elktear.core.Entities.Files;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface IFileHandling
    {
        public Task<string> UploadFile(IFormFile file, Paths paths, string oldFilePath = null);
        public Task<string> UpdateFile(IFormFile file, Paths paths, string imageId);
        public Task<string> DefaultProfile(Paths paths);
        public Task<string> GetFile(string imageId);
    }
}
