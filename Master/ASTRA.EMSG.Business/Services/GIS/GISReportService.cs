using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Reporting;
using System.ComponentModel;
using ASTRA.EMSG.Business.Services.GIS.WMS.WMSObjects;
using ASTRA.EMSG.Business.Services.GIS.WMS;
using System.Drawing;
using System.IO;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common.Master.HttpRequest;
using ASTRA.EMSG.Common.Master.Logging;
using System.Web;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using GeoAPI.Geometries;
using System.Threading;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using System.Globalization;

namespace ASTRA.EMSG.Business.Services.GIS
{
    public class ReportDefintion
    {
        /// <summary>
        /// cm height (landscape)
        /// </summary>
        public double ReportHeight { get; set; }

        /// <summary>
        /// cm width  (landscape)
        /// </summary>
        public double ReportWidth { get; set; }

        /// <summary>
        /// report dpi
        /// </summary>
        public int dpi { get; set; }

        public int numberOfTiles;
        public int heightInPixel { get;  set; }
        public int widthInPixel { get;  set; }
        public double calcTileHeight { get; private set; }
        public double calcTileWidth { get; private set; }



        public ReportDefintion(int dpi)
        {
            //A4 - landscape - cm
            //ReportHeight = 16;
            //ReportWidth = 27;

            //A3 - landscape - cm
            ReportHeight = 24;
            ReportWidth = 42;


            this.dpi = dpi;

            numberOfTiles = 3; //n*n => eg. 4*4 = 16 requests

            double inch = 2.54000508;
            heightInPixel = Convert.ToInt32(ReportHeight / inch * dpi);
            widthInPixel = Convert.ToInt32(ReportWidth / inch * dpi);

            calcTileHeight = heightInPixel / numberOfTiles;
            calcTileWidth = widthInPixel / numberOfTiles;
        }
    }

    public class TileDefinition
    {
        public int TileHeight { get; set; }
        public int TileWidth { get; set; }

        public String TileSizeToString()
        {
            return String.Format("{1},{0}", TileHeight, TileWidth);
        }

        public double ExtentXBottomLeft { get; set; }
        public double ExtentYBottomLeft { get; set; }
        public double ExtentXTopRight { get; set; }
        public double ExtentYTopRight { get; set; }
        public int col { get; set; }
        public int row { get; set; }
        public String ExtentToString()
        {
            return String.Format(CultureInfo.InvariantCulture.NumberFormat,"{0:0.##},{1:0.##},{2:0.##},{3:0.##}", ExtentXBottomLeft, ExtentYBottomLeft, ExtentXTopRight, ExtentYTopRight);
        }
    }

    public class TileWorker
    {
        public int dpi { get; private set; }
        public List<Stream> response { get; private set; }        
        public EmsgGisReportParameter emsgGisReportParameter { get; private set; }
        public TileDefinition tile { get; private set; }
        private readonly IGISReportService gISReportService;
        private HttpContext current;
        public Exception exception { get; private set; }
        public bool isPreview { get; private set; }

        public TileWorker(EmsgGisReportParameter emsgGisReportParameter, TileDefinition tile, int dpi, IGISReportService gISReportService, HttpContext current, bool isPreview)
        {
            this.dpi = dpi;
            
            this.tile = tile;
            this.emsgGisReportParameter = emsgGisReportParameter;
            this.gISReportService = gISReportService;
            this.current = current;
            this.exception = null;
            this.isPreview = isPreview;
        }

        public void Run()
        {
            try
            {
                HttpContext.Current = this.current;
                this.response = gISReportService.PrepareAndSendWMSRequests(emsgGisReportParameter, tile, dpi, isPreview);
            }
            catch (Exception e)
            {
                this.exception = e;
            }
        }

    }
    public class RequestWorker
    {
        public int order { get; private set; }
        public HttpResponseObject response { get; private set; }
        private WMSParamBase parameters;
        private ICreateWMSRequest createWMSRequest;
        private HttpContext current;
        private WMSRequestType type;
        public Exception exception {get; private set;}

