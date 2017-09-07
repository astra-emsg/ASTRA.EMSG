using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Services.SchadenMetadaten;
using NHibernate.Linq;
using NHibernate.Util;

namespace ASTRA.EMSG.Business.Services.EntityServices.Common
{
    public interface IFahrbahnZustandServiceBase
    {
        ZustandsabschnittdetailsModel GetZustandsabschnittdetailsModel(Guid id);
        ZustandsabschnittdetailsModel GetZustandsabschnittdetailsModel(Guid id, ZustandsErfassungsmodus zustandsErfassungsmodus);
        void UpdateZustandsabschnittdetails(ZustandsabschnittdetailsModel zustandsabschnittdetailsModel);
        void ResetZustandsabschnittdetails(ZustandsabschnittBase zustandsabschnittBase);
        ZustandsabschnittdetailsModel GetDefaultZustandsabschnittdetailsModel(Guid strassenabschnittId, ZustandsErfassungsmodus zustandsErfassungsmodus);
    }

    public abstract class FahrbahnZustandServiceBase : IFahrbahnZustandServiceBase
    {
        private readonly ISchadenMetadatenService schadenMetadatenService;
        protected readonly ITransactionScopeProvider transactionScopeProvider;

        protected FahrbahnZustandServiceBase(ITransactionScopeProvider transactionScopeProvider, ISchadenMetadatenService schadenMetadatenService)
        {
            this.schadenMetadatenService = schadenMetadatenService;
            this.transactionScopeProvider = transactionScopeProvider;
        }

        protected abstract ZustandsabschnittBase GetZustandsabschnittBase(Guid id);
        protected abstract StrassenabschnittBase GetStrassenabschnittBase(Guid id);
        protected abstract void UpdateZustandsabschnittBase(ZustandsabschnittBase zustandsabschnittBase);

        protected abstract bool IsLocked(StrassenabschnittBase strassenabschnittBase);

        public ZustandsabschnittdetailsModel GetZustandsabschnittdetailsModel(Guid id)
        {
            var zustandsabschnitt = GetZustandsabschnittBase(id);
            return CreateZustandsabschnittdetailsModel(zustandsabschnitt, zustandsabschnitt.Erfassungsmodus);
        }

        public ZustandsabschnittdetailsModel GetZustandsabschnittdetailsModel(Guid id, ZustandsErfassungsmodus zustandsErfassungsmodus)
        {
            return CreateZustandsabschnittdetailsModel(GetZustandsabschnittBase(id), zustandsErfassungsmodus);
        }

        public ZustandsabschnittdetailsModel GetDefaultZustandsabschnittdetailsModel(Guid strassenabschnittId, ZustandsErfassungsmodus zustandsErfassungsmodus)
        {
            var strassenabschnittBase = GetStrassenabschnittBase(strassenabschnittId);
            var zustandsabschnittdetailsModel = new ZustandsabschnittdetailsModel
            {
                Erfassungsmodus = zustandsErfassungsmodus,
                Strassenname = strassenabschnittBase.Strassenname,
                BelastungskategorieTyp = strassenabschnittBase.BelastungskategorieTyp,
                BezeichnungVon = strassenabschnittBase.BezeichnungVon,
                BezeichnungBis = strassenabschnittBase.BezeichnungBis,
                SchadengruppeModelList = new List<SchadengruppeModel>(),
                IsLocked = IsLocked(strassenabschnittBase),
                Belag = strassenabschnittBase.Belag,
                IsGrobInitializiert = false,
                IsDetailInitializiert = false
            };

           if (zustandsErfassungsmodus != ZustandsErfassungsmodus.Manuel)
            {
                zustandsabschnittdetailsModel.SchadengruppeModelList = schadenMetadatenService
                    .GetSchadengruppeMetadaten(strassenabschnittBase.Belag)
                    .Select(s => CreateSchadengruppeModel(s, zustandsErfassungsmodus))
                    .ToList();
            }

            return zustandsabschnittdetailsModel;
        }

