using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Infrastructure.Xlsx;
using ASTRA.EMSG.Business.Models.Strassennamen;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Common.Enums;
using ClosedXML.Excel;
using NHibernate.Linq;


namespace ASTRA.EMSG.Business.Services.EntityServices.Strassennamen
{
    public interface IStrassenabschnittService : IService
    {
        void CreateCopy(Strassenabschnitt strassenabschnittToCopy);
        List<StrassenabschnittOverviewModel> GetCurrentModelsByStrassenname(string strassennameFilter, string ortsbezeichnungFilter);
        List<StrassenabschnittOverviewModel> GetCurrentModelsByStrassenname(string strassennameFilter);
        List<StrassenabschnittModel> GetCurrentModels();
        void InsertSplittedStrassenabschnittModels(List<StrassenabschnittSplitModel> strassenabschnittModels);
        List<StrassenabschnittSplitModel> GetSplittedStrassenabschnittModels(SplitStrassenabschnittModel splitStrassenabschnittModel);
        StrassenabschnittModel CreateDefaultStrassenabschnittModel();
        bool IsThereMissingZustandsabschnitte();

        IQueryable<Strassenabschnitt> GetEntitiesBy(ErfassungsPeriod erfassungsPeriod);
        IQueryable<Strassenabschnitt> GetEntitiesBy(ErfassungsPeriod erfassungsPeriod, Mandant mandant);
        void DeleteEntity(Guid id);
        StrassenabschnittModel GetById(Guid id);
        StrassenabschnittModel UpdateEntity(StrassenabschnittModel strassenabschnittModel);
        StrassenabschnittModel CreateEntity(StrassenabschnittModel strassenabschnittModel);
        decimal GetSumOfZustandsabschnittLaenge(Guid strassenabschnittId);
        decimal GetStrassenabschnittOriginalLaenge(Guid strassenabschnittId);
        Strassenabschnitt GetStrassenabschnitt(string strassenname, string bezeichnungVon, string bezeichnungBis);
        bool IsUniqueId(string externalId, Guid Id);
        List<ExportStrassenabschnittModel> GetAllStrassenabschnittImportModels(string filter);
    }
    
    public class StrassenabschnittService : MandantAndErfassungsPeriodDependentEntityServiceBase<Strassenabschnitt, StrassenabschnittModel>, IStrassenabschnittService
    {
        private readonly IFahrbahnZustandService fahrbahnZustandService;
        private readonly IMassnahmenvorschlagCopyService massnahmenvorschlagCopyService;
        private readonly ILocalizationService localizationService;

        public StrassenabschnittService(
            ITransactionScopeProvider transactionScopeProvider, 
            IEntityServiceMappingEngine entityServiceMappingEngine,
            ISecurityService securityService,
            IFahrbahnZustandService fahrbahnZustandService,
            IHistorizationService historizationService,
            IMassnahmenvorschlagCopyService massnahmenvorschlagCopyService,
            ILocalizationService localizationService)
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService, historizationService)
        {
            this.fahrbahnZustandService = fahrbahnZustandService;
            this.massnahmenvorschlagCopyService = massnahmenvorschlagCopyService;
            this.localizationService = localizationService;
        }

        public Strassenabschnitt GetStrassenabschnitt(string strassenname, string bezeichnungVon, string bezeichnungBis)
        {
            return FilteredEntities
                .Where(s => s.Strassenname == strassenname)
                .Where(s => s.BezeichnungVon == bezeichnungVon)
                .Where(s => s.BezeichnungBis == bezeichnungBis)
                .Fetch(sa => sa.Belastungskategorie)
                .FirstOrDefault();
        }

        public bool IsUniqueId(string externalId, Guid Id)
        {
            return !FilteredEntities.Any(s => s.Id != Id && s.ExternalId == externalId);
        }

        protected override Expression<Func<Strassenabschnitt, Mandant>> MandantExpression { get { return s => s.Mandant; } }
        protected override Expression<Func<Strassenabschnitt, ErfassungsPeriod>> ErfassungsPeriodExpression { get { return s => s.ErfassungsPeriod; } }

