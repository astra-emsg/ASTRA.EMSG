using System;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.FilterBuilders;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.StrassenabschnitteListe
{
    public class StrassenabschnitteListeParameter : EmsgGisReportParameter, IEigentuemerFilter, ICurrentMandantFilter, IErfassungsPeriodFilter, IBelastungskategorieFilter, IOrtsbezeichnungFilter
    {
        public EigentuemerTyp? Eigentuemer { get; set; }
        public Guid? Belastungskategorie { get; set; }
        public String ExternalId { get; set; }
        public String Ortsbezeichnung { get; set; }
    }
}