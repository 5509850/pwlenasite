using JetBrains.Annotations;
using pw.lena.Core.Data.Models;
using System.Threading;
using System.Threading.Tasks;

namespace pw.lena.Core.Business.Services.Contract
{
    public interface IIdentityService
    {
        [NotNull]
        Identity Identity { get; }

        void RefreshAsync([CanBeNull] string login, [CanBeNull] string sessionHash, CancellationToken cancellationToken = default(CancellationToken));
    }
}