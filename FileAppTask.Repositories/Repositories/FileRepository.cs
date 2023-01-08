using FileAppTask.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = FileAppTask.Data.File;
using FileShare = FileAppTask.Data.FileShare;

namespace FileAppTask.Repositories.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly FileContext _fileContext;

        public FileRepository(FileContext fileContext)
        {
            _fileContext = fileContext;
        }
        public async Task<List<File>> GetAllFiles()
        {
            return await _fileContext.Files.ToListAsync();
        }

        public async Task<List<FileShare>> GetAllSharedFiles()
        {
            return await _fileContext.FileShares.Include(f=>f.File).ToListAsync();
        }

        public async Task<File> GetFileByFileShareId(Guid fileShareId)
        {
            var result = await _fileContext.FileShares.Include(f => f.File)
                                           .Where(w => w.FileShareId == fileShareId && w.DownloadAvailableUntil >= DateTime.Now)
                                           .Select(s => new File()
                                           {
                                               FileId = s.File.FileId,
                                               DownloadCount = s.File.DownloadCount,
                                               FileType = s.File.FileType,
                                               Name = s.File.Name,
                                               Path = s.File.Path,
                                               UploadDate = s.File.UploadDate
                                           }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<File> GetFile(int fileId)
        {
            return await _fileContext.Files.FirstOrDefaultAsync(f => f.FileId == fileId);
        }

        public async Task<bool> SaveFile(File file)
        {
            await _fileContext.Files.AddAsync(file);
            return await _fileContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> SaveFileShare(FileShare fileShare)
        {
            await _fileContext.FileShares.AddAsync(fileShare);
            return await _fileContext.SaveChangesAsync() > 0;
        }

        public async Task UpdateDownloadCount(int fileId)
        {
            var file = await _fileContext.Files.FirstOrDefaultAsync(f => f.FileId == fileId);
            file.DownloadCount++;
            await _fileContext.SaveChangesAsync();
        }
    }
}
