using System.Threading;
using System.Threading.Tasks;

namespace pw.lena.Core.Business.Services.Contract
{
    public interface ISyncService
    {
        Task SyncAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}