using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;

namespace ASTRA.EMSG.Business.Services.Administration
{
    public interface IArbeitsmodusService : IService
    {
        ArbeitsmodusModel GetArbeitsmodusModel();
        void SaveArbeitsmodusModel(ArbeitsmodusModel arbeitsmodusModel);
    }

    public class ArbeitsmodusService : IArbeitsmodusService
    {
        private readonly ITransactionScopeProvider transactionScopeProvider;
        private readonly IErfassungsPeriodService erfassungsPeriodService;
        private readonly IEreignisLogService ereignisLogService;

        public ArbeitsmodusService(
            ITransactionScopeProvider transactionScopeProvider, 
            IErfassungsPeriodService erfassungsPeriodService,
            IEreignisLogService ereignisLogService
            )
        {
            this.transactionScopeProvider = transactionScopeProvider;
            this.erfassungsPeriodService = erfassungsPeriodService;
            this.ereignisLogService = ereignisLogService;
        }

        public ArbeitsmodusModel GetArbeitsmodusModel()
        {
            return new ArbeitsmodusModel { NetzErfassungsmodus = erfassungsPeriodService.GetCurrentErfassungsPeriod().NetzErfassungsmodus };
        }

        public void SaveArbeitsmodusModel(ArbeitsmodusModel arbeitsmodusModel)
        {
            ErfassungsPeriod currentErfassungsPeriod = erfassungsPeriodService.GetCurrentErfassungsPeriod();

            var previousNetzErfassungsmodus = currentErfassungsPeriod.NetzErfassungsmodus;

            currentErfassungsPeriod.NetzErfassungsmodus = arbeitsmodusModel.NetzErfassungsmodus;

            transactionScopeProvider.Update(currentErfassungsPeriod);

            ereignisLogService.LogEreignis(EreignisTyp.Arbeitsmoduswechsel, new Dictionary<string, object> { { "alt", previousNetzErfassungsmodus }, { "neu", arbeitsmodusModel.NetzErfassungsmodus } });
        }
    }
}
