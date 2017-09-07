using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Entities.Summarisch;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.IntegrationTests.Support;
using ASTRA.EMSG.IntegrationTests.Support.MvcTesting;
using ASTRA.EMSG.Tests.Common.Utils;
using ASTRA.EMSG.Web.Areas.Auswertungen.Controllers;
using ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands;
using ASTRA.EMSG.Web.Areas.NetzverwaltungGIS.Controllers;
using ASTRA.EMSG.Web.Controllers;
using NHibernate.Linq;
using NUnit.Framework;

namespace ASTRA.EMSG.IntegrationTests.Common
{
    public abstract class ControllerTestBase : IntegrationTestBase
    {
        protected readonly Dictionary<NetzErfassungsmodus, Mandant> mandants = new Dictionary<NetzErfassungsmodus, Mandant>();

        public Dictionary<NetzErfassungsmodus, ErfassungsPeriod> ClosedErfassungPeriods { get; private set; }
        public Dictionary<NetzErfassungsmodus, ErfassungsPeriod> CurrentErfassungPeriods { get; private set; }


        protected override void DbInit()
        {
            ClosedErfassungPeriods = new Dictionary<NetzErfassungsmodus, ErfassungsPeriod>();
            CurrentErfassungPeriods = new Dictionary<NetzErfassungsmodus, ErfassungsPeriod>();

            using (var scope = new NHibernateTestScope())
            {
                foreach (NetzErfassungsmodus modus in Enum.GetValues(typeof (NetzErfassungsmodus)))
                {
                    var mandantName = modus.ToString() + "Mandant";
                    var mandant = DbHandlerUtils.CreateMandant(scope.Session, mandantName, "0", modus);

                    mandants[modus] = mandant;

                    ClosedErfassungPeriods[modus] = DbHandlerUtils.CreateErfassungsPeriod(scope.Session, mandant,
                                                                                            modus);
                    ClosedErfassungPeriods[modus].IsClosed = true;
                    ClosedErfassungPeriods[modus].Erfassungsjahr = new DateTime(2010, 1, 1);
                }

                DbHandlerUtils.CreateTestUser(scope.Session, TestUserName, mandants.Values, DbHandlerUtils.AllRollen);
            }

            PrepareData();
        }

        protected virtual void Init()
        {
            
        }

        

        private void PrepareData()
        {
            using (var scope = new NHibernateTestScope())
            {
                PrepareSummarischData(scope);
                PrepareTabellarischData(scope);
                PrepareGISData(scope);
            }
        }

        private void PrepareSummarischData(NHibernateTestScope scope)
        {
            CurrentErfassungPeriods[NetzErfassungsmodus.Summarisch] = scope.Session.Query<ErfassungsPeriod>().Fetch(ke => ke.Mandant).First(m => !m.IsClosed && mandants.Values.Contains(m.Mandant) && m.NetzErfassungsmodus == NetzErfassungsmodus.Summarisch);
            CurrentErfassungPeriods[NetzErfassungsmodus.Summarisch].Erfassungsjahr = new DateTime(2012, 1, 1);

            PrepareNetzSummarischData(scope);
            PrepareRealisierteMassnahmenSummarischData(scope);
        }

        private void PrepareTabellarischData(NHibernateTestScope scope)
        {
            CurrentErfassungPeriods[NetzErfassungsmodus.Tabellarisch] = scope.Session.Query<ErfassungsPeriod>().Fetch(ke => ke.Mandant).First(m => !m.IsClosed && mandants.Values.Contains(m.Mandant) && m.NetzErfassungsmodus == NetzErfassungsmodus.Tabellarisch);
            CurrentErfassungPeriods[NetzErfassungsmodus.Tabellarisch].Erfassungsjahr = new DateTime(2012, 1, 1);

            PrepareStrassenabschnittAndZustandabschnittTabellarischData(scope);
            PrepareRealisierteMassnahmenTabellarischData(scope);
        }

        private void PrepareGISData(NHibernateTestScope scope)
        {
            CurrentErfassungPeriods[NetzErfassungsmodus.Gis] = scope.Session.Query<ErfassungsPeriod>().Fetch(ke => ke.Mandant).First(m => !m.IsClosed && mandants.Values.Contains(m.Mandant) && m.NetzErfassungsmodus == NetzErfassungsmodus.Gis);
            CurrentErfassungPeriods[NetzErfassungsmodus.Gis].Erfassungsjahr = new DateTime(2012, 1, 1);

            PrepareStrassenabschnittGISData(scope);
            PrepareRealisierteMassnahmenGISData(scope);
            PrepareKoordinierteMassnahmenGISData(scope);
            PrepareMassnahmenDerTeilsystemGISData(scope);
        }

