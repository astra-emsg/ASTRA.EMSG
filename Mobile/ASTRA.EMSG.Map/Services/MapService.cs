using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using ASTRA.EMSG.Common.DataTransferObjects.EventArgs;
using ASTRA.EMSG.Common.Utils;
using System.Runtime.Serialization.Json;
using System.IO;

namespace ASTRA.EMSG.Map.Services
{
    public interface IMapService
    {
        event EventHandler<SelectZustandsabschnittDataTransferEventArgs> ZustandsabschnittSelected;
        event EventHandler<CreateZustandsabschnittDataTransferEventArgs> ZustandsabschnittCreated;
        event EventHandler<EditZustandsabschnittDataTransferEventArgs> ZustandsabschnittChanged;
        event EventHandler<SelectStrassenabschnittDataTransferEventArgs> StrassenabschnittSelected;
        event EventHandler<DeleteZustandsabschnittDataTransferEventArgs> ZustandsabschnittDeleted;
        event EventHandler ZustandsabschnittCancelled;
        event EventHandler ActivateSelectZustandsabschnittTool;
        event EventHandler<ShowLegendEventArgs> ShowLegend;

        void OnZustandsabschnittSelected(SelectZustandsabschnittDataTransferEventArgs e);
        void OnZustandsabschnittCreated(CreateZustandsabschnittDataTransferEventArgs e);
        void OnZustandsabschnittChanged(EditZustandsabschnittDataTransferEventArgs e);
        void OnStrassenabschnittSelected(SelectStrassenabschnittDataTransferEventArgs e);
        void OnZustandsabschnittDeleted(DeleteZustandsabschnittDataTransferEventArgs e);
        void OnZustandsabschnittCancelled();
        void OnActivateSelectZustandsabschnittTool();
    }
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public class MapService : IMapService
    {
        public event EventHandler<SelectZustandsabschnittDataTransferEventArgs> ZustandsabschnittSelected;
        public event EventHandler<CreateZustandsabschnittDataTransferEventArgs> ZustandsabschnittCreated;
        public event EventHandler<EditZustandsabschnittDataTransferEventArgs> ZustandsabschnittChanged;
        public event EventHandler<SelectStrassenabschnittDataTransferEventArgs> StrassenabschnittSelected;
        public event EventHandler<DeleteZustandsabschnittDataTransferEventArgs> ZustandsabschnittDeleted;
        public event EventHandler ZustandsabschnittCancelled;
        public event EventHandler ActivateSelectZustandsabschnittTool;
        public event EventHandler<ShowLegendEventArgs> ShowLegend;

        private readonly IFormService formservice;
        public MapService(IFormService formservice)
        {
            this.formservice = formservice;
        }

        public void CallZustandsabschnittSelected(string guid)
        {
            SelectZustandsabschnittDataTransferEventArgs args = new SelectZustandsabschnittDataTransferEventArgs();
            args.Id = Guid.Parse(guid);
            this.OnZustandsabschnittSelected(args);
        }
        public void OnZustandsabschnittSelected(SelectZustandsabschnittDataTransferEventArgs e)
        {
            var handler = ZustandsabschnittSelected;
            if (handler != null) handler(this, e);
        }

        public void CallZustandsabschnittCreated(string strassenabschnittId)
        {
            CreateZustandsabschnittDataTransferEventArgs e = new CreateZustandsabschnittDataTransferEventArgs(Guid.NewGuid(), Guid.Parse(strassenabschnittId));
            this.OnZustandsabschnittCreated(e);
        }
        public void OnZustandsabschnittCreated(CreateZustandsabschnittDataTransferEventArgs e)
        {
            var handler = ZustandsabschnittCreated;
            if (handler != null) handler(this, e);
        }

        public void CallZustandsabschnittChanged(string geojson, decimal length)
        {
            EditZustandsabschnittDataTransferEventArgs e = new EditZustandsabschnittDataTransferEventArgs();
           
            e.Length = length == 0 ? (decimal?)null : length;
            e.GeoJson = geojson;
            this.OnZustandsabschnittChanged(e);
        }
        public void OnZustandsabschnittChanged(EditZustandsabschnittDataTransferEventArgs e)
        {
            var handler = ZustandsabschnittChanged;
            if (handler != null) handler(this, e);
        }

        public void CallStrassenabschnittSelected(string guid)
        {
            SelectStrassenabschnittDataTransferEventArgs e = new SelectStrassenabschnittDataTransferEventArgs(Guid.Parse(guid));
            this.OnStrassenabschnittSelected(e);
        }

       

        public bool HasFormChanges()
        {
            return this.formservice.HasFormChanges();
        }

        //Not used and if thrown will cause notImplementedException in FormViewModel
        public void OnStrassenabschnittSelected(SelectStrassenabschnittDataTransferEventArgs e)
        {
            var handler = StrassenabschnittSelected;
            if (handler != null) handler(this, e);
        }

        public void CallZustandsabschnittDeleted(string guid)
        {
            DeleteZustandsabschnittDataTransferEventArgs e = new DeleteZustandsabschnittDataTransferEventArgs(Guid.Parse(guid));
            this.OnZustandsabschnittDeleted(e);
        }

        

        //Not used and if thrown will cause notImplementedException in FormViewModel
        public void OnZustandsabschnittDeleted(DeleteZustandsabschnittDataTransferEventArgs e)
        {
            var handler = ZustandsabschnittDeleted;
            if (handler != null) handler(this, e);
        }
        
        //JS could call these Methods directly since it contains no complex types, but added Method to preserve current naming convention
        public void CallZustandsabschnittCancelled()
        {
            this.OnZustandsabschnittCancelled();
        }
        public void OnZustandsabschnittCancelled()
        {
            var handler = ZustandsabschnittCancelled;
            if (handler != null) handler(this, new EventArgs());
        }

        
        public void CallActivateSelectZustandsabschnittTool()
        {
            this.OnActivateSelectZustandsabschnittTool();
        }
        public void OnActivateSelectZustandsabschnittTool()
        {
            var handler = ActivateSelectZustandsabschnittTool;
            if (handler != null) handler(this, new EventArgs());
        }
        public string GenereateGuid()
        {
            return Guid.NewGuid().ToString();
        }
        public void CallShowLegend(string layername)
        {            
            var handler = ShowLegend;
            if (handler != null) handler(this, new ShowLegendEventArgs(layername));
        }
    }
}
