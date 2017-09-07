using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries.MGeometries;
using NetTopologySuite.Geometries;
using NetTopologySuite.LinearReferencing;
using NetTopologySuite.Operation.Linemerge;
using NetTopologySuite.Algorithm;
using GeoAPI;
using NetTopologySuite.Operation.Distance;
using GeoAPI.Operation.Buffer;

namespace ASTRA.EMSG.Common.Utils
{
    public class GeometryUtils
    {
        private static double THIN = 0.01;

        /**
   * minimale Distanz vom signifikanten Unterschied
   */
        public static double SIGNIFICANTDIST = 0.05;
        private static Coordinate[] CordinateArrTo2d(Coordinate[] coords)
        {
            Coordinate[] coords2d = new Coordinate[coords.Length];

            for (int i=0; i < coords.Length; i++)
            {
                coords2d[i] = new Coordinate(coords[i].X, coords[i].Y);

            }
            return coords2d;
        }


        public static ILineString ConvertMLineStringTo2D(IGeometryFactory gf, ILineString mLineString)
        {
            Coordinate[] coords2d = CordinateArrTo2d(mLineString.Coordinates);

            return gf.CreateLineString(coords2d);
        }
        /**
 * erzeugt eine Offset-Linie mit Hilfe von der buffer(offset) Funktion
 *
 * @param gf
 * @param baseline
 * @param offset ... rechts < 0, links > 0
 * @return
 * @throws VipDatabaseException
 */
        public static ILineString createOffsetLineNew(IGeometryFactory gf, ILineString baseline, double offset)
        {
            // 1.Schritt: Buffer bilden
            double offsetAbs = Math.Abs(offset);
            IGeometry buffer;
            {
                // prüfen, ob eine Berechung nötig/möglich ist
                if (offsetAbs < THIN)
                {
                    return baseline;
                }
                if (!baseline.IsSimple)
                { // sich kreuzende Linie aufteilen
                    return partialOffsetLine(gf, baseline, offset);  // aufteilen und für beiden Teilen versuchen
                }

                buffer = baseline.Buffer(offsetAbs, 12, EndCapStyle.Flat);
                if (buffer == null)
                {
                    return null;
                }
                if (buffer.IsEmpty)
                {
                    return baseline;
                }
                if (buffer is MultiPolygon // wegen JTS Bug: bufer kann manchmal MultiPolygon sein (siehe Nasenweg mit distance=5
                  || (buffer is Polygon && ((Polygon)buffer).NumInteriorRings > 0))
                { // buffer hat Loch
                    return partialOffsetLine(gf, baseline, offset);  // aufteilen und für beiden Teilen versuchen
                }
            }
            Coordinate[] baseCoords = baseline.Coordinates;

            // 2.Schritt: aus der Bufferlinie die Segmente entfernen, die zu nah zu der Basislinie sind
            IGeometry bufferLine = buffer.Boundary;
            {
                //      double minDist = Math.abs(offset) * 0.5;  // Segmente der Bufferlinie die zur baseline näher sind als minDist, werden entfernt
                List<Coordinate> bufferCoords = buffer.Coordinates.ToList();
                for (int i = 0; i < bufferCoords.Count - 1; i++)
                {
                    LineSegment segm = new LineSegment(bufferCoords.ElementAt(i), bufferCoords.ElementAt(i + 1));
                    ILineString segmLine = gf.CreateLineString(new Coordinate[] { segm.P0, segm.P1 });

                    LineSegment segmBeg = createRectangle(gf, baseCoords[0], baseCoords[1], offsetAbs); // Segment quer zur Anfang des Basislinie-Segments
                    LineSegment segmEnd = createRectangle(gf, baseCoords[baseCoords.Length - 1], baseCoords[baseCoords.Length - 2], offsetAbs); // Segment quer zur Anfang des Basislinie-Segments
                    if (contains(segmBeg, segm, THIN) || contains(segmEnd, segm, THIN))
                    {
                        bufferLine = bufferLine.Difference(segmLine); // Segment liegt auf einem Quer-Segment des Basislinien-Segments
                    }

                }
            }
            // 3.Schritt: die Bufferlinie sollte aus 2 Teilen bestehen (die 2 Offsetlinien)           
            
            List<ILineString> lines = new List<ILineString>();
            {
                if (bufferLine is MultiLineString)
                {
                    lines = linesFromMultiLine(gf, (MultiLineString)bufferLine);
                }
                else if (bufferLine is LineString)
                {
                    lines.Add((LineString)bufferLine);
                }
                else
                {
                    return partialOffsetLine(gf, baseline, offset);  // aufteilen und für beiden Teilen versuchen
                }
            }

            // 4. Schritt: jene Linie ist die Offsetlinie die ein Segment hat,
            //   das auf dem Offset eines Anfang/Endsegments der Basisline ist
            ILineString line = null;
            {
                double sumDistMin = Double.MaxValue;

                foreach (ILineString linePart in lines)
                { // die beste Linie auswählen
                    double sumDist = 0;  // Summe Entfernung der Offset-Segmenten der Basislinie von der linePart
                    for (int i = 1; i < baseCoords.Length; i++)
                    {
                        LineSegment segm = new LineSegment(baseCoords[i - 1], baseCoords[i]);
                        LineSegment segmentOffset = createSegmentOffset(segm, offset);
                        if (i == 1 || i == baseCoords.Count() - 1)
                        {// Anfang- oder Endsegment
                            if (segmOnLine(segmentOffset, linePart))
                            {   // ein Segment der LInie linePart liegt auf dem segmentOffset
                                line = linePart; // linePart ist die richtige
                                sumDistMin = 0;
                                break;
                            }
                        }
                        ILineString segmOffsetLine = gf.CreateLineString(new Coordinate[] { segmentOffset.P0, segmentOffset.P1 });
                        double distance = linePart.Distance(segmOffsetLine);  // Entfernung des Segments-Endpunktes von der linePart
                        sumDist += distance * segmOffsetLine.Length;
                    }
                    if (sumDistMin > sumDist)
                    {
                        sumDistMin = sumDist;
                        line = linePart;
                    }
                }
                if (sumDistMin > baseline.Length * offsetAbs)
                { // die gefundene Linie ist wahrscheinlich doch nicht die richtige
                    line = partialOffsetLine(gf, baseline, offset);  // aufteilen und für beiden Teilen versuchen
                    if (line == null)
                    {
                        return null;
                    }
                    IGeometry lineOnBuffer = line.Intersection(buffer.Buffer(THIN));
                    {  // di OffsetlLinie muß auf dem Buffer sein
                        if (lineOnBuffer is LineString)
                            return line;
                    }
                    return null;
                }
            }

            // 5. Schritt: eventuell die Linie umdrehen
            {
                Coordinate[] coords = line.Coordinates;
                IPoint baseLineBeg = gf.CreatePoint(baseCoords[0]);
                IPoint baseLineEnd = gf.CreatePoint(baseCoords[baseCoords.Length - 1]);
                Coordinate offsetLineBeg = coords[0];
                Coordinate offsetLineEnd = coords[coords.Length - 1];
                double distBegBeg = baseLineBeg.Coordinate.Distance(offsetLineBeg);
                double distEndBeg = baseLineEnd.Coordinate.Distance(offsetLineBeg);
                double distBegEnd = baseLineBeg.Coordinate.Distance(offsetLineEnd);
                double distEndEnd = baseLineEnd.Coordinate.Distance(offsetLineEnd);

                double maxDiff = offsetAbs + THIN;
                if (distBegEnd < maxDiff && distEndBeg < maxDiff && !(distBegBeg < maxDiff && distEndEnd < maxDiff))
                {
                    line = reverse(line);
                }
                else if (distBegBeg < maxDiff && distEndEnd < maxDiff && !(distBegEnd < maxDiff && distEndBeg < maxDiff))
                {
                    // line = line;
                }
                else
                {
                    // Ergebnis ist nicht befriedigend
                    // Buffer zu groß zu den Liniensegmenten
                    return partialOffsetLine(gf, baseline, offset);  // aufteilen und für beiden Teilen versuchen
                }
            }

            return line;
        }
        /**
          * prüft, ob <code>segm0</code> das Segment <code>segm1</code> enthält
          * @param segm0
          * @param segm1
          * @param dist
          * @return
          */
        public static bool contains(LineSegment segm0, LineSegment segm1, double dist)
        {
            if (segm0.Distance(segm1.P0) < dist && segm0.Distance(segm1.P1) < dist)
            {
                return true;
            }
            return false;
        }

