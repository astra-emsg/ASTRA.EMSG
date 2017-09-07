using System;
using GeoAPI.Geometries;

namespace ASTRA.EMSG.Business.Interlis.Parser
{
	/// <summary>
    /// Description of IImportedSektor.
	/// </summary>
    public interface IImportedSektor : IImportedItem
	{
		
		Guid SegmentId { get; }
		double Km { get; }
		double SectorLength { get; }
		string SectorName { get; }
		double Sequence { get; }
		IPoint MarkerGeom { get; }
        
	}
}
