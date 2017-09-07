using System;
using System.Collections.Generic;

namespace ASTRA.EMSG.Business.Interlis.Parser
{
	/// <summary>
    /// Description of AchsenCollectedData.
	/// </summary>
	public class AchsenCollectedData : IAxisReaderDataHandler
	{
		
		public Dictionary<Guid, IImportedAchse> achsenDict = new Dictionary<Guid, IImportedAchse>();
		
		public Dictionary<Guid, IList<IImportedSegment>> achsenToSegmentDict = new Dictionary<Guid, IList<IImportedSegment>>();
		
		public Dictionary<Guid, IImportedSegment> achsenSegmentDict = new Dictionary<Guid, IImportedSegment>();
		
		public Dictionary<Guid, IList<IImportedSektor>> achsenSegmentToSektorDict = new Dictionary<Guid, IList<IImportedSektor>>();

        public Dictionary<Guid, IImportedSektor> achsenSektorDict = new Dictionary<Guid, IImportedSektor>();

        public SortedSet<String> achsenOwners = new SortedSet<String>();
		
		public AchsenCollectedData()
		{
		}

        public void EmitXMLFragment(string xml) {}

		public void ReceivedAxis(IImportedAchse axis)
		{
			achsenDict.Add(axis.Id, axis);

            achsenOwners.Add(axis.Owner);
		}
		
		public void ReceivedAxissegment(IImportedSegment axisSegment)
		{
			
			var d = axisSegment.Shape;
			
			if (!achsenToSegmentDict.ContainsKey(axisSegment.AchsenId))
			{
				achsenToSegmentDict[axisSegment.AchsenId] = new List<IImportedSegment>(2);
			}
			
			achsenToSegmentDict[axisSegment.AchsenId].Add(axisSegment);

            achsenSegmentDict.Add(axisSegment.Id, axisSegment);

			//Console.WriteLine("segment" + axisSegment.AchsenId + " -> shape: " + axisSegment.Shape);
		}
		
		public void ReceivedSector(IImportedSektor axisSektor)
		{
			if (!achsenSegmentToSektorDict.ContainsKey(axisSektor.SegmentId))
			{
				achsenSegmentToSektorDict[axisSektor.SegmentId] = new List<IImportedSektor>(2);
			}
			
			achsenSegmentToSektorDict[axisSektor.SegmentId].Add(axisSektor);

            achsenSektorDict.Add(axisSektor.Id, axisSektor);
		}

        public string Statistics()
        {
            string stats = "";
            stats += "#achsen=" + achsenDict.Count + "\n";
            stats += "#achsensegmente=" + achsenSegmentDict.Count + "\n";

            int cntGeoms = 0;
            int cntVertices = 0;
            foreach (IImportedSegment segment in achsenSegmentDict.Values)
            {
                if (segment.Shape != null)
                {
                    cntVertices += segment.Shape.Coordinates.Length;
                    cntGeoms++;
                }
            }

            stats += "#achsensegmente_mit_geom=" + cntGeoms + "\n";
            stats += "#achsensegmente_vertices=" + cntVertices + "\n";

            stats += "#achsensektoren=" + achsenSektorDict.Count + "\n";

            //foreach (String ownerId in achsenOwners)
            //{
            //    stats += ownerId + "\n";
            //}

            return stats;
        }

        public void Finished() { }
	}
}