        public void CreateCopy(Strassenabschnitt strassenabschnittToCopy)
        {
            var copiedStrassenabschnitt = entityServiceMappingEngine.Translate<Strassenabschnitt, Strassenabschnitt>(strassenabschnittToCopy);

            foreach (var zustandsabschnittToCopy in strassenabschnittToCopy.Zustandsabschnitten)
            {
                var copiedZustandsabschnitt = entityServiceMappingEngine.Translate<Zustandsabschnitt, Zustandsabschnitt>(zustandsabschnittToCopy);

                massnahmenvorschlagCopyService.CopyMassnahmenvorschlagen(copiedZustandsabschnitt, zustandsabschnittToCopy);

                copiedStrassenabschnitt.Zustandsabschnitten.Add(copiedZustandsabschnitt);

                foreach (var schadengruppe in zustandsabschnittToCopy.Schadengruppen)
                    copiedZustandsabschnitt.AddSchadengruppe(entityServiceMappingEngine.Translate<Schadengruppe, Schadengruppe>(schadengruppe));

                foreach (var schadendetail in zustandsabschnittToCopy.Schadendetails)
                    copiedZustandsabschnitt.AddSchadendetail(entityServiceMappingEngine.Translate<Schadendetail, Schadendetail>(schadendetail));
            }

            CreateEntity(copiedStrassenabschnitt);
        }

        public List<StrassenabschnittOverviewModel> GetCurrentModelsByStrassenname(string strassennameFilter, string ortsbezeichnungFilter)
        {
            var query = FilteredEntities;

            if (!string.IsNullOrWhiteSpace(strassennameFilter))
                query = query.Where(s => s.Strassenname.ToLower().Contains(strassennameFilter.ToLower()));

            if (!string.IsNullOrWhiteSpace(ortsbezeichnungFilter))
                query = query.Where(s => s.Ortsbezeichnung.ToLower().Contains(ortsbezeichnungFilter.ToLower()));

            return query.Fetch(s => s.Belastungskategorie).FetchMany(s => s.Zustandsabschnitten).Select(CreateStrassenabschnittOverviewModel).ToList();
        }

        public List<StrassenabschnittOverviewModel> GetCurrentModelsByStrassenname(string strassennameFilter)
        {
            var query = FilteredEntities;

            if (!string.IsNullOrWhiteSpace(strassennameFilter))
                query = query.Where(s => s.Strassenname.ToLower().Contains(strassennameFilter.ToLower()));

            return query.Fetch(s => s.Belastungskategorie).FetchMany(s => s.Zustandsabschnitten).Select(CreateStrassenabschnittOverviewModel).ToList();
        }

        private StrassenabschnittOverviewModel CreateStrassenabschnittOverviewModel(Strassenabschnitt strassenabschnitt)
        {
            var strassenabschnittOverviewModel = CreateModelFromEntity<StrassenabschnittOverviewModel>(strassenabschnitt);
            strassenabschnittOverviewModel.BelastungskategorieBezeichnung =
                localizationService.GetLocalizedBelastungskategorieTyp(strassenabschnittOverviewModel.BelastungskategorieTyp);
            if(!strassenabschnitt.Zustandsabschnitten.Any())
                SetErfassungsStatusBezeichnung(strassenabschnittOverviewModel, ErfassungsStatusTyp.Nein);
            else if(strassenabschnitt.Laenge == strassenabschnitt.Zustandsabschnitten.Sum(z => z.Laenge))
                SetErfassungsStatusBezeichnung(strassenabschnittOverviewModel, ErfassungsStatusTyp.Ja);
            else
                SetErfassungsStatusBezeichnung(strassenabschnittOverviewModel, ErfassungsStatusTyp.Teilweise);

            return strassenabschnittOverviewModel;
        }

        private void SetErfassungsStatusBezeichnung(StrassenabschnittOverviewModel strassenabschnittOverviewModel, ErfassungsStatusTyp erfassungsStatusTyp)
        {
            strassenabschnittOverviewModel.ErfassungsStatusBezeichnung = localizationService.GetLocalizedEnum(erfassungsStatusTyp);
        }

        protected override void OnEntityUpdating(StrassenabschnittModel model, Strassenabschnitt entity)
        {
            base.OnEntityUpdating(model, entity);
            if (model.Belag != entity.Belag)
            {
                foreach (var zustandsabschnitt in entity.Zustandsabschnitten)
                {
                    fahrbahnZustandService.ResetZustandsabschnittdetails(zustandsabschnitt);   
                }
            }
        }

