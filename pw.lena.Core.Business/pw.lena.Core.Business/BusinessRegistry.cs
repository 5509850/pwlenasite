using Ninject;
using PlatformAbstractions.Interfaces;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Plugin.Vibrate;
using Plugin.Vibrate.Abstractions;
using pw.lena.Core.Business.Services;
using pw.lena.Core.Business.Services.Contract;
using pw.lena.Core.Business.ViewModels;
using pw.lena.Core.Business.ViewModels.Slave;
using pw.lena.CrossCuttingConcerns.Interfaces;

namespace pw.lena.Core.Business
{
    public class BusinessRegistry : IUIRegistry
    {
        public void Register(IKernel kernel)
        {            
            kernel.Bind<IConnectivity>().ToConstant(CrossConnectivity.Current);
            kernel.Bind<IVibrate>().ToConstant(CrossVibrate.Current);
            
            kernel.Bind<INetworkReachability>().To<NetworkReachability>().InSingletonScope();
            kernel.Bind<IConnectivityService>().To<ConnectivityService>().InSingletonScope();
            kernel.Bind<IAuthenticationService>().To<AuthenticationService>().InSingletonScope();
            kernel.Bind<ISyncService>().To<SyncService>().InSingletonScope();
            kernel.Bind<IIdentityService>().To<IdentityService>().InSingletonScope();
            kernel.Bind<IDeviceAction>().To<DeviceAction>().InSingletonScope();

            kernel.Bind<ISyncWorker>().To<SyncWorker>().InSingletonScope();

            kernel.Bind<LoginViewModel>().ToSelf();
            kernel.Bind<EntryViewModel>().ToSelf();
            kernel.Bind<PairViewModel>().ToSelf();
            kernel.Bind<DeviceViewModel>().ToSelf();
            kernel.Bind<LandingViewModel>().ToSelf();
            kernel.Bind<MenuViewModel>().ToSelf();
            kernel.Bind<TrackerViewModel>().ToSelf();
            kernel.Bind<MapViewModel>().ToSelf();
            


        }
    }
}