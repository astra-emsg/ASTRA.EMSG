using System;
using System.Collections.Generic;
using System.IO;

namespace ASTRA.EMSG.Business.Models.GIS
{
    public class CheckInGISStreams
    {        
        public string Bezeichnung;
        private IList<ShapeStreams> checkInGISStreams;

        public CheckInGISStreams()
        {
            Bezeichnung = String.Empty;
            checkInGISStreams = new List<ShapeStreams>();
        }

        public void Add(ShapeStreams checkInStream)
        {
            checkInGISStreams.Add(checkInStream);
        }

        public IList<ShapeStreams> GetShapeStreams()
        {
            return checkInGISStreams;
        }

        public void Close()
        {
            foreach (ShapeStreams shapeStream in checkInGISStreams)
            {
                shapeStream.Close();
            }
        }

        public void Reset()
        {
            foreach (ShapeStreams shapestream in checkInGISStreams)
            {
                shapestream.shpStream.Seek(0, SeekOrigin.Begin);
                shapestream.shxStream.Seek(0, SeekOrigin.Begin);
                shapestream.dbfStream.Seek(0, SeekOrigin.Begin);
            }
        }
    }
}
