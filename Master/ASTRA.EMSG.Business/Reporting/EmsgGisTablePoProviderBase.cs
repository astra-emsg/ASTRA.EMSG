using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Services.Common;

namespace ASTRA.EMSG.Business.Reporting
{
    public abstract class EmsgGisTablePoProviderBase<TReportParameter, TReportPo, TGisEntity> : EmsgTablePoProviderBase<TReportParameter, TReportPo>
        where TReportParameter : EmsgGisReportParameter
        where TReportPo : new()
        where TGisEntity : Entity
    {
        public IReportLocalizationService ReportLocalizationService { get; set; }

        private bool showMapFootertext;

        protected override void SetReportParameters(TReportParameter parameter)
        {
            var netzErfassungsmodus = GetNetzErfassungsmodus(parameter);
            //showMapFootertext = !parameter.HideMap && netzErfassungsmodus == NetzErfassungsmodus.Gis;

            base.SetReportParameters(parameter);

            //if (netzErfassungsmodus == NetzErfassungsmodus.Gis)
            //{
            //    MapImageInfo mapImageInfo = boundingBoxFiltererBase.GetMapImageInfo(parameter);

            //    AddReportParameter("MapBackgroundImage", mapImageInfo.BackgroundImageUrl);
            //    AddReportParameter("MapImage", mapImageInfo.MapImageUrl);
            //}
            //else
            //{
                AddReportParameter("MapBackgroundImage", "http://");
                AddReportParameter("MapImage", "http://");
            //}

            AddReportParameter("HideMap", true);
            AddReportParameter("HideTable", false);

            //if (parameter.HideTable)
            //    ReportFileNamePostfix = "Karte";
        }

        protected override string FooterText { get { return showMapFootertext ? ReportLocalizationService.MapReportFooterText : string.Empty; } }

        protected override PaperType PaperType { get { return PaperType.A3Landscape; } }
    }
}