using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.IntegrationTests.Support;
using ASTRA.EMSG.IntegrationTests.Support.MvcTesting;
using ASTRA.EMSG.Tests.Common.Utils;
using ASTRA.EMSG.Web.Areas.NetzverwaltungGIS.Controllers;
using ASTRA.EMSG.Web.Areas.NetzverwaltungStrassennamen.Controllers;
using ASTRA.EMSG.Web.Areas.NetzverwaltungSummarisch.Controllers;
using ASTRA.EMSG.Web.Controllers;
using NHibernate.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace ASTRA.EMSG.IntegrationTests.StepDefinitions
{

    public class PageRegistar
    {
        private readonly BrowserDriver browserDriver;
        private Dictionary<string, Action> registar;

        public PageRegistar(BrowserDriver browserDriver)
        {
            this.browserDriver = browserDriver;
            registar = new Dictionary<string, Action>
                {
                    { "Strassenmenge und Zustand", () => browserDriver.InvokeGetAction<StrassenmengeUndZustandController>(c => c.Index()) },
                    { "Netzdefinition (strassennamen)", () => browserDriver.InvokeGetAction<NetzdefinitionUndStrassenabschnittController>(c => c.Index()) },
                    { "Netzdefinition (gis)", () => browserDriver.InvokeGetAction<NetzdefinitionUndStrassenabschnittGISController>(c => c.Index()) },
                    { "Zustände und Massnahmenvorschläge (strassennamen)", () => browserDriver.InvokeGetAction<ZustaendeUndMassnahmenvorschlaegeController>(c => c.Index()) },
                    { "Zustände und Massnahmenvorschläge (gis)", () => browserDriver.InvokeGetAction<ZustaendeUndMassnahmenvorschlaegeGISController>(c => c.Index()) },
                    { "Inspektionsrouten", () => browserDriver.InvokeGetAction<InspektionsroutenGISController>(c => c.Index()) },
                    { "Massnahmenvorschläge anderer Teilsysteme", () => browserDriver.InvokeGetAction<MassnahmenvorschlaegeAndererTeilsystemeController>(c => c.Index()) },
                    { "Koordinierte Massnahmen", () => browserDriver.InvokeGetAction<KoordinierteMassnahmenController>(c => c.Index()) },
                    { "Realisierte Massnehmen (summarisch)", () => browserDriver.InvokeGetAction<RealisierteMassnahmenSummarischController>(c => c.Index()) },
                    { "Realisierte Massnehmen (strassennamen)", () => browserDriver.InvokeGetAction<RealisierteMassnahmenController>(c => c.Index()) },
                    { "Realisierte Massnehmen (gis)", () => browserDriver.InvokeGetAction<RealisierteMassnahmenGISController>(c => c.Index()) },
                    { "Kenngrössen früherer Jahre (summarisch)", () => browserDriver.InvokeGetAction<KenngroessenFruehererJahreSummarischController>(c => c.Index()) },
                    { "Kenngrössen früherer Jahre (strassennamen)", () => browserDriver.InvokeGetAction<KenngroessenFruehererJahreController>(c => c.Index()) },
                    { "Kenngrössen früherer Jahre (gis)", () => browserDriver.InvokeGetAction<KenngroessenFruehererJahreGISController>(c => c.Index()) },

                };
        }

        public Action GetInvoker(string page)
        {
            return registar[page];
        }
    }


    [Binding]
    public class SecuritySteps : StepsBase
    {
        public const string ApplicationName = "EMSG";

        public SecuritySteps(BrowserDriver browserDriver)
            : base(browserDriver)
        {

        }

        [Given(@"ich bin Benutzer von EMSG")]
        public void GegebenSeiIchBinBenutzerVonEMSG()
        {
            //NOP
        }

        [When(@"ich die Seite '(.*)' öffne")]
        public void WennIchDieSeiteSeiteOffne(string seite)
        {
            ScenarioContext.Current.Add("Seite", seite);
        }

        [Then(@"habe ich Zugriff als:")]
        public void DannHabeIchZugriffAls(Table table)
        {
            if (table.RowCount > 1)
                throw new InvalidOperationException("Multipre rows not supported");
            var pageRegistar = new PageRegistar(BrowserDriver);
            foreach (var header in table.Header)
            {
                var rolle = (Rolle)Enum.Parse(typeof(Rolle), header);
                SetupTestUserRole(rolle);
                Debug.WriteLine(rolle);
                Console.WriteLine(rolle);
                pageRegistar.GetInvoker((string)ScenarioContext.Current["Seite"])();
                var canAccess = table.Rows[0][header].ToLower() == "ja";
                if (canAccess)
                    BrowserDriver.GetRequestResult<TestViewResult>();
                else
                    BrowserDriver.GetRequestResult<TestHttpUnauthorizedResult>();
                BrowserDriver.InvokeGetAction<TestingController, ActionResult>((c, r) => c.StartNewSession(), (ActionResult)null);
            }
        }

        private void SetupTestUserRole(Rolle rolle)
        {
            using (var scope = new NHibernateSpecflowScope())
            {
                DbHandlerUtils.SetupTestUserRole(scope.Session, DbHandlerUtils.IntegrationTestUserName, rolle);
            }
        }

        [Given(@"folgende Einstellungen existieren:")]
        public void AngenommenFolgendeEinstellungenExistieren(Table table)
        {
            var einstellungenRows = table.CreateSet<EinstellungenRow>();

            using (var nhScope = new NHibernateSpecflowScope())
            {
                foreach (var einstellungenRow in einstellungenRows)
                {
                    DbHandlerUtils.CreateMandant(nhScope.Session, einstellungenRow.Mandant, null, GetModus(einstellungenRow.Modus));
                }
            }
        }

        [Given(@"ich bin Data-Manager von '(.+)'")]
        public void AngenommenIchBinData_ManagerVon(string mandantName)
        {
            CreateTestUserWithRoles(mandantName, new List<Rolle>() { Rolle.DataManager });
        }

        [Given(@"ich habe alle Rollen für '(.+)'")]
        public void AngenommenIchHabeAlleRollenFurMandant(string mandantName)
        {
            //TODO: Rename to all rolle for Reports
            CreateTestUserWithRoles(mandantName, new List<Rolle>() { Rolle.DataReader, Rolle.Benutzeradministrator });
        }

        private void CreateTestUserWithRoles(string mandantName, List<Rolle> rollen)
        {
            using (var nhScope = new NHibernateSpecflowScope())
            {
                IQueryable<Mandant> mandanten = nhScope.Session.Query<Mandant>().Where(m => m.MandantName == mandantName);
                DbHandlerUtils.CreateTestUser(nhScope.Session, DbHandlerUtils.IntegrationTestUserName, mandanten, rollen);
            }
        }


        private NetzErfassungsmodus GetNetzErfassungsmodus(string netzErfassungsmodus)
        {
            switch (netzErfassungsmodus.ToLower())
            {
                case "summarisch": return NetzErfassungsmodus.Summarisch;
                case "gis": return NetzErfassungsmodus.Gis;
                case "strassennamen": return NetzErfassungsmodus.Tabellarisch;
                default:
                    throw new ArgumentOutOfRangeException("netzErfassungsmodus");
            }
        }
    }
}
