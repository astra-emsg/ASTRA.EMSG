using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Services.Security;

namespace ASTRA.EMSG.Business.Services.Benchmarking
{
    public interface IKenngroessenFruehererJahreBenchmarkingDataService : IBenchmarkingDataService { }

    public class KenngroessenFruehererJahreBenchmarkingDataService : IKenngroessenFruehererJahreBenchmarkingDataService
    {
        private readonly ITransactionScopeProvider transactionScopeProvider;
        private readonly ISecurityService securityService;
        private readonly IHistorizationService historizationService;
        private readonly IBenchmarkingDataDetailCalculatorService calculatorService;

        public KenngroessenFruehererJahreBenchmarkingDataService(
            ITransactionScopeProvider transactionScopeProvider,
            ISecurityService securityService,
            IHistorizationService historizationService,
            IBenchmarkingDataDetailCalculatorService calculatorService)
        {
            this.transactionScopeProvider = transactionScopeProvider;
            this.securityService = securityService;
            this.historizationService = historizationService;
            this.calculatorService = calculatorService;
        }

        public List<BenchmarkingData> GetForMandantList(DateTime jahrDateTime, IList<Mandant> mandants)
        {
            var mandantIds = mandants.Select(m => m.Id).ToArray();
            var result = new List<BenchmarkingData>();
            GetFromKenngroessenFruehererJahre(jahrDateTime, mandantIds, result);
            return result;
        }

        private void GetFromKenngroessenFruehererJahre(DateTime jahrDateTime, Guid[] mandantIds, List<BenchmarkingData> result)
        {
            var kenngroessenFruehererJahres = GetKenngroessenFruehererJahres(jahrDateTime);
            var pageWithMandantIds = PageWithMandantIds(mandantIds, kenngroessenFruehererJahres);
            result.AddRange(GetBenchmarkingDataFromKenngoessen(pageWithMandantIds));
        }

        private IQueryable<KenngroessenFruehererJahre> GetKenngroessenFruehererJahres(DateTime jahrDateTime)
        {
            return transactionScopeProvider.Queryable<KenngroessenFruehererJahre>().OrderBy(k => k.Id).Where(k => k.Jahr == jahrDateTime.Year);
        }

        public BenchmarkingData GetForCurrentMandant(DateTime jahrDateTime)
        {
            var result = new List<BenchmarkingData>();
            var mandantIds = new[] { securityService.GetCurrentMandant().Id };
            GetFromKenngroessenFruehererJahre(jahrDateTime, mandantIds, result);
            return result.SingleOrDefault();
        }

        public List<BenchmarkingData> GetForAllMandant(DateTime jahrDateTime)
        {
            var result = new List<BenchmarkingData>();
            result.AddRange(GetBenchmarkingDataFromKenngoessen(GetKenngroessenFruehererJahres(jahrDateTime)));
            return result;
        }

        private IEnumerable<BenchmarkingData> GetBenchmarkingDataFromKenngoessen(IEnumerable<KenngroessenFruehererJahre> kenngroessenFruehererJahres)
        {
            return kenngroessenFruehererJahres
                .Select(
                    k =>
                        {
                            var mandantDetails = transactionScopeProvider.Queryable<MandantDetails>().Single(m => m.Mandant == k.Mandant && m.ErfassungsPeriod == historizationService.GetCurrentErfassungsperiod(k.Mandant));
                            var kenngroessenFruehererJahreDetails = k.KenngroesseFruehereJahrDetails.ToList();

                            var benchmarkingData = calculatorService.CalculateInventarBenchmarkingData(
                                historizationService.GetCurrentErfassungsperiod(k.Mandant),
                                mandantDetails,
                                kenngroessenFruehererJahreDetails,
                                d => d.Fahrbahnlaenge);

                            calculatorService.CalculateZustandsBenchmarkingData(
                                benchmarkingData,
                                kenngroessenFruehererJahreDetails,
                                d => null, 
                                d => d.MittlererZustand);

                            calculatorService.CalculateRealisierteMassnahmenBenchmarkingData(
                                historizationService.GetCurrentErfassungsperiod(k.Mandant),
                                benchmarkingData,
                                mandantDetails,
                                kenngroessenFruehererJahreDetails,
                                kenngroessenFruehererJahreDetails);

                            return benchmarkingData;
                        });
        }

        private List<KenngroessenFruehererJahre> PageWithMandantIds(Guid[] mandantIds, IQueryable<KenngroessenFruehererJahre> entities)
        {
            var result = new List<KenngroessenFruehererJahre>();
            const int querySize = 500;
            var page = 0;
            while (page * querySize < mandantIds.Length)
            {
                var idPage = mandantIds.Skip(page * querySize).Take(querySize).ToArray();
                result.AddRange(entities.Where(d => idPage.Contains(d.Mandant.Id)));
                page++;
            }
            return result;
        }
    }
}