using pw.lena.Core.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pw.lena.Core.Data.Services.DataService.Contracts
{
    public interface ICommandService
    {
        Task<IEnumerable<Command>> GetCommandRest(DeviceModel device);
    }
}
