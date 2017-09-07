using System;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Common.Services.SchadenMetadaten;

namespace ASTRA.EMSG.Business.Services.EntityServices.GIS
{
    public interface IFahrbahnZustandGISService : IFahrbahnZustandServiceBase, IService
    {
    }

    public class FahrbahnZustandGISService : FahrbahnZustandServiceBase, IFahrbahnZustandGISService
    {
        public FahrbahnZustandGISService(ITransactionScopeProvider transactionScopeProvider, ISchadenMetadatenService schadenMetadatenService)
            : base(transactionScopeProvider, schadenMetadatenService)
        {
        }

        protected override StrassenabschnittBase GetStrassenabschnittBase(Guid id)
        {
            return transactionScopeProvider.GetById<StrassenabschnittGIS>(id);
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

        protected override bool IsLocked(StrassenabschnittBase strassenabschnittBase)
        {
            return ((StrassenabschnittGIS) strassenabschnittBase).IsLocked;
        }
    }
}