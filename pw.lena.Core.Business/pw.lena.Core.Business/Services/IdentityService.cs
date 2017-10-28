using JetBrains.Annotations;
using Ninject;
using pw.lena.Core.Business.Services.Contract;
using pw.lena.Core.Business.ViewModels;
using pw.lena.Core.Data.Models;
using pw.lena.CrossCuttingConcerns.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pw.lena.Core.Business.Services
{
    internal class IdentityService : IIdentityService
    {
        //private readonly ISecureSettingsService secureSettingsService;
      //  private readonly IUserOnlineRepository userOnlineRepository;

        public IdentityService(
          //  [NotNull] ISecureSettingsService secureSettingsService,
          //  [NotNull] IUserOnlineRepository userOnlineRepository,
            [NotNull] Identity identity)
        {
            //Guard.ThrowIfNull(secureSettingsService, nameof(secureSettingsService));
            //Guard.ThrowIfNull(userOnlineRepository, nameof(userOnlineRepository));
            Guard.ThrowIfNull(identity, nameof(identity));

            //this.secureSettingsService = secureSettingsService;
            //this.userOnlineRepository = userOnlineRepository;

            Identity = identity;
        }

        public Identity Identity { get; }

        public void RefreshAsync(string login, string sessionHash, CancellationToken cancellationToken = default(CancellationToken))
        {             
            DeviceViewModel deviceViewModel = (new StandardKernel()).Get<DeviceViewModel>();
            Identity.Login = login;
            Identity.SessionHash = sessionHash ?? string.Empty;
            Identity.SessionHashExpired = false;
            Identity.deviceModel = (deviceViewModel != null)
                ? new DeviceModel
                {
                    AndroidIDmacHash = deviceViewModel.AndroidIDmacHash,
                    codeA = deviceViewModel.codeA,
                    codeB = deviceViewModel.codeB,
                    Name = deviceViewModel.Name,
                    Token = deviceViewModel.Token,
                    TypeDeviceID = deviceViewModel.TypeDeviceID
                }
                : null;

         //   secureSettingsService.Identity = Identity;
        }
    }
}
