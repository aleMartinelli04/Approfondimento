using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Wrapper.wrapper.Classes;
using Wrapper.wrapper.Exceptions;

namespace Wrapper.Bot;

public static partial class Handlers
{
    private static async Task<Message> Films(ITelegramBotClient client, Message message, string? query = null)
    {
        query ??= string.Join(' ', message.Text?.Split(" ").Skip(1).ToArray() ?? Array.Empty<string>());

        if (query == string.Empty)
        {
            return await client.SendTextMessageAsync(message.Chat.Id, "Specifica un nome da cercare");
        }

        List<ShowBasic> shows;
        try
        {
            shows = Accounts.Imdb.GetShows(query).ToList();
        }
        catch (RequestFailedException e)
        {
            return await client.SendTextMessageAsync(message.Chat.Id, $"<code>{e.Message}</code>", 
                ParseMode.Html);
        }
        
        if (shows.Count == 0)
        {
            return await client.SendTextMessageAsync(message.Chat.Id, "Nessun film trovato");
        }

        var buttons = new List<List<InlineKeyboardButton>>();
        var limit = shows.Count % 2 == 0 ? shows.Count : shows.Count - 1;

        for (var i = 0; i < limit; i += 2)
        {
            buttons.Add(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(shows[i].Title, $"openshow_{shows[i].TConst}"),
                InlineKeyboardButton.WithCallbackData(shows[i+1].Title, $"openshow_{shows[i+1].TConst}")
            });
        }

        if (shows.Count % 2 == 1)
        {
            buttons.Add(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(shows[^1].Title, $"openshow_{shows[^1].TConst}")
            });
        }
        
        var replyMarkup = new InlineKeyboardMarkup(buttons);

        return await client.SendTextMessageAsync(message.Chat.Id,
            "Risultati:",
            replyMarkup: replyMarkup);
    }

    private static async Task<Message> Actors(ITelegramBotClient client, Message message, string? query = null)
    {
        query ??= string.Join(' ', message.Text?.Split(" ").Skip(1).ToArray() ?? Array.Empty<string>());
        
        if (query == string.Empty)
        {
            return await client.SendTextMessageAsync(message.Chat.Id, "Specifica un nome da cercare");
        }

        List<ActorBasic> actors;

        try
        {
            actors = Accounts.Imdb.GetActors(query).ToList();
        }
        catch (RequestFailedException e)
        {
            return await client.SendTextMessageAsync(message.Chat.Id, $"<code>{e.Message}</code>", 
                ParseMode.Html);
        }
        
        if (actors.Count == 0)
        {
            return await client.SendTextMessageAsync(message.Chat.Id, "Nessun attore trovato");
        }
        
        var buttons = new List<List<InlineKeyboardButton>>();
        var limit = actors.Count % 2 == 0 ? actors.Count : actors.Count - 1;

        for (var i = 0; i < limit; i += 2)
        {
            buttons.Add(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(actors[i].Name, $"sendactor_{actors[i].NConst}"),
                InlineKeyboardButton.WithCallbackData(actors[i+1].Name, $"sendactor_{actors[i+1].NConst}")
            });
        }
        
        var replyMarkup = new InlineKeyboardMarkup(buttons);

        return await client.SendTextMessageAsync(message.Chat.Id,
            "Risultati:",
            replyMarkup: replyMarkup);
    }

    private static async Task<Message> Usage(ITelegramBotClient client, Message message)
    {
        const string text = "Comandi disponibili:\n\n" +
                            "» film [nome film] - ricerca film\n" +
                            "» actors [nome attore] - ricerca attore\n" +
                            "» usage - mostra questo messaggio";

        return await client.SendTextMessageAsync(message.Chat.Id, text);
    }
}