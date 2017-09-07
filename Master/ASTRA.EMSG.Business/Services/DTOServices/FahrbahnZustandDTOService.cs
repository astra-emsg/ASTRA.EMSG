using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common.Enums;
using NHibernate.Linq;
using NHibernate.Util;
using ASTRA.EMSG.Common.DataTransferObjects;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities;
using Iesi.Collections;

namespace ASTRA.EMSG.Business.Services.DTOServices
{
    public interface IFahrbahnZustandDTOService : IService
    {
        void UpdateFahrbahnZustand(FahrBahnZustandDTO zustand);
    }
    class FahrbahnZustandDTOService : IFahrbahnZustandDTOService
    {
        private readonly IZustandsabschnittGISDTOService zustandsabschnittGISDTOService;
        private readonly ISchadendetailDTOService schadendetailDTOService;
        private readonly ISchadengruppeDTOService schadengruppeDTOService;
        private readonly ITransactionScopeProvider transactionScopeProvider;

        public FahrbahnZustandDTOService(IZustandsabschnittGISDTOService zustandsabschnittGISDTOService, 
            ISchadendetailDTOService schadendetailDTOService, 
            ISchadengruppeDTOService schadengruppeDTOService, 
            ITransactionScopeProvider transactionScopeProvider)
        {
            this.zustandsabschnittGISDTOService = zustandsabschnittGISDTOService;
            this.schadendetailDTOService = schadendetailDTOService;
            this.schadengruppeDTOService = schadengruppeDTOService;
            this.transactionScopeProvider = transactionScopeProvider;
        }
        public void UpdateFahrbahnZustand(FahrBahnZustandDTO zustand)
        {
            ZustandsabschnittBase zustandsabschnittBase = zustandsabschnittGISDTOService.GetZustandsabschnittById(zustand.ZustandsAbschnitt);
            DeleteSchadenData(zustandsabschnittBase);
            switch (zustand.Erfassungsmodus)
            {
                case ZustandsErfassungsmodus.Manuel:
                    
                    break;
                case ZustandsErfassungsmodus.Grob:
                    GetSchadengruppen(zustand.Schadengruppen).ForEach(zustandsabschnittBase.AddSchadengruppe);
                    break;
                case ZustandsErfassungsmodus.Detail:
                    CreateSchadendetails(zustand.Schadendetails).ForEach(zustandsabschnittBase.AddSchadendetail);
                    break;
            }
            zustandsabschnittBase.Erfassungsmodus = zustand.Erfassungsmodus;
            zustandsabschnittBase.Zustandsindex = zustand.Zustandsindex;
            transactionScopeProvider.Update((ZustandsabschnittGIS)zustandsabschnittBase);


        }
        private void DeleteSchadenData(ZustandsabschnittBase zustandsabschnittBase)
        {
            zustandsabschnittBase.Schadengruppen.ForEach(transactionScopeProvider.Delete);
            zustandsabschnittBase.Schadendetails.ForEach(transactionScopeProvider.Delete);
            zustandsabschnittBase.DeleteSchadenFormData();
        }
        private IEnumerable<Schadendetail> CreateSchadendetails(IEnumerable<SchadendetailDTO> schadendetailDTO)
        {
            return schadendetailDTO.Select(sd => new Schadendetail
            {
                SchadendetailTyp = sd.SchadendetailTyp,
                SchadenschwereTyp = sd.SchadenausmassTyp == SchadenausmassTyp.A0 ? SchadenschwereTyp.S1 : sd.SchadenschwereTyp,
                SchadenausmassTyp = sd.SchadenausmassTyp,
            });
        }
        private IEnumerable<Schadengruppe> GetSchadengruppen(IEnumerable<SchadengruppeDTO> schadengruppeDTO)
        {
            return schadengruppeDTO.Select(sgm => new Schadengruppe
            {
                SchadengruppeTyp = sgm.SchadengruppeTyp,
                SchadenschwereTyp = sgm.SchadenausmassTyp == SchadenausmassTyp.A0 ? SchadenschwereTyp.S1 : sgm.SchadenschwereTyp,
                SchadenausmassTyp = sgm.SchadenausmassTyp,
            });
        }
    }
}
