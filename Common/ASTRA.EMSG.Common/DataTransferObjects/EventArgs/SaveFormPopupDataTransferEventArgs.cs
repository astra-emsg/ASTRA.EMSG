using System;

namespace ASTRA.EMSG.Common.DataTransferObjects.EventArgs
{
    public class SaveFormPopupDataTransferEventArgs : System.EventArgs
    {
        public SaveFormPopupDataTransferEventArgs(Guid id, decimal? zustandsindex)
        {
            Id = id;
            Zustandsindex = zustandsindex;
        }

        public Guid Id { get; set; }
        public decimal? Zustandsindex { get; set; }
    }
}