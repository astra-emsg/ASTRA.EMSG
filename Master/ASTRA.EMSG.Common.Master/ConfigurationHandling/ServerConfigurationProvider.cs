using System.Web;
using ASTRA.EMSG.Common.ConfigurationHandling;
using ASTRA.EMSG.Common.Enums;
using System.Linq;

namespace ASTRA.EMSG.Common.Master.ConfigurationHandling
{
    public class ServerConfigurationProvider : FileBasedConfigurationProvider, IServerConfigurationProvider
    {
        public virtual string ConnectionString
        {
            get
            {
                return ReadConnectionString(MachineEnvironment);
            }
        }

        public string MachineEnvironment
        {
            get
            {
                return ReadStringValue(System.Environment.MachineName, "Development");
            }
        }


        public bool EnableJsDebugMode { get { return ReadBoolValue("EnableJsDebugMode", false); } }

        public int MehrzeiligenEingabefelderMaxLengthInPreview { get { return ReadIntValue("MehrzeiligenEingabefelderMaxLengthInPreview", 60); } }
        public int BemerkungMaxDisplayLength { get { return ReadIntValue("BemerkungMaxDisplayLength", 20); } }
        public bool EnableMiniProfiler { get { return ReadBoolValue("EnableMiniProfiler", false); } }
        public string GisScales { get { return ReadStringValue("GisScales", ""); } }
        public string CdnUrls { get { return ReadStringValue("CDN_URLS", ""); } }
        public bool UsePrecompiledViews { get { return ReadBoolValue("UsePrecompiledViews", false); } }
        public bool UseWMSCaching { get { return ReadBoolValue("UseWMSCaching", true); } }
        public int SecurityCacheTimeout { get { return ReadIntValue("SecurityCacheTimeout", 0); } }
        public int ExportBackgroundMapScale { get { return ReadIntValue("ExportBackgroundMapScale", 10000); } }
        public long ExportBackgroundMapMaxPixelCount { get { return ReadLongValue("ExportBackgroundMapMaxPixelCount", 1000000); } }
        public int ExportTileLimit { get { return ReadIntValue("ExportTileLimit", 0); } }
        public int ExportBackgroundMapScaleSteps { get { return ReadIntValue("ExportBackgroundMapScaleSteps", 1000); } }
        public int ExportBackgroundMapDPI { get { return ReadIntValue("ExportBackgroundMapDPI", 96); } }
        public double ExportBackgroundMapBuffer { get { return ReadDoubleValue("ExportBackgroundMapBuffer", 1000); } }
        public int ExportThreads { get { return ReadIntValue("ExportThreads", 6); } }
        public int InspektionsrouteStrassenabschnittLayerReport { get { return ReadIntValue("Inspektionsroute_StrassenabschnittLayer_Report", 19); } }

        public int StrassenabschnittLayerReport { get { return ReadIntValue("StrassenabschnittLayer_Report", 33); } }
        public int StrassenabschnittLayerReportGrey { get { return ReadIntValue("StrassenabschnittLayer_Report_Grey", 34); } }
        public int[] StrassenabschnittLayerSingleColor
        {
            get
            {
                string[] stringIds = ReadStringValue("StrassenabschnittLayer_SingleColor", "14,15,16").Split(',');
                var ids = stringIds.ToList().Select(i => int.Parse(i)).ToArray();
                return ids;
            }
        }
        public int[] ZustandsabschnittLayer_Trottoir
        {
            get
            {
                string[] stringIds = ReadStringValue("ZustandsabschnittLayer_Trottoir", "10,11").Split(',');
                var ids = stringIds.ToList().Select(i => int.Parse(i)).ToArray();
                return ids;
            }
        }
        public int ZustandabschnittLayer_Index { get { return ReadIntValue("ZustandabschnittLayer_Index", 22); } }
        public int[] ZustandabschnittLayer_Trottoir_Index
        {
            get
            {
                string[] stringIds = ReadStringValue("ZustandabschnittLayer_Trottoir_Index", "27,28").Split(',');
                var ids = stringIds.ToList().Select(i => int.Parse(i)).ToArray();
                return ids;
            }
        }
        public int KoordinierteMassnahmeGISReport { get { return ReadIntValue("KoordinierteMassnahmeGIS_Report", 26); } }
        public int MassnahmenVorschlagTeilsystemeLayer { get { return ReadIntValue("MassnahmenVorschlagTeilsystemeLayer", 3); } }
        public int MassnahmenvorschlagTeilsystemeGISReport { get { return ReadIntValue("MassnahmenvorschlagTeilsystemeGIS_Report", 27); } }
        public int ZustandabschnittLayerMassnahmeTypReport { get { return ReadIntValue("ZustandabschnittLayer_MassnahmeTyp", 23); } }
        public int[] ZustandabschnittLayer_Trottoir_MassnahmeTyp
        {
            get
            {
                string[] stringIds = ReadStringValue("ZustandabschnittLayer_Trottoir_MassnahmeTyp", "31,32").Split(',');
                var ids = stringIds.ToList().Select(i => int.Parse(i)).ToArray();
                return ids;
            }
        }

