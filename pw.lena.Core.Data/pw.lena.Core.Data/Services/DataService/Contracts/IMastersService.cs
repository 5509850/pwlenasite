using pw.lena.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pw.lena.Core.Data.Services.DataService.Contracts
{
    public interface IMastersService
    {
        event EventHandler<EventArgs> ListDataChanged;
        Task GetPairedMasters(DeviceModel device);
        Task<IEnumerable<Master>> GetPairedMastersRest(DeviceModel device);
        Task<IEnumerable<Master>> GetSQLPairedMasters();
        Task DeleteMasterPair(DeviceModel device, long MasterId);
        Task<bool> DeleteMasterPairRest(DeviceModel device, long MasterId);
        Task DeleteMasterPairSql(long MasterId);
        Task ClearMasterPair();
        Task SaveMastersToSql(IEnumerable<Master> responce);        
    }
}
