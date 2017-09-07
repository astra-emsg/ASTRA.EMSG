using System;
using System.Reflection;
using System.Web.Routing;
using ASTRA.EMSG.Business.Services.Security;

namespace ASTRA.EMSG.Web.Infrastructure.Security
{
    public static class RouteDataKeyHelper
    {
        public static string GetAreaKey(this RouteData routeData)
        {
            return routeData.GetArea();
        }

        public static string GetAreaKey<TAreaRegistration>()
        {
            const string areaRegistrationPostfix = "AreaRegistration";

            var areaRegistrationTypeName = typeof(TAreaRegistration).Name;
            return areaRegistrationTypeName.Substring(0, areaRegistrationTypeName.Length - areaRegistrationPostfix.Length);
        }

        public static string GetAreaKey(this Type controllerType)
        {
            return controllerType.GetAreaName();
        }

        public static string GetControllerKey(this RouteData routeData)
        {
            var controller = routeData.GetController();
            return string.Format("{0}_{1}", GetAreaKey(routeData), controller);
        }

        public static string GetControllerKey(this Type controllerType)
        {
            var area = controllerType.GetAreaName();
            var controller = controllerType.GetControllerName();
            return string.Format("{0}_{1}", area, controller);
        }

        public static string GetActionKey(this RouteData routeData)
        {
            var action = routeData.GetAction();
            return string.Format("{0}_{1}", GetControllerKey(routeData), action);
        }

        public static string GetActionKey(this Type controllerType, MethodInfo actionMethodInfo)
        {
            return GetActionKey(controllerType, actionMethodInfo.Name);
        }

        public static string GetActionKey(this Type controllerType, string actionName)
        {
            return string.Format("{0}_{1}", GetControllerKey(controllerType), actionName);
        }
    }
}