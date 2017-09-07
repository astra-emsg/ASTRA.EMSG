using System.Collections.Generic;
using System.IO;

namespace ASTRA.EMSG.Business.Services.Common
{
    public interface ILogHandlerService
    {
        void SetLogLevel(string logLevel);
        string GetLogLevel();
        List<string> GetAllLogLevel();
        bool IsLogLevelValid(string logLevel);
        Stream DownloadApplicationLog();
    }
}