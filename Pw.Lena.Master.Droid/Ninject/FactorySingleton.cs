using pw.lena.CrossCuttingConcerns;
using pw.lena.Core.Data;
using pw.lena.Core.Business;
using Specific.Droid;

namespace pw.lena.master.Droid.Ninject
{
    public class FactorySingleton
    {
        private static Factory _factory;

        private FactorySingleton() { }

        public static Factory Factory
        {
            get
            {
                if (_factory == null)
                {
                    _factory = new Factory();
                    _factory.Init(new CrossCuttingConcernsRegistry());
                    _factory.Init(new DataRegistry());
                    _factory.Init(new BusinessRegistry());
                    _factory.Init(new DroidRegistry());
                    _factory.Init(new UIRegistry());                    
                }
                return _factory;
            }
        }
    }
}

