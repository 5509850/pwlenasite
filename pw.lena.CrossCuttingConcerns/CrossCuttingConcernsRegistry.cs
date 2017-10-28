using Ninject;
using PlatformAbstractions.Interfaces;
using pw.lena.CrossCuttingConcerns.Interfaces;
using pw.lena.CrossCuttingConcerns.Localization;

namespace pw.lena.CrossCuttingConcerns
{
    public class CrossCuttingConcernsRegistry : IUIRegistry
    {
        public void Register(IKernel kernel)
        {
            kernel.Bind<ILocalizeService>().To<LocalizeService>().InSingletonScope();
            kernel.Bind<IConfiguration>().To<Configuration>().InSingletonScope();
        }
    }
}
