using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Services.EntityServices.Common;

namespace ASTRA.EMSG.Business.Reporting
{
    public abstract class EmsgBenchmarkauswertungPoProviderBase<TReportParameter, TReportPo> : EmsgFilterablePoProviderBase<TReportParameter, TReportPo>
        where TReportParameter : BenchmarkauswertungParameter
        where TReportPo : new()
    {
        public IKenngroessenFruehererJahreService KenngroessenFruehererJahreService { get; set; }

        public override IEnumerable<ReportDataSource> DataSources { get { return new List<ReportDataSource> { new ReportDataSource(new List<TReportPo>(), ReportDataSourceFactory) }; } }

        public override void LoadDataSources(TReportParameter parameter)
        {
            CalculatePos(parameter);
            HasData = poListDictionary.Any();

            SetReportParameters(parameter);

            base.LoadDataSources(parameter);
        }

        protected override bool IsForClosedErfassungsPeriod(TReportParameter parameter)
        {
            var erfassungsPeriod = ErfassungsPeriodService.GetEntityById(parameter.JahrId);
            return erfassungsPeriod != null && erfassungsPeriod.IsClosed;
        }

        protected readonly Dictionary<string, List<TReportPo>> poListDictionary = new Dictionary<string, List<TReportPo>>();

        protected abstract void CalculatePos(TReportParameter parameter);

        protected override void SetReportParameters(TReportParameter parameter)
        {
            base.SetReportParameters(parameter);
            hideGruppeColumn = !parameter.BenchmarkingGruppenTypList.Any();
            AddReportParameter("HideGruppeColumn", hideGruppeColumn);
            AddReportParameter("TableHeaderBackgroundColor", "#DCE6F1");
            AddReportParameter("TableAlternatingRowBackgroungColor", "#EBF5F5");
            AddReportParameter("TableHorizontalBorderColor", "#587BA5");
            AddReportParameter("TableVerticalBorderColor", "#C2D3E8");
        }

        private bool hideGruppeColumn;

        public override string ReportDefinitionResourceName
        {
            get
            {
                string standardReportDefinitionResourceName = base.ReportDefinitionResourceName;

                if(hideGruppeColumn)
                    return standardReportDefinitionResourceName.Substring(0, standardReportDefinitionResourceName.Length - 5) + "OhneGruppe.rdlc";

                return standardReportDefinitionResourceName;
            }
        }

        protected override void BuildFilterList(IFilterListBuilder<TReportParameter> filterListBuilder)
        {
            base.BuildFilterList(filterListBuilder);
            filterListBuilder.AddFilterListItem(p => p.ErfassungsPeriodId,
                                      p => GetJahrDateTime(p.JahrId).Year.ToString());
            filterListBuilder.AddFilterListItem(p => p.BenchmarkingGruppenTypList, p => string.Join(", ", p.BenchmarkingGruppenTypList.Select(g => LocalizationService.GetLocalizedEnum(g))));
        }

        protected override void OnSubReportProcessing(object sender, ServerSubReportProcessingEventArgs args)
        {
            base.OnSubReportProcessing(sender, args);
            if (poListDictionary.ContainsKey(args.ReportPath))
                ReportDataSource.SetDataSources(args.DataSources, new List<ReportDataSource> { new ReportDataSource(poListDictionary[args.ReportPath], ReportDataSourceFactory) });
        }

        protected DateTime GetJahrDateTime(Guid jahrId)
        {
            var kenngroessenFruehererJahre = KenngroessenFruehererJahreService.GetEntityById(jahrId);
            if (kenngroessenFruehererJahre != null)
                return new DateTime(kenngroessenFruehererJahre.Jahr, 01, 01);

            return ErfassungsPeriodService.GetEntityById(jahrId).Erfassungsjahr;
        }

        protected override PaperType PaperType { get { return PaperType.A4Landscape; } }
    }
}