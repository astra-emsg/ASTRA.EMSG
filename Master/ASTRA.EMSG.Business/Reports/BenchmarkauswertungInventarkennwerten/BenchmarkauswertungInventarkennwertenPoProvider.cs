using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using System.Linq;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.Benchmarking;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.BenchmarkauswertungInventarkennwerten
{
    public interface IBenchmarkauswertungInventarkennwertenPoProvider : IPoProvider
    {
    }

    public class BenchmarkauswertungInventarkennwertenPoProvider : EmsgBenchmarkauswertungPoProviderBase<BenchmarkauswertungInventarkennwertenParameter, BenchmarkauswertungInventarkennwertenPo>, IBenchmarkauswertungInventarkennwertenPoProvider
    { 
        private const string BenchmarkauswertungGesamtlaengeDesStrassenetzesSubReportName = "BenchmarkauswertungGesamtlaengeDesStrassenetzesSubReport";
        private const string BenchmarkauswertungFahrbahnflaecheSubReportName = "BenchmarkauswertungFahrbahnflaecheSubReport";
        private const string BenchmarkauswertungGesamtstrassenflaecheSubReportName = "BenchmarkauswertungGesamtstrassenflaecheSubReport";
        private const string BenchmarkauswertungAnteilJeBelastungskategorieSubReportName = "BenchmarkauswertungAnteilJeBelastungskategorieSubReport";
        private const string BenchmarkauswertungWiederbeschaffungswerSubReportName = "BenchmarkauswertungWiederbeschaffungswerSubReport";
        private const string BenchmarkauswertungWertverlustSubReportName = "BenchmarkauswertungWertverlustSubReport";

        private const string SubReportDefinitionResourceName = "ASTRA.EMSG.Business.Reports.BenchmarkauswertungInventarkennwerten.BenchmarkauswertungInventarkennwertenSubReport.rdlc";

        private readonly IBenchmarkingGruppenService benchmarkingGruppenService;
        private readonly IBenchmarkingDataService benchmarkingDataService;
        private readonly IReportLocalizationService reportLocalizationService;
        private readonly IBelastungskategorieService belastungskategorieService;

        public BenchmarkauswertungInventarkennwertenPoProvider(
            IBenchmarkingGruppenService benchmarkingGruppenService,
            IBenchmarkingDataService benchmarkingDataService,
            IReportLocalizationService reportLocalizationService,
            IBelastungskategorieService belastungskategorieService)
        {
            this.benchmarkingGruppenService = benchmarkingGruppenService;
            this.benchmarkingDataService = benchmarkingDataService;
            this.reportLocalizationService = reportLocalizationService;
            this.belastungskategorieService = belastungskategorieService;
        }
        
        protected override void CalculatePos(BenchmarkauswertungInventarkennwertenParameter parameter)
        {
            DateTime jahrDateTime = GetJahrDateTime(parameter.JahrId);

            var aenlicheMandanten = benchmarkingGruppenService.GetAenlicheMandanten(jahrDateTime, parameter.BenchmarkingGruppenTypList);

            if (aenlicheMandanten.Count < 5)
                return;

            var @group = benchmarkingDataService.GetForMandantList(jahrDateTime, aenlicheMandanten);
            var all = benchmarkingDataService.GetForAllMandant(jahrDateTime);
            var current = benchmarkingDataService.GetForCurrentMandant(jahrDateTime);
            var l = reportLocalizationService;
            poListDictionary[BenchmarkauswertungGesamtlaengeDesStrassenetzesSubReportName] = new List<BenchmarkauswertungInventarkennwertenPo>
                    {
                        CreateBenchmarkauswertungInventarkennwertenPo(all, @group, current, m => m.GesamtlaengeDesStrassennetzesProSiedlungsflaeche, l.Siedlungsflaeche, l.EinheitLaengeProFlaeche),
                        CreateBenchmarkauswertungInventarkennwertenPo(all, @group, current, m => m.GesamtlaengeDesStrassennetzesProEinwohner, l.Einwohner, l.EinheitLaengeProEinwohner),
                    };
            poListDictionary[BenchmarkauswertungFahrbahnflaecheSubReportName] = new List<BenchmarkauswertungInventarkennwertenPo>
                    {
                        CreateBenchmarkauswertungInventarkennwertenPo(all, @group, current, m => m.FahrbahnflaecheProSiedlungsflaeche, l.Siedlungsflaeche, l.EinheitFlaecheProFlaeche),
                        CreateBenchmarkauswertungInventarkennwertenPo(all, @group, current, m => m.FahrbahnflaecheProEinwohner, l.Einwohner, l.EinheitFlaecheProEinwohner),
                    };
            poListDictionary[BenchmarkauswertungGesamtstrassenflaecheSubReportName] = new List<BenchmarkauswertungInventarkennwertenPo>
                    {
                        CreateBenchmarkauswertungInventarkennwertenPo(all, @group, current, m => m.GesamtstrassenflaecheProSiedlungsflaeche, l.Siedlungsflaeche, l.EinheitProzent),
                        CreateBenchmarkauswertungInventarkennwertenPo(all, @group, current, m => m.GesamtstrassenflaecheProEinwohner, l.Einwohner, l.EinheitFlaecheProEinwohner),
                    };
            poListDictionary[BenchmarkauswertungAnteilJeBelastungskategorieSubReportName] = new List<BenchmarkauswertungInventarkennwertenPo>
                    (
                          belastungskategorieService.AlleBelastungskategorie.Select(b => 
                          CreateBenchmarkauswertungInventarkennwertenPo(
                              all.SelectMany(m => m.BenchmarkingDataDetails.Where(d => d.Belastungskategorie.Id == b.Id)).ToList(),
                              @group.SelectMany(m => m.BenchmarkingDataDetails.Where(d => d.Belastungskategorie.Id == b.Id)).ToList(),
                              current.BenchmarkingDataDetails.SingleOrDefault(d => d.Belastungskategorie.Id == b.Id) ?? new BenchmarkingDataDetail(),
                              s => s.FahrbahnflaecheAnteil, null, l.EinheitProzent, LocalizationService.GetLocalizedBelastungskategorieTyp(b.Typ, LocalizationType.Short)
                          ))
                    );
            poListDictionary[BenchmarkauswertungWiederbeschaffungswerSubReportName] = new List<BenchmarkauswertungInventarkennwertenPo>
                    {
                        CreateBenchmarkauswertungInventarkennwertenPo(all, @group, current, m => m.WiederbeschaffungswertProFahrbahn, l.Fahrbahn, l.EinheitKostenProFlaeche),
                        CreateBenchmarkauswertungInventarkennwertenPo(all, @group, current, m => m.WiederbeschaffungswertProEinwohner, l.Einwohner, l.EinheitKostenProEinwohner),
                    };
            poListDictionary[BenchmarkauswertungWertverlustSubReportName] = new List<BenchmarkauswertungInventarkennwertenPo>
                    {
                        CreateBenchmarkauswertungInventarkennwertenPo(all, @group, current, m => m.WertverlustProFahrbahn, l.Fahrbahn, l.EinheitKostenProFlaeche),
                        CreateBenchmarkauswertungInventarkennwertenPo(all, @group, current, m => m.WertverlustProEinwohner, l.Einwohner, l.EinheitKostenProEinwohner),
                    };
        }

        private static BenchmarkauswertungInventarkennwertenPo CreateBenchmarkauswertungInventarkennwertenPo<T>(
            IList<T> all,
            IList<T> @group,
            T current,
            Func<T, decimal?> selector,
            string bezugsgroesse, string einheit, string beschreibungDetails = null)
        {
            return new BenchmarkauswertungInventarkennwertenPo
                       {
                           BeschreibungDetails = beschreibungDetails,
                           Bezugsgroesse = bezugsgroesse,
                           Einheit = einheit,
                           Organisation = selector(current) ?? 0,
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
           subReportDefinitionResourceNames.Add(BenchmarkauswertungGesamtlaengeDesStrassenetzesSubReportName, SubReportDefinitionResourceName);
           subReportDefinitionResourceNames.Add(BenchmarkauswertungFahrbahnflaecheSubReportName, SubReportDefinitionResourceName );
           subReportDefinitionResourceNames.Add(BenchmarkauswertungGesamtstrassenflaecheSubReportName, SubReportDefinitionResourceName );
           subReportDefinitionResourceNames.Add(BenchmarkauswertungAnteilJeBelastungskategorieSubReportName, SubReportDefinitionResourceName  );
           subReportDefinitionResourceNames.Add(BenchmarkauswertungWiederbeschaffungswerSubReportName, SubReportDefinitionResourceName );
           subReportDefinitionResourceNames.Add(BenchmarkauswertungWertverlustSubReportName, SubReportDefinitionResourceName );
        }

        public override int? MaxImagePreviewPageHeight { get { return 700; } }
    }
}