using PlatformAbstractions.Interfaces;
using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace pw.lena.slave.winpc.Services
{
    public class DeviceProperty : IDeviceProperty
    {
        ///https://forums.xamarin.com/discussion/1342/getting-unique-device-id

        private string androidID = string.Empty;
        private string deviceID = string.Empty;
        private string telephonyDeviceID = string.Empty;
        private string telephonySIMSerialNumber = string.Empty;

        public string GetAndroidID()
        {
            GetAll();
            return androidID ?? string.Empty;
        }

        public string GetDeviceID()
        {
            GetAll();
            return deviceID ?? string.Empty;
        }

        public string GetTelephonyDeviceID()
        {
            GetAll();
            return telephonyDeviceID ?? string.Empty;
        }

        public string GetTelephonySIMSerialNumber()
        {
            GetAll();
            return telephonySIMSerialNumber ?? string.Empty;
        }

        private void GetAll()
        {
            //for all property only mac adress!!!!!!!!!!!!!!!
            try
            {
                telephonyDeviceID =
                telephonySIMSerialNumber =
                androidID =
                deviceID =
                (
                    from nic in NetworkInterface.GetAllNetworkInterfaces()
                    where nic.OperationalStatus == OperationalStatus.Up
                    select nic.GetPhysicalAddress().ToString()
                ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                var err = ex.Message;
            }
        }
    }
}