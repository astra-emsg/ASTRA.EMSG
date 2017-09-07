using System;
using NHibernate.Dialect.Function;
using NHibernate.Type;
using NHibernate;

namespace ASTRA.EMSG.Business.Infrastructure.Transactioning
{
    class MsSql2012GeometryDialect : NHibernate.Spatial.Dialect.MsSql2012GeometryDialect
    {
        public MsSql2012GeometryDialect()
        {
            IType type = NHibernateUtil.GuessType(typeof(Guid));
            SQLFunctionTemplate template = new SQLFunctionTemplate(type, "NEWID()");
            RegisterFunction("guid", template);
        }
    }
}
