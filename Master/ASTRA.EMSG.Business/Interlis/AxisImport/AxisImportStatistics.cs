using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTRA.EMSG.Business.Interlis.AxisImport
{
    public class Num
    {
        public int Inserts = 0;
        public int Updates = 0;
        public int Deletes = 0;

        public static Num operator +(Num n1, Num n2)
        {
            Num result = new Num();
            result.Inserts = n1.Inserts + n2.Inserts;
            result.Updates = n1.Updates + n2.Updates;
            result.Deletes = n1.Deletes + n2.Deletes;
            return result;
        }

        public int Count()
        {
            return Inserts + Updates + Deletes;
        }
    }


    public class AxisImportStatistics
    {
        public Num NumAxis = new Num();
        public Num NumSegment = new Num();
        public Num NumSector = new Num();

        public static AxisImportStatistics operator +(AxisImportStatistics s1, AxisImportStatistics s2)
        {
            AxisImportStatistics result = new AxisImportStatistics();
            result.NumAxis = s1.NumAxis + s2.NumAxis;
            result.NumSegment = s1.NumSegment + s2.NumSegment;
            result.NumSector = s1.NumSector + s2.NumSector;
            return result;
        }


        public int AxisCount()
        {
            return NumAxis.Count();
        }

        public int SegmentCount()
        {
            return NumSegment.Count();
        }

        public int SectorCount()
        {
            return NumSector.Count();
        }

        public int Sum()
        {
            return AxisCount() + SegmentCount() + SectorCount();
        }

        public override string ToString()
        {
            return
                  " #Axis: " + AxisCount()
                + " #Segments: " + SegmentCount()
                + " #Sectors: " + SectorCount()
                + " #Total: " + Sum() ;
        }

    }
}
