using System;
using System.Collections.Generic;
using System.Linq;
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
    public interface IEineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISPoProvider : IPoProvider
    {
    }
    
    [ReportInfo(AuswertungTyp.W5_1, NetzErfassungsmodus = NetzErfassungsmodus.Gis)]
    public class EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISPoProvider : RealisiertenMassnahmenGeordnetNachJahren<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISParameter,
                                                                                         EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISPo>, IEineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISPoProvider
    {
        public EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISPoProvider(IFiltererFactory filtererFactory, ITransactionScopeProvider transactionScopeProvider, IKenngroessenFruehererJahreService kenngroessenFruehererJahreService)
            : base(filtererFactory, transactionScopeProvider, kenngroessenFruehererJahreService)
        {
        }

        protected override List<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISPo> GetPresentationObjectListForSummarisch(EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISParameter parameter)
        {
            return NotSupported();
        }

        protected override List<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISPo> GetPresentationObjectListForTabellarisch(EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISParameter parameter)
        {
            return NotSupported();
        }

        protected override List<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISPo> GetPresentationObjectListForGis(EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISParameter parameter)
        {
            return CreatePoList(parameter);
        }

        protected override EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISPo CreatePoFromRealisierteMassnahmeSummarsich(RealisierteMassnahmeSummarsich realisierteMassnahme)
        {
            var result = base.CreatePoFromRealisierteMassnahmeSummarsich(realisierteMassnahme);
            result.FlaecheFahrbahn = realisierteMassnahme.Fahrbahnflaeche;
            return result;
        }

        protected override EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISPo CreatePoFromRealisierteMassnahme(RealisierteMassnahme realisierteMassnahme)
        {
            var result = base.CreatePoFromRealisierteMassnahme(realisierteMassnahme);
            if (realisierteMassnahme.MassnahmenvorschlagFahrbahn != null)
                result.MassnahmenbeschreibungFahrbahnTyp = LocalizationService.GetLocalizedMassnahmenvorschlagTyp(realisierteMassnahme.MassnahmenvorschlagFahrbahn.Typ);
            
            if (realisierteMassnahme.KostenFahrbahn.HasValue || realisierteMassnahme.KostenTrottoirLinks.HasValue || realisierteMassnahme.KostenTrottoirRechts.HasValue)
                result.KostenFahrbahn = (realisierteMassnahme.KostenFahrbahn ?? 0) + (realisierteMassnahme.KostenTrottoirLinks ?? 0) + (realisierteMassnahme.KostenTrottoirRechts ?? 0);
            
            return result;
        }

        protected override EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISPo CreatePoFromRealisierteMassnahmeGIS(RealisierteMassnahmeGIS realisierteMassnahme)
        {
            var result = base.CreatePoFromRealisierteMassnahmeGIS(realisierteMassnahme);
            if (realisierteMassnahme.MassnahmenvorschlagFahrbahn != null)
                result.MassnahmenbeschreibungFahrbahnTyp = LocalizationService.GetLocalizedMassnahmenvorschlagTyp(realisierteMassnahme.MassnahmenvorschlagFahrbahn.Typ);
            result.BeteiligteSystemeListe = string.Join(", ", realisierteMassnahme.BeteiligteSysteme.Select(bs => LocalizationService.GetLocalizedEnum(bs, LocalizationType.Short)));
            
            if (realisierteMassnahme.KostenFahrbahn.HasValue || realisierteMassnahme.KostenTrottoirLinks.HasValue || realisierteMassnahme.KostenTrottoirRechts.HasValue)
                result.KostenFahrbahn = (realisierteMassnahme.KostenFahrbahn ?? 0) + (realisierteMassnahme.KostenTrottoirLinks ?? 0) + (realisierteMassnahme.KostenTrottoirRechts ?? 0);
            
            return result;
        }

        protected override PaperType PaperType { get { return PaperType.A3Landscape; } }

        protected override void BuildFilterList(IFilterListBuilder<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISParameter> filterListBuilder)
        {
            base.BuildFilterList(filterListBuilder);
            filterListBuilder.AddFilterListItem(p => p.Projektname);
            filterListBuilder.AddFilterListItem(p => p.LeitendeOrganisation);
        }
    }
}