using PlatformAbstractions.Interfaces;
using Ninject;

namespace Pw.Lena.Slave.Droid.Ninject
{
    public class UIRegistry : IUIRegistry
    {
        public void Register(IKernel kernel)
        {
            kernel.Bind<ITypeDevice>().To<TypeDevice>().InSingletonScope();
        }
    }
}