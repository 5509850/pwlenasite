
using Android.App;
using Android.OS;
using Android.Widget;
using Carto.Ui;
using Carto.Utils;
using Carto.Projections;
using Carto.Layers;
using Carto.PackageManager;
using Android.Graphics.Drawables;
using Pw.Lena.Slave.Droid.UI.Extensions;

namespace Pw.Lena.Slave.Droid.Screens
{
    public class MapBaseActivity : Activity
    {
        protected MapView MapView { get; set; }
        internal Projection BaseProjection { get; set; }
        protected TileLayer BaseLayer { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            MapView = new MapView(this);
            SetContentView(MapView);

            BaseProjection = MapView.Options.BaseProjection;

            Title = GetType().GetTitle();

            if (ActionBar != null)
            {
                ActionBar.SetDisplayHomeAsUpEnabled(true);
                ActionBar.SetBackgroundDrawable(new ColorDrawable { Color = Android.Graphics.Color.Blue });
                ActionBar.Subtitle = GetType().GetDescription();
            }
        }

        protected Carto.Graphics.Bitmap CreateBitmap(int resource)
        {
            return BitmapUtils.CreateBitmapFromAndroidBitmap(Android.Graphics.BitmapFactory.DecodeResource(Resources, resource));
        }

        protected void AddOnlineBaseLayer(CartoBaseMapStyle style)
        {
            // Initialize map
            var baseLayer = new CartoOnlineVectorTileLayer(style);
            MapView.Layers.Add(baseLayer);
        }

        protected void AddOfflineBaseLayer(CartoPackageManager manager, CartoBaseMapStyle style)
        {
            // Initialize map
            var baseLayer = new CartoOfflineVectorTileLayer(manager, style);
            MapView.Layers.Add(baseLayer);
        }

        protected void Alert(string message)
        {
            RunOnUiThread(delegate
            {
                Toast.MakeText(this, message, ToastLength.Short).Show();
            });
        }
    }
}