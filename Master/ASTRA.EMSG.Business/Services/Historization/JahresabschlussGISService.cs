using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Business.Services.EntityServices;
using ASTRA.EMSG.Business.Models;
using NHibernate.Criterion;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using NHibernate;
using GeoAPI.Geometries;
using ASTRA.EMSG.Common.Master.Logging;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Business.Entities.Katalogs;
using System.Diagnostics;

namespace ASTRA.EMSG.Business.Services.Historization
{
    public interface IJahresabschlussGISService : IService
    {
        void CopyGisData(ErfassungsPeriod closedPeriod);
    }

    public class JahresabschlussGISService : MandantDependentEntityServiceBase<Achse, AchsenModel>, IJahresabschlussGISService
    {
        private readonly IErfassungsPeriodService erfassungsPeriodService;
        private readonly IStrassenabschnittGISService strassenabschnittGISService;
        private readonly IMassnahmenvorschlagCopyService massnahmenvorschlagCopyService;
        private readonly IZustandsabschnittGISService zustandsabschnittGISService;
        private readonly IServerConfigurationProvider serverConfigurationProvider;

        public JahresabschlussGISService(ITransactionScopeProvider transactionScopeProvider, 
            IEntityServiceMappingEngine entityServiceMappingEngine,
            ISecurityService securityService,
            IErfassungsPeriodService erfassungsPeriodService,
            IStrassenabschnittGISService strassenabschnittGISService,
            IMassnahmenvorschlagCopyService massnahmenvorschlagCopyService,
            IServerConfigurationProvider serverConfigurationProvider,
            IZustandsabschnittGISService zustandsabschnittGISService)
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService)
        {
            this.erfassungsPeriodService = erfassungsPeriodService;
            this.strassenabschnittGISService = strassenabschnittGISService;
            this.massnahmenvorschlagCopyService = massnahmenvorschlagCopyService;
            this.serverConfigurationProvider = serverConfigurationProvider;
            this.zustandsabschnittGISService = zustandsabschnittGISService;
        }

        #region Type safe HQL strings

        #region Type names
        //TypeNames
        private string achseTypeName = typeof(Achse).Name;
        private string achssegTypeName = typeof(AchsenSegment).Name;
        private string sektorTypeName = typeof(Sektor).Name;
        private string achsrefTypeName = typeof(AchsenReferenz).Name;
        private string refGruppeTypeName = typeof(ReferenzGruppe).Name;
        private string schadDetTypeName = typeof(Schadendetail).Name;
        private string schadGruppeTypeName = typeof(Schadengruppe).Name;
        private string straGisTypeName = typeof(StrassenabschnittGIS).Name;
        private string zabGisTypeName = typeof(ZustandsabschnittGIS).Name;
        private string realMassGisTypeName = typeof(RealisierteMassnahmeGIS).Name;
        private string koorMassGisTypeName = typeof(KoordinierteMassnahmeGIS).Name;
        private string MassTeilGisTypeName = typeof(MassnahmenvorschlagTeilsystemeGIS).Name;
        #endregion Type names

        #region property Names

        #region Achse Property Names
        //Achse Properties
        private string achseIdPropName = ExpressionHelper.GetPropertyName<Achse, Guid>(a => a.Id);
        private string achseBsidPropName = ExpressionHelper.GetPropertyName<Achse, Guid>(a => a.BsId);
        private string achseValidFromPropName = ExpressionHelper.GetPropertyName<Achse, DateTime>(a => a.VersionValidFrom);
        private string achseNamePropName = ExpressionHelper.GetPropertyName<Achse, string>(a => a.Name);
        private string achseOperationPropName = ExpressionHelper.GetPropertyName<Achse, int>(a => a.Operation);
        private string achseImpnrPropName = ExpressionHelper.GetPropertyName<Achse, int>(a => a.ImpNr);
        private string achseMandantPropName = ExpressionHelper.GetPropertyName<Achse, Mandant>(a => a.Mandant);
        private string achseErfassungsPeriodPropName = ExpressionHelper.GetPropertyName<Achse, ErfassungsPeriod>(a => a.ErfassungsPeriod);
        private string achseCopdiedFromPropName = ExpressionHelper.GetPropertyName<Achse, Achse>(a => a.CopiedFrom);
        #endregion Achse Property Names

