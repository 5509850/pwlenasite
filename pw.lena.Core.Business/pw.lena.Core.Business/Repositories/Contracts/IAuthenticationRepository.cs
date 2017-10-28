using JetBrains.Annotations;
using System.Threading.Tasks;

namespace pw.lena.Core.Business.Repositories
{
    public interface IAuthenticationRepository
    {
        Task<string> LoginAsync([NotNull] string login, [NotNull] string password);

        Task LogoutAsync();
    }
}