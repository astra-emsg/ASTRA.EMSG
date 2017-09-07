using System;
using System.Security.Principal;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using Environment = NHibernate.Cfg.Environment;

namespace ASTRA.EMSG.Business.Infrastructure.Transactioning
{
    public class MsSQLNHibernateConfigurationProvider : INHibernateConfigurationProvider
    {
        private readonly IServerConfigurationProvider serverConfigurationProvider;

        public MsSQLNHibernateConfigurationProvider(IServerConfigurationProvider serverConfigurationProvider)
        {
            this.serverConfigurationProvider = serverConfigurationProvider;
        }

        public Configuration Configuration
        {
            get
            {
                string driver = "NHibernate.Driver.Sql2008ClientDriver";
                if (serverConfigurationProvider.EnableMiniProfiler)
                    driver = "ASTRA.EMSG.Web.Infrastructure.MiniProfiler.ProfilingOracleDataClientDriver, ASTRA.EMSG.Web";
                
                var fluentConfiguration = Fluently.Configure()
                    .Database(FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2012.ConnectionString(serverConfigurationProvider.ConnectionString).ShowSql()
                        .Dialect("ASTRA.EMSG.Business.Infrastructure.Transactioning.MsSql2012GeometryDialect, ASTRA.EMSG.Business"))
                        .ExposeConfiguration(c => c.SetProperty(NHibernate.Cfg.Environment.CommandTimeout, TimeSpan.FromMinutes(10).TotalSeconds.ToString()))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Belastungskategorie>()
                        .Conventions.Add(new EnumConvention(), new MsSqlGeometryConvention()));

                Configuration configuration = fluentConfiguration.BuildConfiguration();
                //configuration.CreateIndexesForForeignKeys();

                AuditEventListener.Register(configuration);
                return configuration
                    .SetProperty(Environment.QuerySubstitutions, "true 1, false 0, yes 'Y', no 'N'");
                    //Validate funktioniert mit Spatial nicht
                    //.SetProperty(Environment.Hbm2ddlAuto, "validate");
            }
        }
    }
}