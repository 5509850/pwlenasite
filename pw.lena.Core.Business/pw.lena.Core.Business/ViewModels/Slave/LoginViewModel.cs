using GalaSoft.MvvmLight.Command;
using JetBrains.Annotations;
using pw.lena.Core.Business.Services.Contract;
using pw.lena.CrossCuttingConcerns.Helpers;
using pw.lena.CrossCuttingConcerns.Interfaces;
using System;

namespace pw.lena.Core.Business.ViewModels.Slave
{
    //for PIN enter when secure start app and get hash keys from service
    public class LoginViewModel : GalaSoft.MvvmLight.ViewModelBase
    {
        private readonly IAuthenticationService authenticationService;

        private string login;
        private string password;
        private string errorMessage;
        private bool isLoggedIn;
        private bool isLoggingIn;
        private bool allowChangeLogin;
        private RelayCommand<string> setLoginCommand;
        private RelayCommand loginCommand;

        private ILocalizeService localizservice;

        public LoginViewModel([NotNull] IAuthenticationService authenticationService, 
            [NotNull] ILocalizeService localizservice)
        {
            this.localizservice = localizservice;
            Guard.ThrowIfNull(authenticationService, nameof(authenticationService));

            this.authenticationService = authenticationService;
            this.authenticationService.AuthenticationChanged += AuthenticationService_AuthenticationChanged;

            Initialize();
        }

        public string Login
        {
            get
            {
                return login;
            }

            private set
            {
                if (Set(ref login, value))
                {
                    SetLoginCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                Set(ref password, value);
            }
        }

        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }

            private set
            {
                Set(ref errorMessage, value);
            }
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

        private bool AllowChangeLogin
        {
            get
            {
                return allowChangeLogin;
            }

            set
            {
                if (Set(ref allowChangeLogin, value))
                {
                    SetLoginCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private bool IsLoggingIn
        {
            get
            {
                return isLoggingIn;
            }

            set
            {
                if (Set(ref isLoggingIn, value))
                {
                    LoginCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand<string> SetLoginCommand
        {
            get
            {
                return setLoginCommand ?? (setLoginCommand = new RelayCommand<string>(
                    newLogin => Login = newLogin,
                    newLogin => AllowChangeLogin));
            }
        }

        public RelayCommand LoginCommand
        {
            get
            {
                return loginCommand ?? (loginCommand = new RelayCommand(
                    async () =>
                    {
                        IsLoggingIn = true;

                        if (Validate())
                        {
                            try
                            {
                                await authenticationService.LoginAsync(Login, Password);
                            }
                            catch (Exception ex)
                            {
                                ErrorMessage = ex.Message;
                            }
                        }

                        IsLoggingIn = false;
                    },
                    () => !IsLoggingIn));
            }
        }

        public override void Cleanup()
        {
            base.Cleanup();

            authenticationService.AuthenticationChanged -= AuthenticationService_AuthenticationChanged;
        }

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = localizservice.Localize("EnterCredentials");

                return false;
            }
            else
            {
                ErrorMessage = string.Empty;

                return true;
            }
        }

        private void Initialize()
        {
            Login = authenticationService.Identity.Login;
            AllowChangeLogin = string.IsNullOrEmpty(Login);
            IsLoggedIn = authenticationService.Identity.IsAuthenticated();
            Password = string.Empty;
            ErrorMessage = string.Empty;
        }

        private void AuthenticationService_AuthenticationChanged(object sender, System.EventArgs e)
        {
            Initialize();
        }
    }
}