        private SchadengruppeModel CreateSchadengruppeModel(SchadengruppeMetadaten schadengruppeMetadaten, ZustandsErfassungsmodus zustandsErfassungsmodus)
        {
            var schadengruppeModel = new SchadengruppeModel
            {
                Id = Guid.Empty,
                SchadenausmassTyp = SchadenausmassTyp.A0,
                SchadenschwereTyp = SchadenschwereTyp.S1,
                Gewicht = schadengruppeMetadaten.Gewicht,
                SchadengruppeTyp = schadengruppeMetadaten.SchadengruppeTyp,
                SchadendetailModelList = new List<SchadendetailModel>()
            };

            if (zustandsErfassungsmodus == ZustandsErfassungsmodus.Detail)
            {
                foreach (var schadendetailMetadaten in schadengruppeMetadaten.Schadendetails)
                {
                    var schadendetailModel = new SchadendetailModel
                    {
                        Id = Guid.Empty,
                        SchadenausmassTyp = SchadenausmassTyp.A0,
                        SchadenschwereTyp = SchadenschwereTyp.S1,
                        SchadendetailTyp = schadendetailMetadaten.SchadendetailTyp
                    };

                    schadengruppeModel.SchadendetailModelList.Add(schadendetailModel);
                }
            }

            return schadengruppeModel;
        }

        private ZustandsabschnittdetailsModel CreateZustandsabschnittdetailsModel(ZustandsabschnittBase zustandsabschnittBase, ZustandsErfassungsmodus zustandsErfassungsmodus)
        {
            var zustandsabschnittdetailsModel = new ZustandsabschnittdetailsModel
                                                    {
                                                        Id = zustandsabschnittBase.Id,
                                                        Erfassungsmodus = zustandsErfassungsmodus,
                                                        Strassenname = zustandsabschnittBase.Strassenname,
                                                        BelastungskategorieTyp = zustandsabschnittBase.BelastungskategorieTyp,
                                                        BezeichnungVon = zustandsabschnittBase.BezeichnungVon,
                                                        BezeichnungBis = zustandsabschnittBase.BezeichnungBis,
                                                        Zustandsindex = zustandsabschnittBase.Zustandsindex,
                                                        SchadengruppeModelList = new List<SchadengruppeModel>(),
                                                        Dringlichkeit = zustandsabschnittBase.Dringlichkeit,
                                                        MassnahmenvorschlagKatalog = zustandsabschnittBase.MassnahmenvorschlagKatalogId,
                                                        KostenFahrbahn = zustandsabschnittBase.KostenFahrbahn,
                                                        IsLocked = (zustandsabschnittBase is ZustandsabschnittGIS) && ((ZustandsabschnittGIS)zustandsabschnittBase).IsLocked,
                                                        Belag = zustandsabschnittBase.Belag,
                                                    };

            if (zustandsabschnittBase.Erfassungsmodus == zustandsErfassungsmodus)
            {
                zustandsabschnittdetailsModel.IsDetailInitializiert = zustandsErfassungsmodus == ZustandsErfassungsmodus.Detail;
                zustandsabschnittdetailsModel.IsGrobInitializiert = zustandsErfassungsmodus == ZustandsErfassungsmodus.Grob;
            }
            else
            {
                zustandsabschnittdetailsModel.IsDetailInitializiert = false;
                zustandsabschnittdetailsModel.IsGrobInitializiert = false;
            }

            zustandsabschnittdetailsModel.Kosten = GetKosten(zustandsabschnittBase.MassnahmenvorschlagFahrbahn, zustandsabschnittBase.Kosten);

            if (zustandsErfassungsmodus != ZustandsErfassungsmodus.Manuel)
            {
                zustandsabschnittdetailsModel.SchadengruppeModelList = schadenMetadatenService
                    .GetSchadengruppeMetadaten(zustandsabschnittBase.StrassenabschnittBase.Belag)
                    .Select(s => CreateSchadengruppeModel(s, zustandsabschnittBase, zustandsErfassungsmodus))
                    .ToList();
            }

            return zustandsabschnittdetailsModel;
        }

