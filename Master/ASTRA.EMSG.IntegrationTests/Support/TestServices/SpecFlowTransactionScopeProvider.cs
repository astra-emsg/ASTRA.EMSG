using ASTRA.EMSG.Business.Infrastructure.Transactioning;

namespace ASTRA.EMSG.IntegrationTests.Support.TestServices
{
    public class SpecFlowTransactionScopeProvider : ITransactionScopeProvider
    {
        private ITransactionScope currentTransactionScope;
        public ITransactionScope CurrentTransactionScope
        {
            get
            {
                if (currentTransactionScope != null)
                    return currentTransactionScope;

                return ScenarioContextWrapper.CurrentScope;
            }
        }

        public void SetCurrentTransactionScope(ITransactionScope transactionScope)
        {
            currentTransactionScope = transactionScope;
        }

        public void ResetCurrentTransactionScope()
        {
            currentTransactionScope = null;
        }

        public bool HasRequestTransaction
        {
            get { return currentTransactionScope != null; }
        }
    }
}