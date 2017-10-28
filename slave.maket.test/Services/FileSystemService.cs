using PlatformAbstractions.Helpers;
using PlatformAbstractions.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace slave.maket.test.Services
{
    public class FileSystemService : IFileSystemService
    {
        public Task<string> GetPath(string dbName)
        {
            string filename = dbName + ".db3";
            var path = GetFilePath(filename);
            return Helper.Complete(path);
        }
        public Task SaveText(string filename, string text)
        {
            var filePath = GetFilePath(filename);
            System.IO.File.WriteAllText(filePath, text);
            return Helper.Complete();
        }
        public Task<string> LoadText(string filename)
        {
            var filePath = GetFilePath(filename);
            return Helper.Complete(System.IO.File.ReadAllText(filePath));
        }
        public Task<bool> ExistsFile(string filename)
        {
            string filepath = GetFilePath(filename);
            return Helper.Complete(File.Exists(filepath));
        }

        #region private methodes
        private string GetFilePath(string filename)
        {
            string docsPath = "D:\\";
            return Path.Combine(docsPath, filename);
        }
        #endregion

    }

}
