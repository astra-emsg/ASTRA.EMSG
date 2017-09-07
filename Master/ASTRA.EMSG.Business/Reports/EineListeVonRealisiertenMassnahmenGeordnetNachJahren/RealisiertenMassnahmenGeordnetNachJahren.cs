using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Entities.Summarisch;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Common.Enums;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Reports.EineListeVonRealisiertenMassnahmenGeordnetNachJahren
{
    public class RealisiertenMassnahmenGeordnetNachJahren<TReportParameter, TReportPo> : EmsgTablePoProviderBase<TReportParameter, TReportPo>
        where TReportParameter : EmsgReportParameter, IErfassungsPeriodVonBisFilter, IFilterParameter
        where TReportPo : EineListeVonRealisiertenMassnahmenGeordnetNachJahrenPoBase, new()
    {
        private readonly IFiltererFactory filtererFactory;
        private readonly ITransactionScopeProvider transactionScopeProvider;
        private readonly IKenngroessenFruehererJahreService kenngroessenFruehererJahreService;
        private ErfassungsPeriod erfassungPeriodVon;
        private ErfassungsPeriod erfassungPeriodBis;

        public RealisiertenMassnahmenGeordnetNachJahren(IFiltererFactory filtererFactory, ITransactionScopeProvider transactionScopeProvider, IKenngroessenFruehererJahreService kenngroessenFruehererJahreService)
        {
            this.filtererFactory = filtererFactory;
            this.transactionScopeProvider = transactionScopeProvider;
            this.kenngroessenFruehererJahreService = kenngroessenFruehererJahreService;
        }

        protected List<TReportPo> CreatePoList(TReportParameter parameter)
        {
            var result = new List<TReportPo>();

            erfassungPeriodVon = ErfassungsPeriodService.GetEntityById(parameter.ErfassungsPeriodIdVon);
            erfassungPeriodBis = ErfassungsPeriodService.GetEntityById(parameter.ErfassungsPeriodIdBis);

            var summarich = filtererFactory.CreateFilterer<RealisierteMassnahmeSummarsich>(parameter).Filter(transactionScopeProvider.Queryable<RealisierteMassnahmeSummarsich>());
            var strassennamen = filtererFactory.CreateFilterer<RealisierteMassnahme>(parameter).Filter(transactionScopeProvider.Queryable<RealisierteMassnahme>());
            var gis = filtererFactory.CreateFilterer<RealisierteMassnahmeGIS>(parameter).Filter(transactionScopeProvider.Queryable<RealisierteMassnahmeGIS>());

            DateTime jahrVon = erfassungPeriodVon.Erfassungsjahr;
            DateTime jahrBis = erfassungPeriodBis.Erfassungsjahr;
            if (!erfassungPeriodBis.IsClosed)
            {
                DateTime notCurrentYear = jahrBis.AddYears(-1);
                result.AddRange(CreatePoListFromRealisierteMassnahmeSummarsich(FilterByErfassungsPeriodVonBis(summarich, jahrVon, notCurrentYear)));
                result.AddRange(CreatePoListFromRealisierteMassnahme(FilterByErfassungsPeriodVonBis(strassennamen, jahrVon, notCurrentYear)));
                result.AddRange(CreatePoFromRealisierteMassnahmeGIS(FilterByErfassungsPeriodVonBis(gis, jahrVon, notCurrentYear)));
                switch (erfassungPeriodBis.NetzErfassungsmodus)
                {
                    case NetzErfassungsmodus.Summarisch:
                        result.AddRange(CreatePoListFromRealisierteMassnahmeSummarsich(summarich.Where(s => s.ErfassungsPeriod.Id == erfassungPeriodBis.Id)));
                        break;
                    case NetzErfassungsmodus.Tabellarisch:
                        result.AddRange(CreatePoListFromRealisierteMassnahme(strassennamen.Where(s => s.ErfassungsPeriod.Id == erfassungPeriodBis.Id)));
                        break;
                    case NetzErfassungsmodus.Gis:
                        result.AddRange(CreatePoFromRealisierteMassnahmeGIS(gis.Where(s => s.ErfassungsPeriod.Id == erfassungPeriodBis.Id)));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                result.AddRange(CreatePoListFromRealisierteMassnahmeSummarsich(FilterByErfassungsPeriodVonBis(summarich, jahrVon, jahrBis)));
                result.AddRange(CreatePoListFromRealisierteMassnahme(FilterByErfassungsPeriodVonBis(strassennamen, jahrVon, jahrBis)));
                result.AddRange(CreatePoFromRealisierteMassnahmeGIS(FilterByErfassungsPeriodVonBis(gis, jahrVon, jahrBis)));
            }
            result = result.OrderByDescending(r => r.AusfuehrungsEnde).ThenBy(r => r.Projektname).ToList();

            return result;
        }

        private IEnumerable<TReportPo> CreatePoFromRealisierteMassnahmeGIS(IQueryable<RealisierteMassnahmeGIS> gis)
        {
            return gis.Fetch(rms => rms.Belastungskategorie)
                .Fetch(rms => rms.MassnahmenvorschlagFahrbahn)
                .ToList().Select(CreatePoFromRealisierteMassnahmeGIS);
        }

        private IEnumerable<TReportPo> CreatePoListFromRealisierteMassnahme(IQueryable<RealisierteMassnahme> strassennamen)
        {
            return strassennamen.Fetch(rms => rms.Belastungskategorie)
                .Fetch(rms => rms.MassnahmenvorschlagFahrbahn)
                .ToList().Select(CreatePoFromRealisierteMassnahme);
        }

        private IEnumerable<TReportPo> CreatePoListFromRealisierteMassnahmeSummarsich(IQueryable<RealisierteMassnahmeSummarsich> summarich)
        {
            return summarich.Fetch(rms => rms.Belastungskategorie).ToList().Select(CreatePoFromRealisierteMassnahmeSummarsich);
        }

        protected override bool IsForClosedErfassungsPeriod(TReportParameter parameter)
        {
            return erfassungPeriodBis.IsClosed && erfassungPeriodVon.IsClosed;
        }

        private IQueryable<T> FilterByErfassungsPeriodVonBis<T>(IQueryable<T> souce, DateTime von, DateTime bis) where T : IErfassungsPeriodDependentEntity
        {
            return souce.Where(s => von <= s.ErfassungsPeriod.Erfassungsjahr && s.ErfassungsPeriod.Erfassungsjahr <= bis);
        }

        private string GetJahr(Guid jahrId)
        {
            var kenngroessenFruehererJahreVon = kenngroessenFruehererJahreService.GetEntityById(jahrId);
            if (kenngroessenFruehererJahreVon != null)
                return kenngroessenFruehererJahreVon.Jahr.ToString();

            return LocalizeErfassungsPeriod(jahrId);
        }

        protected virtual TReportPo CreatePoFromRealisierteMassnahmeSummarsich(RealisierteMassnahmeSummarsich realisierteMassnahme)
        {
            var result = CreatePoFromEntityWithCopyingMatchingProperties(realisierteMassnahme);
            SetAusfuehrungsEndeForClosedPeriods(realisierteMassnahme, result);
            return result;
        }
        
        protected virtual TReportPo CreatePoFromRealisierteMassnahme(RealisierteMassnahme realisierteMassnahme)
        {
            var result = CreatePoFromEntityWithCopyingMatchingProperties(realisierteMassnahme);
            SetAusfuehrungsEndeForClosedPeriods(realisierteMassnahme, result);
            return result;
        }

        protected virtual TReportPo CreatePoFromRealisierteMassnahmeGIS(RealisierteMassnahmeGIS realisierteMassnahme)
        {
            var result = CreatePoFromEntityWithCopyingMatchingProperties(realisierteMassnahme);
            SetAusfuehrungsEndeForClosedPeriods(realisierteMassnahme, result);
            return result;
        }

        private static void SetAusfuehrungsEndeForClosedPeriods(IErfassungsPeriodDependentEntity realisierteMassnahme, TReportPo result)
        {
            if (realisierteMassnahme.ErfassungsPeriod.IsClosed)
                result.AusfuehrungsEnde = realisierteMassnahme.ErfassungsPeriod.Erfassungsjahr;
        }

        protected override void BuildFilterList(IFilterListBuilder<TReportParameter> filterListBuilder)
        {
            base.BuildFilterList(filterListBuilder);
            filterListBuilder.AddFilterListItem(p => p.ErfassungsPeriodIdVon, p => GetJahr(p.ErfassungsPeriodIdVon));
            filterListBuilder.AddFilterListItem(p => p.ErfassungsPeriodIdBis, p => GetJahr(p.ErfassungsPeriodIdBis));
        }
    }
}