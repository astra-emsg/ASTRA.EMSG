using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Reporting;
using System.Linq;
using ASTRA.EMSG.Business.Services.Benchmarking;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.BenchmarkauswertungKennwertenRealisiertenMassnahmen
{
    public interface IBenchmarkauswertungKennwertenRealisiertenMassnahmenPoProvider : IPoProvider
    {
    }

    public class BenchmarkauswertungKennwertenRealisiertenMassnahmenPoProvider : EmsgBenchmarkauswertungPoProviderBase<BenchmarkauswertungKennwertenRealisiertenMassnahmenParameter, BenchmarkauswertungKennwertenRealisiertenMassnahmenPo>, IBenchmarkauswertungKennwertenRealisiertenMassnahmenPoProvider
    {
        private const string BenchmarkauswertungRealisierteMassnahmenSubReportName = "BenchmarkauswertungRealisierteMassnahmenSubReport";
        private const string BenchmarkauswertungRealisierteMassnahmenProWertverlustSubReportName = "BenchmarkauswertungRealisierteMassnahmenProWertverlustSubReport";
        private const string BenchmarkauswertungRealisierteMassnahmenProWBWSubReportName = "BenchmarkauswertungRealisierteMassnahmenProWBWSubReport";
        private const string BenchmarkauswertungRealisierteMassnahmenProBelastungskategorieSubReportName = "BenchmarkauswertungRealisierteMassnahmenProBelastungskategorieSubReport";

        private const string SubReportDefinitionResourceName = "ASTRA.EMSG.Business.Reports.BenchmarkauswertungKennwertenRealisiertenMassnahmen.BenchmarkauswertungKennwertenRealisiertenMassnahmenSubReport.rdlc";

        private readonly IBenchmarkingGruppenService benchmarkingGruppenService;
        private readonly IKenngroessenFruehererJahreService kenngroessenFruehererJahreService;
        private readonly IBenchmarkingDataService benchmarkingDataService;
        private readonly IReportLocalizationService reportLocalizationService;
        private readonly IBelastungskategorieService belastungskategorieService;
        private readonly ISecurityService securityService;

        public BenchmarkauswertungKennwertenRealisiertenMassnahmenPoProvider(
            IBenchmarkingGruppenService benchmarkingGruppenService,
            IKenngroessenFruehererJahreService kenngroessenFruehererJahreService,
            IBenchmarkingDataService benchmarkingDataService,
            IReportLocalizationService reportLocalizationService,
            IBelastungskategorieService belastungskategorieService,
            ISecurityService securityService)
        {
            this.benchmarkingGruppenService = benchmarkingGruppenService;
            this.kenngroessenFruehererJahreService = kenngroessenFruehererJahreService;
            this.benchmarkingDataService = benchmarkingDataService;
            this.reportLocalizationService = reportLocalizationService;
            this.belastungskategorieService = belastungskategorieService;
            this.securityService = securityService;
        }

        protected override void CalculatePos(BenchmarkauswertungKennwertenRealisiertenMassnahmenParameter parameter)
        {
            DateTime bezugsJahr = GetJahrDateTime(parameter.JahrId);

            var aenlicheMandanten = benchmarkingGruppenService.GetAenlicheMandanten(bezugsJahr, parameter.BenchmarkingGruppenTypList);

            if (aenlicheMandanten.Count < 5)
                return;

            var currentMandant = securityService.GetCurrentMandant();
            var jahren = new List<DateTime>();
            jahren.AddRange(ErfassungsPeriodService.GetEntitiesBy(currentMandant).Where(ep => ep.Erfassungsjahr <= bezugsJahr && ep.IsClosed).Select(ep => ep.Erfassungsjahr).ToList());
            jahren.AddRange(kenngroessenFruehererJahreService.GetEntitiesBy(currentMandant).Where(kfj => kfj.Jahr <= bezugsJahr.Year).Select(ep => ep.Jahr).ToList().Select(j => new DateTime(j, 01, 01)));

            jahren = jahren.OrderByDescending(dt => dt).ToList();

            if (jahren.Count > 5)
                jahren = jahren.Take(5).ToList();

            var previousYears = jahren.Select(dateTime => benchmarkingDataService.GetForCurrentMandant(dateTime)).ToList();

            var @group = benchmarkingDataService.GetForMandantList(bezugsJahr, aenlicheMandanten);
            var all = benchmarkingDataService.GetForAllMandant(bezugsJahr);
            var current = benchmarkingDataService.GetForCurrentMandant(bezugsJahr);
            var l = reportLocalizationService;
            poListDictionary[BenchmarkauswertungRealisierteMassnahmenSubReportName] = new List<BenchmarkauswertungKennwertenRealisiertenMassnahmenPo>()
                    {
                        CreateBenchmarkauswertungKennwertenRealisiertenMassnahmenPo(all, @group, previousYears, current, m => m.RealisierteMassnahmenProFahrbahn, l.RealisierteFlaeche, l.EinheitKostenProFlaeche),
                        CreateBenchmarkauswertungKennwertenRealisiertenMassnahmenPo(all, @group, previousYears, current, m => m.RealisierteMassnahmenProEinwohner, l.Einwohner, l.EinheitKostenProEinwohner),
                    };
            poListDictionary[BenchmarkauswertungRealisierteMassnahmenProWertverlustSubReportName] = new List<BenchmarkauswertungKennwertenRealisiertenMassnahmenPo>
                    {
                        new BenchmarkauswertungKennwertenRealisiertenMassnahmenPo(),
                        CreateBenchmarkauswertungKennwertenRealisiertenMassnahmenPo(all, @group, previousYears, current, m => m.RealisierteMassnahmenProWertverlustNetz, "", l.EinheitProzent),
                        new BenchmarkauswertungKennwertenRealisiertenMassnahmenPo(),
                    };
            poListDictionary[BenchmarkauswertungRealisierteMassnahmenProWBWSubReportName] = new List<BenchmarkauswertungKennwertenRealisiertenMassnahmenPo>
                    {
                        new BenchmarkauswertungKennwertenRealisiertenMassnahmenPo(),
                        CreateBenchmarkauswertungKennwertenRealisiertenMassnahmenPo(all, @group, previousYears, current, m => m.RealisierteMassnahmenProWiederbeschaffungswertNetz, "", l.EinheitProzent),
                        new BenchmarkauswertungKennwertenRealisiertenMassnahmenPo(),
                    };
            poListDictionary[BenchmarkauswertungRealisierteMassnahmenProBelastungskategorieSubReportName] = new List<BenchmarkauswertungKennwertenRealisiertenMassnahmenPo>
                    (
                          belastungskategorieService.AlleBelastungskategorie.Select(b =>
                          CreateBenchmarkauswertungKennwertenRealisiertenMassnahmenPo(
                              all.SelectMany(m => m.BenchmarkingDataDetails.Where(d => d.Belastungskategorie.Id == b.Id)).ToList(),
                              @group.SelectMany(m => m.BenchmarkingDataDetails.Where(d => d.Belastungskategorie.Id == b.Id)).ToList(),
                              previousYears.SelectMany(m => m.BenchmarkingDataDetails.Where(d => d.Belastungskategorie.Id == b.Id)).ToList(),
                              current.BenchmarkingDataDetails.SingleOrDefault(d => d.Belastungskategorie.Id == b.Id) ?? new BenchmarkingDataDetail(),
                              s => s.RealisierteMassnahmenProWiederbeschaffungswertNetz, null, l.EinheitProzent, LocalizationService.GetLocalizedBelastungskategorieTyp(b.Typ, LocalizationType.Short)
                          ))
                    );
        }

        private static BenchmarkauswertungKennwertenRealisiertenMassnahmenPo CreateBenchmarkauswertungKennwertenRealisiertenMassnahmenPo<T>(
            IList<T> all,
            IList<T> @group,
            IList<T> previousYears,
            T current,
            Func<T, decimal?> selector,
            string bezugsgroesse, string einheit, string beschreibungDetails = null)
        {
            return new BenchmarkauswertungKennwertenRealisiertenMassnahmenPo
                       {
                           BeschreibungDetails = beschreibungDetails,
                           Bezugsgroesse = bezugsgroesse,
                           Einheit = einheit,
                           Organisation = selector(current) ?? 0,
                           GleitendesMittel = previousYears.Average(selector) ?? 0,
                           AnzahlGemeindenInDerGruppe = @group.Count,
                           MinimalwertInDerGruppe = @group.Min(selector) ?? 0,
                           MittelwertInDerGruppe = @group.Average(selector) ?? 0,
                           MaximalwertInDerGruppe = @group.Max(selector) ?? 0,
                           AnzahlGemeindenInAllerGemeinde = all.Count,
                           MinimalwertInAllerGemeinde = all.Min(selector) ?? 0,
                           MittelwertInAllerGemeinde = all.Average(selector) ?? 0,
                           MaximalwertInAllerGemeinde = all.Max(selector) ?? 0
                       };
        }

        protected override void AddSubReportDefinitionResourceNames(Dictionary<string, string> subReportDefinitionResourceNames)
        {
            base.AddSubReportDefinitionResourceNames(subReportDefinitionResourceNames);
            subReportDefinitionResourceNames.Add(BenchmarkauswertungRealisierteMassnahmenSubReportName, SubReportDefinitionResourceName);
            subReportDefinitionResourceNames.Add(BenchmarkauswertungRealisierteMassnahmenProWertverlustSubReportName, SubReportDefinitionResourceName);
            subReportDefinitionResourceNames.Add(BenchmarkauswertungRealisierteMassnahmenProWBWSubReportName, SubReportDefinitionResourceName);
            subReportDefinitionResourceNames.Add(BenchmarkauswertungRealisierteMassnahmenProBelastungskategorieSubReportName, SubReportDefinitionResourceName);
        }

        public override int? MaxImagePreviewPageHeight { get { return 705; } }
    }
}