        #region AchsenSegment Properties
        //AchsenSegment Properties
        private string achssegIdPropName = ExpressionHelper.GetPropertyName<AchsenSegment, Guid>(a => a.Id);
        private string achssegBsIdPropName = ExpressionHelper.GetPropertyName<AchsenSegment, Guid>(a => a.BsId);
        private string achssegOperationPropName = ExpressionHelper.GetPropertyName<AchsenSegment, int>(a => a.Operation);
        private string achssegNamePropName = ExpressionHelper.GetPropertyName<AchsenSegment, string>(a => a.Name);
        private string achssegSequencePropName = ExpressionHelper.GetPropertyName<AchsenSegment, int>(a => a.Sequence);
        private string achssegImpNrPropName = ExpressionHelper.GetPropertyName<AchsenSegment, int>(a => a.ImpNr);
        private string achssegShapePropName = ExpressionHelper.GetPropertyName<AchsenSegment, IGeometry>(a => a.Shape);
        private string achssegShape4dPropName = ExpressionHelper.GetPropertyName<AchsenSegment, IGeometry>(a => a.Shape4d);
        private string achssegVersionPropName = ExpressionHelper.GetPropertyName<AchsenSegment, int?>(a => a.Version);
        private string achssegAchsenIdPropName = ExpressionHelper.GetPropertyName<AchsenSegment, Guid>(a => a.AchsenId);
        private string achssegAchsPropName = ExpressionHelper.GetPropertyName<AchsenSegment, Achse>(a => a.Achse);
        private string achssegErfPeriodPropName = ExpressionHelper.GetPropertyName<AchsenSegment, ErfassungsPeriod>(a => a.ErfassungsPeriod);
        private string achssegMandantPropName = ExpressionHelper.GetPropertyName<AchsenSegment, Mandant>(a => a.Mandant);
        private string achssegCopdiedFromPropName = ExpressionHelper.GetPropertyName<AchsenSegment, AchsenSegment>(a => a.CopiedFrom);
        private string achssegIsInvertedPropName = ExpressionHelper.GetPropertyName<AchsenSegment, bool>(a => a.IsInverted);
        #endregion AchsenSegment Properties

        #region Sektor Properties
        private string sektIdPropName = ExpressionHelper.GetPropertyName<Sektor, Guid>(a => a.Id);
        private string sektBsIdPropName = ExpressionHelper.GetPropertyName<Sektor, Guid>(a => a.BsId);
        private string sektKmPropName = ExpressionHelper.GetPropertyName<Sektor, double>(a => a.Km);
        private string sektLengthPropName = ExpressionHelper.GetPropertyName<Sektor, double>(a => a.SectorLength);
        private string sektNamePropName = ExpressionHelper.GetPropertyName<Sektor, string>(a => a.Name);
        private string sektSequencePropName = ExpressionHelper.GetPropertyName<Sektor, double>(a => a.Sequence);
        private string sektMarkerPropName = ExpressionHelper.GetPropertyName<Sektor, IGeometry>(a => a.MarkerGeom);
        private string sektOperationPropName = ExpressionHelper.GetPropertyName<Sektor, int>(a => a.Operation);
        private string sektImpNrPropName = ExpressionHelper.GetPropertyName<Sektor, int>(a => a.ImpNr);
        private string sektAchsSegPropName = ExpressionHelper.GetPropertyName<Sektor, AchsenSegment>(a => a.AchsenSegment);
        private string sektCopyFromPropName = ExpressionHelper.GetPropertyName<Sektor, Sektor>(a => a.CopiedFrom);
        #endregion Sektor Properties

        #region ReferenzGruppe Properties
        private string refGruppeIdPropName = ExpressionHelper.GetPropertyName<ReferenzGruppe, Guid>(rg => rg.Id);
        private string refGruppeCopyFromPropName = ExpressionHelper.GetPropertyName<ReferenzGruppe, ReferenzGruppe>(rg => rg.CopiedFrom);
        #endregion ReferenzGruppe Properties

        #region AchsenReferenz Properties
        private string achsRefIdPropName = ExpressionHelper.GetPropertyName<AchsenReferenz, Guid>(a => a.Id);
        private string achsRefStrassenNamePropName = ExpressionHelper.GetPropertyName<AchsenReferenz, string>(a => a.Strassenname);
        private string achsRefVersionPropName = ExpressionHelper.GetPropertyName<AchsenReferenz, int?>(a => a.Version);
        private string achsRefVonRbbsPropName = ExpressionHelper.GetPropertyName<AchsenReferenz, int?>(a => a.VonRBBS);
        private string achsRefNachRbbsPropName = ExpressionHelper.GetPropertyName<AchsenReferenz, int?>(a => a.NachRBBS);
        private string achsRefShapePropName = ExpressionHelper.GetPropertyName<AchsenReferenz, IGeometry>(a => a.Shape);
        private string achsRefAchsSegPropName = ExpressionHelper.GetPropertyName<AchsenReferenz, AchsenSegment>(a => a.AchsenSegment);
        private string achsRefRefGruppePropName = ExpressionHelper.GetPropertyName<AchsenReferenz, ReferenzGruppe>(a => a.ReferenzGruppe);
        private string achsRefCopiedFromPropName = ExpressionHelper.GetPropertyName<AchsenReferenz, AchsenReferenz>(a => a.CopiedFrom);
        #endregion AchsenReferenz Properties

