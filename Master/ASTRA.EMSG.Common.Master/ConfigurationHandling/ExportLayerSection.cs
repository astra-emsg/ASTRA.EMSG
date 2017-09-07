using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.EMSGBruTile;

namespace ASTRA.EMSG.Common.Master.ConfigurationHandling
{
    // Define a custom section that contains a custom 
    // UrlsCollection collection of custom UrlConfigElement elements. 
    // This class shows how to use the ConfigurationCollectionAttribute. 
    public class ExportLayerSection : ConfigurationSection
    {
        // Declare the Urls collection property using the 
        // ConfigurationCollectionAttribute. 
        // This allows to build a nested section that contains 
        // a collection of elements.
        [ConfigurationProperty("layers", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(LayerCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public LayerCollection Layers
        {
            get
            {
                LayerCollection layerCollection =
                    (LayerCollection)base["layers"];
                return layerCollection;
            }
        }

    }

    // Define the custom UrlsCollection that contains the  
    // custom UrlsConfigElement elements. 
    public class LayerCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new LayerConfigElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((LayerConfigElement)element).Name;
        }

        public LayerConfigElement this[int index]
        {
            get
            {
                return (LayerConfigElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public LayerConfigElement this[string Name]
        {
            get
            {
                return (LayerConfigElement)BaseGet(Name);
            }
        }

        public int IndexOf(LayerConfigElement layer)
        {
            return BaseIndexOf(layer);
        }

        public void Add(LayerConfigElement layer)
        {
            BaseAdd(layer);
        }
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(LayerConfigElement layer)
        {
            if (BaseIndexOf(layer) >= 0)
                BaseRemove(layer.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }       
    }

    // Define the custom UrlsConfigElement elements that are contained  
    // by the custom UrlsCollection. 
    // Notice that you can change the default values to create new default elements. 
    public class LayerConfigElement : ConfigurationElement, ILayerConfig
    {
        
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }
        [ConfigurationProperty("container", IsRequired = true)]
        public LayerContainerEnum LayerContainer 
        {
            get
            {
                return  (LayerContainerEnum)this["container"];
            }
            set
            {

                this["container"] = value;
            }
        }

        [ConfigurationProperty("isactive", DefaultValue = false,
            IsRequired = false)]
        public bool IsDefaultVisible 
        {
            get
            {
                return (bool)this["isactive"];
            }
            set
            {
                this["isactive"] = value;
            }
        }

        [ConfigurationProperty("order", IsRequired = true)]
        public int Order 
        {
            get
            {
                return (int)this["order"];
            }
            set
            {
                this["order"] = value;
            }
        }

        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get
            {
                return (string)this["url"];
            }
            set
            {
                this["url"] = value;
            }
        }
        [ConfigurationProperty("wmslayer", IsRequired = true)]
        public string WMSLayer
        {
            get
            {
                return (string)this["wmslayer"];
            }
            set
            {
                this["wmslayer"] = value;
            }
        }

        [ConfigurationProperty("matrixset", IsRequired = false, DefaultValue = null)]
        public string MatrixSet
        {
            get
            {
                return (string)this["matrixset"];
            }
            set
            {
                this["matrixset"] = value;
            }
        }

        [ConfigurationProperty("style", IsRequired = false, DefaultValue = null)]
        public string Style
        {
            get
            {
                return (string)this["style"];
            }
            set
            {
                this["style"] = value;
            }
        }

        [ConfigurationProperty("username", IsRequired = false, DefaultValue = null)]
        public string Username
        {
            get
            {
                return (string)this["username"];
            }
            set
            {
                this["username"] = value;
            }
        }

        [ConfigurationProperty("password", IsRequired = false, DefaultValue = null)]
        public string Password
        {
            get
            {
                return (string)this["password"];
            }
            set
            {
                this["password"] = value;
            }
        }
         [ConfigurationProperty("dimensions", IsRequired = false)]
        public KvpCollection Dimensions
        {
            get
            {
                return (KvpCollection)this["dimensions"];
            }
            set
            {
                this["dimensions"] = value;
            }
        }
         [ConfigurationProperty("localization", IsRequired = false)]
         public KvpCollection Localization
         {
             get
             {
                 return (KvpCollection)this["localization"];
             }
             set
             {
                 this["localization"] = value;
             }
         }
        [ConfigurationProperty("format", IsRequired = false)]
         public string Format
         {
             get
             {
                 return (string)this["format"];
             }
             set
             {
                 this["format"] = value;
             }
         }
        [ConfigurationProperty("servicetype", IsRequired = true)]
        public ServiceType ServiceType
        {
            get
            {
                return (ServiceType)this["servicetype"];
            }
            set
            {
                this["servicetype"] = value;
            }
        }
        public Dictionary<string, string> LocalizationDict
        {
            get
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                if (this.Localization != null)
                {
                    foreach (var item in this.Localization)
                    {
                        KvpConfigElement element = (KvpConfigElement)item;
                        dict.Add(element.Key, element.Value);
                    }
                }
                return dict;
            }
        }
         public Dictionary<string, string> DimensionsDict
         {
             get 
             {
                 Dictionary<string, string> dict = new Dictionary<string, string>();
                 if (this.Dimensions != null)
                 {
                     foreach (var dimension in this.Dimensions)
                     {
                         KvpConfigElement element = (KvpConfigElement)dimension;
                         dict.Add(element.Key, element.Value);
                     }
                 }
                 return dict;
             }
         }
    }

    public class KvpCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new KvpConfigElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((KvpConfigElement)element).Key;
        }

        public KvpConfigElement this[int index]
        {
            get
            {
                return (KvpConfigElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public LayerConfigElement this[string Name]
        {
            get
            {
                return (LayerConfigElement)BaseGet(Name);
            }
        }

        public int IndexOf(KvpConfigElement dimension)
        {
            return BaseIndexOf(dimension);
        }

        public void Add(KvpConfigElement dimension)
        {
            BaseAdd(dimension);
        }
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(KvpConfigElement dimension)
        {
            if (BaseIndexOf(dimension) >= 0)
                BaseRemove(dimension.Key);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }
    }
    public class KvpConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("key",
            IsRequired = true, IsKey = true)]
        public string Key
        {
            get
            {
                return (string)this["key"];
            }
            set
            {
                this["key"] = value;
            }
        }

        [ConfigurationProperty("value",
           IsRequired = true)]
        public string Value
        {
            get
            {
                return (string)this["value"];
            }
            set
            {
                this["value"] = value;
            }
        }

    }
}
