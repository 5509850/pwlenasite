using pw.lena.Core.Data.Models.Enums;
using System.Threading.Tasks;

namespace pw.lena.Core.Data.Services.DataService.Contracts
{
    public interface IPreferenceService
    {
        Task<string> GetPrefValue(PrefEnums key);

        Task<bool> SavePrefValue(PrefEnums key, string value);

        Task ClearPreference();
    }
}
