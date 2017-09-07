using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Strassennamen;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Services.Security;
using System.Linq;
using ASTRA.EMSG.Business.Utils;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Services.EntityServices.Strassennamen
{
    public interface IZustandsabschnittService : IService
    {
        List<ZustandsabschnittModel> GetAllZustandsabschnittModel(Guid strassenabschnittId);
        void DeleteEntity(Guid id);
        List<ZustandsabschnittModel> GetCurrentModelsByOrtsbezeichnung(string ortsbezeichnungFilter);
        List<ZustandsabschnittModel> GetZustandsabschnitteByStrassenabschnittId(Guid strassenabschnittId);
        List<ZustandsabschnittModel> GetCurrentModels();
        IQueryable<Zustandsabschnitt> GetCurrentEntities();
        IQueryable<Zustandsabschnitt> GetEntitiesBy(ErfassungsPeriod erfassungsPeriod);
        IQueryable<Zustandsabschnitt> GetEntitiesBy(ErfassungsPeriod erfassungsPeriod, Mandant mandant);
        ZustandsabschnittModel GetById(Guid id);
        ZustandsabschnittModel UpdateEntity(ZustandsabschnittModel zustandsabschnittModel);
        ZustandsabschnittModel CreateEntity(ZustandsabschnittModel zustandsabschnittModel);
        bool IsZustandsabschnittLaengeValid(Guid strassenabschnittId, Guid zustandsabschnittId, decimal zustandsabschnittLaenge);
        List<ExportZustandsabschnittModel> GetAllZustandsabschnittModels();
    }

    public class ZustandsabschnittService : MandantAndErfassungsPeriodDependentEntityServiceBase<Zustandsabschnitt, ZustandsabschnittModel>, IZustandsabschnittService
    {
        private readonly IServerConfigurationProvider serverConfigurationProvider;

        public ZustandsabschnittService(
            ITransactionScopeProvider transactionScopeProvider, 
            IEntityServiceMappingEngine entityServiceMappingEngine,
            ISecurityService securityService, 
            IHistorizationService historizationService,
            IServerConfigurationProvider serverConfigurationProvider)
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService, historizationService)
        {
            this.serverConfigurationProvider = serverConfigurationProvider;
        }

        protected override Expression<Func<Zustandsabschnitt, Mandant>> MandantExpression { get { return z => z.Strassenabschnitt.Mandant; } }
        protected override Expression<Func<Zustandsabschnitt, ErfassungsPeriod>> ErfassungsPeriodExpression { get { return z => z.Strassenabschnitt.ErfassungsPeriod; } }

        public List<ZustandsabschnittModel> GetAllZustandsabschnittModel(Guid strassenabschnittId)
        {
            return FilteredEntities.Where(z => z.Strassenabschnitt.Id == strassenabschnittId).Fetch(z => z.Strassenabschnitt).ToList().Select(CreateModel).ToList();
        }

        public List<ZustandsabschnittModel> GetCurrentModelsByOrtsbezeichnung(string ortsbezeichnungFilter)
        {
            var query = FilteredEntities;

            if (!string.IsNullOrWhiteSpace(ortsbezeichnungFilter))
                query = query.Where(s => s.Strassenabschnitt.Ortsbezeichnung.ToLower().Contains(ortsbezeichnungFilter.ToLower()));

            return query.Select(CreateModel).ToList();
        }

        public List<ZustandsabschnittModel> GetZustandsabschnitteByStrassenabschnittId(Guid strassenabschnittId)
        {
            var query = FilteredEntities.Where(s => s.Strassenabschnitt.Id.Equals(strassenabschnittId));

            return query.Select(CreateModel).ToList();
        }

        public override List<ZustandsabschnittModel> GetCurrentModels()
        {
            return FilteredEntities.Fetch(z => z.Strassenabschnitt).Select(CreateModel).ToList();
        }

        public bool IsZustandsabschnittLaengeValid(Guid strassenabschnittId, Guid zustandsabschnittId, decimal zustandsabschnittLaenge)
        {
            var strassenabschnitt = GetEntityById<Strassenabschnitt>(strassenabschnittId);
            var zustandsabschnitt = GetEntityById(zustandsabschnittId);

            var zustandsabschnitten = strassenabschnitt.Zustandsabschnitten;
            if (zustandsabschnitt != null)
                zustandsabschnitten = zustandsabschnitten.Where(za => za.Id != zustandsabschnitt.Id).ToList();

            return strassenabschnitt.Laenge >= zustandsabschnitten.Sum(za => za.Laenge) + zustandsabschnittLaenge;
        }

        protected override void OnModelCreated(Zustandsabschnitt entity, ZustandsabschnittModel model)
        {
            base.OnModelCreated(entity, model);
            model.Ortsbezeichnung = entity.Strassenabschnitt.Ortsbezeichnung;
            model.Sreassenabschnittsnummer = entity.Strassenabschnitt.Abschnittsnummer;
            model.StrasseLaenge = entity.Strassenabschnitt.Laenge;
            model.StrasseBezeichnungBis = entity.Strassenabschnitt.BezeichnungBis;
            model.StrasseBezeichnungVon = entity.Strassenabschnitt.BezeichnungVon;
            model.StrasseOrtsbezeichnung = entity.Strassenabschnitt.Ortsbezeichnung;
            model.Strassenabschnitt = entity.Strassenabschnitt.Id;
            model.StrasseLaenge = entity.Strassenabschnitt.Laenge;
            model.BemerkungShort = model.Bemerkung.TrimToMaxLength(serverConfigurationProvider.BemerkungMaxDisplayLength);
        }

        protected override void OnEntityCreating(Zustandsabschnitt entity)
        {
            base.OnEntityCreating(entity);
            entity.Erfassungsmodus = ZustandsErfassungsmodus.Manuel;
        }

        public List<ExportZustandsabschnittModel> GetAllZustandsabschnittModels()
        {
           
            return (from item in GetCurrentEntities().ToArray()
                    select new ExportZustandsabschnittModel
                        {
                            Strassenname = item.Strassenname,
                            StrassennameBezeichnungVon = item.StrassennameBezeichnungVon,
                            StrassennameBezeichnungBis = item.StrassennameBezeichnungBis, 
                            ExternalId = item.ExternalId, 
                            Abschnittsnummer = item.Abschnittsnummer,
                            BezeichnungVon = item.BezeichnungVon, 
                            BezeichnungBis = item.BezeichnungBis, 
                            Laenge = item.Laenge, 
                            Zustandsindex = item.Zustandsindex,
                            ZustandsindexTrottoirLinks = item.ZustandsindexTrottoirLinks,
                            ZustandsindexTrottoirRechts = item.ZustandsindexTrottoirRechts, 
                            Aufnahmedatum = item.Aufnahmedatum, 
                            Aufnahmeteam = item.Aufnahmeteam, 
                            Wetter = item.Wetter, Bemerkung = item.Bemerkung,
                            MassnahmenvorschlagKatalogTypFahrbahn = item.MassnahmenvorschlagKatalogTypFahrbahn,
                            DringlichkeitFahrbahn = item.DringlichkeitFahrbahn,
                            MassnahmenvorschlagKatalogTypTrottoirLinks = item.MassnahmenvorschlagKatalogTypTrottoirLinks,
                            DringlichkeitTrottoirLinks = item.DringlichkeitTrottoirLinks,
                            MassnahmenvorschlagKatalogTypTrottoirRechts = item.MassnahmenvorschlagKatalogTypTrottoirRechts,
                            DringlichkeitTrottoirRechts = item.DringlichkeitTrottoirRechts
                        }).ToList();
        }
    }
}