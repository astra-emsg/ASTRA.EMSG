using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.FilterBuilders;
using ASTRA.EMSG.Common.Enums;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Reports.StrassenabschnitteListe
{
    public interface IStrassenabschnitteListePoProvider : IPoProvider
    {
    }

    [ReportInfo(AuswertungTyp.W1_2, NetzErfassungsmodus = NetzErfassungsmodus.Tabellarisch)]
    [ReportInfo(AuswertungTyp.W1_6, NetzErfassungsmodus = NetzErfassungsmodus.Gis)]
    public class StrassenabschnitteListePoProvider : EmsgTablePoProviderBase<StrassenabschnitteListeParameter, StrassenabschnitteListePo>, IStrassenabschnitteListePoProvider
    {
        private readonly IStrassenabschnitteListeMapProvider strassenabschnitteListeMapProvider;
        private readonly IFiltererFactory filtererFactory;
        private readonly ITransactionScopeProvider transactionScopeProvider;
        private readonly IBelastungskategorieService belastungskategorieService;

        public StrassenabschnitteListePoProvider(
            IStrassenabschnitteListeMapProvider strassenabschnitteListeMapProvider,
            IFiltererFactory filtererFactory,
            ITransactionScopeProvider transactionScopeProvider,
            IBelastungskategorieService belastungskategorieService
            )
        {
            this.strassenabschnitteListeMapProvider = strassenabschnitteListeMapProvider;
            this.filtererFactory = filtererFactory;
            this.transactionScopeProvider = transactionScopeProvider;
            this.belastungskategorieService = belastungskategorieService;
        }

        protected override List<StrassenabschnitteListePo> GetPresentationObjectListForSummarisch(StrassenabschnitteListeParameter parameter)
        {
            return NotSupported();
        }

        protected override List<StrassenabschnitteListePo> GetPresentationObjectListForTabellarisch(StrassenabschnitteListeParameter parameter)
        {
            var queryable = filtererFactory.CreateFilterer<Strassenabschnitt>(parameter).Filter(transactionScopeProvider.Queryable<Strassenabschnitt>());

            return queryable.OrderBy(s => s.Strassenname).ThenBy(s => s.Abschnittsnummer).Fetch(s => s.Belastungskategorie).Select(CreatePo).OrderBy(s => s.Strassenname).ToList();
        }

        protected override List<StrassenabschnitteListePo> GetPresentationObjectListForGis(StrassenabschnitteListeParameter parameter)
        {
            var queryOver = filtererFactory.CreateFilterer<StrassenabschnittGIS>(parameter).Filter(transactionScopeProvider.QueryOver<StrassenabschnittGIS>());

            if (ErfassungsPeriodService.GetCurrentErfassungsPeriod().NetzErfassungsmodus == NetzErfassungsmodus.Gis)
                queryOver = strassenabschnitteListeMapProvider.FilterForBoundingBox(queryOver, parameter);

            return queryOver.Fetch(s => s.Belastungskategorie).Eager.List<StrassenabschnittGIS>().OrderBy(s => s.Strassenname).Select(CreatePo).ToList();
        }

        private StrassenabschnitteListePo CreatePo(StrassenabschnittBase strassenabschnittBase)
        {
            var result = CreatePoFromEntityWithCopyingMatchingProperties(strassenabschnittBase);
            result.BelastungskategorieBezeichnung = LocalizationService.GetLocalizedBelastungskategorieTyp(strassenabschnittBase.BelastungskategorieTyp);
            result.StrasseneigentuemerBezeichnung = LocalizationService.GetLocalizedEnum(strassenabschnittBase.Strasseneigentuemer);
            result.TrottoirBezeichnung = LocalizationService.GetLocalizedEnum(strassenabschnittBase.Trottoir);
            result.FlaecheFahrbahn = Math.Round(result.FlaecheFahrbahn ?? 0);
            if (!strassenabschnittBase.HasTrottoir)
            {
                result.FlaecheTrottoirLinks = null;
                result.FlaecheTrottoirRechts = null;
            }
            else
            {
                if (strassenabschnittBase.Trottoir == TrottoirTyp.Links)
                    result.FlaecheTrottoirRechts = null;
                if (strassenabschnittBase.Trottoir == TrottoirTyp.Rechts)
                    result.FlaecheTrottoirLinks = null;
            }
            return result;
        }

        protected override PaperType PaperType { get { return PaperType.A4Landscape; } }

        protected override void BuildFilterList(IFilterListBuilder<StrassenabschnitteListeParameter> filterListBuilder)
        {
            base.BuildFilterList(filterListBuilder);
            AddErfassungsPeriodFilterListItem(filterListBuilder);
            filterListBuilder.AddFilterListItem(p => p.Eigentuemer);
            filterListBuilder.AddFilterListItem(p => p.Belastungskategorie, GetLocalizedBelastungskategorieTyp);
            filterListBuilder.AddFilterListItem(p => p.Ortsbezeichnung);
        }

        protected string GetLocalizedBelastungskategorieTyp(IBelastungskategorieFilter belastungskategorieFilter)
        {
            if (!belastungskategorieFilter.Belastungskategorie.HasValue)
                return null;

            return LocalizationService.GetLocalizedBelastungskategorieTyp(belastungskategorieService.GetBelastungskategorie(belastungskategorieFilter.Belastungskategorie).Typ);
        }
    }
}