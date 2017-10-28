using System.Threading.Tasks;

namespace pw.lena.Core.Business.Workers
{
    public interface ISyncWorker
    {
        void Run();

        Task CompleteAsync();

        void Cancel();
    }
}
