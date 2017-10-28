using System.Globalization;

namespace PlatformAbstractions.Interfaces
{
    public interface ILocalizer
    {
        string GetCurrentCultureInfo();
        string GetLanguage();
        string GetCountry();

        CultureInfo CurrentCulture();

    }
}
