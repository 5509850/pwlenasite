using SQLite.Net.Interop;
using SQLite.Net.Platform.Win32;

namespace slave.maket.test.Services
{
    public class SQLitePlatformDesktop : ISQLitePlatform
    {
        private SQLitePlatformWin32 _sqlitePlatformWin32;
        public SQLitePlatformDesktop()
        {
            _sqlitePlatformWin32 = new SQLitePlatformWin32();
        }

        public IReflectionService ReflectionService { get { return _sqlitePlatformWin32.ReflectionService; } }
        public ISQLiteApi SQLiteApi { get { return _sqlitePlatformWin32.SQLiteApi; } }
        public IStopwatchFactory StopwatchFactory { get { return _sqlitePlatformWin32.StopwatchFactory; } }
        public IVolatileService VolatileService { get { return _sqlitePlatformWin32.VolatileService; } }
    }
}
