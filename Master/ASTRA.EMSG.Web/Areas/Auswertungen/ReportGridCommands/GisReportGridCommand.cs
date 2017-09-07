namespace ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands
{
    public class GisReportGridCommand : ReportGridCommand
    {
        public bool HideMap { get; set; }
        public bool HideTable { get; set; }

        public string BoundingBox { get; set; }
        public string BoundingBoxFilter { get; set; }
        public string MapSize { get; set; }
        public string BackgroundLayers { get; set; }
        public string Layers { get; set; }
        public string LayersAV { get; set; }
        public string LayersAVBackground { get; set; }
        public string LayerDefs { get; set; }
        public string ScaleWidth { get; set; }
        public string ScaleText { get; set; }
        public bool isPreview { get; set; }
    }
}