using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Drawing;


namespace ASTRA.EMSG.Common.Utils
{
    [Serializable]
    [DataContract]
    public class LegendElement
    {
       
        [DataMember(Name = "label")]
        public string Label { get; set; }
        
        [DataMember(Name = "url")]
        public string Url { get; set; }
       
        [DataMember(Name = "imageData")]
        public string ImageData { get; set; }
       
        [DataMember(Name = "contentType")]
        public string ContentType { get; set; }
                
        public Stream ImageStream
        { get { return new MemoryStream(Convert.FromBase64String(ImageData)); } }

        public Image LegendImage
        { get { return Image.FromStream(ImageStream); } }
    }
    [Serializable]
    [DataContract]
    public class Legende
    {
        public Legende()
        {
            Legend = new List<LegendElement>();
        }
       
        [DataMember(Name = "legend")]
        public IEnumerable<LegendElement> Legend { get; set; }
    }
    [Serializable]
    [DataContract]
    public class JsonLayer
    {
        public JsonLayer()
        {
            Legend = new List<LegendElement>();
        }
       
        [DataMember(Name = "layerId")]
        
        public int LayerId { get; set; }
      
        [DataMember(Name = "layerName")]
        public string LayerName { get; set; }
      
        [DataMember(Name = "layerType")]
        public string LayerType { get; set; }
      
        [DataMember(Name = "minScale")]
        public double MinScale { get; set; }
       
        [DataMember(Name = "maxScale")]
        public double MaxScale { get; set; }
      
        [DataMember(Name = "legend")]
        public IList<LegendElement> Legend { get; set; }
    }
    [Serializable]
    [DataContract]
    public class JsonLayerContainer
    {
        public JsonLayerContainer()
        {
            Layer = new List<JsonLayer>();
        }
      
        [DataMember(Name = "layers")]
        public IList<JsonLayer> Layer { get; set; }
    }
}
