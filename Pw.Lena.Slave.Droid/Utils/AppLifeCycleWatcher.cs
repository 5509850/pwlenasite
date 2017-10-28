using System;

using Android.App;
using Android.OS;

namespace Pw.Lena.Slave.Droid.Utils
{
    public class AppLifeCycleWatcher : Java.Lang.Object, Application.IActivityLifecycleCallbacks
    {
        private const long Delay = 500;

        private static AppLifeCycleWatcher instance;

        private bool foreground;
        private bool paused = true;
        private Handler handler = new Handler();

        private AppLifeCycleWatcher()
        {
        }

        public event EventHandler BecameBackground;

        public event EventHandler BecameForeground;

        public static AppLifeCycleWatcher Instance
        {
            get
            {
                if (instance == null)
                {
                    throw new InvalidOperationException($"Method '{nameof(Init)}' should be run before accessing to '{nameof(Instance)}' property");
                }

                return instance;
            }
        }

        public bool IsForeground => foreground;

        public bool IsBackground => !foreground;

        public static void Init(Application app)
        {
            if (instance == null)
            {
                instance = new AppLifeCycleWatcher();
                app.RegisterActivityLifecycleCallbacks(instance);
            }
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
            paused = true;

            handler.PostDelayed(
                () =>
                {
                    if (foreground && paused)
                    {
                        foreground = false;

                        BecameBackground?.Invoke(this, EventArgs.Empty);
                    }
                },
                Delay);
        }

        public void OnActivityResumed(Activity activity)
        {
            paused = false;

            bool wasBackGround = !foreground;

            foreground = true;

            if (wasBackGround)
            {
                BecameForeground?.Invoke(this, EventArgs.Empty);
            }
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
        }

        public void OnActivityStopped(Activity activity)
        {
        }
    }
}