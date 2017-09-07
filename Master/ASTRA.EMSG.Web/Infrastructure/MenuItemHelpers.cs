using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Models;
using ASTRA.EMSG.Business.Models.Common;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public static class MenuItemHelpers
    {
        public static List<MenuItemModel> GetBreadcrumbModel(ActionInfo actionInfo, List<MenuItemModel> menuItemModels)
        {
            var stack = new Stack<MenuItemModel>();
            GetPath(actionInfo, menuItemModels, stack);

            return stack.Reverse().ToList();
        }

        private static bool GetPath(ActionInfo actionInfo, List<MenuItemModel> menuItemModels, Stack<MenuItemModel> stack)
        {
            foreach (var menuItemModel in menuItemModels)
            {
                stack.Push(menuItemModel);

                if (menuItemModel.Area == actionInfo.Area && menuItemModel.Controller == actionInfo.Controller && menuItemModel.Action == actionInfo.Action)
                    return true;

                if(GetPath(actionInfo, menuItemModel.NotEmptySubMenuItemModels, stack))
                    return true;

                stack.Pop();
            }

            return false;
        }
    }
}