using System.Threading.Tasks;

namespace pw.lena.Core.Data.Services.DataService.Contracts
{
    public interface IAuthenticationClient
    {
        Task<string> LoginAsync(string login, string password);

        Task LogoutAsync();
    }
}