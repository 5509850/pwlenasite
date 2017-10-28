using pw.lena.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pw.lena.Core.Data.Services.DataService.Contracts
{
    public interface IPowerPcService
    {
      
            event EventHandler<EventArgs> PowerTimeChanged;
            Task GetPowerTime(DeviceModel device);
            Task<IEnumerable<PowerPC>> GetPowerTimeRest(DeviceModel device);
            Task<IEnumerable<PowerPC>> GetSQLPowerTime(DateTime date);
            Task<IEnumerable<PowerPC>> GetSQLPowerTime(DateTime from, DateTime to);
            Task DeletePowerTime(DeviceModel device);
            Task<bool> DeletePowerTimeRest(DeviceModel device);
            Task DeletePowerTimeSql();            
            Task<int> SavePowerTimeToSql(PowerPC powerpc);
            Task<int> SynchronizePowerTimeRest(DeviceModel device, DateTime date);

    }
}
