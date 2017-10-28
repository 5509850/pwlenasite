using GalaSoft.MvvmLight.Command;
using Ninject;
using pw.lena.Core.Data.Models;
using pw.lena.Core.Data.Services.DataService.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pw.lena.Core.Business.ViewModels
{
    public class PairViewModel : GalaSoft.MvvmLight.ViewModelBase
    {       
        private readonly IPairDeviceService pairDeviceService;
        private readonly IMastersService mastersDeviceService;
        private string errorMessage;
        private RelayCommand getCodeACommand;
        private IKernel _kernel = new StandardKernel();
        private bool isCodeAGetSuccess;

        public PairViewModel(IPairDeviceService pairDeviceService, IMastersService mastersDeviceService)
        {
            //  Guard.ThrowIfNull(authenticationService, nameof(authenticationService));
            this.pairDeviceService = pairDeviceService;
            this.mastersDeviceService = mastersDeviceService;
            this.pairDeviceService.CodeAChanged += PairDeviceService_CodeAChanged;
            this.mastersDeviceService.ListDataChanged += MastersDeviceService_ListDataChanged;
            Initialize();
        }

        #region public methodes

        public override void Cleanup()
        {
            base.Cleanup();
            pairDeviceService.CodeAChanged -= PairDeviceService_CodeAChanged;
        }

        public RelayCommand GetCodeACommand
        {
            get
            {
                return getCodeACommand ?? (getCodeACommand = new RelayCommand(
                    async () =>
                    {
                        await pairDeviceService.GetCodeA(new DeviceModel
                        {
                            AndroidIDmacHash = deviceViewModel.AndroidIDmacHash,
                            TypeDeviceID = deviceViewModel.TypeDeviceID,
                            codeA = deviceViewModel.codeA,
                            codeB = deviceViewModel.codeB,
                            Name = deviceViewModel.Name,
                            Token = deviceViewModel.Token
                        });
                        IsCodeGetSuccess = false;
                    },
                    () => !IsCodeGetSuccess));
            }
        }

        public async void GetMasterPair()
        {
            await mastersDeviceService.GetPairedMasters(new DeviceModel
            {
                AndroidIDmacHash = deviceViewModel.AndroidIDmacHash,
                TypeDeviceID = deviceViewModel.TypeDeviceID,
                codeA = deviceViewModel.codeA,
                codeB = deviceViewModel.codeB,
                Name = deviceViewModel.Name,
                Token = deviceViewModel.Token
            });
        }
        ///for test only!!!!!!
        public async Task<Pair> GetCodeATestingOnly(DeviceModel device)
        {
            Task task = Task.Run(async () => await pairDeviceService.GetCodeA(device));
            task.Wait();
            return await pairDeviceService.GetPair();
        }

        public async Task DeleteMasterPair(long masterID)
        {
            await mastersDeviceService.DeleteMasterPair(new DeviceModel
            {
                AndroidIDmacHash = deviceViewModel.AndroidIDmacHash,
                TypeDeviceID = deviceViewModel.TypeDeviceID,
                codeA = deviceViewModel.codeA,
                codeB = deviceViewModel.codeB,
                Name = deviceViewModel.Name,
                Token = deviceViewModel.Token
            }, masterID);
        }

        #endregion

        #region private methodes
        private void MastersDeviceService_ListDataChanged(object sender, EventArgs e)
        {
            InitializeMasters();
        }

        private void PairDeviceService_CodeAChanged(object sender, EventArgs e)
        {
            Initialize();
        }

        private async void Initialize()
        {
            pair = await pairDeviceService.GetPair();
        }

        private async void InitializeMasters()
        {
            PairedMasters = await mastersDeviceService.GetSQLPairedMasters();
        }

        #endregion        

        #region private fields
        private DeviceViewModel deviceViewModel { get { return _kernel.Get<DeviceViewModel>(); }}

        private bool IsCodeGetSuccess
        {
            get
            {
                return isCodeAGetSuccess;
            }
            set
            {
                if (Set(ref isCodeAGetSuccess, value))
                {
                    GetCodeACommand.RaiseCanExecuteChanged();
                }
            }
        }
        #endregion

        #region public fields

        public Pair pair{get; private set;}

        public IEnumerable<Master> PairedMasters { get; private set; }

        #endregion
    }
}
