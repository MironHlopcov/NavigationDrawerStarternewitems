using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EfcToXamarinAndroid.MicrationsHelper
{
    public class DataItemContext : DbContext
    {
        public DbSet<DataItem> Cats { get; set; }

        private string DatabasePath { get; set; }

        public DataItemContext(string databasePath)
        {
            DatabasePath = databasePath;
        }
        public DataItemContext()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DatabasePath}");
        }
    }
}
