using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.IntegrationTests.Common;
using ASTRA.EMSG.Web.Areas.Auswertungen.Controllers;
using ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands;
using ASTRA.EMSG.Web.Areas.NetzverwaltungGIS.Controllers;
using Kendo.Mvc.UI;
using NUnit.Framework;

namespace ASTRA.EMSG.IntegrationTests.ControllerTests
{
    [TestFixture]
    public class GetAllTest : ControllerTestBase
    {
        private DataSourceRequest dataSourceRequest = new DataSourceRequest();
        
        [Test]
        public void EineListeVonRealisiertenMassnahmenGeordnetNachJahrenControllerGetAllTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Tabellarisch);
            BrowserDriver.InvokeGetAction<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenController, EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGridCommand>(
                        (c, r) => c.GetAll(dataSourceRequest, null), new EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGridCommand
                        {
                            ErfassungsPeriodIdVon = ClosedErfassungPeriods[NetzErfassungsmodus.Tabellarisch].Id,
                            ErfassungsPeriodIdBis = CurrentErfassungPeriods[NetzErfassungsmodus.Tabellarisch].Id
                        });

            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISControllerGetAllTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Gis);
            BrowserDriver.InvokeGetAction<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISController, EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISGridCommand>(
                        (c, r) => c.GetAll(dataSourceRequest, null), new EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISGridCommand
                        {
                            ErfassungsPeriodIdVon = ClosedErfassungPeriods[NetzErfassungsmodus.Gis].Id,
                            ErfassungsPeriodIdBis = CurrentErfassungPeriods[NetzErfassungsmodus.Gis].Id
                        });

            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischControllerGetAllTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Summarisch);
            BrowserDriver.InvokeGetAction<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischController, EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischGridCommand>(
                        (c, r) => c.GetAll(dataSourceRequest, null), new EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischGridCommand
                        {
                            ErfassungsPeriodIdVon = ClosedErfassungPeriods[NetzErfassungsmodus.Summarisch].Id,
                            ErfassungsPeriodIdBis = CurrentErfassungPeriods[NetzErfassungsmodus.Summarisch].Id
                        });

            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void AusgefuellteErfassungsformulareFuerOberflaechenschaedenControllerGetAllTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Gis);
            BrowserDriver.InvokeGetAction<AusgefuellteErfassungsformulareFuerOberflaechenschaedenController, AusgefuellteErfassungsformulareFuerOberflaechenschaedenGridCommand>(
                (c, r) => c.GetAll(dataSourceRequest, null), new AusgefuellteErfassungsformulareFuerOberflaechenschaedenGridCommand());

            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void MassnahmenvorschlagProZustandsabschnittControllerGetAllTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Tabellarisch);
            BrowserDriver.InvokeGetAction<MassnahmenvorschlagProZustandsabschnittController, MassnahmenvorschlagProZustandsabschnittGridCommand>(
                (c, r) => c.GetAll(dataSourceRequest, null), new MassnahmenvorschlagProZustandsabschnittGridCommand());

            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void MengeProBelastungskategorieControllerGetAllTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Tabellarisch);
            BrowserDriver.InvokeGetAction<MengeProBelastungskategorieController, MengeProBelastungskategorieGridCommand>(
                (c, r) => c.GetAll(dataSourceRequest, null), new MengeProBelastungskategorieGridCommand());

            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void NochNichtInspizierteStrassenabschnitteControllerGetAllTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Tabellarisch);
            BrowserDriver.InvokeGetAction<NochNichtInspizierteStrassenabschnitteController, NochNichtInspizierteStrassenabschnitteGridCommand>(
                (c, r) => c.GetAll(dataSourceRequest, null), new NochNichtInspizierteStrassenabschnitteGridCommand());

            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void StrassenabschnitteListeControllerGetAllTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Tabellarisch);
            BrowserDriver.InvokeGetAction<StrassenabschnitteListeController, StrassenabschnitteListeGridCommand>(
                (c, r) => c.GetAll(dataSourceRequest, null), new StrassenabschnitteListeGridCommand());

            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void StrassenabschnitteListeOhneInspektionsrouteControllerGetAllTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Gis);
            BrowserDriver.InvokeGetAction<StrassenabschnitteListeOhneInspektionsrouteController, StrassenabschnitteListeOhneInspektionsrouteGridCommand>(
                (c, r) => c.GetAll(dataSourceRequest, null), new StrassenabschnitteListeOhneInspektionsrouteGridCommand());

            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void WiederbeschaffungswertUndWertverlustProBelastungskategorieControllerGetAllTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Tabellarisch);
            BrowserDriver.InvokeGetAction<WiederbeschaffungswertUndWertverlustProBelastungskategorieController, WiederbeschaffungswertUndWertverlustProBelastungskategorieGridCommand>(
                (c, r) => c.GetAll(dataSourceRequest, null), new WiederbeschaffungswertUndWertverlustProBelastungskategorieGridCommand());

            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void WiederbeschaffungswertUndWertverlustProStrassenabschnittControllerGetAllTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Tabellarisch);
            BrowserDriver.InvokeGetAction<WiederbeschaffungswertUndWertverlustProStrassenabschnittController, WiederbeschaffungswertUndWertverlustProStrassenabschnittGridCommand>(
                (c, r) => c.GetAll(dataSourceRequest, null), new WiederbeschaffungswertUndWertverlustProStrassenabschnittGridCommand());

            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void ZustandProZustandsabschnittControllerGetAllTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Tabellarisch);
            BrowserDriver.InvokeGetAction<ZustandProZustandsabschnittController, ZustandProZustandsabschnittGridCommand>(
                (c, r) => c.GetAll(dataSourceRequest, null), new ZustandProZustandsabschnittGridCommand());

            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void EineListeVonMassnahmenGegliedertNachTeilsystemenControllerGetAllTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Gis);
            BrowserDriver.InvokeGetAction<EineListeVonMassnahmenGegliedertNachTeilsystemenController, EineListeVonMassnahmenGegliedertNachTeilsystemenGridCommand>(
                (c, r) => c.GetAll(dataSourceRequest, null), new EineListeVonMassnahmenGegliedertNachTeilsystemenGridCommand());

            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void EineListeVonKoordiniertenMassnahmenControllerGetAllTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Gis);
            BrowserDriver.InvokeGetAction<EineListeVonKoordiniertenMassnahmenController, EineListeVonKoordiniertenMassnahmenGridCommand>(
                (c, r) => c.GetAll(dataSourceRequest, null), new EineListeVonKoordiniertenMassnahmenGridCommand());

            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void ListeDerInspektionsroutenControllerGetAllTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Gis);
            BrowserDriver.InvokeGetAction<ListeDerInspektionsroutenController, ListeDerInspektionsroutenGridCommand>(
                (c, r) => c.GetAll(dataSourceRequest, null), new ListeDerInspektionsroutenGridCommand());

            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void InspektionsroutenGISControllerGetAllTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Gis);
            BrowserDriver.InvokeGetAction<InspektionsroutenGISController, ActionResult>((c, r) => c.GetAll(dataSourceRequest), (ActionResult)null);
            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void MassnahmenvorschlagProZustandsabschnittGISControllerGetAllTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Gis);
            BrowserDriver.InvokeGetAction<MassnahmenvorschlagProZustandsabschnittGISController, MassnahmenvorschlagProZustandsabschnittGridCommand>(
                (c, r) => c.GetAll(dataSourceRequest, null), new MassnahmenvorschlagProZustandsabschnittGridCommand());

            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void StrassenabschnitteListeGISSControllerGetAllTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Gis);
            BrowserDriver.InvokeGetAction<StrassenabschnitteListeGISController, StrassenabschnitteListeGridCommand>(
                (c, r) => c.GetAll(dataSourceRequest, null), new StrassenabschnitteListeGridCommand());

            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void ZustandProZustandsabschnittGISSControllerGetAllTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Gis);
            BrowserDriver.InvokeGetAction<ZustandProZustandsabschnittGISController, ZustandProZustandsabschnittGridCommand>(
                (c, r) => c.GetAll(dataSourceRequest, null), new ZustandProZustandsabschnittGridCommand());

            BrowserDriver.AssertLastResultWasNotAnError();
        }
    }
}