        private void PrepareNetzSummarischData(NHibernateTestScope scope)
        {
            var netzSummarisch = scope.Session.Query<NetzSummarisch>().Single(m => m.Mandant.MandantName == mandants[NetzErfassungsmodus.Summarisch].MandantName);

            var belastungskategorieIA = TestDataHelpers.GetBelastungskategorie(scope, "IA");
            var belastungskategorieIC = TestDataHelpers.GetBelastungskategorie(scope, "IC");

            var netzSummarischDetailIA = scope.Session.Query<NetzSummarischDetail>().ToList().Single(m => m.Mandant.MandantName == mandants[NetzErfassungsmodus.Summarisch].MandantName && m.Belastungskategorie == belastungskategorieIA);
            var netzSummarischDetailIC = scope.Session.Query<NetzSummarischDetail>().ToList().Single(m => m.Mandant.MandantName == mandants[NetzErfassungsmodus.Summarisch].MandantName && m.Belastungskategorie == belastungskategorieIC);

            netzSummarischDetailIA.Fahrbahnflaeche = 100;
            netzSummarischDetailIA.Fahrbahnlaenge = 50;
            netzSummarischDetailIA.MittlererZustand = 2.1m;

            netzSummarischDetailIA.Fahrbahnflaeche = 80;
            netzSummarischDetailIA.Fahrbahnlaenge = 30;
            netzSummarischDetailIA.MittlererZustand = 1.3m;

            scope.Session.Save(netzSummarisch);
            scope.Session.Save(netzSummarischDetailIA);
            scope.Session.Save(netzSummarischDetailIC);
        }

        private void PrepareRealisierteMassnahmenSummarischData(NHibernateTestScope scope)
        {
            var realisierteMassnahmeSummarisch = TestDataHelpers.GetRealisierteMassnahmeSummarisch(CurrentErfassungPeriods[NetzErfassungsmodus.Summarisch], "SumTest", "Keine Ahnung");
            scope.Session.Save(realisierteMassnahmeSummarisch);
        }

        private void PrepareStrassenabschnittAndZustandabschnittTabellarischData(NHibernateTestScope scope)
        {
            var belastungskategorieIA = TestDataHelpers.GetBelastungskategorie(scope, "IA");
            var belastungskategorieIC = TestDataHelpers.GetBelastungskategorie(scope, "IC");

            var strAbschnitt1 = TestDataHelpers.GetStrassenabschnitt(CurrentErfassungPeriods[NetzErfassungsmodus.Tabellarisch], "first", belastungskategorieIA, EigentuemerTyp.Gemeinde);
            var strAbschnitt2 = TestDataHelpers.GetStrassenabschnitt(CurrentErfassungPeriods[NetzErfassungsmodus.Tabellarisch], "second", belastungskategorieIC, EigentuemerTyp.Gemeinde);

            scope.Session.Save(strAbschnitt1);
            scope.Session.Save(strAbschnitt2);

            var zndAbschnitt1 = TestDataHelpers.GetZustandsabschnitt(strAbschnitt1, 0.3m);
            var zndAbschnitt2 = TestDataHelpers.GetZustandsabschnitt(strAbschnitt2, 2.3m);

            scope.Session.Save(zndAbschnitt1);
            scope.Session.Save(zndAbschnitt2);
        }

        private void PrepareRealisierteMassnahmenTabellarischData(NHibernateTestScope scope)
        {
            var realisierteMassnahme1 = TestDataHelpers.GetRealisierteMassnahme(CurrentErfassungPeriods[NetzErfassungsmodus.Tabellarisch], "mas1", "mas1");
            var realisierteMassnahme2 = TestDataHelpers.GetRealisierteMassnahme(CurrentErfassungPeriods[NetzErfassungsmodus.Tabellarisch], "mas2", "mas2");

            scope.Session.Save(realisierteMassnahme1);
            scope.Session.Save(realisierteMassnahme2);
        }

