using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Http;
using System.Threading.Tasks;

using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Text.RegularExpressions;
using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputMessageContents;
using pw.lena.Core.Data.Models;
using System.Configuration;

namespace lenapw.test.Controllers
{
    public class WebHookCreditController : ApiController
    {
        #region var 

        private string WEB_HOOK_URL = ConfigurationManager.AppSettings["webhook_urlcredit"];
        static class Bot
        {
            public static readonly TelegramBotClient Api = new TelegramBotClient(ConfigurationManager.AppSettings["token_webhookcredit"]);
        }


        private readonly string example = @"example of use case: 
1000 20 5
-------------------------
<Loan Amount - $> 
<Loan Term - years> 
<Annual Interest Rate - %> 
<Monthly Maintenance Fee - $> (optional)";

        private readonly string exampleInline = @"example of use case: 
@LoanCalculatorBot 1000 20 5
-------------------------
<Loan Amount - $> 
<Loan Term - years> 
<Annual Interest Rate - %> 
<Monthly Maintenance Fee - $> (optional)";


        private readonly string exampleShort = @"example of use case: 
1000 20 5";
        private readonly string exampleShort2 = @"<Loan Amount><Loan term in years><Annual Interest Rate %><Monthly Maintenance Fee $> (optional)";
        #endregion

        #region public methode
        public ICollection<Client> Get()
        {
            return new Collection<Client>
                       {
                           new Client { Id = 21, Title = "You're winner!"}
                       };
        }

        public ICollection<Client> Get(int id)
        {
            if (id == 555)
            {
                Bot.Api.SetWebhookAsync(WEB_HOOK_URL).Wait();
                return new Collection<Client>
                       {
                            {
                                new Client { Id = id, Title = "webhook set up succesful" }
                            }
                       };
            }
            else
                return new Collection<Client>
                       {
                            {
                                new Client { Id = id, Title = "I win again! Just like always!" }
                            }
                       };
        }

