using System;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Models.GIS
{
    [Serializable]
    public class InspektionsRtStrAbschnitteModel: Model
    {
        public Guid StrassenabschnittId  { get; set; }
        public int Reihenfolge { get; set; }
        public string Strassenname { get; set; }
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }
        public EigentuemerTyp Strasseneigentuemer { get; set; }
        public string StrasseneigentuemerString { get { return Strasseneigentuemer.ToString(); } set { Strasseneigentuemer = value.ParseAsEnum<EigentuemerTyp>() ?? EigentuemerTyp.Gemeinde; } }
    }
}
