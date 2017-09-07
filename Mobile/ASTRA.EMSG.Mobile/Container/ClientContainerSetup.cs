using ASTRA.EMSG.Common.Services.SchadenMetadaten;
using ASTRA.EMSG.Localization.Utils;
using ASTRA.EMSG.Mobile.Services;
using ASTRA.EMSG.Mobile.Utils;
using ASTRA.EMSG.Mobile.ViewModels;
using Autofac;
using ASTRA.EMSG.Map.Services;

namespace ASTRA.EMSG.Mobile.Container
{
    public class ClientContainerSetup
    {
        public virtual IContainer BuildContainer()
        {
            return GetContainerBuilder().Build();
        }

        protected ContainerBuilder GetContainerBuilder()
        {
            ContainerBuilder builder = new ContainerBuilder();

            ////ViewModels
            builder.RegisterType<MainWindowViewModel>().As<IMainWindowViewModel>().InstancePerDependency();
            builder.RegisterType<MapViewModel>().As<IMapViewModel>().InstancePerDependency();
            builder.RegisterType<FormViewModel>().As<IFormViewModel>().InstancePerDependency();
            builder.RegisterType<ProgressViewModel>().As<IProgressViewModel>().InstancePerDependency();

            ////Services
            builder.RegisterType<FormService>().As<IFormService>().SingleInstance();
            builder.RegisterType<MapService>().As<IMapService>().SingleInstance();
            builder.RegisterType<ClientConfigurationProvider>().As<IClientConfigurationProvider>().SingleInstance();
            builder.RegisterType<LocalizationHandler>().As<ILocalizationHandler>().SingleInstance();
            builder.RegisterType<FileDialogService>().As<IFileDialogService>().SingleInstance();
            builder.RegisterType<MessageBoxService>().As<IMessageBoxService>().SingleInstance();
            builder.RegisterType<PackageService>().As<IPackageService>().SingleInstance();
            builder.RegisterType<WindowService>().As<IWindowService>().SingleInstance();
            builder.RegisterType<ImportService>().As<IImportService>().SingleInstance();
            builder.RegisterType<DTOService>().As<IDTOService>().SingleInstance();
            builder.RegisterType<SaveService>().As<ISaveService>().SingleInstance();
            builder.RegisterType<LoadService>().As<ILoadService>().SingleInstance();
            builder.RegisterType<SchadenMetadatenService>().As<ISchadenMetadatenService>().SingleInstance();
            builder.RegisterType<LanguageService>().As<ILanguageService>().SingleInstance();
            builder.RegisterType<ExportService>().As<IExportService>().SingleInstance();
            builder.RegisterType<LogService>().As<ILogService>().SingleInstance();
            builder.RegisterType<GeoJsonService>().As<IGeoJsonService>().SingleInstance();
            builder.RegisterType<ProgressService>().As<IProgressService>().SingleInstance();         

            ////Handling the App mode (Test or Live)
            RegisterEnvironmentDependentTypes(builder);
            
            return builder;
        }

        private static void RegisterEnvironmentDependentTypes(ContainerBuilder builder)
        {
            //Note: Register environment dependent stubs if needed
        }
    }
}
