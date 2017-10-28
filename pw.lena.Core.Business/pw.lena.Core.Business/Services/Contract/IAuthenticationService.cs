using JetBrains.Annotations;
using pw.lena.Core.Data.Models;
using System;
using System.Threading.Tasks;

namespace pw.lena.Core.Business.Services.Contract
{
    public interface IAuthenticationService
    {
        event EventHandler<EventArgs> AuthenticationChanged;

        Identity Identity { get; }

        Task LoginAsync([NotNull] string login, [NotNull] string password);

        Task LogoutAsync();
    }
}