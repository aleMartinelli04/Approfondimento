using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

namespace Wrapper.Bot;

public static class Bot
{
    private static TelegramBotClient? _bot;
    
    public static Task Main()
    {
        _bot = new TelegramBotClient(Config.BotToken);

        using var cts = new CancellationTokenSource();

        ReceiverOptions receiverOptions = new() { AllowedUpdates = { } };
        _bot.StartReceiving(Handlers.HandleUpdateAsync,
            Handlers.HandleErrorAsync,
            receiverOptions,
            cts.Token);

        Console.ReadLine();

        cts.Cancel();
        return Task.CompletedTask;
    }
}