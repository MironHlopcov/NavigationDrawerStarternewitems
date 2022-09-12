using Microsoft.EntityFrameworkCore;

namespace EfcToXamarinAndroid.Core
{
    public class DataItemContext : DbContext
    {
        public DbSet<DataItem>? Cats { get; set; }

        private string DatabasePath { get; set; }

        public DataItemContext(string databasePath)
        {
            //Database.EnsureDeleted();   // удаляем бд со старой схемой
           // Database.EnsureCreated();   // создаем бд с новой схемой
            DatabasePath = databasePath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DatabasePath}");
        }
    }
}
