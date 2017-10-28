using System;
using Android.App;
using Android.Telephony;
using Android.Content;
using Java.Util;
using PlatformAbstractions.Interfaces;

namespace Specific.Droid.Services
{
    public class DeviceProperty : IDeviceProperty
    {
        ///https://forums.xamarin.com/discussion/1342/getting-unique-device-id

        private string androidID = string.Empty;
        private string deviceID = string.Empty;
        private string telephonyDeviceID = string.Empty;
        private string telephonySIMSerialNumber = string.Empty;
        Activity activity = (Activity)Application.Context;

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
            try
            {
                TelephonyManager telephonyManager = (TelephonyManager)activity.ApplicationContext.GetSystemService(Context.TelephonyService);
                if (telephonyManager != null)
                {
                    if (!string.IsNullOrEmpty(telephonyManager.DeviceId))
                        telephonyDeviceID = telephonyManager.DeviceId;
                    if (!string.IsNullOrEmpty(telephonyManager.SimSerialNumber))
                        telephonySIMSerialNumber = telephonyManager.SimSerialNumber;
                }
                androidID = Android.Provider.Settings.Secure.GetString(activity.ApplicationContext.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
                var deviceUuid = new UUID(androidID.GetHashCode(), ((long)telephonyDeviceID.GetHashCode() << 32) | telephonySIMSerialNumber.GetHashCode());
                deviceID = deviceUuid.ToString();
            }
            catch (Exception ex)
            {
                var err = ex.Message;
            }
        }
    }
}