using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Business.Models.Katalogs;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Infrastructure;
using Resources;
using Kendo.Mvc.UI;
using ExpressionHelper = ASTRA.EMSG.Common.ExpressionHelper;

namespace ASTRA.EMSG.Web.Areas.Common.Controllers
{
    public abstract class ZustandsabschnittControllerBase : Controller
    {
        private readonly IMassnahmenvorschlagKatalogService massnahmenvorschlagKatalogService;
        protected readonly IFahrbahnZustandServiceBase fahrbahnZustandServiceBase;
        protected readonly ITrottoirZustandServiceBase trottoirZustandServiceBase;

        protected ZustandsabschnittControllerBase(IMassnahmenvorschlagKatalogService massnahmenvorschlagKatalogService, IFahrbahnZustandServiceBase fahrbahnZustandServiceBase, ITrottoirZustandServiceBase trottoirZustandServiceBase)
        {
            this.massnahmenvorschlagKatalogService = massnahmenvorschlagKatalogService;
            this.fahrbahnZustandServiceBase = fahrbahnZustandServiceBase;
            this.trottoirZustandServiceBase = trottoirZustandServiceBase;
        }

        public ActionResult EditZustandsabschnittFahrbahn(Guid id)
        {
            ZustandsabschnittdetailsModel zustandsabschnittdetailsModel = fahrbahnZustandServiceBase.GetZustandsabschnittdetailsModel(id);
            PrepareViewBagForFahrbahn(zustandsabschnittdetailsModel.MassnahmenvorschlagKatalog, zustandsabschnittdetailsModel.BelastungskategorieTyp);
            return PartialView(zustandsabschnittdetailsModel);
        }

        public ActionResult SaveZustandsabschnittFahrbahn(ZustandsabschnittdetailsModel zustandsabschnittdetailsModel)
        {
            if (ModelState.IsValid)
            {
                fahrbahnZustandServiceBase.UpdateZustandsabschnittdetails(zustandsabschnittdetailsModel);
                return new EmsgEmptyResult();
            }

            PrepareViewBagForFahrbahn(zustandsabschnittdetailsModel.MassnahmenvorschlagKatalog, zustandsabschnittdetailsModel.BelastungskategorieTyp);
            return PartialView("EditZustandsabschnittFahrbahn", zustandsabschnittdetailsModel);
        }

        public ActionResult ApplySaveZustandsabschnittFahrbahn(ZustandsabschnittdetailsModel zustandsabschnittdetailsModel)
        {
            if (ModelState.IsValid)
            {
                fahrbahnZustandServiceBase.UpdateZustandsabschnittdetails(zustandsabschnittdetailsModel);
                ModelState.Clear();
            }

            PrepareViewBagForFahrbahn(zustandsabschnittdetailsModel.MassnahmenvorschlagKatalog, zustandsabschnittdetailsModel.BelastungskategorieTyp);
            return PartialView("EditZustandsabschnittFahrbahn", zustandsabschnittdetailsModel);
        }
        
        public ActionResult GetErfassungForm(ZustandsErfassungsmodus zustandsErfassungsmodus, Guid zustandsabschnittId)
        {
            var model = fahrbahnZustandServiceBase.GetZustandsabschnittdetailsModel(zustandsabschnittId, zustandsErfassungsmodus);

            switch (zustandsErfassungsmodus)
            {
                case ZustandsErfassungsmodus.Manuel:
                    return PartialView("ManuelZustandsErfassungForm", model);
                case ZustandsErfassungsmodus.Grob:
                    return PartialView("GrobZustandsErfassungForm", model);
                case ZustandsErfassungsmodus.Detail:
                    return PartialView("DetailZustandsErfassungForm", model);
                default:
                    throw new ArgumentOutOfRangeException("zustandsErfassungsmodus");
            }
        }

        public ActionResult EditZustandsabschnittTrottoir(Guid id)
        {
            var zustandsabschnittdetailsTrottoirModel = trottoirZustandServiceBase.GetZustandsabschnittTrottoirModel(id);
            PrepareViewBagForTrottoir(zustandsabschnittdetailsTrottoirModel.LinkeTrottoirMassnahmenvorschlagKatalogId, zustandsabschnittdetailsTrottoirModel.RechteTrottoirMassnahmenvorschlagKatalogId, zustandsabschnittdetailsTrottoirModel.BelastungskategorieTyp);
            return PartialView(zustandsabschnittdetailsTrottoirModel);
        }

