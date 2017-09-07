using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Utils;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Reports.ZustandProZustandsabschnitt
{
    public interface IZustandProZustandsabschnittPoProvider : IPoProvider
    {
    }

    [ReportInfo(AuswertungTyp.W3_5, NetzErfassungsmodus = NetzErfassungsmodus.Tabellarisch)]
    [ReportInfo(AuswertungTyp.W3_7, NetzErfassungsmodus = NetzErfassungsmodus.Gis)]
    public class ZustandProZustandsabschnittPoProvider : EmsgGisTablePoProviderBase<ZustandProZustandsabschnittParameter, ZustandProZustandsabschnittPo, ZustandsabschnittGIS>, IZustandProZustandsabschnittPoProvider
    {
        private readonly IZustandProZustandsabschnittMapProvider zustandProZustandsabschnittMapProvider;
        private readonly IFiltererFactory filtererFactory;
        private readonly ITransactionScopeProvider transactionScopeProvider;
        private readonly IServerConfigurationProvider serverConfigurationProvider;

        public ZustandProZustandsabschnittPoProvider(
            IZustandProZustandsabschnittMapProvider zustandProZustandsabschnittMapProvider, 
            IFiltererFactory filtererFactory, 
            ITransactionScopeProvider transactionScopeProvider,
            IServerConfigurationProvider serverConfigurationProvider
            )
        {
            this.zustandProZustandsabschnittMapProvider = zustandProZustandsabschnittMapProvider;
            this.filtererFactory = filtererFactory;
            this.transactionScopeProvider = transactionScopeProvider;
            this.serverConfigurationProvider = serverConfigurationProvider;
        }

        protected override List<ZustandProZustandsabschnittPo> GetPresentationObjectListForSummarisch(ZustandProZustandsabschnittParameter parameter)
        {
            return NotSupported();
        }

        protected override List<ZustandProZustandsabschnittPo> GetPresentationObjectListForTabellarisch(ZustandProZustandsabschnittParameter parameter)
        {
            var queryable = filtererFactory.CreateFilterer<Zustandsabschnitt>(parameter).Filter(transactionScopeProvider.Queryable<Zustandsabschnitt>());
            return queryable.Fetch(z => z.Strassenabschnitt).ThenFetch(s => s.Belastungskategorie).ToList().Select(CreatePo)
                .OrderBy(z => z.Strassenname)
                .ThenBy(z => z.Strassenabschnittsnummer)
                .ThenBy(z => z.StrasseBezeichnungVon)
                .ThenBy(z => z.Abschnittsnummer)
                .ThenBy(z => z.BezeichnungVon)
                .ToList();
        }

        protected override List<ZustandProZustandsabschnittPo> GetPresentationObjectListForGis(ZustandProZustandsabschnittParameter parameter)
        {
            ZustandsabschnittGIS za = null;
            StrassenabschnittGIS sa = null;
            var joinAlias = transactionScopeProvider.CurrentTransactionScope.Session.QueryOver(() => za).JoinAlias(() => za.StrassenabschnittGIS, () => sa);
            var queryOver = filtererFactory.CreateFilterer<ZustandsabschnittGIS>(parameter).Filter(joinAlias);
            
            if (ErfassungsPeriodService.GetCurrentErfassungsPeriod().NetzErfassungsmodus == NetzErfassungsmodus.Gis)
                queryOver = zustandProZustandsabschnittMapProvider.FilterForBoundingBox(queryOver, parameter);
            
            return queryOver.Fetch(z => z.StrassenabschnittGIS).Eager.Fetch(z => z.StrassenabschnittGIS.Belastungskategorie).Eager.List<ZustandsabschnittGIS>().Select(CreatePo)
                 .OrderBy(z => z.Strassenname)
                .ThenBy(z => z.Strassenabschnittsnummer)
                .ThenBy(z => z.StrasseBezeichnungVon)
                .ThenBy(z => z.Abschnittsnummer)
                .ThenBy(z => z.BezeichnungVon)
                .ToList();
        }

        private ZustandProZustandsabschnittPo CreatePo(ZustandsabschnittBase zustandsabschnittBase)
        {
            var result = CreatePoFromEntityWithCopyingMatchingProperties(zustandsabschnittBase);

            result.Strassenabschnittsnummer = zustandsabschnittBase.StrassenabschnittBase.Abschnittsnummer;
            result.StrasseBezeichnungVon = zustandsabschnittBase.StrassenabschnittBase.BezeichnungVon;
            result.StrasseBezeichnungBis = zustandsabschnittBase.StrassenabschnittBase.BezeichnungBis;
            result.Ortsbezeichnung = zustandsabschnittBase.StrassenabschnittBase.Ortsbezeichnung;
            result.BemerkungShort =
                zustandsabschnittBase.Bemerkung.TrimToMaxLength(serverConfigurationProvider.BemerkungMaxDisplayLength);

            result.BelagBezeichnung = LocalizationService.GetLocalizedEnum(zustandsabschnittBase.StrassenabschnittBase.Belag, LocalizationType.Short);
            result.BelastungskategorieBezeichnung = LocalizationService.GetLocalizedBelastungskategorieTyp(result.BelastungskategorieTyp);
            result.StrasseneigentuemerBezeichnung = LocalizationService.GetLocalizedEnum(zustandsabschnittBase.StrassenabschnittBase.Strasseneigentuemer);
            result.ZustandsindexTrottoirLinksBezeichnung = LocalizationService.GetLocalizedEnum(result.ZustandsindexTrottoirLinks);
            result.ZustandsindexTrottoirRechtsBezeichnung = LocalizationService.GetLocalizedEnum(result.ZustandsindexTrottoirRechts);

            return result;
        }

        protected override PaperType PaperType{ get { return PaperType.A3Landscape; } }

        protected override void BuildFilterList(IFilterListBuilder<ZustandProZustandsabschnittParameter> filterListBuilder)
        {
            base.BuildFilterList(filterListBuilder);
            AddErfassungsPeriodFilterListItem(filterListBuilder);
            filterListBuilder.AddFilterListItem(p => p.Eigentuemer);
            filterListBuilder.AddFilterListItem(p => p.Strassenname);
            filterListBuilder.AddFilterListItem(p => p.ZustandsindexVon);
            filterListBuilder.AddFilterListItem(p => p.ZustandsindexBis);
            filterListBuilder.AddFilterListItem(p => p.Ortsbezeichnung);
        }
    }
}