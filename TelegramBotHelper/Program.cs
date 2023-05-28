using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotHelper.DbContexts;
using TelegramBotHelper.Models;

class Program
{
    static void Main(string[] args)
    {
        var client = new TelegramBotClient("6089349486:AAGnnEaOITkmUIyEbDpEpBw-gE1R0l9OrLs");
        client.StartReceiving(Update, Error);
        Console.ReadLine();
    }

    async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
    {
        using var db = new ApplicationDbContext();
        var message = update.Message;

        if (message != null && message.Text != null)
        {
            switch (message.Text.ToLower())
            {
                case string text when text.Contains("test"):
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        text: $"Пользователь {message.From.FirstName}, в вашем последнем сообщении мною было обнаружено слово или несколько слов, входящих в черный список " +
                        $"этой группы, в следствии чего я вынужден был удалить ваше последнее сообщение а так же выдать вам предупреждение. Если вы считаете что это ошибка, " +
                        $"обратитесь к администрации группы.");
                break;
                case "/addblacklist":
                    // Получение информации о пользователе
                    User sender = message.From;
                    // Получение информацию о чате | группе
                    Chat chat = message.Chat;
                    
                    if (chat.Type == ChatType.Group || chat.Type == ChatType.Supergroup)
                    {
                        // Получение информации о участниках группы
                        ChatMember[] chatMembers = await botClient.GetChatAdministratorsAsync(chat.Id);
                        bool isAdmin = false;
                        // проверка участников на наличие роли администратора
                        foreach (ChatMember chatMember in chatMembers)
                        {
                            if (chatMember.User.Id == sender.Id && (chatMember.Status == ChatMemberStatus.Creator || chatMember.Status == ChatMemberStatus.Administrator))
                            {
                                // Пользователь является администратором группы
                                // Ваш код для добавления слова в черный список
                                isAdmin = true;
                                await botClient.SendTextMessageAsync(
                                    message.Chat.Id,
                                    text: "Введите слово (или слова в формате text, text, text) для добавления в черный список");

                                // Получение обновлений
                                Update[] updates = await botClient.GetUpdatesAsync();

                                foreach (Update fUpdate in updates)
                                {
                                    if (fUpdate.Type == UpdateType.Message && fUpdate.Message.Type == MessageType.Text)
                                    {
                                        string command = "/addblacklist";
                                        string input = fUpdate.Message.Text;

                                        if (input.StartsWith(command + " "))
                                        {
                                            string words = input.Substring(command.Length + 1);
                                            string[] wordList = words.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                                            foreach (string word in wordList)
                                            {
                                                var blackListWord = new BlacklistOfWords { WordsName = word };
                                                db.Add(blackListWord);
                                            }

                                            await db.SaveChangesAsync();
                                            await botClient.SendTextMessageAsync(
                                                message.Chat.Id,
                                                text: $"Следующие слова '{words}' были успешно добавлены в черный список");
                                        }
                                        else
                                        {
                                            await botClient.SendTextMessageAsync(
                                                message.Chat.Id,
                                                text: $"Неизвестная ошибка или вводимые данные были некорректны. Подробнее в консоли разработчика.");
                                        }

                                        break;
                                    }
                                }

                                break;
                            }
                        }


                        if (!isAdmin)
                        {
                            await botClient.SendTextMessageAsync(
                                message.Chat.Id,
                                replyToMessageId: message.MessageId,
                                text: "Отказ в доступе. Данную команду имеют право вводить только Администраторы.");

                        }
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(
                            message.Chat.Id,
                            replyToMessageId: message.MessageId,
                            text: "Данный чат не является групповым или супергрупповым.");
                    }
                break;
            }
        }
    }

    private static Task<string> Error(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        return Task.FromResult("Результат ошибки");
    }

}