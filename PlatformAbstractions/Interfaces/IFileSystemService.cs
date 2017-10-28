using System.Threading.Tasks;

namespace PlatformAbstractions.Interfaces
{
    public interface IFileSystemService
    {
        Task<string> GetPath(string dbName);
        Task SaveText(string filename, string text);
        Task<string> LoadText(string filename);
        Task<bool> ExistsFile(string filename);
    }
}
