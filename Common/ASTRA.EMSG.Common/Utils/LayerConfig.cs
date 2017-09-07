using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Common.Utils
{
    [Serializable]
    public class LayerConfig
    {
        public LayerConfig()
        { }
        public LayerConfig(string name, int order, bool isDefaultVisible, LayerContainerEnum container)
        {
            this.Name = name;
            this.Order = order;
            this.IsDefaultVisible = isDefaultVisible;
            this.Container = container;
        }

        public string Name { get; set; }
        public int Order { get; set; }
        public bool IsDefaultVisible { get; set; }
        public LayerContainerEnum Container {get; set;}

    }
}
