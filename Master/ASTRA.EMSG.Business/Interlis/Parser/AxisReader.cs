using System;
using System.IO;
using System.Xml.Serialization;

using ASTRA.EMSG.Business.Interlis.Parser.AutoGen;

namespace ASTRA.EMSG.Business.Interlis.Parser
{
	/// <summary>
	/// Description of AxisReader.
	/// </summary>
	public class AxisReader
	{
		private string filename;
		private IAxisReaderDataHandler dataHandler;
		private object data;
		
		public AxisReader(String filename, IAxisReaderDataHandler dataHandler)
		{
			this.filename = filename;
			this.dataHandler = dataHandler;
		}
		
		private object Load()
		{
			XmlSerializer ser = new XmlSerializer(typeof(Transfer));
			FileStream fileStream = new FileStream(filename, FileMode.Open);
			return ser.Deserialize(fileStream);
		}
		
		
		public void Parse()
		{
			data = Load();
			
			Transfer transfer = (Transfer)data;
			
			var i0 = transfer.DATASECTION.Items[0];
			AxisAxis axisAxis = (AxisAxis)i0;
			
			foreach (object item in axisAxis.Items)
			{
				if (item is AxisAxisAxisAxisAxis)
				{
					AxisAxisAxisAxisAxis axis = (AxisAxisAxisAxisAxis)item;
					dataHandler.ReceivedAxis(new AchseWrapper(axis));
				}
				else if (item is AxisAxisAxisAxisAxisSegment) 
				{
					AxisAxisAxisAxisAxisSegment axisSegment = (AxisAxisAxisAxisAxisSegment)item;
					dataHandler.ReceivedAxissegment(new AchsenSegmentWrapper(axisSegment));
				}
				else if (item is AxisAxisAxisAxisSector) 
				{
					AxisAxisAxisAxisSector axisSector = (AxisAxisAxisAxisSector)item;
					
					dataHandler.ReceivedSector(new AchsenSektorWrapper(axisSector));
				}
			}

            dataHandler.Finished();
		}
	}
}
