using AutoMapper;
using FileAppTask.Data;
using FileAppTask.Data.Enums;
using FileAppTask.Models.Models;
using FileAppTask.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using File = FileAppTask.Data.File;
using FileShare = FileAppTask.Data.FileShare;

namespace FileAppTask.Api.Services
{
    public class FileSerivce : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IMapper _mapper;

        public FileSerivce(IFileRepository fileRepository, IMapper mapper)
        {
            _fileRepository = fileRepository;
            _mapper = mapper;
        }
        public async Task<bool> GenerateDownloadLink(int fileId, int hour)
        {
            var newFileShareId = Guid.NewGuid();
            var availableUntil = DateTime.Now.AddHours(hour);
            var fileShare = new FileShare()
            {
                FileId = fileId,
                DownloadAvailableUntil = availableUntil,
                FileShareId = newFileShareId
            };
            var result = await _fileRepository.SaveFileShare(fileShare);
            return result;
        }

        public async Task<List<FileDto>> GetAllFiles()
        {
            var files = await _fileRepository.GetAllFiles();
            return _mapper.Map<List<File>,List<FileDto>>(files);
        }

        public async Task<List<FileShareDto>> GetAllSharedFiles()
        {
            var files = await _fileRepository.GetAllSharedFiles();
            return _mapper.Map<List<FileShare>, List<FileShareDto>>(files);
        }

        public async Task<FileDownloadDto> GetFileByFileShareId(Guid fileShareId)
        {
            var file = await _fileRepository.GetFileByFileShareId(fileShareId);            
            return _mapper.Map<FileDownloadDto>(file);
        }

        public async Task<FileDownloadDto> GetFileDownloadDetails(int fileId)
        {
            var file = await _fileRepository.GetFile(fileId);
            return _mapper.Map<FileDownloadDto>(file);
        }

        public async Task<bool> SaveFile(IFormFile file)
        {
            var path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadedFiles"));

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var filePath = Path.Combine(path, file.FileName);
            var fileExtension = Path.GetExtension(filePath).ToLower();

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileEntity = new File()
            {
                FileType = GetFileType(fileExtension),
                Name = file.FileName,
                Path = filePath,
                UploadDate = DateTime.Now
            };

            return await _fileRepository.SaveFile(fileEntity);
        }

        private static FileType GetFileType(string extension) => extension switch
        {
            ".pdf" => FileType.PDF,
            ".docx" => FileType.DOCX,
            ".gif" => FileType.GIF,
            ".jpeg" => FileType.JPEG,
            ".jpg" => FileType.JPG,
            ".png" => FileType.PNG,
            ".txt" => FileType.TXT,
            ".xlsx" => FileType.XLSX,
            _ => throw new ArgumentOutOfRangeException($"Not expected extension: {extension}")
        };

        public async Task UpdateFileCount(int fileId)
        {
            await _fileRepository.UpdateDownloadCount(fileId);
        }
    }
}
