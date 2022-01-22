using Telegram.Bot;
using Telegram.Bot.Types;
using Wrapper.wrapper.Classes;

namespace Wrapper.Bot;

public partial class Handlers
{
    private static async Task<Message> StartLink(ITelegramBotClient client, Message message)
    {
        var query = string.Join(' ', message.Text!.Split('_').Skip(1));

        var action = message.Text!.Split(' ')[1].Split('_')[0] switch
        {
            "actors" => Actors(client, message, query),
            "film" => Films(client, message, query),
            "id" => Id(client, message, query),
            _ => Usage(client, message)
        };
        return await action;
    }

    private static Task<Message> Id(ITelegramBotClient client, Message message, string id)
    {
        var returnedContent = Accounts.Imdb.GetById(id);

        if (id is null)
        {
            throw new Exception();
        }

        return returnedContent switch
        {
            Actor actor => SendActor(client, message, actor),
            Title title => SendShow(client, message, title, id),
            _ => throw new Exception()
        };
    }
}