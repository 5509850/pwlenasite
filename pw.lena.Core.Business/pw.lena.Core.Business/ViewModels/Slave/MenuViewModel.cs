using JetBrains.Annotations;
using pw.lena.Core.Business.Services.Contract;
using pw.lena.CrossCuttingConcerns.Helpers;

namespace pw.lena.Core.Business.ViewModels.Slave
{
    public class MenuViewModel : GalaSoft.MvvmLight.ViewModelBase
    {
        private readonly IAuthenticationService authenticationService;
        private bool isLoggedIn;
        private string syncStatus;
        private string syncStatusDescription;

        public MenuViewModel([NotNull] IAuthenticationService authenticationService)
        {
            Guard.ThrowIfNull(authenticationService, nameof(authenticationService));

            this.authenticationService = authenticationService;
            this.authenticationService.AuthenticationChanged += AuthenticationService_AuthenticationChanged;

            Initialize();
        }

        public bool IsLoggedIn
        {
            get
            {
                return isLoggedIn;
            }

            private set
            {
                Set(ref isLoggedIn, value);
            }
        }

        public string SyncStatus
        {
            get
            {
                return syncStatus;
            }

            private set
            {
                Set(ref syncStatus, value);
            }
        }

        public string SyncStatusDescription
        {
            get
            {
                return syncStatusDescription;
            }

            private set
            {
                Set(ref syncStatusDescription, value);
            }
        }

        public override void Cleanup()
        {
            base.Cleanup();

            authenticationService.AuthenticationChanged -= AuthenticationService_AuthenticationChanged;
        }

        private void Initialize()
        {
            IsLoggedIn = authenticationService.Identity.IsAuthenticated();

            if (IsLoggedIn)
            {
                SyncStatus = string.Empty;
                SyncStatusDescription = string.Empty;
            }
            else
            {
                SyncStatus = "Status";
                SyncStatusDescription = "CredentialsRequiredSyncStatusDescription";
            }
        }

        private void AuthenticationService_AuthenticationChanged(object sender, System.EventArgs e)
        {
            Initialize();
        }
    }
}
