using PlatformAbstractions.Interfaces;
using System;
using System.Globalization;

namespace Specific.Droid
{
    public class Localizer : ILocalizer
    {
        public CultureInfo CurrentCulture()
        {
            return new CultureInfo($"{GetLanguage()}-{GetCountry()}");
        }

        public string GetCountry()
        {
            return "US";
        }

        public string GetCurrentCultureInfo()
        {
            var androidLocale = Java.Util.Locale.Default;
            string netLanguage = string.Empty;
            try
            {
                netLanguage = androidLocale.ToString().Split('_', '1')[0];
            }
            catch (Exception)
            {
                netLanguage = "en";
            }
            return netLanguage;
        }

        public string GetLanguage()
        {
           return GetCurrentCultureInfo();
        }
    }
}