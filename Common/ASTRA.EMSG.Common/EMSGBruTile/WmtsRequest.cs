using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BruTile.Wmts;
using BruTile.Web;
using BruTile;
using System.Globalization;
using BruTile.Wmts.Generated;

namespace ASTRA.EMSG.Common.EMSGBruTile
{
    public class WmtsRequest : IRequest
    {
        public const string XTag = "{TileCol}";
        public const string YTag = "{TileRow}";
        public const string ZTag = "{TileMatrix}";
        public const string TileMatrixSetTag = "{TileMatrixSet}";
        public const string StyleTag = "{Style}";
        public const string FormatTag = "{Format}";
        private readonly IList<ResourceUrl> _resourceUrls;
        private readonly Dimension[] wmtsDimensions;
        private readonly Dictionary<string, string> dimensions;     
        private int _resourceUrlCounter;
        

        public WmtsRequest(IEnumerable<ResourceUrl> resourceUrls, Dimension[] wmtsDimensions, Dictionary<string, string> dimensions = null )
        {
            this.dimensions = dimensions != null ? dimensions : new Dictionary<string, string>();
            this.wmtsDimensions = wmtsDimensions;
            _resourceUrls = resourceUrls.ToList();
        }

        public Uri GetUri(TileInfo info)
        {
            if (_resourceUrlCounter >= _resourceUrls.Count()) _resourceUrlCounter = 0;
            var urlFormatter = _resourceUrls[_resourceUrlCounter];
            _resourceUrlCounter++;
            var stringBuilder = new StringBuilder(urlFormatter.Template);
            stringBuilder.Replace(XTag, info.Index.Col.ToString(CultureInfo.InvariantCulture));
            stringBuilder.Replace(YTag, info.Index.Row.ToString(CultureInfo.InvariantCulture));
            stringBuilder.Replace(ZTag, info.Index.Level);          
            //Configured values for dimensions
            foreach (KeyValuePair<string, string> kvp in dimensions)
            {
                stringBuilder.Replace("{"+kvp.Key+"}", kvp.Value);
            }
            //Default values for dimensions
            foreach (var dimension in wmtsDimensions)
            {
                stringBuilder.Replace("{" + dimension.Identifier + "}", dimension.Default);
            }
            return new Uri(stringBuilder.ToString());
        }
        
    }
}
