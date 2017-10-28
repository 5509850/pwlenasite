using System.Collections.Generic;

using Android.App;
using Android.OS;
using pw.lena.Core.Business.ViewModels.Slave;
using GalaSoft.MvvmLight.Helpers;
using Pw.Lena.Slave.Droid.UI.ViewHolders;
using Android.Views;
using Android.Widget;
using Android.Content;
using Pw.Lena.Slave.Droid.Services;
using System;

namespace Pw.Lena.Slave.Droid.Screens
{
    [Activity(Label = "TrackerActivity")]
    public class TrackerActivity : BaseActivity<TrackerViewModel>
    {
        static readonly string TAG = "X:" + typeof(TrackerActivity).Name;      
        private readonly List<Binding> bindings = new List<Binding>();

        GPSServiceBinder _binder;
        GPSServiceConnection _gpsServiceConnection;
        Intent _gpsServiceIntent;
        private GPSServiceReciever _receiver;
        public static TrackerActivity Instance;

        protected override int LayoutResource { get; } = Resource.Layout.activity_tracker;

        private ActivityTrackerViewHolder ViewHolder { get; set; }

        #region LifeCycle
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Instance = this;

            ViewHolder = new ActivityTrackerViewHolder(this);
            ViewModel = App.ViewModelLocator.CreateTrackerViewModel();

            Toolbar.Visibility = ViewStates.Visible;
            //  Toolbar.SetTitle(Resource.String.guard_login_title);
            SetToolbarTitle("");
            RegisterService();
            SetBindings();
        }

        protected override void OnResume()
        {
            base.OnResume();
            RegisterBroadcastReceiver();
        }

        protected override void OnPause()
        {
            base.OnPause();
            UnRegisterBroadcastReceiver();
        }

        #endregion

        private void SetBindings()
        {
            ///ViewHolder            
        }
        private void RegisterService()
        {
            try
            {
                _gpsServiceConnection = new GPSServiceConnection(_binder);
                _gpsServiceIntent = new Intent(Android.App.Application.Context, typeof(GPSService));
                BindService(_gpsServiceIntent, _gpsServiceConnection, Bind.AutoCreate);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
            }
        }
        private void RegisterBroadcastReceiver()
        {
            IntentFilter filter = new IntentFilter(GPSServiceReciever.LOCATION_UPDATED);
            filter.AddCategory(Intent.CategoryDefault);
            _receiver = new GPSServiceReciever();
            RegisterReceiver(_receiver, filter);
        }

        private void UnRegisterBroadcastReceiver()
        {
            UnregisterReceiver(_receiver);
        }
        public void UpdateUI(Intent intent)
        {
            ViewHolder.TxtLocation.Text = intent.GetStringExtra("Location");
            ViewHolder.TxtAddress.Text = intent.GetStringExtra("Address");
            ViewHolder.TxtRemarks.Text = intent.GetStringExtra("Remarks");
        }

      

        
    }
}

