using System.Threading.Tasks;

namespace pw.lena.Core.Business.ViewModels.Slave
{
    public class LandingViewModel : GalaSoft.MvvmLight.ViewModelBase
    {
        //private readonly SemaphoreSlim spendingMutex = new SemaphoreSlim(1, 1);
        //private readonly SemaphoreSlim attachmentMutex = new SemaphoreSlim(1, 1);

        //private readonly ISpendingService spendingService;
        //private readonly IAttachmentService attachmentService;
        //private readonly IDispatcherAdapter dispatcher;

        private int spendingCount;
        private int takeCareSpendingCount;
        private int attachmentCount;
        private int takeCareAttachmentCount;

        //private CancellationTokenSource spendingCancellationTokenSource;
        //private CancellationTokenSource attachmentCancellationTokenSource;

        public LandingViewModel(
            //[NotNull] ISpendingService spendingService,
            //[NotNull] IAttachmentService attachmentService,
            //[NotNull] IDispatcherAdapter dispatcher
            )
        {
            //Guard.ThrowIfNull(spendingService, nameof(spendingService));
            //Guard.ThrowIfNull(attachmentService, nameof(attachmentService));
            //Guard.ThrowIfNull(dispatcher, nameof(dispatcher));

            //this.spendingService = spendingService;
            //this.attachmentService = attachmentService;
            //this.dispatcher = dispatcher;
        }

        public int SpendingCount
        {
            get
            {
                return spendingCount;
            }

            private set
            {
                Set(ref spendingCount, value);
            }
        }

        public int TakeCareSpendingCount
        {
            get
            {
                return takeCareSpendingCount;
            }

            private set
            {
                Set(ref takeCareSpendingCount, value);
            }
        }

        public int AttachmentCount
        {
            get
            {
                return attachmentCount;
            }

            private set
            {
                Set(ref attachmentCount, value);
            }
        }

        public int TakeCareAttachmentCount
        {
            get
            {
                return takeCareAttachmentCount;
            }

            private set
            {
                Set(ref takeCareAttachmentCount, value);
            }
        }

        public void RegisterSyncNotifications()
        {
            //Messenger.Default.Register<SpendingSyncMessage>(
            //    this,
            //    MessagingChannel.SpendingPulled,
            //    message => dispatcher.CheckBeginInvokeOnUI(async () => await RefreshSpendingStatisticAsync()));

            //Messenger.Default.Register<SpendingSyncMessage>(
            //    this,
            //    MessagingChannel.SpendingPushed,
            //    message => dispatcher.CheckBeginInvokeOnUI(async () => await RefreshSpendingStatisticAsync()));

            //Messenger.Default.Register<AttachmentSyncMessage>(
            //    this,
            //    MessagingChannel.AttachmentPushed,
            //    message => dispatcher.CheckBeginInvokeOnUI(async () => await RefreshAttachmentStatisticAsync()));

            //RefreshSpendingStatisticAsync().Forget();
            //RefreshAttachmentStatisticAsync().Forget();
        }

        public void UnregisterSyncNotifications()
        {
            //Messenger.Default.Unregister<SpendingSyncMessage>(this, MessagingChannel.SpendingPulled);
            //Messenger.Default.Unregister<SpendingSyncMessage>(this, MessagingChannel.SpendingPushed);
            //Messenger.Default.Unregister<AttachmentSyncMessage>(this, MessagingChannel.AttachmentPushed);
        }

        private async Task RefreshSpendingStatisticAsync()
        {
            //spendingCancellationTokenSource?.Cancel();

            //await spendingMutex.WaitAsync();

            //spendingCancellationTokenSource = new CancellationTokenSource();
            //var cancellationToken = spendingCancellationTokenSource.Token;

            //try
            //{
            //    SpendingCount = await spendingService.GetCountAsync(cancellationToken);
            //    TakeCareSpendingCount = await spendingService.GetTakeCareCountAsync(cancellationToken);
            //}
            //catch (OperationCanceledException)
            //{
            //    //// Cancellation was requested.
            //}
            //finally
            //{
            //    spendingCancellationTokenSource.Dispose();
            //    spendingCancellationTokenSource = null;

            //    spendingMutex.Release();
            //}
        }

        private async Task RefreshAttachmentStatisticAsync()
        {
            /*
            attachmentCancellationTokenSource?.Cancel();

            await attachmentMutex.WaitAsync();

            attachmentCancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = attachmentCancellationTokenSource.Token;

            try
            {
                AttachmentCount = await attachmentService.GetNonLinkedCountAsync(cancellationToken);
                TakeCareAttachmentCount = await attachmentService.GetTakeCareCountAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                //// Cancellation was requested.
            }
            finally
            {
                attachmentCancellationTokenSource.Dispose();
                attachmentCancellationTokenSource = null;

                attachmentMutex.Release();
            }
            */
        }
    }
}