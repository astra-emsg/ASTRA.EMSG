using System.Collections.Generic;
using ASTRA.EMSG.Business.Services.EntityServices;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using Resources;
using Kendo.Mvc.UI;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public static class IBelastungskategorieServiceExtension
    {
        public static IEnumerable<DropDownListItem> ToReportDropDownItems(this IBelastungskategorieService belastungskategorieService)
        {
            //  bk => bk.Typ because Telerik grid does not support filtering by Guids
            return belastungskategorieService.AllBelastungskategorieModel
                .ToDropDownItemList(bk => LookupLocalization.ResourceManager.GetString(bk.Typ), bk => bk.Typ, emptyItemText: TextLocalization.All);
        }
    }
}