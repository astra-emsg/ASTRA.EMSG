using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Infrastructure.Reporting;

namespace ASTRA.EMSG.Business.Reporting
{
    public abstract class EmsgFilterablePoProviderBase<TReportParameter, TReportPo> : EmsgHeaderFooterPoProviderBase<TReportParameter, TReportPo>
        where TReportParameter : EmsgReportParameter
        where TReportPo : new()
    {
        public override void LoadDataSources(TReportParameter parameter)
        {
            SetFilter(parameter);
        }

        private void SetFilter(TReportParameter parameter)
        {
            var filterListBuilder = new FilterListBuilder<TReportParameter>(LocalizationService);
            BuildFilterList(filterListBuilder);
            FilterList = filterListBuilder.GenerateFilterListPos(parameter);
        }

        protected const string FilterListSubReportName = "FilterListSubReport";
        protected const string FilterListSubReportDefinitionResourceName = "ASTRA.EMSG.Business.Reports.CommonSubReport.FilterList.rdlc";

        protected virtual void BuildFilterList(IFilterListBuilder<TReportParameter> filterListBuilder) { }

        protected void AddErfassungsPeriodFilterListItem(IFilterListBuilder<TReportParameter> builder)
        {
            builder.AddFilterListItem(p => p.ErfassungsPeriodId,
                                      p => LocalizeErfassungsPeriod(p.ErfassungsPeriodId));
        }

        protected string LocalizeErfassungsPeriod(Guid? erfassungsPeriodId)
        {
            var erfp = GetErfassungsPeriod(erfassungsPeriodId);
            return erfp.IsClosed
                       ? erfp.Erfassungsjahr.Year.ToString()
                       : LocalizationService.GetLocalizedText("Current");
        }

        protected override void AddSubReportDefinitionResourceNames(Dictionary<string, string> subReportDefinitionResourceNames)
        {
            base.AddSubReportDefinitionResourceNames(subReportDefinitionResourceNames);
            subReportDefinitionResourceNames.Add(FilterListSubReportName, FilterListSubReportDefinitionResourceName);
        }

        protected override void OnSubReportProcessing(object sender, ServerSubReportProcessingEventArgs args)
        {
            base.OnSubReportProcessing(sender, args);
            if (args.ReportPath == FilterListSubReportName)
                ReportDataSource.SetDataSources(args.DataSources, new List<ReportDataSource> { new ReportDataSource(FilterList, ReportDataSourceFactory) });
        }

        private List<FilterListPo> FilterList { get; set; }
    }
}