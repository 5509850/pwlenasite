using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Pw.Lena.Slave.Droid.Utils;
using Pw.Lena.Slave.Droid.Services;
using Carto.Ui;

namespace Pw.Lena.Slave.Droid
{
    [Application(Theme = "@style/AppTheme")]
    public class App : Application
    {
        const string CartoLicense = "XTUN3Q0ZFMVNubERFNGs2eUFQUVVPOFZ5TThlU1FSWThBaFJCbnZJeVFPQ2NTQmUwRGw2QklMckIzZ1pKUXc9PQoKYXBwVG9rZW49ODFiNzBjYTYtOWY0Ni00ZTRkLWI3YWYtMzI3NjdhYTkwMDM1CnBhY2thZ2VOYW1lPVB3LkxlbmEuU2xhdmUuRHJvaWQKb25saW5lTGljZW5zZT0xCnByb2R1Y3RzPXNkay14YW1hcmluLWFuZHJvaWQtNC4qCndhdGVybWFyaz1jYXJ0b2RiCg==";
        public App(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public static ViewModelLocator ViewModelLocator { get; } = new ViewModelLocator();

        public override void OnCreate()
        {
            //ChangeDefaultLocale();

            MapView.RegisterLicense(CartoLicense, ApplicationContext);

            AppLifeCycleWatcher.Init(this);
            AppLifeCycleWatcher.Instance.BecameBackground += AppLifeCycleWatcher_BecameBackground;
            AppLifeCycleWatcher.Instance.BecameForeground += AppLifeCycleWatcher_BecameForeground;

            base.OnCreate();
        }

        public override void OnTerminate()
        {
            //ExecuteBackGroundSyncService(BackGroundSyncService.Action.Cancel);

            //AppLifeCycleWatcher.Instance.BecameBackground -= AppLifeCycleWatcher_BecameBackground;
            //AppLifeCycleWatcher.Instance.BecameForeground -= AppLifeCycleWatcher_BecameForeground;

            base.OnTerminate();
        }

        public void AppLifeCycleWatcher_BecameBackground(object sender, EventArgs e)
        {
            ExecuteBackGroundSyncService(BackGroundSyncService.Action.Complete);
        }

        public void AppLifeCycleWatcher_BecameForeground(object sender, EventArgs e)
        {
            ExecuteBackGroundSyncService(BackGroundSyncService.Action.Run);
        }

        private void ExecuteBackGroundSyncService(BackGroundSyncService.Action action)
        {
            var intent = new Intent(this, typeof(BackGroundSyncService));

            intent.PutExtra(nameof(BackGroundSyncService.Action), (int)action);

            StartService(intent);
        }

        //private bool ServicePointManager_ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        //{
        //    return true;
        //}

        //private void ChangeDefaultLocale()
        //{
        //    Locale.Default = new Locale(CtcCulture.Language, CtcCulture.Country);

        //    var config = new Configuration
        //    {
        //        Locale = Locale.Default
        //    };

        //    BaseContext.Resources.UpdateConfiguration(config, BaseContext.Resources.DisplayMetrics);
        //}
    }
}