using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Wrapper.wrapper.Classes;

namespace Wrapper.Bot;

public partial class Handlers
{
    private static async Task<Message> SendShow(ITelegramBotClient client, CallbackQuery callback)
    {
        var showCode = callback.Data!.Split("_")[1];

        var show = Accounts.Imdb.TitlesGetDetails(showCode);

        return await SendShow(client, callback.Message!, show, showCode);
    }

    private static async Task<Message> SendShow(ITelegramBotClient client, Message message, Title show, string showCode)
    {
        List<InlineKeyboardButton>? firstLineKeyboard = new()
        {
            InlineKeyboardButton.WithCallbackData("Crazy comments", $"crazycomments_{showCode}"),
            InlineKeyboardButton.WithCallbackData("Top Cast", $"topcast_{showCode}")
        };
        
        var keyboard = new List<List<InlineKeyboardButton>?>
        {
            firstLineKeyboard,
            new()
            {
                InlineKeyboardButton.WithUrl("Open On imdb.com", $"https://www.imdb.com/title/{showCode}"),
                InlineKeyboardButton.WithCallbackData("Manda Link", $"sendlink_{showCode}")
            }
        };
        
        var sentMessage = await client.SendPhotoAsync(message.Chat.Id,
            show.ImageUrl,
            $"<b>{show.Name}</b> ({show.Year})" + (show.Episodes == 0 ? "" : $"\n{show.Episodes} episodes"),
            ParseMode.Html, 
            replyMarkup: new InlineKeyboardMarkup(keyboard));
        
        Keyboard.Save(firstLineKeyboard, sentMessage);

        return sentMessage;
    }

    private static async Task<Message> SendActor(ITelegramBotClient client, Message message, Actor actor)
    {
        var firstLineKeyboard = new List<InlineKeyboardButton>
        {
            InlineKeyboardButton.WithCallbackData("Biografia", $"biography_{actor.NConst}"),
            InlineKeyboardButton.WithCallbackData("Ruoli importanti", $"knownfor_{actor.NConst}")
        };
        
        var keyboard = new List<List<InlineKeyboardButton>?>
        {
            firstLineKeyboard,
            new()
            {
                InlineKeyboardButton.WithUrl("Apri su Imdb.com", $"https://imdb.com/{actor.NConst}"),
                InlineKeyboardButton.WithCallbackData("Manda Link", $"sendlink_{actor.NConst}")
            }
        };

        var sentMessage = await client.SendPhotoAsync(message.Chat.Id,
            actor.ImageUrl,
            $"<b>{actor.Name}</b>",
            ParseMode.Html,
            replyMarkup: new InlineKeyboardMarkup(keyboard));
        
        Keyboard.Save(firstLineKeyboard, sentMessage);

        return sentMessage;
    }
    
    private static async Task<Message> SendActor(ITelegramBotClient client, CallbackQuery callback)
    {
        var actorCode = callback.Data!.Split("_")[1];

        var actor = Accounts.Imdb.GetActorBio(actorCode);

        return await SendActor(client, callback.Message!, actor);
    }
}