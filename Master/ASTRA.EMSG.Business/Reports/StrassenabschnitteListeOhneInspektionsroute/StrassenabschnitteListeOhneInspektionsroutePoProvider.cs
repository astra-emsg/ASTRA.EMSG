using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Common.Enums;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Reports.StrassenabschnitteListeOhneInspektionsroute
{
    public interface IStrassenabschnitteListeOhneInspektionsroutePoProvider : IPoProvider
    {
    }

    [ReportInfo(AuswertungTyp.W1_4, NetzErfassungsmodus = NetzErfassungsmodus.Gis)]
    public class StrassenabschnitteListeOhneInspektionsroutePoProvider : EmsgTablePoProviderBase<StrassenabschnitteListeOhneInspektionsrouteParameter, StrassenabschnitteListeOhneInspektionsroutePo>, IStrassenabschnitteListeOhneInspektionsroutePoProvider
    {
        private readonly ITransactionScopeProvider transactionScopeProvider;
        private readonly IFiltererFactory filtererFactory;

        public StrassenabschnitteListeOhneInspektionsroutePoProvider(ITransactionScopeProvider transactionScopeProvider, IFiltererFactory filtererFactory)
        {
            this.transactionScopeProvider = transactionScopeProvider;
            this.filtererFactory = filtererFactory;
        }

        protected override List<StrassenabschnitteListeOhneInspektionsroutePo> GetPresentationObjectListForSummarisch(StrassenabschnitteListeOhneInspektionsrouteParameter parameter)
        {
            return NotSupported();
        }

        protected override List<StrassenabschnitteListeOhneInspektionsroutePo> GetPresentationObjectListForTabellarisch(StrassenabschnitteListeOhneInspektionsrouteParameter parameter)
        {
            return NotSupported();
        }

        protected override List<StrassenabschnitteListeOhneInspektionsroutePo> GetPresentationObjectListForGis(StrassenabschnitteListeOhneInspektionsrouteParameter parameter)
        {
            var queryable = transactionScopeProvider.Queryable<StrassenabschnittGIS>().Where(sa => !sa.InspektionsRtStrAbschnitte.Any());
            queryable = filtererFactory.CreateFilterer<StrassenabschnittGIS>(parameter).Filter(queryable);
            return queryable.OrderBy(s => s.Strassenname).Fetch(s => s.Belastungskategorie).Select(CreatePo).ToList();
        }

        private StrassenabschnitteListeOhneInspektionsroutePo CreatePo(StrassenabschnittGIS strassenabschnittGIS)
        {
            var po = CreatePoFromEntityWithCopyingMatchingProperties(strassenabschnittGIS);
            po.BelastungskategorieBezeichnung = LocalizationService.GetLocalizedBelastungskategorieTyp(po.BelastungskategorieTyp);
            po.StrasseneigentuemerBezeichnung = LocalizationService.GetLocalizedEnum(po.Strasseneigentuemer);
            return po;
        }

        protected override void BuildFilterList(IFilterListBuilder<StrassenabschnitteListeOhneInspektionsrouteParameter> builder)
        {
            base.BuildFilterList(builder);
            AddErfassungsPeriodFilterListItem(builder);
            builder.AddFilterListItem(p => p.Eigentuemer);
            builder.AddFilterListItem(p => p.Strassenname);
        }

        protected override PaperType PaperType { get { return PaperType.A4Landscape; } }
    }
}