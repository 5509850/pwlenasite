
using Android.App;
using Android.OS;
using Pw.Lena.Slave.Droid.Screens.Contracts;
using pw.lena.Core.Business.ViewModels.Slave;
using Android.Content.PM;
using Pw.Lena.Slave.Droid.UI.ViewHolders;
using Android.Views;
using Android.Support.V7.App;
using Android.Content;
using System;
using System.Threading.Tasks;
using Android.Graphics;
using Pw.Lena.Slave.Droid.Screens.Fragments;
using Android.Support.Design.Widget;
using Pw.Lena.Slave.Droid.UI.Extensions;
using Android.Runtime;
using Pw.Lena.Slave.Droid.Screens.Adapters;
using GalaSoft.MvvmLight.Helpers;
using Pw.Lena.Slave.Droid.Screens.Adapters.Models;
using System.Collections.Generic;
using Pw.Lena.Slave.Droid.ServicesBack;
using System.Linq;
//using Android.Support.Design.Widget;

namespace Pw.Lena.Slave.Droid.Screens
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class LandingActivity : BaseActivity<MenuViewModel>, IFragmentContainer
    {
        private int drawerGravity = (int)GravityFlags.Start;
        private ActionBarDrawerToggle drawerToggle;
        protected override int LayoutResource { get; } = Resource.Layout.activity_landing;
        private ActivityLandingViewHolder ViewHolder { get; set; }

        #region GPS vars

        GPSBackgroundServiceBinder _binder;
        GPSBackgroundServiceConnection _gpsServiceConnection;
        Intent _gpsServiceIntent;
        private GPSBackgroundServiceReciever _receiver;

        public static LandingActivity Instance;

        #endregion

        #region IFragmentContainer
        public void ChangeCurrentFragment<T>() where T : Android.Support.V4.App.Fragment, new()
        {
            var currentFragment = SupportFragmentManager.FindFragmentById(Resource.Id.fragmentContainer) as IContentFragment;
            currentFragment?.Cleanup();

            if (typeof(T) == typeof(LandingFragment))
            {
                ViewHolder.ActivityContainer.SetBackgroundResource(Resource.Drawable.landing_background);
                ViewHolder.Toolbar.SetBackgroundResource(Color.Transparent);
            }
            else
            {
                ViewHolder.ActivityContainer.SetBackgroundResource(0);
                ViewHolder.Toolbar.SetBackgroundResource(Resource.Color.Primary);
            }

            var fragment = new T();
            var fragmentTransaction = SupportFragmentManager.BeginTransaction();

            fragmentTransaction.Replace(Resource.Id.fragmentContainer, fragment);
            fragmentTransaction.Commit();

            var contentFragment = fragment as IContentFragment;

            SetToolbarTitle(contentFragment?.TitleResourceId ?? Resource.String.empty);

            CloseMenu();
        }

        public async Task ShowSaveSpendingInfoAsync(Intent data, View view)
        {
            //var name = data.GetStringExtra(SpendingDetailsActivity.SavedSpendingDetailsNameResult);
            //var statusIndex = data.GetIntExtra(SpendingDetailsActivity.SavedSpendingDetailsStatusResult, -1);

            //if (statusIndex > -1)
            //{
            string message = "Snapbar";

            //    var status = (SpendingStatus)statusIndex;

            //    if (status == SpendingStatus.Draft && !string.IsNullOrEmpty(name))
            //    {
            //        message = string.Format(Resources.GetString(Resource.String.msg_spending_saved_as_draft), name);
            //    }
            //    else if (status == SpendingStatus.Draft)
            //    {
            //        message = Resources.GetString(Resource.String.msg_spending_saved_as_draft_empty_summary);
            //    }
            //    else if (status == SpendingStatus.ReadyForSync)
            //    {
            //        message = string.Format(Resources.GetString(Resource.String.msg_spending_saved_as_ready_for_sync), name);
            //    }
            //    else
            //    {
            //        return;
            //    }

            await Snackbar.Make(view, message, Snackbar.LengthLong)
                    .ApplySourceSansProFont()
                    .ShowWithDelay();
            //}
        }
        #endregion

        #region Activity lifecycle

        protected override async void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok && data != null)
            {
                //if (requestCode == ActivityRequestCode.TakePhotoActivity)
                //{
                //    ProccessTakePhotoActivityResult(data);
                //}
                //else if (requestCode == ActivityRequestCode.SaveSpending)
                //{
                //    await ShowSaveSpendingInfoAsync(data, ViewHolder.FragmentContainer);
                //}
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Instance = this;

            ViewHolder = new ActivityLandingViewHolder(this);
            ViewModel = App.ViewModelLocator.CreateMenuViewModel();

            ViewHolder.AppMenuListView.Adapter = new AppMenuItemAdapter(GetMenuItems(), LayoutInflater);

            drawerToggle = new ActionBarDrawerToggle(
                this,
                ViewHolder.DrawerLayout,
                ViewHolder.Toolbar,
                Resource.String.empty,
                Resource.String.empty);

          //  ViewHolder.DrawerLayout.SetDrawerListener(drawerToggle);

            SetBindings();
            SetDrawerSize();

            ChangeCurrentFragment<LandingFragment>();
        }

        public override void OnBackPressed()
        {
            if (ViewHolder.DrawerLayout.IsDrawerOpen(drawerGravity))
            {
                CloseMenu();

                return;
            }

            base.OnBackPressed();
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);

            drawerToggle.SyncState();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (drawerToggle.OnOptionsItemSelected(item))
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        #endregion

        #region UI Initialization

        private void SetBindings()
        {
            this.SetBinding(() => ViewModel.SyncStatusDescription, () => ViewHolder.AppMenuSyncMessage.Text);
        }

        private void SetDrawerSize()
        {
            var screenSize = new Point();
            WindowManager.DefaultDisplay.GetSize(screenSize);
            int screenWidth = screenSize.X;
            int shift = Resources.GetDimensionPixelSize(Resource.Dimension.appMenuScreenIndent);

            ViewHolder.AppMenu.LayoutParameters.Width = screenWidth - shift;
        }

        private List<AppMenuItem> GetMenuItems()
        {
            return new List<AppMenuItem>
            {
                new AppMenuItem
                {
                    LeftSubItem = new AppMenuItemSubItem
                    {
                        ResourceId = Resource.String.appMenu_Home,
                        Action = () => ChangeCurrentFragment<LandingFragment>()
                    }
                }
                ,
                new AppMenuItem
                {
                    LeftSubItem = new AppMenuItemSubItem
                    {
                        ResourceId = Resource.String.appMenu_StartTracker,
                        Action = () => StartTracking()
                    },
                    RightSubItem = new AppMenuItemSubItem
                    {
                        ResourceId = Resource.Drawable.close,
                        Action = () => StopTracking()
                    }
                },
                 new AppMenuItem
                {
                    LeftSubItem = new AppMenuItemSubItem
                    {
                        ResourceId = Resource.String.appMenu_StartBeacon,
                        Action = () => StartNewActivity(typeof(LandingFragment))
                        //=> StartBeacone()
                    },
                    RightSubItem = new AppMenuItemSubItem
                    {
                        ResourceId = Resource.Drawable.close,
                        Action = () => StartNewActivity(typeof(LandingFragment))
                        //=> StopBeacone()
                    }
                },
                //new AppMenuItem
                //{
                //    LeftSubItem = new AppMenuItemSubItem
                //    {
                //        ResourceId = Resource.String.appMenu_Spendings,
                //        Action = () => ChangeCurrentFragment<SpendingListFragment>()
                //    },
                //    RightSubItem = new AppMenuItemSubItem
                //    {
                //        ResourceId = Resource.Drawable.plus,
                //        Action = () => StartNewActivity(typeof(SpendingDetailsActivity), ActivityRequestCode.SaveSpending)
                //    }
                //},
                new AppMenuItem
                {
                    LeftSubItem = new AppMenuItemSubItem
                    {
                        ResourceId = Resource.String.appMenu_Settings,
                        Action = () => StartNewActivity(typeof(LandingFragment))
                    }
                }
            };
        }

        private void StartTracking()
        {
            CloseMenu();            
            RegisterGpsService();
            RegisterBroadcastReceiver();           
        }

        private void StopTracking()
        {
            CloseMenu();          
            UnRegisterGpsService();
            UnRegisterBroadcastReceiver();           
            Alert("Stop Tracking");
        }
        #endregion

        #region GPS-------------------------------------------------

        private void RegisterGpsService()
        {
            try
            {  //  if (!isServiceRun())
                {
                    _gpsServiceConnection = new GPSBackgroundServiceConnection(_binder);
                    _gpsServiceIntent = new Intent(Android.App.Application.Context, typeof(GPSBackgroundService));
                    Android.App.Application.Context.BindService(_gpsServiceIntent, _gpsServiceConnection, Bind.AutoCreate);
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
            }
        }

        private void UnRegisterGpsService()
        {
            try
            {
                if (_gpsServiceConnection != null)
                {
                    {
                        Application.Context.UnbindService(_gpsServiceConnection);
                    }   
                }
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
                RegisterReceiver(_receiver, filter);
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
                try
                {
                    UnregisterReceiver(_receiver);                                      
                }
                catch (Exception ex)
                {
                    var err = ex.Message;
                }
            }
        }

        public void UpdateUI(Intent intent)
        {
            Alert(string.Format("Location {0}\nAddress{1}\nRemarks{2}",
            intent.GetStringExtra("Location"),
            intent.GetStringExtra("Address"),
            intent.GetStringExtra("Remarks")
                ));
            //ViewHolder.TxtLocation.Text = intent.GetStringExtra("Location");
            //ViewHolder.TxtAddress.Text = intent.GetStringExtra("Address");
            //ViewHolder.TxtRemarks.Text = intent.GetStringExtra("Remarks");
        }


        //private IEnumerable<string> GetRunningServices()
        //{
        //    var manager = (ActivityManager)GetSystemService(ActivityService);
        //    return manager.GetRunningServices(int.MaxValue).Select(
        //        service => service.Service.ClassName).ToList();
        //}

        //private bool isServiceRun()
        //{
        //    var servicelist = GetRunningServices();
        //    //TODO:!!!
        //    // if (_gpsServiceIntent != null)
        //    //{
        //     //   var i = _gpsServiceIntent;
        //    //}
        //    //service name not full - get from intent!!!!
        //    if (servicelist != null)
        //    {
        //        foreach (string service in servicelist)
        //        {
        //            var ser = service.ToLower();
        //            var myserv = typeof(GPSBackgroundService).ToString().ToLower();
        //            if (ser.Equals(myserv))
        //            {                        
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        #endregion

        #region Helpers

        private void CloseMenu()
        {
            if (ViewHolder.DrawerLayout.IsDrawerOpen(drawerGravity))
            {
                ViewHolder.DrawerLayout.CloseDrawer(drawerGravity);
            }
        }

        private void StartNewActivity(Type activity, int requestCode = 0)
        {
            CloseMenu();

            if (requestCode == 0)
            {
                StartActivity(activity);
            }
            else
            {
                StartActivityForResult(activity, requestCode);
            }
        }

        #endregion

        #region Process Activity Result

        private void ProccessTakePhotoActivityResult(Intent data)
        {
            //var actionValue = data.GetIntExtra(nameof(TakePhotoActivity.ActionResult), -1);

            //if (actionValue > -1)
            //{
            //    var action = (TakePhotoActivity.ActionResult)actionValue;

            //    switch (action)
            //    {
            //        case TakePhotoActivity.ActionResult.NavigateToGallery:
            //            ChangeCurrentFragment<MyAttachmentsFragment>();
            //            break;
            //    }
            //}
        }

        #endregion
    }
}