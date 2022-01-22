using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Wrapper.Bot;

public class Keyboard
{
    private static readonly Dictionary<string, List<InlineKeyboardButton>?> SavedKeyboards = new();
    
    public static List<InlineKeyboardButton> GetFirstLineKeyboard(Message message)
    {
        return SavedKeyboards[$"{message.Chat.Id}_{message.MessageId}"]!;
    }

    public static void Save(List<InlineKeyboardButton> keyboard, Message message)
    {
        SavedKeyboards[$"{message.Chat.Id}_{message.MessageId}"] = keyboard;
    }
}