        public string ClientFilesFolderPath { get { return GetServerPath(ReadStringValue("ClientFilesFolderPath", "~/App_Data/ClientFiles")); } }
        public string SldFilesFolderPath { get { return GetServerPath(ReadStringValue("SldFilesFolderPath", "~/App_Data/Sld")); } }
        public string LogFolderPath { get { return GetServerPath(ReadStringValue("LogFolderPath", "~/Logs")); } }
        public string WMSCacheFolderPath { get { return GetServerPath(ReadStringValue("WMSCacheFolderPath", "~/App_Data/WMSCache/")); } }


        private static string GetServerPath(string path)
        {
            return path.Contains("~") ? HttpContext.Current.Server.MapPath(path) : path;
        }

        public ApplicationEnvironment Environment { get { return ReadEnumValue("Environment", ApplicationEnvironment.Development); } }

        public string WMSUrl
        {
            get
            {
                string wmsUrlPrefix = "WMS_Url_";
                return ReadStringValue(wmsUrlPrefix + MachineEnvironment, null);
            }
        }

        public string WMSUrlBaseLayer
        {
            get
            {
                return ReadStringValue("WMS_SWSISSTOPO_Url", null);
            }
        }
        public bool WMTSUseControler { get { return ReadBoolValue("WMTS_SWSISSTOPO_Use_Controler", false); } }
        public string WMTSUrls { get { return LandeskarteUrls; } }
        public string WMTSAvLayer { get { return AvHintergrundLayers; } }
        public string WMTSUrlBaseLayer { get { return GetServerPath(ReadStringValue("WMTS_SWSISSTOPO_Url_EMSG", "")); } }
        public string WMSAVUrl { get { return GetServerPath(ReadStringValue("WMS_AV_Url_EMSG", "")); } }
        public string SWMSAVUrl { get { return GetServerPath(ReadStringValue("SWMS_AV_Url_EMSG", "")); } }

        public string WMTSUserName { get { return GetServerPath(ReadStringValue("WMTS_SWSISSTOPO_USERNAME", "")); } }
        public string WMTSPassword { get { return GetServerPath(ReadStringValue("WMTS_SWSISSTOPO_PASSWORD", "")); } }
        public string WMSAVUserName { get { return GetServerPath(ReadStringValue("WMS_AV_USERNAME", "")); } }
        public string WMSAVPassword { get { return GetServerPath(ReadStringValue("WMS_AV_PASSWORD", "")); } }
        public string SWMSAVUserName { get { return GetServerPath(ReadStringValue("SWMS_AV_USERNAME", "")); } }
        public string SWMSAVPassword { get { return GetServerPath(ReadStringValue("SWMS_AV_PASSWORD", "")); } }

        public string LandeskarteType { get { return ReadStringValue("Landeskarte_Type", ""); } }
        public string LandeskarteParameters { get { return ReadStringValue("Landeskarte_Parameters", "{}"); } }
        public string LandeskarteUrls { get { return GetServerPath(ReadStringValue("Landeskarte_Urls", "")); } }
        public string LandeskarteLayers { get { return ReadStringValue("Landeskarte_Layers", ""); } }
        public string LandeskartePrintMapping { get { return ReadStringValue("Landeskarte_PrintMapping", ""); } }

        public string OrthophotoType { get { return ReadStringValue("Orthophoto_Type", ""); } }
        public string OrthophotoParameters { get { return ReadStringValue("Orthophoto_Parameters", "{}"); } }
        public string OrthophotoUrls { get { return GetServerPath(ReadStringValue("Orthophoto_Urls", "")); } }
        public string OrthophotoLayers { get { return ReadStringValue("Orthophoto_Layers", ""); } }
        public string OrthophotoPrintMapping { get { return ReadStringValue("Orthophoto_PrintMapping", ""); } }

