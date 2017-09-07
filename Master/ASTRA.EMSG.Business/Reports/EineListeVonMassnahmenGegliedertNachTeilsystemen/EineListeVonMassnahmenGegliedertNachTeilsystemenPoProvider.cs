using System.Linq;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.EineListeVonMassnahmenGegliedertNachTeilsystemen
{
    public interface IEineListeVonMassnahmenGegliedertNachTeilsystemenPoProvider : IPoProvider
    {
    }

    [ReportInfo(AuswertungTyp.W4_1)]
    [ReportInfo(AuswertungTyp.W4_2, NetzErfassungsmodus = NetzErfassungsmodus.Gis)]
    public class EineListeVonMassnahmenGegliedertNachTeilsystemenPoProvider: EmsgGisTablePoProviderBase<EineListeVonMassnahmenGegliedertNachTeilsystemenParameter, EineListeVonMassnahmenGegliedertNachTeilsystemenPo, MassnahmenvorschlagTeilsystemeGIS>, IEineListeVonMassnahmenGegliedertNachTeilsystemenPoProvider
    {
        private readonly IFiltererFactory filtererFactory;
        private readonly ITransactionScopeProvider transactionScopeProvider;
        private readonly IEineListeVonMassnahmenGegliedertNachTeilsystemenMapProvider eineListeVonMassnahmenGegliedertNachTeilsystemenMapProvider;

        public EineListeVonMassnahmenGegliedertNachTeilsystemenPoProvider(IEineListeVonMassnahmenGegliedertNachTeilsystemenMapProvider eineListeVonMassnahmenGegliedertNachTeilsystemenMapProvider, IFiltererFactory filtererFactory, ITransactionScopeProvider transactionScopeProvider)
        {
            this.eineListeVonMassnahmenGegliedertNachTeilsystemenMapProvider = eineListeVonMassnahmenGegliedertNachTeilsystemenMapProvider;
            this.filtererFactory = filtererFactory;
            this.transactionScopeProvider = transactionScopeProvider;
        }

        protected override List<EineListeVonMassnahmenGegliedertNachTeilsystemenPo> GetPresentationObjectListForSummarisch(EineListeVonMassnahmenGegliedertNachTeilsystemenParameter parameter)
        {
            return NotSupported();
        }

        protected override List<EineListeVonMassnahmenGegliedertNachTeilsystemenPo> GetPresentationObjectListForTabellarisch(EineListeVonMassnahmenGegliedertNachTeilsystemenParameter parameter)
        {
            return NotSupported();           
        }

        protected override List<EineListeVonMassnahmenGegliedertNachTeilsystemenPo> GetPresentationObjectListForGis(EineListeVonMassnahmenGegliedertNachTeilsystemenParameter parameter)
        {
            var queryOver = filtererFactory.CreateFilterer<MassnahmenvorschlagTeilsystemeGIS>(parameter).Filter(transactionScopeProvider.QueryOver<MassnahmenvorschlagTeilsystemeGIS>());
            queryOver = eineListeVonMassnahmenGegliedertNachTeilsystemenMapProvider.FilterForBoundingBox(queryOver, parameter);
            return queryOver.List<MassnahmenvorschlagTeilsystemeGIS>().OrderBy(s => s.Teilsystem).ThenBy(s => s.Projektname).Select(CreatePo).ToList();
        }

        private EineListeVonMassnahmenGegliedertNachTeilsystemenPo CreatePo(MassnahmenvorschlagTeilsystemeGIS massnahmenvorschlagTeilsystemeGIS)
        {
            var result = CreatePoFromEntityWithCopyingMatchingProperties(massnahmenvorschlagTeilsystemeGIS);
            result.StatusBezeichnung = LocalizationService.GetLocalizedEnum(massnahmenvorschlagTeilsystemeGIS.Status);
            result.DringlichkeitBezeichnung = LocalizationService.GetLocalizedEnum(massnahmenvorschlagTeilsystemeGIS.Dringlichkeit);
            result.TeilsystemBezeichnung = LocalizationService.GetLocalizedEnum(massnahmenvorschlagTeilsystemeGIS.Teilsystem);
            return result;
        }

        protected override PaperType PaperType { get { return PaperType.A4Landscape; } }

        protected override void BuildFilterList(IFilterListBuilder<EineListeVonMassnahmenGegliedertNachTeilsystemenParameter> filterListBuilder)
        {
            base.BuildFilterList(filterListBuilder);
            AddErfassungsPeriodFilterListItem(filterListBuilder);
            filterListBuilder.AddFilterListItem(p => p.Status);
            filterListBuilder.AddFilterListItem(p => p.Dringlichkeit);
            filterListBuilder.AddFilterListItem(p => p.Teilsystem);
            filterListBuilder.AddFilterListItem(p => p.Projektname); 
        }
    }
}