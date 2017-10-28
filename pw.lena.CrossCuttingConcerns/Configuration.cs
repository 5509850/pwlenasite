using System;
using pw.lena.CrossCuttingConcerns.Enums;
using pw.lena.CrossCuttingConcerns.Interfaces;

namespace pw.lena.CrossCuttingConcerns
{
    public class Configuration : IConfiguration
    {
        private const string server = "https://alexsoft.in";
        private const string server_debug = "http://localhost:62774";
        //"http://localhost:62774";
        //"https://alexsoft.in";       
        private const string dbName = "pw01";
        private const int serverTimeOutmilisec = 10000;
        private const int timeoutValidcodeAsecond = 120;
        private const DefaultLanguage defaultLanguage = DefaultLanguage.System;
        public string RestServerUrl
        {
            get
            {
                string _server = server;
#if DEBUG
                _server = server_debug;
#endif
                return _server;
            }
        }
        public string SqlDatabaseName
        {
            get
            {
                return dbName;
            }
        }
        public DefaultLanguage GetDefaultLanguage
        {
            get
            {
                return defaultLanguage;
            }
        }

        public int ServerTimeOut
        {
            get
            {
                return serverTimeOutmilisec;
            }
        }

        public int TimeOutValidCodeASecond
        {
            get
            {
                return timeoutValidcodeAsecond;
            }
        }
    }
}