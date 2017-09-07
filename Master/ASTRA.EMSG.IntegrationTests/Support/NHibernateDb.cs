using System;
using System.Data;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using ASTRA.EMSG.IntegrationTests.Support.MvcTesting;
using NLog.Internal;

namespace ASTRA.EMSG.IntegrationTests.Support
{
    public static class NHibernateDb
    {
        public static readonly Lazy<MsSQLNHibernateConfigurationProvider> ConfigurationProvider
            = new Lazy<MsSQLNHibernateConfigurationProvider>(() => new MsSQLNHibernateConfigurationProvider(new TestConfigProvider()));

        public static NHibernateReadWriteTransactionScope GetNewScope()
        {
            return new NHibernateReadWriteTransactionScope(IsolationLevel.ReadCommitted, ConfigurationProvider.Value);
        }
    }

    public class TestConfigProvider : IServerConfigurationProvider
    {
        public string ConnectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["TestDb"].ConnectionString;
            }
        }

        public bool UsePrecompiledViews { get; set; }
        public int SecurityCacheTimeout { get; set; }
        public ApplicationEnvironment Environment { get; set; }
        public string ClientFilesFolderPath { get; set; }
        public int MehrzeiligenEingabefelderMaxLengthInPreview { get; set; }
        public string WMSUrl { get; set; }
        public string WMSUrlBaseLayer { get; set; }
        public string WMTSUrlBaseLayer { get; set; }
        public string WMSAVUrl { get; set; }
        public string SWMSAVUrl { get; set; }
        public string WMTSUserName { get; set; }
        public string WMTSPassword { get; set; }
        public string WMSAVUserName { get; set; }
        public string WMSAVPassword { get; set; }
        public string SWMSAVUserName { get; set; }
        public string SWMSAVPassword { get; set; }
        public int ExportBackgroundMapScale { get; set; }
        public long ExportBackgroundMapMaxPixelCount { get; set; }
        public int ExportTileLimit { get; set; }
        public int ExportThreads { get; set; }
        public int ExportBackgroundMapScaleSteps { get; set; }
        public int ExportBackgroundMapDPI { get; set; }
        public double ExportBackgroundMapBuffer { get; set; }
        public string LogFolderPath { get; set; }
        public string WMSCacheFolderPath { get; set; }
        public bool UseWMSCaching { get; set; }
        public int InspektionsrouteStrassenabschnittLayerReport { get; set; }
        public int ZustandabschnittLayerMassnahmeTypReport { get; set; }
        public bool EnableMiniProfiler { get; set; }
        public LayerCollection ExportLayer { get; set; }
        public int BemerkungMaxDisplayLength { get; set; }

        public string SldFilesFolderPath { get; set; }
    }
}