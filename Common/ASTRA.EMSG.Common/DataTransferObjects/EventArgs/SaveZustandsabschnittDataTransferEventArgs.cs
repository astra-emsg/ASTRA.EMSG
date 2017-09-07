using System;

namespace ASTRA.EMSG.Common.DataTransferObjects.EventArgs
{
    public class SaveZustandsabschnittDataTransferEventArgs : System.EventArgs
    {
        public SaveZustandsabschnittDataTransferEventArgs(Guid id, decimal zustandsindex, string geoJson)
        {
            this.Id = id;
            this.Zustandsindex = zustandsindex;
            this.GeoJson = geoJson;
        }

        public Guid Id { get; set; }
        public decimal Zustandsindex { get; set; }
        public string GeoJson { get; set; }
    }
}