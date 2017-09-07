using ASTRA.EMSG.Business.Services;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.Container;
using Autofac;
using TestSessionService = ASTRA.EMSG.Tests.Common.TestServices.TestSessionService;

namespace ASTRA.EMSG.Tests.Common
{
    public class TestConatainerSetup : ServerContainerSetup
    {
        public ContainerBuilder GetTestContainerBuilder()
        {
            ContainerBuilder containerBuilder = GetContainerBuilder();
            containerBuilder.RegisterType<StubLocalizationService>().As<ILocalizationService>().SingleInstance();
            containerBuilder.RegisterType<StubUserIdentityProvider>().As<IUserIdentityProvider>().SingleInstance();
            containerBuilder.RegisterType<TestSessionService>().As<ISessionService>().SingleInstance();

            return containerBuilder;
        }
        
        public override IContainer BuildContainer()
        {
            return GetTestContainerBuilder().Build();
        }
    }
}