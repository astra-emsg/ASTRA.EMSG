using System;
using ASTRA.EMSG.Business.Interlis.Parser.AutoGen;

namespace ASTRA.EMSG.Business.Interlis.Parser
{
	/// <summary>
	/// Description of AchseWrapper.
	/// </summary>
	public class AchseWrapper : IImportedAchse
	{
		private AxisAxisAxisAxisAxis axis;
		
		public AchseWrapper(AxisAxisAxisAxisAxis axis)
		{
			this.axis = axis;
		}
		
		public Guid Id {
			get {
				return AchsenImportUtils.GuidFromImportString(axis.TID);
			}
		}
		
		public DateTime Version {
			get {
				return axis.Validity.AxisAxisObjectVersionInfo.VersionValidFrom;
			}
		}

        public String Name
        {
            get
            {
                return axis.AxisName;
            }
        }

        public String Owner
        {
            get
            {
                return axis.Owner;
            }
        }

        public int Operation
        {
            get
            {
                if (axis.OPERATIONSpecified)
                {
                    return AchsenImportUtils.OperationTypeToInt(axis.OPERATION);
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