        #region StrassenabschnittGIS Properties
        private string straGisIdPropName = ExpressionHelper.GetPropertyName<StrassenabschnittGIS, Guid>(stg => stg.Id);
        private string straGisNamePropName = ExpressionHelper.GetPropertyName<StrassenabschnittGIS, string>(stg => stg.Strassenname);
        private string straGisBezVonPropName = ExpressionHelper.GetPropertyName<StrassenabschnittGIS, string>(stg => stg.BezeichnungVon);
        private string straGisBezBisPropName = ExpressionHelper.GetPropertyName<StrassenabschnittGIS, string>(stg => stg.BezeichnungBis);
        private string straGisBelagPropName = ExpressionHelper.GetPropertyName<StrassenabschnittGIS, BelagsTyp>(stg => stg.Belag);
        private string straGisLaengePropName = ExpressionHelper.GetPropertyName<StrassenabschnittGIS, decimal>(stg => stg.Laenge);
        private string straGisBreiteFBPropName = ExpressionHelper.GetPropertyName<StrassenabschnittGIS, decimal>(stg => stg.BreiteFahrbahn);
        private string straGisTrottPropName = ExpressionHelper.GetPropertyName<StrassenabschnittGIS, TrottoirTyp>(stg => stg.Trottoir);
        private string straGisBreiteTrLPropName = ExpressionHelper.GetPropertyName<StrassenabschnittGIS, decimal?>(stg => stg.BreiteTrottoirLinks);
        private string straGisBreiteTrRPropName = ExpressionHelper.GetPropertyName<StrassenabschnittGIS, decimal?>(stg => stg.BreiteTrottoirRechts);
        private string straGisEigenTuemerPropName = ExpressionHelper.GetPropertyName<StrassenabschnittGIS, EigentuemerTyp>(stg => stg.Strasseneigentuemer);
        private string straGisOrtsBezPropName = ExpressionHelper.GetPropertyName<StrassenabschnittGIS, string>(stg => stg.Ortsbezeichnung);
        private string straGisShapePropName = ExpressionHelper.GetPropertyName<StrassenabschnittGIS, IGeometry>(stg => stg.Shape);
        private string straGisIsLockedPropName = ExpressionHelper.GetPropertyName<StrassenabschnittGIS, bool>(stg => stg.IsLocked);
        private string straGisBLKPropName = ExpressionHelper.GetPropertyName<StrassenabschnittGIS, Belastungskategorie>(stg => stg.Belastungskategorie);
        private string straGisMandantPropName = ExpressionHelper.GetPropertyName<StrassenabschnittGIS, Mandant>(stg => stg.Mandant);
        private string straGisErfPeriodPropName = ExpressionHelper.GetPropertyName<StrassenabschnittGIS, ErfassungsPeriod>(stg => stg.ErfassungsPeriod);
        private string straGisRefGruppePropName = ExpressionHelper.GetPropertyName<StrassenabschnittGIS, ReferenzGruppe>(stg => stg.ReferenzGruppe);
        private string straGisCopyFromPropName = ExpressionHelper.GetPropertyName<StrassenabschnittGIS, StrassenabschnittGIS>(stg => stg.CopiedFrom);
        #endregion StrassenabschnittGIS Properties

        #region ZustandsabschnittGIS Properties
        private string zabGisIdPropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, Guid>(zsg => zsg.Id);
        private string zabGisIndexPropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, decimal>(zsg => zsg.Zustandsindex);
        private string zabGisBezVonPropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, string>(zsg => zsg.BezeichnungVon);
        private string zabGisBezBisPropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, string>(zsg => zsg.BezeichnungBis);
        private string zabGisErfssungsMPropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, ZustandsErfassungsmodus>(zsg => zsg.Erfassungsmodus);
        private string zabGisLaengePropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, decimal>(zsg => zsg.Laenge);
        private string zabGisAufnahmeDatPropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, DateTime>(zsg => zsg.Aufnahmedatum);
        private string zabGisAufnahmeTeamPropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, string>(zsg => zsg.Aufnahmeteam);
        private string zabGisWetterPropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, WetterTyp>(zsg => zsg.Wetter);
        private string zabGisBemerkungPropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, string>(zsg => zsg.Bemerkung);
        private string zabGisZstIndTrLPropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, ZustandsindexTyp>(zsg => zsg.ZustandsindexTrottoirLinks);
        private string zabGisZstIndTrRPropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, ZustandsindexTyp>(zsg => zsg.ZustandsindexTrottoirRechts);
        private string zabGisFBKostenPropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, decimal?>(zsg => zsg.KostenMassnahmenvorschlagFahrbahn);
        private string zabGisFBDringlichPropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, DringlichkeitTyp>(zsg => zsg.DringlichkeitFahrbahn);
        private string zabGisTrRKostenPropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, decimal?>(zsg => zsg.KostenMassnahmenvorschlagTrottoirRechts);
        private string zabGisTrLKostenPropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, decimal?>(zsg => zsg.KostenMassnahmenvorschlagTrottoirLinks);
        private string zabGisTrRDringlichPropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, DringlichkeitTyp>(zsg => zsg.DringlichkeitTrottoirRechts);
        private string zabGisTrLDringlichPropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, DringlichkeitTyp>(zsg => zsg.DringlichkeitTrottoirLinks);
        private string zabGisShapePropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, IGeometry>(zsg => zsg.Shape);
        private string zabGisMvkFbPropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, MassnahmenvorschlagKatalog>(zsg => zsg.MassnahmenvorschlagFahrbahn);
        private string zabGisMvkTrRPropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, MassnahmenvorschlagKatalog>(zsg => zsg.MassnahmenvorschlagTrottoirRechts);
        private string zabGisMvkTrLPropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, MassnahmenvorschlagKatalog>(zsg => zsg.MassnahmenvorschlagTrottoirLinks);
        private string zabGisStrabPropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, StrassenabschnittGIS>(zsg => zsg.StrassenabschnittGIS);
        private string zabGisRefGruppePropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, ReferenzGruppe>(zsg => zsg.ReferenzGruppe);
        private string zabGisCopyFromPropName = ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, ZustandsabschnittGIS>(zsg => zsg.CopiedFrom);
        #endregion ZustandsabschnittGIS Properties

