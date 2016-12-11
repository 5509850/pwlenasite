using Telegram.Bot;

namespace lenapw.test.Models
{
    static public class BotPWserver
    {
        public static readonly TelegramBotClient Api = new TelegramBotClient("<Token bot1>");
    }

    static public class BotCreditme
    {
        public static readonly TelegramBotClient Api = new TelegramBotClient("<Token bot 2>");
    }
}