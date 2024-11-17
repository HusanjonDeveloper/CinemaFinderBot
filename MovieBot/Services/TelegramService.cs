using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace MovieBot.Services;

public class TelegramService
{
    private readonly TelegramBotClient _botClient;
    private readonly MovieService _movieService;

    public TelegramService()
    {
        var botToken = File.ReadAllText("AppSettings.json"); // JSON o‘qiladi
        _botClient = new TelegramBotClient(botToken);
        _movieService = new MovieService();
    }

    public async Task StartBot()
    {
        // Xabarlarni qabul qilish va ishlov berish
        _botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync
        );

        var botInfo = await _botClient.GetMeAsync();
        Console.WriteLine($"Bot {botInfo.Username} ishga tushdi!");
        Console.ReadLine();
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message?.Text != null)
        {
            var chatId = update.Message.Chat.Id;
            var userMessage = update.Message.Text;

            if (userMessage.ToLower() == "/start")
            {
                await _botClient.SendTextMessageAsync(chatId, "Salom! Kinoni qidirish uchun nomini kiriting:");
            }
            else
            {
                var movieInfo = await _movieService.SearchMovie(userMessage);
                await _botClient.SendTextMessageAsync(chatId, movieInfo);
            }
        }
    }

    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Xatolik yuz berdi: {exception.Message}");
        return Task.CompletedTask;
    }
}