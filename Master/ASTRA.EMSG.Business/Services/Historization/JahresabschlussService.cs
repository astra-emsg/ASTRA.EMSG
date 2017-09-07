using System;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.Services.Common;
using System.Linq;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Business.Services.EntityServices.Strassennamen;
using ASTRA.EMSG.Business.Services.EntityServices.Summarisch;
using ASTRA.EMSG.Common.Enums;
using NHibernate.Linq;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Summarisch;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Master.Logging;
using FluentNHibernate.Utils;
using NHibernate;
using GeoAPI.Geometries;

namespace ASTRA.EMSG.Business.Services.Historization
{
    public interface IJahresabschlussService : IService
    {
        void CloseCurrentErfassungsperiod(ErfassungsabschlussModel erfassungsabschlussModel);
        void RevertLastErfassungsperiod();
    }

    public class JahresabschlussService : IJahresabschlussService
    {
        private const string CurrentPeriodName = "CURRENT";
        private readonly ITransactionScopeProvider transactionScopeProvider;
        private readonly IErfassungsPeriodService erfassungsPeriodService;
        private readonly IStrassenabschnittService strassenabschnittService;
        private readonly INetzSummarischService netzSummarischService;
        private readonly INetzSummarischDetailService netzSummarischDetailService;
        private readonly IStrassenabschnittGISService strassenabschnittGISService;
        private readonly ITrottoirZustandServiceBase trottoirZustandService;
        private readonly ITrottoirZustandGISService trottoirZustandGisService;
        private readonly IKatalogCopyService katalogCopyService;
        private readonly IJahresabschlussGISService jahresabschlussGISService;
        private readonly IInspektionsRouteGISService inspektionsRouteGISService;
        private readonly IKoordinierteMassnahmeGISModelService koordinierteMassnahmeGISModelService;
        private readonly IRealisierteMassnahmeService realisierteMassnahmeService;
        private readonly IRealisierteMassnahmeSummarsichService realisierteMassnahmeSummarsichService;
        private readonly IEreignisLogService ereignisLogService;
        private readonly IMandantenDetailsCopyService mandantenDetailsCopyService;
        private readonly IBenchmarkingDataDetailCalculatorService benchmarkingDataDetailCopyService;
        private readonly IMassnahmenvorschlagTeilsystemeGISModelService massnahmenvorschlagTeilsystemeGISModelService;
        private readonly IKenngroessenFruehererJahreService kenngroessenFruehererJahreService;

        public JahresabschlussService(
            ITransactionScopeProvider transactionScopeProvider,
            IErfassungsPeriodService erfassungsPeriodService,
            IStrassenabschnittService strassenabschnittService,
            INetzSummarischService netzSummarischService,
            IStrassenabschnittGISService strassenabschnittGISService,
            ITrottoirZustandService trottoirZustandService,
            ITrottoirZustandGISService trottoirZustandGisService,
            IKatalogCopyService katalogCopyService,
            INetzSummarischDetailService netzSummarischDetailService,
            IInspektionsRouteGISService inspektionsRouteGISService,
            IJahresabschlussGISService jahresabschlussGISService,
            IRealisierteMassnahmeService realisierteMassnahmeService,
            IKoordinierteMassnahmeGISModelService koordinierteMassnahmeGISModelService,
            IRealisierteMassnahmeSummarsichService realisierteMassnahmeSummarsichService,
            IEreignisLogService ereignisLogService, 
            IMandantenDetailsCopyService mandantenDetailsCopyService,
            IBenchmarkingDataDetailCalculatorService benchmarkingDataDetailCopyService,
            IMassnahmenvorschlagTeilsystemeGISModelService massnahmenvorschlagTeilsystemeGISModelService,
            IKenngroessenFruehererJahreService kenngroessenFruehererJahreService)
        {
            this.transactionScopeProvider = transactionScopeProvider;
            this.erfassungsPeriodService = erfassungsPeriodService;
            this.strassenabschnittService = strassenabschnittService;
            this.netzSummarischService = netzSummarischService;
            this.strassenabschnittGISService = strassenabschnittGISService;
            this.trottoirZustandService = trottoirZustandService;
            this.trottoirZustandGisService = trottoirZustandGisService;
            this.katalogCopyService = katalogCopyService;
            this.netzSummarischDetailService = netzSummarischDetailService;
            this.jahresabschlussGISService = jahresabschlussGISService;
            this.inspektionsRouteGISService = inspektionsRouteGISService;
            this.realisierteMassnahmeService = realisierteMassnahmeService;
            this.realisierteMassnahmeSummarsichService = realisierteMassnahmeSummarsichService;
            this.ereignisLogService = ereignisLogService;
            this.mandantenDetailsCopyService = mandantenDetailsCopyService;
            this.benchmarkingDataDetailCopyService = benchmarkingDataDetailCopyService;
            this.koordinierteMassnahmeGISModelService = koordinierteMassnahmeGISModelService;
            this.massnahmenvorschlagTeilsystemeGISModelService = massnahmenvorschlagTeilsystemeGISModelService;
            this.kenngroessenFruehererJahreService = kenngroessenFruehererJahreService;
        }

