using Microsoft.EntityFrameworkCore;
using TelegramBotHelper.Models;

namespace TelegramBotHelper.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<BlacklistOfWords> _BlacklistOfWords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TelegramBotHelperDb;");
        }
    }
}
