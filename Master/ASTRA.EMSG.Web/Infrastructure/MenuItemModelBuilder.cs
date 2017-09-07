using System;
using System.Collections.Generic;
using System.Reflection;
using ASTRA.EMSG.Business.Models;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.EntityServices;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using ASTRA.EMSG.Web.Infrastructure.Security;
using Resources;
using System.Linq;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public class MenuItemModelBuilder
    {
        private readonly IPermissionService permissionService;
        private readonly IServerConfigurationProvider serverConfigurationProvider;
        private readonly NetzErfassungsmodus netzErfassungsmodus;

        public MenuItemModelBuilder(IPermissionService permissionService, IServerConfigurationProvider serverConfigurationProvider, NetzErfassungsmodus netzErfassungsmodus)
        {
            this.permissionService = permissionService;
            this.serverConfigurationProvider = serverConfigurationProvider;
            this.netzErfassungsmodus = netzErfassungsmodus;
            menuItemModels = new List<MenuItemModel>();
        }

        private MenuItemModelBuilder(IPermissionService permissionService, IServerConfigurationProvider serverConfigurationProvider, NetzErfassungsmodus netzErfassungsmodus, List<MenuItemModel> menuItemModelList)
        {
            this.permissionService = permissionService;
            this.serverConfigurationProvider = serverConfigurationProvider;
            this.netzErfassungsmodus = netzErfassungsmodus;
            menuItemModels = menuItemModelList;
        }

        private readonly List<MenuItemModel> menuItemModels;
        public List<MenuItemModel> MenuItemModels { get { return menuItemModels.Where(mim => !mim.IsEmpty).ToList(); } }

        public void AddMenuItems<TController>(Action<MenuItemModelBuilder> subMenuBuilderAction, IEnumerable<NetzErfassungsmodus> netzErfassungsmoduses, string action = "Index")
        {
            subMenuBuilderAction(AddMenuItem<TController>(netzErfassungsmoduses, action));
        }

        public MenuItemModelBuilder AddMenuItem<TController>(NetzErfassungsmodus netzErfassungsmodus, string action = "Index")
        {
            return AddMenuItem<TController>(new[] { netzErfassungsmodus }, action);
        }

        public MenuItemModelBuilder AddMenuItem<TController>(IEnumerable<NetzErfassungsmodus> netzErfassungsmoduses, string action = "Index")
        {
            var controllerType = typeof(TController);

            if (permissionService.CheckAccess(controllerType, action) == Access.Denied)
                return new MenuItemModelBuilder(permissionService, serverConfigurationProvider, netzErfassungsmodus, new List<MenuItemModel>());

            if (!netzErfassungsmoduses.Contains(netzErfassungsmodus))
                return new MenuItemModelBuilder(permissionService,serverConfigurationProvider, netzErfassungsmodus, new List<MenuItemModel>());
            
            var menuItemModel = new MenuItemModel
                                    {
                                        Area = controllerType.GetAreaName(),
                                        Controller = controllerType.GetControllerName(),
                                        Action = action
                                    };



            menuItemModel.Text = (serverConfigurationProvider.Environment == ApplicationEnvironment.Development ? ReportTypeService.GetReportType(controllerType, netzErfassungsmodus) : string.Empty) + GetStringFromResource(menuItemModel.ResourceKey);

            menuItemModels.Add(menuItemModel);

            return new MenuItemModelBuilder(permissionService, serverConfigurationProvider, netzErfassungsmodus, menuItemModel.SubMenuItemModels);
        }

        public void AddPlaceHolderMenuItems(string text, Action<MenuItemModelBuilder> subMenuBuilderAction)
        {
            subMenuBuilderAction(AddPlaceHolderMenuItem(text));
        }

        public MenuItemModelBuilder AddPlaceHolderMenuItem(string text)
        {
            var menuItemModel = new MenuItemModel
                                    {
                                        Text = text,
                                        IsPlaceHolder = true
                                    };

            menuItemModels.Add(menuItemModel);

            return new MenuItemModelBuilder(permissionService, serverConfigurationProvider, netzErfassungsmodus, menuItemModel.SubMenuItemModels);
        }

        private string GetStringFromResource(string resourceKey)
        {
            return MenuLocalization.ResourceManager.GetString(resourceKey) ?? resourceKey;
        }
    }
}