namespace ASTRA.EMSG.Business.Services.Common
{
    public interface IReportLocalizationService
    {
        string Fahrbahnflaeche { get; }
        string Fahrbahnlaenge { get; }
        string Trottoirflaeche { get; }
        string Wiederbeschaffungswert { get; }
        string WertverlustsI { get; }
        string WertverlustsII { get; }
        string FlaecheFahrbahn { get; }
        string FlaecheTrottoir { get; }
        string FlaecheTrottoirLinks { get; }
        string FlaecheTrottoirRechts { get; }
        string NetzAnteil { get; }
        string MittlererZustandsindex { get; }
        string MittleresAlterDerZustandsaufnahmen { get; }
        string GesamtFlaeche { get; }
        string WV { get; }
        string WBW { get; }
        string RealisiertenMassnahmen { get; }
        string Kosten { get; }
        string FlaecheAxis { get; }
        string WBWAxis { get; }
        
        string Einwohner { get; }
        string Siedlungsflaeche { get; }
        string Fahrbahn { get; }
        string RealisierteFlaeche { get; }

        string EinheitLaengeProFlaeche { get; }
        string EinheitLaengeProEinwohner { get; }
        string EinheitFlaecheProFlaeche { get; }
        string EinheitFlaecheProEinwohner { get; }
        string EinheitProzent { get; }
        string EinheitKostenProFlaeche { get; }
        string EinheitKostenProEinwohner { get; }
        string EinheitNichts { get; }
        string EinheitDatum { get; }
        string TausendQuadratMeter { get; }
        string MapReportFooterText { get; }
    }
}