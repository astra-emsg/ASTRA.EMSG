using System;

namespace ASTRA.EMSG.Common.DataTransferObjects.EventArgs
{
    public class EditZustandsabschnittDataTransferEventArgs : System.EventArgs
    {
        public string  GeoJson { get; set; }
        public decimal? Length { get; set; }
    }
}