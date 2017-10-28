using PlatformAbstractions.Interfaces;
using System.Globalization;
using System.Threading;


namespace pw.lena.slave.winpc.Services
{
    public class Localizer : ILocalizer
    {
        public string GetCurrentCultureInfo()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentUICulture;
            return currentCulture.TwoLetterISOLanguageName;
        }

        public CultureInfo CurrentCulture()
        {
            return new CultureInfo($"{GetLanguage()}-{GetCountry()}");
        }

        public string GetCountry()
        {
            return "US";
        }

        public string GetLanguage()
        {
            return GetCurrentCultureInfo();
        }
    }
}