﻿using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputMessageContents;
using Telegram.Bot.Types.ReplyMarkups;

namespace pwserver_bot
{
    //read me https://habrahabr.ru/post/316222/
    class Program
    {
        private static string token = "<Token Bot>";
        private static readonly TelegramBotClient bot = new TelegramBotClient(token);
        static void Main(string[] args)
        {
            bot.OnMessage += Bot_OnMessage;
            bot.OnInlineQuery += Bot_OnInlineQuery;
            bot.OnInlineResultChosen += Bot_OnInlineResultChosen;
            bot.SetWebhookAsync();

            var me = bot.GetMeAsync().Result;
            Console.Title = me.Username;

            bot.StartReceiving();
            Console.ReadLine();
            bot.StopReceiving();
        }

        private static void Bot_OnInlineResultChosen(object sender, Telegram.Bot.Args.ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            Console.WriteLine($"Received choosen inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
        }

        private static async void Bot_OnInlineQuery(object sender, Telegram.Bot.Args.InlineQueryEventArgs inlineQueryEventArgs)
        {
            InlineQueryResult[] results = {
                new InlineQueryResultLocation
                {
                    Id = "1",
                    Latitude = 40.7058316f, // displayed result
                    Longitude = -74.2581888f,
                    Title = "New York",
                    InputMessageContent = new InputLocationMessageContent // message if result is selected
                    {
                        Latitude = 40.7058316f,
                        Longitude = -74.2581888f,
                    }
                },

                new InlineQueryResultLocation
                {
                    Id = "2",
                    Longitude = 52.507629f, // displayed result
                    Latitude = 13.1449577f,
                    Title = "Berlin",
                    InputMessageContent = new InputLocationMessageContent // message if result is selected
                    {
                        Longitude = 52.507629f,
                        Latitude = 13.1449577f
                    }
                }
            };
            try
            {
                await bot.AnswerInlineQueryAsync(inlineQueryEventArgs.InlineQuery.Id, results, isPersonal: true, cacheTime: 0);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
            }
        }

        private static async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            Message msg = e.Message;

            if (msg == null || msg.Text == null) return;
            long chatid = msg.Chat.Id;
            // Если сообщение текстовое
            if (msg.Type == MessageType.TextMessage)
            {
                if (msg.Text.StartsWith("/inline")) // send inline keyboard
                {
                    await bot.SendChatActionAsync(msg.Chat.Id, ChatAction.Typing);

                    var keyboard = new InlineKeyboardMarkup(new[]
                    {
                    new[] // first row
                    {
                        new InlineKeyboardButton("1.1"),
                        new InlineKeyboardButton("1.2"),
                    },
                    new[] // second row
                    {
                        new InlineKeyboardButton("2.1"),
                        new InlineKeyboardButton("2.2"),
                    }
                });

                    await Task.Delay(500); // simulate longer running task

                    await bot.SendTextMessageAsync(msg.Chat.Id, "Choose",
                        replyMarkup: keyboard);
                    return;
                }

                switch (msg.Text)
                {
                    case "/start":
                        {
                            SendMessage(msg, "Welcome to Loan Calculator");
                            break;
                        }                   
                    case "/example":
                        {
                            SendMessage(chatid, @"1000 3 5");
                            break;
                        }
                    case "/stop":
                        {
                            break;
                        }

                    default:
                        {
                            string[] array = msg.Text.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                            if ((array != null && array.Length == 3 || array.Length == 4) && IsArrayDigit(array))
                            {
                                long a = 0;
                                long.TryParse(Regex.Replace(array[0], "[^0-9]", ""), out a);                               

                                double y = 0;
                                Match m = Regex.Match(array[1], @"[0-9]+(\.[0-9]+)?");
                                y = Convert.ToDouble(m.Value, CultureInfo.InvariantCulture);

                                double p = 0;
                                m = Regex.Match(array[2], @"[0-9]+(\.[0-9]+)?");
                                p = Convert.ToDouble(m.Value, CultureInfo.InvariantCulture);

                                double c = 0;
                                if (array.Length == 4)
                                {
                                    m = Regex.Match(array[3], @"[0-9]+(\.[0-9]+)?");
                                    c = Convert.ToDouble(m.Value, CultureInfo.InvariantCulture);
                                }
                                if (c != 0)
                                {
                                    SendMessage(chatid, string.Format(@"${0} Loan, {1} years term, {2}% - annual interest, ${3} - monthly Maintenance Fee", a, y, p, c));
                                }
                                else
                                {
                                    SendMessage(chatid, string.Format(@"${0} Loan, {1} years term, {2}% - annual interest", a, y, p));
                                }

                            }
                            else
                            {
                                SendMessage(chatid, @"example of use case: 
1000 3 5
<Loan Amount - $> 
<Loan term - years> 
<Annual Interest Rate - %> 
<Monthly Maintenance Fee - $> (optional)");
                            }
                            break;
                        }
                }

                /*
example - Example of input data for Loan Calculator 
loan - Loan Amount
interest - Interest Rate
term - Number of Years Loan Term
fee - (optional) Monthly Maintenance Fee
calculate - Calculate of Monthly Payments
clear - Reset Input Data
help - Displays a Help



                 */
                //for easy calculate of Monthly Payments
                // await bot.SendTextMessageAsync(msg.Chat.Id, "Hello, " + msg.From.FirstName + msg.Text + " msgID" + msg.MessageId);
            }


        }


        private static async void SendMessage(Message incomingMessage, string messageForSend)
        {
            try
            {
                if (incomingMessage.From.FirstName != null)
                {
                    await bot.SendTextMessageAsync(incomingMessage.Chat.Id, string.Format("{0}, {1}", messageForSend, incomingMessage.From.FirstName));
                }
                else
                {
                    await bot.SendTextMessageAsync(incomingMessage.Chat.Id, messageForSend);
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
            }
        }

        private static async void SendMessage(long messId, string messageForSend)
        {
            try
            {
                await bot.SendTextMessageAsync(messId, messageForSend);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
            }                
        }

        private static bool IsArrayDigit(string[] array)
        {
            bool result = true;
            if (array == null || array.Length == 0)
            {
                return false;
            }

            Match m;           
            foreach (var vol in array)
            {
                m = Regex.Match(vol, @"[0-9]+(\.[0-9]+)?");
                if (string.IsNullOrEmpty(m.Value))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

    }
}
