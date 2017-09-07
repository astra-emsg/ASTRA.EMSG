using System;
using System.Collections.Generic;

namespace ASTRA.EMSG.Web.Infrastructure.Help
{
    [Serializable]
    public class HelpFileMapping
    {
        public HelpFileMapping()
        {
            HelpMappingItems = new List<HelpMappingItem>();
        }

        public List<HelpMappingItem> HelpMappingItems { get; set; }

        public List<FilePath> HelpContents { get; set; }
    }
}