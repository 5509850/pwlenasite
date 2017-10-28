using Ninject;
using PlatformAbstractions.Interfaces;
using pw.lena.slave.winpc.Services;
using SQLite.Net.Interop;

namespace pw.lena.slave.winpc
{
    public class DesktopRegistry : IUIRegistry
    {
        public void Register(IKernel kernel)
        {
            kernel.Bind<IFileSystemService>().To<FileSystemService>().InSingletonScope();
            kernel.Bind<ILocalizer>().To<Localizer>().InSingletonScope();
            // kernel.Bind<ISQLitePlatform>().To<SQLitePlatformWin32>().InSingletonScope();
            kernel.Bind<ISQLitePlatform>().To<SQLitePlatformDesktop>().InSingletonScope();
            kernel.Bind<IPlatformException>().To<PlatformException>().InSingletonScope();
            kernel.Bind<IDeviceProperty>().To<DeviceProperty>().InSingletonScope();
        }
    }
}
