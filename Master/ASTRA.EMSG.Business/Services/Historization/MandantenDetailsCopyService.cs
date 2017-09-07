using System;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Business.Services.EntityServices.Strassennamen;
using ASTRA.EMSG.Business.Services.EntityServices.Summarisch;
using ASTRA.EMSG.Common.Enums;
using System.Linq;

namespace ASTRA.EMSG.Business.Services.Historization
{
    public interface IMandantenDetailsCopyService : IService
    {
        void CopyMandantenDetailsData(ErfassungsPeriod closedPeriod);
    }

    public class MandantenDetailsCopyService : IMandantenDetailsCopyService
    {
        private readonly IMandantDetailsService mandantDetailsService;
        private readonly INetzSummarischDetailService netzSummarischDetailService;
        private readonly IStrassenabschnittService strassenabschnittService;
        private readonly IStrassenabschnittGISService strassenabschnittGISService;
        private readonly ITransactionScopeProvider transactionScopeProvider;

        public MandantenDetailsCopyService(
            IMandantDetailsService mandantDetailsService, 
            INetzSummarischDetailService netzSummarischDetailService, 
            IStrassenabschnittService strassenabschnittService, 
            IStrassenabschnittGISService strassenabschnittGISService,
            ITransactionScopeProvider transactionScopeProvider)
        {
            this.mandantDetailsService = mandantDetailsService;
            this.netzSummarischDetailService = netzSummarischDetailService;
            this.strassenabschnittService = strassenabschnittService;
            this.strassenabschnittGISService = strassenabschnittGISService;
            this.transactionScopeProvider = transactionScopeProvider;
        }

        public void CopyMandantenDetailsData(ErfassungsPeriod closedPeriod)
        {
            mandantDetailsService.CreateCopy(closedPeriod);

            MandantDetails mandantDetails = mandantDetailsService.GetEntitiesBy(closedPeriod).Single();

            switch (closedPeriod.NetzErfassungsmodus)
            {
                case NetzErfassungsmodus.Summarisch:
                    mandantDetails.NetzLaenge = netzSummarischDetailService.GetEntitiesBy(closedPeriod).Sum(nsd => nsd.Fahrbahnlaenge);
                    break;
                case NetzErfassungsmodus.Tabellarisch:
                    mandantDetails.NetzLaenge = strassenabschnittService.GetEntitiesBy(closedPeriod).Sum(sa => (decimal?)sa.Laenge) ?? 0;
                    break;
                case NetzErfassungsmodus.Gis:
                    mandantDetails.NetzLaenge = strassenabschnittGISService.GetEntitiesBy(closedPeriod).Sum(sa => (decimal?)sa.Laenge) ?? 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //NetzLaenge's unit is km
            mandantDetails.NetzLaenge /= 1000m;

            transactionScopeProvider.Update(mandantDetails);
        }
    }
}