        #region RealisierteMassnahmeGIS Properties
        private string realMassGisIdPropName = ExpressionHelper.GetPropertyName<RealisierteMassnahmeGIS, Guid>(rmg => rmg.Id);
        private string realMassGisRefGruppePropName = ExpressionHelper.GetPropertyName<RealisierteMassnahmeGIS, ReferenzGruppe>(rmg => rmg.ReferenzGruppe);
        private string realMassGisErfPeriodPropName = ExpressionHelper.GetPropertyName<RealisierteMassnahmeGIS, ErfassungsPeriod>(rmg => rmg.ErfassungsPeriod);
        private string realMassGisMandantPropName = ExpressionHelper.GetPropertyName<RealisierteMassnahmeGIS, Mandant>(rmg => rmg.Mandant);
        #endregion RealisierteMassnahmeGIS Properties

        #region KoordinierteMassnahmeGIS Properties
        private string koormassgisIdPropName = ExpressionHelper.GetPropertyName<KoordinierteMassnahmeGIS, Guid>(kmg => kmg.Id);
        private string koormassgisRefGruppePropName = ExpressionHelper.GetPropertyName<KoordinierteMassnahmeGIS, ReferenzGruppe>(kmg => kmg.ReferenzGruppe);
        private string koormassgisMandantPropName = ExpressionHelper.GetPropertyName<KoordinierteMassnahmeGIS, Mandant>(kmg => kmg.Mandant);
        #endregion KoordinierteMassnahmeGIS Properties

        #region MassnahmenvorschlagTeilsystemeGIS Properties
        private string massTeilGisIdPropName = ExpressionHelper.GetPropertyName<MassnahmenvorschlagTeilsystemeGIS, Guid>(mtg => mtg.Id);
        private string massTeilGisRefGruppePropName = ExpressionHelper.GetPropertyName<MassnahmenvorschlagTeilsystemeGIS, ReferenzGruppe>(mtg => mtg.ReferenzGruppe);
        private string massTeilGisMandantPropName = ExpressionHelper.GetPropertyName<MassnahmenvorschlagTeilsystemeGIS, Mandant>(mtg => mtg.Mandant);
        #endregion MassnahmenvorschlagTeilsystemeGIS Properties

        #region SchadenDetail Properties
        private string schadDetIdPropName = ExpressionHelper.GetPropertyName<Schadendetail, Guid>(scd => scd.Id);
        private string schadDetTypPropName = ExpressionHelper.GetPropertyName<Schadendetail, SchadendetailTyp>(scd => scd.SchadendetailTyp);
        private string schadDetSchwerePropName = ExpressionHelper.GetPropertyName<Schadendetail, SchadenschwereTyp>(scd => scd.SchadenschwereTyp);
        private string schadDetAusmassPropName = ExpressionHelper.GetPropertyName<Schadendetail, SchadenausmassTyp>(scd => scd.SchadenausmassTyp);
        #endregion SchadenDetail Properties

        #endregion property Names

        #endregion Type safe HQL strings

        protected override Expression<Func<Achse, Mandant>> MandantExpression { get { return sGis => sGis.Mandant; } }

