using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;

namespace ASTRA.EMSG.Business.Services.EntityServices.GIS
{
    public interface ITrottoirZustandGISService : ITrottoirZustandServiceBase, IService
    {
    }

    public class TrottoirZustandGISService : TrottoirZustandServiceBase, ITrottoirZustandGISService
    {
        public TrottoirZustandGISService(ITransactionScopeProvider transactionScopeProvider)
            : base(transactionScopeProvider)
        {
        }

        protected override ZustandsabschnittBase GetZustandsabschnittBase(Guid id)
        {
            return transactionScopeProvider.GetById<ZustandsabschnittGIS>(id);
        }

        protected override void UpdateZustandsabschnittBase(ZustandsabschnittBase zustandsabschnittBase)
        {
            if (((ZustandsabschnittGIS)zustandsabschnittBase).IsLocked)
                throw new InvalidOperationException("ZustandsabschnittGIS is locked.");
            transactionScopeProvider.Update((ZustandsabschnittGIS)zustandsabschnittBase);
        }

        protected override IEnumerable<ZustandsabschnittBase> GetAllByErfassungsPeriod(ErfassungsPeriod erfassungsPeriod)
        {
            return transactionScopeProvider.Queryable<ZustandsabschnittGIS>().Where(z => z.StrassenabschnittGIS.ErfassungsPeriod == erfassungsPeriod).ToArray();
        }
    }
}