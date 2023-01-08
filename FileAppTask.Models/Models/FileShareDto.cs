using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAppTask.Models.Models
{
    public class FileShareDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DownloadLink { get; set; }
        public string DownloadAvailableUntil { get; set; }
    }
}
