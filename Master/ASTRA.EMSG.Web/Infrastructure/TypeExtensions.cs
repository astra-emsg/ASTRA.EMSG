using System;
using System.Linq;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public static class TypeExtensions
    {
        const string ControllerPostfix = "Controller";

        public static string GetControllerName(this Type controllerType)
        {
            var controllerTypeName = controllerType.Name;
            return controllerTypeName.Substring(0, controllerTypeName.Length - ControllerPostfix.Length);
        }

        public static string GetAreaName(this Type controllerType)
        {
            var namespaceParts = controllerType.Namespace.Split('.');
            return namespaceParts.Count() >= 5 ? namespaceParts[4] : string.Empty;
        }
    }
}