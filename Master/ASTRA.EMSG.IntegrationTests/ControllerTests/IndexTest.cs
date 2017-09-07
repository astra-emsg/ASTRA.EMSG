using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.IntegrationTests.Common;
using ASTRA.EMSG.Web.Areas.Administration.Controllers;
using ASTRA.EMSG.Web.Areas.Auswertungen.Controllers;
using ASTRA.EMSG.Web.Areas.NetzverwaltungGIS.Controllers;
using NUnit.Framework;

namespace ASTRA.EMSG.IntegrationTests.ControllerTests
{
    [TestFixture]
    public class IndexTest : ControllerTestBase
    {
        [Test]
        public void EineListeVonRealisiertenMassnahmenGeordnetNachJahrenControllerIndexTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Tabellarisch);
            BrowserDriver.InvokeGetAction<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenController>(c => c.Index());
            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISControllerIndexTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Gis);
            BrowserDriver.InvokeGetAction<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISController>(c => c.Index());
            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischControllerIndexTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Summarisch);
            BrowserDriver.InvokeGetAction<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischController>(c => c.Index());
            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void AusgefuellteErfassungsformulareFuerOberflaechenschaedenControllerIndexTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Gis);
            BrowserDriver.InvokeGetAction<AusgefuellteErfassungsformulareFuerOberflaechenschaedenController>(c => c.Index());
            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void MassnahmenvorschlagProZustandsabschnittControllerIndexTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Tabellarisch);
            BrowserDriver.InvokeGetAction<MassnahmenvorschlagProZustandsabschnittController>(c => c.Index());
            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void MengeProBelastungskategorieControllerIndexTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Tabellarisch);
            BrowserDriver.InvokeGetAction<MengeProBelastungskategorieController>(c => c.Index());

            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void NochNichtInspizierteStrassenabschnitteControllerIndexTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Tabellarisch);
            BrowserDriver.InvokeGetAction<NochNichtInspizierteStrassenabschnitteController>(c => c.Index());
            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void StrassenabschnitteListeControllerIndexTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Tabellarisch);
            BrowserDriver.InvokeGetAction<StrassenabschnitteListeController>(c => c.Index());
            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void StrassenabschnitteListeOhneInspektionsrouteControllerIndexTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Gis);
            BrowserDriver.InvokeGetAction<StrassenabschnitteListeOhneInspektionsrouteController>(c => c.Index());

            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void WiederbeschaffungswertUndWertverlustProBelastungskategorieControllerIndexTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Tabellarisch);
            BrowserDriver.InvokeGetAction<WiederbeschaffungswertUndWertverlustProBelastungskategorieController>(c => c.Index());
            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void WiederbeschaffungswertUndWertverlustProStrassenabschnittControllerIndexTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Tabellarisch);
            BrowserDriver.InvokeGetAction<WiederbeschaffungswertUndWertverlustProStrassenabschnittController>(c => c.Index());
            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void ZustandProZustandsabschnittControllerIndexTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Tabellarisch);
            BrowserDriver.InvokeGetAction<ZustandProZustandsabschnittController>(c => c.Index());
            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void EineListeVonMassnahmenGegliedertNachTeilsystemenControllerIndexTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Gis);
            BrowserDriver.InvokeGetAction<EineListeVonMassnahmenGegliedertNachTeilsystemenController>(c => c.Index());
            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void EineListeVonKoordiniertenMassnahmenControllerIndexTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Gis);
            BrowserDriver.InvokeGetAction<EineListeVonKoordiniertenMassnahmenController>(c => c.Index());
            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void ListeDerInspektionsroutenControllerIndexTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Gis);
            BrowserDriver.InvokeGetAction<ListeDerInspektionsroutenController>(c => c.Index());
            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void InspektionsroutenGISControllerIndexTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Gis);
            BrowserDriver.InvokeGetAction<InspektionsroutenGISController>(c => c.Index());
            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void MassnahmenvorschlagProZustandsabschnittGISControllerIndexTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Gis);
            BrowserDriver.InvokeGetAction<MassnahmenvorschlagProZustandsabschnittGISController>(c => c.Index());
            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void StrassenabschnitteListeGISSControllerIndexTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Gis);
            BrowserDriver.InvokeGetAction<StrassenabschnitteListeGISController>(c => c.Index());
            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void ZustandProZustandsabschnittGISSControllerIndexTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Gis);
            BrowserDriver.InvokeGetAction<ZustandProZustandsabschnittGISController>(c => c.Index());
            BrowserDriver.AssertLastResultWasNotAnError();
        }

        [Test]
        public void ErfassungsPeriodAbschlussControllerIndexTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Gis);
            BrowserDriver.InvokeGetAction<ErfassungsPeriodAbschlussController>(c => c.Index());
            BrowserDriver.AssertLastResultWasNotAnError();
        }
        
        [Ignore]
        [Test]
        public void EreignisLogControllerIndexTest()
        {
            SetMandantByModus(NetzErfassungsmodus.Gis);
            BrowserDriver.InvokeGetAction<EreignisLogController>(c => c.Index());
            BrowserDriver.AssertLastResultWasNotAnError();
        }
    }
}
