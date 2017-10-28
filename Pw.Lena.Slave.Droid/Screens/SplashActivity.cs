
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;

namespace Pw.Lena.Slave.Droid.Screens
{   
    [Activity(
        MainLauncher = true,
        Theme = "@style/AppTheme.Splash",
        NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var entryViewModel = App.ViewModelLocator.CreateEntryViewModel();           
            StartActivity(entryViewModel.HasSecurityPinSet ? typeof(LandingActivity) : typeof(GuardLoginActivity));
        }
    }
}