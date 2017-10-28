using JetBrains.Annotations;
using pw.lena.Core.Business.Utils;
using pw.lena.Core.Data.Services.DataService.Contracts;
using pw.lena.CrossCuttingConcerns.Helpers;
using System.Threading.Tasks;

namespace pw.lena.Core.Business.Repositories
{
    internal class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly IAuthenticationClient authenticationClient;

        public AuthenticationRepository([NotNull] IAuthenticationClient authenticationClient)
        {
            Guard.ThrowIfNull(authenticationClient, nameof(authenticationClient));

            this.authenticationClient = authenticationClient;
        }

        public async Task<string> LoginAsync(string login, string password)
        {
            Guard.ThrowIfEmptyString(login, nameof(login));
            Guard.ThrowIfEmptyString(password, nameof(password));

            return await authenticationClient.LoginAsync(login, password);
        }

        public async Task LogoutAsync()
        {
            await authenticationClient.LogoutAsync();
        }
    }
}
