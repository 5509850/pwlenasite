using System;
using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Views;
using Pw.Lena.Slave.Droid.Screens.Contracts;
using GalaSoft.MvvmLight.Helpers;
using Pw.Lena.Slave.Droid.UI.ViewHolders;
using pw.lena.Core.Business.ViewModels.Slave;
using pw.lena.CrossCuttingConcerns.Helpers;
using Pw.Lena.Slave.Droid.Screens.ActivityRequestCodes;
using Pw.Lena.Slave.Droid.ServicesBack;

namespace Pw.Lena.Slave.Droid.Screens.Fragments
{
    public class LandingFragment : Android.Support.V4.App.Fragment, IContentFragment
    {
        private readonly List<Binding> bindings = new List<Binding>();

        public int TitleResourceId { get; } = Resource.String.empty;

        private FragmentLandingViewHolder ViewHolder { get; set; }

        private LandingViewModel ViewModel { get; set; }

        GPSBackgroundServiceBinder _binder;
        GPSBackgroundServiceConnection _gpsServiceConnection;
        Intent _gpsServiceIntent;
        private GPSBackgroundServiceReciever _receiver;

        #region Fragment Lifecycle

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_landing, null);
            
            ViewHolder = new FragmentLandingViewHolder(view);
            ViewModel = App.ViewModelLocator.CreateLandingViewModel();

            ViewHolder.SpendingsLayout.Click += SpendingsLayout_Click;
            ViewHolder.LayoutAttachments.Click += LayoutAttachments_Click;
            ViewHolder.TakePhotoLayout.Click += TakePhotoLayout_Click;

            SetBindings();

            return view;
        }

        public override void OnStart()
        {
            base.OnStart();

            ViewModel.RegisterSyncNotifications();
        }

        public override void OnStop()
        {
            base.OnStop();

            ViewModel.UnregisterSyncNotifications();
        }

        public override void OnResume()
        {
            base.OnResume();
  //          RegisterBroadcastReceiver();
        }

       

        public override void OnPause()
        {
            base.OnPause();
        //    UnRegisterBroadcastReceiver();
        }


        #endregion

        #region GPS TEST - REMOVE TO ViewModels -------------------------------------

        private void RegisterService()
        {
            try
            {              
                _gpsServiceConnection = new GPSBackgroundServiceConnection(_binder);
                _gpsServiceIntent = new Intent(Android.App.Application.Context, typeof(GPSBackgroundService));
                Android.App.Application.Context.BindService(_gpsServiceIntent, _gpsServiceConnection, Bind.AutoCreate);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
            }
        }

        private void RegisterBroadcastReceiver()
        {
            try
            {
                IntentFilter filter = new IntentFilter(GPSBackgroundServiceReciever.LOCATION_UPDATED);
                filter.AddCategory(Intent.CategoryDefault);
                _receiver = new GPSBackgroundServiceReciever();
                Android.App.Application.Context.RegisterReceiver(_receiver, filter);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
            }
        }

        private void UnRegisterBroadcastReceiver()
        {
            if (_receiver != null)
            {
                Android.App.Application.Context.UnregisterReceiver(_receiver);
            }
        }

        #endregion

        #region UI Initialization

        private void SetBindings()
        {
        //    bindings.Add(
        //        this.SetBinding(() => ViewModel.SpendingCount, () => ViewHolder.SpendingCountTextView.Text, BindingMode.OneWay)
        //            .ConvertSourceToTarget(x => ViewModelValueConverter.Convert<NumberValueConverter>(x)));
        //    bindings.Add(
        //        this.SetBinding(() => ViewModel.TakeCareSpendingCount, () => ViewHolder.TakeCareSpendingCountTextView.Text, BindingMode.OneWay)
        //            .ConvertSourceToTarget(x => ViewModelValueConverter.Convert<NumberValueConverter>(x)));
        //    bindings.Add(
        //        this.SetBinding(() => ViewModel.AttachmentCount, () => ViewHolder.AttachmentCountTextView.Text, BindingMode.OneWay)
        //            .ConvertSourceToTarget(x => ViewModelValueConverter.Convert<NumberValueConverter>(x)));
        //    bindings.Add(
        //        this.SetBinding(() => ViewModel.TakeCareAttachmentCount, () => ViewHolder.TakeCareAttachmentCountTextView.Text, BindingMode.OneWay)
        //            .ConvertSourceToTarget(x => ViewModelValueConverter.Convert<NumberValueConverter>(x)));
        }

        #endregion

        #region Handlers

        private void SpendingsLayout_Click(object sender, EventArgs e)
        {
            var container = Activity as IFragmentContainer;

           // container?.ChangeCurrentFragment<SpendingListFragment>();
        }

        private void LayoutAttachments_Click(object sender, EventArgs e)
        {
            var container = Activity as IFragmentContainer;
    //        RegisterService();

           // container?.ChangeCurrentFragment<MyAttachmentsFragment>();
        }       

        private void TakePhotoLayout_Click(object sender, EventArgs e)
        {
            var intent = new Intent(Activity, typeof(TrackerActivity));

            Activity.StartActivityForResult(intent, ActivityRequestCode.TrackerActivity);

            //var intent = new Intent(Activity, typeof(MapViewActivity));

            //Activity.StartActivityForResult(intent, ActivityRequestCode.MapViewActivity);
        }

        #endregion

        public void Cleanup()
        {
            ViewModel.Cleanup();
        }
    }
}