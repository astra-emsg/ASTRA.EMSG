using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTRA.EMSG.Common.DataTransferObjects
{
    public interface IDataTransferObject : IModel, IIdHolder
    {
        bool IsAdded { get; set; }
        bool IsEdited { get; set; }
        bool IsDeleted { get; set; }
    }
    [Serializable]
    public class DataTransferObject : IDataTransferObject
    {
        public DataTransferObject()
        {
            this.IsAdded = false;
            this.IsEdited = false;
            this.IsDeleted = false;
        }
        public Guid Id { get; set; }
        public bool IsAdded { get; set; }
        public bool IsEdited { get; set; }
        public bool IsDeleted { get; set; }
    }
}