        /**
    * die Linie auf 2 Teile zerlegen und für beiden Teilen rekursiv die Offsetlinie erzeugen
    * @param gf
    * @param baseline
    * @param offset
    * @return
    */
        public static ILineString partialOffsetLine(IGeometryFactory gf, ILineString baseline, double offset)
        {
            Coordinate[] coords = baseline.Coordinates;
            if (coords.Length < 4)
            {  // die Linie hat zu wenig Koordinaten, hat keinen Sinn mehr zu verteilen
                return null; // Ende der Bemühungen
            }
            List<Coordinate> coordList = coords.ToList();
            int halfIndx = coords.Length / 2;
            Coordinate midPoint = new LineSegment(coords[halfIndx - 1], coords[halfIndx]).MidPoint;   // bei dem neuen Vertex wird die Linie getrennt
            List<Coordinate> coordList0 = new List<Coordinate>(coordList.Take(halfIndx));
            coordList0.Add(midPoint);
            ILineString baseLine0 = gf.CreateLineString(coordList0.ToArray());  // erste Hälfte
            List<Coordinate> coordList1 = new List<Coordinate>(coordList.Skip(halfIndx));
            coordList1.Insert(0, midPoint);
            ILineString baseLine1 = gf.CreateLineString(coordList1.ToArray());  // zweite Hälfte
            ILineString shiftLine0 = createOffsetLineNew(gf, baseLine0, offset);
            ILineString shiftLine1 = createOffsetLineNew(gf, baseLine1, offset);
            if (shiftLine0 == null && shiftLine1 == null)
            {
                return null; // Ende der Bemühungen
            }

            IGeometry buffer = baseline.Buffer(Math.Abs(offset), 12, EndCapStyle.Flat);
            //    Geometry buffer = baseline.buffer(Math.abs(offset), 0, BufferOp.CAP_BUTT);
            IGeometry bufferBoundary = buffer.Boundary.Buffer(THIN);
            List<Coordinate> shiftCoordList = new List<Coordinate>();
            if (shiftLine0 == null)
            {
                shiftCoordList.AddRange(shiftLine1.Intersection(bufferBoundary).Coordinates);
            }
            else
                if (shiftLine1 == null)
                {
                    shiftCoordList.AddRange(shiftLine0.Intersection(bufferBoundary).Coordinates);
                }
                else
                {
                    if (shiftLine0.GetPointN(shiftLine0.NumPoints - 1).Distance(shiftLine1.GetPointN(0)) < 2 * THIN)
                    {  // Ende der ersten Linie ist bei Anfang der zweiten Linie
                        shiftCoordList.AddRange(shiftLine0.Coordinates);
                        shiftCoordList.AddRange(shiftLine1.Coordinates);
                        shiftCoordList.RemoveAt(shiftLine0.NumPoints);  // doppelter Punkt nur einmal
                    }
                    else
                    {  // Ende erster Linie ist nicht Anfang zweiter Linie
                        shiftCoordList = concat(shiftLine0, shiftLine1);
                        //          List<Coordinate> list0 = Arrays.asList(shiftLine0.intersection(bufferBoundary).getCoordinates());
                        //          shiftCoordList.addAll(list0);
                        //          List<Coordinate> list1 = Arrays.asList(shiftLine1.intersection(bufferBoundary).getCoordinates());
                        //          shiftCoordList.addAll(list1);
                    }
                }
            if (shiftCoordList.Count < 2)
            {
                return null;
            }
            ILineString shiftLine = gf.CreateLineString(shiftCoordList.ToArray());
            if (shiftLine.Length < 3 * SIGNIFICANTDIST)
            {  // 3-fache Distanz, damit die reduzierte Linie auch größer wird als die Distanz
                return null;
            }

            return checkLineString(gf, shiftLine, SIGNIFICANTDIST, false);
        }
        /**
    * erzeugt einen Punkt p, wo
    * die Linie zwischen <code>anchor</code> und p
    * die Länge <code>length</code> hat und
    * quer zur Linie zwischen <code>anchor</code> und <code>coordinate</code> ist
    * @param gf
    * @param anchor
    * @param coordinate
    * @param direction wenn true: der Winkel ist 270° (rechts), sonst 90° (links)
    * @param length
    * @return
    */
        public static Coordinate createRectangle(Coordinate anchor, Coordinate coordinate, bool direction, double length)
        {
            if (coordinate.Equals2D(anchor)) return null;

            Coordinate coordShift = shift(coordinate, new Coordinate(-anchor.X, -anchor.Y));  // verschieben
            double angle = AngleUtility.Angle(coordShift) + (direction ? -1 : 1) * Math.PI / 2;  // Winkel links oder rechts um 90° drehen
            Coordinate coord = new Coordinate(length * Math.Cos(angle), length * Math.Sin(angle));
            return shift(coord, anchor);  // zurückverschieben
        }

