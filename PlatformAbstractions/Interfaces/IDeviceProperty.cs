namespace PlatformAbstractions.Interfaces
{
    public interface IDeviceProperty
    {
        string GetTelephonyDeviceID();

        string GetTelephonySIMSerialNumber();

        string GetAndroidID();

        string GetDeviceID();
    }
}
