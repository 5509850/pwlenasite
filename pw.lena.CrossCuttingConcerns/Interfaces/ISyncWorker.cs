using System.Threading.Tasks;

namespace pw.lena.CrossCuttingConcerns.Interfaces
{
    public interface ISyncWorker
    {
        void Run();

        Task CompleteAsync();

        void Cancel();
    }
}