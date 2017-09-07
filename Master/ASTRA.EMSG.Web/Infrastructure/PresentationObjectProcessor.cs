using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Reporting;
using Kendo.Mvc.UI;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public class PresentationObjectProcessor<TPresentationObject> : IPresentationObjectProcessor<TPresentationObject>
    {
        private readonly ControllerBase controllerBase;

        public PresentationObjectProcessor(ControllerBase controllerBase)
        {
            this.controllerBase = controllerBase;
        }

        public List<TPresentationObject> ProcessPresentationObjects(List<TPresentationObject> presentationObjects)
        {
            //var gridDataProcessor = new GridDataProcessor(new GridActionBindingContext(false, controllerBase, presentationObjects, presentationObjects.Count()));
            //return gridDataProcessor.ProcessedDataSource.Cast<TPresentationObject>().ToList();
            return presentationObjects;
        }
    }
}