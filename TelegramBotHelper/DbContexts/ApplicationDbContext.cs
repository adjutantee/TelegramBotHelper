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
        public DbSet<CommandsName> _CommandsName { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TelegramBotHelperDb;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommandsName>().HasData(new List<CommandsName>()
            {
                new CommandsName
                {
                    Id = 1,
                    CommandName = "/addblacklist"
                },
                new CommandsName
                {
                    Id = 2,
                    CommandName = "/wordsinfo"
                },
                new CommandsName
                {
                    Id = 3,
                    CommandName = "/deleteword"
                }
            });
        }
    }
}
