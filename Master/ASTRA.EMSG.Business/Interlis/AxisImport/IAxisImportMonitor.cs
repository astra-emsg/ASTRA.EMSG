using System;
namespace ASTRA.EMSG.Business.Interlis.AxisImport
{
    public interface IAxisImportMonitor
    {
        bool IsCancelled();
        void WriteLog(string text);
    }
}
