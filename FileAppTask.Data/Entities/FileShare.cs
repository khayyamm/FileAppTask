using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAppTask.Data
{
    public class FileShare
    {
        public Guid FileShareId { get; set; }
        public DateTime DownloadAvailableUntil { get; set; }
        public File File { get; set; }
        [ForeignKey("FileId")]
        public int FileId { get; set; }
    }
}