        public RequestWorker(int order, WMSParamBase parameters, ICreateWMSRequest createWMSRequest, HttpContext current, WMSRequestType type)
        {
            this.order = order;
            this.parameters = parameters;
            this.createWMSRequest = createWMSRequest;
            this.current = current;
            this.type = type;
            this.exception = null;
        }
        public void Run()
        {
            try
            {
                HttpContext.Current = this.current;
                this.response = createWMSRequest.WMSRequest(type, parameters);
            }
            catch (Exception e)
            {
                this.exception = e;
            }
        }
    }

    public interface IGISReportService : IService
    {
        string GenerateReportBitmap(EmsgGisReportParameter emsgGisReportParameter);
        List<Stream> PrepareAndSendWMSRequests(EmsgGisReportParameter emsgGisReportParameter, TileDefinition tile, int dpi, bool isPreview);
    }


    public class GISReportService : IGISReportService
    {
        private readonly ICreateWMSRequest createWMSRequest;
        private readonly IServerPathProvider serverPathProvider;
        private readonly ILocalizationService localizationService;
        private readonly ILegendService legendService;

        public GISReportService(
            ICreateWMSRequest createWmsRequest,
            IServerPathProvider serverPathProvider,
            ILocalizationService localizationService,
            ILegendService legendService)
        {
            createWMSRequest = createWmsRequest;
            this.serverPathProvider = serverPathProvider;
            this.localizationService = localizationService;
            this.legendService = legendService;
            
        }

