using PlatformAbstractions.Interfaces;

namespace pw.lena.Core.Business.ViewModels
{
    public class DeviceViewModel
    {        
        private ITypeDevice typeDevice;
        private IDeviceProperty deviceProperty;
        public DeviceViewModel(ITypeDevice typeDevice, IDeviceProperty deviceProperty)
        {
            this.typeDevice = typeDevice;
            this.deviceProperty = deviceProperty;
            Initialize();
        }

        private void Initialize()
        {
            AndroidIDmacHash = deviceProperty.GetAndroidID();
        }

        public string Name { get; set; }

        public int TypeDeviceID { get { return typeDevice.GetTypeDivice();}}

        public string Token { get; set; }

        public string AndroidIDmacHash { get; private set; }

        public int codeA { get; set; }
        public int codeB { get; set; }
    }
}
