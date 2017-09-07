using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using BruTile.Wmts.Generated;
using System.Xml.Serialization;
using System.IO;
using BruTile.Wmts;
using BruTile.Web;
using BruTile;
using GeoAPI.Geometries;
using System.Globalization;
using ASTRA.EMSG.Common.EMSGBruTile;
using Microsoft.Win32;

namespace ASTRA.EMSG.Common.EMSGBruTile
{
    public class WmtsTileSourceFactory : ASTRA.EMSG.Common.EMSGBruTile.ITileSourceFactory
    {
        private ILayerConfig layer;
        private Capabilities capabilities;
        public string Format { get; private set; }
        public WmtsTileSourceFactory(ILayerConfig layer)
        {
            this.layer = layer;
            Uri uri = new Uri(layer.Url);
            var request = (HttpWebRequest)WebRequest.Create(uri);

            //referer and username password for access
            request.Referer = "http://astra.admin.ch";
            if (String.IsNullOrEmpty(layer.Username) && String.IsNullOrEmpty(layer.Password))
            {
                request.Credentials = new NetworkCredential(layer.Username, layer.Password);
            }
            WebResponse resp = request.GetResponse();


            
            var ser = new XmlSerializer(typeof(Capabilities));

            using (var reader = new StreamReader(resp.GetResponseStream()))
            {
                this.capabilities = (Capabilities)ser.Deserialize(reader);
            }

            var capLayer = this.capabilities.Contents.Layers.Single(l => l.Identifier.Value == layer.WMSLayer);

            //we only want one MatrixSet either the first or the user defined
            if (String.IsNullOrEmpty(layer.MatrixSet))
            {
                capLayer.TileMatrixSetLink = new TileMatrixSetLink[] { capLayer.TileMatrixSetLink.First() };
            }
            else
            {
                capLayer.TileMatrixSetLink = new TileMatrixSetLink[] { capLayer.TileMatrixSetLink.Single(tm => tm.TileMatrixSet == layer.MatrixSet) };
            }

            //ditto with style: either the the first, default, or the user defined
            if (String.IsNullOrEmpty(layer.Style))
            {
                Style style = capLayer.Style.SingleOrDefault(s => s.isDefault);
                if (style == null)
                {
                    style = capLayer.Style.First();
                }
                capLayer.Style = new Style[] { style };
            }
            else
            {
                capLayer.Style = new Style[] { capLayer.Style.SingleOrDefault(s => s.Identifier.Value == layer.Style) };
            }

            //ditto with format: either first or user defined
            Format = String.IsNullOrEmpty(layer.Format) ? capLayer.Format.First() : layer.Format;
            URLTemplateType[] urls = capLayer.ResourceURL.Where(u => u.format == Format).ToArray();
            capLayer.Format = new string[] { Format };
            capLayer.ResourceURL = urls;
        }
        public ITileSource GetTileSource()
        {
            var ser = new XmlSerializer(typeof(Capabilities));

            Stream mstream = new MemoryStream();
            ser.Serialize(mstream, capabilities);
            mstream.Seek(0, 0);

            var sources = WmtsParser.Parse(mstream);
            var source = sources.Single(s => s.Title == layer.WMSLayer);

            source = GetLayers(capabilities, sources.Select(s =>s.Schema).ToList(), layer.DimensionsDict, layer.Username, layer.Password).Single(s => s.Title == layer.WMSLayer);
            return source;
        }

    

