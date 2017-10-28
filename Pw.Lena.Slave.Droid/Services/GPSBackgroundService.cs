using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Locations;
using Pw.Lena.Slave.Droid.Services.GPS;
using Android.Widget;
using Pw.Lena.Slave.Droid.Screens;

namespace Pw.Lena.Slave.Droid.ServicesBack
{
    //GPSBackgroundService

    [Service]
    public class GPSBackgroundService : Service, ILocationListener
    {
        // private const string _sourceAddress = "TGU Tower, Cebu IT Park, Jose Maria del Mar St,Lahug, Cebu City, 6000 Cebu";
        private string _location = string.Empty;
        private string _address = string.Empty;
        private string _remarks = string.Empty;

        public const string LOCATION_UPDATE_ACTION = "LOCATION_UPDATED_BACKGROUND";
        private Location _currentLocation;
        IBinder _binder;
        protected LocationManager _locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(LocationService);

        /// <summary>
        /// This method must be implemented by all bound services. It is invoked when the first client tries to connect to the service. It will return an instance of IBinder so that the client may interact with the service. As long as the service is running, the IBinder object will be used to fulfill any future client requests to bind to the service.
        /// </summary>
        /// <param name="intent"></param>
        /// <returns></returns>
        public override IBinder OnBind(Intent intent)
        {
            _binder = new GPSBackgroundServiceBinder(this);
            return _binder;
        }

        /// <summary>
        /// This method is invoked by Android as it is instantiating the service. It is used to initialize any variables or objects that are required by the service during it's lifetime. This method is optional.
        /// </summary>
        public override void OnCreate()
        {
            base.OnCreate();
        }
        /// <summary>
        /// This method is called when all bound clients have unbound. By returning true from this method, the service will later call OnRebind with the intent passed to OnUnbind when new clients bind to it. You would do this when a service continues running after it has been unbound. This would happen if the recently unbound service were also a started service, and StopService or StopSelf hadn’t been called. In such a scenario, OnRebind allows the intent to be retrieved. The default returns false , which does nothing. Optional.
        /// </summary>
        /// <param name="intent"></param>
        /// <returns></returns>
        public override bool OnUnbind(Intent intent)
        {
            return base.OnUnbind(intent);
        }

        /// <summary>
        /// This method is called when Android is destroying the service. Any necessary cleanup, such as releasing resources, should be performed in this method. Optional.
        /// </summary>
        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {       
            return StartCommandResult.Sticky;
        }

        /// <summary>
        ///A constant indicating an approximate accuracy
        ///Accuracy.High
        ///Accuracy.Low
        ///Accuracy.Fine
        ///Accuracy.Medium
        ///Accuracy.NoRequirement
        /// </summary>
        /// <param name="minTimeUpdate"></param>
        /// <param name="minDistance"></param>
        /// <param name="accuracy"></param>
        /// <param name="power"></param>
        public void StartLocationUpdates(
            long minTimeUpdate = 0, 
            float minDistance = 0, 
            Accuracy accuracy = Accuracy.Coarse, 
            Power power = Power.Medium)
        {            
            //TODO 
            Criteria criteriaForGPSService = new Criteria
            {                
                Accuracy = accuracy,
                PowerRequirement = power
            };
            try
            {
                var locationProvider = _locationManager.GetBestProvider(criteriaForGPSService, true);
                _locationManager.RequestLocationUpdates(locationProvider, minTimeUpdate, minDistance, this);
            }
        //     Exceptions:
        //   T:Java.Lang.IllegalArgumentException:
        //     if provider is null or doesn't exist /// on this device
        //
        //   T:Java.Lang.IllegalArgumentException:
        //     if listener is null
        //
        //   T:Java.Lang.RuntimeException:
        //     if the calling thread has no Looper
        //
        //   T:Java.Lang.SecurityException:
      
            catch (Java.Lang.IllegalArgumentException ex)
            {
                var error = ex.Message;
            }           
            catch (Java.Lang.SecurityException ex)
            {  //     if no suitable permission is present ///
                var error = ex.Message;
            }           
            catch (Java.Lang.RuntimeException ex)
            {
                var error = ex.Message;                
            }
        }

        public void StopLocationUpdate()
        {
            if (_locationManager != null)
            {
                _locationManager.RemoveUpdates(this);
            }
        }

