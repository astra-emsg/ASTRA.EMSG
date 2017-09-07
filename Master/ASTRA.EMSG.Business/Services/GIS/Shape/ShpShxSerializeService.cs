using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Common;
using GeoAPI.Geometries;
using System.IO;
using NetTopologySuite.IO;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Common.DataTransferObjects;
using NetTopologySuite.Features;
using System.Collections;
using ASTRA.EMSG.Business.Entities.GIS;
using NetTopologySuite.Geometries;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common.Utils;
using Ionic.Zip;
using ASTRA.EMSG.Business.Utils.Shape;

namespace ASTRA.EMSG.Business.Services.GIS.Shape
{
    public interface IShpShxSerializeService:IService
    {
        //Feature CreateAchsenSegmentFeature(AchsenSegment achsensegment);
        //Feature CreateAchsenReferenzFeature(AchsenReferenz achsenreferenz, Guid inspektionsrouteid);
        //Feature CreateZustandsabschnittFeature(ZustandsabschnittGIS zustandsabschnitt, Guid inspektionsrouteid);
        //Feature CreateStrassenabschnittFeature(InspektionsRtStrAbschnitte inspektionsroutenAbschnitt, Guid inspektionsrouteid);
        void ReadShpShxDbfDto(ShapeStreams shapeStreams);
        Stream WriteShape(IList<Feature> featureCollection, string name, DbaseFileHeader header = null);

    }
    public class ShpShxSerializeService:IShpShxSerializeService
    {
       
