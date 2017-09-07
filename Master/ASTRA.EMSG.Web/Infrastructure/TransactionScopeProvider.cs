using System;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Services.Common;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public class TransactionScopeProvider : ITransactionScopeProvider
    {
        private readonly IHttpRequestService httpRequestService;
        private readonly ITransactionScopeFactory transactionScopeFactory;

        private const string CurrentTransactionScopeKey = "__CurrentTransactionScopeKey__";

        public TransactionScopeProvider(IHttpRequestService httpRequestService, ITransactionScopeFactory transactionScopeFactory)
        {
            this.httpRequestService = httpRequestService;
            this.transactionScopeFactory = transactionScopeFactory;
        }

        public ITransactionScope CurrentTransactionScope
        {
            get
            {
                var transactionScope = (ITransactionScope) httpRequestService[CurrentTransactionScopeKey];
                if (transactionScope == null)
                {
                    transactionScope = transactionScopeFactory.CreateReadWrite();
                    SetCurrentTransactionScope(transactionScope);
                }

                return transactionScope;
            }
        }

        public void SetCurrentTransactionScope(ITransactionScope transactionScope)
        {
            httpRequestService[CurrentTransactionScopeKey] = transactionScope;
        }

        public void ResetCurrentTransactionScope()
        {
            var transactionScope = (ITransactionScope)httpRequestService[CurrentTransactionScopeKey];
            if (transactionScope == null)
                throw new InvalidOperationException("There is no Current TransactionScope to reset");

            transactionScope.Dispose();
            httpRequestService[CurrentTransactionScopeKey] = null;
        }

        public bool HasRequestTransaction
        {
            get { return httpRequestService[CurrentTransactionScopeKey] != null; }
        }
    }
}