        public void CopyGisData(ErfassungsPeriod closedPeriod)
        {

            Stopwatch stopwatch = new Stopwatch();
            Loggers.TechLogger.Debug("Starting JahresabschlussGis");
            stopwatch.Start();
            ErfassungsPeriod currentErfassungsperiod = erfassungsPeriodService.GetCurrentErfassungsPeriod();



            var achsQuery = getAchsQuery(currentErfassungsperiod, closedPeriod);
            
            var achsSegQuery = getAchsSegQuery(currentErfassungsperiod, closedPeriod);
            
            var sektorQuery = getSektorQuery(closedPeriod);

            var refGruppeQueryStg = getRefGruppeQuerystg(closedPeriod);
            var refGruppeQueryZsg = getRefGruppeQueryZsg(closedPeriod);
            //var refGruppeQueryRmg = getRefGruppeQueryRmg(closedPeriod);
            var refGruppeQueryKmg = getRefGruppeQueryKmg();
            var refGruppeQueryMtg = getRefGruppeQueryMtg();

            var achsRefQuery = getAchsRefQuery(closedPeriod);

            var straGisQuery = getStraGisQuery(closedPeriod, currentErfassungsperiod);

            var zabGisQuery = getZabGisQuery(closedPeriod);

            var massTeilGisQuery = getMassTeilGisQuery();

            var koorMassGisQuery = getKoorMassGisQuery();

            


            TimeSpan achscopyStart = stopwatch.Elapsed;
            int achsCopyCount = achsQuery.ExecuteUpdate();
            TimeSpan achscopyEnd = stopwatch.Elapsed;
            Loggers.PeformanceLogger.Debug("Copied " + achsCopyCount + " Achsen in " + (achscopyEnd-achscopyStart).ToString());

            TimeSpan achsSegcopyStart = stopwatch.Elapsed;
            int achsSegCopyCount = achsSegQuery.ExecuteUpdate();
            TimeSpan achsSegcopyEnd = stopwatch.Elapsed;
            Loggers.PeformanceLogger.Debug("Copied " + achsSegCopyCount + " AchsenSegmente in " + (achsSegcopyEnd - achsSegcopyStart).ToString());

            TimeSpan sektorCopyStart = stopwatch.Elapsed;
            int sektorCopyCount = sektorQuery.ExecuteUpdate();
            TimeSpan sektorCopyEnd = stopwatch.Elapsed;
            Loggers.PeformanceLogger.Debug("Copied " + sektorCopyCount + " Sektoren in " + (sektorCopyEnd - sektorCopyStart).ToString());

            TimeSpan refGruppeCopyStart = stopwatch.Elapsed;
            int refGruppeCopyCount = 0;
            refGruppeCopyCount += refGruppeQueryKmg.ExecuteUpdate();
            refGruppeCopyCount += refGruppeQueryMtg.ExecuteUpdate();
            //refGruppeCopyCount += refGruppeQueryRmg.ExecuteUpdate();
            refGruppeCopyCount += refGruppeQueryStg.ExecuteUpdate();
            refGruppeCopyCount += refGruppeQueryZsg.ExecuteUpdate();
            TimeSpan refGruppeCopyEnd = stopwatch.Elapsed;
            Loggers.PeformanceLogger.Debug("Copied " + refGruppeCopyCount + " ReferenzGruppen in " + (refGruppeCopyEnd - refGruppeCopyStart).ToString());

            TimeSpan achsRefCopyStart = stopwatch.Elapsed;
            
            int achsRefCopyCount = achsRefQuery.ExecuteUpdate();
            TimeSpan achsRefCopyEnd = stopwatch.Elapsed;
            Loggers.PeformanceLogger.Debug("Copied " + achsRefCopyCount + " AchsenReferenzen in " + (achsRefCopyEnd - achsRefCopyStart).ToString());

            TimeSpan straGisCopyStart = stopwatch.Elapsed;
            int straGisCopyCount = straGisQuery.ExecuteUpdate();
            TimeSpan straGisCopyEnd = stopwatch.Elapsed;
            Loggers.PeformanceLogger.Debug("Copied " + straGisCopyCount + " StrassenabschnittGis in " + (straGisCopyEnd - straGisCopyStart).ToString());

            TimeSpan zabGisCopyStart = stopwatch.Elapsed;
            int zabGisCopyCount = zabGisQuery.ExecuteUpdate();
            TimeSpan zabGisCopyEnd = stopwatch.Elapsed;
            Loggers.PeformanceLogger.Debug("Copied " + zabGisCopyCount + " ZustandsabschnittGis in " + (zabGisCopyEnd - zabGisCopyStart).ToString());

            TimeSpan massTeilGisUpdateStart = stopwatch.Elapsed;
            int massTeilGisUpdateCount = massTeilGisQuery.ExecuteUpdate();
            TimeSpan massTeilGisUpdateEnd = stopwatch.Elapsed;
            Loggers.PeformanceLogger.Debug("Updated " + massTeilGisUpdateCount + " MassnahmeTeilsystemGis in " + (massTeilGisUpdateEnd - massTeilGisUpdateStart).ToString());

            TimeSpan koorMassGisUpdateStart = stopwatch.Elapsed;
            int koorMassGisUpdateCount = koorMassGisQuery.ExecuteUpdate();
            TimeSpan koorMassGisUpdateEnd = stopwatch.Elapsed;
            Loggers.PeformanceLogger.Debug("Updated " + koorMassGisUpdateCount + " MassnahmeTeilsystemGis in " + (koorMassGisUpdateEnd - koorMassGisUpdateStart).ToString());

            TimeSpan ZustandsabschnitteGisUpdateStart = stopwatch.Elapsed;
            int ZustandsabschnitteGisUpdateCount = updateZustandsabschnitteGis(closedPeriod);
            TimeSpan ZustandsabschnitteGisUpdateEnd = stopwatch.Elapsed;
            Loggers.PeformanceLogger.Debug("Updated " + ZustandsabschnitteGisUpdateCount + " ZustandsabschnittGIS in " + (ZustandsabschnitteGisUpdateEnd - ZustandsabschnitteGisUpdateStart).ToString());

            
            
            stopwatch.Stop() ;
                        
        }

        private IQuery getAchsQuery(ErfassungsPeriod currentErfassungsperiod, ErfassungsPeriod closedPeriod)
        {
            string achsQuerystring = String.Format(
              "insert into {0}({1},{2},{3},{4},{5},{6},{7},{8},{9}) " +
              "Select guid(),a.{2},a.{3},a.{4},a.{5},a.{6},a.{7},:newErfPeriod,a " +
              "FROM {0} a " +
              "where a.{8} =:oldErfPeriod"
                , new string[] { 
                    achseTypeName, achseIdPropName, achseBsidPropName, achseValidFromPropName, 
                    achseNamePropName, achseOperationPropName, achseImpnrPropName, achseMandantPropName, 
                    achseErfassungsPeriodPropName, achseCopdiedFromPropName 
                });
            var achsQuery = CurrentSession.CreateQuery(achsQuerystring);
            achsQuery.SetParameter("newErfPeriod", currentErfassungsperiod);
            achsQuery.SetParameter("oldErfPeriod", closedPeriod);

            return achsQuery;
                       
        }

