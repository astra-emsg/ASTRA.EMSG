using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Katalogs;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.EntityServices.Strassennamen;
using ASTRA.EMSG.Business.Services.EntityServices.Summarisch;
using ASTRA.EMSG.Common.Enums;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Services.Historization
{
    public interface IBenchmarkingDataDetailCalculatorService : IService
    {
        void CalculateBenchmarkingData(ErfassungsPeriod closedPeriod);

        BenchmarkingData CalculateZustandsBenchmarkingData<TEntity>(BenchmarkingData benchmarkingData, List<TEntity> zustandEntities, Func<TEntity, DateTime?> getAufnahmeDatum, Func<TEntity, decimal?> getZustandsindex) where TEntity : IBelastungskategorieHolder, IFlaecheFahrbahnUndTrottoirHolder;
        BenchmarkingData CalculateRealisierteMassnahmenBenchmarkingData<TEntity, TStrassenEntity>(ErfassungsPeriod closedPeriod, BenchmarkingData benchmarkingData, MandantDetails mandantDetails, List<TEntity> realisierteMassnahmenEntities, List<TStrassenEntity> strassenEntities)
            where TEntity : IBelastungskategorieHolder, IRealisierteFlaecheHolder, IRealisierteMassnahmeKostenHolder
            where TStrassenEntity : IBelastungskategorieHolder, IFlaecheFahrbahnUndTrottoirHolder;
        BenchmarkingData CalculateInventarBenchmarkingData<TEntity>(ErfassungsPeriod closedPeriod, MandantDetails mandantDetails, List<TEntity> strassenEntities, Func<TEntity, decimal> getLaenge) where TEntity : IBelastungskategorieHolder, IFlaecheFahrbahnUndTrottoirHolder;
    }

    public interface ITestBenchmarkingDataDetailCalculatorService
    {
        BenchmarkingData CalculateBenchmarkingDataForSummarischeModus(ErfassungsPeriod closedPeriod, MandantDetails mandantDetails);
        BenchmarkingData CalculateBenchmarkingDataForTabellarischeModus(ErfassungsPeriod closedPeriod, MandantDetails mandantDetails);
        BenchmarkingData CalculateBenchmarkingDataForGisModus(ErfassungsPeriod closedPeriod, MandantDetails mandantDetails);
    }

    public class BenchmarkingDataDetailCalculatorService : ITestBenchmarkingDataDetailCalculatorService, IBenchmarkingDataDetailCalculatorService
    {
        private readonly IMandantDetailsService mandantDetailsService;
        private readonly INetzSummarischDetailService netzSummarischDetailService;
        private readonly IBelastungskategorieService belastungskategorieService;
        private readonly IStrassenabschnittService strassenabschnittService;
        private readonly IStrassenabschnittGISService strassenabschnittGISService;
        private readonly IZustandsabschnittService zustandsabschnittService;
        private readonly IZustandsabschnittGISService zustandsabschnittGISService;
        private readonly IRealisierteMassnahmeSummarsichService realisierteMassnahmeSummarsichService;
        private readonly IRealisierteMassnahmeService realisierteMassnahmeService;
        private readonly IRealisierteMassnahmeGISModelService realisierteMassnahmeGISModelService;
        private readonly ITimeService timeService;
        private readonly ITransactionScopeProvider transactionScopeProvider;
        private readonly IWiederbeschaffungswertKatalogService wiederbeschaffungswertKatalogService;

        public BenchmarkingDataDetailCalculatorService(
            IMandantDetailsService mandantDetailsService,
            INetzSummarischDetailService netzSummarischDetailService,
            IBelastungskategorieService belastungskategorieService,
            IStrassenabschnittService strassenabschnittService,
            IStrassenabschnittGISService strassenabschnittGISService,
            ITransactionScopeProvider transactionScopeProvider,
            IWiederbeschaffungswertKatalogService wiederbeschaffungswertKatalogService,
            IZustandsabschnittService zustandsabschnittService,
            IZustandsabschnittGISService zustandsabschnittGISService, 
            IRealisierteMassnahmeSummarsichService realisierteMassnahmeSummarsichService, 
            IRealisierteMassnahmeService realisierteMassnahmeService, 
            IRealisierteMassnahmeGISModelService realisierteMassnahmeGISModelService,
            ITimeService timeService
            )
        {
            this.mandantDetailsService = mandantDetailsService;
            this.netzSummarischDetailService = netzSummarischDetailService;
            this.belastungskategorieService = belastungskategorieService;
            this.strassenabschnittService = strassenabschnittService;
            this.strassenabschnittGISService = strassenabschnittGISService;
            this.transactionScopeProvider = transactionScopeProvider;
            this.wiederbeschaffungswertKatalogService = wiederbeschaffungswertKatalogService;
            this.zustandsabschnittService = zustandsabschnittService;
            this.zustandsabschnittGISService = zustandsabschnittGISService;
            this.realisierteMassnahmeSummarsichService = realisierteMassnahmeSummarsichService;
            this.realisierteMassnahmeService = realisierteMassnahmeService;
            this.realisierteMassnahmeGISModelService = realisierteMassnahmeGISModelService;
            this.timeService = timeService;
        }

        public void CalculateBenchmarkingData(ErfassungsPeriod closedPeriod)
        {
            var mandantDetails = mandantDetailsService.GetEntitiesBy(closedPeriod).Single();

            BenchmarkingData benchmarkingData;

            switch (closedPeriod.NetzErfassungsmodus)
            {
                case NetzErfassungsmodus.Summarisch:
                    benchmarkingData = CalculateBenchmarkingDataForSummarischeModus(closedPeriod, mandantDetails);
                    break;
                case NetzErfassungsmodus.Tabellarisch:
                    benchmarkingData = CalculateBenchmarkingDataForTabellarischeModus(closedPeriod, mandantDetails);
                    break;
                case NetzErfassungsmodus.Gis:
                    benchmarkingData = CalculateBenchmarkingDataForGisModus(closedPeriod, mandantDetails);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            transactionScopeProvider.Create(benchmarkingData);
        }

        public BenchmarkingData CalculateBenchmarkingDataForSummarischeModus(ErfassungsPeriod closedPeriod, MandantDetails mandantDetails)
        {
            var netzSummarischDetails = netzSummarischDetailService.GetEntitiesBy(closedPeriod, mandantDetails.Mandant).Fetch(nsd => nsd.NetzSummarisch).ToList();
            var realisierteMassnahmeSummarsichen = realisierteMassnahmeSummarsichService.GetEntitiesBy(closedPeriod, mandantDetails.Mandant).ToList();

            BenchmarkingData benchmarkingData = CalculateInventarBenchmarkingData(closedPeriod, mandantDetails, netzSummarischDetails, nsd => nsd.Fahrbahnlaenge);
            CalculateZustandsBenchmarkingData(benchmarkingData, netzSummarischDetails.ToList(), nsd => nsd.NetzSummarisch.MittleresErhebungsJahr, za => za.MittlererZustand);

            CalculateRealisierteMassnahmenBenchmarkingData(closedPeriod, benchmarkingData, mandantDetails, realisierteMassnahmeSummarsichen.ToList(), netzSummarischDetails);
            return benchmarkingData;
        }

        public BenchmarkingData CalculateBenchmarkingDataForTabellarischeModus(ErfassungsPeriod closedPeriod, MandantDetails mandantDetails)
        {
            var strassenabschnitten = strassenabschnittService.GetEntitiesBy(closedPeriod, mandantDetails.Mandant).Where(sa => sa.Strasseneigentuemer == EigentuemerTyp.Gemeinde);
            var zustandsabschnitten = zustandsabschnittService.GetEntitiesBy(closedPeriod, mandantDetails.Mandant).Where(za => za.Strassenabschnitt.Strasseneigentuemer == EigentuemerTyp.Gemeinde);
            var realisierteMassnahmen = realisierteMassnahmeService.GetEntitiesBy(closedPeriod, mandantDetails.Mandant);
            return CalculateBenchmarkingDataForStrassenModus(closedPeriod, mandantDetails, strassenabschnitten, zustandsabschnitten, realisierteMassnahmen);
        }

        public BenchmarkingData CalculateBenchmarkingDataForGisModus(ErfassungsPeriod closedPeriod, MandantDetails mandantDetails)
        {
            var strassenabschnittenGis = strassenabschnittGISService.GetEntitiesBy(closedPeriod, mandantDetails.Mandant).Where(sa => sa.Strasseneigentuemer == EigentuemerTyp.Gemeinde);
            var zustandsabschnittenGis = zustandsabschnittGISService.GetEntitiesBy(closedPeriod, mandantDetails.Mandant).Where(za => za.StrassenabschnittGIS.Strasseneigentuemer == EigentuemerTyp.Gemeinde);
            var realisierteMassnahmenGis = realisierteMassnahmeGISModelService.GetEntitiesBy(closedPeriod, mandantDetails.Mandant);
            return CalculateBenchmarkingDataForStrassenModus(closedPeriod, mandantDetails, strassenabschnittenGis, zustandsabschnittenGis, realisierteMassnahmenGis);
        }

        private BenchmarkingData CalculateBenchmarkingDataForStrassenModus<TStrassenabschnittEntity, TZustandsabschnittEntity, TRealisierteMassnahmeEntity>(ErfassungsPeriod closedPeriod, MandantDetails mandantDetails, IQueryable<TStrassenabschnittEntity> strassenabschnitten, IQueryable<TZustandsabschnittEntity> zustandsabschnitten, IQueryable<TRealisierteMassnahmeEntity> realisierteMassnahmen)
            where TStrassenabschnittEntity : StrassenabschnittBase
            where TZustandsabschnittEntity : ZustandsabschnittBase
            where TRealisierteMassnahmeEntity : RealisierteMassnahmeBase
        {
            BenchmarkingData benchmarkingData = CalculateInventarBenchmarkingData(closedPeriod, mandantDetails, strassenabschnitten.ToList(), sa => sa.Laenge);
            
            CalculateZustandsBenchmarkingData(benchmarkingData, zustandsabschnitten.ToList(), za => za.Aufnahmedatum, za => za.Zustandsindex);
            CalculateRealisierteMassnahmenBenchmarkingData(closedPeriod, benchmarkingData, mandantDetails, realisierteMassnahmen.ToList(), strassenabschnitten.ToList());
            return benchmarkingData;
        }

        public BenchmarkingData CalculateZustandsBenchmarkingData<TEntity>(BenchmarkingData benchmarkingData, List<TEntity> zustandEntities, Func<TEntity, DateTime?> getAufnahmeDatum, Func<TEntity, decimal?> getZustandsindex)
            where TEntity : IBelastungskategorieHolder, IFlaecheFahrbahnUndTrottoirHolder
        {
            benchmarkingData.ZustandsindexNetz = null;
            var entitiesWithZustandsindex = zustandEntities.Where(e => getZustandsindex(e).HasValue).ToList();
            if(entitiesWithZustandsindex.Any())
                benchmarkingData.ZustandsindexNetz = Divide(entitiesWithZustandsindex.Sum(e => getZustandsindex(e).Value * e.FlaecheFahrbahn), entitiesWithZustandsindex.Sum(e => e.FlaecheFahrbahn));

            benchmarkingData.MittleresAlterDerZustandsaufnahmenNetz = null;
            var entitiesWithAufnahmeDatum = zustandEntities.Where(e => getAufnahmeDatum(e).HasValue).ToList();
            if (entitiesWithAufnahmeDatum.Any())
                benchmarkingData.MittleresAlterDerZustandsaufnahmenNetz = DateTime.FromBinary((long)(Divide(entitiesWithAufnahmeDatum.Sum(e => getAufnahmeDatum(e).Value.Ticks * e.FlaecheFahrbahn), entitiesWithAufnahmeDatum.Sum(e => e.FlaecheFahrbahn))));

            foreach (var bdd in benchmarkingData.BenchmarkingDataDetails)
            {
                var entitiesForBelastungskategorie = zustandEntities.Where(e => e.Belastungskategorie.Id == bdd.Belastungskategorie.Id && getZustandsindex(e).HasValue).ToList();
                if (entitiesForBelastungskategorie.Any())
                    bdd.Zustandsindex = Divide(entitiesForBelastungskategorie.Sum(e => getZustandsindex(e).Value * e.FlaecheFahrbahn), entitiesWithAufnahmeDatum.Where(e => e.Belastungskategorie.Id == bdd.Belastungskategorie.Id).Sum(e => e.FlaecheFahrbahn));
            }

            return benchmarkingData;
        }

        public BenchmarkingData CalculateRealisierteMassnahmenBenchmarkingData<TEntity, TStrassenEntity>(
            ErfassungsPeriod closedPeriod,
            BenchmarkingData benchmarkingData, 
            MandantDetails mandantDetails,
            List<TEntity> realisierteMassnahmenEntities, List<TStrassenEntity> strassenEntities)
            where TEntity : IBelastungskategorieHolder, IRealisierteFlaecheHolder, IRealisierteMassnahmeKostenHolder
            where TStrassenEntity : IBelastungskategorieHolder, IFlaecheFahrbahnUndTrottoirHolder
        {
            var realisierteMassnahmenDataProBelastungskategorie = belastungskategorieService.AlleBelastungskategorie.ToDictionary(b => b, b => new RealisierteMassnahmeData { Fleache = 0m, Wbw = 0m, Wertverlust = 0m, Kosten = 0m });

            foreach (var entity in strassenEntities)
            {
                var bk = realisierteMassnahmenDataProBelastungskategorie.Keys.Single(b => b.Id == entity.Belastungskategorie.Id);
                
                var wieder = GetWieder(bk, closedPeriod);
                var wiederbeschaffungswert = GetWiederbeschaffungswert(entity, wieder);
                var wertverlustII = wiederbeschaffungswert * wieder.AlterungsbeiwertII / 100;

                realisierteMassnahmenDataProBelastungskategorie[bk].Wbw += wiederbeschaffungswert;
                realisierteMassnahmenDataProBelastungskategorie[bk].Wertverlust += wertverlustII;
            }

            foreach (var entity in realisierteMassnahmenEntities)
            {
                var bk = realisierteMassnahmenDataProBelastungskategorie.Keys.Single(b => b.Id == entity.Belastungskategorie.Id);
                realisierteMassnahmenDataProBelastungskategorie[bk].Fleache += entity.RealisierteFlaeche;
                realisierteMassnahmenDataProBelastungskategorie[bk].Kosten += entity.Kosten;
                realisierteMassnahmenDataProBelastungskategorie[bk].WbwKosten += entity.WbwKosten;
            }

            decimal sumKosten = realisierteMassnahmenDataProBelastungskategorie.Values.Sum(r => r.Kosten);
            decimal sumFleache = realisierteMassnahmenDataProBelastungskategorie.Values.Sum(r => r.Fleache);
            decimal sumWbw = realisierteMassnahmenDataProBelastungskategorie.Values.Sum(r => r.Wbw);
            decimal sumWertverlust = realisierteMassnahmenDataProBelastungskategorie.Values.Sum(r => r.Wertverlust);

            benchmarkingData.RealisierteMassnahmenProFahrbahn = Divide(sumKosten, sumFleache);
            benchmarkingData.RealisierteMassnahmenProEinwohner = Divide(sumKosten, mandantDetails.Einwohner ?? 0);
            benchmarkingData.RealisierteMassnahmenProWertverlustNetz = Percent(sumKosten, sumWertverlust);
            benchmarkingData.RealisierteMassnahmenProWiederbeschaffungswertNetz = Percent(sumKosten, sumWbw);

            foreach (var bdd in benchmarkingData.BenchmarkingDataDetails)
            {
                var r = realisierteMassnahmenDataProBelastungskategorie.Single(f => f.Key.Id == bdd.Belastungskategorie.Id);
                bdd.RealisierteMassnahmenProWiederbeschaffungswertNetz = Percent(r.Value.WbwKosten, r.Value.Wbw);
            }

            return benchmarkingData;
        }

        public class RealisierteMassnahmeData
        {
            public decimal Fleache { get; set; }
            public decimal Wbw { get; set; }
            public decimal Wertverlust { get; set; }
            public decimal Kosten { get; set; }
            public decimal WbwKosten { get; set; }
        }

        public BenchmarkingData CalculateInventarBenchmarkingData<TEntity>(ErfassungsPeriod closedPeriod, MandantDetails mandantDetails, List<TEntity> strassenEntities, Func<TEntity, decimal> getLaenge)
            where TEntity : IBelastungskategorieHolder, IFlaecheFahrbahnUndTrottoirHolder
        {
            var flaecheProBelastungskategorie = belastungskategorieService.AlleBelastungskategorie.ToDictionary(b => b, b => 0m);

            decimal wbw = 0m;
            decimal wvl = 0m;
            
            foreach (var entity in strassenEntities)
            {
                var bk = flaecheProBelastungskategorie.Keys.Single(b => b.Id == entity.Belastungskategorie.Id);
                flaecheProBelastungskategorie[bk] += entity.FlaecheFahrbahn;

                var wieder = GetWieder(bk, closedPeriod);
                var wiederbeschaffungswert = GetWiederbeschaffungswert(entity, wieder);
                var wertverlustII = wiederbeschaffungswert * wieder.AlterungsbeiwertII / 100;

                wbw += wiederbeschaffungswert;
                wvl += wertverlustII;
            }

            decimal laengeSum = strassenEntities.Sum(getLaenge);
            decimal flaecheSum = strassenEntities.Sum(e => e.FlaecheFahrbahn);

            var benchmarkingData = GetBenchmarkingData(closedPeriod, mandantDetails, wvl, wbw, laengeSum, flaecheSum);

            foreach (var bdd in benchmarkingData.BenchmarkingDataDetails)
            {
                var value = flaecheProBelastungskategorie.Single(f => f.Key.Id == bdd.Belastungskategorie.Id);
                bdd.FahrbahnflaecheAnteil = Percent(value.Value, flaecheSum);
            }

            return benchmarkingData;
        }

        private decimal GetWiederbeschaffungswert(IFlaecheFahrbahnUndTrottoirHolder flaecheFahrbahnUndTrottoirHolder, WiederbeschaffungswertKatalogModel wieder)
        {
            if (flaecheFahrbahnUndTrottoirHolder.HasTrottoirInformation)
                return flaecheFahrbahnUndTrottoirHolder.FlaecheFahrbahn * wieder.FlaecheFahrbahn + wieder.FlaecheTrottoir * flaecheFahrbahnUndTrottoirHolder.FlaecheTrottoir;

            return flaecheFahrbahnUndTrottoirHolder.FlaecheFahrbahn * wieder.GesamtflaecheFahrbahn;
        }

        private BenchmarkingData GetBenchmarkingData(ErfassungsPeriod closedPeriod, MandantDetails mandantDetails, decimal wvl, decimal wbw, decimal laengeSum, decimal flaecheSum)
        {
            var benchmarkingData = new BenchmarkingData
                                       {
                                           CalculatedAt = timeService.Now,
                                           NeedsRecalculation = false,
                                           GesamtlaengeDesStrassennetzesProSiedlungsflaeche = Divide(ToKiloMeter(laengeSum), (mandantDetails.Siedlungsflaeche ?? 0)),
                                           GesamtlaengeDesStrassennetzesProEinwohner = Divide(laengeSum, mandantDetails.Einwohner ?? 0),
                                           FahrbahnflaecheProSiedlungsflaeche = Divide(flaecheSum, (mandantDetails.Siedlungsflaeche ?? 0)),
                                           FahrbahnflaecheProEinwohner = Divide(flaecheSum, mandantDetails.Einwohner ?? 0),
                                           GesamtstrassenflaecheProSiedlungsflaeche = Percent(flaecheSum, ToSquareMeter(mandantDetails.Siedlungsflaeche ?? 0)),
                                           GesamtstrassenflaecheProEinwohner = Divide(flaecheSum, mandantDetails.Einwohner ?? 0),
                                           WiederbeschaffungswertProFahrbahn = Divide(wbw, flaecheSum),
                                           WiederbeschaffungswertProEinwohner = Divide(wbw, mandantDetails.Einwohner ?? 0),
                                           WertverlustProFahrbahn = Divide(wvl, flaecheSum),
                                           WertverlustProEinwohner = Divide(wvl, mandantDetails.Einwohner ?? 0),
                                           Mandant = closedPeriod.Mandant,
                                           ErfassungsPeriod = closedPeriod
                                       };

            benchmarkingData.BenchmarkingDataDetails = belastungskategorieService.AlleBelastungskategorie.Select(b => 
                new BenchmarkingDataDetail
                    {
                        Belastungskategorie = b, 
                        BenchmarkingData = benchmarkingData
                    }).ToList();

            return benchmarkingData;
        }

        private WiederbeschaffungswertKatalogModel GetWieder(Belastungskategorie belastungskategorie, ErfassungsPeriod closedPeriod)
        {
            return wiederbeschaffungswertKatalogService.GetWiederbeschaffungswertKatalogModel(belastungskategorie, closedPeriod);
        }

        private decimal Divide(decimal dividend, decimal divisor)
        {
            return divisor == 0 ? 0 : decimal.Divide(dividend, divisor);
        }

        private decimal Percent(decimal dividend, decimal divisor)
        {
            return Divide(dividend, divisor) * 100;
        }

        private decimal ToSquareMeter(decimal hektar)
        {
            return hektar * 10000;
        }

        private decimal ToKiloMeter(decimal meterValue)
        {
            return Divide(meterValue, 1000);
        }
    }
}