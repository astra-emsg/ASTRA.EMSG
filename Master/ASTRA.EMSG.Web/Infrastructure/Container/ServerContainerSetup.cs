using System;
using System.Collections.Generic;
using System.Reflection;
using ASTRA.EMSG.Business.Infrastructure.Caching;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Business.Infrastructure.MappingRules;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports;
using ASTRA.EMSG.Business.Reports.AusgefuellteErfassungsformulareFuerOberflaechenschaeden;
using ASTRA.EMSG.Business.Reports.BenchmarkauswertungInventarkennwerten;
using ASTRA.EMSG.Business.Reports.BenchmarkauswertungKennwertenRealisiertenMassnahmen;
using ASTRA.EMSG.Business.Reports.BenchmarkauswertungZustandskennwerten;
using ASTRA.EMSG.Business.Reports.EineListeVonKoordiniertenMassnahmen;
using ASTRA.EMSG.Business.Reports.EineListeVonMassnahmenGegliedertNachTeilsystemen;
using ASTRA.EMSG.Business.Reports.EineListeVonRealisiertenMassnahmenGeordnetNachJahren;
using ASTRA.EMSG.Business.Reports.ErfassungsformulareFuerOberflaechenschaeden;
using ASTRA.EMSG.Business.Reports.GISExport;
using ASTRA.EMSG.Business.Reports.ListeDerInspektionsrouten;
using ASTRA.EMSG.Business.Reports.MassnahmenvorschlagProZustandsabschnitt;
using ASTRA.EMSG.Business.Reports.MengeProBelastungskategorie;
using ASTRA.EMSG.Business.Reports.MengeProBelastungskategorieGrafische;
using ASTRA.EMSG.Business.Reports.NochNichtInspizierteStrassenabschnitte;
using ASTRA.EMSG.Business.Reports.RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafische;
using ASTRA.EMSG.Business.Reports.StrassenabschnitteListe;
using ASTRA.EMSG.Business.Reports.StrassenabschnitteListeOhneInspektionsroute;
using ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustGrafische;
using ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustProBelastungskategorie;
using ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustProJahrGrafische;
using ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustProStrassenabschnitt;
using ASTRA.EMSG.Business.Reports.ZustandProZustandsabschnitt;
using ASTRA.EMSG.Business.Reports.ZustandsspiegelProBelastungskategorieGrafische;
using ASTRA.EMSG.Business.Reports.ZustandsspiegelProJahrGrafische;
using ASTRA.EMSG.Business.Services.Benchmarking;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.FilterBuilders;
using ASTRA.EMSG.Business.Services.GIS;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Business.Validators.Strassennamen;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using ASTRA.EMSG.Common.Services.SchadenMetadaten;
using ASTRA.EMSG.Master;
using ASTRA.EMSG.Web.Areas.Common.ControllerServices;
using ASTRA.EMSG.Web.Infrastructure.Security;
using Autofac;
using Autofac.Integration.Mvc;
using System.Linq;
using ASTRA.EMSG.Business.Services.Identity;
using FluentValidation;

namespace ASTRA.EMSG.Web.Infrastructure.Container
{
    public class ServerContainerSetup
    {
        public virtual IContainer BuildContainer()
        {
            return GetContainerBuilder().Build();
        }

