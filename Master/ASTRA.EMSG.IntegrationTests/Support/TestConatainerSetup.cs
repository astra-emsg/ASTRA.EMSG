using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.Identity;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using ASTRA.EMSG.IntegrationTests.Support.TestServices;
using ASTRA.EMSG.Tests.Common;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.Container;
using ASTRA.EMSG.Web.Infrastructure.Security;
using Autofac;
using TestSessionService = ASTRA.EMSG.Tests.Common.TestServices.TestSessionService;

namespace ASTRA.EMSG.IntegrationTests.Support
{
    public class SpecFlowTestConatainerSetup : ServerContainerSetup
    {
        public ContainerBuilder GetTestContainerBuilder()
        {
            ContainerBuilder containerBuilder = GetContainerBuilder();
            containerBuilder.RegisterType<StubLocalizationService>().As<ILocalizationService>().SingleInstance();
            containerBuilder.RegisterType<TestUserIdentityProvider>().As<IUserIdentityProvider>().SingleInstance();
            containerBuilder.RegisterType<TestSessionService>().As<ISessionService>().SingleInstance();
            containerBuilder.RegisterType<TestCookieService>().As<ICookieService>().SingleInstance();
            containerBuilder.RegisterType<TestHttpRequestService>().As<IHttpRequestService>().SingleInstance();
            containerBuilder.RegisterType<IdentityServiceFake>().As<IIdentityService>().SingleInstance();
            containerBuilder.RegisterType<TestPermissionService>().As<IPermissionService>().SingleInstance();
            containerBuilder.RegisterType<SpecFlowTransactionScopeProvider>().As<ITransactionScopeProvider>().SingleInstance();
            containerBuilder.RegisterType<StubReportTemplatingService>().As<IReportTemplatingService>().SingleInstance();
            containerBuilder.RegisterType<TestConfigProvider>().As<IServerConfigurationProvider>().SingleInstance();

            return containerBuilder;
        }
        
        public override IContainer BuildContainer()
        {
            return GetTestContainerBuilder().Build();
        }
    }
}