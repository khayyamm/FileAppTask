using FileAppTask.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.ComponentModel.DataAnnotations;

namespace FileAppTask.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileController(ILogger<FileController> logger, IFileService fileService, IHttpContextAccessor httpContextAccessor)
        {
            _fileService = fileService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("allfiles")]
        public async Task<IActionResult> GetAllFiles()
        {
            try
            {               
                var result = await _fileService.GetAllFiles();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("allsharedfiles")]
        public async Task<IActionResult> GetAllSharedFiles()
        {
            try
            {
                var result = await _fileService.GetAllSharedFiles();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("getfile/{fileid}")]
        public async Task<IActionResult> GetFile(int fileid)
        {
            try
            {
                var file = await _fileService.GetFileDownloadDetails(fileid);
              
                if (!System.IO.File.Exists(file.Path))
                    return NotFound();

                return await GetFileStream(file.Path);
              
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("getsharedfile/{fileshareid}")]
        public async Task<IActionResult> GetFile(Guid fileshareid)
        {
            try
            {
                var file = await _fileService.GetFileByFileShareId(fileshareid);

                if (!System.IO.File.Exists(file?.Path))
                    return NotFound();

                await _fileService.UpdateFileCount(file.Id);
                return await GetFileStream(file.Path);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("getfiledownloadlink/{fileid}/{hour}")]
        public async Task<IActionResult> GetFileDownloadLink(int fileid, int hour)
        {
            try
            {
                var result = await _fileService.GenerateDownloadLink(fileid, hour);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm]IFormFile file)
        {
            try
            {
                var result = await _fileService.SaveFile(file);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        #region Private Methods
        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;

            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }

        private async Task<FileStreamResult> GetFileStream(string path)
        {
            var memory = new MemoryStream();
            await using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(path));
        }
        #endregion

    }
}
