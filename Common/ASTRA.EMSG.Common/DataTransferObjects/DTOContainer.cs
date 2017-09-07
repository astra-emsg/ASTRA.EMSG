using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTRA.EMSG.Common.DataTransferObjects
{
    public interface IDTOContainer 
    {
        IList<DataTransferObject> DataTransferObjects { get; set; }
    }
    [Serializable]
    public class DTOContainer:IDTOContainer
    {
        public IList<DataTransferObject> DataTransferObjects { get; set; }
        public DTOContainer()
        {   
            DataTransferObjects = new List<DataTransferObject>();
        }
    }
}
