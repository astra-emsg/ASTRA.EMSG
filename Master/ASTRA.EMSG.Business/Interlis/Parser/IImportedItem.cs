using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTRA.EMSG.Business.Interlis.Parser
{
    public interface IImportedItem
    {
        Guid Id { get; }
        int Operation { get; }
    }
}
