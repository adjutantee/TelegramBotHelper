using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TelegramBotHelper.DbContexts;

namespace TelegramBotHelper.Models
{
    public class BlacklistOfWords
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string WordsName { get; set; }

        public async Task<List<BlacklistOfWords>> GetAllBlacklistWords(ApplicationDbContext db)
        {
            return await db._BlacklistOfWords.ToListAsync();
        }
    }
}
