using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common.Master;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using System.Collections;
using ASTRA.EMSG.Business.Models;
using NetTopologySuite.Geometries.MGeometries;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Common.Utils;
using NetTopologySuite.Precision;
using ASTRA.EMSG.Common.Master.Logging;

namespace ASTRA.EMSG.Business.Services.GIS
{
    public interface IGISService : IService
    {
        T GetNearestGeometry<T>(IGeometry filterGeom, IList<T> geometryList);
        bool CheckOverlapp<T>(IEnumerable<T> alte_achsenreferenzen, IGeometry neue_achsenreferenz)
            where T : IShapeHolder;
        bool CheckGeometriesIsInControlGeometry<T>(IEnumerable<T> achsenReferenzFromZustandsabschnitt, IGeometry strassenabschnittShape)
            where T : IShapeHolder;
        bool CheckOverlapp(IEnumerable<IGeometry> alte_achsenreferenzen, IGeometry neue_achsenreferenz);
        bool CheckGeometriesIsInControlGeometry(IEnumerable<IGeometry> achsenReferenzFromZustandsabschnitt, IGeometry strassenabschnittShape);
    }

    public class GISService : IGISService
    {



        public static IGeometryFactory CreateGeometryFactory() { return new GeometryFactory(new PrecisionModel(), GisConstants.SRID); }

        public static MGeometryFactory CreateMGeometryFactory() { return new MGeometryFactory(new PrecisionModel(), GisConstants.SRID); }

        public static bool ValidateGeometrySRID(IGeometry geom)
        {
            return geom.SRID == GisConstants.SRID ? true : false;
        }

        public static IGeometry GetGeometryFromBoundingBox(string bbox)
        {
            string[] coordArray = bbox.Split(',');
            double x1 = double.Parse(coordArray[0], System.Globalization.NumberFormatInfo.InvariantInfo);
            double y1 = double.Parse(coordArray[1], System.Globalization.NumberFormatInfo.InvariantInfo);
            double x2 = double.Parse(coordArray[2], System.Globalization.NumberFormatInfo.InvariantInfo);
            double y2 = double.Parse(coordArray[3], System.Globalization.NumberFormatInfo.InvariantInfo);
            IGeometry geometry = new GeometryFactory().ToGeometry(new Envelope(new Coordinate(x1, y1), new Coordinate(x2, y2)));
            geometry.SRID = GisConstants.SRID;
            return geometry;
        }