        protected ContainerBuilder GetContainerBuilder()
        {
            ContainerBuilder builder = new ContainerBuilder();

            ////Controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            
            ////Services
            RegisterAllDerivedTypeAsSingleton<IService>(builder);

            ////Singleton DependencyPackages
            RegisterAllDerivedTypeAsSingleton<ISingletonDependencyPackage>(builder);

            ////Validators
            builder.RegisterType<AutofacValidatorFactory>().As<IValidatorFactory>().SingleInstance();
            foreach (var item in AssemblyScanner.FindValidatorsInAssembly(typeof(StrassenabschnittModelValidator).Assembly))
                builder.RegisterType(item.ValidatorType).Keyed<IValidator>(item.InterfaceType);

            ////Transactioning
            builder.RegisterType<TransactionScopeProvider>().As<ITransactionScopeProvider>().SingleInstance();
            builder.RegisterType<TransactionScopeFactory>().As<ITransactionScopeFactory>().SingleInstance();

            ////Localization
            builder.RegisterType<LocalizationService>().As<ILocalizationService>().SingleInstance();
            builder.RegisterType<ReportLocalizationService>().As<IReportLocalizationService>().SingleInstance();

            ////Reporting
            builder.RegisterType<ServerReportDataSourceFactory>().As<IReportDataSourceFactory>().SingleInstance();
            builder.RegisterType<ServerReportGenerator>().As<IServerReportGenerator>().SingleInstance();
            builder.RegisterType<EmsgPoProviderService>().As<IEmsgPoProviderService>().SingleInstance();
            builder.RegisterType<ReportControllerService>().As<IReportControllerService>().SingleInstance();
            RegisterPoProviders(builder);

            //Filtering
            builder.RegisterType<FiltererFactory>().As<IFiltererFactory>().SingleInstance();
            RegisterFilterBuilders(builder);

            ////Others
            builder.RegisterType<LogHandlerService>().As<ILogHandlerService>().SingleInstance();
            builder.RegisterType<ServerConfigurationProvider>().As<IServerConfigurationProvider>().SingleInstance();
            builder.RegisterType<SchadenMetadatenService>().As<ISchadenMetadatenService>().SingleInstance();

            ////Session, HttpRequest, Cookie, Cache Services
            builder.RegisterType<HttpRequestService>().As<IHttpRequestService>().SingleInstance();
            builder.RegisterType<HttpRequestCacheService>().As<ICacheService>().SingleInstance();
            builder.RegisterType<HttpRequestCacheService>().As<IHttpRequestCacheService>().SingleInstance();

            ////Reflection mapper
            builder.RegisterType<EntityServiceMappingConfiguration>().As<IEntityServiceMappingConfiguration>().SingleInstance();
            builder.RegisterType<EntityServiceMappingEngine>().As<IEntityServiceMappingEngine>().SingleInstance();
            builder.RegisterType<ReportingMappingConfiguration>().As<IReportingMappingConfiguration>().SingleInstance();
            builder.RegisterType<ReportingMappingEngine>().As<IReportingMappingEngine>().SingleInstance();
            builder.RegisterType<DataTransferObjectServiceMappingConfiguration>().As<IDataTransferObjectServiceMappingConfiguration>().SingleInstance();
            builder.RegisterType<DataTransferObjectServiceMappingEngine>().As<IDataTransferObjectServiceMappingEngine>().SingleInstance();
            builder.RegisterType<GISKopieMappingConfiguration>().As<IAchsKopieMappingConfiguration>().SingleInstance();
            builder.RegisterType<GISKopieMappingEngine>().As<IAchsKopieMappingEngine>().SingleInstance();

            builder.RegisterType<EntityToIdMappingRule>().As<IEntityToIdMappingRule>().SingleInstance();
            builder.RegisterType<IdToEntityMappingRule>().As<IIdToEntityMappingRule>().SingleInstance();
            builder.RegisterType<IgnoreReadonlyPropertiesMappingRule>().As<IIgnoreReadonlyPropertiesMappingRule>().SingleInstance();
            builder.RegisterType<IgnoreIdPropertyMappingRule>().As<IIgnoreIdPropertyMappingRule>().SingleInstance();
            builder.RegisterType<NullableToNonNullableMappingRule>().As<INullableToNonNullableMappingRule>().SingleInstance();
            builder.RegisterType<IgnoreIdMappingRule>().As<IIgnoreIdMappingRule>().SingleInstance();

            ////Handling the App mode (Test or Live)
            RegisterEnvironmentDependentTypes(builder);
            
            return builder;
        }

        private static void RegisterFilterBuilders(ContainerBuilder builder)
        {
            builder.RegisterFilterBuilder<StrasennameFilterBuilder, IStrassennameFilter>();
            builder.RegisterFilterBuilder<InspektionsroutenameFilterBuilder, IInspektionsroutenameFilter>();
            builder.RegisterFilterBuilder<InspektionsrouteInInspektionBeiFilterBuilder, IInspektionsrouteInInspektionBeiFilter>();
            builder.RegisterFilterBuilder<InspektionsrouteInInspektionBisVonBisFilterBuilder, IInspektionsrouteInInspektionBisVonBisFilter>();
            builder.RegisterFilterBuilder<EigentuemerFilterBuilder, IEigentuemerFilter>();
            builder.RegisterFilterBuilder<AufnahmedatumVonBisFilterBuilder, IAufnahmedatumVonBisFilter>();
            builder.RegisterFilterBuilder<ZustandsindexVonBisFilterBuilder, IZustandsindexVonBisFilter>();
            builder.RegisterFilterBuilder<CurrentMandantFilterBuilder, ICurrentMandantFilter>();
            builder.RegisterFilterBuilder<ErfassungsPeriodFilterBuilder, IErfassungsPeriodFilter>();
            builder.RegisterFilterBuilder<BelastungskategorieFilterBuilder, IBelastungskategorieFilter>();
            builder.RegisterFilterBuilder<DringlichkeitFilterBuilder, IDringlichkeitFilter>();
            builder.RegisterFilterBuilder<ProjektnameFilterBuilder, IProjektnameFilter>();
            builder.RegisterFilterBuilder<StatusFilterBuilder, IStatusFilter>();
            builder.RegisterFilterBuilder<AusfuehrungsanfangVonBisFilterBuilder, IAusfuehrungsanfangVonBisFilter>();
            builder.RegisterFilterBuilder<TeilsystemFilterBuilder, ITeilsystemFilter>();
            builder.RegisterFilterBuilder<LeitendeOrganisationFilterBuilder, ILeitendeOrganisation>();
            builder.RegisterFilterBuilder<OrtsbezeichnungFilterBuilder, IOrtsbezeichnungFilter>();
        }
         
