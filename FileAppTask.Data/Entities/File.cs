using FileAppTask.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAppTask.Data
{
    public class File
    {
        [Key]
        public int FileId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public FileType FileType { get; set; }
        public int DownloadCount { get; set; }       
        public DateTime UploadDate { get; set; }
        public List<FileShare> FileShares { get; set; }
    }
}