        private IQuery getAchsSegQuery(ErfassungsPeriod currentErfassungsperiod, ErfassungsPeriod closedPeriod)
        {
            string achssegQueryString = String.Format(
                "insert into {0} ({1},{2},{3},{4},{5},{6},{7},{9},{10},{11},{12},{13},{14}, {18}) " +
                "SELECT guid(),acs.{2},acs.{3},acs.{4},acs.{5},acs.{6},acs.{7},acs.{9},acs.{10},ach,:newErfPeriod,acs.{13},acs, acs.{18} " +
                "FROM {0} acs, {15} ach " +
                "where acs.{12} =:oldErfPeriod and ach.{17} = acs.{11}",
                new string[] { 
                    achssegTypeName, achssegIdPropName, achssegBsIdPropName, achssegOperationPropName,                              //0-3
                    achssegNamePropName,achssegSequencePropName,achssegImpNrPropName,achssegShapePropName,achssegShape4dPropName,   //4-8
                    achssegVersionPropName,achssegAchsenIdPropName,achssegAchsPropName,achssegErfPeriodPropName,                    //9-12
                    achssegMandantPropName,achssegCopdiedFromPropName, achseTypeName, achseIdPropName,                              //13-16
                    achseCopdiedFromPropName, achssegIsInvertedPropName
                });

            var achsSegQuery = CurrentSession.CreateQuery(achssegQueryString);
            achsSegQuery.SetParameter("newErfPeriod", currentErfassungsperiod);
            achsSegQuery.SetParameter("oldErfPeriod", closedPeriod);
            return achsSegQuery;
        }

        private IQuery getSektorQuery(ErfassungsPeriod closedPeriod)
        {
            string sektorQueryString = String.Format(
                    "insert into {0} ({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}) " +
                    "SELECT guid(),sek.{2},sek.{3},sek.{4},sek.{5},sek.{6},sek.{7},sek.{8},sek.{9}, acs2, sek " +
                    "FROM {0} sek, {12} acs1, {12} acs2 " +
                    "where sek.{10}.{13} =:oldErfPeriod and acs1.{15} = sek.{10}.{15} and acs1.{15} = acs2.{14}.{15}",
                    new string[] 
                { 
                    sektorTypeName,sektIdPropName,sektBsIdPropName,sektKmPropName,
                    sektLengthPropName,sektNamePropName,sektSequencePropName,sektMarkerPropName,
                    sektOperationPropName,sektImpNrPropName,sektAchsSegPropName,sektCopyFromPropName,
                    achssegTypeName,achssegErfPeriodPropName, achssegCopdiedFromPropName,achssegIdPropName
                });

            var sektorQuery = CurrentSession.CreateQuery(sektorQueryString);
            sektorQuery.SetParameter("oldErfPeriod", closedPeriod);
            return sektorQuery;
        }

        private IQuery getRefGruppeQuerystg(ErfassungsPeriod closedPeriod)
        {
            string refGruppeQueryStringStg = String.Format("insert into {0} ({1},{2}) " +
                   "SELECT guid(),rfg " +
                   "FROM {0} rfg , {3} stg Where rfg.{1} = stg.{4}.{1} and stg.{5} = :oldErfPeriod ",
                   new string[] {
                    refGruppeTypeName, refGruppeIdPropName,refGruppeCopyFromPropName, straGisTypeName,
                    straGisRefGruppePropName, straGisErfPeriodPropName
                });
            var refGruppeQuerystg = CurrentSession.CreateQuery(refGruppeQueryStringStg);
            refGruppeQuerystg.SetParameter("oldErfPeriod", closedPeriod);
            return refGruppeQuerystg;
        }

        private IQuery getRefGruppeQueryZsg(ErfassungsPeriod closedPeriod)
        {
            string refGruppeQueryStringZsg = String.Format("insert into {0} ({1},{2}) " +
                    "SELECT guid(),rfg " +
                    "FROM {0} rfg , {3} zsg " +
                    "Where rfg.{1} = zsg.{4}.{1} and zsg.{5}.{6} = :oldErfPeriod",
                    new string[] {
                    refGruppeTypeName, refGruppeIdPropName,refGruppeCopyFromPropName, zabGisTypeName,
                    zabGisRefGruppePropName,zabGisStrabPropName, straGisErfPeriodPropName
                });
            var refGruppeQueryZsg = CurrentSession.CreateQuery(refGruppeQueryStringZsg);
            refGruppeQueryZsg.SetParameter("oldErfPeriod", closedPeriod);
            return refGruppeQueryZsg;
        }

        private IQuery getRefGruppeQueryRmg(ErfassungsPeriod closedPeriod)
        {
            string refGruppeQueryStringRmg = String.Format("insert into {0} ({1},{2}) " +
                    "SELECT guid(),rfg " +
                    "FROM {0} rfg , {3} rmg " +
                    "Where rfg.{1} = rmg.{4}.{1} and rmg.{5} = :oldErfPeriod ",
                    new string[] {
                    refGruppeTypeName, refGruppeIdPropName,refGruppeCopyFromPropName, realMassGisTypeName,
                    realMassGisRefGruppePropName, realMassGisErfPeriodPropName
                });
            var refGruppeQueryRmg = CurrentSession.CreateQuery(refGruppeQueryStringRmg);
            refGruppeQueryRmg.SetParameter("oldErfPeriod", closedPeriod);
            return refGruppeQueryRmg;
        }

