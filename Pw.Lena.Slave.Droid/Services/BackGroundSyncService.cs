using System;
using pw.lena.CrossCuttingConcerns.Interfaces;
using Pw.Lena.Slave.Droid.Ninject;
using pw.lena.Core.Business.Utils.Extensions;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;

namespace Pw.Lena.Slave.Droid.Services
{
    [Service]
    [IntentFilter(new String[] { "pw.lena.DroidSyncService" })]
    public class BackGroundSyncService : Service
    {
        private ISyncWorker syncWorker;

        public static ViewModelLocator ViewModelLocator { get; } = new ViewModelLocator();

        public enum Action
        {
            Cancel,

            Complete,

            Run
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnCreate()
        {
            base.OnCreate();

            syncWorker = FactorySingleton.Factory.Get<ISyncWorker>();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            if (intent != null)
            {
                DoWork(intent);
            }

            return base.OnStartCommand(intent, flags, startId);
        }

        private void DoWork(Intent intent)
        {
            var actionIndex = intent.GetIntExtra(nameof(Action), -1);
            var sendedActionIsDefined = Enum.IsDefined(typeof(Action), actionIndex);

            if (!sendedActionIsDefined)
            {
                throw new OverflowException($"Unable to convert '{actionIndex}' to '{nameof(Action)}' enum.");
            }

            var action = (Action)actionIndex;

            switch (action)
            {
                case Action.Cancel:
                    ExecuteCancel();
                    break;
                case Action.Complete:
                    ExecuteComplete();
                    break;
                case Action.Run:
                    ExecuteRun();
                    break;
                default:
                    throw new ArgumentException($"Unable to process '{action}' action.");
            }
        }

        private void ExecuteRun()
        {
            syncWorker.Run();
        }

        private void ExecuteComplete()
        {
            syncWorker.CompleteAsync().Forget();
        }

        private void ExecuteCancel()
        {
            syncWorker.Cancel();

            StopSelf();
        }
    }
}