using PlatformAbstractions.Interfaces;
using pw.lena.CrossCuttingConcerns.Enums;

namespace Pw.Lena.Slave.Droid.Ninject
{
    public class TypeDevice : ITypeDevice
    {
        public int GetTypeDivice()
        {
            return (int)TypeDevicePW.AndroidSlave;
        }
    }
}