        private static void RegisterEnvironmentDependentTypes(ContainerBuilder builder)
        {
            var applicationEnvironment = new ServerConfigurationProvider().Environment;

            builder.RegisterType<MsSQLNHibernateConfigurationProvider>().As<INHibernateConfigurationProvider>().SingleInstance();
            builder.RegisterType<IdentityServiceFake>().As<IIdentityService>().SingleInstance();
            builder.RegisterType<UserIdentityProvider>().As<IUserIdentityProvider>().SingleInstance();
            builder.RegisterType<SessionService>().As<ISessionService>().SingleInstance();
            builder.RegisterType<CookieService>().As<ICookieService>().SingleInstance();
            builder.RegisterType<SessionCacheService>().As<ISessionCacheService>().SingleInstance();
            builder.RegisterType<EmbeddedResourceReportResourceLocator>().As<IReportResourceLocator>().SingleInstance();
            builder.RegisterType<MandantLogoService>().As<IMandantLogoService>().SingleInstance();
            builder.RegisterType<ReportTemplatingService>().As<IReportTemplatingService>().SingleInstance();
            builder.RegisterType<BenchmarkingDataService>().As<IBenchmarkingDataService>().SingleInstance();
            
            ////SecurityService
            builder.RegisterType<PermissionService>().As<IPermissionService>().SingleInstance();
            builder.RegisterType<AvailabiltyService>().As<IAvailabilityService>().SingleInstance();
            builder.RegisterType<CachedIdentityRoleService>().As<IIdentityCacheService>().SingleInstance();
            builder.RegisterType<IdentityRoleService>();

            switch (applicationEnvironment)
            {
                case ApplicationEnvironment.SpecFlow:
                    builder.RegisterType<MsSQLNHibernateConfigurationProvider>().As<INHibernateConfigurationProvider>().SingleInstance();
                    builder.RegisterType<TestUserIdentityProvider>().As<IUserIdentityProvider>().SingleInstance();
                    builder.RegisterType<TestSessionService>().As<ISessionService>().SingleInstance();
                    builder.RegisterType<TestCookieService>().As<ICookieService>().SingleInstance();
                    builder.RegisterType<TestSessionCacheService>().As<ISessionCacheService>().SingleInstance();
                    builder.RegisterType<TestPermissionService>().As<IPermissionService>().SingleInstance();
                    builder.RegisterType<TestMandantLogoService>().As<IMandantLogoService>().SingleInstance();
                    builder.RegisterType<StubLocalizationService>().As<ILocalizationService>().SingleInstance();
                    builder.RegisterType<StubReportLocalizationService>().As<IReportLocalizationService>().SingleInstance();
                    builder.RegisterType<StubEreignisLogService>().As<IEreignisLogService>().SingleInstance();
                    builder.RegisterType<StubLegendService>().As<ILegendService>().SingleInstance();
                    builder.RegisterType<StubReportTemplatingService>().As<IReportTemplatingService>().SingleInstance();
                    builder.RegisterType<TestJahresabschlussGISService>().As<IJahresabschlussGISService>().SingleInstance();

                    //MapProviders
                    builder.RegisterType<TestListeDerInspektionsroutenMapProvider>().As<IListeDerInspektionsroutenMapProvider>().SingleInstance();
                    builder.RegisterType<TestStrassenabschnitteListeMapProvider>().As<IStrassenabschnitteListeMapProvider>().SingleInstance();
                    builder.RegisterType<TestZustandProZustandsabschnittMapProvider>().As<IZustandProZustandsabschnittMapProvider>().SingleInstance();
                    builder.RegisterType<TestMassnahmenvorschlagProZustandsabschnittMapProvider>().As<IMassnahmenvorschlagProZustandsabschnittMapProvider>().SingleInstance();
                    builder.RegisterType<TestEineListeVonKoordiniertenMassnahmenMapProvider>().As<IEineListeVonKoordiniertenMassnahmenMapProvider>().SingleInstance();
                    builder.RegisterType<TestEineListeVonMassnahmenGegliedertNachTeilsystemenMapProvider>().As<IEineListeVonMassnahmenGegliedertNachTeilsystemenMapProvider>().SingleInstance(); 
                    break;
                case ApplicationEnvironment.Development:
                    builder.RegisterType<SourceCodeReportResourceLocator>().As<IReportResourceLocator>().SingleInstance();
                    //builder.RegisterType<RealTimeCalculatingBenchmarkingDataService>().As<IBenchmarkingDataService>().SingleInstance();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Environment", applicationEnvironment, "Not Supported!");
            }
        }

