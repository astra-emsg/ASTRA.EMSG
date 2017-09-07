using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Services.Common;

namespace ASTRA.EMSG.Business.Reporting
{
    public abstract class EmsgGisMapPoProviderBase<TReportParameter, TReportMapParameter, TReportPo> : EmsgFilterablePoProviderBase<TReportMapParameter, TReportPo>
        where TReportParameter : EmsgGisReportParameter
        where TReportMapParameter : TReportParameter
        where TReportPo : new()
    {
        private readonly IMapInfoProviderBase<TReportMapParameter> mapProviderBase;

        public IReportLocalizationService ReportLocalizationService { get; set; }

        protected EmsgGisMapPoProviderBase(IMapInfoProviderBase<TReportMapParameter> mapProviderBase)
        {
            this.mapProviderBase = mapProviderBase;
        }

        protected override bool IsForClosedErfassungsPeriod(TReportMapParameter parameter)
        {
            return GetErfassungsPeriod(parameter.ErfassungsPeriodId).IsClosed;
        }

        public override IEnumerable<ReportDataSource> DataSources
        {
            get
            {
                return new List<ReportDataSource> { new ReportDataSource(new List<TReportPo>(), ReportDataSourceFactory) };
            }
        }

        public override void LoadDataSources(TReportMapParameter parameter)
        {
            base.LoadDataSources(parameter);
            HasData = true;
            SetReportParameters(parameter);
        }

        protected override void SetReportParameters(TReportMapParameter parameter)
        {
            base.SetReportParameters(parameter);
            parameter.ReportImagePath = mapProviderBase.GetMapImageInfo(parameter);
            AddReportParameter("MapImage", "file:" + parameter.ReportImagePath);
        }

        protected override string FooterText { get { return ReportLocalizationService.MapReportFooterText; } }

        protected override PaperType PaperType { get { return PaperType.A3Landscape; } }
    }
}