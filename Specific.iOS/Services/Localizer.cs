using System;
using Foundation;
using PlatformAbstractions.Interfaces;
using System.Globalization;

namespace Specific.iOS.Services
{
    public class Localizer : ILocalizer
    {
        public string GetCountry()
        {
            return "US";
        }

        public string GetCurrentCultureInfo()
        {
            var lang = NSLocale.PreferredLanguages[0].Split('_', '1', '-')[0];
            return lang;
        }

        public string GetLanguage()
        {
            return GetCurrentCultureInfo();
        }

        public CultureInfo CurrentCulture()
        {
            return new CultureInfo($"{GetLanguage()}-{GetCountry()}");
        }
    }
}