        public void CloseCurrentErfassungsperiod(ErfassungsabschlussModel erfassungsabschlussModel)
        {
            Guid closedPeriodId;
            try
            {
                var closedPeriod = CloseCurrentPeriod(erfassungsabschlussModel);
                closedPeriodId = closedPeriod.Id;
                var currentPeriod = CreateNewPeriod(closedPeriod);
                erfassungsPeriodService.InvalidateCurrentErfassungsPeriodCache();
                    CreateNewErfassungsPeriodData(closedPeriod, currentPeriod);

                ereignisLogService.LogEreignis(EreignisTyp.Jahresabschluss, 
                    new Dictionary<string, object> { {"abgeschlossene jahr", closedPeriod.Erfassungsjahr.Year} });
                transactionScopeProvider.CurrentTransactionScope.Commit();
            }
            catch (Exception e)
            {
                Loggers.ApplicationLogger.Warn(String.Format("Error during CloseCurrentErfassungsperiod: {0}", e.ToString()));
                transactionScopeProvider.CurrentTransactionScope.Rollback();
                throw;
            }
            finally
            {
                transactionScopeProvider.ResetCurrentTransactionScope();
            }
            //Run the DeleteNotUsedData in its own transaction to avoid deadlocks in the database
            DeleteNotUsedData(closedPeriodId);
        }

        public void RevertLastErfassungsperiod()
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                var currentPeriod = erfassungsPeriodService.GetCurrentErfassungsPeriod();
                var lastClosedPeriod = erfassungsPeriodService.GetNewestClosedErfassungsPeriod();
                if (lastClosedPeriod == null)
                    throw new IndexOutOfRangeException("There is no closed ErfassungsPeriod found for this Mandant!");

                DeleteAllKenngroessenFruehererJahre(currentPeriod.Mandant);
                DeleteAllDataFromCurrentPeriod(currentPeriod, lastClosedPeriod);
                MakePeriodCurrent(lastClosedPeriod);

                ereignisLogService.LogEreignis(EreignisTyp.JahresabschlussRueckgaengingMachen,
                    new Dictionary<string, object> {{"jahr", lastClosedPeriod.Erfassungsjahr.Year}});

