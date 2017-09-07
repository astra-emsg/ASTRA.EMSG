using System;

namespace ASTRA.EMSG.Business.BusinessExceptions
{
    public class ImportException : Exception
    {
        public ImportException(string message, Exception e): base(message, e)
        { }
        public ImportException(string message): base(message)
        { }
    }
}