        public event EventHandler<LocationChangedEventArgs> LocationChanged = delegate { };
        public void OnLocationChanged(Location location)
        {
            try
            {
                _currentLocation = location;

                if (_currentLocation == null)
                    _location = "Unable to determine your location.";
                else
                {
                    _location = string.Format("{0},{1}", _currentLocation.Latitude, _currentLocation.Longitude);

                    Geocoder geocoder = new Geocoder(this);

                    //The Geocoder class retrieves a list of address from Google over the internet
                    IList<Address> addressList = geocoder.GetFromLocation(_currentLocation.Latitude, _currentLocation.Longitude, 10);

                    Address addressCurrent = addressList.FirstOrDefault();

                    if (addressCurrent != null)
                    {
                        System.Text.StringBuilder deviceAddress = new StringBuilder();

                        for (int i = 0; i < addressCurrent.MaxAddressLineIndex; i++)
                            deviceAddress.Append(addressCurrent.GetAddressLine(i))
                                .AppendLine(",");

                        _address = deviceAddress.ToString();
                    }
                    else
                        _address = "Unable to determine the address.";

                    //IList<Address> source = geocoder.GetFromLocationName(_sourceAddress, 1);
                    //Address addressOrigin = source.FirstOrDefault();
                    //var coord1 = new LatLng(addressOrigin.Latitude, addressOrigin.Longitude);

                    var coord1 = new LatLng(53.885053, 27.5027954);
                    var coord2 = new LatLng(addressCurrent.Latitude, addressCurrent.Longitude);

                    //var distanceInRadius = GPS.Utils.HaversineDistance(coord1, coord2, GPS.Utils.DistanceUnit.Miles);
                    var distanceInRadius = Services.GPS.Utils.HaversineDistance(coord1, coord2, Services.GPS.Utils.DistanceUnit.Kilometers);

                    _remarks = string.Format("Your are {0} km away from your original location.", distanceInRadius.ToString("F2"));//miles

                    Intent intent = new Intent(this, typeof(GPSBackgroundServiceReciever));
                    intent.SetAction(GPSBackgroundServiceReciever.LOCATION_UPDATED);
                    intent.AddCategory(Intent.CategoryDefault);
                    intent.PutExtra("Location", _location);
                    intent.PutExtra("Address", _address);
                    intent.PutExtra("Remarks", _remarks);
                    SendBroadcast(intent);
                }
            }
            catch (Exception ex)
            {
                _address = "Unable to determine the address.";
            }

        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            Alert(string.Format("{0} OnStatusChanged {1}", provider == null ? "n/a": provider, status.ToString()));
            //Availability.Available
            //Availability.OutOfService
            //Availability.TemporarilyUnavailable
            //TO DO:
        }

        public void OnProviderDisabled(string provider)
        {
            //TO DO:
            Alert(string.Format("OnProviderDisabled {0}", provider.ToString()));
        }

        public void OnProviderEnabled(string provider)
        {
            Alert(string.Format("OnProviderEnabled {0}", provider.ToString()));
            //TO DO:
        }

        private void Alert(string message)
        {
           Toast.MakeText(this, message, ToastLength.Short).Show();           
        }
    }
    public class GPSBackgroundServiceBinder : Binder
    {
        public GPSBackgroundService Service { get { return this.LocService; } }
        protected GPSBackgroundService LocService;
        public bool IsBound { get; set; }
        public GPSBackgroundServiceBinder(GPSBackgroundService service) { this.LocService = service; }
    }
    public class GPSBackgroundServiceConnection : Java.Lang.Object, IServiceConnection
    {

        GPSBackgroundServiceBinder _binder;

        public event Action Connected;
        public GPSBackgroundServiceConnection(GPSBackgroundServiceBinder binder)
        {
            if (binder != null)
                this._binder = binder;
        }

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            GPSBackgroundServiceBinder serviceBinder = (GPSBackgroundServiceBinder)service;

            if (serviceBinder != null)
            {
                this._binder = serviceBinder;
                this._binder.IsBound = true;
                serviceBinder.Service.StartLocationUpdates();
                if (Connected != null)
                    Connected.Invoke();
            }
        }
        public void OnServiceDisconnected(ComponentName name)
        {            
            this._binder.IsBound = false;
            try
            {
                if (this._binder != null)
                {
                    this._binder.Service.StopLocationUpdate();
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
            }
        }

        private void Alert(Context context, string message)
        {
            Toast.MakeText(context, message, ToastLength.Short).Show();
        }
    }

    [BroadcastReceiver]
    internal class GPSBackgroundServiceReciever : BroadcastReceiver
    {
        public static readonly string LOCATION_UPDATED = "LOCATION_UPDATED_BACKGROUND";
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action.Equals(LOCATION_UPDATED))
            {
                if (LandingActivity.Instance != null)
                {
                    LandingActivity.Instance.UpdateUI(intent);
                }

                
                //Alert(context, string.Format("Location {0}/nAddress{1}/nRemarks{2}",
                //intent.GetStringExtra("Location"),
                //intent.GetStringExtra("Address"),
                //intent.GetStringExtra("Remarks")
                //    ));

            }

        }

        private void Alert(Context context, string message)
        {
            Toast.MakeText(context, message, ToastLength.Short).Show();
        }
    }
}