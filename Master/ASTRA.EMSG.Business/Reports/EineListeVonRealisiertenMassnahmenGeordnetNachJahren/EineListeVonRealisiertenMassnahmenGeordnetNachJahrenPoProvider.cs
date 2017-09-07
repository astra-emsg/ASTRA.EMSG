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
    public interface IEineListeVonRealisiertenMassnahmenGeordnetNachJahrenPoProvider : IPoProvider
    {
    }

    [ReportInfo(AuswertungTyp.W5_1, NetzErfassungsmodus = NetzErfassungsmodus.Tabellarisch)]
    public class EineListeVonRealisiertenMassnahmenGeordnetNachJahrenPoProvider : RealisiertenMassnahmenGeordnetNachJahren<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenParameter,
                                                                                        EineListeVonRealisiertenMassnahmenGeordnetNachJahrenPo>, IEineListeVonRealisiertenMassnahmenGeordnetNachJahrenPoProvider
    {
        public EineListeVonRealisiertenMassnahmenGeordnetNachJahrenPoProvider(IFiltererFactory filtererFactory, ITransactionScopeProvider transactionScopeProvider, IKenngroessenFruehererJahreService kenngroessenFruehererJahreService) : base(filtererFactory, transactionScopeProvider, kenngroessenFruehererJahreService)
        {
        }

        protected override List<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenPo> GetPresentationObjectListForSummarisch(EineListeVonRealisiertenMassnahmenGeordnetNachJahrenParameter parameter)
        {
            return NotSupported();
        }

        protected override List<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenPo> GetPresentationObjectListForTabellarisch(EineListeVonRealisiertenMassnahmenGeordnetNachJahrenParameter parameter)
        {
            return CreatePoList(parameter);
        }

        protected override List<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenPo> GetPresentationObjectListForGis(EineListeVonRealisiertenMassnahmenGeordnetNachJahrenParameter parameter)
        {
            return NotSupported();
        }

        protected override EineListeVonRealisiertenMassnahmenGeordnetNachJahrenPo CreatePoFromRealisierteMassnahmeSummarsich(RealisierteMassnahmeSummarsich realisierteMassnahme)
        {
            var result = base.CreatePoFromRealisierteMassnahmeSummarsich(realisierteMassnahme);
            result.FlaecheFahrbahn = realisierteMassnahme.Fahrbahnflaeche;
            return result;
        }

        protected override EineListeVonRealisiertenMassnahmenGeordnetNachJahrenPo CreatePoFromRealisierteMassnahme(RealisierteMassnahme realisierteMassnahme)
        {
            var result = base.CreatePoFromRealisierteMassnahme(realisierteMassnahme);
            if (realisierteMassnahme.MassnahmenvorschlagFahrbahn != null)
                result.MassnahmenbeschreibungFahrbahnTyp = LocalizationService.GetLocalizedMassnahmenvorschlagTyp(realisierteMassnahme.MassnahmenvorschlagFahrbahn.Typ);
    
            if (realisierteMassnahme.KostenFahrbahn.HasValue || realisierteMassnahme.KostenTrottoirLinks.HasValue || realisierteMassnahme.KostenTrottoirRechts.HasValue)
                result.KostenFahrbahn = (realisierteMassnahme.KostenFahrbahn ?? 0) + (realisierteMassnahme.KostenTrottoirLinks ?? 0) + (realisierteMassnahme.KostenTrottoirRechts ?? 0);
            
            return result;
        }

        protected override EineListeVonRealisiertenMassnahmenGeordnetNachJahrenPo CreatePoFromRealisierteMassnahmeGIS(RealisierteMassnahmeGIS realisierteMassnahme)
        {
            var result = base.CreatePoFromRealisierteMassnahmeGIS(realisierteMassnahme);
            if (realisierteMassnahme.MassnahmenvorschlagFahrbahn != null)
                result.MassnahmenbeschreibungFahrbahnTyp = LocalizationService.GetLocalizedMassnahmenvorschlagTyp(realisierteMassnahme.MassnahmenvorschlagFahrbahn.Typ);

            if (realisierteMassnahme.KostenFahrbahn.HasValue || realisierteMassnahme.KostenTrottoirLinks.HasValue || realisierteMassnahme.KostenTrottoirRechts.HasValue)
                result.KostenFahrbahn = (realisierteMassnahme.KostenFahrbahn ?? 0) + (realisierteMassnahme.KostenTrottoirLinks ?? 0) + (realisierteMassnahme.KostenTrottoirRechts ?? 0);
            
            return result;
        }

        protected override PaperType PaperType { get { return PaperType.A3Landscape; } }

        protected override void BuildFilterList(IFilterListBuilder<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenParameter> filterListBuilder)
        {
            base.BuildFilterList(filterListBuilder);
            filterListBuilder.AddFilterListItem(p => p.Projektname);
        }

    }
}
