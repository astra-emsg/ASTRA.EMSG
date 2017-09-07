using System;
using ASTRA.EMSG.Business.Interlis.Parser.AutoGen;
using NetTopologySuite.Geometries.MGeometries;
using NetTopologySuite.Geometries;
using ASTRA.EMSG.Business.Interlis.AxisImport;

namespace ASTRA.EMSG.Business.Interlis.Parser
{
	/// <summary>
	/// Description of AchsenImportUtils.
	/// </summary>
	public class AchsenImportUtils
	{
		public AchsenImportUtils()
		{
		}
		
		public static Guid GuidFromImportString(string s)
		{
			return new Guid(s.Replace("GUID:", ""));
		}

        public static int OperationTypeToInt(OperationType type)
        {
            switch (type)
            {
                case OperationType.INSERT: return AxisImportOperation.INSERT;
                case OperationType.UPDATE: return AxisImportOperation.UPDATE;
                case OperationType.DELETE: return AxisImportOperation.DELETE;
            }
            return -1;
        }
	}
}
