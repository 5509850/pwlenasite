using System;
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
        private static string token = "<Bot Token>";
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
                            SendMessage(chatid, @"1000 3 5
Waiting...");
                            await Task.Delay(500); // simulate longer running task                            
                            SendMessage(chatid, string.Format(@"{0} Loan, {1} years term, {2}% - annual interest", 1000.ToString("C2"), 3, 5));
                            await Task.Delay(500); // simulate longer running task                            
                            SendMessage(chatid, GetResultLoanEvenTotalPayments(1000, 3, 5, 0));
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

        private static string GetResultLoanEvenTotalPayments(int loan, double year, double interestpercent, double commis)
        {
           var interest = interestpercent / 100;           
           var monthly = (loan * (interest / 12)) / (1 - (1 / Math.Pow(1 + (interest / 12), (year * 12)))) + commis; // fixed
           string monthlyText = monthly.ToString("C2");
            

           string totalText = (monthly * year * 12).ToString("C2");
           string percentText = ((monthly * year * 12) - loan).ToString("C2");
           return string.Format(@"Results:
Payment Every Month	    {0}
Total of {1} Payments	{2}
Total Interest	  {3}", monthlyText, year * 12, totalText, percentText);
        }

        private static string GetResultLoanEvenPrincipalPayments(int loan, double year, double interestpercent, double commis)
        {
            var interest = interestpercent / 100;
            var percTotal = 0;
            var mainTotal = 0;
            var monthlyTotal = 0;         

            var amountRest = loan; //остаток основного долга            
            var mainloan = (loan / (year * 12)); //fixed
            var percent = (amountRest * interest / 12);
            var monthly = mainloan + percent + commis;
            string monthlyText = monthly.ToString("C2");

            for (int i = 1; i < (year * 12) +1; i++) {
               
                    
                    //amount: amountRest.toFixed(2).replace(/ (\d)(?= (\d{ 3})+\.)/ g, '$1,'),
                    perc: (percent + $scope.commis).toFixed(2).replace(/ (\d)(?= (\d{ 3})+\.)/ g, '$1,'),
                    main: mainloan.toFixed(2).replace(/ (\d)(?= (\d{ 3})+\.)/ g, '$1,'),
                    pay: monthly.toFixed(2).replace(/ (\d)(?= (\d{ 3})+\.)/ g, '$1,'),                   
                );

                amountRest -= mainloan;
                mainTotal += mainloan;
                percTotal += percent + $scope.commis;
                monthlyTotal += monthly;

                percent = parseFloat(($scope.amount - (i * ($scope.amount / ($scope.years * 12)))) *(interest / 12));
                monthly = mainloan + percent + $scope.commis;
            }
            //Итого:
            $scope.data.payments.push({
                month: "ИТОГО:",
                amount: "0",
                perc: percTotal.toFixed(2).replace(/ (\d)(?= (\d{ 3})+\.)/ g, '$1,'),
                main: mainTotal.toFixed(2).replace(/ (\d)(?= (\d{ 3})+\.)/ g, '$1,'),
                pay: monthlyTotal.toFixed(2).replace(/ (\d)(?= (\d{ 3})+\.)/ g, '$1,'),
                passed: true,
                style: 'danger',
                font: 'large'
            });
            $scope.total = monthlyTotal.toFixed(2).replace(/ (\d)(?= (\d{ 3})+\.)/ g, '$1,');
            $scope.percent = percTotal.toFixed(2).replace(/ (\d)(?= (\d{ 3})+\.)/ g, '$1,');

            return string.Format(@"Results:
Even Principal Payments
Payment First Month	    {0}
Payment Last Month	    {1}
Total of {2} Payments	{3}
Total Interest	  {3}", monthlyText, monthlyText, year * 12, totalText, percentText);
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
