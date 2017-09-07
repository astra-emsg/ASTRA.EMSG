using System;
namespace ASTRA.EMSG.Common.EMSGBruTile
{
   public interface ILayerConfig
    {
        System.Collections.Generic.Dictionary<string, string> DimensionsDict { get; }
        string Format { get; set; }
        System.Collections.Generic.Dictionary<string, string> LocalizationDict { get; }
        bool IsDefaultVisible { get; set; }
        ASTRA.EMSG.Common.Enums.LayerContainerEnum LayerContainer { get; set; }
        string MatrixSet { get; set; }
        string Name { get; set; }
        int Order { get; set; }
        string Password { get; set; }
        string Style { get; set; }
        string Url { get; set; }
        string Username { get; set; }
        string WMSLayer { get; set; }
    }
}
