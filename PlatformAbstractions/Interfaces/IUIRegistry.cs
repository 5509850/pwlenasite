using Ninject;

namespace PlatformAbstractions.Interfaces
{
    public interface IUIRegistry
    {
        void Register(IKernel kernel);
    }
}
