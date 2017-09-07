using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Common.DataTransferObjects;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;

namespace ASTRA.EMSG.Business.Services.DTOServices
{
    public interface IStrassenabschnittGISDTOService : IDTOServiceBase<StrassenabschnittGIS, StrassenabschnittGISDTO>
    { }
    class StrassenabschnittGISDTOService:DTOServiceBase<StrassenabschnittGIS, StrassenabschnittGISDTO>, IStrassenabschnittGISDTOService
    {
        private readonly IReferenzGruppeDTOService referenzGruppeDTOService;

        public StrassenabschnittGISDTOService(ITransactionScopeProvider transactionScopeProvider, IDataTransferObjectServiceMappingEngine dataTransferObjectServiceMappingEngine, 
            IReferenzGruppeDTOService referenzGruppeDTOService)
            : base(transactionScopeProvider, dataTransferObjectServiceMappingEngine)
        {
            this.referenzGruppeDTOService = referenzGruppeDTOService;
        }

        protected override StrassenabschnittGISDTO CreateModel(StrassenabschnittGIS entity)
        {
            var DTO =  base.CreateModel(entity);
            DTO.ReferenzGruppeDTO = referenzGruppeDTOService.GetDTOByID(entity.ReferenzGruppe.Id);
            //there should be exactly one Inspektionsroute as you cant export a DTO without it belonging to a Inspektionsroute
            DTO.InspektionsRouteId = entity.InspektionsRtStrAbschnitte.Single().InspektionsRouteGIS.Id;
            DTO.ZustandsabschnittenId = entity.Zustandsabschnitten.Select(z => z.Id).ToList();
            return DTO;
        }
    }
}
