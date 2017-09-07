using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Master.Logging;
using ASTRA.EMSG.Web.Areas.Administration;
using ASTRA.EMSG.Web.Areas.Auswertungen;
using ASTRA.EMSG.Web.Areas.Benchmarking;
using ASTRA.EMSG.Web.Areas.NetzverwaltungGIS;
using ASTRA.EMSG.Web.Areas.NetzverwaltungStrassennamen;
using ASTRA.EMSG.Web.Areas.NetzverwaltungSummarisch;
using ASTRA.EMSG.Web.Controllers;

namespace ASTRA.EMSG.Web.Infrastructure.Security
{
    public interface IPermissionService
    {
        Access CheckAccess();
        Access CheckAccess(Type controllerType, string actionName);
    }

    public class PermissionService : IPermissionService
    {
        private readonly ISecurityService securityService;
        private readonly Dictionary<string, List<Rolle>> actionRollenAssociationDictionary = new Dictionary<string, List<Rolle>>();
        private readonly Dictionary<string, List<Rolle>> controllerRollenAssociationDictionary = new Dictionary<string, List<Rolle>>();
        private readonly Dictionary<string, List<Rolle>> areaRollenAssociationDictionary = new Dictionary<string, List<Rolle>>();
        
        public PermissionService(ISecurityService securityService)
        {
            this.securityService = securityService;
            Init();
        }

        private void Init()
        {
            areaRollenAssociationDictionary.Add(string.Empty, new List<Rolle>());

            SetUpAreaRollen<AdministrationAreaRegistration>(new List<Rolle> { Rolle.Benutzeradministrator, Rolle.Applikationsadministrator });
            SetUpAreaRollen<NetzverwaltungSummarischAreaRegistration>(new List<Rolle> { Rolle.DataManager });
            SetUpAreaRollen<NetzverwaltungStrassennamenAreaRegistration>(new List<Rolle> { Rolle.DataManager });
            SetUpAreaRollen<NetzverwaltungGISAreaRegistration>(new List<Rolle> { Rolle.DataManager });
            SetUpAreaRollen<AuswertungenAreaRegistration>(new List<Rolle> { Rolle.DataReader });
            SetUpAreaRollen<BenchmarkingAreaRegistration>(new List<Rolle> { Rolle.Benchmarkteilnehmer });

            foreach (Type controllerType in typeof (HomeController).Assembly.FindDerivedTypesFromAssembly(typeof (Controller), true))
            {
                var controllerAccessRightsAttribute = controllerType.GetCustomAttributes(typeof (AccessRightsAttribute), false).SingleOrDefault() as AccessRightsAttribute;
                if(controllerAccessRightsAttribute != null)
                    controllerRollenAssociationDictionary.Add(controllerType.GetControllerKey(), controllerAccessRightsAttribute.Rollen);

                foreach (var methodInfo in controllerType.GetMethods())
                {
                    var actionAccessRightsAttribute = methodInfo.GetCustomAttributes(typeof(AccessRightsAttribute), false).SingleOrDefault() as AccessRightsAttribute;
                    if (actionAccessRightsAttribute != null)
                        actionRollenAssociationDictionary.Add(controllerType.GetActionKey(methodInfo), actionAccessRightsAttribute.Rollen);
                }
            }
        }

        private void SetUpAreaRollen<TAreaRegistration>(List<Rolle> rollen)
        {
            areaRollenAssociationDictionary.Add(RouteDataKeyHelper.GetAreaKey<TAreaRegistration>(), rollen);
        }

        public virtual Access CheckAccess()
        {
            RouteData routeData = HttpContext.Current.Request.RequestContext.RouteData;

            //Authenticate
            if(!securityService.IsAuthenticated)
            {
                Loggers.ApplicationLogger.Warn("Anonymous user is unauthenticated.");
                return Access.Denied;
            }

            //Authorization
            return CheckAccess(routeData);
        }

        public virtual Access CheckAccess(Type controllerType, string actionName)
        {
            var actionKey = controllerType.GetActionKey(actionName);
            var controllerKey = controllerType.GetControllerKey();
            var areaKey = controllerType.GetAreaKey();

            return CheckAccess(areaKey, controllerKey, actionKey);
        }

        private Access CheckAccess(RouteData routeData)
        {
            var actionKey = routeData.GetActionKey();
            var controllerKey = routeData.GetControllerKey();
            var areaKey = routeData.GetAreaKey();

            return CheckAccess(areaKey, controllerKey, actionKey);
        }

        private Access CheckAccess(string areaKey, string controllerKey, string actionKey)
        {
            List<Rolle> requiredRollen;
            if (!actionRollenAssociationDictionary.TryGetValue(actionKey, out requiredRollen))
            {
                if (!controllerRollenAssociationDictionary.TryGetValue(controllerKey, out requiredRollen))
                {
                    if (!areaRollenAssociationDictionary.TryGetValue(areaKey, out requiredRollen))
                    {
                        return Access.Denied;
                    }
                }
            }

            if (requiredRollen.Count == 0)
                return Access.Granted;

            if (requiredRollen.Any(UserRollen.Contains))
                return Access.Granted;

            if (controllerKey == "NetzverwaltungGIS_WMS")
                return Access.Granted;

            return Access.Denied;
        }

        private List<Rolle> UserRollen
        {
            get
            {
                const string emsgUserRollen = "EmsgUserRollen";
                if (!HttpContext.Current.Items.Contains(emsgUserRollen) || HttpContext.Current.Items[emsgUserRollen] == null)
                {
                    List<Rolle> rollen = securityService.GetCurrentRollen();
                    HttpContext.Current.Items.Add(emsgUserRollen, rollen);
                    return rollen;
                }
                return (List<Rolle>)HttpContext.Current.Items[emsgUserRollen];
            }
        }
    }
}