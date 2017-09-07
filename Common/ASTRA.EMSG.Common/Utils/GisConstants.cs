using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace ASTRA.EMSG.Common.Utils
{
    public static class GisConstants
    {
        private static int srid = 21781;
        public static int SRID { get { return srid; } }
        public static string SRS { get { return "EPSG:" + srid; } }

        private static double[] exportResolutions = new double[] { 4000, 3750, 3500, 3250, 3000, 2750, 2500, 2250, 2000, 1750, 1500, 1250, 1000, 750, 650, 500, 250, 100, 50, 20, 10, 5, 2.5, 2, 1.5, 1, 0.5, 0.25, 0.1 };
        public static double[] ExportResolutions { get { return exportResolutions; } }

        //taken from http://spatialreference.org/ref/epsg/4326/ogcwkt/        
        public static string Wkt4326 { get { return "GEOGCS[\"WGS 84\",DATUM[\"WGS_1984\",SPHEROID[\"WGS 84\",6378137,298.257223563,AUTHORITY[\"EPSG\",\"7030\"]],AUTHORITY[\"EPSG\",\"6326\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.01745329251994328,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4326\"]]"; } }

        //taken from http://spatialreference.org/ref/epsg/21781/ogcwkt/
        public static string Wkt21781 { get { return "PROJCS[\"CH1903 / LV03\",GEOGCS[\"CH1903\",DATUM[\"CH1903\",SPHEROID[\"Bessel 1841\",6377397.155,299.1528128,AUTHORITY[\"EPSG\",\"7004\"]],TOWGS84[674.374,15.056,405.346,0,0,0,0],AUTHORITY[\"EPSG\",\"6149\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.01745329251994328,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4149\"]],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],PROJECTION[\"Hotine_Oblique_Mercator\"],PARAMETER[\"latitude_of_center\",46.95240555555556],PARAMETER[\"longitude_of_center\",7.439583333333333],PARAMETER[\"azimuth\",90],PARAMETER[\"rectified_grid_angle\",90],PARAMETER[\"scale_factor\",1],PARAMETER[\"false_easting\",600000],PARAMETER[\"false_northing\",200000],AUTHORITY[\"EPSG\",\"21781\"],AXIS[\"Y\",EAST],AXIS[\"X\",NORTH]]";} }

        //taken from http://spatialreference.org/ref/epsg/21781/esriwkt/
        public static string EsriWkt21781 { get { return "PROJCS[\"CH1903 / LV03\",GEOGCS[\"CH1903\",DATUM[\"D_CH1903\",SPHEROID[\"Bessel_1841\",6377397.155,299.1528128]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.017453292519943295]],PROJECTION[\"Hotine_Oblique_Mercator_Azimuth_Center\"],PARAMETER[\"latitude_of_center\",46.95240555555556],PARAMETER[\"longitude_of_center\",7.439583333333333],PARAMETER[\"azimuth\",90],PARAMETER[\"scale_factor\",1],PARAMETER[\"false_easting\",600000],PARAMETER[\"false_northing\",200000],UNIT[\"Meter\",1]]"; } }

        public static string EsriUTF8CodePage { get { return "65001";}}

        public static Envelope MaxExtent { get { return new Envelope(new Coordinate(485869.5728, 76443.1884), new Coordinate(837076.5648, 299941.7864)); } }
    }
}