        private IQuery getRefGruppeQueryKmg()
        {
            string refGruppeQueryStringKmg = String.Format("insert into {0} ({1},{2}) " +
                  "SELECT guid(),rfg " +
                  "FROM {0} rfg , {3} kmg " +
                  "Where rfg.{1} = kmg.{4}.{1} and kmg.{5} = :Mandant ",
                  new string[] {
                    refGruppeTypeName, refGruppeIdPropName,refGruppeCopyFromPropName, koorMassGisTypeName,
                    koormassgisRefGruppePropName, koormassgisMandantPropName
                });
            var refGruppeQueryKmg = CurrentSession.CreateQuery(refGruppeQueryStringKmg);
            refGruppeQueryKmg.SetParameter("Mandant", CurrentMandant);
            return refGruppeQueryKmg;
        }

        private IQuery getRefGruppeQueryMtg()
        {
            string refGruppeQueryStringMtg = String.Format("insert into {0} ({1},{2}) " +
                  "SELECT guid(),rfg " +
                  "FROM {0} rfg , {3} mtg " +
                  "Where rfg.{1} = mtg.{4}.{1} and mtg.{5} = :Mandant ",
                  new string[] {
                    refGruppeTypeName, refGruppeIdPropName,refGruppeCopyFromPropName, MassTeilGisTypeName,
                    massTeilGisRefGruppePropName, massTeilGisMandantPropName
                });

            var refGruppeQueryMtg = CurrentSession.CreateQuery(refGruppeQueryStringMtg);
            refGruppeQueryMtg.SetParameter("Mandant", CurrentMandant);
            return refGruppeQueryMtg;
        }

        private IQuery getAchsRefQuery(ErfassungsPeriod closedPeriod)
        {
            string achsRefQueryString = String.Format("insert into {0} ({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}) " +
                    "SELECT guid(), acr.{2}, acr.{3}, acr.{4}, acr.{5}, acr.{6}, acs, rfg, acr " +
                    "FROM {0} acr join acr.{7} acs2,{10} acs,{11} rfg " +
                    "WHERE acs.{12} = acr.{7} and rfg.{13} = acr.{8} and acs2.{14} = :oldErfPeriod",
                    new string[] {
                    achsrefTypeName, achsRefIdPropName, achsRefStrassenNamePropName, achsRefVersionPropName,
                    achsRefVonRbbsPropName, achsRefNachRbbsPropName, achsRefShapePropName, achsRefAchsSegPropName,
                    achsRefRefGruppePropName, achsRefCopiedFromPropName, achssegTypeName, refGruppeTypeName, 
                    achssegCopdiedFromPropName, refGruppeCopyFromPropName, achseErfassungsPeriodPropName
                });

            var achsRefQuery = CurrentSession.CreateQuery(achsRefQueryString);
            achsRefQuery.SetParameter("oldErfPeriod", closedPeriod);
            return achsRefQuery;
        }
        private IQuery getStraGisQuery(ErfassungsPeriod closedPeriod, ErfassungsPeriod currentErfassungsperiod )
        {
            string straGisQueryString = String.Format("insert into {0} ({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, " +
                "{11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}) " +
                "Select guid(), stg.{2}, stg.{3}, stg.{4}, stg.{5}, stg.{6}, stg.{7}, stg.{8}, stg.{9}, stg.{10}, " +
                "stg.{11}, stg.{12}, stg.{13}, stg.{14}, stg.{15}, stg.{16},:newErfPeriod, rfg, stg " +
                "From {0} stg, {20} rfg " +
                "Where stg.{17} = :oldErfPeriod and stg.{18} = rfg.{21}",
                new string[] 
            { 
                straGisTypeName, straGisIdPropName, straGisNamePropName, straGisBezVonPropName, 
                straGisBezBisPropName, straGisBelagPropName, straGisLaengePropName,  straGisBreiteFBPropName, 
                straGisTrottPropName, straGisBreiteTrLPropName, straGisBreiteTrRPropName, straGisEigenTuemerPropName, 
                straGisOrtsBezPropName, straGisShapePropName, straGisIsLockedPropName,  straGisBLKPropName, 
                straGisMandantPropName, straGisErfPeriodPropName, straGisRefGruppePropName, straGisCopyFromPropName, 
                refGruppeTypeName, refGruppeCopyFromPropName
            });

            var straGisQuery = CurrentSession.CreateQuery(straGisQueryString);
            straGisQuery.SetParameter("newErfPeriod", currentErfassungsperiod);
            straGisQuery.SetParameter("oldErfPeriod", closedPeriod);
            return straGisQuery;
        }

