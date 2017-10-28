using System.Globalization;
using PlatformAbstractions.Interfaces;
using Pw.Lena.Slave.Droid.Ninject;

namespace Pw.Lena.Slave.Droid.UI.Utils
{
    public class CurCult
    {        
        static CurCult()
        {            
            CurrentCulture = FactorySingleton.Factory.Get<ILocalizer>().CurrentCulture();
        }

        public static CultureInfo CurrentCulture { get; private set; }
    }
}