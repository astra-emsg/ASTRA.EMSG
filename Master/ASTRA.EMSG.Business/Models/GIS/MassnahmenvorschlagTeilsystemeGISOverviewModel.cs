using System;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Models.GIS
{
    [Serializable]
    public class MassnahmenvorschlagTeilsystemeGISOverviewModel : Model
    {
        public string Projektname { get; set; }
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }

        public TeilsystemTyp Teilsystem { get; set; }
        public string TeilsystemBezeichnung { get; set; }

        public DringlichkeitTyp Dringlichkeit { get; set; }
        public string DringlichkeitBezeichnung { get; set; }

        public StatusTyp Status { get; set; }
        public string StatusBezeichnung { get; set; }
    }
}