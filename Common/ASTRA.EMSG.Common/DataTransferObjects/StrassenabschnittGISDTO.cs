using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Common.Enums;
using GeoAPI.Geometries;

namespace ASTRA.EMSG.Common.DataTransferObjects
{
    [Serializable]
    public class StrassenabschnittGISDTO : DataTransferObject, IReferenzGruppeDTOHolder
    {
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }
        public string Strassenname { get; set; }
        public Guid Belastungskategorie { get; set; }
        public Guid InspektionsRouteId { get; set; }
        public string BelastungskategorieTyp { get; set; }
        public BelagsTyp Belag { get; set; }
        public decimal? BreiteFahrbahn { get; set; }
        public TrottoirTyp Trottoir { get; set; }
        public decimal? BreiteTrottoirLinks { get; set; }
        public decimal? BreiteTrottoirRechts { get; set; }
        public ReferenzGruppeDTO ReferenzGruppeDTO { get; set; }
        public virtual IGeometry Shape { get; set; }
        public decimal Laenge { get; set; }
        public IList<Guid> ZustandsabschnittenId { get; set; }
    }
}