        private void PrepareStrassenabschnittGISData(NHibernateTestScope scope)
        {
            var belastungskategorieIB = TestDataHelpers.GetBelastungskategorie(scope, "IB");
            var belastungskategorieIC = TestDataHelpers.GetBelastungskategorie(scope, "IC");

            var strassenabschnittGisIB = TestDataHelpers.GetStrassenabschnittGIS(CurrentErfassungPeriods[NetzErfassungsmodus.Gis], "str1", belastungskategorieIB, EigentuemerTyp.Gemeinde);
            var strassenabschnittGisIC = TestDataHelpers.GetStrassenabschnittGIS(CurrentErfassungPeriods[NetzErfassungsmodus.Gis], "str2", belastungskategorieIC, EigentuemerTyp.Gemeinde);

            scope.Session.Save(strassenabschnittGisIB);
            scope.Session.Save(strassenabschnittGisIC);

            var zustandsabschnittGisIB = TestDataHelpers.GetZustandsabschnittGIS(strassenabschnittGisIB, 2.3m);
            var zustandsabschnittGisIC = TestDataHelpers.GetZustandsabschnittGIS(strassenabschnittGisIC, 0.3m);

            scope.Session.Save(zustandsabschnittGisIB);
            scope.Session.Save(zustandsabschnittGisIC);

            var inspektionsRouteGisIB = TestDataHelpers.GetInspektionsRouteGIS(CurrentErfassungPeriods[NetzErfassungsmodus.Gis], "insp1", DateTime.Parse("2012.01.01"), strassenabschnittGisIB);
            var inspektionsRouteGisIC = TestDataHelpers.GetInspektionsRouteGIS(CurrentErfassungPeriods[NetzErfassungsmodus.Gis], "insp1", DateTime.Parse("2012.03.21"), strassenabschnittGisIC);

            scope.Session.Save(inspektionsRouteGisIB);
            scope.Session.Save(inspektionsRouteGisIC);
        }

        private void PrepareRealisierteMassnahmenGISData(NHibernateTestScope scope)
        {
            var realisierteMassnahmeGis1 = TestDataHelpers.GetRealisierteMassnahmeGIS(CurrentErfassungPeriods[NetzErfassungsmodus.Gis], "rma1", "org1", "bes1");
            var realisierteMassnahmeGis2 = TestDataHelpers.GetRealisierteMassnahmeGIS(CurrentErfassungPeriods[NetzErfassungsmodus.Gis], "rma2", "org2", "bes2");

            scope.Session.Save(realisierteMassnahmeGis1);
            scope.Session.Save(realisierteMassnahmeGis2);
        }

        private void PrepareKoordinierteMassnahmenGISData(NHibernateTestScope scope)
        {
            var koordinierteMassnahmeGis1 = TestDataHelpers.GetKoordinierteMassnahmeGIS(CurrentErfassungPeriods[NetzErfassungsmodus.Gis], "kor1", StatusTyp.InKoordination, DateTime.Parse("2012.01.01"));
            var koordinierteMassnahmeGis2 = TestDataHelpers.GetKoordinierteMassnahmeGIS(CurrentErfassungPeriods[NetzErfassungsmodus.Gis], "kor2", StatusTyp.Abgeschlossen, DateTime.Parse("2012.11.12"));

            scope.Session.Save(koordinierteMassnahmeGis1);
            scope.Session.Save(koordinierteMassnahmeGis2);
        }

        private void PrepareMassnahmenDerTeilsystemGISData(NHibernateTestScope scope)
        {
            var massnahmenvorschlagTeilsystemeGis1 = TestDataHelpers.GetMassnahmenvorschlagTeilsystemeGIS(CurrentErfassungPeriods[NetzErfassungsmodus.Gis], "mas1", StatusTyp.InKoordination, DringlichkeitTyp.Dringlich, TeilsystemTyp.Abwasseranlagen);
            var massnahmenvorschlagTeilsystemeGis2 = TestDataHelpers.GetMassnahmenvorschlagTeilsystemeGIS(CurrentErfassungPeriods[NetzErfassungsmodus.Gis], "mas2", StatusTyp.Vorgeschlagen, DringlichkeitTyp.Langfristig, TeilsystemTyp.Gruenanlagen);

            scope.Session.Save(massnahmenvorschlagTeilsystemeGis1);
            scope.Session.Save(massnahmenvorschlagTeilsystemeGis2);
        }

        protected void SetMandantByModus(NetzErfassungsmodus modus)
        {
            BrowserDriver.InvokeGetAction<HeaderController, Guid?>((c, r) => c.SetMandant(null), new NameValueCollection { { "mandantId", mandants[modus].Id.ToString() } });
        }


    }
}
