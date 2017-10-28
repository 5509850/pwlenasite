using pw.lena.Core.Data.Services.DataService.Contracts;
using pw.lena.CrossCuttingConcerns.Helpers;
using System.Threading.Tasks;

namespace pw.lena.Core.Data.Services.DataService
{
    internal class AuthenticationClient : IAuthenticationClient
    {
        //private readonly IRestApiContext apiContext;

        public AuthenticationClient()
        {
            //IRestApiContext apiContext
            //Guard.ThrowIfNull(apiContext, "apiContext");

            //this.apiContext = apiContext;
        }

        public async Task<string> LoginAsync(string login, string password)
        {
            Guard.ThrowIfEmptyString(login, "login");
            Guard.ThrowIfEmptyString(password, "password");

            //var auth = await apiContext.ExecuteAsync<Auth>(
            //    "/rest/auth/token",
            //    HttpMethod.Post,
            //    request =>
            //    {
            //        request.AddJsonBody(new { login = login, password = password });
            //    },
            //    false);
            //NEED TODO:
            return "hash"; // auth.SessionHash;
        }

        public async Task LogoutAsync()
        {
            //await apiContext.ExecuteAsync<object>(
            //    "/rest/auth/logout",
            //    HttpMethod.Get,
            //    true);
        }
    }
}