        public Feature CreateAchsenSegmentFeature(AchsenSegment achsensegment)
        {
            Feature feature = new Feature();
            IAttributesTable attributes = new AttributesTable();

            feature.Geometry = achsensegment.Shape;

            //ID
            attributes.AddAttribute(checkStringLengthforDBF(CheckOutGISDefaults.AchsensegmentID), achsensegment.Id.ToString());

            feature.Attributes = attributes;
            return feature;
        }
        public Feature CreateAchsenReferenzFeature(AchsenReferenz achsenreferenz, Guid inspektionsrouteid)
        {
            Feature arFeature = new Feature();
            IAttributesTable saArAttributes = new AttributesTable();

            arFeature.Geometry = achsenreferenz.Shape;

            //ID
            saArAttributes.AddAttribute(checkStringLengthforDBF(CheckOutGISDefaults.AchsenreferenzID), achsenreferenz.Id.ToString());
            //AchsenID
            saArAttributes.AddAttribute(checkStringLengthforDBF(CheckOutGISDefaults.AchsensegmentID), achsenreferenz.AchsenSegment.Id.ToString());
            //ReferenzgruppeID
            saArAttributes.AddAttribute(checkStringLengthforDBF(CheckOutGISDefaults.ReferenzgruppeID), achsenreferenz.ReferenzGruppe.Id.ToString());
            //inspektionsRouteID
            saArAttributes.AddAttribute(checkStringLengthforDBF(CheckOutGISDefaults.InspektionsRouteID), inspektionsrouteid.ToString());

            //ZustandsabschnittID if Achsenreferenz belongs to a Zustandsabschnitt else StrassenabschnittsID (belongs to Strassenabschnitt)
            if (achsenreferenz.ReferenzGruppe.ZustandsabschnittGIS != null)
            {
                saArAttributes.AddAttribute(checkStringLengthforDBF(CheckOutGISDefaults.ZustandsabschnittID), achsenreferenz.ReferenzGruppe.ZustandsabschnittGIS.Id.ToString());
                saArAttributes.AddAttribute(checkStringLengthforDBF(CheckOutGISDefaults.StrassenabschnittGISID), achsenreferenz.ReferenzGruppe.ZustandsabschnittGIS.StrassenabschnittGIS.Id.ToString());
            }
            else if (achsenreferenz.ReferenzGruppe.StrassenabschnittGIS != null)
            {
                saArAttributes.AddAttribute(checkStringLengthforDBF(CheckOutGISDefaults.StrassenabschnittGISID), achsenreferenz.ReferenzGruppe.StrassenabschnittGIS.Id.ToString());
            }


            arFeature.Attributes = saArAttributes;
            return arFeature;
        }
        private string checkStringLengthforDBF(string propertystring)
        {
            return propertystring.Length > 11 ? propertystring.Remove(11) : propertystring;
        }
        public Feature CreateZustandsabschnittFeature(ZustandsabschnittGIS zustandsabschnitt, Guid inspektionsrouteid)
        {
            Feature zaFeature = new Feature();
            IAttributesTable zaAttributes = new AttributesTable();

            zaFeature.Geometry = zustandsabschnitt.Shape;
            //ID
            zaAttributes.AddAttribute(checkStringLengthforDBF(CheckOutGISDefaults.ZustandsabschnittID), zustandsabschnitt.Id.ToString());
            //StrassenabschnittID
            zaAttributes.AddAttribute(checkStringLengthforDBF(CheckOutGISDefaults.StrassenabschnittGISID), zustandsabschnitt.StrassenabschnittGIS.Id.ToString());
            //inspektionsRouteID
            zaAttributes.AddAttribute(checkStringLengthforDBF(CheckOutGISDefaults.InspektionsRouteID), inspektionsrouteid.ToString());
            //Zustandsindex
            //zaAttributes.AddAttribute(CheckOutGISDefaults.ZustandsIndex, zustandsabschnitt.Zustandsindex.ToString().Replace('.', ','));
            if (zustandsabschnitt.Zustandsindex != null)
            {
                zaAttributes.AddAttribute(CheckOutGISDefaults.ZustandsIndex, double.Parse(zustandsabschnitt.Zustandsindex.ToString()));
            }
            else
            {
                zaAttributes.AddAttribute(CheckOutGISDefaults.ZustandsIndex, double.Parse("-1"));
            }

            zaFeature.Attributes = zaAttributes;
            return zaFeature;
        }
        public Feature CreateStrassenabschnittFeature(InspektionsRtStrAbschnitte inspektionsroutenAbschnitt, Guid inspektionsrouteid)
        {
            Feature feature = new Feature();
            IAttributesTable attributes = new AttributesTable();

            feature.Geometry = inspektionsroutenAbschnitt.StrassenabschnittGIS.Shape;

            //ID
            attributes.AddAttribute(checkStringLengthforDBF(CheckOutGISDefaults.StrassenabschnittGISID), inspektionsroutenAbschnitt.StrassenabschnittGIS.Id.ToString());
            //Reihenfolge
            attributes.AddAttribute(checkStringLengthforDBF(CheckOutGISDefaults.StrassenabschnittReihenfolge), inspektionsroutenAbschnitt.Reihenfolge);
            //Belastungskategorie
            attributes.AddAttribute(checkStringLengthforDBF(CheckOutGISDefaults.BelastungsKategorie), inspektionsroutenAbschnitt.StrassenabschnittGIS.Belastungskategorie.Typ);
            //inspektionsRouteID
            attributes.AddAttribute(checkStringLengthforDBF(CheckOutGISDefaults.InspektionsRouteID), inspektionsrouteid.ToString());

            feature.Attributes = attributes;
            return feature;
        }
        public Stream WriteShape(IList<Feature> featureCollection, string name, DbaseFileHeader header = null)
        {
            if (featureCollection.IsEmpty())
            {
                return null;
            }
            ShapeMemoryStreamDataWriter shapeWriter = new ShapeMemoryStreamDataWriter(GISService.CreateGeometryFactory());
            shapeWriter.Header = header == null ? ShapefileDataWriter.GetHeader(featureCollection.First(), featureCollection.Count) : header;
            shapeWriter.Write(featureCollection as IList);

            MemoryStream shpMemStream = shapeWriter.GetShpStream();
            MemoryStream shxMemStream = shapeWriter.GetShxStream();
            MemoryStream dbfMemStream = shapeWriter.GetDbfStream();
            MemoryStream prjMemStream = new MemoryStream(Encoding.UTF8.GetBytes(GisConstants.EsriWkt21781));
            MemoryStream cpgMemStream = new MemoryStream(Encoding.UTF8.GetBytes(GisConstants.EsriUTF8CodePage));

            ZipFile zipfile = new ZipFile();

            shpMemStream.Seek(0, 0);
            shxMemStream.Seek(0, 0);
            dbfMemStream.Seek(0, 0);
            prjMemStream.Seek(0, 0);

            zipfile.AddEntry(name + ".shp", shpMemStream);
            zipfile.AddEntry(name + ".shx", shxMemStream);
            zipfile.AddEntry(name + ".dbf", dbfMemStream);
            zipfile.AddEntry(name + ".prj", prjMemStream);
            zipfile.AddEntry(name + ".cpg", cpgMemStream);

            MemoryStream stream = new MemoryStream();
            zipfile.Save(stream);
            stream.Seek(0, 0);

            shpMemStream.Close();
            shxMemStream.Close();
            dbfMemStream.Close();
            prjMemStream.Close();
            return stream;
        }

