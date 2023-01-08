using FileAppTask.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAppTask.Api.Services
{
    public interface IFileService
    {
        Task<List<FileDto>> GetAllFiles();
        Task<List<FileShareDto>> GetAllSharedFiles();
        Task<FileDownloadDto> GetFileDownloadDetails(int fileId);
        Task<bool> SaveFile(IFormFile file);
        Task<bool> GenerateDownloadLink(int fileId, int hour);
        Task<FileDownloadDto> GetFileByFileShareId(Guid fileShareId);
        Task UpdateFileCount(int fileId);
        
    }
}