        public string GenerateReportBitmap(EmsgGisReportParameter emsgGisReportParameter)
        {
            Size webPreviewSize = new Size(int.Parse(emsgGisReportParameter.MapSize.Split(',')[0]),
                int.Parse(emsgGisReportParameter.MapSize.Split(',')[1]));
            int dpi = emsgGisReportParameter.IsPreview ? 72 : 300;
            ReportDefintion reportDefinition = new ReportDefintion(dpi);

            Bitmap legendBitmap = legendService.FormatLegendForReport(emsgGisReportParameter.Layers,  emsgGisReportParameter, reportDefinition);
           
            //reportDefinition.widthInPixel -= legendBitmap.Width;
            Bitmap map = new Bitmap(Convert.ToInt32(reportDefinition.widthInPixel - legendBitmap.Width), Convert.ToInt32(reportDefinition.heightInPixel));

            emsgGisReportParameter.BoundingBox = UpdateExtent(map.Size, emsgGisReportParameter.BoundingBox);

            List<TileDefinition> tiles = DivideIntoTiles(reportDefinition.numberOfTiles, map.Size, emsgGisReportParameter.BoundingBox);

            

            List<Thread> threads = new List<Thread>();

            List<TileWorker> workers = new List<TileWorker>();
            
            foreach (TileDefinition tile in tiles)
            {
                TileWorker worker = new TileWorker(emsgGisReportParameter, tile, dpi, this, HttpContext.Current, emsgGisReportParameter.IsPreview);
                //List<Stream> responseList = PrepareAndSendWMSRequests(emsgGisReportParameter, tile, reportDefinition.dpi);
                Thread thread = new Thread(new ThreadStart(worker.Run));
                thread.Start();
                threads.Add(thread);
                workers.Add(worker);

                //AddTileToBitmap(map, responseList, reportDefinition, tilePosition);
                
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            foreach (TileWorker worker in workers)
            {
                if (worker.exception != null)
                {
                    throw worker.exception;
                }
                AddTileToBitmap(map, worker.response, reportDefinition, worker.tile, tiles);
                
            }


            Bitmap all = AddMapInfosToMapBitmap(map,
                legendBitmap,
                emsgGisReportParameter,
                reportDefinition.dpi,
                webPreviewSize
                );

            //return all;
            //return map;
            string extension = emsgGisReportParameter.IsPreview ? ".jpg" : ".png";
            string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + extension);
            if (emsgGisReportParameter.IsPreview)
            {
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 80L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                all.Save(tempPath, jpgEncoder, myEncoderParameters);
            }
            else 
            {
                all.Save(tempPath, ImageFormat.Png);
            }
            return tempPath;

        }
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        public List<Stream> PrepareAndSendWMSRequests(EmsgGisReportParameter emsgGisReportParameter, TileDefinition tile, int dpi, bool isPreview)
        {
            List<Stream> responseStreamList = new List<Stream>();
            List<Thread> threads = new List<Thread>();
            List<RequestWorker> workers = new List<RequestWorker>();

            #region request background tile
            if (!string.IsNullOrEmpty(emsgGisReportParameter.BackgroundLayers))
            {
                WMSParameter wmsBackrParameter = new WMSParameter(LAYERS: emsgGisReportParameter.BackgroundLayers,
                                                                  BBOX: tile.ExtentToString(),
                                                                  HEIGHT: tile.TileHeight.ToString(),
                                                                  WIDTH: tile.TileWidth.ToString(),
                                                                  FORMAT: isPreview ? "jpeg" : "png24");
                //HttpResponseObject tileResponseBackground = createWMSRequest.WMSRequest(WMSRequestType.BackgroundLayer, wmsBackrParameter);
                RequestWorker worker = new RequestWorker(0, wmsBackrParameter, this.createWMSRequest, HttpContext.Current, WMSRequestType.BackgroundLayer);
                Thread thread = new Thread(new ThreadStart(worker.Run));
                thread.Start();
                threads.Add(thread);
                workers.Add(worker);
                //if (ValidateResponse(tileResponseBackground))
                //    responseStreamList.Add(tileResponseBackground.responseStream);              
            }
            else
            { 
                
                WMSParameter wmsAVBackgroundParameter = new WMSParameter(LAYERS: emsgGisReportParameter.LayersAVBackground,
                                                                      BBOX: tile.ExtentToString(),
                                                                      HEIGHT: tile.TileHeight.ToString(),
                                                                      WIDTH: tile.TileWidth.ToString(),
                                                                      FORMAT: "image/png");

                //HttpResponseObject tileResponseAVBackground = createWMSRequest.WMSRequest(WMSRequestType.AVCLayer, wmsAVBackgroundParameter);
                RequestWorker worker = new RequestWorker(0, wmsAVBackgroundParameter, this.createWMSRequest, HttpContext.Current, WMSRequestType.AVCLayer);
                Thread thread = new Thread(new ThreadStart(worker.Run));
                thread.Start();
                threads.Add(thread);
                workers.Add(worker);
                //if(ValidateResponse(tileResponseAVBackground))
                //    responseStreamList.Add(tileResponseAVBackground.responseStream);
            }
            #endregion


            
          
            #region request AV tile
            if (emsgGisReportParameter.LayersAV != null)
            {
                var reportDefinition = new ReportDefintion(dpi);

                bool containsHausnummernLayer = false;

                var avLayer = emsgGisReportParameter.LayersAV.Split(',').ToList();

                containsHausnummernLayer = avLayer.Remove(new ServerConfigurationProvider().AvUeberlagerndLayers);

                //the Layer "Hausnummer" aka "ch.bfs.gebaeude_wohnungs_register-label" this layer only contains labels Because we request high resolution images this labels become unreadable small so this layer is requested with "low dpi" and then resized
                #region request Hausnummer tile
                if (containsHausnummernLayer)
                {
                    int reducedTileHeight = (int)Math.Ceiling((tile.TileHeight / reportDefinition.dpi * 120d));
                    int reducedTileWidth = (int)Math.Ceiling((tile.TileWidth / reportDefinition.dpi * 120d));

                    WMSParameter wmsAVHNParameter = new WMSParameter(LAYERS: new ServerConfigurationProvider().AvUeberlagerndLayers,
                                                                      BBOX: tile.ExtentToString(),
                                                                      HEIGHT: reducedTileHeight.ToString(),
                                                                      WIDTH: reducedTileWidth.ToString());

                    //HttpResponseObject tileResponseAVHN = createWMSRequest.WMSRequest(WMSRequestType.AVLayer, wmsAVHNParameter);
                    RequestWorker worker = new RequestWorker(1, wmsAVHNParameter, this.createWMSRequest, HttpContext.Current, WMSRequestType.AVLayer);
                    Thread thread = new Thread(new ThreadStart(worker.Run));
                    thread.Start();
                    threads.Add(thread);
                    workers.Add(worker);
                    //if (ValidateResponse(tileResponseAVHN))
                    //    responseStreamList.Add(resizeBitmap(tileResponseAVHN.responseStream, tile.TileHeight, tile.TileWidth));
                }

                
                string avLayerToDownload = string.Join(",", avLayer);

                WMSParameter wmsAVParameter = new WMSParameter(LAYERS: avLayerToDownload,
                                                                  BBOX: tile.ExtentToString(),
                                                                  HEIGHT: tile.TileHeight.ToString(),
                                                                  WIDTH: tile.TileWidth.ToString());

                //HttpResponseObject tileResponseAV = createWMSRequest.WMSRequest(WMSRequestType.AVLayer, wmsAVParameter);
                RequestWorker avworker = new RequestWorker(2, wmsAVParameter, this.createWMSRequest, HttpContext.Current, WMSRequestType.AVLayer);
                Thread avthread = new Thread(new ThreadStart(avworker.Run));
                avthread.Start();
                threads.Add(avthread);
                workers.Add(avworker);
                //if (ValidateResponse(tileResponseAV))
                //    responseStreamList.Add(resizeBitmap(tileResponseAV.responseStream, tile.TileHeight, tile.TileWidth));

                
               
                #endregion request Hausnummer tile
            }
            #endregion

            #region request EMSG feature tile
            if (emsgGisReportParameter.Layers != null)
            {
                var emsgLayers = emsgGisReportParameter.Layers.Split(',').ToList();
                emsgLayers.Reverse();
                string emsgLayersToDownload = string.Join(",", emsgLayers);

                WMSRestParameter wmsParameter = new WMSRestParameter(emsgLayersToDownload,
                                                                    tile.ExtentToString(),
                                                                    tile.TileSizeToString(),
                                                                    layerdefs: emsgGisReportParameter.LayerDefs,
                                                                    dpi: dpi.ToString());

                //HttpResponseObject tileResponseEMSGFeatures = createWMSRequest.WMSRequest(WMSRequestType.OverlayEMSGLayer, wmsParameter);
                RequestWorker worker = new RequestWorker(3, wmsParameter, this.createWMSRequest, HttpContext.Current, WMSRequestType.OverlayEMSGLayer);
                Thread thread = new Thread(new ThreadStart(worker.Run));
                thread.Start();
                threads.Add(thread);
                workers.Add(worker);

                //if (ValidateResponse(tileResponseEMSGFeatures))
                //    responseStreamList.Add(tileResponseEMSGFeatures.responseStream);
            }
            #endregion


            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            workers = workers.OrderBy(w => w.order).ToList();
            foreach (RequestWorker worker in workers)
            {
                if (worker.exception != null)
                {
                    throw worker.exception;
                }
                if (ValidateResponse(worker.response))
                    responseStreamList.Add(worker.response.responseStream);
            }


            return responseStreamList;
        }

