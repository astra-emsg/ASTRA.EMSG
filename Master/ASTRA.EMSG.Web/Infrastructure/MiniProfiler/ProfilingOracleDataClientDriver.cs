using System.Data;
using System.Data.Common;
using NHibernate.Driver;
using StackExchange.Profiling.Data;

namespace ASTRA.EMSG.Web.Infrastructure.MiniProfiler
{
    public class ProfilingOracleDataClientDriver : OracleDataClientDriver
    {
        protected override void OnBeforePrepare(IDbCommand command)
        {
            var profiledCommand = command as ProfiledDbCommand;
            if (profiledCommand == null)
                return;

            profiledCommand.BindByName = true;
        }


        public override IDbCommand CreateCommand()
        {
            return new ProfiledDbCommand(
                base.CreateCommand() as DbCommand,
                null,
                StackExchange.Profiling.MiniProfiler.Current);
        }

        public override IDbConnection CreateConnection()
        {

            return new ProfiledDbConnection(
                base.CreateConnection() as DbConnection,
                StackExchange.Profiling.MiniProfiler.Current);
        }
    }
}