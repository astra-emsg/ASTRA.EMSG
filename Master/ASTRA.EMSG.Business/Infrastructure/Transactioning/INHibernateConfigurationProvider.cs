using NHibernate.Cfg;

namespace ASTRA.EMSG.Business.Infrastructure.Transactioning
{
    public interface INHibernateConfigurationProvider
    {
        Configuration Configuration { get; }
    }
}