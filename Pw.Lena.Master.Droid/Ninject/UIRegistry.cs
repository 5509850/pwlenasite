using PlatformAbstractions.Interfaces;
using Ninject;

namespace pw.lena.master.Droid
{
    public class UIRegistry : IUIRegistry
    {
        public void Register(IKernel kernel)
        {
            kernel.Bind<ITypeDevice>().To<TypeDevice>().InSingletonScope();            
        }
    }
}