        public string AvHintergrundType { get { return ReadStringValue("AvHintergrund_Type", ""); } }
        public string AvHintergrundParameters { get { return ReadStringValue("AvHintergrund_Parameters", "{}"); } }
        public string AvHintergrundUrls { get { return GetServerPath(ReadStringValue("AvHintergrund_Urls", "")); } }
        public string AvHintergrundLayers { get { return ReadStringValue("AvHintergrund_Layers", ""); } }
        public string AvHintergrundMapping { get { return ReadStringValue("AvHintergrund_PrintMapping", ""); } }



        public string AvcHintergrundType { get { return ReadStringValue("AvcHintergrund_Type", ""); } }
        public string AvcHintergrundParameters { get { return ReadStringValue("AvcHintergrund_Parameters", "{}"); } }
        public string AvcHintergrundUrls { get { return GetServerPath(ReadStringValue("AvcHintergrund_Urls", "")); } }
        public string AvcHintergrundLayers { get { return ReadStringValue("AvcHintergrund_Layers", ""); } }

        public string GrenzfalecheType { get { return ReadStringValue("Grenzflaeche_Type", ""); } }
        public string GrenzfalecheParameters { get { return ReadStringValue("Grenzfaleche_Parameters", "{}"); } }
        public string GrenzfalecheUrls { get { return GetServerPath(ReadStringValue("Grenzflaeche_Urls", "")); } }
        public string GrenzfalecheLayers { get { return ReadStringValue("Grenzflaeche_Layers", ""); } }

        public string AvUeberlagerndType { get { return ReadStringValue("AvUeberlagernd_Type", ""); } }
        public string AvUeberlagerndParameters { get { return ReadStringValue("AvUeberlagernd_Parameters", "{}"); } }
        public string AvUeberlagerndUrls { get { return GetServerPath(ReadStringValue("AvUeberlagernd_Urls", "")); } }
        public string AvUeberlagerndLayers { get { return ReadStringValue("AvUeberlagernd_Layers", ""); } }

        public string ZusatzinformationenType { get { return ReadStringValue("Zusatzinformationen_Type", ""); } }
        public string ZusatzinformationenParameters { get { return ReadStringValue("Zusatzinformationen_Parameters", "{}"); } }
        public string ZusatzinformationenUrls { get { return GetServerPath(ReadStringValue("Zusatzinformationen_Urls", "")); } }
        public string ZusatzinformationenLayers { get { return ReadStringValue("Zusatzinformationen_Layers", ""); } }

        public string ZusatzinformationenGWVKType { get { return ReadStringValue("ZusatzinformationenGWVK_Type", ""); } }
        public string ZusatzinformationenGWVKParameters { get { return ReadStringValue("ZusatzinformationenGWVK_Parameters", "{}"); } }
        public string ZusatzinformationenGWVKUrls { get { return GetServerPath(ReadStringValue("ZusatzinformationenGWVK_Urls", "")); } }
        public string ZusatzinformationenGWVKLayers { get { return ReadStringValue("ZusatzinformationenGWVK_Layers", ""); } }

        public string ZusatzinformationenGWVULType { get { return ReadStringValue("ZusatzinformationenGWVUL_Type", ""); } }
        public string ZusatzinformationenGWVULParameters { get { return ReadStringValue("ZusatzinformationenGWVUL_Parameters", "{}"); } }
        public string ZusatzinformationenGWVULUrls { get { return GetServerPath(ReadStringValue("ZusatzinformationenGWVUL_Urls", "")); } }
        public string ZusatzinformationenGWVULLayers { get { return ReadStringValue("ZusatzinformationenGWVUL_Layers", ""); } }

        public string ZusatzinformationenKBSType { get { return ReadStringValue("ZusatzinformationenKBS_Type", ""); } }
        public string ZusatzinformationenKBSParameters { get { return ReadStringValue("ZusatzinformationenKBS_Parameters", "{}"); } }
        public string ZusatzinformationenKBSUrls { get { return GetServerPath(ReadStringValue("ZusatzinformationenKBS_Urls", "")); } }
        public string ZusatzinformationenKBSLayers { get { return ReadStringValue("ZusatzinformationenKBS_Layers", ""); } }

        public string GetWMSLayerId(string wmsLayer)
        {
            return ReadStringValue(wmsLayer, "");
        }

        public LayerCollection ExportLayer { get { return ((ExportLayerSection)System.Configuration.ConfigurationManager.GetSection("ExportLayer")).Layers; } }
    }
}
