using JetBrains.Annotations;
using pw.lena.Core.Business.Services.Contract;
using pw.lena.Core.Business.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pw.lena.Core.Business.Services
{
    internal class SyncService : ISyncService
    {
        //private readonly ISpendingService spendingService;
        //private readonly IAttachmentService attachmentService;

        public SyncService(
            //[NotNull] ISpendingService spendingService,
            //[NotNull] IAttachmentService attachmentService
            )
        {
            //Guard.ThrowIfNull(spendingService, nameof(spendingService));
            //Guard.ThrowIfNull(attachmentService, nameof(attachmentService));

            //this.spendingService = spendingService;
            //this.attachmentService = attachmentService;
        }

        public async Task SyncAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            //await attachmentService.ExportAsync(cancellationToken);
            //await spendingService.ImportAsync(cancellationToken);
            //await spendingService.ExportAsync(cancellationToken);
            //await attachmentService.CleanupAsync(cancellationToken);
        }
    }
}
