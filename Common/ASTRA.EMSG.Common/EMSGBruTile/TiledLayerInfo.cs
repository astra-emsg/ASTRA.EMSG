using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;
using System.Xml.Serialization;
using System.Web.Script.Serialization;
using System.Globalization;

namespace ASTRA.EMSG.Common.EMSGBruTile
{
    [Serializable]
    public class TiledLayerInfo
    {
        public TiledLayerInfo()
        {
            this.MatrixSet = new List<TileMatrix>();
            this.Localization = new List<XMLKeyValuePair<string, string>>();
        }
        public string Name { get; set; }
        public int Order { get; set; }
        public bool IsDefaultVisible { get; set; }
        public LayerContainerEnum Container { get; set; }

        public string BasePath { get; set; }
        public string RelativePath { get; set; }
        [XmlIgnore()]
        [ScriptIgnore]
        public string[] AbsoluteFilePaths { get; set; }
        public string SourceUrl { get; set; }
        public List<TileMatrix> MatrixSet { get; set; }
        public List<XMLKeyValuePair<string, string>> Localization {get; set;}
        public string LocalizedName
        {
            get
            {
                XMLKeyValuePair<string, string> kvp = Localization.SingleOrDefault(k => k.Key == CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
                if (kvp.Key!= null)
                {
                    return kvp.Value;
                }
                else
                {
                    return this.Name;
                }
            }
        }
    }
}
