using System;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Common.Exceptions
{
    public class EmsgException : Exception
    {
        /// <summary>
        /// Exception code for localization.
        /// </summary>
        public EmsgExceptionType EmsgExceptionType { get; private set; }

        public EmsgException(EmsgExceptionType emsgExceptionType) : base(emsgExceptionType.ToString())
        {
            EmsgExceptionType = emsgExceptionType;
        }
        public EmsgException(EmsgExceptionType emsgExceptionType, Exception innerException)
            : base(emsgExceptionType.ToString(), innerException)
        {
            EmsgExceptionType = emsgExceptionType;
        }
    }
}
