using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Reporting;
using Autofac;
using Autofac.Builder;

namespace ASTRA.EMSG.Web.Infrastructure.Container
{
    public static class AutofacReportingModuleExtensiont
    {
        public static IRegistrationBuilder<TPoProvider, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterPoProvider<TPoProvider, TPoProviderInterface, TParameter>(this ContainerBuilder builder)
            where TPoProvider : PoProviderBase<TParameter>, TPoProviderInterface
            where TParameter : ReportParameter
        {
            builder.RegisterType<TPoProvider>().As<TPoProviderInterface>().PropertiesAutowired().InstancePerDependency();
            return builder.RegisterType<TPoProvider>().PropertiesAutowired().Keyed<IPoProvider>(typeof(TParameter)).InstancePerDependency();
        }

        public static IRegistrationBuilder<TFilterBuilder, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterFilterBuilder<TFilterBuilder, TFilterParameter>(this ContainerBuilder builder) where TFilterParameter : IFilterParameter where TFilterBuilder : FilterBuilderBase<TFilterParameter>
        {
            return builder.RegisterType<TFilterBuilder>().PropertiesAutowired().Keyed<IFilterBuilder>(typeof(TFilterParameter)).InstancePerDependency();
        }
    }
}