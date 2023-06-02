using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TelegramBotHelper.DbContexts;

namespace TelegramBotHelper.Models
{
    public class CommandsName
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CommandName { get; set; }

        public async Task<List<CommandsName>> GetAllCommands(ApplicationDbContext db)
        {
            return await db._CommandsName.ToListAsync();
        }
    }
}