using System;
using System.IO;
using System.Xml.Serialization;

using ASTRA.EMSG.Business.Interlis.Parser.AutoGen;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

namespace ASTRA.EMSG.Business.Interlis.Parser
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.interlis.ch/INTERLIS2.3")]
    [System.Xml.Serialization.XmlRootAttribute("MyRoot", Namespace = "http://www.interlis.ch/INTERLIS2.3", IsNullable = false)]
    public partial class MyRoot
    {
        private object item;

        [System.Xml.Serialization.XmlElementAttribute("Axis.Axis.Axis", typeof(AxisAxisAxisAxisAxis) )]
        [System.Xml.Serialization.XmlElementAttribute("Axis.Axis.AxisSegment", typeof(AxisAxisAxisAxisAxisSegment))]
        [System.Xml.Serialization.XmlElementAttribute("Axis.Axis.Sector", typeof(AxisAxisAxisAxisSector))]
        public object Item
        {
            get { return item; }
            set { item = value; }
        }
    }

    /// <summary>
    /// Description of AxisReader.
    /// </summary>
    public class AxisReader2
    {
        private string filename;
        private IAxisReaderDataHandler dataHandler;
        private object data;
        private HashSet<Guid> deletesToIgnore = new HashSet<Guid>();

        public AxisReader2(String filename, IAxisReaderDataHandler dataHandler)
        {
            this.filename = filename;
            this.dataHandler = dataHandler;
        }

        private void Load()
        {

            using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                CleanInsertDeletes(fileStream);

                using (XmlReader reader = XmlReader.Create(fileStream))
                {
                    reader.MoveToContent();
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (reader.Name == "Axis.Axis.Axis"
                                    || reader.Name == "Axis.Axis.AxisSegment"
                                    || reader.Name == "Axis.Axis.Sector")
                                {
                                    XElement el = XElement.ReadFrom(reader)
                                                          as XElement;
                                    if (el != null)
                                    {
                                        MyRoot myRoot = parseMyRoot(el);
                                        EmitItem(myRoot.Item);

                                    }

                                }
                                break;
                        }
                    }
                    reader.Close();
                }
                fileStream.Close();
            }
            dataHandler.Finished();
        }
        
        public void Parse()
        {
            Load();
        }

        private MyRoot parseMyRoot(XElement el)
        {
            StringWriter strW = new StringWriter();
            XmlReader xmlReader = el.CreateReader();

            string xml = el.ToString();

            dataHandler.EmitXMLFragment(xml);

            XmlSerializer ser = new XmlSerializer(typeof(MyRoot));

            string xmlHead = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

            StringReader sr = new StringReader(xmlHead + " <MyRoot xmlns=\"http://www.interlis.ch/INTERLIS2.3\"> " + xml + " </MyRoot>");
            return (MyRoot)ser.Deserialize(sr);
        }

        public void CleanInsertDeletes(FileStream fileStream)
        {
            HashSet<Guid> insertset = new HashSet<Guid>();
            HashSet<Guid> deleteset = new HashSet<Guid>();
            using (XmlReader reader = XmlReader.Create(fileStream))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name == "Axis.Axis.Axis"
                                || reader.Name == "Axis.Axis.AxisSegment"
                                || reader.Name == "Axis.Axis.Sector")
                            {
                                XElement el = XElement.ReadFrom(reader)
                                                          as XElement;
                                if (el != null)
                                {
                                    MyRoot myRoot = parseMyRoot(el);
                                    object item = myRoot.Item;
                                    if (item is AxisAxisAxisAxisAxis)
                                    {
                                        AxisAxisAxisAxisAxis axis = (AxisAxisAxisAxisAxis)item;
                                        if (axis.OPERATIONSpecified)
                                        {
                                            switch (axis.OPERATION)
                                            {
                                                case OperationType.INSERT:
                                                    insertset.Add(AchsenImportUtils.GuidFromImportString(axis.TID));
                                                    break;
                                                case OperationType.DELETE:
                                                    deleteset.Add(AchsenImportUtils.GuidFromImportString(axis.TID));
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            insertset.Add(AchsenImportUtils.GuidFromImportString(axis.TID));
                                        }
                                    }
                                    else if (item is AxisAxisAxisAxisAxisSegment)
                                    {
                                        AxisAxisAxisAxisAxisSegment axisSegment = (AxisAxisAxisAxisAxisSegment)item;
                                        if (axisSegment.OPERATIONSpecified)
                                        {
                                            switch (axisSegment.OPERATION)
                                            {
                                                case OperationType.INSERT:
                                                    insertset.Add(AchsenImportUtils.GuidFromImportString(axisSegment.TID));
                                                    break;
                                                case OperationType.DELETE:
                                                    deleteset.Add(AchsenImportUtils.GuidFromImportString(axisSegment.TID));
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            insertset.Add(AchsenImportUtils.GuidFromImportString(axisSegment.TID));
                                        }
                                    }
                                    else if (item is AxisAxisAxisAxisSector)
                                    {
                                        AxisAxisAxisAxisSector axisSector = (AxisAxisAxisAxisSector)item;

                                        if (axisSector.OPERATIONSpecified)
                                        {
                                            switch (axisSector.OPERATION)
                                            {
                                                case OperationType.INSERT:
                                                    insertset.Add(AchsenImportUtils.GuidFromImportString(axisSector.TID));
                                                    break;
                                                case OperationType.DELETE:
                                                    deleteset.Add(AchsenImportUtils.GuidFromImportString(axisSector.TID));
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            insertset.Add(AchsenImportUtils.GuidFromImportString(axisSector.TID));
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            foreach (Guid deleteId in deleteset)
            {
                if(insertset.Contains(deleteId)){
                    this.deletesToIgnore.Add(deleteId);
                }
            }
            fileStream.Seek(0, SeekOrigin.Begin);
        }
        public void EmitItem(object item)
        {
            if (item is AxisAxisAxisAxisAxis)
            {
                AxisAxisAxisAxisAxis axis = (AxisAxisAxisAxisAxis)item;
                if (deletesToIgnore.Contains(AchsenImportUtils.GuidFromImportString(axis.TID)))
                {
                    if (!axis.OPERATIONSpecified)
                    {
                        axis.OPERATIONSpecified = true;
                        axis.OPERATION = OperationType.UPDATE;
                    }
                    else 
                    {
                        switch (axis.OPERATION)
                        {
                            case OperationType.INSERT:
                                axis.OPERATION = OperationType.UPDATE;
                                break;
                            case OperationType.DELETE:
                                return;
                        }
                    }
                }
                dataHandler.ReceivedAxis(new AchseWrapper(axis));
            }
            else if (item is AxisAxisAxisAxisAxisSegment)
            {
                AxisAxisAxisAxisAxisSegment axisSegment = (AxisAxisAxisAxisAxisSegment)item;
                if (deletesToIgnore.Contains(AchsenImportUtils.GuidFromImportString(axisSegment.TID)))
                {
                    if (!axisSegment.OPERATIONSpecified)
                    {
                        axisSegment.OPERATIONSpecified = true;
                        axisSegment.OPERATION = OperationType.UPDATE;
                    }
                    else
                    {
                        switch (axisSegment.OPERATION)
                        {
                            case OperationType.INSERT:
                                axisSegment.OPERATION = OperationType.UPDATE;
                                break;
                            case OperationType.DELETE:
                                return;
                        }
                    }
                }
                dataHandler.ReceivedAxissegment(new AchsenSegmentWrapper(axisSegment));
            }
            else if (item is AxisAxisAxisAxisSector)
            {
                AxisAxisAxisAxisSector axisSector = (AxisAxisAxisAxisSector)item;
                if (deletesToIgnore.Contains(AchsenImportUtils.GuidFromImportString(axisSector.TID)))
                {
                    if (!axisSector.OPERATIONSpecified)
                    {
                        axisSector.OPERATIONSpecified = true;
                        axisSector.OPERATION = OperationType.UPDATE;
                    }
                    else
                    {
                        switch (axisSector.OPERATION)
                        {
                            case OperationType.INSERT:
                                axisSector.OPERATION = OperationType.UPDATE;
                                break;
                            case OperationType.DELETE:
                                return;
                        }
                    }
                }
                dataHandler.ReceivedSector(new AchsenSektorWrapper(axisSector));
            }
        }

        public static bool CheckFileIsValid(string file)
        {
            bool ret = false;
            try
            {
                using (FileStream fileStream = new FileStream(file, FileMode.Open))
                {

                    using (System.Xml.XmlReader reader = System.Xml.XmlReader.Create(fileStream))
                    {
                        reader.MoveToContent();
                        if (reader.Read())
                            ret = true;
                    }
                }
            }
            catch
            {
                // do nothing
            }
            return ret;
        }
    }
}
