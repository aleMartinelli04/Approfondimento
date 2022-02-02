using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Wrapper.Bot;

public partial class Handlers
{
    public static async Task HandleUpdateAsync(ITelegramBotClient client, Update update)
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
            await HandleErrorAsync(client, exception);
        }
    }
    
    public static Task HandleErrorAsync(ITelegramBotClient client, Exception exception)
    {
        var errorMessage = exception switch
        {
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }
}