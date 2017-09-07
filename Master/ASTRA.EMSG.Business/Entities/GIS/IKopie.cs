using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTRA.EMSG.Business.Entities.GIS
{
    public interface IKopie
    {
        Guid Id { get; set; }
        int Operation { get; set; }
        int ImpNr { get; set; }
    }
}
