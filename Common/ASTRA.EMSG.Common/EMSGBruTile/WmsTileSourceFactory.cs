using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Common.EMSGBruTile.Generated;
using BruTile;
using ASTRA.EMSG.Common.Utils;
using System.Globalization;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using BruTile.Wmsc;
using BruTile.Web;
using System.Net;
using System.IO;
using System.Xml.Serialization;

namespace ASTRA.EMSG.Common.EMSGBruTile
{

    public class WmsTileSourceFactory: ITileSourceFactory
    {
        /// <summary>
        /// According to OGC SLD 1.0 specification:
        /// The "standardized rendering pixel size" is defined to be 0.28mm × 0.28mm (millimeters).
        /// </summary>
        private const double ScaleHint = 0.00028;
        private const int tileSize = 256;
        private WMS_Capabilities capabilities;
        private ILayerConfig layerConfig;
        public string Format { get; private set; }
        public WmsTileSourceFactory(ILayerConfig layerConfig)
        {
            this.layerConfig = layerConfig;
              Uri uri = new Uri(layerConfig.Url);
            var request = (HttpWebRequest)WebRequest.Create(uri);

            //referer and username password for access
            request.Referer = "http://astra.admin.ch";
            if (String.IsNullOrEmpty(layerConfig.Username) && String.IsNullOrEmpty(layerConfig.Password))
            {
                request.Credentials = new NetworkCredential(layerConfig.Username, layerConfig.Password);
            }
            WebResponse resp = request.GetResponse();


           
            var ser = new XmlSerializer(typeof(WMS_Capabilities));

            using (var reader = new StreamReader(resp.GetResponseStream()))
            {
                capabilities = (WMS_Capabilities)ser.Deserialize(reader);
            }
            this.Format = !String.IsNullOrEmpty(layerConfig.Format) && capabilities.Capability.Request.GetMap.Format.Contains(layerConfig.Format)
                ? layerConfig.Format
                : capabilities.Capability.Request.GetMap.Format.First();
        }
        public ITileSource GetTileSource()
        {
            var styles = layerConfig.Style.Split(',');
            var layers = layerConfig.WMSLayer.Split(',').ToList();
            var schema = generateSchema();
            var wmscRequest = new WmscRequest(new Uri(capabilities.Capability.Request.GetMap.DCPType.First().HTTP.Get.OnlineResource.href), schema, layers, styles, layerConfig.DimensionsDict, capabilities.version);
            return new TileSource(new WebTileProvider(wmscRequest, null, new TileFetcher(layerConfig.Username, layerConfig.Password).fetchTile), schema);
        }
        private ITileSchema generateSchema()
        {
            var schema = new TileSchema { Name = this.layerConfig.Name };


            schema.Srs = GisConstants.SRS;
            schema.Width = tileSize;
            schema.Height = tileSize;
            schema.Format = this.Format;
            var bbox = capabilities.Capability.Layer.BoundingBox.SingleOrDefault(b => b.CRS == GisConstants.SRS);
            double minx, miny, maxx, maxy = Double.NaN;
            if (bbox == null)
            {
                var exbbox = capabilities.Capability.Layer.EX_GeographicBoundingBox;
                CoordinateSystemFactory cf = new CoordinateSystemFactory();
                CoordinateTransformationFactory f = new CoordinateTransformationFactory();
                ICoordinateSystem sys4326 = cf.CreateFromWkt(GisConstants.Wkt4326);
                ICoordinateSystem sys21781 = cf.CreateFromWkt(GisConstants.Wkt21781);
                ICoordinateTransformation transformer = f.CreateFromCoordinateSystems(sys4326, sys21781);
                double[] lowerLeft = transformer.MathTransform.Transform(new double[] { exbbox.southBoundLatitude, exbbox.eastBoundLongitude });
                double[] upperRight = transformer.MathTransform.Transform(new double[] { exbbox.northBoundLatitude, exbbox.westBoundLongitude });
                miny = lowerLeft[0];
                minx = lowerLeft[1];
                maxy = upperRight[0];
                maxx = upperRight[1];
            }
            else
            {
                minx = bbox.minx;
                miny = bbox.miny;
                maxx = bbox.maxx;
                maxy = bbox.maxy;
            }


            schema.Extent = new Extent(bbox.minx, bbox.miny, bbox.maxx, bbox.maxy);

            
            schema.OriginX = bbox.minx;
            schema.OriginY = bbox.maxy;
            schema.Axis = AxisDirection.InvertedY;


            var count = 0;

            foreach (var resolution in GisConstants.ExportResolutions)
            {

                var levelId = count.ToString(CultureInfo.InvariantCulture);
                schema.Resolutions[levelId] = new Resolution
                {
                    Id = levelId,
                    UnitsPerPixel = resolution,
                    ScaleDenominator = resolution / ScaleHint,
                    Left = Convert.ToDouble(minx, CultureInfo.InvariantCulture),
                    Top = Convert.ToDouble(maxy, CultureInfo.InvariantCulture),
                    TileWidth = tileSize,
                    TileHeight = tileSize
                };
                count++;
            }

            return schema;
        }
    }
}
