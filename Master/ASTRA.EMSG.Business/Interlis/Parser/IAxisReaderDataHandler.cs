using System;
using ASTRA.EMSG.Business.Interlis.Parser.AutoGen;

namespace ASTRA.EMSG.Business.Interlis.Parser
{
	/// <summary>
	/// Description of AxisReaderDataHandler.
	/// </summary>
	public interface IAxisReaderDataHandler
	{
        void EmitXMLFragment(string xml);

		void ReceivedAxis(IImportedAchse axis);
		
		void ReceivedAxissegment(IImportedSegment axisSegment);
		
		void ReceivedSector(IImportedSektor axisSektor);

        void Finished();
	}
}