        public List<StrassenabschnittSplitModel> GetSplittedStrassenabschnittModels(SplitStrassenabschnittModel splitStrassenabschnittModel)
        {
            var strassenabschnittModels = new List<StrassenabschnittSplitModel>();

            var strassenabschnittModelById = GetById(splitStrassenabschnittModel.StrassenabschnittId);

            StrassenabschnittSplitModel strassenabschnittModel = CreateDefaultStrassenabschnittModelInternal<StrassenabschnittSplitModel>();
            strassenabschnittModel.Id = strassenabschnittModelById.Id;
            strassenabschnittModel.Strassenname = strassenabschnittModelById.Strassenname;
            strassenabschnittModel.Strasseneigentuemer = strassenabschnittModelById.Strasseneigentuemer;
            strassenabschnittModel.Belastungskategorie = strassenabschnittModelById.Belastungskategorie;
            strassenabschnittModel.BelastungskategorieTyp = strassenabschnittModelById.BelastungskategorieTyp;
            strassenabschnittModel.Belag = strassenabschnittModelById.Belag;
            strassenabschnittModel.BreiteFahrbahn = strassenabschnittModelById.BreiteFahrbahn;

            for (int i = 0; i < splitStrassenabschnittModel.Count; i++)
                strassenabschnittModels.Add(strassenabschnittModel);

            return strassenabschnittModels;
        }

        public void InsertSplittedStrassenabschnittModels(List<StrassenabschnittSplitModel> strassenabschnittModels)
        {
            DeleteEntity(strassenabschnittModels.First().Id);

            foreach (var strassenabschnittModel in strassenabschnittModels)
                CreateEntity(strassenabschnittModel);
        }
        
        public StrassenabschnittModel CreateDefaultStrassenabschnittModel()
        {
            return CreateDefaultStrassenabschnittModelInternal<StrassenabschnittModel>();
        }

        private static T CreateDefaultStrassenabschnittModelInternal<T>() where T : StrassenabschnittModel, new()
        {
            return new T
                {
                    Trottoir = TrottoirTyp.NochNichtErfasst,
                    Belag = BelagsTyp.Asphalt,
                    Strasseneigentuemer = EigentuemerTyp.Gemeinde
                };
        }

        public bool IsThereMissingZustandsabschnitte()
        {
            return FilteredEntities.Any(s => !s.Zustandsabschnitten.Any());
        }

        public decimal GetSumOfZustandsabschnittLaenge(Guid strassenabschnittId)
        {
            var entityById = GetEntityById(strassenabschnittId);
            return entityById == null ? 0 : entityById.Zustandsabschnitten.Sum(za => za.Laenge);
        }

        public decimal GetStrassenabschnittOriginalLaenge(Guid strassenabschnittId)
        {
            return GetEntityById(strassenabschnittId).Laenge;
        }

        public List<ExportStrassenabschnittModel> GetAllStrassenabschnittImportModels(string filter)
        {
            var strassenabschnittOverviewModels = GetCurrentModelsByStrassenname(filter);

            return (from item in strassenabschnittOverviewModels
                    let strassenabschnittModel = GetById(item.Id)
                    select new ExportStrassenabschnittModel
                    {
                        Strassenname = item.Strassenname,
                        BezeichnungVon = item.BezeichnungVon,
                        BezeichnungBis = item.BezeichnungBis,
                        ExternalId = item.ExternalId,
                        Abschnittsnummer = item.Abschnittsnummer,
                        Strasseneigentuemer = strassenabschnittModel.Strasseneigentuemer,
                        Ortsbezeichnung = strassenabschnittModel.Ortsbezeichnung,
                        BelastungskategorieTyp = item.BelastungskategorieTyp,
                        Belag = strassenabschnittModel.Belag,
                        BreiteFahrbahn = strassenabschnittModel.BreiteFahrbahn,
                        Laenge = strassenabschnittModel.Laenge,
                        Trottoir = strassenabschnittModel.Trottoir,
                        BreiteTrottoirLinks = strassenabschnittModel.BreiteTrottoirLinks,
                        BreiteTrottoirRechts = strassenabschnittModel.BreiteTrottoirRechts
                    }).ToList();
        }
    }
}