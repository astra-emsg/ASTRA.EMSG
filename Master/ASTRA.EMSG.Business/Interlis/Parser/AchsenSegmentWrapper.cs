using System;
using ASTRA.EMSG.Business.Interlis.Parser.AutoGen;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NetTopologySuite.Geometries.MGeometries;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Services.GIS;

namespace ASTRA.EMSG.Business.Interlis.Parser
{
	/// <summary>
	/// Description of AchsenSegmentWrapper.
	/// </summary>
	public class AchsenSegmentWrapper : IImportedSegment
	{
		private AxisAxisAxisAxisAxisSegment segment;
		
		public AchsenSegmentWrapper(AxisAxisAxisAxisAxisSegment segment)
		{
			this.segment = segment;
		}

		public Guid AchsenId {
			get {
				return AchsenImportUtils.GuidFromImportString(segment.rAxisContainer.REF);
			}
		}

        public MLineString Shape
        {
			get {

                if (segment.Geometry.AxisAxisAxisSegmentGeometry == null) return null;

				object[] coords = segment.Geometry.AxisAxisAxisSegmentGeometry.Geometry.POLYLINE;

                List<MCoordinate> cl = new List<MCoordinate>();
				
				AxisAxisMeasure[] linDistItems = segment.Geometry.AxisAxisAxisSegmentGeometry.LinDist;
				
				if (coords.Length != linDistItems.Length)
				{
					throw new Exception("coords.Length != linDistItems.Length");
				}
				
                int cnt = 0;
				foreach (object coord in coords)
				{
					CoordValue coordiateValue = (CoordValue)coord;
					
					if (!coordiateValue.C2Specified)
					{
						throw new Exception("!coordiateValue.C2Specified");
					}

                    double mValue = linDistItems[cnt].Measure;

					if (coordiateValue.C3Specified)
					{
						cl.Add(MCoordinate.Create3dWithMeasure(coordiateValue.C1, coordiateValue.C2, coordiateValue.C3, mValue));
					}
					else
					{
                        cl.Add(MCoordinate.Create2dWithMeasure(coordiateValue.C1, coordiateValue.C2, mValue));
					}
                    cnt++;
				}

                MLineString ml = GISService.CreateMGeometryFactory().CreateMLineString(cl.ToArray());

                return ml;
			}
		}
		
		public string Name {
			get {
				return segment.SegmentName;
			}
		}

        public int Sequence
        {
            get
            {
                return Int32.Parse(segment.Sequence);
            }
        }

		public Guid Id {
			get {
				return AchsenImportUtils.GuidFromImportString(segment.TID);
			}
		}

        public int Operation
        {
            get
            {
                if (segment.OPERATIONSpecified)
                {
                    return AchsenImportUtils.OperationTypeToInt(segment.OPERATION);
                }
                else
                {
                    // return -1;
                  return AchsenImportUtils.OperationTypeToInt(OperationType.INSERT);
                }
            }
        }
	}
}
