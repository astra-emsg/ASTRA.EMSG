using System;
using ASTRA.EMSG.Common.DataTransferObjects.EventArgs;
using System.Runtime.InteropServices;

namespace ASTRA.EMSG.Map.Services
{
    [ComVisible(false)]
    public interface IFormService
    {
        event EventHandler<SaveZustandsabschnittDataTransferEventArgs> ZustandsabschnittSaved;
        event EventHandler<SaveZustandsabschnittDataTransferEventArgs> ZustandsabschnittApplySave;
        event EventHandler<DeleteZustandsabschnittDataTransferEventArgs> ZustandsabschnittDeleted;
        event EventHandler ZustandsabschnittCancelled;
        event EventHandler<FormChangesEventArgs> GettingFormHasChanges;
        event EventHandler<DataChangedEventArgs> DataChanged;
        event EventHandler<InspektionsRouteChangedEventArgs> InspektionsRouteChanged;

        void OnZustandsabschnittSaved(SaveZustandsabschnittDataTransferEventArgs e);
        void OnZustandsabschnittApplySave(SaveZustandsabschnittDataTransferEventArgs e);
        void OnZustandsabschnittDeleted(DeleteZustandsabschnittDataTransferEventArgs e);        
        void OnZustandsabschnittCancelled();
        void OnDataChanged(DataChangedEventArgs e);
        void OnInspektionsRouteChanged(InspektionsRouteChangedEventArgs e);
        bool HasFormChanges();

        Guid GetActiveInspektionsRoute();
    }
    
    public class FormService : IFormService
    {
        private Guid activeInspektionsRouteId;
        [JSEventHandler("ZustandsabschnittSaved")]
        public event EventHandler<SaveZustandsabschnittDataTransferEventArgs> ZustandsabschnittSaved;
        [JSEventHandler("ZustandsabschnittApplySave")]
        public event EventHandler<SaveZustandsabschnittDataTransferEventArgs> ZustandsabschnittApplySave;
        [JSEventHandler("ZustandsabschnittDeleted")]
        public event EventHandler<DeleteZustandsabschnittDataTransferEventArgs> ZustandsabschnittDeleted;
        [JSEventHandler("ZustandsabschnittCancelled")]
        public event EventHandler ZustandsabschnittCancelled;
        [JSEventHandler("GettingFormHasChanges")]
        public event EventHandler<FormChangesEventArgs> GettingFormHasChanges;
        [JSEventHandler("DataChanged")]
        public event EventHandler<DataChangedEventArgs> DataChanged;
        [JSEventHandler("InspektionsRouteChanged")]
        public event EventHandler<InspektionsRouteChangedEventArgs> InspektionsRouteChanged;

       
        public void OnZustandsabschnittSaved(SaveZustandsabschnittDataTransferEventArgs e)
        {
            var handler = ZustandsabschnittSaved;
            if (handler != null) handler(this, e);
        }

        public void OnZustandsabschnittApplySave(SaveZustandsabschnittDataTransferEventArgs e)
        {
            var handler = ZustandsabschnittApplySave;
            if (handler != null) handler(this, e);
        }

        public void OnZustandsabschnittDeleted(DeleteZustandsabschnittDataTransferEventArgs e)
        {
            var handler = ZustandsabschnittDeleted;
            if (handler != null) handler(this, e);
        }

        public void OnZustandsabschnittCancelled()
        {
            var handler = ZustandsabschnittCancelled;
            if (handler != null) handler(this, new EventArgs());
        }

        public void OnGettingFormHasChanges(FormChangesEventArgs e)
        {
            EventHandler<FormChangesEventArgs> handler = GettingFormHasChanges;
            if (handler != null) handler(this, e);
        }

        public bool HasFormChanges()
        {
            var formChangesEventArgs = new FormChangesEventArgs();
            OnGettingFormHasChanges(formChangesEventArgs);

            return formChangesEventArgs.HasFormChanges;
        }
        public void OnDataChanged(DataChangedEventArgs e)
        {
            var handler = DataChanged;
            if (handler != null) handler(this, e);
        }
        public void OnInspektionsRouteChanged(InspektionsRouteChangedEventArgs e)
        {
            var handler = InspektionsRouteChanged;
            if (handler != null) handler(this, e);
            this.activeInspektionsRouteId = e.inspektionsRouteId;
        }
        public Guid GetActiveInspektionsRoute()
        {
            return this.activeInspektionsRouteId;
        }
    }
}
