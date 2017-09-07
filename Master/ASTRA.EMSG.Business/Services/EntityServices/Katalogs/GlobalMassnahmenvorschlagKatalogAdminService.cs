using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Katalogs;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.Historization;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Services.EntityServices.Katalogs
{
    public interface IGlobalMassnahmenvorschlagKatalogEditService : IService
    {
        void AddMassnahmenvorschlag(MassnahmenvorschlagKatalogCreateModel editModel);
        void UpdateMassnahmenvorschlag(MassnahmenvorschlagKatalogEditModel editModel);
        MassnahmenvorschlagKatalogEditModel GetMassnahmenvorschlagKatalogModel(string massnahmenvorschlagTyp);
        void DeleteMassnahmenvorschlag(string typ);
        bool IsTypUniqe(string typ);
        MassnahmenvorschlagKatalogEditModel GetDefaultMassnahmenvorschlagKatalogModel(string massnahmenvorschlagTyp);
        bool IsMassnahmenvorschlagKatalogUsed(string typ);
    }

    public class GlobalMassnahmenvorschlagKatalogEditService : EntityServiceBase<GlobalMassnahmenvorschlagKatalog, MassnahmenvorschlagKatalogEditModel>, IGlobalMassnahmenvorschlagKatalogEditService
    {
        private readonly ILocalizationService localizationService;
        private readonly IHistorizationService historizationService;
        private readonly IMassnahmenvorschlagKatalogService massnahmenvorschlagKatalogService;

        public GlobalMassnahmenvorschlagKatalogEditService(
            ITransactionScopeProvider transactionScopeProvider, 
            IEntityServiceMappingEngine entityServiceMappingEngine, 
            ILocalizationService localizationService, 
            IHistorizationService historizationService,
            IMassnahmenvorschlagKatalogService massnahmenvorschlagKatalogService)
            : base(transactionScopeProvider, entityServiceMappingEngine)
        {
            this.localizationService = localizationService;
            this.historizationService = historizationService;
            this.massnahmenvorschlagKatalogService = massnahmenvorschlagKatalogService;
        }
        
        public bool IsTypUniqe(string typ)
        {
            GlobalMassnahmenvorschlagKatalog globalMassnahmenvorschlagKatalog = Queryable.FirstOrDefault(m => m.Parent.Typ == typ);
            return globalMassnahmenvorschlagKatalog == null;
        }

        public MassnahmenvorschlagKatalogEditModel GetDefaultMassnahmenvorschlagKatalogModel(string massnahmenvorschlagTyp)
        {
            return CreateModels(Queryable.Where(m => m.Parent.Typ == massnahmenvorschlagTyp).ToArray());
        }

        public MassnahmenvorschlagKatalogEditModel GetMassnahmenvorschlagKatalogModel(string massnahmenvorschlagTyp)
        {
            return CreateModels(GetCurrentEntities().Where(m => m.Parent.Typ == massnahmenvorschlagTyp).ToArray());
        }

        public void DeleteMassnahmenvorschlag(string typ)
        {
            if (IsMassnahmenvorschlagKatalogUsed(typ))
                throw new InvalidOperationException("GlobalRealisierteMassnahmenvorschlagKatalog can not be Deleted because it is in use!");

            MassnahmentypKatalog parent = null;
            foreach (var massnahmenvorschlagKatalog in Queryable.Where(m => m.Parent.Typ == typ))
            {
                parent = massnahmenvorschlagKatalog.Parent;
                DeleteEntity(massnahmenvorschlagKatalog.Id);
            }
            if (parent != null)
                Delete(parent);
        }

        protected override void OnEntityUpdating(GlobalMassnahmenvorschlagKatalog entity)
        {
            base.OnEntityUpdating(entity);
            var query = CurrentSession.CreateQuery(
                @"update MassnahmenvorschlagKatalog mv 
                  set mv.DefaultKosten = :kosten 
                  where mv.Id IN (select mk.Id from MassnahmenvorschlagKatalog mk where mk.IsCustomized = false and mk.ErfassungsPeriod.IsClosed = false and mk.Belastungskategorie = :bel and mk.Parent = :parent)").
                SetParameter("kosten", entity.DefaultKosten)
                .SetParameter("parent", entity.Parent)
                .SetParameter("bel", entity.Belastungskategorie);
            query.ExecuteUpdate();
        }

        protected override void OnEntityDeleting(GlobalMassnahmenvorschlagKatalog entity)
        {
            base.OnEntityDeleting(entity);

            var realisierteMassnahmenvorschlagKatalogs = Query<MassnahmenvorschlagKatalog>()
                .Where(rmk => !rmk.ErfassungsPeriod.IsClosed)
                .Where(rmk => rmk.Parent.Typ == entity.Parent.Typ)
                .ToList();

            foreach (var realisierteMassnahmenvorschlagKatalog in realisierteMassnahmenvorschlagKatalogs)
                Delete(realisierteMassnahmenvorschlagKatalog);
        }

        public void AddMassnahmenvorschlag(MassnahmenvorschlagKatalogCreateModel editModel)
        {
            var erfassungsPeriods = Query<ErfassungsPeriod>().Where(ep => !ep.IsClosed).Fetch(ep => ep.Mandant).ToList();
            int legendNumber = GetLegendNumber(editModel.Typ);
            var parent = new MassnahmentypKatalog();
            parent.KatalogTyp = editModel.KatalogTyp;
            parent.Typ = editModel.Typ;
            parent.LegendNumber = legendNumber;
            CurrentSession.Save(parent);                  
            foreach (var kosten in editModel.KonstenModels)
            {
                var massnahmenvorschlagKatalogEditModel =
                    new GlobalMassnahmenvorschlagKatalog
                        {
                            Belastungskategorie = GetEntityById<Belastungskategorie>(kosten.Belastungskategorie),
                            DefaultKosten = kosten.DefaultKosten ?? 0,
                            Parent = parent
                        };
                var entity = CreateEntity(massnahmenvorschlagKatalogEditModel);
                foreach (var erfassungsPeriod in erfassungsPeriods)
                {
                    var realisierteMassnahmenvorschlagKatalog = new MassnahmenvorschlagKatalog();
                    entityServiceMappingEngine.Translate(entity, realisierteMassnahmenvorschlagKatalog);

                    realisierteMassnahmenvorschlagKatalog.Mandant = erfassungsPeriod.Mandant;
                    realisierteMassnahmenvorschlagKatalog.ErfassungsPeriod = erfassungsPeriod;

                    Create(realisierteMassnahmenvorschlagKatalog);
                }
            }
        }

        private MassnahmenvorschlagKatalogEditModel CreateModels<T>(T[] massnahmenvorschlagKatalogs) where T : MassnahmenvorschlagKatalogBase
        {
            var result = new MassnahmenvorschlagKatalogEditModel
                {
                    Typ = massnahmenvorschlagKatalogs.First().Typ,
                    KatalogTyp = massnahmenvorschlagKatalogs.First().KatalogTyp,
                    KonstenModels = massnahmenvorschlagKatalogs.OrderBy(gm => gm.Belastungskategorie.Reihenfolge).Select(k => new MassnahmenvorschlagKatalogKonstenEditModel
                    {
                        Id = k.Id,
                        DefaultKosten = k.DefaultKosten,
                        Belastungskategorie = k.Belastungskategorie.Id,
                        BelastungskategorieBezeichnung = localizationService.GetLocalizedBelastungskategorieTyp(k.BelastungskategorieTyp)
                    }).ToList()
                };
            return result;
        }

        public void UpdateMassnahmenvorschlag(MassnahmenvorschlagKatalogEditModel editModel)
        {
            foreach (var kosten in editModel.KonstenModels)
            {
                var entity = GetEntityById(kosten.Id);
                entity.DefaultKosten = kosten.DefaultKosten ?? 0;
                UpdateEntity(entity);
            }
        }
        private int GetLegendNumber(string typ)
        {
            var existingTyp = massnahmenvorschlagKatalogService.GetAllEntities().Where(x => x.Parent.LegendNumber != null && x.Parent.Typ == typ);
            if (existingTyp.Count() > 0)
            {
                return existingTyp.Select(x => (int)x.LegendNumber).Distinct().Single();
            }
            var lnList = massnahmenvorschlagKatalogService.GetAllEntities().Where(x => x.Parent.LegendNumber != null && x.Parent.Typ != typ).Select(x => (int)x.Parent.LegendNumber).Distinct().ToList();
            var result = Enumerable.Range(1, 59).Except(lnList);
            var orderedList = result.OrderBy(ln => ln);

            if (orderedList.Count() > 0)
            {
                return orderedList.First();
            }
            else
            {
                bool lnFound = false;
                while (!lnFound)
                {
                    foreach (int i in Enumerable.Range(1, 59).OrderBy(r => r))
                    {
                        lnFound = !lnList.Remove(i);
                        if (lnFound)
                        {
                            return i;
                        }
                    }
                }
                return (massnahmenvorschlagKatalogService.GetAllEntities().Select(mvk => mvk.Parent.Typ).Distinct().Count() % 59) + 1;
            }
        }
        
        public bool IsMassnahmenvorschlagKatalogUsed(string typ)
        {
            return
                Query<KoordinierteMassnahmeGIS>().Any(
                    k => 
                    k.MassnahmenvorschlagFahrbahn.Typ == typ 
                    ) ||
                Query<RealisierteMassnahme>().Any(
                    r => 
                    r.MassnahmenvorschlagFahrbahn.Typ == typ ||
                    r.MassnahmenvorschlagTrottoir.Typ == typ
                    ) ||
                Query<RealisierteMassnahmeGIS>().Any(
                    r =>
                    r.MassnahmenvorschlagFahrbahn.Typ == typ ||
                    r.MassnahmenvorschlagTrottoir.Typ == typ
                    ) ||
                Query<Zustandsabschnitt>().Any(
                    z =>
                    z.Strassenabschnitt.ErfassungsPeriod.IsClosed == false &&
                    (z.MassnahmenvorschlagFahrbahn.Parent.Typ == typ ||
                     z.MassnahmenvorschlagTrottoirLinks.Parent.Typ == typ ||
                     z.MassnahmenvorschlagTrottoirRechts.Parent.Typ == typ)) ||
                Query<ZustandsabschnittGIS>().Any(
                    z =>
                    z.StrassenabschnittGIS.ErfassungsPeriod.IsClosed == false &&
                    (z.MassnahmenvorschlagFahrbahn.Parent.Typ == typ ||
                     z.MassnahmenvorschlagTrottoirLinks.Parent.Typ == typ ||
                     z.MassnahmenvorschlagTrottoirRechts.Parent.Typ == typ));
        }
    }
}