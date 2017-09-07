using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Entities.Summarisch;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.BenchmarkauswertungInventarkennwerten;
using ASTRA.EMSG.Business.Reports.BenchmarkauswertungKennwertenRealisiertenMassnahmen;
using ASTRA.EMSG.Business.Reports.BenchmarkauswertungZustandskennwerten;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.IntegrationTests.Support;
using ASTRA.EMSG.IntegrationTests.Support.MvcTesting;
using ASTRA.EMSG.Tests.Common.Utils;
using ASTRA.EMSG.Web.Areas.Benchmarking.Controllers;
using ASTRA.EMSG.Web.Areas.Common.Controllers;
using NHibernate.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace ASTRA.EMSG.IntegrationTests.StepDefinitions
{
    [Binding]
    public class BenchmarkingSteps : StepsBase
    {
        private readonly Dictionary<NetzErfassungsmodus, Dictionary<Type, IEnumerable<IErfassungsPeriodDependentEntity>>> entitesProErfassungsmodus
            = new Dictionary<NetzErfassungsmodus, Dictionary<Type, IEnumerable<IErfassungsPeriodDependentEntity>>>
                  {
                      { NetzErfassungsmodus.Summarisch, new Dictionary<Type, IEnumerable<IErfassungsPeriodDependentEntity>>() },
                      { NetzErfassungsmodus.Tabellarisch, new Dictionary<Type, IEnumerable<IErfassungsPeriodDependentEntity>>() },
                      { NetzErfassungsmodus.Gis, new Dictionary<Type, IEnumerable<IErfassungsPeriodDependentEntity>>() }
                  };

        private string currentMandantName;
        private readonly AdministrationSteps administrationSteps;
        private ReportSteps reportsSteps;

        public BenchmarkingSteps(BrowserDriver browserDriver)
            : base(browserDriver)
        {
            administrationSteps = new AdministrationSteps(BrowserDriver);
            reportsSteps = new ReportSteps(BrowserDriver);
        }

        [Given(@"folgende Einstellungen für Benchmarkauswertung existieren:")]
        public void GegebenSeiFolgendeEinstellungenExistieren(Table table)
        {

            var reader = GetObjectReaderConfigurationFor<MandantDetails>()
                .ConverterFor(e => e.Mandant, (s, p) => new Mandant() { MandantName = s })
                .ConverterFor(e => e.Gemeindetyp, (s, p) => ScenarioContextWrapper.CurrentScope.Session.Query<GemeindeKatalog>().ToArray().Single(i => i.Typ == s))
                .ConverterFor(e => e.OeffentlicheVerkehrsmittel, (s, p) => ScenarioContextWrapper.CurrentScope.Session.Query<OeffentlicheVerkehrsmittelKatalog>().Single(i => i.Typ == s))

                .GetObjectReader();

            using (var nhScope = new NHibernateSpecflowScope())
            {
                foreach (var row in reader.GetObjectListWithRow<MandantDetails>(table))
                {
                    row.Item2.IsCompleted = true;

                    DbHandlerUtils.CreateMandant(nhScope.Session, row.Item1["Mandant"], null, mandantDetails: row.Item2);
                }
            }

            using (var nhScope = new NHibernateSpecflowScope())
            {
                IQueryable<Mandant> mandanten = nhScope.Session.Query<Mandant>();
                DbHandlerUtils.CreateTestUser(nhScope.Session, DbHandlerUtils.IntegrationTestUserName, mandanten, new List<Rolle>() { Rolle.Benutzeradministrator, Rolle.DataManager, Rolle.Benchmarkteilnehmer });
            }
        }

        [Given(@"die folgenden Strassenabschnitte existieren:")]
        public void AngenommenDieFolgendenStrassenabschnitteExistieren(Table table)
        {
            var reader = GetObjectReaderConfigurationFor<Strassenabschnitt>()
                .PropertyAliasFor(e => e.ErfassungsPeriod, "Jahr")
                .ConverterFor(e => e.Belastungskategorie, (s, p) => ScenarioContextWrapper.CurrentScope.Session.Query<Belastungskategorie>().ToArray().Single(i => i.Typ == s))
                .ConverterFor(e => e.Mandant, (s, p) => new Mandant() { MandantName = s })
                .ConverterFor(e => e.ErfassungsPeriod, (s, p) => new ErfassungsPeriod() { Erfassungsjahr = new DateTime(int.Parse(s), 1, 1) });
            using (var nhScope = new NHibernateSpecflowScope())
            {
                entitesProErfassungsmodus[NetzErfassungsmodus.Tabellarisch][typeof(Strassenabschnitt)] = reader.GetObjectReader().GetObjectList<Strassenabschnitt>(table);
            }
        }

        [Given(@"die folgenden Zustandsabschnitte existieren:")]
        public void AngenommenDieFolgendenZustandsabschnitteExistieren(Table table)
        {
            var reader = GetObjectReaderConfigurationFor<Zustandsabschnitt>()
                .PropertyAliasFor(e => e.ErfassungsPeriod, "Jahr")
                .PropertyAliasFor(e => e.Strassenabschnitt, "Strassenname")
                .ConverterFor(e => e.Mandant, (s, p) => new Mandant())
                .ConverterFor(e => e.Strassenabschnitt, (s, p) => new Strassenabschnitt())
                .ConverterFor(e => e.ErfassungsPeriod, (s, p) => new ErfassungsPeriod());

            var strassenabschnitts = entitesProErfassungsmodus[NetzErfassungsmodus.Tabellarisch][typeof (Strassenabschnitt)].Cast<Strassenabschnitt>().ToArray();
            foreach (var zustandabscnitt in reader.GetObjectReader().GetObjectListWithRow<Zustandsabschnitt>(table))
            {
                var sa = strassenabschnitts.Single(s =>
                                                   s.Mandant.MandantName == zustandabscnitt.Item1["Mandant"] &&
                                                   s.ErfassungsPeriod.Erfassungsjahr.Year.ToString() == zustandabscnitt.Item1["Jahr"] &&
                                                   s.Strassenname == zustandabscnitt.Item1["Strassenname"]
                    );
                zustandabscnitt.Item2.Strassenabschnitt = sa;
                sa.Zustandsabschnitten.Add(zustandabscnitt.Item2);
            }
        }

        [Given(@"die folgenden ZustandsabschnitteGIS existieren:")]
        public void AngenommenDieFolgendenZustandsabschnitteGISExistieren(Table table)
        {
            var reader = GetObjectReaderConfigurationFor<ZustandsabschnittGIS>()
                .PropertyAliasFor(e => e.ErfassungsPeriod, "Jahr")
                .PropertyAliasFor(e => e.StrassenabschnittGIS, "Strassenname")
                .ConverterFor(e => e.Mandant, (s, p) => new Mandant())
                .ConverterFor(e => e.StrassenabschnittGIS, (s, p) => new StrassenabschnittGIS())
                .ConverterFor(e => e.ErfassungsPeriod, (s, p) => new ErfassungsPeriod());

            var strassenabschnitts = entitesProErfassungsmodus[NetzErfassungsmodus.Gis][typeof(StrassenabschnittGIS)].Cast<StrassenabschnittGIS>().ToArray();
            foreach (var zustandabscnitt in reader.GetObjectReader().GetObjectListWithRow<ZustandsabschnittGIS>(table))
            {
                var sa = strassenabschnitts.Single(s =>
                                                   s.Mandant.MandantName == zustandabscnitt.Item1["Mandant"] &&
                                                   s.ErfassungsPeriod.Erfassungsjahr.Year.ToString() == zustandabscnitt.Item1["Jahr"] &&
                                                   s.Strassenname == zustandabscnitt.Item1["Strassenname"]
                    );
                zustandabscnitt.Item2.StrassenabschnittGIS = sa;

                var achsenReferenz = new AchsenReferenz();
                achsenReferenz.AchsenSegment = sa.ReferenzGruppe.AchsenReferenzen[0].AchsenSegment;
                var referenzGruppe = new ReferenzGruppe();
                zustandabscnitt.Item2.ReferenzGruppe = referenzGruppe;
                referenzGruppe.ZustandsabschnittGISList.Add(zustandabscnitt.Item2);
                achsenReferenz.ReferenzGruppe = referenzGruppe;
                referenzGruppe.AchsenReferenzen.Add(achsenReferenz);
                sa.Zustandsabschnitten.Add(zustandabscnitt.Item2);
            }
        }

        [Given(@"die folgenden StrassenabschnitteGIS existieren:")]
        public void AngenommenDieFolgendenStrassenabschnitteGUSExistieren(Table table)
        {

            using (var scope = new NHibernateSpecflowScope())
            {
                foreach (var currentErfassungsperiod in scope.Session.Query<ErfassungsPeriod>())
                {
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
                }
            }

            var reader = GetObjectReaderConfigurationFor<StrassenabschnittGIS>()
                .PropertyAliasFor(e => e.ErfassungsPeriod, "Jahr")
                .ConverterFor(e => e.Belastungskategorie, (s, p) => ScenarioContextWrapper.CurrentScope.Session.Query<Belastungskategorie>().ToArray().Single(i => i.Typ == s))
                .ConverterFor(e => e.Mandant, (s, p) => new Mandant { MandantName = s })
                .ConverterFor(e => e.ErfassungsPeriod, (s, p) => new ErfassungsPeriod { Erfassungsjahr = new DateTime(int.Parse(s), 1, 1) });

            using (var nhScope = new NHibernateSpecflowScope())
            {
                entitesProErfassungsmodus[NetzErfassungsmodus.Gis][typeof(StrassenabschnittGIS)] = reader.GetObjectReader().GetObjectList<StrassenabschnittGIS>(table);
                foreach (StrassenabschnittGIS strassenabschnitt in entitesProErfassungsmodus[NetzErfassungsmodus.Gis][typeof(StrassenabschnittGIS)])
                {
                    var segment =
                        nhScope.Session.Query<AchsenSegment>().First(a => !a.ErfassungsPeriod.IsClosed &&
                            a.ErfassungsPeriod.Mandant.MandantName == strassenabschnitt.Mandant.MandantName);
                    var achsenReferenz = new AchsenReferenz();
                    achsenReferenz.AchsenSegment = segment;
                    var referenzGruppe = new ReferenzGruppe();
                    strassenabschnitt.ReferenzGruppe = referenzGruppe;
                    referenzGruppe.StrassenabschnittGISList.Add(strassenabschnitt);
                    achsenReferenz.ReferenzGruppe = referenzGruppe;
                    referenzGruppe.AchsenReferenzen.Add(achsenReferenz);
                }
            }
        }

        [Given(@"die folgenden Details zum NetzSummarisch existieren:")]
        public void AngenommenDieFolgendenDetailsZumNetzSummarischExistieren(Table table)
        {

            var reader = GetObjectReaderConfigurationFor<NetzSummarischDetail>()
                .PropertyAliasFor(e => e.ErfassungsPeriod, "Jahr")
                .PropertyAliasFor(e => e.NetzSummarisch, "MittleresErhebungsJahr")
                .ConverterFor(e => e.ErfassungsPeriod, (s, p) => new ErfassungsPeriod())
                .ConverterFor(e => e.Belastungskategorie, (s, p) => new Belastungskategorie())
                .ConverterFor(e => e.Mandant, (s, p) => new Mandant())
                .ConverterFor(e => e.NetzSummarisch, (s, p) => new NetzSummarisch()
                    {
                        MittleresErhebungsJahr = string.IsNullOrEmpty(s) ? (DateTime?) null : DateTime.Parse(s)
                    });
            foreach (var item in reader.GetObjectReader().GetObjectListWithRow<NetzSummarischDetail>(table)
                .GroupBy(i => new { Year = int.Parse(i.Item1["Jahr"]), Madndat = i.Item1["Mandant"] }))
            {
                Guid mandantId;
                using (var nhScope = new NHibernateSpecflowScope())
                {
                    var main =
                        nhScope.Session.Query<NetzSummarisch>().Single(
                            e => e.Mandant.MandantName == item.Key.Madndat && !e.ErfassungsPeriod.IsClosed);
                    mandantId = main.Mandant.Id;
                    main.MittleresErhebungsJahr = item.First().Item2.NetzSummarisch != null ? item.First().Item2.NetzSummarisch.MittleresErhebungsJahr : null;
                    main.NetzSummarischDetails.Clear();
                    foreach (var d in item)
                    {
                        d.Item2.Belastungskategorie = nhScope.Session.Query<Belastungskategorie>().ToArray().Single(i => i.Typ == d.Item1["Belastungskategorie"]);
                        d.Item2.NetzSummarisch = main;
                        main.NetzSummarischDetails.Add(d.Item2);
                        nhScope.Session.Save(d.Item2);
                    }
                }
                CloseYear(item.Key.Year, mandantId);
            }
        }

        [Given(@"die folgenden KenngroessenFruehererJahre existieren:")]
        public void AngenommenDieFolgendenKenngroessenFruehererJahreExistieren(Table table)
        {
            foreach (var item in table.Rows.GroupBy(i => new { Jahr = i["Jahr"], Madndat = i["Mandant"] }))
            {
                using (var nhScope = new NHibernateSpecflowScope())
                {
                    var main = new KenngroessenFruehererJahre()
                                   {
                                       Jahr = int.Parse(item.Key.Jahr),
                                       Mandant = nhScope.Session.Query<Mandant>().Single(m => m.MandantName == item.Key.Madndat),
                                   };
                    if (item.First().ContainsKey("KostenFuerWerterhaltung"))
                        main.KostenFuerWerterhaltung = decimal.Parse(item.First()["KostenFuerWerterhaltung"]);
                    nhScope.Session.Save(main);
                    foreach (var row in item)
                    {
                        var d = new KenngroessenFruehererJahreDetail();
                        d.Belastungskategorie = nhScope.Session.Query<Belastungskategorie>().ToArray().Single(i => i.Typ == row["Belastungskategorie"]);
                        d.MittlererZustand = decimal.Parse(row["MittlererZustand"]);
                        d.Fahrbahnlaenge =  decimal.Parse(row["Fahrbahnlaenge"]);
                        d.Fahrbahnflaeche =  int.Parse(row["Fahrbahnflaeche"]);
                        d.KenngroessenFruehererJahre = main;
                        nhScope.Session.Save(d);
                    }
                }
            }
        }

        [Given(@"die folgende RealisierteMassnahmeSummarsich existieren:")]
        public void AngenommenDieFolgendeRealisierteMassnahmeSummarsichExistieren(Table table)
        {
            LoadRealisierteMassnahmeEntities<RealisierteMassnahmeSummarsich>(table, NetzErfassungsmodus.Summarisch);
        }

        private void LoadRealisierteMassnahmeEntities<TEnitity>(Table table, NetzErfassungsmodus erfassungsmodus) 
            where TEnitity : IErfassungsPeriodDependentEntity, IFlaecheFahrbahnUndTrottoirHolder,IRealisierteMassnahmeKostenHolder, IBelastungskategorieHolder, new()
        {
            var reader = GetObjectReaderConfigurationFor<TEnitity>()
                .PropertyAliasFor(e => e.ErfassungsPeriod, "Jahr")
                .ConverterFor(e => e.Belastungskategorie,
                              (s, p) =>
                              ScenarioContextWrapper.CurrentScope.Session.Query<Belastungskategorie>().ToArray().Single(
                                  i => i.Typ == s))
                .ConverterFor(e => e.Mandant, (s, p) => new Mandant() {MandantName = s})
                .ConverterFor(e => e.ErfassungsPeriod,
                              (s, p) => new ErfassungsPeriod() { Erfassungsjahr = new DateTime(int.Parse(s), 1, 1), NetzErfassungsmodus = erfassungsmodus });
            using (var nhScope = new NHibernateSpecflowScope())
            {
                entitesProErfassungsmodus[erfassungsmodus][typeof(TEnitity)] = (IEnumerable<IErfassungsPeriodDependentEntity>)reader.GetObjectReader().GetObjectList<TEnitity>(table);
            }
        }

        [Given(@"die folgende RealisierteMassnahme existieren:")]
        public void AngenommenDieFolgendeRealisierteMassnahmeExistieren(Table table)
        {
            LoadRealisierteMassnahmeEntities<RealisierteMassnahme>(table, NetzErfassungsmodus.Tabellarisch);
        }

        [Given(@"die folgende RealisierteMassnahmeGIS existieren:")]
        public void AngenommenDieFolgendeRealisierteMassnahmeGISExistieren(Table table)
        {
            LoadRealisierteMassnahmeEntities<RealisierteMassnahmeGIS>(table, NetzErfassungsmodus.Gis);
        }

        [Given(@"ich bin Benchmarkteilnehmer von '(.+)'")]
        public void AngenommenIchBinBenchmarkteilnehmerVon(string mandantName)
        {
            currentMandantName = mandantName;
        }

        [When(@"ich die Benchmarkauswertung zu meinen Inventarkennwerten für das Jahre '(.+)' unter Berücksichtigung der Klassen '(.+)' generiere")]
        public void WennIchDieBenchmarkauswertungFurDasJahreUnterBerucksichtigungDerKlassenNetzgrosseGemeindetypGeneriere(int currentYear, string gruppen)
        {
            WennIchDieBenchmarkauswertungFurDasJahreUnterBerucksichtigungDerKlassen
                <BenchmarkauswertungInventarkennwertenController, BenchmarkauswertungInventarkennwertenParameter, BenchmarkauswertungInventarkennwertenPo>(currentYear, gruppen);
        }

        [When(@"ich die Benchmarkauswertung zu meinen Zustandskennwerten für das Jahre '(.+)' unter Berücksichtigung der Klassen '(.+)' generiere")]
        public void WennIchDieBenchmarkauswertungZuMeinenZustandskennwertenFurDasJahreUnterBerucksichtigungDerKlassen(int currentYear, string gruppen)
        {
            WennIchDieBenchmarkauswertungFurDasJahreUnterBerucksichtigungDerKlassen
                <BenchmarkauswertungZustandskennwertenController, BenchmarkauswertungZustandskennwertenParameter, BenchmarkauswertungZustandskennwertenPo>(currentYear, gruppen);
        }

        [When(@"ich die Benchmarkauswertung zu meinen Kennwerten der realisierten Massnahmen für das Jahre '(.+)' unter Berücksichtigung der Klassen '(.+)' generiere")]
        public void WennIchDieBenchmarkauswertungZuMeinenKennwertenDerRealisiertenMassnahmenFurDasJahreUnterBerucksichtigungDerKlassen(int currentYear, string gruppen)
        {
            WennIchDieBenchmarkauswertungFurDasJahreUnterBerucksichtigungDerKlassen
                <BenchmarkauswertungKennwertenRealisiertenMassnahmenController, BenchmarkauswertungKennwertenRealisiertenMassnahmenParameter, BenchmarkauswertungKennwertenRealisiertenMassnahmenPo>(currentYear, gruppen);
        }

        private void WennIchDieBenchmarkauswertungFurDasJahreUnterBerucksichtigungDerKlassen<TControler, TParameter, TPo>(int currentYear, string gruppen)
            where TControler : BenchmarkauswertungControllerBase<TParameter, TPo>
            where TParameter : EmsgReportParameter, new()
            where TPo : new()
        {
            foreach (var entitesProModus in entitesProErfassungsmodus)
            {
                var years = entitesProModus.Value.SelectMany(e => e.Value).Select(m => m.ErfassungsPeriod.Erfassungsjahr.Year).Distinct().ToArray();

                foreach (var year in years)
                {
                    using (var nhScope = new NHibernateSpecflowScope())
                    {
                        foreach (var entity in entitesProModus.Value.SelectMany(e => e.Value).Where(m => m.ErfassungsPeriod.Erfassungsjahr.Year == year).ToArray())
                        {
                            entity.Mandant = nhScope.Session.Query<Mandant>().Single(m => m.MandantName == entity.Mandant.MandantName);
                            entity.ErfassungsPeriod = nhScope.Session.Query<ErfassungsPeriod>()
                                .Single(e => !e.IsClosed && e.Mandant == entity.Mandant);
                            entity.ErfassungsPeriod.NetzErfassungsmodus = entitesProModus.Key;
                            nhScope.Session.Save(entity);
                        }
                    }

                    foreach (var manadantId in entitesProModus.Value.SelectMany(e => e.Value).Select(m => m.Mandant.Id).Distinct())
                    {
                        CloseYear(year, manadantId);
                    }
                }
            }

            GenerateReport<TControler, TParameter, TPo>(currentYear, gruppen);
        }

        private void GenerateReport<TControler, TParameter, TPo>(int currentYear, string gruppen)
            where TControler : BenchmarkauswertungControllerBase<TParameter, TPo>
            where TParameter : EmsgReportParameter, new()
            where TPo : new()
        {
            Guid currentYearId;
            using (var nhScope = new NHibernateSpecflowScope())
            {
                var currentMandant = nhScope.Session.Query<Mandant>().Single(m => m.MandantName == currentMandantName);
                SetMandant(currentMandant.Id);
                var currentEp = nhScope.Session.Query<ErfassungsPeriod>().SingleOrDefault(e => e.Mandant == currentMandant && e.Erfassungsjahr.Year == currentYear);
                currentYearId = currentEp != null ? currentEp.Id : nhScope.Session.Query<KenngroessenFruehererJahre>().Single(e => e.Mandant == currentMandant && e.Jahr == currentYear).Id;
            }
            //GetBenchmarkauswertungPreview
            var nameValueCollection = new NameValueCollection();
            nameValueCollection["JahrId"] = currentYearId.ToString();
            var index = 0;
            foreach (var gruppe in gruppen.Split(','))
            {
                nameValueCollection.Add(string.Format("BenchmarkingGruppenTypList[{0}]", index), gruppe);
                index++;
            }

            BrowserDriver.InvokePostAction<TControler, TParameter>((c, r) => c.GetBenchmarkauswertungPreview(r), nameValueCollection);
            reportsSteps.GeneratReports((EmsgReportParameter)new TParameter(), rp => BrowserDriver.InvokePostAction<TControler, TParameter>((c, r) => c.GetReport(r), (TParameter)rp, false));
        }

        private void CloseYear(int year, Guid manadantId)
        {
            SetMandant(manadantId);
            administrationSteps.AngenommenIchEinenJahresabschlussFurDasJahrDurchfuhre(year.ToString());
        }

        private void SetMandant(Guid manadantId)
        {
            BrowserDriver.InvokeGetAction("/Header/SetMandant?mandantId=" + manadantId);
        }

        [Then(@"zeigt die Grafik folgende Ergebnisse '(.+)'")]
        public void DannZeigtDieGrafikFolgendeErgebnisseInventarkennwerten_EinwohnerGroesse(string refenz, Table table)
        {
            reportsSteps.DannIstDasErgebnisDasGleicheWieInDerReferenzAuswertung(refenz);
        }

        [Then(@"informiert mich das System, dass zu wenige Mandanten für die Benchmarkauswertung zur Verfügung stehen und die Auswertung nicht generiert werden kann")]
        public void DannInformiertMichDasSystemDassZuWenigeMandantenFurDieBenchmarkauswertungZurVerfugungStehenUndDieAuswertungNichtGeneriertWerdenKann()
        {
            Assert.AreEqual("NoData", BrowserDriver.GetRequestResult<TestFileContentResult>().ContentType);
        }

    }
}
