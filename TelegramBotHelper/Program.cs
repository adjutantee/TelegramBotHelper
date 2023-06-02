using Microsoft.EntityFrameworkCore;
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
        var client = new TelegramBotClient("YOUR TOKEN");
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
                case string text when db._BlacklistOfWords.AsEnumerable().Any(x => string.Equals(text, x.WordsName, StringComparison.OrdinalIgnoreCase)):
                    await botClient.DeleteMessageAsync(
                        message.Chat.Id, 
                        message.MessageId);
                break;
                case string text when db._CommandsName.Any(w => text.Contains(w.CommandName)):
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
                                isAdmin = true;
                                Update[] updates = await botClient.GetUpdatesAsync();

                                foreach (Update fUpdate in updates)
                                {
                                    if (fUpdate.Type == UpdateType.Message && fUpdate.Message.Type == MessageType.Text)
                                    {
                                        string addCommand = "/addblacklist";
                                        string infoCommand = "/wordsinfo";
                                        string wordDeleteCommand = "/deleteword";
                                        string helpCommand = "/help";
                                        string input = fUpdate.Message.Text;

                                        if (input.StartsWith(addCommand))
                                        {
                                            string[] words = input.Substring(addCommand.Length).Trim().Split(',');

                                            if (string.IsNullOrEmpty(words[0]))
                                            {
                                                await botClient.SendTextMessageAsync(
                                                    message.Chat.Id,
                                                    text: $"Возможно вводимые данные были некорректны. Пожалуйста, вводите данные по следующему формату - /addblacklist Word1, Word2, Word3 ");
                                                break;
                                            }

                                            foreach (string word in words)
                                            {
                                                var blackListWord = new BlacklistOfWords { WordsName = word.Trim() };
                                                db.Add(blackListWord);
                                            }

                                            await db.SaveChangesAsync();
                                            await botClient.SendTextMessageAsync(
                                                message.Chat.Id,
                                                text: $"Следующие слова '{string.Join(", ", words)}' были успешно добавлены в черный список");
                                        }

                                        else if (input.StartsWith(infoCommand))
                                        {
                                            var blacklist = new BlacklistOfWords();
                                            var allWords = await blacklist.GetAllBlacklistWords(db);

                                            string wordsInfo = string.Join(", ", allWords.Select(word => word.WordsName));
                                            await botClient.SendTextMessageAsync(
                                                message.Chat.Id,
                                                text: $"Список слов в черном списке: {wordsInfo}");
                                        }

                                        else if (input.StartsWith(wordDeleteCommand))
                                        {
                                            string[] words = input.Substring(wordDeleteCommand.Length).Trim().Split(',');

                                            if (string.IsNullOrEmpty(words[0]))
                                            {
                                                await botClient.SendTextMessageAsync(
                                                    message.Chat.Id,
                                                    text: $"Возможно вводимые данные были некорректны. Пожалуйста, вводите данные по следующему формату - /deleteword Word1, Word2, Word3 ");
                                                break;
                                            }

                                            foreach (string word in words)
                                            {
                                                var blackListWord = await db._BlacklistOfWords.FirstOrDefaultAsync(x => x.WordsName == word.Trim());
                                                if (blackListWord != null)
                                                {
                                                    db.Remove(blackListWord);
                                                }
                                            }

                                            await db.SaveChangesAsync();
                                            await botClient.SendTextMessageAsync(
                                                message.Chat.Id,
                                                text: $"Следующие слова '{string.Join(", ", words)}' были успешно удалены из черного списка!");
                                        }

                                        else if (input.StartsWith(helpCommand))
                                        {
                                            var commandInfo = new CommandsName();
                                            var allComand = await commandInfo.GetAllCommands(db);

                                            string comandInfo = string.Join(", ", allComand.Select(x => x.CommandName));
                                            await botClient.SendTextMessageAsync(
                                                message.Chat.Id,
                                                text: $"Список всех доступных команд: {comandInfo}");
                                        }

                                        else
                                        {
                                            await botClient.SendTextMessageAsync(
                                                message.Chat.Id,
                                                text: $"Неизвестная ошибка или вводимые данные были некорректны.");
                                        }

                                        break;
                                    }
                                }
                            }
                        }

                        if (!isAdmin)
                        {
                            await botClient.SendTextMessageAsync(
                                message.Chat.Id,
                                replyToMessageId: message.MessageId,
                                text: "Отказ в доступе. Данную команду имеют право вводить только Администраторы.");
                            break;
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