using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;
using NHibernate;
using NHibernate.Criterion;

namespace ASTRA.EMSG.Business.Services.Benchmarking
{
    public interface IBenchmarkingGruppenService : IService
    {
        List<Mandant> GetAenlicheMandanten(DateTime erfassungsJahr, List<BenchmarkingGruppenTyp> benchmarkingGruppenTypList);
    }

    public class BenchmarkingGruppenService : IBenchmarkingGruppenService
    { 
        private readonly ITransactionScopeProvider transactionScopeProvider;
        private readonly IMandantDetailsService mandantDetailsService;
        private readonly IBenchmarkingGruppenConfigurationService benchmarkingGruppenConfigurationService;

        public BenchmarkingGruppenService(
            ITransactionScopeProvider transactionScopeProvider,
            IMandantDetailsService mandantDetailsService,
            IBenchmarkingGruppenConfigurationService benchmarkingGruppenConfigurationService)
        {
            this.transactionScopeProvider = transactionScopeProvider;
            this.mandantDetailsService = mandantDetailsService;
            this.benchmarkingGruppenConfigurationService = benchmarkingGruppenConfigurationService;
        }

        public List<Mandant> GetAenlicheMandanten(DateTime erfassungsJahr, List<BenchmarkingGruppenTyp> benchmarkingGruppenTypList)
        {
            var currentMandantDetails = mandantDetailsService.GetMandantDetailsByJahr(erfassungsJahr);

            MandantDetails mandantDetails = null;
            Mandant mandant = null;
            ErfassungsPeriod erfassungsPeriod = null;
            var queryOver = transactionScopeProvider.CurrentTransactionScope.Session
                .QueryOver(() => mandantDetails)
                .JoinAlias(() => mandantDetails.ErfassungsPeriod, () => erfassungsPeriod)
                .Where(() => mandantDetails.IsCompleted)
                .Where(() => erfassungsPeriod.IsClosed)
                .Where(() => erfassungsPeriod.Erfassungsjahr == erfassungsJahr);

            queryOver = BuildGroupping(queryOver, benchmarkingGruppenTypList, currentMandantDetails);

            var aenlicheMandanten = queryOver.Select(md => md.Mandant).List<Mandant>().ToList();

            var kengrosseMandantId = transactionScopeProvider.Queryable<KenngroessenFruehererJahre>()
                .Where(k => k.Jahr == erfassungsJahr.Year).Select(k => k.Mandant.Id).ToList();

            queryOver = transactionScopeProvider.CurrentTransactionScope.Session
                .QueryOver(() => mandantDetails)
                .JoinAlias(() => mandantDetails.ErfassungsPeriod, () => erfassungsPeriod)
                .JoinAlias(() => mandantDetails.Mandant, () => mandant)
                .Where(() => mandantDetails.IsCompleted) // Safety check
                .Where(() => !erfassungsPeriod.IsClosed) // You can only get the data from the current erfassungsPeriod is calculation with KenngroessenFruehererJahre
                .WhereRestrictionOn(() => mandant.Id).IsIn(kengrosseMandantId);

            queryOver = BuildGroupping(queryOver, benchmarkingGruppenTypList, currentMandantDetails);

            aenlicheMandanten.AddRange(queryOver.Select(md => md.Mandant).List<Mandant>().ToList());

            return aenlicheMandanten;
        }

        private IQueryOver<MandantDetails, MandantDetails> BuildGroupping(IQueryOver<MandantDetails, MandantDetails> queryOver, IEnumerable<BenchmarkingGruppenTyp> benchmarkingGruppenTypList, MandantDetails currentMandantDetails)
        {
            foreach (var benchmarkingGruppenTyp in benchmarkingGruppenTypList)
            {
                switch (benchmarkingGruppenTyp)
                {
                    case BenchmarkingGruppenTyp.NetzGroesse:
                        queryOver = BuildQueryOver(md => md.NetzLaenge, queryOver, benchmarkingGruppenTyp, currentMandantDetails);
                        break;
                    case BenchmarkingGruppenTyp.EinwohnerGroesse:
                        queryOver = BuildQueryOver(md => md.Einwohner, queryOver, benchmarkingGruppenTyp, currentMandantDetails);
                        break;
                    case BenchmarkingGruppenTyp.Gemeinde:
                        queryOver = queryOver.Where(md => md.Gemeindetyp == currentMandantDetails.Gemeindetyp);
                        break;
                    case BenchmarkingGruppenTyp.MittlereHoehenlageSiedlungsgebieteGroesse:
                        queryOver = BuildQueryOver(md => md.MittlereHoehenlageSiedlungsgebiete, queryOver,
                                                   benchmarkingGruppenTyp, currentMandantDetails);
                        break;
                    case BenchmarkingGruppenTyp.OeffentlicheVerkehrsmittel:
                        queryOver =
                            queryOver.Where(
                                md => md.OeffentlicheVerkehrsmittel == currentMandantDetails.OeffentlicheVerkehrsmittel);
                        break;
                    case BenchmarkingGruppenTyp.SteuerertragGroesse:
                        queryOver = BuildQueryOver(md => md.Steuerertrag, queryOver, benchmarkingGruppenTyp, currentMandantDetails);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return queryOver;
        }

        private IQueryOver<MandantDetails, MandantDetails> BuildQueryOver<TProperty>(Expression<Func<MandantDetails, TProperty>> expression, IQueryOver<MandantDetails, MandantDetails> queryOver, BenchmarkingGruppenTyp benchmarkingGruppenTyp, MandantDetails currentMandantDetails)
        {
            string propertyName = ExpressionHelper.GetPropertyName(expression);
            var rangeModel = benchmarkingGruppenConfigurationService.GetRange(benchmarkingGruppenTyp, currentMandantDetails);

            if (rangeModel.UntereInclusieveGrenzwert.HasValue)
                queryOver = queryOver.Where(Restrictions.Ge(propertyName, Convert<TProperty>(rangeModel.UntereInclusieveGrenzwert.Value)));

            if (rangeModel.ObereExclusiveGrenzwert.HasValue)
                queryOver = queryOver.Where(Restrictions.Lt(propertyName, Convert<TProperty>(rangeModel.ObereExclusiveGrenzwert.Value)));

            return queryOver;
        }

        private static object Convert<TProperty>(decimal value)
        {
            return typeof (TProperty) == typeof (int?) ? System.Convert.ChangeType(value, typeof (int)) : value;
        }
    }
}