        private bool ValidateResponse(HttpResponseObject httpResponseObject) 
        {
            if (httpResponseObject != null && !httpResponseObject.contentType.Contains("text"))
                return true;
            else
            {
                if (httpResponseObject != null)
                    using (StreamReader reader = new StreamReader(httpResponseObject.responseStream))
                        Loggers.ApplicationLogger.ErrorFormat(reader.ReadToEnd());
            }
            return false;
        }

        private Stream resizeBitmap(Stream inputStream, int targetHeight, int targetWidth)
        {
            Bitmap result = new Bitmap(targetWidth, targetHeight);
            using (Graphics g = Graphics.FromImage((Image)result))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                Bitmap bmp = new Bitmap(inputStream);
                g.DrawImage(bmp, 0, 0, targetWidth, targetHeight);
                bmp.Dispose();
            }
            Stream resultStream = new MemoryStream();
            result.Save(resultStream, ImageFormat.Png);
            result.Dispose();
            resultStream.Seek(0,0);
            return resultStream;

        }
        private string UpdateExtent(Size reportDefinition, string bbox)
        {
            string[] bboxCoordinates = bbox.Split(',');
            ////[0] - bottom left x
            ////[1] - bottom left y
            ////[2] - top right x
            ////[3] - top right y

            PointF bottomLeft = new PointF(float.Parse(bboxCoordinates[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(bboxCoordinates[1], CultureInfo.InvariantCulture.NumberFormat));
            PointF topRight = new PointF(float.Parse(bboxCoordinates[2], CultureInfo.InvariantCulture.NumberFormat), float.Parse(bboxCoordinates[3], CultureInfo.InvariantCulture.NumberFormat));
          

            float ratio1 = (float)reportDefinition.Width / (float)reportDefinition.Height;

            float xLength = topRight.X - bottomLeft.X;
            float yHeight = topRight.Y - bottomLeft.Y;
            float ratio2 = xLength / yHeight;

            SizeF additionalDistance = new SizeF(0,0);
            if (ratio1 < ratio2)
                additionalDistance.Height = xLength / ratio1 - yHeight;
            else
                additionalDistance.Width = yHeight * ratio1 - xLength;

                       
            PointF newBottomLeft = new PointF(bottomLeft.X - (additionalDistance.Width / 2),
                bottomLeft.Y - (additionalDistance.Height / 2));
                                  
            PointF newTopRight = new PointF(topRight.X + (additionalDistance.Width / 2), 
                topRight.Y + (additionalDistance.Height / 2));


            return string.Format(CultureInfo.InvariantCulture.NumberFormat,"{0},{1},{2},{3}", newBottomLeft.X, newBottomLeft.Y, newTopRight.X, newTopRight.Y);
        }

        private Rectangle ScalePreviewToReport(Size mapSize, int dpi, Size wmsPreviewSize, int rectLineWidth)
        {           
            var widthScale = mapSize.Width / (double)wmsPreviewSize.Width;
            var heightScale = mapSize.Height / (double)wmsPreviewSize.Height;

            var scale = Math.Min(widthScale, heightScale);
            Size previewSize = new Size(
                (int)Math.Round((wmsPreviewSize.Width * scale)) - 2 * rectLineWidth,
                (int)Math.Round((wmsPreviewSize.Height * scale)) - 2 * rectLineWidth
                );

            Point rectPoint = new Point(Convert.ToInt32((mapSize.Width - previewSize.Width) / 2 + rectLineWidth)-1,
                Convert.ToInt32((mapSize.Height - previewSize.Height) / 2) + rectLineWidth-1);

            return new Rectangle(rectPoint, previewSize);

        }

        private Bitmap AddMapInfosToMapBitmap(Bitmap map, Bitmap legendBitmap, EmsgGisReportParameter emsgGisReportParameter, int dpi, Size webPreviewSize)
        {
            var widthScale = map.Size.Width / (float)webPreviewSize.Width;
            var heightScale = map.Size.Height / (float)webPreviewSize.Height;
            map.SetResolution(dpi, dpi);
            var scale = Math.Min(widthScale, heightScale);

            float lineLength = float.Parse(emsgGisReportParameter.ScaleWidth, CultureInfo.InvariantCulture.NumberFormat) * scale;
            string lineText = emsgGisReportParameter.ScaleText;
            int fontSize = 10;
            int padding = (int)Math.Ceiling(0.033*dpi);
            int margin = (int)Math.Ceiling(0.05 * dpi);

            Font font = new Font("Arial", fontSize, FontStyle.Regular, GraphicsUnit.Point);
            SolidBrush brush = new SolidBrush(Color.Black);

            using (Graphics graphics = Graphics.FromImage(map))
            {
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                SizeF sizeLength = graphics.MeasureString(lineText, font);

                #region north arrow
                Bitmap northArrowBitmap = new Bitmap(Bitmap.FromFile(serverPathProvider.MapPath(@"~/Content/images/north_arrow.png")));
                
                Point ancorNorthArrow = new Point(map.Width - (int)(northArrowBitmap.Width/96d*dpi) - padding, padding);
                graphics.DrawImage(northArrowBitmap, ancorNorthArrow);

                #endregion

                #region length
                //Point ancor = new Point(map.Width - 80, map.Height - 40);

                IGeometry bbox = GISService.GetGeometryFromBoundingBox(emsgGisReportParameter.BoundingBox);
                double numericScale = bbox.Envelope.EnvelopeInternal.Width / ((new ReportDefintion(dpi).ReportWidth / 100));
                int scaleRoundingFactor = 1000;
                if (numericScale < 1000)
                {
                    scaleRoundingFactor = 100;
                }

                numericScale = Math.Round(numericScale / scaleRoundingFactor) * scaleRoundingFactor;
                string scaletext = string.Format("1:{0}", (numericScale.ToString("###,###", new System.Globalization.CultureInfo(localizationService.CurrentCultureCode))));
                SizeF scaleSize = graphics.MeasureString(scaletext, font);
                float rectangleWidth = (lineLength > scaleSize.Width ? lineLength : scaleSize.Width) + margin * 2;
                float rectangleHeight = (scaleSize.Height + sizeLength.Height) + margin * 2;

                Point ancor = new Point(map.Width - Convert.ToInt32(rectangleWidth) - padding, map.Height - Convert.ToInt32(rectangleHeight) - padding);

                Point endLineP = new Point(Convert.ToInt32(ancor.X + rectangleWidth / 2 + lineLength / 2), ancor.Y + Convert.ToInt32(sizeLength.Height) + margin);
                Point startLineP = new Point(Convert.ToInt32(ancor.X + rectangleWidth / 2 - lineLength / 2), ancor.Y + Convert.ToInt32(sizeLength.Height) + margin);

                SizeF lineScaleSize = graphics.MeasureString(lineText, font);
                Rectangle rect = new Rectangle(ancor.X,
                    ancor.Y,
                    Convert.ToInt32(rectangleWidth),
                    Convert.ToInt32(rectangleHeight));
                graphics.FillRectangle(new SolidBrush(Color.WhiteSmoke), rect);

                Pen pen = new Pen(Color.Black, 2 * dpi / 100);
                graphics.DrawLine(pen, startLineP, endLineP);
                graphics.DrawLine(pen, startLineP, new Point(startLineP.X, startLineP.Y - Convert.ToInt32(sizeLength.Height) / 2));
                graphics.DrawLine(pen, endLineP, new Point(endLineP.X, endLineP.Y - Convert.ToInt32(sizeLength.Height) / 2));
                graphics.DrawString(lineText, font, brush, ancor.X + ((rectangleWidth + margin) / 2) - sizeLength.Width / 2, ancor.Y + margin);
                graphics.DrawString(scaletext, font, brush, ancor.X + ((rectangleWidth + margin) / 2) - scaleSize.Width / 2, ancor.Y + scaleSize.Height + margin);

                #endregion

                northArrowBitmap.Dispose();
            }

            #region add legend and preview rectangle
            int rectLineWidth = (int)Math.Ceiling(0.01*dpi);
            Rectangle previewRect = ScalePreviewToReport(map.Size, dpi, webPreviewSize, rectLineWidth);
            Bitmap all = new Bitmap(map.Width + legendBitmap.Width, map.Height);
            all.SetResolution(dpi, dpi);
            if (map != null && legendBitmap != null)
            {
                using (Graphics graphics = Graphics.FromImage(all))
                {
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.DrawImage(map, 0, 0);
                    graphics.DrawRectangle(new Pen(Color.Red, (float)Math.Ceiling(0.01*dpi)), previewRect);
                    graphics.FillRectangle(new SolidBrush(Color.White), map.Width, 0, legendBitmap.Width, map.Height);
                    graphics.DrawImage(legendBitmap, map.Width, 0);
                    graphics.DrawRectangle(new Pen(Color.Black, (float)Math.Ceiling(0.01 * dpi)), map.Width + rectLineWidth - 1, 0 + rectLineWidth, legendBitmap.Width - 2 * rectLineWidth, map.Height - 2 * rectLineWidth);
                }
            }
            map.Dispose();
            legendBitmap.Dispose();
            #endregion
            return all;
        }

        private List<TileDefinition> DivideIntoTiles(int numberOfTiles, Size mapSize, string extent)
        {
            string[] bboxCoordinates = extent.Split(',');
            ////[0] - bottom left x
            ////[1] - bottom left y
            ////[2] - top right x
            ////[3] - top right y

            //zero pont = bottom left
            double zeroPointX = double.Parse(bboxCoordinates[0], CultureInfo.InvariantCulture.NumberFormat);
            double zeroPointY = double.Parse(bboxCoordinates[1], CultureInfo.InvariantCulture.NumberFormat);

            double extentTileSizeX = (double.Parse(bboxCoordinates[2], CultureInfo.InvariantCulture.NumberFormat) - zeroPointX) / numberOfTiles;
            double extentTileSizeY = (double.Parse(bboxCoordinates[3], CultureInfo.InvariantCulture.NumberFormat) - zeroPointY) / numberOfTiles;

            List<TileDefinition> tiles = new List<TileDefinition>();
            for (int indexY = 0; indexY < numberOfTiles; indexY++)
            {
                for (int indexX = 0; indexX < numberOfTiles; indexX++)
                {
                    tiles.Add(new TileDefinition
                    {
                        TileHeight = (mapSize.Height / numberOfTiles),
                        TileWidth = (mapSize.Width / numberOfTiles),

                        ExtentXBottomLeft = zeroPointX + (extentTileSizeX * indexX),
                        ExtentYBottomLeft = zeroPointY + (extentTileSizeY * indexY),

                        ExtentXTopRight = zeroPointX + (extentTileSizeX * (indexX + 1)),
                        ExtentYTopRight = zeroPointY + (extentTileSizeY * (indexY + 1)),

                        col = indexX,
                        row = indexY
                    });
                }
            }
            return tiles;
        }

        private void AddTileToBitmap(Bitmap map, List<Stream> tileStreamList, ReportDefintion reportDefinition, TileDefinition tile, List<TileDefinition> tiles)
        {
            
            int x = tiles.Where(t => (t.col < tile.col) && (t.row == tile.row)).Sum(t => t.TileWidth);
            int y = tiles.Where(t => (t.row > tile.row) && (t.col == tile.col)).Sum(t => t.TileHeight);
            foreach (Stream tileStream in tileStreamList)
            {
                using (Graphics graphics = Graphics.FromImage(map))
                {
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    Bitmap temp = new Bitmap(tileStream);
                    //graphics.DrawImage(temp, tilePosition.xPos, tilePosition.yPos, map.Width / reportDefinition.numberOfTiles, map.Height / reportDefinition.numberOfTiles);
                    graphics.DrawImage(temp, x, y, tile.TileWidth, tile.TileHeight);
                    temp.Dispose();
                    tileStream.Close();
                    tileStream.Dispose();
                }
            }
        }
    }
}
