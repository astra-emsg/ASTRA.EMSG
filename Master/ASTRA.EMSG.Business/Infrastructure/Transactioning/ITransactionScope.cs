using System;
using NHibernate;

namespace ASTRA.EMSG.Business.Infrastructure.Transactioning
{
    public interface ITransactionScope : IDisposable
    {
        void Commit();
        void Flush();
        ISession Session { get; }
        bool IsActive { get; }
        void Rollback();
    }
}