        private IQuery getZabGisQuery(ErfassungsPeriod closedPeriod)
        {
            string zabGisQueryString = String.Format("insert into {0} ({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, " +
            "{13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}, {23}, {24}, {25}) " +
            "Select guid(),zsg.{2}, zsg.{3}, zsg.{4}, zsg.{5}, zsg.{6}, zsg.{7}, zsg.{8}, zsg.{9}, zsg.{10}, zsg.{11}, zsg.{12}, " +
            "zsg.{13}, zsg.{14}, zsg.{15}, zsg.{16}, zsg.{17}, zsg.{18}, zsg.{19}, zsg.{20}, zsg.{21}, zsg.{22}, stg, rfg, zsg " +
            "From {0} zsg, {26} stg, {27} rfg " +
            "Where zsg.{23} = stg.{28} and zsg.{24} = rfg.{29} and zsg.{23}.{30}=:oldErfPeriod",
            new string[] 
            { 
                zabGisTypeName, zabGisIdPropName, zabGisIndexPropName, zabGisBezVonPropName,
                zabGisBezBisPropName, zabGisErfssungsMPropName, zabGisLaengePropName, zabGisAufnahmeDatPropName,
                zabGisAufnahmeTeamPropName, zabGisWetterPropName, zabGisBemerkungPropName, zabGisZstIndTrLPropName,
                zabGisZstIndTrRPropName, zabGisFBKostenPropName, zabGisFBDringlichPropName, zabGisTrRKostenPropName,
                zabGisTrRDringlichPropName, zabGisTrLKostenPropName, zabGisTrLDringlichPropName, zabGisShapePropName,
                zabGisMvkFbPropName, zabGisMvkTrRPropName, zabGisMvkTrLPropName, zabGisStrabPropName,
                zabGisRefGruppePropName, zabGisCopyFromPropName, straGisTypeName, refGruppeTypeName,
                straGisCopyFromPropName, refGruppeCopyFromPropName, straGisErfPeriodPropName
            });

            var zabGisQuery = CurrentSession.CreateQuery(zabGisQueryString);
            zabGisQuery.SetParameter("oldErfPeriod", closedPeriod);
            return zabGisQuery;
        }

        protected virtual IQuery getMassTeilGisQuery()
        {
            string massTeilGisQueryString = String.Format("Update {0} mtg " +
                    "set mtg.{3} =  (Select rfg From {1} rfg Where rfg.{2} = mtg.{3}) " +
                    "Where mtg.{4} = :Mandant",
                    new string[] 
                { 
                    MassTeilGisTypeName, refGruppeTypeName, refGruppeCopyFromPropName, massTeilGisRefGruppePropName,
                    massTeilGisMandantPropName
                });

            var massTeilGisQuery = CurrentSession.CreateQuery(massTeilGisQueryString);
            massTeilGisQuery.SetParameter("Mandant", CurrentMandant);
            return massTeilGisQuery;
        }

        protected virtual IQuery getKoorMassGisQuery()
        {
            string koorMassGisQueryString = String.Format("Update {0} kmg " +
                    "set kmg.{1} = (Select rfg from {2} rfg where rfg.{3} = kmg.{1}) " +
                    "Where kmg.{4}=:Mandant",
                    new string[] 
                {
                    koorMassGisTypeName, koormassgisRefGruppePropName, refGruppeTypeName, refGruppeCopyFromPropName,
                    koormassgisMandantPropName
                });

            var koorMassGisQuery = CurrentSession.CreateQuery(koorMassGisQueryString);
            koorMassGisQuery.SetParameter("Mandant", CurrentMandant);
            return koorMassGisQuery;
        }

       
        private int updateZustandsabschnitteGis(ErfassungsPeriod closedPeriod)
        {
            var newZabs = zustandsabschnittGISService.GetCurrentEntities();

            foreach (var zab in newZabs)
            {
                massnahmenvorschlagCopyService.CopyMassnahmenvorschlagen(zab, zab.CopiedFrom);

                foreach (Schadengruppe schadengruppe in zab.CopiedFrom.Schadengruppen)
                    zab.AddSchadengruppe(entityServiceMappingEngine.Translate<Schadengruppe, Schadengruppe>(schadengruppe));

                foreach (Schadendetail schadendetail in zab.CopiedFrom.Schadendetails)
                    zab.AddSchadendetail(entityServiceMappingEngine.Translate<Schadendetail, Schadendetail>(schadendetail));

                Update(zab);
            }
            return newZabs.Count();
        }

       
    }

    public class TestJahresabschlussGISService : JahresabschlussGISService
    {
        public TestJahresabschlussGISService(ITransactionScopeProvider transactionScopeProvider, IEntityServiceMappingEngine entityServiceMappingEngine, ISecurityService securityService, IErfassungsPeriodService erfassungsPeriodService, IStrassenabschnittGISService strassenabschnittGISService, IMassnahmenvorschlagCopyService massnahmenvorschlagCopyService, IServerConfigurationProvider serverConfigurationProvider, IZustandsabschnittGISService zustandsabschnittGISService) : base(transactionScopeProvider, entityServiceMappingEngine, securityService, erfassungsPeriodService, strassenabschnittGISService, massnahmenvorschlagCopyService, serverConfigurationProvider, zustandsabschnittGISService)
        {
        }

        protected override IQuery getMassTeilGisQuery()
        {
            //Dummy update for the integration tests: SQLCE does not support subquery in Update statements
            string massTeilGisQueryString = String.Format("Update {0} set Id = guid() Where Id = guid()", typeof(MassnahmenvorschlagTeilsystemeGIS).Name);
            var massTeilGisQuery = CurrentSession.CreateQuery(massTeilGisQueryString);
            return massTeilGisQuery;
        }

        protected override IQuery getKoorMassGisQuery()
        {
            //Dummy update for the integration tests: SQLCE does not support subquery in Update statements
            string massTeilGisQueryString = String.Format("Update {0} set Id = guid() Where Id = guid()", typeof(KoordinierteMassnahmeGIS).Name);
            var massTeilGisQuery = CurrentSession.CreateQuery(massTeilGisQueryString);
            return massTeilGisQuery;
        }
    }
}