        private bool? getBoolean(object o)
        {
            string ret = o.ToString();
            switch (ret)
            {
                case "1.0":
                    return true;
                case "0.0":
                    return false;
                case "1":
                    return true;
                case "0":
                    return false;
                case "T":
                    return true;
                case "F":
                    return false;
                default:
                    return null;
            }
        }
        public void ReadShpShxDbfDto(ShapeStreams shapeStreams)
        {
            shapeStreams.Seek(0, SeekOrigin.Begin);
            //ShapeMemoryStreamDataReader shapeDataReader = new ShapeMemoryStreamDataReader(shapeStreams.shpStream, shapeStreams.dbfStream, GISService.CreateGeometryFactory());

            //while (shapeDataReader.Read())
            //{
            //    IGeometry geometry = shapeDataReader.Geometry;
                //bool? deleted = getBoolean(shapeDataReader[CheckOutGISDefaults.IsDeleted]);
                //switch (shapeStreams.layerName)
                //{

                //    case CheckOutGISDefaults.ZustandsabschnitteLayer:
                //        Guid zustandsabschnittID = Guid.Parse(shapeDataReader[CheckOutGISDefaults.ZustandsabschnittID].ToString());

                //        if (deleted == true)
                //        {
                //            checkInGIS.AddDeletedZustandsabschnitt(zustandsabschnittID);
                //        }
                //        else
                //        {
                //            ZustandsabschnittGISDTO zustandsabschnitt = dtoContainer.DataTransferObjects.Where(dto => dto.Id == zustandsabschnittID).SingleOrDefault() as ZustandsabschnittGISDTO;
                //            zustandsabschnitt.Shape = geometry;
                //            zustandsabschnitt.ReferenzGruppe = Guid.Empty;
                //            zustandsabschnitt.ReferenzGruppeDTO = new ReferenzGruppeDTO();
                //            checkInGIS.AddZustandsabschnitt(zustandsabschnitt);
                //        }
                //        break;

                //    case CheckOutGISDefaults.ZustandsabschnitteAchsenreferenzenLayer:
                //        if (deleted != true)
                //        {
                //            //shape handles linestrings internally als multilinestrings,
                //            //achsenreferenzen should be linestrings instead of multilinestrings

                //            IGeometry convertedGeometry = (geometry is MultiLineString) ? GISService.CreateGeometryFactory().CreateLineString(geometry.Coordinates) : geometry;

                //            Guid zustandsabschnittAchsenreferenzID = Guid.Parse(shapeDataReader[CheckOutGISDefaults.AchsenreferenzID].ToString());
                //            Guid zustandsabschnittOfAchsenreferenzID = Guid.Parse(shapeDataReader[CheckOutGISDefaults.ZustandsabschnittID].ToString());
                //            Guid achsenSegmentID = Guid.Parse(shapeDataReader[CheckOutGISDefaults.AchsensegmentID].ToString());
                //            checkInGIS.AddAchsenreferenz(new AchsenReferenzDTO() { AchsenSegment = achsenSegmentID, Shape = convertedGeometry }, zustandsabschnittOfAchsenreferenzID);
                //        }
                //        break;
                //    default:
                //        break;
                //}
            //}

        }



    }
}

