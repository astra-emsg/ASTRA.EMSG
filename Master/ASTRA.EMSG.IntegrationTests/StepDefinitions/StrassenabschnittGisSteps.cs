using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Models;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.IntegrationTests.Support;
using ASTRA.EMSG.IntegrationTests.Support.MvcTesting;
using ASTRA.EMSG.IntegrationTests.Support.ObjectReader;
using DocumentFormat.OpenXml.Wordprocessing;
using TechTalk.SpecFlow;
using Table = TechTalk.SpecFlow.Table;

namespace ASTRA.EMSG.IntegrationTests.StepDefinitions
{
    [Binding]
    public class StrassenabschnittGisSteps : StepsBase
    {
        public static Dictionary<int, Guid> StrassenabschnittGisIds
        {
            get { return (Dictionary<int, Guid>)ScenarioContext.Current["StrassenabschnittGisIds"]; }
            set { ScenarioContext.Current["StrassenabschnittGisIds"] = value; }
        }

        public static Dictionary<Guid, StrassenabschnittGIS> StrassenabschnittGiss
        {
            get { return (Dictionary<Guid, StrassenabschnittGIS>)ScenarioContext.Current["StrassenabschnittGiss"]; }
            set { ScenarioContext.Current["StrassenabschnittGiss"] = value; }
        }

        public StrassenabschnittGisSteps(BrowserDriver browserDriver)
            : base(browserDriver)
        {
            StrassenabschnittGisIds = new Dictionary<int, Guid>();
            StrassenabschnittGiss = new Dictionary<Guid, StrassenabschnittGIS>();
        }

        [Given(@"für Mandant '(.+)' existieren folgende GIS Netzinformationen:")]
        public void AngenommenFurMandantExistierenFolgendeNetzinformationen(string mandant, Table table)
        {
            using (NHibernateSpecflowScope scope = new NHibernateSpecflowScope())
            {
                var strassenabschnittReader = GetStrassenabschnittGISReader();
                var strassenabschnitten = strassenabschnittReader.GetObjectList<StrassenabschnittGIS>(table);
                ErfassungsPeriod currentErfassungsperiod = scope.GetCurrentErfassungsperiod(mandant);
                var achse = new Achse
                {
                    VersionValidFrom = DateTime.Now,
                    ErfassungsPeriod = currentErfassungsperiod
                };
                var segment = new AchsenSegment();
                segment.Achse = achse;
                segment.ErfassungsPeriod = currentErfassungsperiod;
                achse.AchsenSegmente.Add(segment);
                scope.Session.Save(segment);
                scope.Session.Save(achse);

                foreach (var strassenabschnitt in strassenabschnitten)
                {
                    strassenabschnitt.Mandant = scope.GetMandant(mandant);

                    strassenabschnitt.ErfassungsPeriod = currentErfassungsperiod;
                    var achsenReferenz = new AchsenReferenz();
                    achsenReferenz.AchsenSegment = segment;
                    segment.AchsenReferenzen.Add(achsenReferenz);
                    scope.Session.Save(achsenReferenz);
                    var referenzGruppe = new ReferenzGruppe();
                    strassenabschnitt.ReferenzGruppe = referenzGruppe;
                    referenzGruppe.StrassenabschnittGISList.Add(strassenabschnitt);
                    achsenReferenz.ReferenzGruppe = referenzGruppe;
                    referenzGruppe.AchsenReferenzen.Add(achsenReferenz);
                    scope.Session.Save(referenzGruppe);
                    scope.Session.Save(strassenabschnitt);
                }
            }
        }

        private ObjectReader GetStrassenabschnittGISModelReader()
        {

            return GetObjectReaderConfigurationFor<StrassenabschnittGISModel>()
                .ConverterFor(e => e.Belastungskategorie, ConvertBelastungskategorieId)
                .PropertyAliasFor(e => e.Laenge, "Gesamtlänge")
                .GetObjectReader();
        }

        private ObjectReader GetStrassenabschnittGISReader()
        {
            return GetObjectReaderConfigurationFor<StrassenabschnittGIS>()
                .ConverterFor(e => e.Id, (s, propertyInfo) => ConvertId(s, StrassenabschnittGisIds))
                .ConverterFor(e => e.Belastungskategorie, (s, p) => ConvertBelastungskategorie(s))
                .PropertyAliasFor(e => e.Laenge, "Gesamtlänge")
                .GetObjectReader();
        }
    }
}