        private IEnumerable<ITileSource> GetLayers(Capabilities capabilties, List<ITileSchema> tileSchemas, Dictionary<string, string> dimensions, string username, string password)
        {
            var tileSources = new List<ITileSource>();

            foreach (var layer in capabilties.Contents.Layers)
            {
                foreach (var tileMatrixLink in layer.TileMatrixSetLink)
                {
                    foreach (var style in layer.Style)
                    {
                        IRequest wmtsRequest = null;                     
                        if (layer.ResourceURL == null)
                        {
                            wmtsRequest = new WmtsRequest(CreateResourceUrlsFromOperations(
                                capabilties.OperationsMetadata.Operation,
                                layer.Format.First(),
                                capabilties.ServiceIdentification.ServiceTypeVersion.First(),
                                layer.Identifier.Value,
                                style.Identifier.Value,
                                tileMatrixLink.TileMatrixSet), layer.Dimension, dimensions);
                        }
                        else
                        {
                            wmtsRequest = new WmtsRequest(CreateResourceUrlsFromResourceUrlNode(
                                layer.ResourceURL,
                                style.Identifier.Value,
                                tileMatrixLink.TileMatrixSet), layer.Dimension, dimensions);
                        }
                        var tileSchema = tileSchemas.First(s => Equals(s.Name, tileMatrixLink.TileMatrixSet));
                        var tileSource = new TileSource(new WebTileProvider(wmtsRequest, null, new TileFetcher(username, password).fetchTile), tileSchema)
                            {
                                Title = layer.Identifier.Value
                            };
                        tileSources.Add(tileSource);
                    }
                }
            }
            return tileSources;
        }

        private IEnumerable<ResourceUrl> CreateResourceUrlsFromOperations(IEnumerable<Operation> operations,
            string format, string version, string layer, string style, string tileMatrixSet)
        {
            var list = new List<KeyValuePair<string, string>>();
            foreach (var operation in operations)
            {
                if (!operation.name.ToLower().Equals("gettile")) continue;
                foreach (var dcp in operation.DCP)
                {
                    foreach (var item in dcp.Http.Items)
                    {
                        foreach (var constraint in item.Constraint)
                        {
                            foreach (var allowedValue in constraint.AllowedValues)
                            {
                                list.Add(new KeyValuePair<string, string>(((BruTile.Wmts.Generated.ValueType)allowedValue).Value, item.href));
                            }
                        }
                    }
                }
            }

            return list.Select(s => new ResourceUrl
                {
                    Template = s.Key.ToLower() == "kvp" ?
                        CreateKvpFormatter(s.Value, format, version, layer, style, tileMatrixSet) :
                        CreateRestfulFormatter(s.Value, format, style, tileMatrixSet),
                    ResourceType = URLTemplateTypeResourceType.tile,
                    Format = format
                });
        }

        private string CreateRestfulFormatter(string baseUrl, string format, string style, string tileMatrixSet)
        {
            if (!baseUrl.EndsWith("/")) baseUrl += "/";
            return new StringBuilder(baseUrl).Append(style).Append("/").Append(tileMatrixSet)
                .Append("/{TileMatrix}/{TileRow}/{TileCol}").Append(".").Append(format).ToString();
        }

        private string CreateKvpFormatter(string baseUrl, string format, string version, string layer, string style, string tileMatrixSet)
        {
            var requestBuilder = new StringBuilder(baseUrl);
            if (!baseUrl.Contains("?")) requestBuilder.Append("?");
            requestBuilder.Append("SERVICE=").Append("WMTS")
                          .Append("&REQUEST=").Append("GetTile")
                          .Append("&VERSION=").Append(version)
                          .Append("&LAYER=").Append(layer)
                          .Append("&STYLE=").Append(style)
                          .Append("&TILEMATRIXSET=").Append(tileMatrixSet)
                          .Append("&TILEMATRIX=").Append(WmtsRequest.ZTag)
                          .Append("&TILEROW=").Append(WmtsRequest.YTag)
                          .Append("&TILECOL=").Append(WmtsRequest.XTag)
                          .Append("&FORMAT=").Append(format);
            return requestBuilder.ToString();
        }

        private static IEnumerable<ResourceUrl> CreateResourceUrlsFromResourceUrlNode(IEnumerable<URLTemplateType> inputResourceUrls,
            string style, string tileMatrixSet)
        {
            var resourceUrls = new List<ResourceUrl>();
            foreach (var resourceUrl in inputResourceUrls)
            {
                var template = resourceUrl.template.Replace(WmtsRequest.TileMatrixSetTag, tileMatrixSet);
                template = template.Replace(WmtsRequest.StyleTag, style);
                resourceUrls.Add(new ResourceUrl
                    {
                        Format = resourceUrl.format,
                        ResourceType = resourceUrl.resourceType,
                        Template = template
                    });
            }
            return resourceUrls;
        }
        
    }
}