        public async Task<IHttpActionResult> Post(Update update)
        {
            if (update == null)
            {
                return null;
            }
            var msg = update.Message;
            if (msg == null)
            {
                //--------------------------------------------------------------------------Inline mode
                if (update.Type == UpdateType.InlineQueryUpdate)
                {
                    InlineQueryResult[] results = {
                new InlineQueryResultArticle
                {
                    Id= "1",
                    Title = "1) Even Principal Payments",
                    InputMessageContent = new InputTextMessageContent {
                        DisableWebPagePreview = true,
                        MessageText = GetInlinePrincipal(update.InlineQuery.Query)
                    },
                    Description = GetDescriptionInputData(update.InlineQuery.Query)

                },
                 new InlineQueryResultArticle
                {
                    Id= "2",
                    Title = "2) Even Total Payments",
                    InputMessageContent = new InputTextMessageContent {
                        DisableWebPagePreview = true,
                        MessageText = GetInlineTotal(update.InlineQuery.Query)
                    },
                    Description = GetDescriptionInputData2(update.InlineQuery.Query)
                }
                 };
                    try
                    {
                        await Bot.Api.AnswerInlineQueryAsync(update.InlineQuery.Id, results, isPersonal: true, cacheTime: 0);
                    }
                    catch (Exception ex)
                    {
                        var err = ex.Message;
                    }
                }
            }
            if (msg == null || msg.Text == null) return Ok();
            long chatid = msg.Chat.Id;
            //Message recive
            if (msg.Type == MessageType.TextMessage)
            {
                switch (msg.Text)
                {
                    case "/start":
                        {
                            SendMessage(msg, "Welcome to Loan Calculator");
                            break;
                        }

                    case "/rate":
                        {
                            SendMessage(msg, "https://telegram.me/storebot?start=loancalculatorbot");
                            break;
                        }


                    case "/example":
                        {
                            SendMessage(chatid, @"10000 20 7
.........(you)");
                            await Task.Delay(500); // simulate longer running task                            
                            SendMessage(chatid, string.Format(@"{0} Loan, {1} Years Term, {2}% Annual Interest
{3}", 10000.ToString("C2"), 20, 7,
GetResultLoanEvenPrincipalPayments(10000, 20, 7, 0, true)));
                            await Task.Delay(500); // simulate longer running task                            
                            SendMessage(chatid, string.Format(@"{0} Loan, {1} Years Term, {2}% Annual Interest
{3}", 10000.ToString("C2"), 20, 7,
GetResultLoanEvenTotalPayments(10000, 20, 7, 0, true)));
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
                                int a = 0;
                                int.TryParse(Regex.Replace(array[0], "[^0-9]", ""), out a);
                                a = Math.Abs(a);

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
                                if (a == 0 || y == 0 || p == 0)
                                {
                                    SendMessage(chatid, example);
                                    break;
                                }
                                if (c != 0)
                                {
                                    SendMessage(chatid, string.Format(@"{0} Loan, {1} years term, {2}% - annual interest, {3} - monthly Maintenance Fee", a.ToString("C2"), y, p, c.ToString("C2")));
                                }
                                else
                                {
                                    SendMessage(chatid, string.Format(@"{0} Loan, {1} years term, {2}% - annual interest", a.ToString("C2"), y, p));
                                }
                                await Task.Delay(1000); // simulate longer running task                            
                                SendMessage(chatid, string.Format(@"{0}
**************************
{1}", GetResultLoanEvenPrincipalPayments(a, y, p, c, false), GetResultLoanEvenTotalPayments(a, y, p, c, false)));

                            }
                            else
                            {
                                SendMessage(chatid, example);
                            }
                            break;
                        }
                }
            }
            return Ok();
        }

        #endregion

        #region private methodes
        private async void SendMessage(Message incomingMessage, string messageForSend)
        {
            try
            {
                if (incomingMessage.From.FirstName != null)
                {
                    Message x = await Bot.Api.SendTextMessageAsync(incomingMessage.Chat.Id, string.Format("{0}, {1} - click /example", messageForSend, incomingMessage.From.FirstName));
                }
                else
                {
                    Message x = await Bot.Api.SendTextMessageAsync(incomingMessage.Chat.Id, messageForSend + " - click /example");
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
            }
        }

        private async void SendMessage(long messId, string messageForSend)
        {
            try
            {
                Message x = await Bot.Api.SendTextMessageAsync(messId, messageForSend);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
            }
        }

        private bool IsArrayDigit(string[] array)
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

        private string GetResultLoanEvenPrincipalPayments(int loan, double year, double interestpercent, double commis, bool demo)
        {
            var interest = interestpercent / 100;
            double percTotal = 0;
            double monthlyTotal = 0;

            double amountRest = loan; //остаток основного долга            
            var mainloan = (loan / (year * 12)); //fixed
            var percent = (amountRest * interest / 12);
            var monthly = mainloan + percent + commis;
            string monthlyFirstText = monthly.ToString("C2");
            double lastmonth = 0;

            for (int i = 1; i < (year * 12) + 1; i++)
            {
                amountRest -= mainloan;
                percTotal += percent + commis;
                monthlyTotal += monthly;
                percent = (loan - (i * (loan / (year * 12)))) * (interest / 12);
                lastmonth = monthly;
                monthly = mainloan + percent + commis;
            }

            double principalTotal = (loan / monthlyTotal) * 100;
            double InterestTotal = (percTotal / monthlyTotal) * 100;

            string monthlyTotalText = monthlyTotal.ToString("C2");
            string percTotallText = percTotal.ToString("C2");
            string monthlyLastText = lastmonth.ToString("C2");

            return string.Format(@"case 1: Even Principal Payments
Results:
Pay in the first month  {0}
Pay in the last month  {1}
Total interest	       {4}
Total of {2} payments  {3}
---------------------------
Principal              {5}%
Interest               {6}%
{7}
",
monthlyFirstText,
monthlyLastText,
year * 12,
monthlyTotalText,
percTotallText,
principalTotal.ToString("F1"),
InterestTotal.ToString("F1"),
geturl("b", demo, (int)(percTotal - (commis * year * 12)), loan, (int)(commis * year * 12), loan, (int)year, interestpercent, commis));
        }

        private string geturl(string type, bool demo, int per, int pri, int fee, int a, int y, double i, double f)
        {
            if (demo)
            {
                if (type.Equals("a"))
                {
                    return "http://lena.pw/img/f2.png";
                }
                else
                {
                    return "http://lena.pw/img/f1.png";
                }
            }
            return string.Format(@"
Chart 1:
http://lena.pw/chart.html?a={0}&y={1}&i={2}&f={3}&t={4}
Chart 2:
http://lena.pw/diagram.html?per={5}&pri={6}&fee={7}", a, y, i, f, type,
per, pri, fee);

        }


        private string GetResultLoanEvenTotalPayments(int loan, double year, double interestpercent, double commis, bool demo)
        {
            var interest = interestpercent / 100;
            var monthly = (loan * (interest / 12)) / (1 - (1 / Math.Pow(1 + (interest / 12), (year * 12)))) + commis; // fixed
            string monthlyText = monthly.ToString("C2");

            double principalTotal = (loan / (monthly * year * 12)) * 100;
            double InterestTotal = (((monthly * year * 12) - loan) / (monthly * year * 12)) * 100;

            string totalText = (monthly * year * 12).ToString("C2");
            string percentText = ((monthly * year * 12) - loan).ToString("C2");
            return string.Format(@"case 2: Even Total Payments
Results:
Payment every month	    {0}
Total interest          {3}
Total of {1} payments   {2}
---------------------------
Principal              {4}%
Interest               {5}%
{6}",
monthlyText,
year * 12,
totalText,
percentText,
principalTotal.ToString("F1"),
InterestTotal.ToString("F1"),
geturl("a", demo, (int)(((monthly * year * 12) - loan - (commis * year * 12))), loan, (int)(commis * year * 12), loan, (int)year, interestpercent, commis)); //
        }

        private string GetInlineTotal(string msg)
        {
            try
            {
                if (string.IsNullOrEmpty(msg))
                {
                    return exampleInline;

                }
                string[] array = msg.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if ((array != null && array.Length == 3 || array.Length == 4) && IsArrayDigit(array))
                {
                    int a = 0;
                    int.TryParse(Regex.Replace(array[0], "[^0-9]", ""), out a);
                    a = Math.Abs(a);

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
                    if (a == 0 || y == 0 || p == 0)
                    {
                        return exampleInline;
                    }
                    if (c != 0)
                    {
                        return string.Format(@"{0} Loan, {1} years term, {2}% - annual interest, {3} - monthly Maintenance Fee
{4}"
    , a.ToString("C2"), y, p, c.ToString("C2"), GetResultLoanEvenTotalPayments(a, y, p, c, false));
                    }
                    else
                    {
                        return string.Format(@"{0} Loan, {1} years term, {2}% - annual interest
{3}", a.ToString("C2"), y, p, GetResultLoanEvenTotalPayments(a, y, p, c, false));
                    }
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                return exampleInline;
            }

            return exampleInline;
        }

        private string GetDescriptionInputData2(string msg)
        {
            try
            {
                if (string.IsNullOrEmpty(msg))
                {
                    return exampleShort2;
                }
                string[] array = msg.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (array != null && array.Length == 1 && IsArrayDigit(array))
                {
                    int a = 0;
                    int.TryParse(Regex.Replace(array[0], "[^0-9]", ""), out a);
                    a = Math.Abs(a);

                    if (a == 0)
                    {
                        return example;
                    }
                    return string.Format(@"{0} Loan, ... years term",
                            a.ToString("C2"));
                }

                if (array != null && array.Length == 2 && IsArrayDigit(array))
                {
                    int a = 0;
                    int.TryParse(Regex.Replace(array[0], "[^0-9]", ""), out a);
                    a = Math.Abs(a);

                    double y = 0;
                    Match m = Regex.Match(array[1], @"[0-9]+(\.[0-9]+)?");
                    y = Convert.ToDouble(m.Value, CultureInfo.InvariantCulture);

                    if (a == 0 || y == 0)
                    {
                        return example;
                    }
                    return string.Format(@"{0} Loan, {1} years term, .... - annual interest",
                            a.ToString("C2"), y);
                }

                if ((array != null && array.Length == 3 || array.Length == 4) && IsArrayDigit(array))
                {
                    int a = 0;
                    int.TryParse(Regex.Replace(array[0], "[^0-9]", ""), out a);
                    a = Math.Abs(a);

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
                    if (a == 0 || y == 0 || p == 0)
                    {
                        return example;
                    }
                    if (c != 0)
                    {
                        return string.Format(@"{0} Loan, {1} years term, {2}% - annual interest, {3} - monthly Maintenance Fee",
                            a.ToString("C2"), y, p, c.ToString("C2"));
                    }
                    else
                    {
                        return string.Format(@"{0} Loan, {1} years term, {2}% - annual interest",
                            a.ToString("C2"), y, p);
                    }
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                return exampleShort2;
            }
            return exampleShort2;
        }

        private string GetDescriptionInputData(string msg)
        {
            try
            {
                if (string.IsNullOrEmpty(msg))
                {
                    return exampleShort;
                }
                string[] array = msg.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if ((array != null && array.Length == 3 || array.Length == 4) && IsArrayDigit(array))
                {
                    int a = 0;
                    int.TryParse(Regex.Replace(array[0], "[^0-9]", ""), out a);
                    a = Math.Abs(a);

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
                    if (a == 0 || y == 0 || p == 0)
                    {
                        return example;
                    }
                    if (c != 0)
                    {
                        return string.Format(@"{0} Loan, {1} years term, {2}% - annual interest, {3} - monthly Maintenance Fee",
                            a.ToString("C2"), y, p, c.ToString("C2"));
                    }
                    else
                    {
                        return string.Format(@"{0} Loan, {1} years term, {2}% - annual interest",
                            a.ToString("C2"), y, p);
                    }
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                return exampleShort;
            }
            return exampleShort;
        }

        private string GetInlinePrincipal(string msg)
        {
            try
            {
                if (string.IsNullOrEmpty(msg))
                {
                    return exampleInline;
                }
                string[] array = msg.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if ((array != null && array.Length == 3 || array.Length == 4) && IsArrayDigit(array))
                {
                    int a = 0;
                    int.TryParse(Regex.Replace(array[0], "[^0-9]", ""), out a);
                    a = Math.Abs(a);

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
                    if (a == 0 || y == 0 || p == 0)
                    {
                        return exampleInline;
                    }
                    if (c != 0)
                    {
                        return string.Format(@"{0} Loan, {1} years term, {2}% - annual interest, {3} - monthly Maintenance Fee
{4}"
    , a.ToString("C2"), y, p, c.ToString("C2"), GetResultLoanEvenPrincipalPayments(a, y, p, c, false));
                    }
                    else
                    {
                        return string.Format(@"{0} Loan, {1} years term, {2}% - annual interest
{3}", a.ToString("C2"), y, p, GetResultLoanEvenPrincipalPayments(a, y, p, c, false));
                    }
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                return exampleInline;
            }
            return exampleInline;
        }


        #endregion
    }
}