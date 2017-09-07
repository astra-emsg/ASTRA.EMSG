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
using ASTRA.EMSG.Common.Utils;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Reports.MassnahmenvorschlagProZustandsabschnitt
{
    public interface IMassnahmenvorschlagProZustandsabschnittPoProvider : IPoProvider
    {
    }

    [ReportInfo(AuswertungTyp.W3_8, NetzErfassungsmodus = NetzErfassungsmodus.Gis)]
    [ReportInfo(AuswertungTyp.W3_6, NetzErfassungsmodus = NetzErfassungsmodus.Tabellarisch)]
    public class MassnahmenvorschlagProZustandsabschnittPoProvider : EmsgGisTablePoProviderBase<MassnahmenvorschlagProZustandsabschnittParameter, MassnahmenvorschlagProZustandsabschnittPo, ZustandsabschnittGIS>, IMassnahmenvorschlagProZustandsabschnittPoProvider
    {
        private readonly IMassnahmenvorschlagProZustandsabschnittMapProvider massnahmenvorschlagProZustandsabschnittMapProvider;
        private readonly IFiltererFactory filtererFactory;
        private readonly ITransactionScopeProvider transactionScopeProvider;
        private readonly IServerConfigurationProvider serverConfigurationProvider;

        public MassnahmenvorschlagProZustandsabschnittPoProvider(
            IMassnahmenvorschlagProZustandsabschnittMapProvider massnahmenvorschlagProZustandsabschnittMapProvider, 
            IFiltererFactory filtererFactory, 
            ITransactionScopeProvider transactionScopeProvider,
            IServerConfigurationProvider serverConfigurationProvider
            )
        {
            this.massnahmenvorschlagProZustandsabschnittMapProvider = massnahmenvorschlagProZustandsabschnittMapProvider;
            this.filtererFactory = filtererFactory;
            this.transactionScopeProvider = transactionScopeProvider;
            this.serverConfigurationProvider = serverConfigurationProvider;
        }

        protected override List<MassnahmenvorschlagProZustandsabschnittPo> GetPresentationObjectListForSummarisch(MassnahmenvorschlagProZustandsabschnittParameter parameter)
        {
            return NotSupported();
        }

        protected override List<MassnahmenvorschlagProZustandsabschnittPo> GetPresentationObjectListForTabellarisch(MassnahmenvorschlagProZustandsabschnittParameter parameter)
        {
            var queryable = filtererFactory.CreateFilterer<Zustandsabschnitt>(parameter)
                .Filter(transactionScopeProvider.Queryable<Zustandsabschnitt>())
                .Where(z => z.MassnahmenvorschlagFahrbahn != null || z.MassnahmenvorschlagTrottoirLinks != null || z.MassnahmenvorschlagTrottoirRechts != null);

            var fetchedQuery = queryable
                .Fetch(z => z.Strassenabschnitt)
                .Fetch(z => z.MassnahmenvorschlagFahrbahn)
                .Fetch(z => z.MassnahmenvorschlagTrottoirLinks)
                .Fetch(z => z.MassnahmenvorschlagTrottoirRechts);

            return fetchedQuery.ToList().Select(CreatePo)
                .OrderBy(z => z.Strassenname)
                .ThenBy(z => z.Strassenabschnittsnummer)
                .ThenBy(z => z.StrasseBezeichnungVon)
                .ThenBy(z => z.Abschnittsnummer)
                .ThenBy(z => z.BezeichnungVon)
                .ToList();
        }
        
        protected override List<MassnahmenvorschlagProZustandsabschnittPo> GetPresentationObjectListForGis(MassnahmenvorschlagProZustandsabschnittParameter parameter)
        {
            ZustandsabschnittGIS za = null;
            StrassenabschnittGIS sa = null;
            var joinAlias = transactionScopeProvider.CurrentTransactionScope.Session.QueryOver(() => za).JoinAlias(() => za.StrassenabschnittGIS, () => sa);
            var queryOver = filtererFactory.CreateFilterer<ZustandsabschnittGIS>(parameter).Filter(joinAlias)
                .Where(z => z.MassnahmenvorschlagFahrbahn != null || z.MassnahmenvorschlagTrottoirLinks != null || z.MassnahmenvorschlagTrottoirRechts != null);
            
            if (ErfassungsPeriodService.GetCurrentErfassungsPeriod().NetzErfassungsmodus == NetzErfassungsmodus.Gis)
                queryOver = massnahmenvorschlagProZustandsabschnittMapProvider.FilterForBoundingBox(queryOver, parameter);

            var fetchedQueryOver = queryOver
                .Fetch(z => z.StrassenabschnittGIS).Eager
                .Fetch(z => z.MassnahmenvorschlagFahrbahn).Eager
                .Fetch(z => z.MassnahmenvorschlagTrottoirLinks).Eager
                .Fetch(z => z.MassnahmenvorschlagTrottoirRechts).Eager;

            return fetchedQueryOver.List<ZustandsabschnittGIS>().Select(CreatePo)
                .OrderBy(z => z.Strassenname)
                .ThenBy(z => z.Strassenabschnittsnummer)
                .ThenBy(z => z.StrasseBezeichnungVon)
                .ThenBy(z => z.Abschnittsnummer)
                .ThenBy(z => z.BezeichnungVon)
                .ToList();
        }

        private MassnahmenvorschlagProZustandsabschnittPo CreatePo(ZustandsabschnittBase zustandsabschnittBase)
        {
            var result = CreatePoFromEntityWithCopyingMatchingProperties(zustandsabschnittBase);

            result.Strassenabschnittsnummer = zustandsabschnittBase.StrassenabschnittBase.Abschnittsnummer;
            result.StrasseBezeichnungVon = zustandsabschnittBase.StrassenabschnittBase.BezeichnungVon;
            result.StrasseBezeichnungBis = zustandsabschnittBase.StrassenabschnittBase.BezeichnungBis;
            result.Ortsbezeichnung = zustandsabschnittBase.StrassenabschnittBase.Ortsbezeichnung;
            result.BemerkungShort = zustandsabschnittBase.Bemerkung.TrimToMaxLength(serverConfigurationProvider.BemerkungMaxDisplayLength);

            result.StrasseneigentuemerBezeichnung = LocalizationService.GetLocalizedEnum(zustandsabschnittBase.StrassenabschnittBase.Strasseneigentuemer);
            result.ZustandsindexTrottoirLinksBezeichnung = LocalizationService.GetLocalizedEnum(result.ZustandsindexTrottoirLinks);
            result.ZustandsindexTrottoirRechtsBezeichnung = LocalizationService.GetLocalizedEnum(result.ZustandsindexTrottoirRechts);

            result.DringlichkeitFahrbahnBezeichnung = LocalizationService.GetLocalizedEnum(result.DringlichkeitFahrbahn);
            result.DringlichkeitTrottoirLinksBezeichnung = LocalizationService.GetLocalizedEnum(result.DringlichkeitTrottoirLinks);
            result.DringlichkeitTrottoirRechtsBezeichnung = LocalizationService.GetLocalizedEnum(result.DringlichkeitTrottoirRechts);

            result.MassnahmenvorschlagKatalogTypFahrbahnBezeichnung = result.MassnahmenvorschlagKatalogTypFahrbahn.HasText() ? LocalizationService.GetLocalizedMassnahmenvorschlagTyp(result.MassnahmenvorschlagKatalogTypFahrbahn) : null;
            result.MassnahmenvorschlagKatalogTypTrottoirLinksBezeichnung = result.MassnahmenvorschlagKatalogTypTrottoirLinks.HasText() ? LocalizationService.GetLocalizedMassnahmenvorschlagTyp(result.MassnahmenvorschlagKatalogTypTrottoirLinks) : null;
            result.MassnahmenvorschlagKatalogTypTrottoirRechtsBezeichnung = result.MassnahmenvorschlagKatalogTypTrottoirRechts.HasText() ? LocalizationService.GetLocalizedMassnahmenvorschlagTyp(result.MassnahmenvorschlagKatalogTypTrottoirRechts) : null;

            return result;
        }

        protected override PaperType PaperType { get { return PaperType.A3Landscape; } }

        protected override void BuildFilterList(IFilterListBuilder<MassnahmenvorschlagProZustandsabschnittParameter> filterListBuilder)
        {
            base.BuildFilterList(filterListBuilder);
            AddErfassungsPeriodFilterListItem(filterListBuilder);
            filterListBuilder.AddFilterListItem(p => p.Eigentuemer);
            filterListBuilder.AddFilterListItem(p => p.Dringlichkeit);
            filterListBuilder.AddFilterListItem(p => p.Strassenname);
            filterListBuilder.AddFilterListItem(p => p.ZustandsindexVon);
            filterListBuilder.AddFilterListItem(p => p.ZustandsindexBis);
            filterListBuilder.AddFilterListItem(p => p.Ortsbezeichnung);
        }

    }
}