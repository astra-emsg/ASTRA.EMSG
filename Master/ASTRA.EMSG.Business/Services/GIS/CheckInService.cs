using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.PackageService;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Common.DataTransferObjects;
using ASTRA.EMSG.Common.Master.Logging;

using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.DTOServices;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Common.Utils;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Services.GIS
{
    public interface ICheckInService : IService
    {
        ImportResult CheckInData(ImportResult importResult);
    }
    public class CheckInService : ICheckInService
    {
        
        private readonly IInspektionsRouteGISService inspektionsRouteGISService;
        private readonly IInspektionsRouteStatusverlaufService inspektionsRouteStatusverlaufService;
        
        private readonly IZustandsabschnittGISDTOService zustandsabschnittGISDTOService;
        private readonly IFahrbahnZustandDTOService fahrbahnZustandDTOService;
        private readonly ICheckOutsGISService checkOutsGISService;
        private readonly IEreignisLogService ereignisLogService;
        private readonly ITimeService timeService;


        public CheckInService(
            IInspektionsRouteGISService inspektionsRouteGISService,
            IInspektionsRouteStatusverlaufService inspektionsRouteStatusverlaufService,
            
            IZustandsabschnittGISDTOService zustandsabschnittGISDTOService,
            IFahrbahnZustandDTOService fahrbahnZustandDTOService,
            ICheckOutsGISService checkOutsGISService,
             IEreignisLogService ereignisLogService,
            ITimeService timeService
            )
        {
            this.inspektionsRouteGISService = inspektionsRouteGISService;
            this.inspektionsRouteStatusverlaufService = inspektionsRouteStatusverlaufService;
           
            this.zustandsabschnittGISDTOService = zustandsabschnittGISDTOService;
            this.fahrbahnZustandDTOService = fahrbahnZustandDTOService;
            this.checkOutsGISService = checkOutsGISService;
            this.ereignisLogService = ereignisLogService;
            this.timeService = timeService;
        }
        public ImportResult CheckInData(ImportResult importResult)
        {
            
            try
            {
                DTOContainer dtoContainer = importResult.dtocontainer;
                var zustandsAbschnitte = dtoContainer.DataTransferObjects.Where(dto => dto.GetType() == typeof(ZustandsabschnittGISDTO)).Select(sd => sd as ZustandsabschnittGISDTO);
                var schadendetailList = dtoContainer.DataTransferObjects.Where(dto => dto.GetType() == typeof(SchadendetailDTO)).Select(sd => sd as SchadendetailDTO);
                var schadengruppeList = dtoContainer.DataTransferObjects.Where(dto => dto.GetType() == typeof(SchadengruppeDTO)).Select(sd => sd as SchadengruppeDTO);
                var deletedZustandsabschnittList = zustandsAbschnitte.Where(z => z.IsDeleted && (!z.IsAdded)).Select(z => z.Id);                
                
               
                IList<ZustandsabschnittGISDTO> zustandsabschnitteToValidate = new List<ZustandsabschnittGISDTO>();
                zustandsabschnitteToValidate = zustandsabschnitteToValidate.Concat(zustandsAbschnitte.Where(z => !z.IsDeleted && (z.IsEdited||z.IsAdded))).ToList();
                
                foreach (var za in zustandsabschnitteToValidate)
                {
                    var result = zustandsabschnittGISDTOService.Validate(za, deletedZustandsabschnittList.ToList(), zustandsabschnitteToValidate);
                    foreach (string error in result)
                    {
                        importResult.Errors.Add(error);
                    }
                }

                if (importResult.Errors.Count == 0)
                {
                    foreach (Guid id in deletedZustandsabschnittList)
                    {
                        try
                        {
                            zustandsabschnittGISDTOService.DeleteEntity(id);
                        }
                        catch { }
                    }

                    foreach (var za in zustandsabschnitteToValidate)
                    {
                        if (za.Shape.SRID == 0)
                        {
                            za.Shape.SRID = GisConstants.SRID;
                        }
                        zustandsabschnittGISDTOService.CreateOrUpdateEntityFromDTO(za);
                        FahrBahnZustandDTO zustand = new FahrBahnZustandDTO()
                        {
                            Erfassungsmodus = za.Erfassungsmodus,
                            Schadendetails = za.Erfassungsmodus == ZustandsErfassungsmodus.Detail ? schadendetailList.Where(sd => sd.ZustandsabschnittId == za.Id && !sd.IsDeleted).ToList() : new List<SchadendetailDTO>(),
                            Schadengruppen = za.Erfassungsmodus == ZustandsErfassungsmodus.Grob ? schadengruppeList.Where(sg => sg.ZustandsabschnittId == za.Id && !sg.IsDeleted).ToList() : new List<SchadengruppeDTO>(),
                            ZustandsAbschnitt = za.Id,
                            Zustandsindex = za.Zustandsindex
                        };
                        fahrbahnZustandDTOService.UpdateFahrbahnZustand(zustand);
                    }
                }               
                if (importResult.Errors.Count == 0)
                {
                    foreach (var kvp in importResult.descriptor.CheckOutsGISInspektionsroutenList)
                    {
                        var cogModel = checkOutsGISService.GetById(kvp.Key);
                        cogModel.CheckInDatum = timeService.Now;
                        checkOutsGISService.UpdateEntity(cogModel);
                        inspektionsRouteGISService.UnLockInspektionsRouten(importResult.InspektionsRouten);
                        foreach (var ir in importResult.InspektionsRouten)
                        {
                            inspektionsRouteStatusverlaufService.HistorizeRouteImportiert(inspektionsRouteGISService.GetInspektionsRouteById(ir));
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Loggers.ApplicationLogger.Error(e.Message, e);
                importResult.Errors.Add("Error When Importing");
            }
            var inspektionsrouten = inspektionsRouteGISService.GetCurrentEntities().Where(ir => importResult.InspektionsRouten.Contains(ir.Id));
            ereignisLogService.LogEreignis(EreignisTyp.InspektionsRoutenImport, new Dictionary<string, object>() { { "Inspektionsrouten", string.Join(", ", inspektionsrouten.Select(ir => ir.Bezeichnung)) }, { "Fehleranzahl", importResult.Errors.Count } });
            return importResult;
        }

    }
}