        public ActionResult SaveZustandsabschnittTrottoir(ZustandsabschnittdetailsTrottoirModel zustandsabschnittdetailsTrottoirModel)
        {
            if (ModelState.IsValid)
            {
                trottoirZustandServiceBase.UpdateZustandsabschnittTrottoirModel(zustandsabschnittdetailsTrottoirModel);
                return new EmsgEmptyResult();
            }

            PrepareViewBagForTrottoir(zustandsabschnittdetailsTrottoirModel.LinkeTrottoirMassnahmenvorschlagKatalogId, zustandsabschnittdetailsTrottoirModel.RechteTrottoirMassnahmenvorschlagKatalogId, zustandsabschnittdetailsTrottoirModel.BelastungskategorieTyp);
            return PartialView("EditZustandsabschnittTrottoir", zustandsabschnittdetailsTrottoirModel);
        }

        public ActionResult ApplySaveZustandsabschnittTrottoir(ZustandsabschnittdetailsTrottoirModel zustandsabschnittdetailsTrottoirModel)
        {
            if (ModelState.IsValid)
            {
                trottoirZustandServiceBase.UpdateZustandsabschnittTrottoirModel(zustandsabschnittdetailsTrottoirModel);
                ModelState.Clear();
            }

            PrepareViewBagForTrottoir(zustandsabschnittdetailsTrottoirModel.LinkeTrottoirMassnahmenvorschlagKatalogId, zustandsabschnittdetailsTrottoirModel.RechteTrottoirMassnahmenvorschlagKatalogId, zustandsabschnittdetailsTrottoirModel.BelastungskategorieTyp);
            return PartialView("EditZustandsabschnittTrottoir", zustandsabschnittdetailsTrottoirModel);
        }

        public string GetMassnahmenvorschlagKosten(Guid? massnahmenvorschlagKatalogId)
        {
            return string.Format(FormatStrings.LongDecimalFormat, massnahmenvorschlagKatalogService.GetMassnahmenvorschlagKosten(massnahmenvorschlagKatalogId));
        }

        protected void PrepareViewBagForTrottoir(Guid? selectedTrottoirLinksMassnahmenvorschlagKatalogId, Guid? selectedTrottoirRechtsMassnahmenvorschlagKatalogId, string belastungsKategorieTyp)
        {
            var massnahmenvorschlagKatalogModelList = massnahmenvorschlagKatalogService.GetMassnahmenvorschlagKatalogModelList(MassnahmenvorschlagKatalogTyp.Trottoir, belastungsKategorieTyp);
            ViewBag.TrottoirLinksMassnahmenvorschlagKatalogen = GetMassnamenKatalogDropDownItems(massnahmenvorschlagKatalogModelList, selectedTrottoirLinksMassnahmenvorschlagKatalogId, TextLocalization.EmptyMessage);
            ViewBag.TrottoirRechtsMassnahmenvorschlagKatalogen = GetMassnamenKatalogDropDownItems(massnahmenvorschlagKatalogModelList, selectedTrottoirRechtsMassnahmenvorschlagKatalogId, TextLocalization.EmptyMessage);
        }

        protected void PrepareViewBagForFahrbahn(Guid? selectedTrottoirRechtsMassnahmenvorschlagKatalogId,string belastungsKategorieTyp)
        {
            var massnahmenvorschlagKatalogModelList = massnahmenvorschlagKatalogService.GetMassnahmenvorschlagKatalogModelList(MassnahmenvorschlagKatalogTyp.Fahrbahn, belastungsKategorieTyp);
            ViewBag.MassnahmenvorschlagKatalogen = GetMassnamenKatalogDropDownItems(massnahmenvorschlagKatalogModelList, selectedTrottoirRechtsMassnahmenvorschlagKatalogId, TextLocalization.EmptyMessage);
        }

        private static IEnumerable<DropDownListItem> GetMassnamenKatalogDropDownItems(List<MassnahmenvorschlagKatalogModel> massnahmenvorschlagKatalogModelList, Guid? selectedTrottoirRechtsMassnahmenvorschlagKatalogId, string emptyMessageText = null)
        {
            var selectedTrottoirRechtsModel = massnahmenvorschlagKatalogModelList.FirstOrDefault(m => m.Id == selectedTrottoirRechtsMassnahmenvorschlagKatalogId);
            var trottoirRechtsMassnahmenvorschlagKatalogen = massnahmenvorschlagKatalogModelList
                .ToDropDownItemList(mk => LookupLocalization.ResourceManager.GetString(mk.Typ) ?? mk.Typ, mk => mk.Id, selectedTrottoirRechtsModel, emptyMessageText)
                .OrderBy(ddi => ddi.Text);
            return trottoirRechtsMassnahmenvorschlagKatalogen;
        }
    }
}