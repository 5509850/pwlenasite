using GalaSoft.MvvmLight.Command;
using JetBrains.Annotations;
using pw.lena.Core.Data.Models.Enums;
using pw.lena.Core.Data.Services.DataService.Contracts;
using pw.lena.CrossCuttingConcerns.Helpers;
using System.Threading.Tasks;
using pw.lena.Core.Business.Services.Contract;
using System;

namespace pw.lena.Core.Business.ViewModels.Slave
{
    public class EntryViewModel : GalaSoft.MvvmLight.ViewModelBase
    {
        private readonly IPreferenceService preferenceService;
        private readonly IDeviceAction deviceAction;
        private string errorMessage;
        private string titletext;
        private bool hasSecurityPinSet;
        private RelayCommand<string> setDigitCommand;
        private RelayCommand backSpaceCommand;
        private RelayCommand clearPinCommand;
        private bool isLoggingIn;
        private bool reEnterPin;
        private int errorcount;

        private string pin;

        public EntryViewModel(
            [NotNull] IPreferenceService preferenceService
            ,[NotNull] IDeviceAction deviceAction
            )
        {            
            Guard.ThrowIfNull(preferenceService, nameof(preferenceService));
            this.preferenceService = preferenceService;
            this.deviceAction = deviceAction;
            Initialize();          
        }

        private async void Initialize()
        {
            // await preferenceService.ClearPreference(); ///TODO: need delete clear all preference
            await GetHash();
            Pin = string.Empty;                                   
            ErrorMessage = string.Empty;
            IsLoggingIn = false;
            errorcount = 0;
            if (HasSecurityPinSet)
            {
                TitleText = "Enter PIN";
                reEnterPin = false;
            }
            else
            {              
                TitleText = "Create PIN";
                reEnterPin = true;
            }
        }

        public string TitleText
        {
            get
            {
                return titletext;
            }
            private set
            {
                Set(ref titletext, value);
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

        public bool HasSecurityPinSet
        {
            get
            {
                return hasSecurityPinSet;
            }

            private set
            {
                Set(ref hasSecurityPinSet, value);
            }
        }

        public string Pin
        {
            get
            {
                return pin;
            }

            private set
            {
                if (Set(ref pin, value))
                {
                    if (Validate())
                    {
                        if (HasSecurityPinSet)
                        {
                            CheckValidPin();
                        }
                        else
                        {
                            SavePin();
                        }                        
                    }
                    SetDigitCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private async void SavePin()
        {
            if (preferenceService != null)
            {
                var result = await preferenceService.SavePrefValue(PrefEnums.PinSecurityHash, Pin.GetHashCode().ToString());
                await GetHash();
                Pin = string.Empty;
                ErrorMessage = string.Empty;
                if (HasSecurityPinSet && result)
                {
                    TitleText = "Re-enter PIN";
                }
                else
                {
                    ErrorMessage = "Error saving pin";
                    TitleText = "Create PIN";
                }
            }
        }

        private async void CheckValidPin()
        {
            try
            {
                string savedhash = await preferenceService.GetPrefValue(PrefEnums.PinSecurityHash);
                string pinhash = Pin.GetHashCode().ToString();

                if (savedhash.Equals(pinhash))
                {
                    IsLoggingIn = true;
                }
                else
                {
                    if (reEnterPin)
                    {
                        ErrorMessage = "PIN do not match!";
                        errorcount++;
                        if (errorcount >= 3)
                        {
                            await preferenceService.ClearPreference();
                            await GetHash();                           
                            ErrorMessage = string.Empty;                            
                            TitleText = "Create a new PIN";
                            errorcount = 0;
                        }
                    }
                    else
                    {
                        ErrorMessage = "PIN number is not valid";
                    }                    
                    Pin = string.Empty;
                    deviceAction.VibrateNow(500);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            
        }

        public bool IsLoggingIn
        {
            get
            {
                return isLoggingIn;
            }

            set
            {
                if (Set(ref isLoggingIn, value))
                {
                    SetDigitCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand<string> SetDigitCommand
        {
            get
            {
                return setDigitCommand ?? (setDigitCommand = new RelayCommand<string>(
                    newPin => Pin += newPin));                
            }
        }

        public RelayCommand BackSpaceCommand
        {
            get
            {
                return backSpaceCommand ?? (backSpaceCommand = new RelayCommand(
                   () => {
                       if (!string.IsNullOrEmpty(Pin))
                       {
                           Pin = Pin.Remove(Pin.Length - 1);
                       }
                   }));
            }
        }

        public RelayCommand ClearPinCommand
        {
            get
            {
                return clearPinCommand ?? (clearPinCommand = new RelayCommand(
                   () => {
                       if (!string.IsNullOrEmpty(Pin))
                       {
                           Pin = string.Empty;
                       }
                   }));
            }
        }

        private bool Validate()
        {
            if (Pin.Length > 4)
            {
                Pin = Pin.Remove(Pin.Length - 1);
                return false;
            }
            if (Pin.Length != 4 && Pin.Length != 0)
            {
                ErrorMessage = string.Empty;
            }
            
            return !string.IsNullOrEmpty(Pin) && Pin.Length.Equals(4);
        }

        private async Task GetHash()
        {
            string hash = await preferenceService.GetPrefValue(PrefEnums.PinSecurityHash);          
            HasSecurityPinSet = !string.IsNullOrEmpty(hash);
        }
    }
}