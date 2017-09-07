using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Services.GIS.WMS.WMSObjects;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;

namespace ASTRA.EMSG.Business.Services.GIS.WMS
{
    public class CreateWMSQuery
    {
        public string CreateBackgroundRequestString(WMSParameter parameter)
        {
            if (string.IsNullOrEmpty(parameter.FORMAT)) parameter.FORMAT = "image/jpeg";
            string spatialReferenceSystem = "SRS=" + parameter.SRS + "&CRS=" + parameter.CRS;

            string wmsPraefix = string.Format("?SERVICE={0}&VERSION={1}&REQUEST={2}&", parameter.SERVICE, parameter.VERSION, parameter.REQUEST);
            string wmsParams = String.Format(
              "LAYERS={0}&{1}&BBOX={2}&WIDTH={3}&HEIGHT={4}&FORMAT={5}&STYLES=default",
              parameter.LAYERS,
              spatialReferenceSystem,
              parameter.BBOX,
              parameter.WIDTH,
              parameter.HEIGHT,
              parameter.FORMAT);

            return wmsPraefix + wmsParams;
        }

        public string CreateMapRequestString(WMSRestParameter parameter, Guid mandandID)
        {
            if (string.IsNullOrEmpty(parameter.FORMAT)) parameter.FORMAT = "image/jpeg";

            string[] allLayers = (parameter.LAYERS.Replace("show:", "")).Split(',');
           
            string layerIdsFromConfig = "";
            //iteration für wms request mit mehreren layern
            foreach (string singleLayer in allLayers)
            {
                if (!string.IsNullOrEmpty(singleLayer))
                {
                    string id = new ServerConfigurationProvider().GetWMSLayerId(singleLayer);
                    if (parameter.LAYERDEFS != null && parameter.LAYERDEFS.Contains(singleLayer))
                    {
                        string[] layerIDsWithNearlySameContent = id.Split(','); //zb. Zustandsabschnitt u. zustandsabschnitt_indexIsNull
                        string tempLayerDefs = parameter.LAYERDEFS; //zwischenspeichern 
                        parameter.LAYERDEFS = "";  //zurücksetzen d. layerdefs 
                        //ACHTUNG funktioniert NICHT bei einer Abfrage mit mehreren ReportLayern wenn gleichzeitig mehrere andere RestLayer abgefragt werden!!!
                        foreach (string tempL in layerIDsWithNearlySameContent)
                            parameter.LAYERDEFS += tempLayerDefs.Replace(singleLayer, tempL) + ";";
                    }
                    //zusammenfassen aller layer die angezeigt werden sollen
                    layerIdsFromConfig += (layerIdsFromConfig != "" ? "," : "") + id;
                }
            }
            //mandantfilter for every layer
            Dictionary<string, string> cqlFilterDict = new Dictionary<string, string>();
            string[] layerIDs = layerIdsFromConfig.Split(',');//list with ids from this layer from config file

            foreach (string singleLayerId in layerIDs)
                cqlFilterDict.Add(singleLayerId, "MANDANT_ID%3D'" + mandandID.ToString()+"'");

            if (parameter.LAYERDEFS != null)
            {
                string[] defintions = parameter.LAYERDEFS.Split(';');
                foreach (string def in defintions)
                {
                    if (String.IsNullOrEmpty(def)) continue;
                    string layerId = def.Split(':')[0];
                    string filter = def.Substring(layerId.Length + 1);
                    cqlFilterDict[layerId] += " and " + filter;
                }
            }

            string[] size = parameter.SIZE.Split(',');
            string width = size[0];
            string height = size[1];
            string cqlFilter = "";
            foreach (string layerId in layerIDs)
            {
                if (!String.IsNullOrEmpty(cqlFilter))
                    cqlFilter += ";";
                cqlFilter += cqlFilterDict[layerId];
            }

            return String.Format(
              "?service=wms&version=1.3&request=GetMap&layers={1}&transparent={2}&format={0}%2F{3}&bbox={4}&width={5}&height={6}&crs={7}&format_options=dpi:{8}&CQL_FILTER={9}",
               parameter.F,
               layerIdsFromConfig,
               parameter.TRANSPARENT,
               parameter.FORMAT,
               parameter.BBOX,
               width,
               height,
               parameter.IMAGESR,
               parameter.DPI,
               cqlFilter);
        }
       
        private IEnumerable<String> Nested(string value)
        {
            if (string.IsNullOrEmpty(value))
                yield break;

            Stack<int> brackets = new Stack<int>();

            for (int i = 0; i < value.Length; ++i)
            {
                char ch = value[i];

                if (ch == '(')
                    brackets.Push(i);
                else if (ch == ')')
                {
                    if (!brackets.Any()) throw new System.Exception("No opening bracket for closing bracket found");
                    int openBracket = brackets.Pop();

                    yield return value.Substring(openBracket + 1, i - openBracket - 1);
                }
            }

            if (!brackets.Any()) throw new System.Exception("No closing bracket for opening bracket found");

            yield return value;
        }
    }
}
