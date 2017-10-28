using pw.lena.Core.Business.ViewModels;
using pw.lena.Core.Business.ViewModels.Slave;
using Pw.Lena.Slave.Droid.Ninject;

namespace Pw.Lena.Slave.Droid
{
    public class ViewModelLocator
    {
        public PairViewModel CreatePairViewModel()
        {
            return FactorySingleton.Factory.Get<PairViewModel>();            
        }

        public DeviceViewModel CreateDeviceViewModel()
        {
            return FactorySingleton.Factory.Get<DeviceViewModel>();
        }

        public EntryViewModel CreateEntryViewModel()
        {
            return FactorySingleton.Factory.Get<EntryViewModel>();
        }

        public MenuViewModel CreateMenuViewModel()
        {
            return FactorySingleton.Factory.Get<MenuViewModel>();
        }

        public LandingViewModel CreateLandingViewModel()
        {
            return FactorySingleton.Factory.Get<LandingViewModel>();
        }

        public TrackerViewModel CreateTrackerViewModel()
        {
            return FactorySingleton.Factory.Get<TrackerViewModel>();
        }

        public MapViewModel CreateMapViewModel()
        {
            return FactorySingleton.Factory.Get<MapViewModel>();
        }
    }
}