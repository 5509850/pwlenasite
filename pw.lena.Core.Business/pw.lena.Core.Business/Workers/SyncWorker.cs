using JetBrains.Annotations;
using pw.lena.Core.Business.Services.Contract;
using pw.lena.Core.Data.Models.EventArgs;
using pw.lena.CrossCuttingConcerns.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace pw.lena.Core.Business.Workers
{
    internal class SyncWorker : ISyncWorker
    {
        private const int SyncTimeout = 60000;

        private readonly SemaphoreSlim mutex = new SemaphoreSlim(1, 1);

        private readonly IAuthenticationService authenticationService;
        private readonly IConnectivityService connectivityService;
        private readonly ISyncService syncService;

        private CancellationTokenSource cancelSyncTokenSource;
        private Task<bool> executeSyncCycleTask;

        public SyncWorker(
            [NotNull] IAuthenticationService authenticationService,
            [NotNull] IConnectivityService connectivityService,
            [NotNull] ISyncService syncService)
        {
            Guard.ThrowIfNull(authenticationService, nameof(authenticationService));
            Guard.ThrowIfNull(connectivityService, nameof(connectivityService));
            Guard.ThrowIfNull(syncService, nameof(syncService));

            this.authenticationService = authenticationService;
            this.connectivityService = connectivityService;
            this.syncService = syncService;

            this.authenticationService.AuthenticationChanged += AuthenticationService_AuthenticationChanged;
            this.connectivityService.ConnectivityChanged += ConnectivityService_ConnectivityChanged;
        }

        public void Run()
        {
            if (authenticationService.Identity.IsAuthenticated() &&
                !authenticationService.Identity.SessionHashExpired &&
                connectivityService.IsConnected)
            {
                Task.Run(
                    async () =>
                    {
                        if (executeSyncCycleTask != null && !executeSyncCycleTask.IsCanceled && !executeSyncCycleTask.IsFaulted)
                        {
                            //// Wait for completion of existing sync cycle
                            try
                            {
                                await executeSyncCycleTask;
                            }
                            catch (OperationCanceledException)
                            {
                                //// Cancellation was requested.
                            }
                        }

                        cancelSyncTokenSource?.Cancel();

                        await mutex.WaitAsync();

                        cancelSyncTokenSource = new CancellationTokenSource();
                        var cancellationToken = cancelSyncTokenSource.Token;

                        try
                        {
                            while (true)
                            {
                                executeSyncCycleTask = ExecuteSyncCycleAsync(cancellationToken);

                                if (!await executeSyncCycleTask)
                                {
                                    break;
                                }

                                await Task.Delay(SyncTimeout, cancellationToken);
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            //// Cancellation was requested.
                        }
                        finally
                        {
                            cancelSyncTokenSource.Dispose();
                            cancelSyncTokenSource = null;

                            mutex.Release();
                        }
                    });
            }
        }

        public async Task CompleteAsync()
        {
            if (authenticationService.Identity.IsAuthenticated() &&
                !authenticationService.Identity.SessionHashExpired &&
                connectivityService.IsConnected)
            {
                await Task.Run(
                    async () =>
                    {
                        if (executeSyncCycleTask == null || executeSyncCycleTask.IsCompleted)
                        {
                            cancelSyncTokenSource?.Cancel();

                            await mutex.WaitAsync();

                            cancelSyncTokenSource = new CancellationTokenSource();
                            var cancellationToken = cancelSyncTokenSource.Token;

                            executeSyncCycleTask = ExecuteSyncCycleAsync(cancellationToken);

                            try
                            {
                                await executeSyncCycleTask;
                            }
                            catch (OperationCanceledException)
                            {
                                //// Cancellation was requested.
                            }
                            finally
                            {
                                cancelSyncTokenSource.Dispose();
                                cancelSyncTokenSource = null;

                                mutex.Release();
                            }
                        }
                        else
                        {
                            try
                            {
                                await executeSyncCycleTask;
                            }
                            catch (OperationCanceledException)
                            {
                                //// Cancellation was requested.
                            }
                        }
                    });
            }
        }

        public void Cancel()
        {
            cancelSyncTokenSource?.Cancel();
        }

        private async Task<bool> ExecuteSyncCycleAsync(CancellationToken cancellationToken)
        {
            var success = true;

            try
            {
                await syncService.SyncAsync(cancellationToken);
            }
            //catch (ApiAuthenticationException)
            //{
            //    //// We cannot continue syncing. User should authenticate first.
            //    success = false;
            //}
            //catch (ApiReachabilityException)
            //{
            //    //// Do nothing. It might be temporary issue due to connectivity problems.
            //}
            //catch (ApiServerException ex)
            //{
            //    Insights.Report(ex, Insights.Severity.Error);

            //    //// Something went wrong on API side.
            //    //// TODO: send notification to UI
            //}
            catch (OperationCanceledException)
            {
                //// Cancellation was requested.
                success = false;
            }
            catch (Exception ex)
            {
                //// Something unexpected happend. Abandon syncing.
                //Insights.Report(ex, Insights.Severity.Critical);

                success = false;
            }

            return success;
        }

        private void AuthenticationService_AuthenticationChanged(object sender, EventArgs e)
        {
            if (authenticationService.Identity.IsAuthenticated() &&
                !authenticationService.Identity.SessionHashExpired)
            {
                Run();
            }
            else
            {
                Cancel();
            }
        }

        private void ConnectivityService_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.IsConnected)
            {
                Run();
            }
            else
            {
                Cancel();
            }
        }
    }
}