        /**
    * erzeugt ein Querlinie mit 2 Endpunten auf der Linie zwischen <code>coordinate0</code> und <code>coordinate1</code>
    * der Mittelunkt ist <code>coordinate0</code>
    * die Entfernung der Endpunkte der Linie von <code>coordinate0</code> ist <code>offset</code>
    * @param gf
    * @param coordinate0
    * @param coordinate1
    * @param offset
    * @return
    */
        public static LineSegment createRectangle(IGeometryFactory gf, Coordinate coordinate0, Coordinate coordinate1, double offset)
        {
            Coordinate rectangle0 = createRectangle(coordinate0, coordinate1, true, offset);
            Coordinate rectangle1 = createRectangle(coordinate0, coordinate1, false, offset);
            LineSegment segment = new LineSegment(rectangle0, rectangle1);
            return segment;
        }
        /** erzeugt aus einem MultiLineString eine Collection von LineString's
    *  wo die zusammengehörende Teile zu einem LineString zusammengefasst werden
    * @param geometry
    * @return
    */
        public static List<ILineString> linesFromMultiLine(IGeometryFactory gf, MultiLineString geometry)
        {

            List<ILineString> lines = new List<ILineString>();
            for (int i = 0; i < geometry.NumGeometries; i++)
            {
                lines.Add((LineString)geometry.GetGeometryN(i));
            }

            return mergeLines(gf, lines);
        }

