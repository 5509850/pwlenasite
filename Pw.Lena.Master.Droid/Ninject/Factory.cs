using System;
using Ninject;
using PlatformAbstractions.Interfaces;

namespace pw.lena.master.Droid.Ninject
{
    public class Factory
    {
        private IKernel _kernel = new StandardKernel();
        public void Init(IUIRegistry registry)
        {
            registry.Register(_kernel);
        }
        public T Get<T>()
        {
            try
            {
                return _kernel.Get<T>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Error {0} with Exception {1}", typeof(T).Name, ex));
                throw;
            }
        }
    }
}
