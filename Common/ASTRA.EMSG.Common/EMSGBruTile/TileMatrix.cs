using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTRA.EMSG.Common.EMSGBruTile
{
    [Serializable]
    public class TileMatrix
    {
        public string Identifier { get; set; }
        public double ScaleDenominator { get; set; }
        public double Top { get; set; }
        public double Left { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
    }
}
