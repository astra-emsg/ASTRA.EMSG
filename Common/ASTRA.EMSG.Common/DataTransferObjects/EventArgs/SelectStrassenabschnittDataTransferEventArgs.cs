using System;

namespace ASTRA.EMSG.Common.DataTransferObjects.EventArgs
{
    public class SelectStrassenabschnittDataTransferEventArgs : System.EventArgs
    {
        public SelectStrassenabschnittDataTransferEventArgs(Guid id)
        {
            Id = id;
        }
        
        public Guid Id { get; set; }
    }
}