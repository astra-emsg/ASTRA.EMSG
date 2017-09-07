using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Common;
using JetBrains.Annotations;

namespace ASTRA.EMSG.Business.Models.Common
{
    public class MenuItemModel : IModel
    {
        public MenuItemModel()
        {
            SubMenuItemModels = new List<MenuItemModel>();
        }

        public MenuItemModel([AspMvcArea] string area, [AspMvcController] string controller, [AspMvcAction] string action) : this()
        {
            Area = area;
            Controller = controller;
            Action = action;
        }

        public string Text { get; set; }

        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        public Guid? Id { get; set; }

        public bool IsPlaceHolder { get; set; }
        public bool IsEmpty { get { return IsPlaceHolder && SubMenuItemModels.All(smim => smim.IsEmpty); } }

        public List<MenuItemModel> SubMenuItemModels { get; set; }
        public List<MenuItemModel> NotEmptySubMenuItemModels { get { return SubMenuItemModels.Where(smim => !smim.IsEmpty).ToList(); } }

        public string ResourceKey { get { return string.Format("{0}_{1}_{2}", Area, Controller, Action); } }
    }
}