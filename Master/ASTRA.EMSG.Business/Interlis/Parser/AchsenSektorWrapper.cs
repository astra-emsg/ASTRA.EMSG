using System;
using ASTRA.EMSG.Business.Interlis.Parser.AutoGen;
using NetTopologySuite.Geometries;
using ASTRA.EMSG.Business.Services.GIS;
using GeoAPI.Geometries;

namespace ASTRA.EMSG.Business.Interlis.Parser
{
	/// <summary>
	/// Description of AchsenSektorWrapper.
	/// </summary>
	public class AchsenSektorWrapper : IImportedSektor
	{
		
		private AxisAxisAxisAxisSector axisSector;
		
		public AchsenSektorWrapper(AxisAxisAxisAxisSector axisSector)
		{
			this.axisSector = axisSector;
		}
		
		public Guid Id {
			get {
				return AchsenImportUtils.GuidFromImportString(axisSector.TID);
			}
		}
		
		public double Km {
			get {
				return axisSector.Km;
			}
		}
		
		public double SectorLength {
			get {
				return axisSector.SectorLength;
			}
		}
		
		public string SectorName {
			get {
				return axisSector.SectorName;
			}
		}
		
		public double Sequence {
			get {
				return axisSector.Sequence;
			}
		}
		
		public GeoAPI.Geometries.IPoint MarkerGeom {
			get {

                // always 3d?

                var coord = axisSector.MarkerGeometry.COORD;

                if (coord.C3Specified)
                {
                    Coordinate c = new Coordinate(coord.C1, coord.C2, coord.C3);
                    return GISService.CreateMGeometryFactory().CreatePoint(c);
                }
                else
                {
                    Coordinate c = new Coordinate(coord.C1, coord.C2, 0);
                    return GISService.CreateMGeometryFactory().CreatePoint(c);
                }
			}
		}

        public int Operation
        {
            get
            {
                if (axisSector.OPERATIONSpecified)
                {
                    return AchsenImportUtils.OperationTypeToInt(axisSector.OPERATION);
                }
                else
                {
                    //return -1;
                  return AchsenImportUtils.OperationTypeToInt(OperationType.INSERT);
                }
            }
        }

		public Guid SegmentId {
			get {
				return AchsenImportUtils.GuidFromImportString(axisSector.rAxisSegmentLR.REF);
			}
		}
	}
}
