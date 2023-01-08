using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FileAppTask.Data
{
    public class FileContext : DbContext
    {
        public FileContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<File>()
                .Property(f => f.DownloadCount)
                .HasDefaultValue(0);
        }
        public DbSet<File> Files { get; set; }
        public DbSet<FileShare> FileShares { get; set; }
    }
}
