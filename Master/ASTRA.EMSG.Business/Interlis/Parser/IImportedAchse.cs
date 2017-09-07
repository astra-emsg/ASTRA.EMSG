using System;

namespace ASTRA.EMSG.Business.Interlis.Parser
{
	/// <summary>
    /// Description of IImportedAchse.
	/// </summary>
    public interface IImportedAchse : IImportedItem
	{
		
		DateTime Version { get; }
        String Name { get; }
        String Owner { get; }
        
	}
}
