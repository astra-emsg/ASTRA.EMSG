using System;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Master;

namespace ASTRA.EMSG.Business.Entities.Common
{
    public interface IEntity : IIdHolder, IAuditableEntity
    {

    }


    public interface IAuditableEntity
    {
        DateTime? CreatedAt { get; set; }

        string CreatedBy { get; set; }

        DateTime? UpdatedAt { get; set; }

        string UpdatedBy { get; set; }
    }
}