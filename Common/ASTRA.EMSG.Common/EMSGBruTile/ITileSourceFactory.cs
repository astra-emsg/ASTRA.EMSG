using System;
namespace ASTRA.EMSG.Common.EMSGBruTile
{
    public interface ITileSourceFactory
    {
        string Format { get; }
        BruTile.ITileSource GetTileSource();
    }
}
