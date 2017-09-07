namespace ASTRA.EMSG.Business.Reporting
{
    public abstract class EmsgTablePoProviderBase<TReportParameter, TReportPo> : EmsgModeDependentPoProviderBase<TReportParameter, TReportPo>
        where TReportParameter : EmsgReportParameter
        where TReportPo : new()
    {
        protected override void SetReportParameters(TReportParameter parameter)
        {
            base.SetReportParameters(parameter);

            AddReportParameter("TableHeaderBackgroundColor", "#DCE6F1");
            AddReportParameter("TableFooterBackgroundColor", "#F2F2F2");
            AddReportParameter("TableAlternatingRowBackgroungColor", "#EBF5F5");
            AddReportParameter("TableHorizontalBorderColor", "#587BA5");
            AddReportParameter("TableFooterHorizontalBorderColor", "#1B4B84");
            AddReportParameter("TableVerticalBorderColor", "#C2D3E8");
        }
    }
}