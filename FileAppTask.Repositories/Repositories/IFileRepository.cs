using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileAppTask.Data;
using FileAppTask.Models.Models;
using File = FileAppTask.Data.File;
using FileShare = FileAppTask.Data.FileShare;

namespace FileAppTask.Repositories.Repositories
{
    public interface IFileRepository
    {
        Task<List<File>> GetAllFiles();
        Task<List<FileShare>> GetAllSharedFiles();
        Task<File> GetFile(int fileId);
        Task<File> GetFileByFileShareId(Guid fileShareId);
        Task<bool> SaveFile(File file);
        Task<bool> SaveFileShare(FileShare fileShare);
        Task UpdateDownloadCount(int fileId);
    }
}
