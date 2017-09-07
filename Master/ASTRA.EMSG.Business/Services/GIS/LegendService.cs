using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Mvc;
using System.Runtime.Serialization.Json;
using ASTRA.EMSG.Common.Utils;
using System.Runtime.Serialization;
using System.Net;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common.Master.Logging;
using System.Drawing;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using GeoAPI.Geometries;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Common.Enums;
using System.Globalization;
using System.Xml;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.GIS;
using System.Drawing.Drawing2D;

namespace ASTRA.EMSG.Business.Services.GIS
{
    public interface ILegendService : IService
    {
        Stream GetLegendStream(string layer, int dpi = 72, int size = 30, string fontName = "Arial", bool fontAntiAliasing = true, string fontColor = "0x000000", string bgColor = "0xFFFFFF", int fontSize = 14, EmsgGisReportParameter reportParameter = null);
        ActionResult GetInspektionsRouteLegendImage(int LegendNumber, int dpi = 90, int size = 20, string bgColor = "0xFFFFFF");
        string GetInspektionsRouteLegendImageUrl(int legendNumber, string baseURL);
        Bitmap FormatLegendForReport(string layerList, EmsgGisReportParameter reportparams, ReportDefintion reportDefinition);
    }

    public partial class LegendService : ILegendService
    {
        private readonly IServerConfigurationProvider serverConfigurationProvider;
        private readonly ILocalizationService localizationService;
        private readonly IInspektionsRouteGISService inspektionsRouteGISService;
        private readonly IMassnahmenvorschlagKatalogService massnahmenvorschlagKatalogService;
        private readonly ITransactionScopeProvider transactionScopeProvider;
        private const string zustandabschnittLayerMassnahmeTyp = "ZustandabschnittLayer_MassnahmeTyp";
        private const string zustandabschnittLayerTrottoirMassnahmeTyp = "ZustandabschnittLayer_Trottoir_MassnahmeTyp";
        private const string inspektionsrouteStrassenabschnittLayerReport = "Inspektionsroute_StrassenabschnittLayer_Report";
        private readonly Dictionary<string, Tuple<string, string>> layerInfo;
        public LegendService(IServerConfigurationProvider serverConfigurationProvider, ILocalizationService localizationService,
            IInspektionsRouteGISService inspektionsRouteGISService, IMassnahmenvorschlagKatalogService massnahmenvorschlagKatalogService,
            ITransactionScopeProvider transactionScopeProvider)
        {
            this.serverConfigurationProvider = serverConfigurationProvider;
            this.localizationService = localizationService;
            this.inspektionsRouteGISService = inspektionsRouteGISService;
            this.massnahmenvorschlagKatalogService = massnahmenvorschlagKatalogService;
            this.transactionScopeProvider = transactionScopeProvider;

            layerInfo = new Dictionary<string, Tuple<string, string>> {
    {"AchsenUpdateKonflikteLayer" ,Tuple.Create("AchsenUpdateKonflikteStyle.sld", this.localizationService.GetLocalizedValue("MapLocalization","LayerAchsenUpdateKonflikte"))},
    {"KoordinierteMassnahmenLayer" ,Tuple.Create("KoordinierteMassnahmenStyle.sld", this.localizationService.GetLocalizedValue("MapLocalization","LayerKoordinierteMassnahmen"))},
    {"MassnahmenVorschlagTeilsystemeLayer" ,Tuple.Create("MassnahmenVorschlagTeilsystemeStyle.sld", this.localizationService.GetLocalizedValue("MapLocalization","LayerMassnahmenVorschlagTeilsysteme"))},
    {"RealisierteMassnahmenLayer" ,Tuple.Create("RealisierteMassnahmeGISStyle.sld", this.localizationService.GetLocalizedValue("MapLocalization","RealisierteMassnahmeGIS_Auswertung"))},
    {"InspektionsroutenLayer" ,Tuple.Create("InspektionsrouteStyle.sld", this.localizationService.GetLocalizedValue("MapLocalization","LayerInspektionsroute"))},
    {"ZustandsabschnittLayer" ,Tuple.Create("ZustandsabschnittStyle.sld", this.localizationService.GetLocalizedValue("MapLocalization","LayerZustandsabschnitte"))},
    {"ZustandsabschnittLayer_Trottoir" ,Tuple.Create("ZustandtrottoirLeftStyle.sld", this.localizationService.GetLocalizedValue("MapLocalization","LayerZustandsabschnitteTrottoir"))},
    {"StrassenabschnittLayer" ,Tuple.Create("StrassenabschnittGISBK.sld", this.localizationService.GetLocalizedValue("MapLocalization","LayerStrassenabschnitte"))},
    {"StrassenabschnittLayer_SingleColor" ,Tuple.Create("StrassenabschnittStyle.sld", this.localizationService.GetLocalizedValue("MapLocalization","LayerStrassenabschnitte"))},
    {zustandabschnittLayerMassnahmeTyp ,Tuple.Create("Zustandsabschnitt_Massnahmetyp_AuswertungStyle.sld", this.localizationService.GetLocalizedValue("MapLocalization","LayerZustandabschnittMassnahmeTyp"))},
    {zustandabschnittLayerTrottoirMassnahmeTyp ,Tuple.Create("Zustandsabschnitt_Massnahmetyp_Trottoir_AuswertungLeftStyle.sld", this.localizationService.GetLocalizedValue("MapLocalization","LayerZustandabschnittMassnahmeTypTrottoir"))},
    {"ZustandabschnittLayer_Index" ,Tuple.Create("Zustandabschnitt_IndexStyle.sld", this.localizationService.GetLocalizedValue("MapLocalization","LayerZustandsabschnitte"))},
    {"ZustandabschnittLayer_Trottoir_Index" ,Tuple.Create("ZustandtrottoirAuswertungLeftStyle.sld", this.localizationService.GetLocalizedValue("MapLocalization","LayerZustandsabschnitteTrottoir"))},
    {"StrassenabschnittLayer_Report" ,Tuple.Create("Strassenabschnitt_AuswertungStyle.sld", this.localizationService.GetLocalizedValue("MapLocalization","LayerStrassenabschnitte"))},
    {"StrassenabschnittLayer_Report_Grey" ,Tuple.Create("Strassenabschnitt_Auswertung_GrayStyle.sld", this.localizationService.GetLocalizedValue("MapLocalization","LayerStrassenabschnitte"))},
    {inspektionsrouteStrassenabschnittLayerReport ,Tuple.Create("Inspektionsroute_Strassenabschnitt_AuswertungStyle.sld", this.localizationService.GetLocalizedValue("MapLocalization","InspektionsrouteStrassenabschnittGISAuswertung"))},
    {"KoordinierteMassnahmeGIS_Report" ,Tuple.Create("KoordinierteMassnahme_AuswertungStyle.sld", this.localizationService.GetLocalizedValue("MapLocalization","KoordinierteMassnahmeGIS_Auswertung"))},
    {"MassnahmenvorschlagTeilsystemeGIS_Report" ,Tuple.Create("MassnahmenvorschlagTeilsysteme_AuswertungStyle.sld", this.localizationService.GetLocalizedValue("MapLocalization","LayerMassnahmenVorschlagTeilsysteme"))},
    {"AchsenSegmentLayer" ,Tuple.Create("AchsenSegmentStyle.sld", this.localizationService.GetLocalizedValue("MapLocalization","LayerAchsensegmente"))}
        };
        }
        




