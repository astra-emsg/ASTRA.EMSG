using System;
using System.Collections.Generic;
using System.IO;
using ASTRA.EMSG.Common.EMSGBruTile;

namespace ASTRA.EMSG.Business.Models.GIS
{
    public class ShapeStreams
    {
        public ShapeStreams(string layerName)
        {
            this.layerName = layerName;

            shpStream = new MemoryStream();
            shxStream = new MemoryStream();
            dbfStream = new MemoryStream();
        }

        public string layerName { get; set; }
        public MemoryStream shpStream { get; set; }
        public MemoryStream shxStream { get; set; }
        public MemoryStream dbfStream { get; set; }

        public void Close()
        {
            shpStream.Close();
            shxStream.Close();
            dbfStream.Close();
        }
        public void Seek(long offset, SeekOrigin loc)
        {
            shpStream.Seek(offset, loc);
            shxStream.Seek(offset, loc);
            dbfStream.Seek(offset, loc);
        }
    }
    public class TiffStream
    {
        public TiffStream(string layerName, string fullFilePath)
        {
            this.layerName = layerName;
            //this.tiffStream = tiffStream;
            this.FullFilePath = fullFilePath;
            TiffFileStream = new FileStream(fullFilePath, FileMode.Open);
        }
        public FileStream TiffFileStream { get; private set; }
        public string FullFilePath { get; private set; }
        public string layerName;
        //public MemoryStream tiffStream { get; set; }
        public void Close()
        {
            TiffFileStream.Close();
            File.Delete(FullFilePath);
        }
        public void Seek(long offset, SeekOrigin loc)
        {
            TiffFileStream.Seek(offset, loc);
        }

    }
    public class CheckOutGISStreams
    {
        public CheckOutGISStreams()
        {
            Bezeichnung = String.Empty;
            Tiles = new List<TiledLayerInfo>();
            this.LegendStreams = new Dictionary<string, Stream>();
        }
        public Stream ModelsToExport { get; set; }
        public string Bezeichnung;      
        public Dictionary<string, Stream> LegendStreams { get; set; }
        public List<TiledLayerInfo> Tiles { get; set; }
    }
}
