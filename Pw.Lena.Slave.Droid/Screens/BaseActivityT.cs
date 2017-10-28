using GalaSoft.MvvmLight;


namespace Pw.Lena.Slave.Droid.Screens
{
    public abstract class BaseActivity<T> : BaseActivity where T : ICleanup
    {
        protected T ViewModel { get; set; }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            ViewModel?.Cleanup();
        }
    }
}