        public Stream GetLegendStream(string layer, int dpi = 72, int size = 30, string fontName = "Arial", bool fontAntiAliasing = true, string fontColor = "0x000000", string bgColor = "0xFFFFFF", int fontSize = 14, EmsgGisReportParameter reportParameter=null)
        {
            Tuple<string,string> sldFileName;
            if (!this.layerInfo.TryGetValue(layer, out sldFileName))
            {
                throw new Exception("No style found for layer \""+layer+"\"");
            }
            string path = Path.Combine(serverConfigurationProvider.SldFilesFolderPath, sldFileName.Item1);
            if (!File.Exists(path))
            {
                throw new Exception("SLD File \""+path+"\" not found for layer \""+layer+"\"");
            }

            XmlDocument xml = new XmlDocument();
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                xml.Load(stream);
            }
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xml.NameTable);
            nsmgr.AddNamespace("sld", "http://www.opengis.net/sld");
            nsmgr.AddNamespace("ogc", "http://www.opengis.net/ogc");
            nsmgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            nsmgr.AddNamespace("xlink", "http://www.w3.org/1999/xlink");
            nsmgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            nsmgr.AddNamespace("se", "http://www.opengis.net/se");

            switch (layer)
            {
                case zustandabschnittLayerMassnahmeTyp:
                    xml = localizeMassnahmen(xml, nsmgr, MassnahmenvorschlagKatalogTyp.Fahrbahn);
                    break;
                case zustandabschnittLayerTrottoirMassnahmeTyp:
                    xml = localizeMassnahmen(xml, nsmgr, MassnahmenvorschlagKatalogTyp.Trottoir);
                    break;
                case inspektionsrouteStrassenabschnittLayerReport:
                    xml = InsertInspektionsroutenNames(xml, nsmgr, reportParameter);
                    break;
                default:
                    xml = localizeSld(xml, nsmgr);
                    break;
            }
            string namedLayer = ((ServerConfigurationProvider)serverConfigurationProvider).GetWMSLayerId(layer).Split(',')[0];//xml.SelectSingleNode("sld:StyledLayerDescriptor/sld:NamedLayer/se:Name", nsmgr).InnerText;
            string sldBody = String.Empty;
            using (TextWriter streamwrite = new StringWriter())
            {
                using (XmlWriter writer = new XmlTextWriter(streamwrite))
                {
                    xml.WriteTo(writer);
                }
                sldBody = Uri.EscapeDataString(streamwrite.ToString());
                sldBody = sldBody.Replace("SvgParameter", "CssParameter");
            }
                      
