using System.ComponentModel.DataAnnotations;

namespace TelegramBotHelper.Models
{
    public class BlacklistOfWords
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string WordsName { get; set; }
    }
}
