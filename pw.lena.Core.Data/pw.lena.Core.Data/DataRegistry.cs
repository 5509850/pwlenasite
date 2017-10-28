using Ninject;
using PlatformAbstractions.Interfaces;
using pw.lena.Core.Data.Services.DataService;
using pw.lena.Core.Data.Services.DataService.Contracts;

namespace pw.lena.Core.Data
{
    public class DataRegistry : IUIRegistry
    {
        public void Register(IKernel kernel)
        {
            kernel.Bind<IPairDeviceService>().To<PairDeviceService>().InSingletonScope();
            kernel.Bind<IMastersService>().To<MastersService>().InSingletonScope();
            kernel.Bind<IAuthenticationClient>().To<AuthenticationClient>().InSingletonScope();
            kernel.Bind<IPreferenceService>().To<PreferenceService>().InSingletonScope();

            kernel.Bind<ICommandService>().To<CommandService>().InSingletonScope();
            kernel.Bind<IMastersService>().To<MastersService>().InSingletonScope();
            kernel.Bind<IPowerPcService>().To<PowerPcService>().InSingletonScope();
            kernel.Bind<IScreenShotPCService>().To<ScreenShotPCService>().InSingletonScope();
    
        }
    }
}