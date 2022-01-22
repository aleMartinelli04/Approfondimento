using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Wrapper.Bot;

public static partial class Handlers
{
    public static Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }

    public static async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        var handler = update.Type switch
        {
            UpdateType.Message            => BotOnMessageReceived(client, update.Message!),
            UpdateType.EditedMessage      => BotOnMessageReceived(client, update.EditedMessage!),
            UpdateType.CallbackQuery      => BotOnCallbackQueryReceived(client, update.CallbackQuery!),
            _                             => UnknownUpdateHandlerAsync(client, update)
        };

        try
        {
            await handler;
        }
        catch (Exception exception)
        {
            await HandleErrorAsync(client, exception, cancellationToken);
        }
    }

    private static async Task BotOnMessageReceived(ITelegramBotClient client, Message message)
    {
        if (message.Type != MessageType.Text)
            return;
        
        var action = message.Text!.Split(' ')[0] switch
        {
            "/start" => StartLink(client, message),
            "/film" => Films(client, message),
            "/actors" => Actors(client, message),
            "/usage" => Usage(client, message),
            _ => Usage(client, message)
        };
        await action;
    }

    private static async Task BotOnCallbackQueryReceived(ITelegramBotClient client, CallbackQuery callback)
    {
        var handler = callback.Data!.Split("_")[0] switch
        {
            "openshow" => SendShow(client, callback),
            "crazycomments" => CrazyComments(client, callback),
            "topcast" => TopCast(client, callback),
            "sendactor" => SendActor(client, callback),
            "biography" => Biography(client, callback),
            "knownfor" => KnownFor(client, callback),
            "sendlink" => SendLink(client, callback),
            _ => NoCallbacksFound(client, callback)
        };
        await handler;
    }
    
    private static Task UnknownUpdateHandlerAsync(ITelegramBotClient unused, Update update)
    {
        Console.WriteLine($"Unknown update type: {update.Type}");
        return Task.CompletedTask;
    }
}