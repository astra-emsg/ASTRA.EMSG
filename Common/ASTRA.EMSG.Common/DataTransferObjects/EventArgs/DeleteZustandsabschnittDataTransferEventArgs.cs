using System;

namespace ASTRA.EMSG.Common.DataTransferObjects.EventArgs
{
    public class DeleteZustandsabschnittDataTransferEventArgs : System.EventArgs
    {
        public DeleteZustandsabschnittDataTransferEventArgs(Guid id)
        {
            Id = id;
        }
        
        public Guid Id { get; set; }
    }
}