        public T GetNearestGeometry<T>(IGeometry filterGeom, IList<T> list)
        {
            T nearestAchsensegment = default(T);
            double nearestDistance = double.MaxValue;

            foreach (var currentAchsensegment in list)
            {
                IGeometry currentShape = currentAchsensegment
                    .GetType()
                    .GetProperty(ExpressionHelper.GetPropertyName<StrassenabschnittGIS, IGeometry>(e => e.Shape))
                    .GetValue(currentAchsensegment, null) as IGeometry;

                double currentDistance = filterGeom.Distance(currentShape);
                if (currentDistance < nearestDistance)
                {
                    nearestDistance = currentDistance;
                    nearestAchsensegment = currentAchsensegment;
                }
            }
            return nearestAchsensegment;
        }
        public bool CheckOverlapp<T>(IEnumerable<T> alte_achsenreferenzen, IGeometry neue_achsenreferenz)
           where T : IShapeHolder
        {
            return this.CheckOverlapp(alte_achsenreferenzen.Select(a => a.Shape), neue_achsenreferenz);
        }
        public bool CheckOverlapp(IEnumerable<IGeometry> alte_achsenreferenzen, IGeometry neue_achsenreferenz)
        {
            GeometryPrecisionReducer precReduc = new GeometryPrecisionReducer(new PrecisionModel(1000));
            neue_achsenreferenz = precReduc.Reduce(neue_achsenreferenz);
            foreach (var alte_achsenreferenz in alte_achsenreferenzen)
            {

                IGeometry alteGeometry = alte_achsenreferenz;
                alteGeometry = precReduc.Reduce(alteGeometry);

                if (neue_achsenreferenz.Coordinates.Length == 2)
                {
                    double distancefirst = alteGeometry.Distance(neue_achsenreferenz.Factory.CreatePoint(neue_achsenreferenz.Coordinates.First()));
                    double distancelast = alteGeometry.Distance(neue_achsenreferenz.Factory.CreatePoint(neue_achsenreferenz.Coordinates.Last()));
                    if (distancefirst < 0.001 && distancelast < 0.001)
                    {
                        return false;
                    }
                }


                if (alteGeometry.Coordinates.Length == 2)
                {
                    double distancefirst = neue_achsenreferenz.Distance(alteGeometry.Factory.CreatePoint(alteGeometry.Coordinates.First()));
                    double distancelast = neue_achsenreferenz.Distance(alteGeometry.Factory.CreatePoint(alteGeometry.Coordinates.Last()));
                    if (distancefirst < 0.001 && distancelast < 0.001)
                    {
                        return false;
                    }
                }


                //Is the new Achsenreferenz-Geometry intersecting with existing achsenreferenzen?
                if (neue_achsenreferenz.Intersects(alteGeometry))
                {
                    //check if the intersection is just the snapping point of the two geometries
                    IGeometry intersection = neue_achsenreferenz.Intersection(alteGeometry);
                    bool intersectionisfirstorlastpoint_neu = intersection.Coordinates[0].Equals3D(neue_achsenreferenz.Coordinates.First());
                    intersectionisfirstorlastpoint_neu |= intersection.Coordinates[0].Equals3D(neue_achsenreferenz.Coordinates.Last());

                    bool intersectionisfirstorlastpoint_alt = intersection.Coordinates[0].Equals3D(alteGeometry.Coordinates.First());
                    intersectionisfirstorlastpoint_alt |= intersection.Coordinates[0].Equals3D(alteGeometry.Coordinates.Last());

                    

                    if (!(intersection.GetType() == typeof(Point) && (intersectionisfirstorlastpoint_neu || intersectionisfirstorlastpoint_alt)))
                    {
                        return false;
                    }
                }
                else
                {
                    //check if lines with nonsimilar points overlap
                    NetTopologySuite.Operation.Buffer.BufferParameters bufferPara = new NetTopologySuite.Operation.Buffer.BufferParameters(0, GeoAPI.Operation.Buffer.EndCapStyle.Flat);
                    IGeometry alt_bufferedstrabs = alteGeometry.Buffer(0.001d, bufferPara);

                    IGeometry intersect = alt_bufferedstrabs.Intersection(neue_achsenreferenz);

                    if (intersect.Length > 0.001)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public bool CheckGeometriesIsInControlGeometry<T>(IEnumerable<T> achsenreferenzenToCheck, IGeometry controlGeometry)
            where T : IShapeHolder
        {
            return this.CheckGeometriesIsInControlGeometry(achsenreferenzenToCheck.Select(a => a.Shape), controlGeometry);
        }
        public bool CheckGeometriesIsInControlGeometry(IEnumerable<IGeometry> achsenreferenzenToCheck, IGeometry controlGeometry)
        {
            if (achsenreferenzenToCheck.Any(a => a == null) || controlGeometry == null)
            {
                Loggers.TechLogger.Warn("AchsenReferenz or Controlgeometry with null value");
                return false;
            }
            //perform check
            GeometryPrecisionReducer precReduc = new GeometryPrecisionReducer(new PrecisionModel(1000));
            precReduc.ChangePrecisionModel = true;
            NetTopologySuite.Operation.Buffer.BufferParameters bufferPara = new NetTopologySuite.Operation.Buffer.BufferParameters(32, GeoAPI.Operation.Buffer.EndCapStyle.Round);
            controlGeometry = precReduc.Reduce((IGeometry)controlGeometry.Clone());
            IGeometry bufferedcontrolgeometry = controlGeometry.Buffer(1.5, bufferPara);
            foreach (var achsref in achsenreferenzenToCheck)
            {
                IGeometry ar = achsref;
                ar = precReduc.Reduce((IGeometry)ar.Clone());
                IGeometry bufferedar = ar.Buffer(1.498, bufferPara);
                IGeometry diff = bufferedar.Difference(bufferedcontrolgeometry);
                if (!diff.IsEmpty)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
