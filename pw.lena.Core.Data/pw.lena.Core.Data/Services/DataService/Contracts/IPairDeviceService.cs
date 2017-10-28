using pw.lena.Core.Data.Models;
using System;
using System.Threading.Tasks;

namespace pw.lena.Core.Data.Services.DataService.Contracts
{
    public interface IPairDeviceService
    {
        event EventHandler<EventArgs> CodeAChanged;
        Task GetCodeA(DeviceModel device);        
        Task<CodeResponce> SetCodeB(DeviceModel device);
        Task<Pair> GetPair();
        Task UpdateOrCreateCodeAToSql(CodeResponce codeResponce);
        Task DeletePair();        

        //Task<string> SendAnswer(string code, DeviceModel device);
        //Task<string> SendInfo(DeviceModel device);
    }
}
