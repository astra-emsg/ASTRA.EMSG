using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Common.Utils
{
    public static class EmsgLayerInfo
    {
       


        public const string AchsenSegmentLayerName = "ACHSENSEGMENT";
        public const string StrassenabschnittLayerName = "STRASSENABSCHNITTGIS";
        public const string ZustandabschnittLayerName = "ZUSTANDSABSCHNITTGIS";
        public const string ZustandabschnittNullIndexLayerName = "ZUSTANDSABSCHNITTGIS_IndexIsNull";
        public const string ZustandabschnittLayerNameReport = "ZUSTANDSABSCHNITTGIS_INDEX_AUSWERTUNG";
        public const string ZustandabschnittNullIndexLayerNameReport = "ZUSTANDSABSCHNITTGIS_INDEX_AUSWERTUNG_IndexIsNull";


        public const string StrassenabschnittAchsenreferenzLayerName = "StrassenabschnittAchsenreferenzLayer";
        public const string ZustandabschnittAchsenreferenzLayerName = "ZustandabschnittAchsenreferenzLayer";
        public const string EditPolylineLayerName = "EditPolylineLayer";
        public const string EditPointLayerName = "EditPointLayer";

        

        public static List<LayerDefinition> Layers = new List<LayerDefinition>(){
                

            new EmsgFeatureLayerDefinition(AchsenSegmentLayerName,0,false, false, false),
            new EmsgFeatureLayerDefinition(StrassenabschnittLayerName,1,false, false, true),
            new EmsgFeatureLayerDefinition(ZustandabschnittLayerName,2,true, false, true),
            
          

            new EmsgFeatureLayerDefinition(StrassenabschnittAchsenreferenzLayerName,null,true, true, false),
            new EmsgFeatureLayerDefinition(ZustandabschnittAchsenreferenzLayerName,null,true, true, false)

            
            
        };
        public static List<EmsgFeatureLayerDefinition> EMSGFeatureLayers { get { return Layers.OfType<EmsgFeatureLayerDefinition>().ToList(); } }
        public static List<string> DeletableLayerNames { get { return Layers.OfType<EmsgFeatureLayerDefinition>().Where(eld => eld.IsDeletable).Select(eld => eld.Name).ToList(); } }
        public static List<string> AchsenReferenzLayerNames { get { return Layers.OfType<EmsgFeatureLayerDefinition>().Where(eld => eld.IsAchsenReferenzLayer).Select(eld => eld.Name).ToList(); } }
    }
    public abstract class LayerDefinition
    {
        public LayerDefinition(string name, LayerContainerEnum LayerContainer, int? order, bool isDefaultVisible)
        {
            this.Name = name;
            this.LayerContainer = LayerContainer;
            this.Order = order;

            if (this.CanBeVisible)
            {
                this.IsDefaultVisible = isDefaultVisible;
            }
            else { IsDefaultVisible = false; }
            
           
            
        }
        public string Name { get; private set; }
        public int? Order { get; private set; }
        public LayerContainerEnum LayerContainer { get; private set; }
        public bool CanBeVisible { get { return Order != null; } }
        public bool IsDefaultVisible { get; private set; }      

    }
  
    public class EmsgFeatureLayerDefinition : LayerDefinition
    {
        public EmsgFeatureLayerDefinition(string name, int? order, bool isDeletable, bool isAchsenReferenzLayer, bool isDefaultVisible, string FileName=null)
            : base(name, LayerContainerEnum.Overlay, order, isDefaultVisible)
        {
            this.IsDeletable = isDeletable;
            this.IsAchsenReferenzLayer = isAchsenReferenzLayer;

        }
        public bool IsDeletable { get; private set; }

        public bool IsAchsenReferenzLayer { get; private set; }
    }
    
}
