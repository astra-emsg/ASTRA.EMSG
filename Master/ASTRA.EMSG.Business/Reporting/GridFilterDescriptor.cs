using System;

namespace ASTRA.EMSG.Business.Reporting
{
    public class GridFilterDescriptor
    {
        public string Member { get; set; }
        public Type MemberType { get; set; }
        public GridFilterOperator Operator { get; set; }
        public object Value { get; set; }
    }
}