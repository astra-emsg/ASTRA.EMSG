using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Common.Master.ConfigurationHandling
{
    public interface IServerConfigurationProvider
    {
        string ConnectionString { get; }
        bool UsePrecompiledViews { get; }
        int SecurityCacheTimeout { get; }
        ApplicationEnvironment Environment { get; }
        string ClientFilesFolderPath { get; }
        string SldFilesFolderPath { get; }
        int MehrzeiligenEingabefelderMaxLengthInPreview { get; }
        string WMSUrl { get; }
        string WMSUrlBaseLayer { get; }

        string WMTSUrlBaseLayer { get; }
        string WMSAVUrl { get; }
        string SWMSAVUrl { get; }

        string WMTSUserName { get; }
        string WMTSPassword { get; }
        string WMSAVUserName { get; }
        string WMSAVPassword { get; }
        string SWMSAVUserName { get; }
        string SWMSAVPassword { get; }


        int ExportBackgroundMapScale { get; }
        long ExportBackgroundMapMaxPixelCount { get; }
        int ExportTileLimit { get; }
        int ExportThreads { get; }
        int ExportBackgroundMapScaleSteps { get; }
        int ExportBackgroundMapDPI { get; }
        double ExportBackgroundMapBuffer { get; }
        string LogFolderPath { get; }
        string WMSCacheFolderPath { get; }
        bool UseWMSCaching { get; }
        int InspektionsrouteStrassenabschnittLayerReport { get; }
        int ZustandabschnittLayerMassnahmeTypReport { get; }
        bool EnableMiniProfiler { get; }

        LayerCollection ExportLayer { get; }
        int BemerkungMaxDisplayLength { get; }
    }
}
