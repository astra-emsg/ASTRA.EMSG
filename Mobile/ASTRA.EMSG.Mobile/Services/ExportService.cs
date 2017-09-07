using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using ASTRA.EMSG.Map.Services;
using ASTRA.EMSG.Common.Mobile;

using ASTRA.EMSG.Common;
using System.IO;

using ASTRA.EMSG.Common.DataTransferObjects;

using ASTRA.EMSG.Common.Utils;
using ASTRA.EMSG.Common.Mobile.Utils;


namespace ASTRA.EMSG.Mobile.Services
{
    public interface IExportService
    {
        DTOContainer Export(string tempExportDirectory, bool exportAll);
    }
    public class ExportService : IExportService
    {
       
      
        private readonly IClientConfigurationProvider clientConfigurationProvider;
        private readonly IDTOService dtoService;
        private readonly ILoadService loadService;
        private readonly IFormService formService;
        
        public ExportService(
            IClientConfigurationProvider clientConfigurationProvider,
            IDTOService dtoService,
            ILoadService loadService,
            IFormService formService)
        {
            this.formService = formService;
            this.clientConfigurationProvider = clientConfigurationProvider;
            this.dtoService = dtoService;
            this.loadService = loadService;
        }
        public DTOContainer Export(string tempExportDirectory, bool exportAll)
        {
            IEnumerable<Guid> zustandsAbschnittIDs = null;
            IEnumerable<ZustandsabschnittGISDTO> zustandsAbschnitte = null;

            if (exportAll)
            {
                zustandsAbschnitte = dtoService.GetAll<ZustandsabschnittGISDTO>();
                zustandsAbschnittIDs = zustandsAbschnitte.Select(z => z.Id);
            }
            else
            {
                var strassenabschnitte = dtoService.GetAll<StrassenabschnittGISDTO>().Where(s => s.InspektionsRouteId == formService.GetActiveInspektionsRoute()).Select(s => s.Id);

                zustandsAbschnitte = dtoService.GetAll<ZustandsabschnittGISDTO>().Where(z => strassenabschnitte.Contains(z.StrassenabschnittGIS));
                zustandsAbschnittIDs = zustandsAbschnitte.Select(z => z.Id);
            }


            DTOContainer dtocontainer = new DTOContainer();
            dtocontainer.DataTransferObjects = dtocontainer.DataTransferObjects.Concat(zustandsAbschnitte).ToList();
            
            foreach (var schadendetail in dtoService.GetAll<SchadendetailDTO>().Where(sd => zustandsAbschnittIDs.Contains(sd.ZustandsabschnittId)))
            {
                dtocontainer.DataTransferObjects.Add(schadendetail);
            }
            foreach (var schadengrouppe in dtoService.GetAll<SchadengruppeDTO>().Where(sg => zustandsAbschnittIDs.Contains(sg.ZustandsabschnittId)))
            {
                dtocontainer.DataTransferObjects.Add(schadengrouppe);
            }
            dtoService.saveFile(dtocontainer, Path.Combine(tempExportDirectory, FileNameConstants.DTOContainerFileName));

            return dtocontainer;
        }
    }
}