            string wmsParams = "REQUEST=GetLegendGraphic&VERSION=1.1.0&FORMAT=image/png&WIDTH="+size+"&HEIGHT="+size+"&layer="+namedLayer+"&legend_options=fontName:"+fontName+";fontAntiAliasing:"+fontAntiAliasing+";fontColor:"+fontColor+";fontSize:"+fontSize+";bgColor:"+bgColor+";dpi:"+dpi+";forceLabels:ON&SLD_BODY="+sldBody+"";
            FileStreamResult legendFileStreamResult = WmsPostRequest(serverConfigurationProvider.WMSUrl, wmsParams, "image/png") as FileStreamResult;
            return legendFileStreamResult.FileStream;

        }
        private XmlDocument InsertInspektionsroutenNames(XmlDocument xml, XmlNamespaceManager nsmgr, EmsgGisReportParameter reportparams)
        {
            if (reportparams == null)
            {
                return this.InsertInspektionsroutenNames(xml, nsmgr);
            }
            string[] bboxCoordinates = reportparams.BoundingBox.Split(',');

            Coordinate bottomLeft = new Coordinate(float.Parse(bboxCoordinates[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(bboxCoordinates[1], CultureInfo.InvariantCulture.NumberFormat));
            Coordinate topRight = new Coordinate(float.Parse(bboxCoordinates[2], CultureInfo.InvariantCulture.NumberFormat), float.Parse(bboxCoordinates[3], CultureInfo.InvariantCulture.NumberFormat));
            Coordinate bottomRight = new Coordinate(float.Parse(bboxCoordinates[2], CultureInfo.InvariantCulture.NumberFormat), float.Parse(bboxCoordinates[1], CultureInfo.InvariantCulture.NumberFormat));
            Coordinate topLeft = new Coordinate(float.Parse(bboxCoordinates[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(bboxCoordinates[3], CultureInfo.InvariantCulture.NumberFormat));


            ILinearRing linearRing = new NetTopologySuite.Geometries.LinearRing(new Coordinate[] { topLeft, topRight, bottomRight, bottomLeft, topLeft });

            IGeometry filterGeom = new NetTopologySuite.Geometries.Polygon(linearRing);
            filterGeom.SRID = GisConstants.SRID;

            var inspekrouten = inspektionsRouteGISService.GetInspektionsrouteByFilterGeom(filterGeom);
            return this.InsertInspektionsroutenNames(xml, nsmgr, inspekrouten);
        }
        private XmlDocument InsertInspektionsroutenNames(XmlDocument xml, XmlNamespaceManager nsmgr)
        {
            var inspekrouten = inspektionsRouteGISService.GetCurrentEntities().ToList();
            return this.InsertInspektionsroutenNames(xml, nsmgr, inspekrouten);
        }
        private XmlDocument InsertInspektionsroutenNames(XmlDocument xml, XmlNamespaceManager nsmgr, IList<InspektionsRouteGIS> inspekrouten)
        {
            var rules = xml.SelectNodes("sld:StyledLayerDescriptor/sld:NamedLayer/sld:UserStyle/se:FeatureTypeStyle/se:Rule", nsmgr);
            foreach (XmlNode rule in rules)
            {
                XmlNode nameNode = rule.SelectSingleNode("se:Name", nsmgr);
                var route = inspekrouten.Where(t => t.LegendNumber.ToString() == nameNode.InnerText).FirstOrDefault();
                if (route != null)
                {
                    nameNode.InnerText = route.Bezeichnung;
                }
                else
                {
                    //Unused Legendnumber: remove
                    rule.ParentNode.RemoveChild(rule);
                }
            }
            return xml;
        }

        private XmlDocument localizeMassnahmen(XmlDocument xml, XmlNamespaceManager nsmgr, MassnahmenvorschlagKatalogTyp katType)
        {
            var rules = xml.SelectNodes("sld:StyledLayerDescriptor/sld:NamedLayer/sld:UserStyle/se:FeatureTypeStyle/se:Rule", nsmgr);
            var typen = this.transactionScopeProvider.CurrentTransactionScope.Session.QueryOver<MassnahmentypKatalog>().List().Where(m => m.KatalogTyp == katType);            
            foreach (XmlNode rule in rules)
            {
                XmlNode nameNode = rule.SelectSingleNode("se:Name", nsmgr);
                var type = typen.Where(t => t.LegendNumber.ToString() == nameNode.InnerText).FirstOrDefault();
                if (type != null)
                {
                    nameNode.InnerText = localizationService.GetLocalizedMassnahmenvorschlagTyp(type.Typ);
                }
                else
                {
                    //Unused Legendnumber: remove
                    rule.ParentNode.RemoveChild(rule);
                }
            }            
            return xml;
        }
        private XmlDocument localizeSld(XmlDocument xml, XmlNamespaceManager nsmgr)
        {
            var rules = xml.SelectNodes("sld:StyledLayerDescriptor/sld:NamedLayer/sld:UserStyle/se:FeatureTypeStyle/se:Rule", nsmgr);
            foreach (XmlNode rule in rules)
            {
                XmlNode nameNode = rule.SelectSingleNode("se:Name", nsmgr);
                string ruleName = null;
                if (nameNode != null)
                {
                    ruleName = nameNode.InnerText;
                }
                if (string.IsNullOrEmpty(ruleName))
                {
                    //Rules without names are redundant and dont need to be displayed
                    rule.ParentNode.RemoveChild(rule);
                }
                else
                {
                    nameNode.InnerText = localizationService.GetLocalizedLegendLabel(ruleName);
                }
            }
            return xml;
        }        
        private ActionResult WmsPostRequest(string requestURI,string parameters, string contentType)
        {
            byte[] data = Encoding.UTF8.GetBytes(parameters);
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(requestURI);
            myReq.Method = "POST";
            myReq.ContentType = "application/x-www-form-urlencoded";
            myReq.ServicePoint.ConnectionLimit = 1000;


            try
            {
                myReq.GetRequestStream().Write(data, 0, data.Length);
                return new FileStreamResult(myReq.GetResponse().GetResponseStream(), contentType);
            }
            catch (System.Net.WebException ex)
            {
                return new HttpNotFoundResult(ex.Message.ToString());
            }
            catch (Exception ex)
            {
                return new HttpUnauthorizedResult(ex.Message.ToString());
            }
        }
        public string GetInspektionsRouteLegendImageUrl(int legendNumber, string baseURL)
        {
            return string.Format(@"{0}/{1}", baseURL, legendNumber);
        }
        public ActionResult GetInspektionsRouteLegendImage(int LegendNumber, int dpi = 90, int size = 20, string bgColor = "0xFFFFFF")
        {
            string namedLayer = ((ServerConfigurationProvider)serverConfigurationProvider).GetWMSLayerId(inspektionsrouteStrassenabschnittLayerReport).Split(',')[0];
            string wmsParams = "REQUEST=GetLegendGraphic&VERSION=1.1.0&FORMAT=image/png&WIDTH="+size+"&HEIGHT="+size+"&layer="+namedLayer+"&legend_options=bgColor:"+bgColor+";dpi:"+dpi+";forceLabels:OFF&RULE="+LegendNumber+"";
            return WmsPostRequest(serverConfigurationProvider.WMSUrl, wmsParams, "image/png");

        }
       
        public Bitmap FormatLegendForReport(string layerList, EmsgGisReportParameter reportparams, ReportDefintion reportDefinition)
        {
          

            ServerConfigurationProvider serverConfigurationProv = new ServerConfigurationProvider();

            string[] layerToShow = layerList.Split(',');
            //calc legendBitmap
            Bitmap legendBitmap = new Bitmap(5, 5);
            legendBitmap.SetResolution(reportDefinition.dpi, reportDefinition.dpi);
            SizeF legendBitmapSize = CalcAndDrawInfoImage( legendBitmap, layerToShow, reportparams, false, reportDefinition);

            //create/draw legendBitmap
            legendBitmapSize.Width += (float)Math.Ceiling(0.05 * reportDefinition.dpi); //add buffer
            legendBitmap = new Bitmap(Convert.ToInt32(legendBitmapSize.Width), Convert.ToInt32(legendBitmapSize.Height));
            legendBitmap.SetResolution(reportDefinition.dpi, reportDefinition.dpi);
            CalcAndDrawInfoImage(legendBitmap, layerToShow, reportparams, true, reportDefinition);

            return legendBitmap;
        }

        private SizeF CalcAndDrawInfoImage(Bitmap infoImage, string[] layers, EmsgGisReportParameter reportparams, bool drawImage, ReportDefintion reportDefinition)
        {
            InfoImageHeightAndCurrentPosition currentPosAndSize = new InfoImageHeightAndCurrentPosition(reportDefinition.dpi);
            ServerConfigurationProvider serverConfigurationProv = new ServerConfigurationProvider();
            using (Graphics graphics = Graphics.FromImage(infoImage))
            {
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                #region setFilterparams

                currentPosAndSize.IncreaseSizeAndPositionY(DrawString(graphics, localizationService.GetLocalizedLegendLabel("Filterparameter"), currentPosAndSize.GetCurrentPosition(), reportDefinition, 1, drawImage));
                currentPosAndSize.IncreaseHeightAndPositionY(SpaceAndOffsets.getSpaceYBetweenHeadline(reportDefinition.dpi));

                float valueOffset = 0;
                if (reportparams != null)
                {
                    foreach (FilterListPo filterItem in reportparams.Filterparams)
                    {
                        valueOffset = Math.Max(valueOffset, graphics.MeasureString(filterItem.Name, Fontstyles.getDefaultFont()).Width);
                    }

                    foreach (FilterListPo filterItem in reportparams.Filterparams)
                    {
                        Point filterNamePos = new Point(currentPosAndSize.GetCurrentPosition().X, currentPosAndSize.GetCurrentPosition().Y);
                        Point filterValuePos = new Point(currentPosAndSize.GetCurrentPosition().X + Convert.ToInt32(valueOffset), currentPosAndSize.GetCurrentPosition().Y);

                        SizeF filterparamSize = DrawString(graphics, filterItem.Name + ":", filterNamePos, reportDefinition, drawString: drawImage);
                        SizeF filterValueSize = DrawString(graphics, " " + filterItem.Value, filterValuePos, reportDefinition, drawString: drawImage);

                        SizeF lineSize = new SizeF(filterparamSize.Width + filterValueSize.Width,
                            filterparamSize.Height < filterValueSize.Height ? filterValueSize.Height : filterparamSize.Height);

                        currentPosAndSize.IncreaseSizeAndPositionY(lineSize);
                        currentPosAndSize.IncreaseHeightAndPositionY(SpaceAndOffsets.getSpaceYBetweenLayers(reportDefinition.dpi));
                    }
                }

                #endregion

                Point lineStart = new Point(0, currentPosAndSize.GetCurrentPosition().Y + SpaceAndOffsets.getSpaceYBetweenLegendAndFilter(reportDefinition.dpi) / 2);
                Point lineEnd = new Point(infoImage.Width - 2, currentPosAndSize.GetCurrentPosition().Y + SpaceAndOffsets.getSpaceYBetweenLegendAndFilter(reportDefinition.dpi) / 2);
                graphics.DrawLine(new Pen(Color.Black, (int)Math.Ceiling(0.01 * reportDefinition.dpi)), lineStart, lineEnd);

                currentPosAndSize.IncreaseHeightAndPositionY(SpaceAndOffsets.getSpaceYBetweenLegendAndFilter(reportDefinition.dpi));

                #region set legend

                if (!layers.IsEmpty())
                {
                    currentPosAndSize.IncreaseSizeAndPositionY(DrawString(graphics, localizationService.GetLocalizedLegendLabel("Legende"), currentPosAndSize.GetCurrentPosition(), reportDefinition, 1, drawImage));
                    currentPosAndSize.IncreaseHeightAndPositionY(SpaceAndOffsets.getSpaceYBetweenHeadline(reportDefinition.dpi));

                    
                    foreach (var layer in layers)
                    {
                        string layername = layer;
                        Tuple<string, string> info;
                        if (this.layerInfo.TryGetValue(layername, out info))
                        {
                            layername = info.Item2;
                        }
                        currentPosAndSize.IncreaseSizeAndPositionY(DrawString(graphics, layername, currentPosAndSize.GetCurrentPosition(), reportDefinition, 2, drawImage));
                        Bitmap bm = new Bitmap(this.GetLegendStream(layer, dpi: reportDefinition.dpi, reportParameter: reportparams));
                        bm.SetResolution(reportDefinition.dpi, reportDefinition.dpi);
                        if (drawImage)
                        {
                            graphics.DrawImage(bm, currentPosAndSize.GetCurrentPosition().X, currentPosAndSize.GetCurrentPosition().Y);
                        }
                        currentPosAndSize.IncreaseSizeAndPositionY(new SizeF(bm.Width, bm.Height));
                        currentPosAndSize.IncreaseHeightAndPositionY(SpaceAndOffsets.getSpaceYBetweenLayers(reportDefinition.dpi));
                    }

                    lineStart = new Point(0, currentPosAndSize.GetCurrentPosition().Y + SpaceAndOffsets.getSpaceYBetweenLegendAndFilter(reportDefinition.dpi) / 2);
                    lineEnd = new Point(infoImage.Width - 2, currentPosAndSize.GetCurrentPosition().Y + SpaceAndOffsets.getSpaceYBetweenLegendAndFilter(reportDefinition.dpi) / 2);
                    graphics.DrawLine(new Pen(Color.Black, (int)Math.Ceiling(0.01 * reportDefinition.dpi)), lineStart, lineEnd);

                    currentPosAndSize.IncreaseHeightAndPositionY(SpaceAndOffsets.getSpaceYBetweenLegendAndFilter(reportDefinition.dpi));
                    currentPosAndSize.IncreaseSizeAndPositionY(DrawString(graphics, localizationService.GetLocalizedLegendLabel("MapReportRedBox"), currentPosAndSize.GetCurrentPosition(), reportDefinition, drawString: drawImage));
                    //margin: resolutions not in multiple of 300 dpi may cause rounding errors resulting in 1 pixel of the MapReportRedBox text to be cut of, therefore safity margin
                    currentPosAndSize.IncreaseHeightAndPositionY(SpaceAndOffsets.getSpaceYMargin(reportDefinition.dpi));
                }
                #endregion
            }

            return currentPosAndSize.GetInfoImageSize();
        }
        

        private SizeF DrawString(Graphics graphics, string text, System.Drawing.Point position, ReportDefintion reportDefinition, int fontType = 0, bool drawString = true)
        {

            Font font;
            SolidBrush brush = new SolidBrush(Color.Black);
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            switch (fontType)
            {
                case 1:
                    font = Fontstyles.getHadlineFont();
                    break;
                default:
                    font = Fontstyles.getDefaultFont();
                    break;
            }
            SizeF size = graphics.MeasureString(text, font);

            if (drawString)
            {
                StringFormat format = new StringFormat();
                format.Trimming = StringTrimming.None;
                RectangleF rect = new RectangleF(position, size);                
                graphics.DrawString(text, font, brush, rect);
            }


            return size;
        }

        public class InfoImageHeightAndCurrentPosition
        {

            private SizeF infoImageSize = new SizeF();
            public SizeF GetInfoImageSize() { return infoImageSize; }


            public Point GetCurrentPosition() { return new Point(this.xOffset, (int)Math.Ceiling(this.infoImageSize.Height)); }
            private int dpi;
            private int xOffset;
            private int yOffset;

            public InfoImageHeightAndCurrentPosition(int dpi)
            {
                this.dpi = dpi;
                this.xOffset = SpaceAndOffsets.getOffsetXLeft(dpi);
                this.yOffset = (int)Math.Ceiling(0.017 * dpi);
                this.infoImageSize.Height += yOffset;
                this.infoImageSize.Width += xOffset;
            }

            public void IncreaseSizeAndPositionY(SizeF size)
            {
                infoImageSize.Width = Math.Max(infoImageSize.Width, size.Width);
                infoImageSize.Height += (float)Math.Ceiling(size.Height);
            }

            public void IncreaseHeightAndPositionY(float height)
            {
                infoImageSize.Height += (float)Math.Ceiling(height);
            }

        }

        private static class Fontstyles
        {
            private static string fontFamily = "Arial";
            public static Font getDefaultFont()
            {
                return new Font(fontFamily, 12, FontStyle.Regular, GraphicsUnit.Point);
            }
            public static Font getHadlineFont()
            {
                return new Font(fontFamily, 14, FontStyle.Bold, GraphicsUnit.Point);
            }
        }

        private static class SpaceAndOffsets
        {
            //public const int SpaceYBetweenHeadline = 25;
            //public const int SpaceYBetweenLayers = 15;
            //public const int SpaceYBetweenLegendAndFilter = 100;
            //public const int SpaceYBetweenLegendItems = 5;
            //public const int SpaceBetweenLegendBitmapAndText = 3;

            //public const int OffsetXLeft = 5;
            //public const int OffsetXLegendItems = 25;
            public static int getSpaceYBetweenHeadline(int dpi) { return (int)Math.Ceiling(0.0825 * dpi); }
            public static int getSpaceYBetweenLayers(int dpi) { return (int)Math.Ceiling(0.05 * dpi); }
            public static int getSpaceYBetweenLegendAndFilter(int dpi) { return (int)Math.Ceiling(0.325 * dpi); }
            public static int getSpaceYBetweenLegendItems(int dpi) { return (int)Math.Ceiling(0.0165 * dpi); }
            public static int getSpaceBetweenLegendBitmapAndText(int dpi) { return (int)Math.Ceiling(0.01 * dpi); }
            public static int getSpaceYMargin(int dpi) { return (int)Math.Ceiling(0.01 * dpi); }

            public static int getOffsetXLeft(int dpi) { return (int)Math.Ceiling(0.0165 * dpi); }
            public static int getOffsetXLegendItems(int dpi) { return (int)Math.Ceiling(0.0825 * dpi); }


        }
    }
}
