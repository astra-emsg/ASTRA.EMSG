using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reporting
{
    public interface IEmsgModeDependentPoProviderBase<TReportParameter, TReportPo> : IEmsgPoProviderBase
        where TReportParameter : EmsgReportParameter
        where TReportPo : new()
    {
        void SetPresentationObjectProcessor(IPresentationObjectProcessor<TReportPo> processor);
        List<TReportPo> PresentationObjectList { get; }
    }

    public abstract class EmsgModeDependentPoProviderBase<TReportParameter, TReportPo> : EmsgFilterablePoProviderBase<TReportParameter, TReportPo>, IEmsgModeDependentPoProviderBase<TReportParameter, TReportPo>
        where TReportParameter : EmsgReportParameter
        where TReportPo : new()
    {
        private IPresentationObjectProcessor<TReportPo> presentationObjectProcessor;

        public void SetPresentationObjectProcessor(IPresentationObjectProcessor<TReportPo> processor)
        {
            presentationObjectProcessor = processor;
        }

        public override void LoadDataSources(TReportParameter parameter)
        {
            base.LoadDataSources(parameter);

            var presentationObjectList = GetPresentationObjectList(parameter);

            PresentationObjectList = presentationObjectList;

            if (presentationObjectProcessor != null)
                presentationObjectList = presentationObjectProcessor.ProcessPresentationObjects(presentationObjectList);

            ProcessedPresentationObjectList = presentationObjectList;

            HasData = ProcessedPresentationObjectList.Count > 0;

            SetReportParameters(parameter);
        }

        private List<TReportPo> GetPresentationObjectList(TReportParameter parameter)
        {
            if (AvailableInCurrentErfassungPeriod || parameter.ErfassungsPeriodId != null)
            {
                var erfassungsmodus = GetNetzErfassungsmodus(parameter);

                switch (erfassungsmodus)
                {
                    case NetzErfassungsmodus.Summarisch:
                        return GetPresentationObjectListForSummarisch(parameter);
                    case NetzErfassungsmodus.Tabellarisch:
                        return GetPresentationObjectListForTabellarisch(parameter);
                    case NetzErfassungsmodus.Gis:
                        return GetPresentationObjectListForGis(parameter);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return new List<TReportPo>();
        }

        protected NetzErfassungsmodus GetNetzErfassungsmodus(TReportParameter parameter)
        {
            NetzErfassungsmodus erfassungsmodus;
            if (parameter.ErfassungsPeriodId.HasValue)
            {
                erfassungsmodus = ErfassungsPeriodService.GetEntityById(parameter.ErfassungsPeriodId.Value).NetzErfassungsmodus;
            }
            else
            {
                var currentErfassungsPeriod = ErfassungsPeriodService.GetCurrentErfassungsPeriod();
                erfassungsmodus = currentErfassungsPeriod.NetzErfassungsmodus;
                parameter.ErfassungsPeriodId = parameter.ErfassungsPeriodId ?? currentErfassungsPeriod.Id;
            }

            return erfassungsmodus;
        }

        protected virtual List<TReportPo> GetPresentationObjectListForSummarisch(TReportParameter parameter) { return NotSupported(); }
        protected virtual List<TReportPo> GetPresentationObjectListForTabellarisch(TReportParameter parameter) { return NotSupported(); }
        protected virtual List<TReportPo> GetPresentationObjectListForGis(TReportParameter parameter) { return NotSupported(); }

        private List<TReportPo> ProcessedPresentationObjectList { get; set; }

        public List<TReportPo> PresentationObjectList { get; private set; }

        public override IEnumerable<ReportDataSource> DataSources
        {
            get
            {
                return new List<ReportDataSource> { new ReportDataSource(ProcessedPresentationObjectList, ReportDataSourceFactory) };
            }
        }

        protected override bool IsForClosedErfassungsPeriod(TReportParameter parameter)
        {
            return GetErfassungsPeriod(parameter.ErfassungsPeriodId).IsClosed;
        }

        protected List<TReportPo> NotSupported()
        {
            return new List<TReportPo>();
        }

        protected override PaperType PaperType { get { return PaperType.A4Portrait; } }
    }
}