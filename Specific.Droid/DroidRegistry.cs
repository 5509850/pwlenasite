using Ninject;
using PlatformAbstractions.Interfaces;
using Specific.Droid.Services;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinAndroid;

namespace Specific.Droid
{
    public class DroidRegistry : IUIRegistry
    {
        public void Register(IKernel kernel)
        {
            kernel.Bind<IFileSystemService>().To<FileSystemService>().InSingletonScope();
            kernel.Bind<ILocalizer>().To<Localizer>().InSingletonScope();
            kernel.Bind<ISQLitePlatform>().To<SQLitePlatformAndroid>().InSingletonScope();
            kernel.Bind<IPlatformException>().To<PlatformException>().InSingletonScope();
            kernel.Bind<IDeviceProperty>().To<DeviceProperty>().InSingletonScope();          
        }
    }
}