        public static void RegisterPoProviders(ContainerBuilder builder)
        {
            builder.RegisterPoProvider<MengeProBelastungskategoriePoProvider, IMengeProBelastungskategoriePoProvider, MengeProBelastungskategorieParameter>();
            builder.RegisterPoProvider<MengeProBelastungskategorieGrafischePoProvider, IMengeProBelastungskategorieGrafischePoProvider, MengeProBelastungskategorieGrafischeParameter>();
            builder.RegisterPoProvider<StrassenabschnitteListePoProvider, IStrassenabschnitteListePoProvider, StrassenabschnitteListeParameter>();
            builder.RegisterPoProvider<StrassenabschnitteListeMapPoProvider, IStrassenabschnitteListeMapPoProvider, StrassenabschnitteListeMapParameter>();
            builder.RegisterPoProvider<ListeDerInspektionsroutenPoProvider, IListeDerInspektionsroutenPoProvider, ListeDerInspektionsroutenParameter>();
            builder.RegisterPoProvider<ListeDerInspektionsroutenMapPoProvider, IListeDerInspektionsroutenMapPoProvider, ListeDerInspektionsroutenMapParameter>();
            builder.RegisterPoProvider<NochNichtInspizierteStrassenabschnittePoProvider, INochNichtInspizierteStrassenabschnittePoProvider, NochNichtInspizierteStrassenabschnitteParameter>();
            builder.RegisterPoProvider<WiederbeschaffungswertUndWertverlustProStrassenabschnittPoProvider, IWiederbeschaffungswertUndWertverlustProStrassenabschnittPoProvider, WiederbeschaffungswertUndWertverlustProStrassenabschnittParameter>();
            builder.RegisterPoProvider<WiederbeschaffungswertUndWertverlustGrafischePoProvider, IWiederbeschaffungswertUndWertverlustGrafischePoProvider, WiederbeschaffungswertUndWertverlustGrafischeParameter>();
            builder.RegisterPoProvider<AusgefuellteErfassungsformulareFuerOberflaechenschaedenPoProvider, IAusgefuellteErfassungsformulareFuerOberflaechenschaedenPoProvider, AusgefuellteErfassungsformulareFuerOberflaechenschaedenParameter>();
            builder.RegisterPoProvider<ErfassungsformulareFuerOberflaechenschaedenPoProvider, IErfassungsformulareFuerOberflaechenschaedenPoProvider, ErfassungsformulareFuerOberflaechenschaedenParameter>();
            builder.RegisterPoProvider<ZustandsspiegelProBelastungskategorieGrafischePoProvider, IZustandsspiegelProBelastungskategorieGrafischePoProvider, ZustandsspiegelProBelastungskategorieGrafischeParameter>();
            builder.RegisterPoProvider<ZustandProZustandsabschnittPoProvider, IZustandProZustandsabschnittPoProvider, ZustandProZustandsabschnittParameter>();
            builder.RegisterPoProvider<ZustandProZustandsabschnittMapPoProvider, IZustandProZustandsabschnittMapPoProvider, ZustandProZustandsabschnittMapParameter>();
            builder.RegisterPoProvider<MassnahmenvorschlagProZustandsabschnittPoProvider, IMassnahmenvorschlagProZustandsabschnittPoProvider, MassnahmenvorschlagProZustandsabschnittParameter>();
            builder.RegisterPoProvider<MassnahmenvorschlagProZustandsabschnittMapPoProvider, IMassnahmenvorschlagProZustandsabschnittMapPoProvider, MassnahmenvorschlagProZustandsabschnittMapParameter>();
            builder.RegisterPoProvider<WiederbeschaffungswertUndWertverlustProBelastungskategoriePoProvider, IWiederbeschaffungswertUndWertverlustProBelastungskategoriePoProvider, WiederbeschaffungswertUndWertverlustProBelastungskategorieParameter>();
            builder.RegisterPoProvider<StrassenabschnitteListeOhneInspektionsroutePoProvider, IStrassenabschnitteListeOhneInspektionsroutePoProvider, StrassenabschnitteListeOhneInspektionsrouteParameter>();
            builder.RegisterPoProvider<EineListeVonKoordiniertenMassnahmenPoProvider, IEineListeVonKoordiniertenMassnahmenPoProvider, EineListeVonKoordiniertenMassnahmenParameter>();
            builder.RegisterPoProvider<EineListeVonKoordiniertenMassnahmenMapPoProvider, IEineListeVonKoordiniertenMassnahmenMapPoProvider, EineListeVonKoordiniertenMassnahmenMapParameter>();
            builder.RegisterPoProvider<EineListeVonMassnahmenGegliedertNachTeilsystemenPoProvider, IEineListeVonMassnahmenGegliedertNachTeilsystemenPoProvider, EineListeVonMassnahmenGegliedertNachTeilsystemenParameter>();
            builder.RegisterPoProvider<EineListeVonMassnahmenGegliedertNachTeilsystemenMapPoProvider, IEineListeVonMassnahmenGegliedertNachTeilsystemenMapPoProvider, EineListeVonMassnahmenGegliedertNachTeilsystemenMapParameter>();
            builder.RegisterPoProvider<ZustandsspiegelProJahrGrafischePoProvider, IZustandsspiegelProJahrGrafischePoProvider, ZustandsspiegelProJahrGrafischeParameter>();
            builder.RegisterPoProvider<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenPoProvider, IEineListeVonRealisiertenMassnahmenGeordnetNachJahrenPoProvider, EineListeVonRealisiertenMassnahmenGeordnetNachJahrenParameter>();
            builder.RegisterPoProvider<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischPoProvider, IEineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischPoProvider, EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischParameter>();
            builder.RegisterPoProvider<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISPoProvider, IEineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISPoProvider, EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISParameter>();
            builder.RegisterPoProvider<WiederbeschaffungswertUndWertverlustProJahrGrafischePoProvider, IWiederbeschaffungswertUndWertverlustProJahrGrafischePoProvider, WiederbeschaffungswertUndWertverlustProJahrGrafischeParameter>();
            builder.RegisterPoProvider<RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischePoProvider, IRealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischePoProvider, RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeParameter>();
            builder.RegisterPoProvider<BenchmarkauswertungInventarkennwertenPoProvider, IBenchmarkauswertungInventarkennwertenPoProvider, BenchmarkauswertungInventarkennwertenParameter>();
            builder.RegisterPoProvider<BenchmarkauswertungZustandskennwertenPoProvider, IBenchmarkauswertungZustandskennwertenPoProvider, BenchmarkauswertungZustandskennwertenParameter>();
            builder.RegisterPoProvider<BenchmarkauswertungKennwertenRealisiertenMassnahmenPoProvider, IBenchmarkauswertungKennwertenRealisiertenMassnahmenPoProvider, BenchmarkauswertungKennwertenRealisiertenMassnahmenParameter>();
            builder.RegisterPoProvider<GISExportPoProvider, IGISExportPoProvider, GISExportParameter>();
            builder.RegisterPoProvider<GISExportMapPoProvider, IGISExportMapPoProvider, GISExportMapParameter>();
        }

