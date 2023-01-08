using FileAppTask.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAppTask.Models.Models
{
    public class FileDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileExtension { get; set; }
        public string UploadDate { get; set; }
        public int NumberOfDownloads { get; set; }
    }
}
