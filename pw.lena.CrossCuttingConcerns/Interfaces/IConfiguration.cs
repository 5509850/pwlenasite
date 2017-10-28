using pw.lena.CrossCuttingConcerns.Enums;

namespace pw.lena.CrossCuttingConcerns.Interfaces
{
    public interface IConfiguration
    {
        string RestServerUrl { get; }
        string SqlDatabaseName { get; }

        int ServerTimeOut { get; }

        int TimeOutValidCodeASecond { get; }

        DefaultLanguage GetDefaultLanguage { get; }
    }
}
