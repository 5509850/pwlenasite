using Plugin.Connectivity;
using pw.lena.CrossCuttingConcerns.Interfaces;
using System.Threading.Tasks;

namespace pw.lena.Core.Business.Services
{

    ///Depricated - need use ConnectivityService
    public class NetworkReachability : INetworkReachability
    {
        public bool IsOnlineMode
        {
            get
            {
                return CrossConnectivity.Current.IsConnected;
            }
        }
        public async Task<bool> IsReachableHost(string host, int msTimeout)
        {
            return await CrossConnectivity.Current.IsReachable(host, msTimeout);
        }
    }
}