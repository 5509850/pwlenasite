using System.Collections.Generic;
using Android.App;
using Android.OS;
using Carto.Layers;
using Carto.Ui;
using Carto.Core;
using Carto.Styles;
using Carto.VectorElements;
using Carto.DataSources;
using Carto.Geometry;
using Android.Support.V4.App;
using Android.Locations;
using Android.Runtime;

namespace Pw.Lena.Slave.Droid.Screens
{
    //https://carto.com/docs/carto-engine/mobile-sdk/getting-started#xamarin-android-and-ios-implementation
    //https://5509850.carto.com/your_apps/mobile
    //https://carto.com/docs/carto-engine/mobile-sdk/getting-started#basic-map-features
    //d:\DEV\(GPS)\mobile-dotnet-samples-master\mobile-dotnet-samples-master\
    //github auth

    [Activity(Label = "MapViewActivity")]
    public class MapViewActivity : MapBaseActivity, ILocationListener, ActivityCompat.IOnRequestPermissionsResultCallback 
        //BaseActivity<MapViewModel>         
    {
        static readonly string TAG = "X:" + typeof(MapViewActivity).Name;

        List<VectorTileLayer> VectorLayers
        {
            get
            {
                List<VectorTileLayer> layers = new List<VectorTileLayer>();

                for (int i = 0; i < MapView.Layers.Count; i++)
                {
                    var layer = MapView.Layers[i];

                    if (layer is VectorTileLayer)
                    {
                        layers.Add(layer as VectorTileLayer);
                    }
                }
                return layers;
            }
        }

        LocalVectorDataSource markerSource;

        bool isMarkerAdded;

        Marker positionMarker;
        BalloonPopup positionLabel;
        protected const int RequestCode = 1;
        protected const int Marshmallow = 23;
        LocationManager manager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (((int)Build.VERSION.SdkInt) >= Marshmallow)
            {
                // Ask runtime location permission
                RequestLocationPermission();
            }
            else
            {
                // Initialize the location manager to get the current position
                InitializeLocationManager();
            }

            AddOnlineBaseLayer(CartoBaseMapStyle.CartoBasemapStyleGray);             

            markerSource = new LocalVectorDataSource(MapView.Options.BaseProjection);
            var markerLayer = new VectorLayer(markerSource);

            MapView.Layers.Add(markerLayer);          

            // Animate map to the content area
            MapPos zhukova = MapView.Options.BaseProjection.FromWgs84(new MapPos(27.5027954, 53.885053)); //longitude, latitude            
            MapView.SetFocusPos(zhukova, 1);
            MapView.SetZoom(15, 1);
        }

        void RequestLocationPermission()
        {
            string fine = Android.Manifest.Permission.AccessFineLocation;
            string coarse = Android.Manifest.Permission.AccessCoarseLocation;
            ActivityCompat.RequestPermissions(this, new string[] { fine, coarse }, RequestCode);
        }

        void InitializeLocationManager()
        {
            manager = (LocationManager)GetSystemService(LocationService);

            foreach (string provider in manager.GetProviders(true))
            {
                manager.RequestLocationUpdates(provider, 1000, 50, this);
            }
        }

        void AddMarker(string title, string subtitle, float latitude, float longitude)
        {
            // Define the location of the marker, it must be converted to base map coordinate system
            MapPos location = MapView.Options.BaseProjection.FromWgs84(new MapPos(longitude, latitude));

            // Load default market style
            MarkerStyleBuilder markerBuilder = new MarkerStyleBuilder();

            // Add the label to the Marker
            positionMarker = new Marker(location, markerBuilder.BuildStyle());

            // Define label what is shown when you click on marker, with default style
            var balloonBuilder = new BalloonPopupStyleBuilder();
            positionLabel = new BalloonPopup(positionMarker, balloonBuilder.BuildStyle(), title, subtitle);

            // Add the marker and label to the layer
            markerSource.Add(positionMarker);
            markerSource.Add(positionLabel);

            // Center the map in the current location
            MapView.FocusPos = location;

            // Zoom in the map in the current location
            MapView.Zoom = 19f;
        }

        void UpdateMarker(string myPosition, string subtitle, float latitude, float longitude)
        {
            if (!isMarkerAdded)
            {
                AddMarker(myPosition, subtitle, latitude, longitude);               
                isMarkerAdded = true;
            }
            else
            {
                positionLabel.Title = myPosition;
                positionLabel.Description = subtitle;
                positionMarker.Geometry = new PointGeometry(MapView.Options.BaseProjection.FromWgs84(new MapPos(longitude, latitude)));
            }
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();

            foreach (VectorTileLayer layer in VectorLayers)
            {
                layer.VectorTileEventListener = null;
            }
        }

        public void OnLocationChanged(Location location)
        {
            LocationFound(location);
        }

        public void OnProviderDisabled(string provider)
        {
            Alert("Location provider disabled, bro!");
        }

        public void OnProviderEnabled(string provider)
        {
            Alert("Location provider enabled... scanning for location");
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
           /////////////////////////////////////
        }

        void LocationFound(Location location)
        {
            // Add a marker in the map when a new location is found.

            string title = string.Format("Location from '{0}'", location.Provider);
            string subtitle = string.Format("lat:{0} lon:{1}", location.Latitude, location.Longitude);

            if (location.HasAccuracy)
            {
                subtitle += string.Format("\naccuracy: {0} m", location.Accuracy);
            }
            if (location.HasAltitude)
            {
                subtitle += string.Format("\naltitude {0} m", location.Altitude);
            }
            if (location.HasSpeed)
            {
                subtitle += string.Format("\nspeed: {0} m/s", location.Speed);
            }
            if (location.HasBearing)
            {
                subtitle += string.Format("\nbearing: {0}", location.Bearing);
            }

            UpdateMarker(title, subtitle, (float)location.Latitude, (float)location.Longitude);
         
        }
    }
}