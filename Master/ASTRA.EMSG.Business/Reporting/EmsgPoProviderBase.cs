using System;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Reporting
{
    public interface IEmsgPoProviderBase : IPoProvider
    {
        bool AvailableInCurrentErfassungPeriod { get; }
    }

    public abstract class EmsgPoProviderBase<TReportParameter, TReportPo> : PoProviderBase<TReportParameter>, IEmsgPoProviderBase
        where TReportParameter : EmsgReportParameter
        where TReportPo : new()
    {
        public IReportDataSourceFactory ReportDataSourceFactory { get; set; }
        public IReportingMappingEngine ReportingMappingEngine { get; set; }
        public IErfassungsPeriodService ErfassungsPeriodService { get; set; }
        public IHistorizationService HistorizationService { get; set; }
        public IMandantLogoService MandantLogoService { get; set; }

        protected void AddReportParameter<TValue>(string name, TValue value)
        {
            ReportParameters.Add(new ServerReportParameter(name, (value == null ? string.Empty : value.ToString())));
        }

        protected void AddLazyReportParameter(string name, Func<object> valueProvider)
        {
            ReportParameters.Add(new LazyServerReportParameter(name, valueProvider));
        }

        protected TReportPo CreatePoFromEntityWithCopyingMatchingProperties(IEntity entity)
        {
            var reportPo = new TReportPo();
            return ReportingMappingEngine.Translate(entity, reportPo);
        }

        protected ErfassungsPeriod GetErfassungsPeriod(EmsgReportParameter emsgReportParameter)
        {
            return GetErfassungsPeriod(emsgReportParameter.ErfassungsPeriodId);
        }

        protected ErfassungsPeriod GetErfassungsPeriod(Guid? erfassungsPeriodId)
        {
            if (erfassungsPeriodId == null)
                return HistorizationService.GetCurrentErfassungsperiod();

            return ErfassungsPeriodService.GetEntityById(erfassungsPeriodId.Value);
        }

        protected bool IsCurrentErfassungsPeriod(EmsgReportParameter emsgReportParameter)
        {
            return IsCurrentErfassungsPeriod(emsgReportParameter.ErfassungsPeriodId);
        }

        protected bool IsCurrentErfassungsPeriod(Guid? erfassungsPeriodId)
        {
            if (erfassungsPeriodId == null)
                return true;
            return HistorizationService.GetCurrentErfassungsperiod() == ErfassungsPeriodService.GetEntityById(erfassungsPeriodId.Value);
        }

        protected virtual void SetReportParameters(TReportParameter parameter)
        {
            var logo = MandantLogoService.GetMandantLogo();
            mandantLogoModel = new Lazy<MandantLogoModel>(() => logo);

            AddReportParameter("CultureCode", LocalizationService.CurrentCultureCode);
            AddLazyReportParameter("MandantImage", () => Convert.ToBase64String(logo.Logo));

            AddReportParameter("ReportNoDecimalFormat", FormatStrings.ReportNoDecimalFormat);
            AddReportParameter("ReportShortDecimalFormat", FormatStrings.ReportShortDecimalFormat);
            AddReportParameter("ReportLongDecimalFormat", FormatStrings.ReportLongDecimalFormat);

            AddReportParameter("IsPreview", parameter.IsPreview);
            AddReportParameter("IsForClosedErfassungsPeriod", IsForClosedErfassungsPeriod(parameter));
        }

        private Lazy<MandantLogoModel> mandantLogoModel;
        protected MandantLogoModel MandantLogoModel { get { return mandantLogoModel.Value; } }

        protected abstract bool IsForClosedErfassungsPeriod(TReportParameter parameter);

        public virtual bool AvailableInCurrentErfassungPeriod { get { return true; } }
    }
}