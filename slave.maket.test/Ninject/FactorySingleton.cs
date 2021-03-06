﻿using pw.lena.Core.Data;
using pw.lena.CrossCuttingConcerns;

namespace slave.maket.test.Ninject
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
                    _factory.Init(new DesktopRegistry());
                }
                return _factory;
            }
        }
    }
}

