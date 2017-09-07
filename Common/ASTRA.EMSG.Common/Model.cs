using System;

namespace ASTRA.EMSG.Common
{
    public interface IModel
    {
    }

    [Serializable]
    public abstract class Model : IModel, IIdHolder
    {
        public Guid Id { get; set; }
    }
}