        private SchadengruppeModel CreateSchadengruppeModel(SchadengruppeMetadaten schadengruppeMetadaten, ZustandsabschnittBase zustandsabschnittBase, ZustandsErfassungsmodus zustandsErfassungsmodus)
        {
            var schadengruppe = zustandsabschnittBase.Schadengruppen.SingleOrDefault(s => s.SchadengruppeTyp == schadengruppeMetadaten.SchadengruppeTyp);

            var schadengruppeModel = new SchadengruppeModel
                                         {
                                             Id = schadengruppe == null ? Guid.Empty : schadengruppe.Id,
                                             SchadenausmassTyp = schadengruppe == null ? SchadenausmassTyp.A0 : schadengruppe.SchadenausmassTyp,
                                             SchadenschwereTyp = schadengruppe == null ? SchadenschwereTyp.S1 : schadengruppe.SchadenschwereTyp,
                                             Gewicht = schadengruppeMetadaten.Gewicht,
                                             SchadengruppeTyp = schadengruppeMetadaten.SchadengruppeTyp,
                                             ZustandsabschnittId = zustandsabschnittBase.Id,
                                             SchadendetailModelList = new List<SchadendetailModel>()
                                         };

            if (zustandsErfassungsmodus == ZustandsErfassungsmodus.Detail)
            {
                foreach (var schadendetailMetadaten in schadengruppeMetadaten.Schadendetails)
                {
                    var schadendetail = zustandsabschnittBase.Schadendetails.SingleOrDefault(s => s.SchadendetailTyp == schadendetailMetadaten.SchadendetailTyp);

                    var schadendetailModel = new SchadendetailModel
                                                 {
                                                     Id = schadendetail == null ? Guid.Empty : schadendetail.Id,
                                                     SchadenausmassTyp = schadendetail == null ? SchadenausmassTyp.A0 : schadendetail.SchadenausmassTyp,
                                                     SchadenschwereTyp = schadendetail == null ? SchadenschwereTyp.S1 : schadendetail.SchadenschwereTyp,
                                                     ZustandsabschnittId = zustandsabschnittBase.Id,
                                                     SchadendetailTyp = schadendetailMetadaten.SchadendetailTyp
                                                 };

                    schadengruppeModel.SchadendetailModelList.Add(schadendetailModel);
                }
            }

            return schadengruppeModel;
        }

