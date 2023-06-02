using System.ComponentModel.DataAnnotations;

namespace TelegramBotHelper.Models
{
    public class CommandsName
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CommandName { get; set; }
    }
}