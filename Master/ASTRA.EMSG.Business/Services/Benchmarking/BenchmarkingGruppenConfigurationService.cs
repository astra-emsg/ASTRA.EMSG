using System;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Utils;

namespace ASTRA.EMSG.Business.Services.Benchmarking
{
    public interface IBenchmarkingGruppenConfigurationService : IService
    {
        RangeModel GetRange(BenchmarkingGruppenTyp benchmarkingGruppenTyp, MandantDetails currentMandantDetails);
    }

    public class BenchmarkingGruppenConfigurationService : IBenchmarkingGruppenConfigurationService
    {
        private readonly ITransactionScopeProvider transactionScopeProvider;

        public BenchmarkingGruppenConfigurationService(
            ITransactionScopeProvider transactionScopeProvider)
        {
            this.transactionScopeProvider = transactionScopeProvider;
        }

        public RangeModel GetRange(BenchmarkingGruppenTyp benchmarkingGruppenTyp, MandantDetails currentMandantDetails)
        {
            var dataBaseEigenschaftTyp = benchmarkingGruppenTyp.ToDataBaseEigenschaftTyp();

            var wert = GetBenchmarkingGruppenWert(currentMandantDetails, benchmarkingGruppenTyp);
            var benchmarkingGruppenConfigurations = transactionScopeProvider.Queryable<BenchmarkingGruppenConfiguration>().Where(bgc => bgc.EigenschaftTyp == dataBaseEigenschaftTyp);

            return new RangeModel
                       {
                           BenchmarkingGruppen = benchmarkingGruppenTyp,
                           UntereInclusieveGrenzwert = benchmarkingGruppenConfigurations.Where(bgc => bgc.Grenzwert <= wert).OrderByDescending(bgc => bgc.Grenzwert).Select(bgc => (decimal?)bgc.Grenzwert).FirstOrDefault(),
                           ObereExclusiveGrenzwert = benchmarkingGruppenConfigurations.Where(bgc => bgc.Grenzwert > wert).OrderBy(bgc => bgc.Grenzwert).Select(bgc => (decimal?)bgc.Grenzwert).FirstOrDefault(),
                       };
        }

        private decimal GetBenchmarkingGruppenWert(MandantDetails mandantDetailsModel, BenchmarkingGruppenTyp benchmarkingGruppenTyp)
        {
            switch (benchmarkingGruppenTyp)
            {
                case BenchmarkingGruppenTyp.NetzGroesse:
                    return mandantDetailsModel.NetzLaenge;
                case BenchmarkingGruppenTyp.EinwohnerGroesse:
                    return mandantDetailsModel.Einwohner ?? 0;
                case BenchmarkingGruppenTyp.MittlereHoehenlageSiedlungsgebieteGroesse:
                    return mandantDetailsModel.MittlereHoehenlageSiedlungsgebiete ?? 0;
                case BenchmarkingGruppenTyp.SteuerertragGroesse:
                    return mandantDetailsModel.Steuerertrag ?? 0;
                default:
                    throw new ArgumentOutOfRangeException("benchmarkingGruppenTyp");
            }
        }
    }
}
