using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;

namespace ASTRA.EMSG.Business.Services.EntityServices.Strassennamen
{
    public interface ITrottoirZustandService : ITrottoirZustandServiceBase, IService
    {
    }

    public class TrottoirZustandService : TrottoirZustandServiceBase, ITrottoirZustandService
    {
        public TrottoirZustandService(ITransactionScopeProvider transactionScopeProvider)
            : base(transactionScopeProvider)
        {
        }

        protected override ZustandsabschnittBase GetZustandsabschnittBase(Guid id)
        {
            return transactionScopeProvider.GetById<Zustandsabschnitt>(id);
        }

        protected override void UpdateZustandsabschnittBase(ZustandsabschnittBase zustandsabschnittBase)
        {
            transactionScopeProvider.Update((Zustandsabschnitt)zustandsabschnittBase);
        }

        protected override IEnumerable<ZustandsabschnittBase> GetAllByErfassungsPeriod(ErfassungsPeriod erfassungsPeriod)
        {
            return transactionScopeProvider.Queryable<Zustandsabschnitt>().Where(z => z.Strassenabschnitt.ErfassungsPeriod == erfassungsPeriod).ToArray();
        }
    }
}