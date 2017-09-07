using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Entities.Summarisch;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.EineListeVonRealisiertenMassnahmenGeordnetNachJahren
{
    public interface IEineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischPoProvider : IPoProvider
    {
    }

    [ReportInfo(AuswertungTyp.W5_1, NetzErfassungsmodus = NetzErfassungsmodus.Summarisch)]
    public class EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischPoProvider : RealisiertenMassnahmenGeordnetNachJahren<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischParameter,
                                                                                                EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischPo>, IEineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischPoProvider
    {
        public EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischPoProvider(IFiltererFactory filtererFactory, ITransactionScopeProvider transactionScopeProvider, IKenngroessenFruehererJahreService kenngroessenFruehererJahreService)
            : base(filtererFactory, transactionScopeProvider, kenngroessenFruehererJahreService)
        {
        }

        protected override List<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischPo> GetPresentationObjectListForSummarisch(EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischParameter parameter)
        {
            return CreatePoList(parameter);
        }

        protected override List<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischPo> GetPresentationObjectListForTabellarisch(EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischParameter parameter)
        {
            return NotSupported();
        }

        protected override List<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischPo> GetPresentationObjectListForGis(EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischParameter parameter)
        {
            return NotSupported();
        }

        protected override EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischPo CreatePoFromRealisierteMassnahmeSummarsich(RealisierteMassnahmeSummarsich realisierteMassnahme)
        {
            var result = base.CreatePoFromRealisierteMassnahmeSummarsich(realisierteMassnahme);
            result.FlaecheFahrbahn = realisierteMassnahme.Fahrbahnflaeche;
            if (realisierteMassnahme.BelastungskategorieTyp != null)
                result.BelastungskategorieTyp = LocalizationService.GetLocalizedBelastungskategorieTyp(realisierteMassnahme.BelastungskategorieTyp);
            return result;
        }

        protected override EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischPo CreatePoFromRealisierteMassnahme(RealisierteMassnahme realisierteMassnahme)
        {
            var result = base.CreatePoFromRealisierteMassnahme(realisierteMassnahme);
            
            if (realisierteMassnahme.KostenFahrbahn.HasValue || realisierteMassnahme.KostenTrottoirLinks.HasValue || realisierteMassnahme.KostenTrottoirRechts.HasValue)
                result.KostenFahrbahn = (realisierteMassnahme.KostenFahrbahn ?? 0) + (realisierteMassnahme.KostenTrottoirLinks ?? 0) + (realisierteMassnahme.KostenTrottoirRechts ?? 0);
            
            return result;
        }

        protected override EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischPo CreatePoFromRealisierteMassnahmeGIS(RealisierteMassnahmeGIS realisierteMassnahme)
        {
            var result = base.CreatePoFromRealisierteMassnahmeGIS(realisierteMassnahme);
            
            if (realisierteMassnahme.KostenFahrbahn.HasValue || realisierteMassnahme.KostenTrottoirLinks.HasValue || realisierteMassnahme.KostenTrottoirRechts.HasValue)
                result.KostenFahrbahn = (realisierteMassnahme.KostenFahrbahn ?? 0) + (realisierteMassnahme.KostenTrottoirLinks ?? 0) + (realisierteMassnahme.KostenTrottoirRechts ?? 0);
            
            return result;
        }

        protected override PaperType PaperType { get { return PaperType.A3Landscape; } }

        protected override void BuildFilterList(IFilterListBuilder<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischParameter> filterListBuilder)
        {
            base.BuildFilterList(filterListBuilder);
            filterListBuilder.AddFilterListItem(p => p.Projektname);
        }
    }
}