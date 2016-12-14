using Telegram.Bot;

namespace lenapw.test.Models
{
    static public class BotPWserver
    {
        public static readonly TelegramBotClient Api = new TelegramBotClient("<Bot Token PW>");
    }

    static public class BotCreditme
    {
        public static readonly TelegramBotClient Api = new TelegramBotClient("<Bot Token Credit>");
    }
}