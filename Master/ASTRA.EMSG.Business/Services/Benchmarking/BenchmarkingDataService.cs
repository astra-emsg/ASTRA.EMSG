using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.Security;
using System.Linq;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Services.Benchmarking
{
    public interface IBenchmarkingDataService : IService
    {
        List<BenchmarkingData> GetForMandantList(DateTime jahrDateTime, IList<Mandant> mandants);
        BenchmarkingData GetForCurrentMandant(DateTime jahrDateTime);
        List<BenchmarkingData> GetForAllMandant(DateTime jahrDateTime);
    }

    public class BenchmarkingDataService : IBenchmarkingDataService
    {
        private readonly ITransactionScopeProvider transactionScopeProvider;
        private readonly ISecurityService securityService;
        private readonly IKenngroessenFruehererJahreBenchmarkingDataService kenngroessenFruehererJahreBenchmarkingDataService;

        public BenchmarkingDataService(
            ITransactionScopeProvider transactionScopeProvider,
            ISecurityService securityService,
            IKenngroessenFruehererJahreBenchmarkingDataService kenngroessenFruehererJahreBenchmarkingDataService)
        {
            this.transactionScopeProvider = transactionScopeProvider;
            this.securityService = securityService;
            this.kenngroessenFruehererJahreBenchmarkingDataService = kenngroessenFruehererJahreBenchmarkingDataService;
        }

        public List<BenchmarkingData> GetForMandantList(DateTime jahrDateTime, IList<Mandant> mandants)
        {

            var result = new List<BenchmarkingData>();

            var mandantIds = mandants.Select(m => m.Id).ToArray();

            GetFromBenchmarkingData(result, jahrDateTime, mandantIds);

            result.AddRange(kenngroessenFruehererJahreBenchmarkingDataService.GetForMandantList(jahrDateTime, mandants));

            return result;
        }

        public List<BenchmarkingData> GetForAllMandant(DateTime jahrDateTime)
        {
            var result = new List<BenchmarkingData>();

            result.AddRange(GetBenchmarkingDatasFromMandant(jahrDateTime));

            result.AddRange(kenngroessenFruehererJahreBenchmarkingDataService.GetForAllMandant(jahrDateTime));

            return result;
        }

        public BenchmarkingData GetForCurrentMandant(DateTime jahrDateTime)
        {
            var result = new List<BenchmarkingData>();

            var mandantIds = new[] { securityService.GetCurrentMandant().Id };

            GetFromBenchmarkingData(result, jahrDateTime, mandantIds);

            BenchmarkingData benchmarkingData = kenngroessenFruehererJahreBenchmarkingDataService.GetForCurrentMandant(jahrDateTime);
            if (benchmarkingData != null)
                result.Add(benchmarkingData);

            return result.Single();
        }

        private void GetFromBenchmarkingData(List<BenchmarkingData> result, DateTime jahrDateTime, Guid[] mandantIds)
        {
            var benchmarkingDatas = GetBenchmarkingDatasFromMandant(jahrDateTime);
            result.AddRange(PageWithMandantIds(mandantIds, benchmarkingDatas));
        }

        private IQueryable<BenchmarkingData> GetBenchmarkingDatasFromMandant(DateTime jahrDateTime)
        {
            return transactionScopeProvider.Queryable<BenchmarkingData>()
                .OrderBy(b => b.Id)
                .Where(d => d.ErfassungsPeriod.Erfassungsjahr.Year == jahrDateTime.Year);
        }

        private List<BenchmarkingData> PageWithMandantIds(Guid[] mandantIds, IQueryable<BenchmarkingData> entities)
        {
            var result = new List<BenchmarkingData>();
            const int querySize = 500;
            var page = 0;
            while (page * querySize < mandantIds.Length)
            {
                var idPage = mandantIds.Skip(page * querySize).Take(querySize).ToArray();
                result.AddRange(entities.Where(d => idPage.Contains(d.Mandant.Id)).Fetch(d => d.BenchmarkingDataDetails));
                page++;
            }
            return result;
        }
    }
}