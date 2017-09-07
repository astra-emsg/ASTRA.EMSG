using System.Collections.Generic;

namespace ASTRA.EMSG.Business.Reporting
{
    public interface IPresentationObjectProcessor<TPresentationObject>
    {
        List<TPresentationObject> ProcessPresentationObjects(List<TPresentationObject> presentationObjects);
    }
}