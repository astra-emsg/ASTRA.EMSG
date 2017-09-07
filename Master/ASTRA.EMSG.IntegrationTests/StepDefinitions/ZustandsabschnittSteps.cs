using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Models;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Business.Models.Strassennamen;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.IntegrationTests.Support;
using ASTRA.EMSG.IntegrationTests.Support.MvcTesting;
using ASTRA.EMSG.IntegrationTests.Support.ObjectReader;
using ASTRA.EMSG.Web.Areas.NetzverwaltungStrassennamen.Controllers;
using NHibernate.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.IntegrationTests.StepDefinitions
{
    [Binding]
    public class ZustandsabschnittSteps : StepsBase
    {
        private Mandant currentMandant;

        public static Dictionary<int, Guid> ZustandsabschnittIds
        {
            get { return (Dictionary<int, Guid>)ScenarioContext.Current["ZustandsabschnittIds"]; }
            set { ScenarioContext.Current["ZustandsabschnittIds"] = value; }
        }

        public static Dictionary<Guid, Zustandsabschnitt> Zustandsabschnitten
        {
            get { return (Dictionary<Guid, Zustandsabschnitt>)ScenarioContext.Current["Zustandsabschnitten"]; }
            set { ScenarioContext.Current["Zustandsabschnitten"] = value; }
        }

        public ZustandsabschnittSteps(BrowserDriver browserDriver) : base(browserDriver)
        {
            ZustandsabschnittIds = new Dictionary<int, Guid>();
            Zustandsabschnitten = new Dictionary<Guid, Zustandsabschnitt>();
        }

        [Given(@"für Mandant '(.+)' existieren folgende Zustände und Massnahmen:")]
        [Given(@"für Mandant '(.+)' existieren folgende Zustandsinformationen:")]
        public void AngenommenFurMandantMandant_1ExistierenFolgendeZustandsinformationen(string mandant, Table table)
        {
            using (NHibernateSpecflowScope scope = new NHibernateSpecflowScope())
            {
                var zustandsabschnittReader = GetZustandsabschnittReader();
                currentMandant = scope.GetMandant(mandant);
                var zustandsabschnitten = zustandsabschnittReader.GetObjectListWithRow<Zustandsabschnitt>(table);

                foreach (var zustandsabschnitt in zustandsabschnitten)
                {
                    Zustandsabschnitt za = zustandsabschnitt.Item2;
                    za.Aufnahmedatum = DateTime.Now;
                    scope.Session.Save(za);
                }
            }
        }

        [When(@"ich für Id '(.+)' folgende Zustandsabschnitte erfasse")]
        public void WennIchFurId4FolgendeZustandsabschnitteErfasse(int strassenabschnittId, Table table)
        {
            ZustandsabschnittModel zustandsabschnittModel;
            using (new NHibernateSpecflowScope())
            {
                zustandsabschnittModel = GetZustandsabschnittModelReader().GetObject<ZustandsabschnittModel>(table);
            }
            var model = new ZustandsabschnittMonsterModel
                            {
                                Stammdaten = zustandsabschnittModel,
                                Fahrbahn = new ZustandsabschnittdetailsModel {Id = Guid.NewGuid(), Zustandsindex = zustandsabschnittModel.Zustandsindex},
                                Trottoir = new ZustandsabschnittdetailsTrottoirModel {Id = Guid.NewGuid()}
                            };
            zustandsabschnittModel.Strassenabschnitt = StrassenabschnittSteps.StrassenabschnittIds[strassenabschnittId];
            BrowserDriver.InvokePostAction<ZustandsabschnittController, ZustandsabschnittMonsterModel>((c, r) => c.Insert(r), model, resuseLastRequest: false);
        }

        [Then(@"sind folgende Zustandsabschnitte im System")]
        public void DannSindFolgendeZustandsabschnitteImSystem(Table table)
        {
            if (HasFieldValidationError()) //don't assert if the previous save was unsuccessfull
                return;

            using (NHibernateSpecflowScope nHibernateSpecflowScope = new NHibernateSpecflowScope())
            {
                var strassenabschnittReader = GetZustandsabschnittReader();
                var areObjectListWithTableEqual = strassenabschnittReader.ARe(nHibernateSpecflowScope.Session.Query<Zustandsabschnitt>().ToList(), table);
                Assert.IsTrue(areObjectListWithTableEqual);
            }
        }



        [Then(@"sind für den Strassenabschnitt mit der Id '(.+)' keine Zustände und Massnahmen im System")]
        public void DannSindFurDenStrassenabschnittMitDerId1KeineZustandeUndMassnahmenImSystem(int strassenabschnittId)
        {
            using (NHibernateSpecflowScope scope = new NHibernateSpecflowScope())
            {
                var zustandsabschnitten = scope.Session.Query<Zustandsabschnitt>()
                    .Where(i => i.Strassenabschnitt.Id == StrassenabschnittSteps.StrassenabschnittIds[strassenabschnittId]);
                CollectionAssert.IsEmpty(zustandsabschnitten.ToArray());
            }
        }

        private ObjectReader GetZustandsabschnittModelReader()
        {
            return GetObjectReaderConfigurationFor<ZustandsabschnittModel>()
                .GetObjectReader();
        }

        private ObjectReader GetZustandsabschnittReader()
        {
            return GetObjectReaderConfigurationFor<Zustandsabschnitt>()
                .PropertyAliasFor(z => z.FlaceheTrottoirLinks, "FlächeTrottoirLinks")
                .PropertyAliasFor(z => z.FlaceheTrottoirRechts, "FlächeTrottoirRechts")
                .ConverterFor(e => e.Id, (s, propertyInfo) => ConvertId(s, ZustandsabschnittIds))
                .ConverterFor(z => z.Strassenabschnitt, (s, p) => ScenarioContextWrapper.CurrentScope.Session.Query<Strassenabschnitt>()
                    .Single(i => i.Id == StrassenabschnittSteps.StrassenabschnittIds[int.Parse(s)]))
                .CustomSetterFor<Zustandsabschnitt>("MassnahmenvorschlagFahrbahnDringlichkeit", (r, z) => z.DringlichkeitFahrbahn = r["MassnahmenvorschlagFahrbahnDringlichkeit"].ParseEnum<DringlichkeitTyp>())
                .CustomSetterFor<Zustandsabschnitt>("MassnahmenvorschlagFahrbahnTyp", (r, z) => z.MassnahmenvorschlagFahrbahn = ConvertMassnahmenvorschlagKatalog(r["MassnahmenvorschlagFahrbahnTyp"], currentMandant, z.Strassenabschnitt.Belastungskategorie))
                .CustomSetterFor<Zustandsabschnitt>("MassnahmenvorschlagTrottoirLinksDringlichkeit", (r, z) => z.DringlichkeitTrottoirLinks = r["MassnahmenvorschlagTrottoirLinksDringlichkeit"].ParseEnum<DringlichkeitTyp>())
                .CustomSetterFor<Zustandsabschnitt>("MassnahmenvorschlagTrottoirLinksTyp", (r, z) => z.MassnahmenvorschlagTrottoirLinks = ConvertMassnahmenvorschlagKatalog(r["MassnahmenvorschlagTrottoirLinksTyp"], currentMandant, z.Strassenabschnitt.Belastungskategorie))
                .CustomSetterFor<Zustandsabschnitt>("MassnahmenvorschlagTrottoirRechtsDringlichkeit", (r, z) => z.DringlichkeitTrottoirRechts= r["MassnahmenvorschlagTrottoirRechtsDringlichkeit"].ParseEnum<DringlichkeitTyp>())
                .CustomSetterFor<Zustandsabschnitt>("MassnahmenvorschlagTrottoirRechtsTyp", (r, z) => z.MassnahmenvorschlagTrottoirRechts = ConvertMassnahmenvorschlagKatalog(r["MassnahmenvorschlagTrottoirRechtsTyp"], currentMandant, z.Strassenabschnitt.Belastungskategorie))
                .GetObjectReader();
        }
    }
}
