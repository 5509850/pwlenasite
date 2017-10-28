using System.Globalization;

namespace pw.lena.CrossCuttingConcerns.Interfaces
{
    public interface ILocalizeService
    {
        void LoadLocalization(string language = null);

        string Localize(string key);
              
    }
}
