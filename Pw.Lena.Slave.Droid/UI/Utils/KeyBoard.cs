using Android.App;
using Android.Content;
using Android.Views.InputMethods;

namespace Pw.Lena.Slave.Droid.UI.Utils
{
    public static class KeyBoard
    {
        public static void HideSoftKeyboard(Activity activity)
        {
            var inputMethodManager = (InputMethodManager)activity.GetSystemService(Context.InputMethodService);
            inputMethodManager.HideSoftInputFromWindow(activity.CurrentFocus.WindowToken, 0);
        }

        public static void ShowSoftKeyboard(Activity activity, EditText editText)
        {
            var inputMethodManager = (InputMethodManager)activity.GetSystemService(Context.InputMethodService);
            inputMethodManager.ShowSoftInput(editText, ShowFlags.Forced);
        }
    }
}