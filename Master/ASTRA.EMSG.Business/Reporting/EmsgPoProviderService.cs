using System;
using System.Collections;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Common.Enums;
using Autofac.Features.Indexed;
using System.Linq;

namespace ASTRA.EMSG.Business.Reporting
{
    public delegate IPoProvider PoProviderResolver(Type poProviderType);

    public interface IEmsgPoProviderService
    {
        IPoProvider CreatePoProvider(ReportParameter poProviderParameter);

        IEmsgPoProviderBase CreateEmsgPoProvider<TEmsgReportParameter, TReportPo>(TEmsgReportParameter poProviderParameter, IPresentationObjectProcessor<TReportPo> presentationObjectProcessor)
            where TEmsgReportParameter : EmsgReportParameter
            where TReportPo : new();

        bool IsReportAvailableInCurrentErfassungPeriod<TEmsgReportParameter, TReportPo>()
            where TEmsgReportParameter : EmsgReportParameter
            where TReportPo : new();

        IEnumerable<NetzErfassungsmodus> GetSupportedNetzErfassungsmodus<TEmsgReportParameter>()
            where TEmsgReportParameter : EmsgReportParameter;

        bool IsReportAvailableInCurrentErfassungPeriod<TEmsgReportParameter, TReportPo>(TEmsgReportParameter emsgReportParameter)
            where TEmsgReportParameter : EmsgReportParameter
            where TReportPo : new();

        IEmsgPoProviderBase CreateEmsgPoProvider<TEmsgReportParameter, TReportPo>(TEmsgReportParameter poProviderParameter)
            where TEmsgReportParameter : EmsgReportParameter
            where TReportPo : new();
    }

    public class EmsgPoProviderService : IEmsgPoProviderService
    {
        private readonly IIndex<Type, IPoProvider> poProviderIndex;

        public EmsgPoProviderService(IIndex<Type, IPoProvider> poProviderIndex)
        {
            this.poProviderIndex = poProviderIndex;
        }

        public IPoProvider CreatePoProvider(ReportParameter poProviderParameter)
        {
            var poProvider = GetPoProvider(poProviderParameter);

            poProvider.SetBaseParameter(poProviderParameter);
            poProvider.LoadDataSources(poProviderParameter);
            return poProvider;
        }

        public IEmsgPoProviderBase CreateEmsgPoProvider<TEmsgReportParameter, TReportPo>(TEmsgReportParameter poProviderParameter)
            where TEmsgReportParameter : EmsgReportParameter
            where TReportPo : new()
        {
            return CreateEmsgPoProvider<TEmsgReportParameter, TReportPo>(poProviderParameter, null);
        }

        public IEmsgPoProviderBase CreateEmsgPoProvider<TEmsgReportParameter, TReportPo>(TEmsgReportParameter poProviderParameter, IPresentationObjectProcessor<TReportPo> presentationObjectProcessor)
            where TEmsgReportParameter : EmsgReportParameter
            where TReportPo : new()
        {
            IPoProvider poProvider = GetPoProvider(poProviderParameter);
            
            if(poProvider is IEmsgModeDependentPoProviderBase<TEmsgReportParameter, TReportPo>)
            {
                var emsgPoProvider = (IEmsgModeDependentPoProviderBase<TEmsgReportParameter, TReportPo>)poProvider;
                emsgPoProvider.SetPresentationObjectProcessor(presentationObjectProcessor);
            }

            poProvider.SetBaseParameter(poProviderParameter);
            poProvider.LoadDataSources(poProviderParameter);

            return (IEmsgPoProviderBase)poProvider;
        }

        public bool IsReportAvailableInCurrentErfassungPeriod<TEmsgReportParameter, TReportPo>()
            where TEmsgReportParameter : EmsgReportParameter
            where TReportPo : new()
        {
            return IsReportAvailableInCurrentErfassungPeriod<TEmsgReportParameter, TReportPo>(typeof(TEmsgReportParameter));
        }

        public IEnumerable<NetzErfassungsmodus> GetSupportedNetzErfassungsmodus<TEmsgReportParameter>() where TEmsgReportParameter : EmsgReportParameter 
        {
            IPoProvider poProvider;
            if (!poProviderIndex.TryGetValue(typeof(TEmsgReportParameter), out poProvider))
                throw new ArgumentException("For the Report-Parameter " + typeof(TEmsgReportParameter).FullName + " was no PoProvider registered.");
            ReportInfoAttribute[] attributes =  ((ReportInfoAttribute[]) poProvider.GetType().GetCustomAttributes(typeof (ReportInfoAttribute), false))
                .Where(a => a.NetzErfassungsmodus != ((NetzErfassungsmodus) (-1))).ToArray();
            if (attributes.Length == 0)
                return Enum.GetValues(typeof (NetzErfassungsmodus)).Cast<NetzErfassungsmodus>().ToArray();
            return  attributes.Select(a => a.NetzErfassungsmodus).ToArray();
        }

        public bool IsReportAvailableInCurrentErfassungPeriod<TEmsgReportParameter, TReportPo>(TEmsgReportParameter emsgReportParameter)
            where TEmsgReportParameter : EmsgReportParameter
            where TReportPo : new()
        {
            return IsReportAvailableInCurrentErfassungPeriod<TEmsgReportParameter, TReportPo>(typeof(TEmsgReportParameter));
        }

        private bool IsReportAvailableInCurrentErfassungPeriod<TEmsgReportParameter, TReportPo>(Type type)
            where TEmsgReportParameter : EmsgReportParameter
            where TReportPo : new()
        {
            IPoProvider poProvider;
            if (!poProviderIndex.TryGetValue(type, out poProvider))
                throw new ArgumentException("For the Report-Parameter " + type.FullName + " was no PoProvider registered.");

            return ((IEmsgModeDependentPoProviderBase<TEmsgReportParameter, TReportPo>) poProvider).AvailableInCurrentErfassungPeriod;
        }

        private IPoProvider GetPoProvider(ReportParameter poProviderParameter)
        {
            IPoProvider poProvider;
            if (!poProviderIndex.TryGetValue(poProviderParameter.GetType(), out poProvider))
                throw new ArgumentException("For the Report-Parameter " + poProviderParameter.GetType().FullName + " was no PoProvider registered.");
            return poProvider;
        }
    }
}