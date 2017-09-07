using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Caching;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;

namespace ASTRA.EMSG.Business.Services.Historization
{
    public interface IKatalogCopyService : IService
    {
        void CopyKatalogData(ErfassungsPeriod closedPeriod);
        T GetKatalogCopy<T>(Guid oldId);
    }

    public class KatalogCopyService : IKatalogCopyService
    {
        private readonly IHttpRequestCacheService httpRequestCacheService;
        private readonly IWiederbeschaffungswertKatalogEditService wiederbeschaffungswertKatalogEditService;
        private readonly IMassnahmenvorschlagKatalogEditService massnahmenvorschlagKatalogEditService;
        private const string KatalogCopyLookupKey = "KatalogCopyLookup";


        public KatalogCopyService(
            IHttpRequestCacheService httpRequestCacheService,
            IWiederbeschaffungswertKatalogEditService wiederbeschaffungswertKatalogEditService,
            IMassnahmenvorschlagKatalogEditService massnahmenvorschlagKatalogEditService
            )
        {
            this.httpRequestCacheService = httpRequestCacheService;
            this.wiederbeschaffungswertKatalogEditService = wiederbeschaffungswertKatalogEditService;
            this.massnahmenvorschlagKatalogEditService = massnahmenvorschlagKatalogEditService;
        }

        public void CopyKatalogData(ErfassungsPeriod closedPeriod)
        {
            var katalogCopyLookup = new Dictionary<Guid, object>();
            foreach (var wbk in wiederbeschaffungswertKatalogEditService.GetEntitiesBy(closedPeriod))
                katalogCopyLookup.Add(wbk.Id, wiederbeschaffungswertKatalogEditService.CreateCopy(wbk));
            
            foreach (var rmk in massnahmenvorschlagKatalogEditService.GetEntitiesBy(closedPeriod))
                katalogCopyLookup.Add(rmk.Id, massnahmenvorschlagKatalogEditService.CreateCopy(rmk));

            httpRequestCacheService[KatalogCopyLookupKey] = katalogCopyLookup;
        }

        public T GetKatalogCopy<T>(Guid oldId)
        {
            if (httpRequestCacheService[KatalogCopyLookupKey] == null)
                throw new InvalidOperationException("KatalogCopyLookup is empty");
            var katalogCopyLookup = (Dictionary<Guid, object>)httpRequestCacheService[KatalogCopyLookupKey];
            return (T)katalogCopyLookup[oldId];
        }
    }
}