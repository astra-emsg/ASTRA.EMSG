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
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.BenchmarkauswertungZustandskennwerten
{
    public interface IBenchmarkauswertungZustandskennwertenPoProvider : IPoProvider
    {
    }

    public class BenchmarkauswertungZustandskennwertenPoProvider : EmsgBenchmarkauswertungPoProviderBase<BenchmarkauswertungZustandskennwertenParameter, BenchmarkauswertungZustandskennwertenPo>, IBenchmarkauswertungZustandskennwertenPoProvider
    {
        private const string BenchmarkauswertungZustandsindexNetzSubReportName = "BenchmarkauswertungZustandsindexNetzSubReport";
        private const string BenchmarkauswertungZustandsindexJeBelastungskategorieSubReportName = "BenchmarkauswertungZustandsindexJeBelastungskategorieSubReport";
        private const string BenchmarkauswertungMittleresAlterDerZustandsaufnahmenNetzSubReportName = "BenchmarkauswertungMittleresAlterDerZustandsaufnahmenNetzSubReport";

        private const string SubReportDefinitionResourceName = "ASTRA.EMSG.Business.Reports.BenchmarkauswertungZustandskennwerten.BenchmarkauswertungZustandskennwertenSubReport.rdlc";

        private readonly IBenchmarkingGruppenService benchmarkingGruppenService;
        private readonly IKenngroessenFruehererJahreService kenngroessenFruehererJahreService;
        private readonly IBenchmarkingDataService benchmarkingDataService;
        private readonly IReportLocalizationService reportLocalizationService;
        private readonly IBelastungskategorieService belastungskategorieService;

        public BenchmarkauswertungZustandskennwertenPoProvider(
            IBenchmarkingGruppenService benchmarkingGruppenService,
            IKenngroessenFruehererJahreService kenngroessenFruehererJahreService,
            IBenchmarkingDataService benchmarkingDataService,
            IReportLocalizationService reportLocalizationService,
            IBelastungskategorieService belastungskategorieService)
        {
            this.benchmarkingGruppenService = benchmarkingGruppenService;
            this.kenngroessenFruehererJahreService = kenngroessenFruehererJahreService;
            this.benchmarkingDataService = benchmarkingDataService;
            this.reportLocalizationService = reportLocalizationService;
            this.belastungskategorieService = belastungskategorieService;
        }

        protected override void CalculatePos(BenchmarkauswertungZustandskennwertenParameter parameter)
        {
            DateTime jahrDateTime = GetJahrDateTime(parameter.JahrId);

            var aenlicheMandanten = benchmarkingGruppenService.GetAenlicheMandanten(jahrDateTime, parameter.BenchmarkingGruppenTypList);

            if (aenlicheMandanten.Count < 5)
                return;

            var @group = benchmarkingDataService.GetForMandantList(jahrDateTime, aenlicheMandanten).Where(bd => bd.MittleresAlterDerZustandsaufnahmenNetz.HasValue && bd.MittleresAlterDerZustandsaufnahmenNetz >= jahrDateTime.AddYears(-5)).ToList();
            var all = benchmarkingDataService.GetForAllMandant(jahrDateTime).Where(bd => bd.MittleresAlterDerZustandsaufnahmenNetz.HasValue && bd.MittleresAlterDerZustandsaufnahmenNetz >= jahrDateTime.AddYears(-5)).ToList();

            if (@group.Count < 5)
                return;

            var curentBenchmarkingData = benchmarkingDataService.GetForCurrentMandant(jahrDateTime);
            var current = (curentBenchmarkingData.MittleresAlterDerZustandsaufnahmenNetz.HasValue && curentBenchmarkingData.MittleresAlterDerZustandsaufnahmenNetz >= jahrDateTime.AddYears(-5)) ? curentBenchmarkingData : new BenchmarkingData();
            var l = reportLocalizationService;
            poListDictionary[BenchmarkauswertungZustandsindexNetzSubReportName] = new List<BenchmarkauswertungZustandskennwertenPo>
                    {
                        CreateBenchmarkauswertungZustandskennwertenPo(
                            all.Where(bd => bd.ZustandsindexNetz.HasValue).ToList(), 
                            @group.Where(bd => bd.ZustandsindexNetz.HasValue).ToList(), 
                            current, 
                            m => m.ZustandsindexNetz, 
                            l.EinheitNichts, 
                            d => string.Format("{0:0.00}", d)),
                    };
            poListDictionary[BenchmarkauswertungZustandsindexJeBelastungskategorieSubReportName] = new List<BenchmarkauswertungZustandskennwertenPo>
                    (
                          belastungskategorieService.AlleBelastungskategorie.Select(b =>
                          CreateBenchmarkauswertungZustandskennwertenPo(
                              all.SelectMany(bd => bd.BenchmarkingDataDetails.Where(d => d.Belastungskategorie.Id == b.Id)).Where(bdd => bdd.Zustandsindex.HasValue).ToList(),
                              @group.SelectMany(bd => bd.BenchmarkingDataDetails.Where(d => d.Belastungskategorie.Id == b.Id)).Where(bdd => bdd.Zustandsindex.HasValue).ToList(),
                              current.BenchmarkingDataDetails.SingleOrDefault(d => d.Belastungskategorie.Id == b.Id) ?? new BenchmarkingDataDetail(),
                              bd => bd.Zustandsindex,
                              l.EinheitNichts,
                              d => string.Format("{0:0.00}", d),
                              LocalizationService.GetLocalizedBelastungskategorieTyp(b.Typ, LocalizationType.Short)
                              )
                            )
                    );
            poListDictionary[BenchmarkauswertungMittleresAlterDerZustandsaufnahmenNetzSubReportName] = new List<BenchmarkauswertungZustandskennwertenPo>
                    {
                            new BenchmarkauswertungZustandskennwertenPo(),
                            CreateBenchmarkauswertungZustandskennwertenPo(
                                all.Where(bd => bd.MittleresAlterDerZustandsaufnahmenNetz.HasValue).ToList(),
                                @group.Where(bd => bd.MittleresAlterDerZustandsaufnahmenNetz.HasValue).ToList(), 
                                current, 
                                bd => bd.MittleresAlterDerZustandsaufnahmenNetz.HasValue ? bd.MittleresAlterDerZustandsaufnahmenNetz.Value.Ticks : (decimal?) null,
                                l.EinheitDatum, 
                                d => d.HasValue ? DateTime.FromBinary((long)d).ToString("d") : string.Empty),
                            new BenchmarkauswertungZustandskennwertenPo(),
                    };
        }

        private static BenchmarkauswertungZustandskennwertenPo CreateBenchmarkauswertungZustandskennwertenPo<T>(
            IList<T> all,
            IList<T> @group,
            T current,
            Func<T, decimal?> selector, string einheit, Func<decimal?, string> formatter, string beschreibungDetails = null)
        {
            return new BenchmarkauswertungZustandskennwertenPo
                       {
                           BeschreibungDetails = beschreibungDetails,
                           Einheit = einheit,
                           Organisation = formatter(selector(current)),
                           AnzahlGemeindenInDerGruppe = @group.Count.ToString("0"),
                           MinimalwertInDerGruppe = formatter(@group.Min(selector)),
                           MittelwertInDerGruppe = formatter(@group.Average(selector)),
                           MaximalwertInDerGruppe = formatter(@group.Max(selector)),
                           AnzahlGemeindenInAllerGemeinde = all.Count.ToString("0"),
                           MinimalwertInAllerGemeinde = formatter(all.Min(selector)),
                           MittelwertInAllerGemeinde = formatter(all.Average(selector)),
                           MaximalwertInAllerGemeinde = formatter(all.Max(selector))
                       };
        }

        protected override void AddSubReportDefinitionResourceNames(Dictionary<string, string> subReportDefinitionResourceNames)
        {
            base.AddSubReportDefinitionResourceNames(subReportDefinitionResourceNames);
            subReportDefinitionResourceNames.Add(BenchmarkauswertungZustandsindexNetzSubReportName, SubReportDefinitionResourceName);
            subReportDefinitionResourceNames.Add(BenchmarkauswertungZustandsindexJeBelastungskategorieSubReportName, SubReportDefinitionResourceName);
            subReportDefinitionResourceNames.Add(BenchmarkauswertungMittleresAlterDerZustandsaufnahmenNetzSubReportName, SubReportDefinitionResourceName);
        }

        public override int? MaxImagePreviewPageHeight { get {  return 640; } }
    }
}