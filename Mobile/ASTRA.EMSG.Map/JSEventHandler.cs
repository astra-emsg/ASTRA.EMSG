using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTRA.EMSG.Map
{
    [AttributeUsageAttribute(AttributeTargets.Event)]
    public class JSEventHandler : Attribute
    {
        public String MethodName { get; private set; }
        public JSEventHandler(String methodName)
        {
            this.MethodName = methodName;
        }
    }
}
