using System;

namespace ASTRA.EMSG.Common.DataTransferObjects.EventArgs
{
    public class CreateZustandsabschnittDataTransferEventArgs : System.EventArgs
    {
        public CreateZustandsabschnittDataTransferEventArgs(Guid id, Guid strassenabschnittID)
        {
            Id = id;
            StrassenabschnittId = strassenabschnittID;
        }

        public Guid Id { get; set; }
        public Guid StrassenabschnittId { get; set; }
    }
}