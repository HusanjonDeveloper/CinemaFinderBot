using MovieBot.Services;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Telegram bot ishga tushmoqda...");

        // Botni ishga tushirishproejct n
        var telegramService = new TelegramService();
        await telegramService.StartBot();
    }
}