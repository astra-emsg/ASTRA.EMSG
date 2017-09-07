using System;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries.MGeometries;

namespace ASTRA.EMSG.Business.Interlis.Parser
{
	/// <summary>
    /// Description of IImportedSegment.
	/// </summary>
    public interface IImportedSegment : IImportedItem
	{
		
		Guid AchsenId { get; }
        MLineString Shape { get; }
      	string Name { get; }
        int Sequence { get; }
       
	}
}
