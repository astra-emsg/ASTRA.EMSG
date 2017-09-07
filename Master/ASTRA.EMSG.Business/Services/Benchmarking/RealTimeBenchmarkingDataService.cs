using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Services.Benchmarking
{
    public class RealTimeCalculatingBenchmarkingDataService : IBenchmarkingDataService
    {
        private readonly IKenngroessenFruehererJahreBenchmarkingDataService kenngroessenFruehererJahreBenchmarkingDataService;
        private readonly ITransactionScopeProvider transactionScopeProvider;
        private readonly ISecurityService securityService;
        private readonly ITestBenchmarkingDataDetailCalculatorService benchmarkingDataDetailCalculatorService;

        public RealTimeCalculatingBenchmarkingDataService(
            IBenchmarkingDataDetailCalculatorService benchmarkingDataDetailCalculatorService, 
            IKenngroessenFruehererJahreBenchmarkingDataService kenngroessenFruehererJahreBenchmarkingDataService,
            ITransactionScopeProvider transactionScopeProvider, 
            ISecurityService securityService)
        {
            this.kenngroessenFruehererJahreBenchmarkingDataService = kenngroessenFruehererJahreBenchmarkingDataService;
            this.transactionScopeProvider = transactionScopeProvider;
            this.securityService = securityService;
            this.benchmarkingDataDetailCalculatorService = benchmarkingDataDetailCalculatorService as ITestBenchmarkingDataDetailCalculatorService;
        }

        public List<BenchmarkingData> GetForMandantList(DateTime jahrDateTime, IList<Mandant> mandants)
        {
            var mandantIds = mandants.Select(m => m.Id).ToArray();
            var q = transactionScopeProvider.Queryable<MandantDetails>()
                                            .OrderBy(b => b.Id)
                                            .Where(d => d.ErfassungsPeriod.Erfassungsjahr.Year == jahrDateTime.Year && mandantIds.Contains(d.Mandant.Id));
            var result = ForMandantList(q);
            result.AddRange(kenngroessenFruehererJahreBenchmarkingDataService.GetForMandantList(jahrDateTime, mandants));
            return result;
        }

        public BenchmarkingData GetForCurrentMandant(DateTime jahrDateTime)
        {
            var q = transactionScopeProvider.Queryable<MandantDetails>()
                                            .OrderBy(b => b.Id)
                                            .Where(
                                                d =>
                                                d.ErfassungsPeriod.Erfassungsjahr.Year == jahrDateTime.Year &&
                                                d.Mandant.Id == securityService.GetCurrentMandant().Id);
            var result = ForMandantList(q);
            BenchmarkingData benchmarkingData = kenngroessenFruehererJahreBenchmarkingDataService.GetForCurrentMandant(jahrDateTime);
            if (benchmarkingData != null)
                result.Add(benchmarkingData);
            return result.First();
        }

        public List<BenchmarkingData> GetForAllMandant(DateTime jahrDateTime)
        {
            var q = transactionScopeProvider.Queryable<MandantDetails>()
                                            .OrderBy(b => b.Id)
                                            .Where(d => d.ErfassungsPeriod.Erfassungsjahr.Year == jahrDateTime.Year);
            var result = ForMandantList(q);
            result.AddRange(kenngroessenFruehererJahreBenchmarkingDataService.GetForAllMandant(jahrDateTime));
            return result;
        }

        private List<BenchmarkingData> ForMandantList(IQueryable<MandantDetails> q)
        {
            var result = new List<BenchmarkingData>();
            foreach (var mandantDetailse in q)
            {
                switch (mandantDetailse.ErfassungsPeriod.NetzErfassungsmodus)
                {
                    case NetzErfassungsmodus.Summarisch:
                        result.Add(benchmarkingDataDetailCalculatorService.CalculateBenchmarkingDataForSummarischeModus(
                            mandantDetailse.ErfassungsPeriod, mandantDetailse));
                        break;
                    case NetzErfassungsmodus.Tabellarisch:
                        result.Add(benchmarkingDataDetailCalculatorService.CalculateBenchmarkingDataForTabellarischeModus(
                            mandantDetailse.ErfassungsPeriod, mandantDetailse));
                        break;
                    case NetzErfassungsmodus.Gis:
                        result.Add(benchmarkingDataDetailCalculatorService.CalculateBenchmarkingDataForGisModus(
                            mandantDetailse.ErfassungsPeriod, mandantDetailse));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return result;
        }
    }
}