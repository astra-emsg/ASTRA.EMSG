using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ASTRA.EMSG.Common
{
    [Serializable]
    public struct XMLKeyValuePair<K, V>
    {
        public K Key
        { get; set; }

        public V Value
        { get; set; }
        public XMLKeyValuePair(K k, V v) : this() { Key = k; Value = v; }
    }
}
