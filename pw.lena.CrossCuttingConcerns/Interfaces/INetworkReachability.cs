using System.Threading.Tasks;

namespace pw.lena.CrossCuttingConcerns.Interfaces
{
    public interface INetworkReachability
    {
        bool IsOnlineMode { get; }
        Task<bool> IsReachableHost(string host, int msTimeout);
    }
}
