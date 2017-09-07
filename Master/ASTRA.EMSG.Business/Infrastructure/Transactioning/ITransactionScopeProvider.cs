namespace ASTRA.EMSG.Business.Infrastructure.Transactioning
{
    public interface ITransactionScopeProvider
    {
        ITransactionScope CurrentTransactionScope { get; }
        void ResetCurrentTransactionScope();
        bool HasRequestTransaction { get; }
    }
}
