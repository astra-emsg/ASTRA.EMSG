using System;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Common.Services.SchadenMetadaten;

namespace ASTRA.EMSG.Business.Services.EntityServices.Strassennamen
{
    public interface IFahrbahnZustandService : IFahrbahnZustandServiceBase, IService
    {
    }

    public class FahrbahnZustandService : FahrbahnZustandServiceBase, IFahrbahnZustandService
    {
        public FahrbahnZustandService(ITransactionScopeProvider transactionScopeProvider, ISchadenMetadatenService schadenMetadatenService)
            : base(transactionScopeProvider, schadenMetadatenService)
        {
        }

        protected override ZustandsabschnittBase GetZustandsabschnittBase(Guid id)
        {
            return transactionScopeProvider.GetById<Zustandsabschnitt>(id);
        }

        protected override StrassenabschnittBase GetStrassenabschnittBase(Guid id)
        {
            return transactionScopeProvider.GetById<Strassenabschnitt>(id);
        }

        protected override void UpdateZustandsabschnittBase(ZustandsabschnittBase zustandsabschnittBase)
        {
            transactionScopeProvider.Update((Zustandsabschnitt)zustandsabschnittBase);
        }

        protected override bool IsLocked(StrassenabschnittBase strassenabschnittBase)
        {
            return false;
        }
    }
}