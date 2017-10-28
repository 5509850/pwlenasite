using Plugin.Connectivity.Abstractions;
using System;
using System.Threading.Tasks;

namespace pw.lena.Core.Business.Services.Contract
{
    public interface IConnectivityService
    {
        event EventHandler<Data.Models.EventArgs.ConnectivityChangedEventArgs> ConnectivityChanged;

        bool IsConnected { get; }

        Task<bool> IsReachableHost(string host, int msTimeout);
    }
}