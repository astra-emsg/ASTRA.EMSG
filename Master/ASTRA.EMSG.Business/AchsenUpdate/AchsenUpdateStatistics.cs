using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTRA.EMSG.Business.AchsenUpdate
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

        public int Sum()
        {
            return Inserts + Updates + Deletes;
        }
    }


    public class AchsenUpdateStatistics
    {
        public Num NumAchsen = new Num();
        public Num NumSegment = new Num();
        public Num NumSector = new Num();

        public static AchsenUpdateStatistics operator +(AchsenUpdateStatistics s1, AchsenUpdateStatistics s2)
        {
            AchsenUpdateStatistics result = new AchsenUpdateStatistics();
            result.NumAchsen = s1.NumAchsen + s2.NumAchsen;
            result.NumSegment = s1.NumSegment + s2.NumSegment;
            result.NumSector = s1.NumSector + s2.NumSector;
            return result;
        }


        public int AxisCount()
        {
            return NumAchsen.Sum();
        }

        public int SegmentCount()
        {
            return NumSegment.Sum();
        }

        public int SectorCount()
        {
            return NumSector.Sum();
        }

        public int Sum()
        {
            return NumAchsen.Sum() + NumSegment.Sum() + NumSector.Sum();
        }

        public override string ToString()
        {
            return 
                  " #Axis: " + NumAchsen.Sum()
                + " #Segments: " + NumSegment.Sum()
                + " #Sectors: " + NumSector.Sum()
                + " #Total: " + Sum() ;
        }

    }
}
