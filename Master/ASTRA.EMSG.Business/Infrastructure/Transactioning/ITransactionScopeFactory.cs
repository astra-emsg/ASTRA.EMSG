namespace ASTRA.EMSG.Business.Infrastructure.Transactioning
{
    public interface ITransactionScopeFactory
    {
        ITransactionScope CreateReadOnly();
        ITransactionScope CreateReadWrite();
        ITransactionScope CreateIsolatedReadWrite();
    }
}