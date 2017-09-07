using ASTRA.EMSG.Business.Services.Common;
using Resources;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public class ReportLocalizationService : LocalizationServiceBase, IReportLocalizationService
    {
        public string Fahrbahnflaeche { get { return GetLocalization(() => ReportLocalization.Fahrbahnflaeche); } }
        public string Fahrbahnlaenge { get { return GetLocalization(() => ReportLocalization.Fahrbahnlaenge); } }
        public string Trottoirflaeche { get { return GetLocalization(() => ReportLocalization.Trottoirflaeche); } }

        public string Wiederbeschaffungswert { get { return GetLocalization(() => ReportLocalization.Wiederbeschaffungswert); } }
        public string WertverlustsI { get { return GetLocalization(() => ReportLocalization.WertverlustsI); } }
        public string WertverlustsII { get { return GetLocalization(() => ReportLocalization.WertverlustsII); } }

        public string FlaecheFahrbahn { get { return GetLocalization(() => ReportLocalization.FlaecheFahrbahn); } }
        public string FlaecheTrottoirLinks { get { return GetLocalization(() => ReportLocalization.FlaecheTrottoirLinks); } }
        public string FlaecheTrottoirRechts { get { return GetLocalization(() => ReportLocalization.FlaecheTrottoirRechts); } }

        public string NetzAnteil { get { return GetLocalization(() => ReportLocalization.NetzAnteil); } }
        public string MittlererZustandsindex { get { return GetLocalization(() => ReportLocalization.MittlererZustandsindex); } }
        public string MittleresAlterDerZustandsaufnahmen { get { return GetLocalization(() => ReportLocalization.MittleresAlterDerZustandsaufnahmen); } }

        public string FlaecheTrottoir { get { return GetLocalization(() => ReportLocalization.FlaecheTrottoir); } }

        public string GesamtFlaeche { get { return GetLocalization(() => ReportLocalization.GesamtFlaeche); } }
        public string WV { get { return GetLocalization(() => ReportLocalization.WV); } }
        public string WBW { get { return GetLocalization(() => ReportLocalization.WBW); } }
        public string RealisiertenMassnahmen { get { return GetLocalization(() => ReportLocalization.RealisiertenMassnahmen); } }
        public string Kosten { get { return GetLocalization(() => ReportLocalization.Kosten); } }

        public string FlaecheAxis { get { return GetLocalization(() => ReportLocalization.FlaecheAxis); } }
        public string WBWAxis { get { return GetLocalization(() => ReportLocalization.WBWAxis); } }

        public string Einwohner { get { return GetLocalization(() => ReportLocalization.Einwohner); } }
        public string Siedlungsflaeche { get { return GetLocalization(() => ReportLocalization.Siedlungsflaeche); } }
        public string Fahrbahn { get { return GetLocalization(() => ReportLocalization.Fahrbahn); } }
        public string RealisierteFlaeche { get { return GetLocalization(() => ReportLocalization.RealisierteFlaeche); } }

        public string EinheitLaengeProFlaeche { get { return GetLocalization(() => ReportLocalization.EinheitLaengeProFlaeche); } }
        public string EinheitLaengeProEinwohner { get { return GetLocalization(() => ReportLocalization.EinheitLaengeProEinwohner); } }
        public string EinheitFlaecheProFlaeche { get { return GetLocalization(() => ReportLocalization.EinheitFlaecheProFlaeche); } }
        public string EinheitFlaecheProEinwohner { get { return GetLocalization(() => ReportLocalization.EinheitFlaecheProEinwohner); } }
        public string EinheitProzent { get { return GetLocalization(() => ReportLocalization.EinheitProzent); } }
        public string EinheitKostenProFlaeche { get { return GetLocalization(() => ReportLocalization.EinheitKostenProFlaeche); } }
        public string EinheitKostenProEinwohner { get { return GetLocalization(() => ReportLocalization.EinheitKostenProEinwohner); } }
        public string EinheitNichts { get { return GetLocalization(() => ReportLocalization.EinheitNichts); } }
        public string EinheitDatum { get { return GetLocalization(() => ReportLocalization.EinheitDatum); } }
        public string TausendQuadratMeter { get { return GetLocalization(() => ReportLocalization.TausendQuadratMeter); } }
        public string MapReportFooterText { get { return GetLocalization(() => ReportLocalization.MapReportFooterText); } }
    }
}