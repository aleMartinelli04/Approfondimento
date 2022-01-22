using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Wrapper.Bot;

public static class Keyboard
{
    private static readonly Dictionary<string, List<InlineKeyboardButton>?> SavedKeyboards = new();
    
    public static List<InlineKeyboardButton>? GetFirstLineKeyboard(Message message)
    {
        var key = $"{message.Chat.Id}_{message.MessageId}";

        return SavedKeyboards.ContainsKey(key) ? SavedKeyboards[key] : null;
    }

    public static void Save(List<InlineKeyboardButton>? keyboard, Message message)
    {
        SavedKeyboards[$"{message.Chat.Id}_{message.MessageId}"] = keyboard;
    }
}