        public void UpdateZustandsabschnittdetails(ZustandsabschnittdetailsModel zustandsabschnittdetailsModel)
        {
            var zustandsabschnittBase = GetZustandsabschnittBase(zustandsabschnittdetailsModel.Id);

            zustandsabschnittBase.Erfassungsmodus = zustandsabschnittdetailsModel.Erfassungsmodus;

            zustandsabschnittBase.MassnahmenvorschlagFahrbahn = zustandsabschnittdetailsModel.MassnahmenvorschlagKatalog == null ? null : transactionScopeProvider.GetById<MassnahmenvorschlagKatalog>(zustandsabschnittdetailsModel.MassnahmenvorschlagKatalog.Value);
            zustandsabschnittBase.DringlichkeitFahrbahn = zustandsabschnittdetailsModel.Dringlichkeit;

            if (zustandsabschnittBase.MassnahmenvorschlagFahrbahn == null)
                zustandsabschnittBase.KostenMassnahmenvorschlagFahrbahn = null;
            else
                zustandsabschnittBase.KostenMassnahmenvorschlagFahrbahn =
                    zustandsabschnittBase.KostenMassnahmenvorschlagFahrbahn == zustandsabschnittBase.MassnahmenvorschlagFahrbahn.DefaultKosten
                        ? null
                        : zustandsabschnittdetailsModel.Kosten;

            //Update back the Kosten and KostenFahrbahn calculated field
            zustandsabschnittdetailsModel.KostenFahrbahn = zustandsabschnittBase.KostenFahrbahn;
            zustandsabschnittdetailsModel.Kosten = GetKosten(zustandsabschnittBase.MassnahmenvorschlagFahrbahn, zustandsabschnittBase.Kosten);
            zustandsabschnittdetailsModel.Belag = zustandsabschnittBase.StrassenabschnittBase.Belag;

            DeleteSchadenData(zustandsabschnittBase);

            switch (zustandsabschnittdetailsModel.Erfassungsmodus)
            {
                case ZustandsErfassungsmodus.Manuel:
                    zustandsabschnittBase.Zustandsindex = zustandsabschnittdetailsModel.Zustandsindex.Value;
                    break;
                case ZustandsErfassungsmodus.Grob:
                    zustandsabschnittBase.Zustandsindex = zustandsabschnittdetailsModel.ZustandsindexCalculated;
                    GetSchadengruppen(zustandsabschnittdetailsModel.SchadengruppeModelList).ForEach(zustandsabschnittBase.AddSchadengruppe);
                    break;
                case ZustandsErfassungsmodus.Detail:
                    zustandsabschnittBase.Zustandsindex = zustandsabschnittdetailsModel.ZustandsindexCalculated;
                    CreateSchadendetails(zustandsabschnittdetailsModel.SchadengruppeModelList.SelectMany(sg => sg.SchadendetailModelList)).ForEach(zustandsabschnittBase.AddSchadendetail);
                    break;
            }

            UpdateZustandsabschnittBase(zustandsabschnittBase);
        }

        private void DeleteSchadenData(ZustandsabschnittBase zustandsabschnittBase)
        {
            zustandsabschnittBase.Schadengruppen.ForEach(transactionScopeProvider.Delete);
            zustandsabschnittBase.Schadendetails.ForEach(transactionScopeProvider.Delete);
            zustandsabschnittBase.DeleteSchadenFormData();
        }

        public void ResetZustandsabschnittdetails(ZustandsabschnittBase zustandsabschnittBase)
        {
            zustandsabschnittBase.Zustandsindex = 0.0m;
            zustandsabschnittBase.Erfassungsmodus = ZustandsErfassungsmodus.Manuel;
            DeleteSchadenData(zustandsabschnittBase);
            UpdateZustandsabschnittBase(zustandsabschnittBase);
        }

        private IEnumerable<Schadendetail> CreateSchadendetails(IEnumerable<SchadendetailModel> schadendetailModelList)
        {
            return schadendetailModelList.Select(sdm => new Schadendetail
                                                            {
                                                                SchadendetailTyp = sdm.SchadendetailTyp,
                                                                SchadenschwereTyp = sdm.SchadenausmassTyp == SchadenausmassTyp.A0 ? SchadenschwereTyp.S1 : sdm.SchadenschwereTyp,
                                                                SchadenausmassTyp = sdm.SchadenausmassTyp,
                                                            });
        }

        private IEnumerable<Schadengruppe> GetSchadengruppen(IEnumerable<SchadengruppeModel> schadengruppeModelList)
        {
            return schadengruppeModelList.Select(sgm => new Schadengruppe
                                                            {
                                                                SchadengruppeTyp = sgm.SchadengruppeTyp,
                                                                SchadenschwereTyp = sgm.SchadenausmassTyp == SchadenausmassTyp.A0 ? SchadenschwereTyp.S1 : sgm.SchadenschwereTyp,
                                                                SchadenausmassTyp = sgm.SchadenausmassTyp,
                                                            });
        }

        private decimal? GetKosten(MassnahmenvorschlagKatalog massnahmenvorschlag, decimal? kosten)
        {
            if (massnahmenvorschlag == null)
                return null;

            return kosten ?? massnahmenvorschlag.DefaultKosten;
        }
    }
}