using JetBrains.Annotations;
using Plugin.Connectivity.Abstractions;
using pw.lena.Core.Business.Services.Contract;
using pw.lena.CrossCuttingConcerns.Helpers;
using System;
using System.Threading.Tasks;

namespace pw.lena.Core.Business.Services
{
    internal class ConnectivityService : IConnectivityService
    {
        private readonly IConnectivity connectivity;

        public ConnectivityService([NotNull] IConnectivity connectivity)
        {
            Guard.ThrowIfNull(connectivity, nameof(connectivity));

            this.connectivity = connectivity;
            this.connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        public event EventHandler<Data.Models.EventArgs.ConnectivityChangedEventArgs> ConnectivityChanged;

        public bool IsConnected => connectivity.IsConnected;

        public async Task<bool> IsReachableHost(string host, int msTimeout)
        {
            return await connectivity.IsReachable(host, msTimeout);
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            OnConnectivityChanged(new Data.Models.EventArgs.ConnectivityChangedEventArgs { IsConnected = e.IsConnected });
        }

        private void OnConnectivityChanged(Data.Models.EventArgs.ConnectivityChangedEventArgs e)
        {
            ConnectivityChanged?.Invoke(this, e);
        }
    }
}
