using pw.lena.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pw.lena.Core.Data.Services.DataService.Contracts
{
    public interface IScreenShotPCService
    {
        Task<IEnumerable<ScreenShot>> GetScreenShotRest(DeviceModel device);
        Task<IEnumerable<ScreenShot>> GetSQLScreenShot(DateTime date);
        Task<IEnumerable<ScreenShot>> GetSQLScreenShot(DateTime from, DateTime to);
        Task DeleteScreenShot(DeviceModel device);
        Task<bool> DeleteScreenShotRest(DeviceModel device);
        Task DeleteScreenShotSql();
        Task<int> SaveScreenShotToSql(ScreenShot screenShot);
        Task<int> SynchronizeScreenShotRest(DeviceModel device, DateTime date);
    }
}