        private void RegisterAllDerivedTypeAsSingleton<TMarkerInterface>(ContainerBuilder builder)
        {
            Assembly assembly = typeof(TMarkerInterface).Assembly;
            FindAndRegister<TMarkerInterface>(builder, assembly);

            if (typeof(ServerContainerSetup).Assembly != assembly)
                FindAndRegister<TMarkerInterface>(builder, typeof(ServerContainerSetup).Assembly);

            if (typeof(ServerContainerSetup).Assembly != assembly)
                FindAndRegister<TMarkerInterface>(builder, typeof(IService).Assembly);
        }

        private void FindAndRegister<TMarkerInterface>(ContainerBuilder builder, Assembly assembly)
        {
            IEnumerable<Type> interfacesToRegister = assembly.GetTypes()
                .Where(t => typeof (TMarkerInterface).IsAssignableFrom(t))
                .Where(t => t.IsInterface)
                .Where(t => !t.IsGenericType)
                .Where(t => t != typeof (TMarkerInterface));

            foreach (var interfaceType in interfacesToRegister)
            {
                Type implementationType = assembly.FindDerivedTypesFromAssembly(interfaceType, true)
                    .First(t => t.Name == interfaceType.Name.Substring(1));
                builder.RegisterType(implementationType).As(interfaceType).SingleInstance();
            }
        }
    }
}