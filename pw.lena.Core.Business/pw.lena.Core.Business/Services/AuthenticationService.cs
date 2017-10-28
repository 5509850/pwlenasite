using JetBrains.Annotations;
using pw.lena.Core.Business.Repositories;
using pw.lena.Core.Business.Services.Contract;
using pw.lena.Core.Data.Models;
using pw.lena.CrossCuttingConcerns.Helpers;
using System;
using System.Threading.Tasks;
using WeakEvent;

namespace pw.lena.Core.Business.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly WeakEventSource<EventArgs> authenticationChangedEventSource = new WeakEventSource<EventArgs>();
        private readonly IIdentityService identityService;
        private readonly IAuthenticationRepository authenticationRepository;

        public AuthenticationService(
            [NotNull] IIdentityService identityService
            //,
            //[NotNull] IAuthenticationRepository authenticationRepository
            )
        {
            Guard.ThrowIfNull(identityService, nameof(identityService));
            //Guard.ThrowIfNull(authenticationRepository, nameof(authenticationRepository));

            this.identityService = identityService;
            //this.authenticationRepository = authenticationRepository;

            //Identity.SessionExpired += Identity_SessionExpired;
        }

        public event EventHandler<EventArgs> AuthenticationChanged
        {
            add { authenticationChangedEventSource.Subscribe(value); }
            remove { authenticationChangedEventSource.Unsubscribe(value); }
        }

        public Identity Identity => identityService.Identity;

        public async Task LoginAsync(string login, string password)
        {
            //Guard.ThrowIfEmptyString(login, nameof(login));
            //Guard.ThrowIfEmptyString(password, nameof(password));

            var sessionHash = await authenticationRepository.LoginAsync(login, password);

            identityService.RefreshAsync(login, sessionHash);

            OnAuthenticationChanged();
        }

        public async Task LogoutAsync()
        {
            //try
            //{
            //    await authenticationRepository.LogoutAsync();
            //}
            //catch (ApiReachabilityException)
            //{
            //}
            //catch (Exception ex)
            //{
            //    Insights.Report(ex, Insights.Severity.Error);
            //}
            //finally
            //{
            //    await identityService.RefreshAsync(Identity.Login, null);
            //}

            OnAuthenticationChanged();
        }

        private void Identity_SessionExpired(object sender, EventArgs e)
        {
            OnAuthenticationChanged();
        }

        private void OnAuthenticationChanged()
        {
            authenticationChangedEventSource.Raise(this, EventArgs.Empty);
        }
    }
}