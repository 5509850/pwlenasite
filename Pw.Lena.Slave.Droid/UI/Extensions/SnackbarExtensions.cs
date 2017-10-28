using Android.Support.Design.Widget;
using System.Threading.Tasks;

namespace Pw.Lena.Slave.Droid.UI.Extensions
{
    public static class SnackbarExtensions
    {
        public static Snackbar ApplySourceSansProFont(this Snackbar snackbar)
        {
            var textView = snackbar.View.FindViewById<Android.Widget.TextView>(Resource.Id.snackbar_text);
            textView.ApplySourceSansProFont();
            textView.SetTextColor(App.Context.Resources.GetColor(Resource.Color.snackbar_text_color));

            return snackbar;
        }

        public static async Task ShowWithDelay(this Snackbar snackbar, int delay = 700)
        {
            await Task.Delay(delay);

            snackbar.Show();
        }
    }
}