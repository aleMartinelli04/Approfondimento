using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegraph.Net.Models;

namespace Wrapper.Bot;

public static partial class Handlers
{
    private static async Task<Message> NoCallbacksFound(ITelegramBotClient client, CallbackQuery callback)
    {
        return await client.SendTextMessageAsync(callback.Message!.Chat.Id,
            "Qualcosa è andato storto...");
    }
    
    private static async Task<Message> CrazyComments(ITelegramBotClient client, CallbackQuery callback)
    {
        var showCode = callback.Data!.Split("_")[1];

        var crazyComments = Accounts.Imdb.GetCrazyCommentsFor(showCode);

        var content = crazyComments
            .Select(crazyComment =>
                new NodeElement("p", null, crazyComment, new NodeElement("p", null)))
            .ToArray();

        var page = Accounts.Telegraph.CreatePageAsync("Crazy comments", content).Result;

        var firstLineKeyboard = Keyboard.GetFirstLineKeyboard(callback.Message!);
        firstLineKeyboard[0] = InlineKeyboardButton.WithUrl($"Crazy Comments", page.Url);
        Keyboard.Save(firstLineKeyboard, callback.Message!);
        
        var keyboard = new List<List<InlineKeyboardButton>>
        {
            firstLineKeyboard,
            new()
            {
                InlineKeyboardButton.WithUrl("Open On imdb.com", $"https://www.imdb.com/title/{showCode}")
            }
        };
        
        return await client.EditMessageReplyMarkupAsync(callback.Message!.Chat!, callback.Message!.MessageId,
            new InlineKeyboardMarkup(keyboard));
    }

    private static async Task<Message> TopCast(ITelegramBotClient client, CallbackQuery callback)
    {
        var showCode = callback.Data!.Split("_")[1];

        var topCast = Accounts.Imdb.GetTopCast(showCode);
        var actors = topCast
            .Select(nconst => Accounts.Imdb.GetActorBio(nconst))
            .ToArray();

        List<NodeElement> content = new();
        for (var i = 0; i < actors.Length; i++)
        {
            var actor = actors[i];
            content.Add(new NodeElement("a",
                    new Dictionary<string, string>
                    {{"href", $"https://t.me/movie_scraper_bot?start=id_{actor.NConst}"}}, 
                    new NodeElement("h3", null, actor.Name)));
            
            content.Add(new NodeElement("img", new Dictionary<string, string> {{"src", actor.ImageUrl}}));


            if (i != actors.Length - 1)
            {
                content.Add(new NodeElement("hr", null));
            }
        }

        var page = Accounts.Telegraph.CreatePageAsync("Top cast", content.ToArray()).Result;
        
        var firstLineKeyboard = Keyboard.GetFirstLineKeyboard(callback.Message!);
        firstLineKeyboard[1] = InlineKeyboardButton.WithUrl($"Top Cast", page.Url);
        Keyboard.Save(firstLineKeyboard, callback.Message!);
        
        var keyboard = new List<List<InlineKeyboardButton>>
        {
            firstLineKeyboard,
            new()
            {
                InlineKeyboardButton.WithUrl("Open On imdb.com", $"https://www.imdb.com/title/{showCode}")
            }
        };
        
        return await client.EditMessageReplyMarkupAsync(callback.Message!.Chat!, callback.Message!.MessageId,
            new InlineKeyboardMarkup(keyboard));
    }
    
    private static async Task<Message> Biography(ITelegramBotClient client, CallbackQuery callback)
    {
        var actorCode = callback.Data!.Split("_")[1];

        var actor = Accounts.Imdb.GetActorBio(actorCode);

        NodeElement[] content = {actor.Bio};
        var page = Accounts.Telegraph.CreatePageAsync($"{actor.Name} - biografia", content).Result;

        var firstLineKeyboard = Keyboard.GetFirstLineKeyboard(callback.Message!);
        firstLineKeyboard[0] = InlineKeyboardButton.WithUrl("Biografia", page.Url);
        Keyboard.Save(firstLineKeyboard, callback.Message!);
        
        var keyboard = new List<List<InlineKeyboardButton>>
        {
            firstLineKeyboard,
            new ()
            {
                InlineKeyboardButton.WithUrl("Apri su Imdb.com", $"https://imdb.com/{actor.NConst}")
            }
        };

        return await client.EditMessageReplyMarkupAsync(callback.Message!.Chat!, callback.Message.MessageId,
            new InlineKeyboardMarkup(keyboard));
    }

    private static async Task<Message> KnownFor(ITelegramBotClient client, CallbackQuery callback)
    {
        var actorCode = callback.Data!.Split("_")[1];

        var knownFor = Accounts.Imdb.GetKnownFor(actorCode).ToArray();

        var content = new List<NodeElement>();
        for (var i = 0; i < knownFor.Length; i++)
        {
            var show = knownFor[i];
            
            content.Add(new NodeElement("a", 
                new Dictionary<string, string>{{"href", $"https://t.me/movie_scraper_bot?start=id_{show.TConst}"}},
                new NodeElement("h3", null, $"{show.Title} ({show.ReleaseYear})")));
            content.Add(new NodeElement("p", null, 
                $"Rating: {show.Rating}/10\nPersonaggio interpretato: {show.Role}"));

            content.Add(new NodeElement("img", new Dictionary<string, string> {{"src", show.ImageUrl}}));

            if (i != knownFor.Length - 1)
            {
                content.Add(new NodeElement("hr", null));
            }
        }
        
        var page = Accounts.Telegraph.CreatePageAsync("Known For", content.ToArray()).Result;

        var firstLineKeyboard = Keyboard.GetFirstLineKeyboard(callback.Message!);
        firstLineKeyboard[1] = InlineKeyboardButton.WithUrl("Apri ruoli importanti", page.Url);
        Keyboard.Save(firstLineKeyboard, callback.Message!);
        
        var keyboard = new List<List<InlineKeyboardButton>>
        {
            firstLineKeyboard,
            new()
            {
                InlineKeyboardButton.WithUrl("Apri su Imdb.com", $"https://imdb.com/{actorCode}")
            }
        };
        return await client.EditMessageReplyMarkupAsync(callback.Message!.Chat!, callback.Message!.MessageId,
            new InlineKeyboardMarkup(keyboard));
    }
}