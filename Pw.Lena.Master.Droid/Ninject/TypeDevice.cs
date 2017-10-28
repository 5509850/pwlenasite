using PlatformAbstractions.Interfaces;
using pw.lena.CrossCuttingConcerns.Enums;

namespace pw.lena.master.Droid
{
    public class TypeDevice : ITypeDevice
    {
        public int GetTypeDivice()
        {
            return (int)TypeDevicePW.AndroidMaster;
        }
    }
}