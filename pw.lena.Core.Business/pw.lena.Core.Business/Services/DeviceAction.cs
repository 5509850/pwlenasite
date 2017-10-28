using JetBrains.Annotations;
using Plugin.Vibrate.Abstractions;
using pw.lena.Core.Business.Services.Contract;

namespace pw.lena.Core.Business.Services
{
    public class DeviceAction : IDeviceAction
    {
        private readonly IVibrate vibrate;
        public DeviceAction([NotNull] IVibrate vibrate)
        {
            this.vibrate = vibrate;            
        }

        public void VibrateNow(int timemilisec)
        {
            vibrate.Vibration(timemilisec);                  
        }
    }
}