                transactionScopeProvider.CurrentTransactionScope.Session.Transaction.Commit();
                stopwatch.Stop();
                Loggers.PeformanceLogger.Info(String.Format("RevertLastErfassungsperiod finished. Mandant: {0} - Duration: {1}",
                        currentPeriod.Mandant.MandantDisplayName, stopwatch.Elapsed));
            }
            catch (Exception e)
            {
                Loggers.ApplicationLogger.Warn(String.Format("Error during RevertLastErfassungsperiod: {0}", e.ToString()));
                transactionScopeProvider.CurrentTransactionScope.Rollback();
                throw;
            }
            finally
            {
                transactionScopeProvider.ResetCurrentTransactionScope();
            }
        }

        private void DeleteAllDataFromCurrentPeriod(ErfassungsPeriod currentPeriod, ErfassungsPeriod lastClosedPeriod)
        {
            var session = transactionScopeProvider.CurrentTransactionScope.Session;

            //Summarsich
            DeleteEntitesByErfassungsPeriod<RealisierteMassnahmeSummarsich>(session, currentPeriod);
            DeleteEntitesByErfassungsPeriod<NetzSummarischDetail, NetzSummarisch>(session, currentPeriod, n => n.NetzSummarisch);
            DeleteEntitesByErfassungsPeriod<NetzSummarisch>(session, currentPeriod);

            //Tabellarisch
            DeleteEntitesByErfassungsPeriod<RealisierteMassnahme>(session, currentPeriod);

            DeleteEntitesByErfassungsPeriod<Schadengruppe, Zustandsabschnitt, Strassenabschnitt>(session, currentPeriod, s => s.Zustandsabschnitt, z => z.Strassenabschnitt);
            DeleteEntitesByErfassungsPeriod<Schadendetail, Zustandsabschnitt, Strassenabschnitt>(session, currentPeriod, s => s.Zustandsabschnitt, z => z.Strassenabschnitt);
            DeleteEntitesByErfassungsPeriod<Zustandsabschnitt, Strassenabschnitt>(session, currentPeriod, n => n.Strassenabschnitt);
            DeleteEntitesByErfassungsPeriod<Strassenabschnitt>(session, currentPeriod);

            //GIS
            DeleteGisEntities(session, currentPeriod);

            //Benchmarking: needs to be deleted from the previous period because it is calculated during thee Jahresabschluss
            DeleteEntitesByErfassungsPeriod<BenchmarkingDataDetail, BenchmarkingData>(session, lastClosedPeriod, b => b.BenchmarkingData);
            DeleteEntitesByErfassungsPeriod<BenchmarkingData>(session, lastClosedPeriod);

            //Other
            DeleteEntitesByErfassungsPeriod<MandantDetails>(session, currentPeriod);
            DeleteEntitesByErfassungsPeriod<AchsenUpdateConflict>(session, currentPeriod);
            
            //Katalogs
            DeleteEntitesByErfassungsPeriod<WiederbeschaffungswertKatalog>(session, currentPeriod);
            DeleteEntitesByErfassungsPeriod<MassnahmenvorschlagKatalog>(session, currentPeriod);
            
            //ErfassungsPeriod
            session.Delete(currentPeriod);
        }
        
        private void MakePeriodCurrent(ErfassungsPeriod lastClosedPeriod)
        {
            lastClosedPeriod.Name = CurrentPeriodName;
            lastClosedPeriod.IsClosed = false;
            erfassungsPeriodService.InvalidateCurrentErfassungsPeriodCache();
        }

        private void DeleteAllKenngroessenFruehererJahre(Mandant mandant)
        {
            if (erfassungsPeriodService.GetClosedErfassungsPeriodModels().Count == 1)
                kenngroessenFruehererJahreService
                    .DeleteAllKenngroessenFruehererJahre(kenngroessenFruehererJahreService.GetEntitiesBy(mandant));
        }


        private void DeleteEntitesByErfassungsPeriod<T>(ISession session, ErfassungsPeriod erfassungsPeriod) where T : IErfassungsPeriodDependentEntity
        {
            var typeName = typeof(T).Name;
            var queryString = string.Format("delete {0} c where c.ErfassungsPeriod = :ep", typeName);
            session.CreateQuery(queryString).SetParameter("ep", erfassungsPeriod).ExecuteUpdate();
        }

        private void DeleteEntitesByErfassungsPeriod<TEntity, TParent>(ISession session, 
            ErfassungsPeriod erfassungsPeriod,
            Expression<Func<TEntity, TParent>> parentSelector) where TParent : IErfassungsPeriodDependentEntity
        {
            
            var typeName = typeof(TEntity).Name;
            var parentTypeName = typeof (TParent).Name;
            var parentPropName = ExpressionHelper.GetPropertyName(parentSelector);
            var queryString = string.Format("delete {0} c where c.{1}.Id in (select p.id from {2} p where p.ErfassungsPeriod = :ep)", typeName, parentPropName, parentTypeName);
            session.CreateQuery(queryString).SetParameter("ep", erfassungsPeriod).ExecuteUpdate();
        }

        private void DeleteEntitesByErfassungsPeriod<TEntity, TParent, TGrandParent>(ISession session,
            ErfassungsPeriod erfassungsPeriod,
            Expression<Func<TEntity, TParent>> parentSelector,
            Expression<Func<TParent, TGrandParent>> grandParentSelector) where TGrandParent : IErfassungsPeriodDependentEntity
        {

            var typeName = typeof(TEntity).Name;
            var parentTypeName = typeof(TParent).Name;
            var grandParentTypeName = typeof(TGrandParent).Name;
            var parentPropName = ExpressionHelper.GetPropertyName(parentSelector);
            var grandParentPropName = ExpressionHelper.GetPropertyName(grandParentSelector);
            var queryString = string.Format("delete {0} c where c.{1}.Id in (select p.id from {2} p where p.{3}.Id in (select gp.Id from {4} gp where gp.ErfassungsPeriod = :ep))", typeName, parentPropName, parentTypeName, grandParentPropName, grandParentTypeName);
            session.CreateQuery(queryString).SetParameter("ep", erfassungsPeriod).ExecuteUpdate();
        }
        private void DeleteEntitesByErfassungsPeriod<TEntity, TParent, TGrandParent, TGreatGrandParent>(ISession session,
           ErfassungsPeriod erfassungsPeriod,
           Expression<Func<TEntity, TParent>> parentSelector,
           Expression<Func<TParent, TGrandParent>> grandParentSelector,
           Expression<Func<TGrandParent, TGreatGrandParent>> greatGrandParentSelector) where TGreatGrandParent : IErfassungsPeriodDependentEntity
        {

            var typeName = typeof(TEntity).Name;
            var parentTypeName = typeof(TParent).Name;
            var grandParentTypeName = typeof(TGrandParent).Name;
            var greatGrandParentTypeName = typeof(TGreatGrandParent).Name;
            var parentPropName = ExpressionHelper.GetPropertyName(parentSelector);
            var grandParentPropName = ExpressionHelper.GetPropertyName(grandParentSelector);
            var greatGrandParentPropName = ExpressionHelper.GetPropertyName(greatGrandParentSelector);
            var queryString = string.Format("delete {0} c where c.{1}.Id in (select p.id from {2} p where p.{3}.Id in (select gp.Id from {4} gp where gp.{5}.Id in (select ggp.Id from {6} ggp where ggp.ErfassungsPeriod = :ep))", 
                typeName, parentPropName, parentTypeName, grandParentPropName, grandParentTypeName, greatGrandParentPropName, greatGrandParentTypeName);
            session.CreateQuery(queryString).SetParameter("ep", erfassungsPeriod).ExecuteUpdate();
        }
        private void DeleteGisEntities(ISession session, ErfassungsPeriod currentPeriod)
        {
            String MassnahmenvorschlagTeilsystemeGisTypeName = typeof(MassnahmenvorschlagTeilsystemeGIS).Name;
            String KoordinierteMassnahmeGisTypeName = typeof(KoordinierteMassnahmeGIS).Name;
            String referenzGruppeTypeName = typeof(ReferenzGruppe).Name;
            String ZustandsabschnittGISTypeName = typeof(ZustandsabschnittGIS).Name;
            String StrassenabschnittGISTypeName = typeof(StrassenabschnittGIS).Name;
            String RealisierteMassnahmeGISTypeName = typeof(RealisierteMassnahmeGIS).Name;
            String achsenreferenzTypeName = typeof(AchsenReferenz).Name;
            String achsenSegmentTypeName = typeof(AchsenSegment).Name;
            String teilSystemTypeName = typeof(TeilsystemTyp).Name;
            String schaddetTypeName = typeof(Schadendetail).Name;
            String schadgrupTypeName = typeof(Schadengruppe).Name;

            String mvtReferenzgruppePropName = ExpressionHelper.GetPropertyName<MassnahmenvorschlagTeilsystemeGIS, ReferenzGruppe>(r => r.ReferenzGruppe);
            String kmReferenzgruppePropName = ExpressionHelper.GetPropertyName<KoordinierteMassnahmeGIS, ReferenzGruppe>(r => r.ReferenzGruppe);
            String zaReferenzgruppePropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, ReferenzGruppe>(r => r.ReferenzGruppe);
            String saReferenzgruppePropName = ExpressionHelper.GetPropertyName<StrassenabschnittGIS, ReferenzGruppe>(r => r.ReferenzGruppe);
            String arReferenzgruppePropName = ExpressionHelper.GetPropertyName<AchsenReferenz, ReferenzGruppe>(r => r.ReferenzGruppe);
            String rmReferenzgruppePropName = ExpressionHelper.GetPropertyName<RealisierteMassnahmeGIS, ReferenzGruppe>(r => r.ReferenzGruppe);
            String zaSchadgruppePropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, IList<Schadengruppe>>(z => z.Schadengruppen);
            String zaSchaddetPropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, IList<Schadendetail>>(z => z.Schadendetails);
            String zaStrabPropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, StrassenabschnittGIS>(z => z.StrassenabschnittGIS);
            String idPropName = ExpressionHelper.GetPropertyName<IIdHolder, Guid>(i => i.Id);
            String erpPropName = ExpressionHelper.GetPropertyName<IErfassungsPeriodDependentEntity, ErfassungsPeriod>(i => i.ErfassungsPeriod);

            String kmBetSysPropName = ExpressionHelper.GetPropertyName<KoordinierteMassnahmeGIS, IList<TeilsystemTyp>>(ktg => ktg.BeteiligteSysteme);
            String rgAchsenReferenzPropName = ExpressionHelper.GetPropertyName<ReferenzGruppe, IList<AchsenReferenz>>(rg => rg.AchsenReferenzen);
            String arAchssegPropName = ExpressionHelper.GetPropertyName<AchsenReferenz, AchsenSegment>(ar => ar.AchsenSegment);
            String arRefgrpPropName = ExpressionHelper.GetPropertyName<AchsenReferenz, ReferenzGruppe>(ar => ar.ReferenzGruppe);
            
            DeleteEntitesByErfassungsPeriod<AchsenUpdateLog>(session, currentPeriod);
            DeleteEntitesByErfassungsPeriod<AchsenUpdateConflict>(session, currentPeriod);
            DeleteEntitesByErfassungsPeriod<AchsenUpdateConflict>(session, currentPeriod);

            String queryString;

            IList<MassnahmenvorschlagTeilsystemeGIS> massTeilGis = session.QueryOver<MassnahmenvorschlagTeilsystemeGIS>().Where(mvt => mvt.Mandant.Id == currentPeriod.Mandant.Id).List();
            foreach(MassnahmenvorschlagTeilsystemeGIS mvt in massTeilGis){
                if (mvt.ReferenzGruppe.CopiedFrom != null)
                {
                    //this entity is not Erfassungsperiode dependent but the Achsenreferenzen are, so the referenzen get copied during Jahresabschluss but the entity simply references the new Achsenrefernzen
                    //Here we simply assing the old Referenzen
                    mvt.ReferenzGruppe = mvt.ReferenzGruppe.CopiedFrom;
                    IGeometry geom = null;
                    foreach (AchsenReferenz ar in mvt.ReferenzGruppe.AchsenReferenzen)
                    {
                        geom = geom == null ? ar.Shape : geom.Union(ar.Shape);
                    }
                    mvt.Shape = geom;
                    session.Update(mvt);
                }
                else
                {
                    //there is no copied from so the Massnahme was created in this Erfassungsperiode
                    //therefore there are no previous Referenzen to map it to so delete it
                    massnahmenvorschlagTeilsystemeGISModelService.DeleteEntity(mvt.Id);
                }
            }

            IList<KoordinierteMassnahmeGIS> massKoordGis = session.QueryOver<KoordinierteMassnahmeGIS>().Where(kmg => kmg.Mandant.Id == currentPeriod.Mandant.Id).List();

            foreach (KoordinierteMassnahmeGIS kmg in massKoordGis)
            {
                if (kmg.ReferenzGruppe.CopiedFrom != null)
                {
                    //this entity is not Erfassungsperiode dependent but the Achsenreferenzen are, so the referenzen get copied during Jahresabschluss but the entity simply references the new Achsenrefernzen
                    //Here we simply assing the old Referenzen
                    kmg.ReferenzGruppe = kmg.ReferenzGruppe.CopiedFrom;
                    IGeometry geom = null;
                    foreach (AchsenReferenz ar in kmg.ReferenzGruppe.AchsenReferenzen)
                    {
                        geom = geom == null ? ar.Shape : geom.Union(ar.Shape);
                    }
                    kmg.Shape = geom;
                    session.Update(kmg);
                }
                else
                {
                    //there is no copied from so the Massnahme was created in this Erfassungsperiode
                    //therefore there are no previous Referenzen to map it to so delete it
                    koordinierteMassnahmeGISModelService.DeleteEntity(kmg.Id);
                }
            }

            DeleteEntitesByErfassungsPeriod<RealisierteMassnahmeGIS>(session, currentPeriod);

            DeleteEntitesByErfassungsPeriod<CheckOutsGIS, InspektionsRouteGIS>(session, currentPeriod, co => co.InspektionsRouteGIS);
            DeleteEntitesByErfassungsPeriod<InspektionsRtStrAbschnitte, InspektionsRouteGIS>(session, currentPeriod, irs => irs.InspektionsRouteGIS);

            DeleteEntitesByErfassungsPeriod<InspektionsRouteStatusverlauf, InspektionsRouteGIS>(session, currentPeriod, irv => irv.InspektionsRouteGIS);
            DeleteEntitesByErfassungsPeriod<InspektionsRouteGIS>(session, currentPeriod);

            DeleteEntitesByErfassungsPeriod<RealisierteMassnahme>(session, currentPeriod);
            DeleteEntitesByErfassungsPeriod<AchsenReferenz, AchsenSegment>(session, currentPeriod, ar => ar.AchsenSegment);

            queryString = string.Format("delete {1} sd where sd.{0} in (select ssd.{0} from {3} z inner join z.{2} ssd inner join z.{4} s where s.{5} = :ep)",
                 idPropName, schaddetTypeName, zaSchaddetPropName, ZustandsabschnittGISTypeName, zaStrabPropName, erpPropName);
            session.CreateQuery(queryString).SetParameter("ep", currentPeriod).ExecuteUpdate();
             queryString = string.Format("delete {1} sd where sd.{0} in (select ssd.{0} from {3} z inner join z.{2} ssd inner join z.{4} s where s.{5} = :ep)",
                 idPropName, schadgrupTypeName, zaSchadgruppePropName, ZustandsabschnittGISTypeName, zaStrabPropName, erpPropName);
            session.CreateQuery(queryString).SetParameter("ep", currentPeriod).ExecuteUpdate();

            DeleteEntitesByErfassungsPeriod<ZustandsabschnittGIS, StrassenabschnittGIS>(session, currentPeriod, z => z.StrassenabschnittGIS);
            DeleteEntitesByErfassungsPeriod<StrassenabschnittGIS>(session, currentPeriod);


            queryString = String.Format(@"delete {13} c where 
                    not exists (select n.{12} from {0} n where n.{6}.{12} = c.{12}) and
                    not exists (select n.{12} from {1} n where n.{7}.{12} = c.{12}) and
                    not exists (select n.{12} from {2} n where n.{8}.{12} = c.{12}) and
                    not exists (select n.{12} from {3} n where n.{9}.{12} = c.{12}) and
                    not exists (select n.{12} from {4} n where n.{10}.{12} = c.{12}) and
                    not exists (select n.{12} from {5} n where n.{11}.{12} = c.{12})
                    ", MassnahmenvorschlagTeilsystemeGisTypeName, KoordinierteMassnahmeGisTypeName, ZustandsabschnittGISTypeName, StrassenabschnittGISTypeName, RealisierteMassnahmeGISTypeName, achsenreferenzTypeName,
                      mvtReferenzgruppePropName, kmReferenzgruppePropName, zaReferenzgruppePropName, saReferenzgruppePropName, rmReferenzgruppePropName, arReferenzgruppePropName,
                      idPropName, referenzGruppeTypeName
             );
            session.CreateQuery(queryString).ExecuteUpdate();

            DeleteEntitesByErfassungsPeriod<Sektor, AchsenSegment>(session, currentPeriod, s => s.AchsenSegment);
            DeleteEntitesByErfassungsPeriod<AchsenSegment>(session, currentPeriod);
            DeleteEntitesByErfassungsPeriod<Achse>(session, currentPeriod);


        }


        private ErfassungsPeriod CloseCurrentPeriod(ErfassungsabschlussModel erfassungsabschlussModel)
        {
            var currentPeriod = erfassungsPeriodService.GetCurrentErfassungsPeriod();
            currentPeriod.Erfassungsjahr = erfassungsabschlussModel.AbschlussDate;
            currentPeriod.Name = erfassungsabschlussModel.AbschlussDate.Year.ToString();
            return currentPeriod;
        }

        private ErfassungsPeriod CreateNewPeriod(ErfassungsPeriod currentPeriod)
        {
            //Close the currentPeriod here to limit the time while the current is closed but the new one is not yet created
            currentPeriod.IsClosed = true;
            var erfassungsPeriod = new ErfassungsPeriod
                                       {
                                           IsClosed = false,
                                           Name = CurrentPeriodName,
                                           Mandant = currentPeriod.Mandant,
                                           NetzErfassungsmodus = currentPeriod.NetzErfassungsmodus,
                                           Erfassungsjahr = currentPeriod.Erfassungsjahr.AddYears(1)
                                       };

            transactionScopeProvider.Create(erfassungsPeriod);

            return erfassungsPeriod;
        }

        private void CreateNewErfassungsPeriodData(ErfassungsPeriod closedPeriod, ErfassungsPeriod currentPeriod)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            netzSummarischService.CreateNetzSummarischFor(currentPeriod);

            CopyKatalogData(closedPeriod);
            CopyMandantenDetailsData(closedPeriod);
            CalculateBenchmarkingData(closedPeriod);

            switch (closedPeriod.NetzErfassungsmodus)
            {
                case NetzErfassungsmodus.Summarisch:
                    CopySummarischeModusData(closedPeriod);
                    break;
                case NetzErfassungsmodus.Tabellarisch:
                    CopyStrassenabschnitteData(closedPeriod);
                    break;
                case NetzErfassungsmodus.Gis:
                    // moved copyGisData because Axis should be copied in all cases.
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
                       

            TimeSpan gisJahresabschlussStart = stopwatch.Elapsed;
            jahresabschlussGISService.CopyGisData(closedPeriod);
            TimeSpan gisJahresabschlussEnd = stopwatch.Elapsed;
            Loggers.PeformanceLogger.Debug("Gis Jahresabschluss completed in " + (gisJahresabschlussEnd - gisJahresabschlussStart).ToString());
        }

        private void CalculateBenchmarkingData(ErfassungsPeriod closedPeriod)
        {
            benchmarkingDataDetailCopyService.CalculateBenchmarkingData(closedPeriod);
        }

        private void CopyMandantenDetailsData(ErfassungsPeriod closedPeriod)
        {
            mandantenDetailsCopyService.CopyMandantenDetailsData(closedPeriod);
        }

        private void CopyKatalogData(ErfassungsPeriod closedPeriod)
        {
            katalogCopyService.CopyKatalogData(closedPeriod);
        }

        private void CopySummarischeModusData(ErfassungsPeriod closedPeriod)
        {
            var currentNetzSummarisch = netzSummarischService.GetCurrentNetzSummarisch();

            var currentNetzSummarischDetails = netzSummarischDetailService.GetEntites();
            var previousNetzSummarischDetails = netzSummarischDetailService.GetEntitiesBy(closedPeriod);

            foreach (var previous in previousNetzSummarischDetails.ToList())
            {
                var current = currentNetzSummarischDetails.Single(nsd => nsd.Belastungskategorie == previous.Belastungskategorie);
                
                current.Fahrbahnflaeche = previous.Fahrbahnflaeche;
                current.Fahrbahnlaenge = previous.Fahrbahnlaenge;

                transactionScopeProvider.Update(current);
            }

            transactionScopeProvider.Update(currentNetzSummarisch);
        }

        private void CopyStrassenabschnitteData(ErfassungsPeriod closedPeriod)
        {
            var query = strassenabschnittService.GetEntitiesBy(closedPeriod);
           
            //TODO: Fix me
            //query.FetchMany(sa => sa.Zustandsabschnitten).ThenFetch(za => za.MassnahmenvorschlagFahrbahn).Fetch(sa => sa.Belastungskategorie).ToList();
            //query.FetchMany(sa => sa.Zustandsabschnitten).ThenFetch(za => za.MassnahmenvorschlagTrottoirLinks).ToList();
            //query.FetchMany(sa => sa.Zustandsabschnitten).ThenFetch(za => za.MassnahmenvorschlagTrottoirRechts).ToList();
            //query.FetchMany(sa => sa.Zustandsabschnitten).ThenFetch(za => za.Schadendetails).ToList();
            //query.FetchMany(sa => sa.Zustandsabschnitten).ThenFetch(za => za.Schadengruppen).ToList();

            foreach (var st in query.ToList())
                strassenabschnittService.CreateCopy(st);
        }

        private void DeleteNotUsedData(Guid closedPeriodId)
        {
            var closedPeriod = erfassungsPeriodService.GetEntityById(closedPeriodId);
            switch (closedPeriod.NetzErfassungsmodus)
            {
                case NetzErfassungsmodus.Summarisch:
                    DeleteStrassenabschnitteData(closedPeriod);
                    DeleteStrassenabschnitteGISData(closedPeriod);
                    break;
                case NetzErfassungsmodus.Tabellarisch:
                    DeleteSummarischData(closedPeriod);
                    DeleteStrassenabschnitteGISData(closedPeriod);
                    CleanUpZustandsabschnitt(closedPeriod);
                    break;
                case NetzErfassungsmodus.Gis:
                    DeleteSummarischData(closedPeriod);
                    DeleteStrassenabschnitteData(closedPeriod);
                    CleanUpZustandsabschnittGIS(closedPeriod);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CleanUpZustandsabschnittGIS(ErfassungsPeriod closedPeriod)
        {
            trottoirZustandGisService.DeleteUnusedData(closedPeriod);
        }

        private void CleanUpZustandsabschnitt(ErfassungsPeriod closedPeriod)
        {
            trottoirZustandService.DeleteUnusedData(closedPeriod);
        }

        private void DeleteSummarischData(ErfassungsPeriod closedPeriod)
        {
            foreach (var netzSummarisch in netzSummarischService.GetEntitiesBy(closedPeriod))
            {
                netzSummarisch.MittleresErhebungsJahr = null;
                foreach (var netzSummarischDetail in netzSummarisch.NetzSummarischDetails)
                {
                    netzSummarischDetail.MittlererZustand = null;
                    netzSummarischDetail.Fahrbahnlaenge = 0;
                    netzSummarischDetail.Fahrbahnflaeche = 0;
                }
            }

            foreach (var realisierteMassnahmeSummarsich in realisierteMassnahmeSummarsichService.GetEntitiesBy(closedPeriod))
                transactionScopeProvider.CurrentTransactionScope.Session.Delete(realisierteMassnahmeSummarsich);
        }

        private void DeleteStrassenabschnitteData(ErfassungsPeriod closedPeriod)
        {
            foreach (var strassenabschnitt in strassenabschnittService.GetEntitiesBy(closedPeriod))
                transactionScopeProvider.CurrentTransactionScope.Session.Delete(strassenabschnitt);

            foreach (var realisierteMassnahme in realisierteMassnahmeService.GetEntitiesBy(closedPeriod))
                transactionScopeProvider.CurrentTransactionScope.Session.Delete(realisierteMassnahme);
        }

        private void DeleteStrassenabschnitteGISData(ErfassungsPeriod closedPeriod)
        {
            foreach (var inspektionsrouteGIS in inspektionsRouteGISService.GetEntitiesBy(closedPeriod))
                transactionScopeProvider.CurrentTransactionScope.Session.Delete(inspektionsrouteGIS);

            foreach (var strassenabschnittGIS in strassenabschnittGISService.GetEntitiesBy(closedPeriod))
                transactionScopeProvider.CurrentTransactionScope.Session.Delete(strassenabschnittGIS);
        }
    }
}
