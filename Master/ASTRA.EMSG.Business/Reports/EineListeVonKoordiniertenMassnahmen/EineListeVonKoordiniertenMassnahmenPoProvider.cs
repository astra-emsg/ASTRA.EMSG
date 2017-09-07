using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Common.Enums;
using Environment = System.Environment;

namespace ASTRA.EMSG.Business.Reports.EineListeVonKoordiniertenMassnahmen
{
    public interface IEineListeVonKoordiniertenMassnahmenPoProvider : IPoProvider
    {
    }

    [ReportInfo(AuswertungTyp.W4_3)]
    [ReportInfo(AuswertungTyp.W4_4, NetzErfassungsmodus = NetzErfassungsmodus.Gis)]
    public class EineListeVonKoordiniertenMassnahmenPoProvider : EmsgGisTablePoProviderBase<EineListeVonKoordiniertenMassnahmenParameter, EineListeVonKoordiniertenMassnahmenPo, KoordinierteMassnahmeGIS>, IEineListeVonKoordiniertenMassnahmenPoProvider
    {
        private readonly IFiltererFactory filtererFactory;
        private readonly ITransactionScopeProvider transactionScopeProvider;
        private readonly IEineListeVonKoordiniertenMassnahmenMapProvider eineListeVonKoordiniertenMassnahmenMapProvider;

        public EineListeVonKoordiniertenMassnahmenPoProvider(IEineListeVonKoordiniertenMassnahmenMapProvider eineListeVonKoordiniertenMassnahmenMapProvider, IFiltererFactory filtererFactory, ITransactionScopeProvider transactionScopeProvider)
        {
            this.eineListeVonKoordiniertenMassnahmenMapProvider = eineListeVonKoordiniertenMassnahmenMapProvider;
            this.filtererFactory = filtererFactory;
            this.transactionScopeProvider = transactionScopeProvider;
        }

        protected override List<EineListeVonKoordiniertenMassnahmenPo> GetPresentationObjectListForSummarisch(EineListeVonKoordiniertenMassnahmenParameter parameter)
        {
            return NotSupported();
        }

        protected override List<EineListeVonKoordiniertenMassnahmenPo> GetPresentationObjectListForTabellarisch(EineListeVonKoordiniertenMassnahmenParameter parameter)
        {
            return NotSupported();
        }

        protected override List<EineListeVonKoordiniertenMassnahmenPo> GetPresentationObjectListForGis(EineListeVonKoordiniertenMassnahmenParameter parameter)
        {
            var queryOver = filtererFactory.CreateFilterer<KoordinierteMassnahmeGIS>(parameter).Filter(transactionScopeProvider.QueryOver<KoordinierteMassnahmeGIS>());
            queryOver = eineListeVonKoordiniertenMassnahmenMapProvider.FilterForBoundingBox(queryOver, parameter);
            return queryOver.List<KoordinierteMassnahmeGIS>().OrderBy(s => s.Projektname).Select(CreatePo).ToList();
        }

        private EineListeVonKoordiniertenMassnahmenPo CreatePo(KoordinierteMassnahmeGIS koordinierteMassnahmeGIS)
        {
            var result = CreatePoFromEntityWithCopyingMatchingProperties(koordinierteMassnahmeGIS);
            result.StatusBezeichnung = LocalizationService.GetLocalizedEnum(koordinierteMassnahmeGIS.Status);
            result.BeteiligteSystemeListe = string.Join(", ", koordinierteMassnahmeGIS.BeteiligteSysteme.Select(bs => LocalizationService.GetLocalizedEnum(bs, LocalizationType.Short)));
            return result;
        }

        protected override PaperType PaperType { get { return PaperType.A3Landscape; } }

        protected override void BuildFilterList(IFilterListBuilder<EineListeVonKoordiniertenMassnahmenParameter> filterListBuilder)
        {
            base.BuildFilterList(filterListBuilder);
            AddErfassungsPeriodFilterListItem(filterListBuilder);
            filterListBuilder.AddFilterListItem(p => p.Projektname);
            filterListBuilder.AddFilterListItem(p => p.Status);
            filterListBuilder.AddFilterListItem(p => p.AusfuehrungsanfangVon);
            filterListBuilder.AddFilterListItem(p => p.AusfuehrungsanfangBis);
        }
    }
}