        /** erzeugt aus einer Collection von LineString's eine Collection von LineStrings,
         *  wo die zusammengehörende Teile zu einem LineString zusammengefasst werden
         * @param geometry
         * @return
         */
        public static List<ILineString> mergeLines(IGeometryFactory gf, List<ILineString> lines)
        {
            LineMerger lineMerger = new LineMerger();

            List<ILineString>.Enumerator iterator = lines.GetEnumerator();
            while (iterator.MoveNext())
            {
                lineMerger.Add(iterator.Current);
            }
            List<ILineString> mergedLineStrings = lineMerger.GetMergedLineStrings().Select(g => (ILineString)g).ToList();
            foreach (LineString line in mergedLineStrings)
            {
                line.SRID = gf.SRID;
            }
            return mergedLineStrings;
        }
        /**
   * Offset zu einem LineSegment erzeugen
   *
   * @param segment
   * @param offset
   * @return
   */
        public static LineSegment createSegmentOffset(
            LineSegment segment,
            double offset)
        {

            Coordinate anf = segment.P0;
            Coordinate end = segment.P1;

            double vektorX = end.X - anf.X;
            double vektorY = end.Y - anf.Y;

            if (vektorX == 0 && vektorY == 0)
            {
                // oder was soll ma machen, wenn der Vektor die Länge 0 hat?
                return null;
            }

            if (offset == 0)
            {
                // kein Offset Input ist gleich Output
                return new LineSegment(anf, end);
            }

            //normalvektor der länge offset, positiv = links
            double faktor = offset / Math.Sqrt(vektorX * vektorX + vektorY * vektorY);

            double offsetX = -vektorY * faktor;
            double offsetY = vektorX * faktor;

            return new LineSegment(
                new Coordinate(anf.X + offsetX, anf.Y + offsetY),
                new Coordinate(end.X + offsetX, end.Y + offsetY));
        }
        /**
    * liefert true, wenn ein Segment der Linie <code>line</code>
    * auf dem Sement <code>segment</code> liegt
    * @param segment
    * @param line
    * @return
    */
        public static bool segmOnLine(LineSegment segment, ILineString line)
        {
            for (int i = 1; i < line.NumPoints; i++)
            { // alle Segments der LInie line
                double distance0 = segment.Distance(line.GetCoordinateN(i - 1));  // Entfernung des Anfangspunktes des aktuellen Segments vom segment
                double distance1 = segment.Distance(line.GetCoordinateN(i));  // Entfernung des Endpunktes des aktuellen Segments vom segment
                if (distance0 + distance1 < THIN)
                {
                    return true;
                }
            }

            return false;
        }
        public static ILineString reverse(ILineString line)
        {
            if (line == null) return null;
            ILineString result = (ILineString)((IGeometry)line).Reverse();
            result.SRID = line.SRID;
            return result;
        }
        /**
  * verbindet die zwei Linien bei den nähesten Punkten der Linien:
  * erster Teil it Anfang der Linie <code>line0</code> bis zum (ersten) nähesten Punkt zu <code>line1</code>
  * zweitert Teilverbindet die zwei nähesten Punkte
  * dritter Teil ist der Endteil von <code>line1</code> ab dem nähesten Punkt
  * @param line0
  * @param line1
  * @return
  */
        private static List<Coordinate> concat(ILineString line0, ILineString line1)
        {
            List<Coordinate> listCoords = new List<Coordinate>();
            IGeometry intersection = line0.Intersection(line1);
            LengthIndexedLine lil0 = new LengthIndexedLine(line0);
            LengthIndexedLine lil1 = new LengthIndexedLine(line1);
            Coordinate[] closestPoints = DistanceOp.NearestPoints(line0, line1);
            if (!intersection.IsEmpty)
            { // die 2 Linien kreuzen sich
                double maxPos = 0;
                for (int i = 0; i < intersection.NumPoints; i++)
                { // den letzten Schnittpunkt suchen
                    Coordinate crossPoint = intersection.Coordinates[i];
                    double pos = lil0.Project(crossPoint);
                    if (maxPos < pos)
                    {
                        maxPos = pos;
                        closestPoints[0] = crossPoint;
                        closestPoints[1] = crossPoint;
                    }
                }
            }
            for (int i = 0; i < line0.Coordinates.Length; i++)
            {
                if (lil0.Project(line0.Coordinates[i]) < lil0.Project(closestPoints[0]))
                {
                    listCoords.Add(line0.Coordinates[i]);
                }
            }
            listCoords.Add(closestPoints[0]);
            if (closestPoints[0] != closestPoints[1])
            { // die 2 Linien kreuzen sich nicht
                listCoords.Add(closestPoints[1]);
            }
            for (int i = 0; i < line1.Coordinates.Length; i++)
            {
                if (lil1.Project(line1.Coordinates[i]) > lil1.Project(closestPoints[1]))
                {
                    listCoords.Add(line1.Coordinates[i]);
                }
            }
            return listCoords;
        }
        public static ILineString checkLineString(IGeometryFactory gf, ILineString geomToCheck, double tolerance, bool strict)
        {
            //Geometry geomout = checkGeometry(gf, geomToCheck, tolerance, strict);
            //if (geomout == null || geomout is LineString) return (LineString)geomout;

            //MultiLineString mline = (MultiLineString) geomout;
            //if (mline.NumGeometries != 1)
            //  throw new Exception("LineString muss genau einen Part haben.");

            //return (LineString)mline.GetGeometryN(0);
            return geomToCheck;
        }
        /**
   * verschiebt den Punkt von <code>coordinate</code> mit dem Vektor von <code>anchor</code>
   * @param coordinate
   * @param anchor
   * @return
   */
        public static Coordinate shift(Coordinate coordinate, Coordinate anchor)
        {
            return new Coordinate(coordinate.X + anchor.X, coordinate.Y + anchor.Y);
        }

 
    }
}
