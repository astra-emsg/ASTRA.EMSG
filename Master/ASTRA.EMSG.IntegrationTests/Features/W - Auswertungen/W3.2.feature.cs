﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.1.0.0
//      SpecFlow Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace ASTRA.EMSG.IntegrationTests.Features.W_Auswertungen
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [TechTalk.SpecRun.FeatureAttribute("W3.2 - Eine Grafik mit Zustandsspiegel, pro Belastungskategorie für mein ganzes N" +
        "etz erhalten", Description="    Als Data-Reader,\r\n\twill ich eine Grafik mit Zustandsspiegel, pro Belastungska" +
        "tegorie für mein ganzes Netz erhalten\r\n\tdamit ich für Entscheidungsträger meiner" +
        " Gemeinde eine Informationsbasis habe", SourceFile="Features\\W - Auswertungen\\W3.2.feature", SourceLine=0)]
    public partial class W3_2_EineGrafikMitZustandsspiegelProBelastungskategorieFurMeinGanzesNetzErhaltenFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "W3.2.feature"
#line hidden
        
        [TechTalk.SpecRun.FeatureInitialize()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("de-AT"), "W3.2 - Eine Grafik mit Zustandsspiegel, pro Belastungskategorie für mein ganzes N" +
                    "etz erhalten", "    Als Data-Reader,\r\n\twill ich eine Grafik mit Zustandsspiegel, pro Belastungska" +
                    "tegorie für mein ganzes Netz erhalten\r\n\tdamit ich für Entscheidungsträger meiner" +
                    " Gemeinde eine Informationsbasis habe", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [TechTalk.SpecRun.FeatureCleanup()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        [TechTalk.SpecRun.ScenarioCleanup()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 6
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Mandant",
                        "Modus"});
            table1.AddRow(new string[] {
                        "Mandant_1",
                        "strassennamen"});
#line 7
testRunner.Given("folgende Einstellungen existieren:", ((string)(null)), table1, "Gegeben sei ");
#line 10
testRunner.And("ich bin Data-Manager von \'Mandant_1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Id",
                        "Strassenname",
                        "BezeichnungVon",
                        "BezeichnungBis",
                        "BreiteFahrbahn",
                        "Trottoir",
                        "BreiteTrottoirLinks",
                        "BreiteTrottoirRechtss",
                        "Belastungskategorie",
                        "Länge"});
            table2.AddRow(new string[] {
                        "1",
                        "Jesuitenbachweg",
                        "Hackl",
                        "Schweighofer",
                        "4,5",
                        "Links",
                        "2,5",
                        "-",
                        "IA",
                        "10000"});
            table2.AddRow(new string[] {
                        "2",
                        "Lagerstrasse",
                        "Nr. 13",
                        "Nr. 22",
                        "5,75",
                        "NochNichtErfasst",
                        "-",
                        "-",
                        "IB",
                        "2400"});
            table2.AddRow(new string[] {
                        "3",
                        "Föhrenweg",
                        "Unterer Ortsteil",
                        "Lager",
                        "7",
                        "Keines",
                        "-",
                        "-",
                        "IC",
                        "1300"});
            table2.AddRow(new string[] {
                        "4",
                        "Gartenstrasse",
                        "1",
                        "66",
                        "5",
                        "BeideSeiten",
                        "2",
                        "1,5",
                        "II",
                        "700"});
#line 11
testRunner.And("folgende Netzinformationen existieren:", ((string)(null)), table2, "Und ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Id",
                        "Strassenname",
                        "BezeichnungVonStrasse",
                        "BezeichnungBisStrasse",
                        "BezeichnungVon",
                        "BezeichnungBis",
                        "Länge",
                        "ZustandsIndexFahrbahn",
                        "ZustandsindexTrottoirLinks",
                        "ZustandsindexTrottoirRechts"});
            table3.AddRow(new string[] {
                        "5",
                        "Jesuitenbachweg",
                        "Hackl",
                        "Schweighofer",
                        "Nr. 1",
                        "Nr. 7",
                        "1000",
                        "2,3",
                        "-",
                        "-"});
            table3.AddRow(new string[] {
                        "6",
                        "Jesuitenbachweg",
                        "Hackl",
                        "Schweighofer",
                        "Nr. 8",
                        "Nr. 12",
                        "800",
                        "2,3",
                        "-",
                        "-"});
            table3.AddRow(new string[] {
                        "7",
                        "Jesuitenbachweg",
                        "Hackl",
                        "Schweighofer",
                        "Nr. 13",
                        "Nr. 55",
                        "5000",
                        "1,2",
                        "Gut",
                        "-"});
            table3.AddRow(new string[] {
                        "8",
                        "Lagerstrasse",
                        "Nr. 13",
                        "Nr. 22",
                        "0.0",
                        "2.1",
                        "2100",
                        "1,1",
                        "-",
                        "-"});
            table3.AddRow(new string[] {
                        "9",
                        "Lagerstrasse",
                        "Nr. 13",
                        "Nr. 22",
                        "2.1",
                        "5.3",
                        "300",
                        "1,1",
                        "-",
                        "-"});
            table3.AddRow(new string[] {
                        "10",
                        "Lagerstrasse",
                        "Nr. 13",
                        "Nr. 22",
                        "5.4",
                        "7.1",
                        "100",
                        "2,1",
                        "-",
                        "-"});
            table3.AddRow(new string[] {
                        "11",
                        "Föhrenweg",
                        "Unterer Ortsteil",
                        "Lager",
                        "Brunner",
                        "Maier",
                        "1300",
                        "3,4",
                        "-",
                        "-"});
            table3.AddRow(new string[] {
                        "12",
                        "Gartenstrasse",
                        "1",
                        "66",
                        "1",
                        "66",
                        "700",
                        "3",
                        "Ausreichend",
                        "Mittel"});
#line 17
testRunner.And("folgende Zustandsinformationen existieren:", ((string)(null)), table3, "Und ");
#line hidden
        }
        
        [TechTalk.SpecRun.ScenarioAttribute("Der Data-Reader kann ein Jahr auswählen, für das vom System die Auswertung generi" +
            "ert werden soll", new string[] {
                "Manuell"}, SourceLine=30)]
        public virtual void DerData_ReaderKannEinJahrAuswahlenFurDasVomSystemDieAuswertungGeneriertWerdenSoll()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Reader kann ein Jahr auswählen, für das vom System die Auswertung generi" +
                    "ert werden soll", new string[] {
                        "Manuell"});
#line 31
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Jahr"});
            table4.AddRow(new string[] {
                        "2008"});
            table4.AddRow(new string[] {
                        "2009"});
#line 32
testRunner.Given("es gibt Jahresabschlüsse für folgende Jahre:", ((string)(null)), table4, "Gegeben sei ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Jahr"});
            table5.AddRow(new string[] {
                        "Aktuelles Erfassungsjahr"});
            table5.AddRow(new string[] {
                        "2009"});
            table5.AddRow(new string[] {
                        "2008"});
#line 36
testRunner.Then("kann ich folgende Jahre für ausgefüllte Erfassungsformulare auswählen:", ((string)(null)), table5, "Dann ");
#line 41
testRunner.And("der Eintrag \'Aktuelles Erfassungsjahr\' ist vorausgewählt", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [TechTalk.SpecRun.ScenarioAttribute("Das System generiert eine Vorschau der Auswertung am UI", new string[] {
                "Manuell"}, SourceLine=45)]
        public virtual void DasSystemGeneriertEineVorschauDerAuswertungAmUI()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Das System generiert eine Vorschau der Auswertung am UI", new string[] {
                        "Manuell"});
#line 46
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            this.ScenarioCleanup();
        }
        
        [TechTalk.SpecRun.ScenarioAttribute("Das System liefert eine Grafik mit Zustandsspiegel pro Belastungskategorie des St" +
            "rassennetzes des Mandanten", new string[] {
                "Manuell"}, SourceLine=51)]
        public virtual void DasSystemLiefertEineGrafikMitZustandsspiegelProBelastungskategorieDesStrassennetzesDesMandanten()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Das System liefert eine Grafik mit Zustandsspiegel pro Belastungskategorie des St" +
                    "rassennetzes des Mandanten", new string[] {
                        "Manuell"});
#line 52
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 53
testRunner.When("ich einen Jahresabschluss für das Jahr \'2010\' durchführe", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Wenn ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Filter",
                        "Filter Wert"});
            table6.AddRow(new string[] {
                        "Erfassungsperiode",
                        "2010"});
#line 54
testRunner.And("ich die Grafik mit Zustandsspiegel pro Belastungskategorie für mein ganzes Netz g" +
                    "eneriere", ((string)(null)), table6, "Und ");
#line 57
testRunner.Then("ist das Ergebnis das gleiche wie in der Referenz Auswertung \'W3.2_Strassennamen_M" +
                    "andant1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dann ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Belastungskategorie",
                        "Zustandindex",
                        "FlächeFahrbahn",
                        "FlächeTrottoirLinks",
                        "FlächeTrottoirRechts"});
            table7.AddRow(new string[] {
                        "IA",
                        "0.0 - 0.9",
                        "0%",
                        "100%",
                        "-"});
            table7.AddRow(new string[] {
                        "IA",
                        "1.0 - 1.9",
                        "74%",
                        "0%",
                        "-"});
            table7.AddRow(new string[] {
                        "IA",
                        "2.0 - 2.9",
                        "26%",
                        "0%",
                        "-"});
            table7.AddRow(new string[] {
                        "IA",
                        "3.0 - 3.9",
                        "0%",
                        "0%",
                        "-"});
            table7.AddRow(new string[] {
                        "IA",
                        "4.0 - 5.0",
                        "0%",
                        "0%",
                        "-"});
            table7.AddRow(new string[] {
                        "IA",
                        "Anteil des erfassten Netzes",
                        "100%",
                        "74%",
                        "-"});
            table7.AddRow(new string[] {
                        "IB",
                        "0.0 - 0.9",
                        "0%",
                        "-",
                        "-"});
            table7.AddRow(new string[] {
                        "IB",
                        "1.0 - 1.9",
                        "96%",
                        "-",
                        "-"});
            table7.AddRow(new string[] {
                        "IB",
                        "2.0 - 2.9",
                        "4%",
                        "-",
                        "-"});
            table7.AddRow(new string[] {
                        "IB",
                        "3.0 - 3.9",
                        "0%",
                        "-",
                        "-"});
            table7.AddRow(new string[] {
                        "IB",
                        "4.0 - 5.0",
                        "0%",
                        "-",
                        "-"});
            table7.AddRow(new string[] {
                        "IB",
                        "Anteil des erfassten Netzes",
                        "100%",
                        "",
                        ""});
            table7.AddRow(new string[] {
                        "IC",
                        "0.0 - 0.9",
                        "0%",
                        "-",
                        "-"});
            table7.AddRow(new string[] {
                        "IC",
                        "1.0 - 1.9",
                        "0%",
                        "-",
                        "-"});
            table7.AddRow(new string[] {
                        "IC",
                        "2.0 - 2.9",
                        "0%",
                        "-",
                        "-"});
            table7.AddRow(new string[] {
                        "IC",
                        "3.0 - 3.9",
                        "100%",
                        "-",
                        "-"});
            table7.AddRow(new string[] {
                        "IC",
                        "4.0 - 5.0",
                        "0%",
                        "-",
                        "-"});
            table7.AddRow(new string[] {
                        "IC",
                        "Anteil des erfassten Netzes",
                        "100%",
                        "",
                        ""});
            table7.AddRow(new string[] {
                        "II",
                        "0.0 - 0.9",
                        "0%",
                        "0%",
                        "0%"});
            table7.AddRow(new string[] {
                        "II",
                        "1.0 - 1.9",
                        "0%",
                        "0%",
                        "100%"});
            table7.AddRow(new string[] {
                        "II",
                        "2.0 - 2.9",
                        "0%",
                        "100%",
                        "0%"});
            table7.AddRow(new string[] {
                        "II",
                        "3.0 - 3.9",
                        "100%",
                        "0%",
                        "0%"});
            table7.AddRow(new string[] {
                        "II",
                        "4.0 - 5.0",
                        "0%",
                        "0%",
                        "0%"});
            table7.AddRow(new string[] {
                        "II",
                        "Anteil des erfassten Netzes",
                        "100%",
                        "100%",
                        "100%"});
#line 58
testRunner.Then("zeigen die Grafiken folgende Verteilung: (manuell)", ((string)(null)), table7, "Dann ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [TechTalk.SpecRun.ScenarioAttribute("Der Data-Reader kann die Graphik filtern", new string[] {
                "Manuell"}, SourceLine=87)]
        public virtual void DerData_ReaderKannDieGraphikFiltern()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Reader kann die Graphik filtern", new string[] {
                        "Manuell"});
#line 88
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            this.ScenarioCleanup();
        }
        
        [TechTalk.SpecRun.ScenarioAttribute("Der Data-Reader kann die Vorschau als PDF exportieren", new string[] {
                "Manuell"}, SourceLine=92)]
        public virtual void DerData_ReaderKannDieVorschauAlsPDFExportieren()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Reader kann die Vorschau als PDF exportieren", new string[] {
                        "Manuell"});
#line 93
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            this.ScenarioCleanup();
        }
        
        [TechTalk.SpecRun.TestRunCleanup()]
        public virtual void TestRunCleanup()
        {
            TechTalk.SpecFlow.TestRunnerManager.GetTestRunner().OnTestRunEnd();
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("W3.2 - Eine Grafik mit Zustandsspiegel, pro Belastungskategorie für mein ganzes N" +
        "etz erhalten")]
    public partial class W3_2_EineGrafikMitZustandsspiegelProBelastungskategorieFurMeinGanzesNetzErhaltenFeature_NUnit
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "W3.2.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("de-AT"), "W3.2 - Eine Grafik mit Zustandsspiegel, pro Belastungskategorie für mein ganzes N" +
                    "etz erhalten", "    Als Data-Reader,\r\n\twill ich eine Grafik mit Zustandsspiegel, pro Belastungska" +
                    "tegorie für mein ganzes Netz erhalten\r\n\tdamit ich für Entscheidungsträger meiner" +
                    " Gemeinde eine Informationsbasis habe", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 6
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Mandant",
                        "Modus"});
            table1.AddRow(new string[] {
                        "Mandant_1",
                        "strassennamen"});
#line 7
testRunner.Given("folgende Einstellungen existieren:", ((string)(null)), table1, "Gegeben sei ");
#line 10
testRunner.And("ich bin Data-Manager von \'Mandant_1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Id",
                        "Strassenname",
                        "BezeichnungVon",
                        "BezeichnungBis",
                        "BreiteFahrbahn",
                        "Trottoir",
                        "BreiteTrottoirLinks",
                        "BreiteTrottoirRechtss",
                        "Belastungskategorie",
                        "Länge"});
            table2.AddRow(new string[] {
                        "1",
                        "Jesuitenbachweg",
                        "Hackl",
                        "Schweighofer",
                        "4,5",
                        "Links",
                        "2,5",
                        "-",
                        "IA",
                        "10000"});
            table2.AddRow(new string[] {
                        "2",
                        "Lagerstrasse",
                        "Nr. 13",
                        "Nr. 22",
                        "5,75",
                        "NochNichtErfasst",
                        "-",
                        "-",
                        "IB",
                        "2400"});
            table2.AddRow(new string[] {
                        "3",
                        "Föhrenweg",
                        "Unterer Ortsteil",
                        "Lager",
                        "7",
                        "Keines",
                        "-",
                        "-",
                        "IC",
                        "1300"});
            table2.AddRow(new string[] {
                        "4",
                        "Gartenstrasse",
                        "1",
                        "66",
                        "5",
                        "BeideSeiten",
                        "2",
                        "1,5",
                        "II",
                        "700"});
#line 11
testRunner.And("folgende Netzinformationen existieren:", ((string)(null)), table2, "Und ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Id",
                        "Strassenname",
                        "BezeichnungVonStrasse",
                        "BezeichnungBisStrasse",
                        "BezeichnungVon",
                        "BezeichnungBis",
                        "Länge",
                        "ZustandsIndexFahrbahn",
                        "ZustandsindexTrottoirLinks",
                        "ZustandsindexTrottoirRechts"});
            table3.AddRow(new string[] {
                        "5",
                        "Jesuitenbachweg",
                        "Hackl",
                        "Schweighofer",
                        "Nr. 1",
                        "Nr. 7",
                        "1000",
                        "2,3",
                        "-",
                        "-"});
            table3.AddRow(new string[] {
                        "6",
                        "Jesuitenbachweg",
                        "Hackl",
                        "Schweighofer",
                        "Nr. 8",
                        "Nr. 12",
                        "800",
                        "2,3",
                        "-",
                        "-"});
            table3.AddRow(new string[] {
                        "7",
                        "Jesuitenbachweg",
                        "Hackl",
                        "Schweighofer",
                        "Nr. 13",
                        "Nr. 55",
                        "5000",
                        "1,2",
                        "Gut",
                        "-"});
            table3.AddRow(new string[] {
                        "8",
                        "Lagerstrasse",
                        "Nr. 13",
                        "Nr. 22",
                        "0.0",
                        "2.1",
                        "2100",
                        "1,1",
                        "-",
                        "-"});
            table3.AddRow(new string[] {
                        "9",
                        "Lagerstrasse",
                        "Nr. 13",
                        "Nr. 22",
                        "2.1",
                        "5.3",
                        "300",
                        "1,1",
                        "-",
                        "-"});
            table3.AddRow(new string[] {
                        "10",
                        "Lagerstrasse",
                        "Nr. 13",
                        "Nr. 22",
                        "5.4",
                        "7.1",
                        "100",
                        "2,1",
                        "-",
                        "-"});
            table3.AddRow(new string[] {
                        "11",
                        "Föhrenweg",
                        "Unterer Ortsteil",
                        "Lager",
                        "Brunner",
                        "Maier",
                        "1300",
                        "3,4",
                        "-",
                        "-"});
            table3.AddRow(new string[] {
                        "12",
                        "Gartenstrasse",
                        "1",
                        "66",
                        "1",
                        "66",
                        "700",
                        "3",
                        "Ausreichend",
                        "Mittel"});
#line 17
testRunner.And("folgende Zustandsinformationen existieren:", ((string)(null)), table3, "Und ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Der Data-Reader kann ein Jahr auswählen, für das vom System die Auswertung generi" +
            "ert werden soll")]
        [NUnit.Framework.CategoryAttribute("Manuell")]
        public virtual void DerData_ReaderKannEinJahrAuswahlenFurDasVomSystemDieAuswertungGeneriertWerdenSoll()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Reader kann ein Jahr auswählen, für das vom System die Auswertung generi" +
                    "ert werden soll", new string[] {
                        "Manuell"});
#line 31
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Jahr"});
            table4.AddRow(new string[] {
                        "2008"});
            table4.AddRow(new string[] {
                        "2009"});
#line 32
testRunner.Given("es gibt Jahresabschlüsse für folgende Jahre:", ((string)(null)), table4, "Gegeben sei ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Jahr"});
            table5.AddRow(new string[] {
                        "Aktuelles Erfassungsjahr"});
            table5.AddRow(new string[] {
                        "2009"});
            table5.AddRow(new string[] {
                        "2008"});
#line 36
testRunner.Then("kann ich folgende Jahre für ausgefüllte Erfassungsformulare auswählen:", ((string)(null)), table5, "Dann ");
#line 41
testRunner.And("der Eintrag \'Aktuelles Erfassungsjahr\' ist vorausgewählt", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Das System generiert eine Vorschau der Auswertung am UI")]
        [NUnit.Framework.CategoryAttribute("Manuell")]
        public virtual void DasSystemGeneriertEineVorschauDerAuswertungAmUI()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Das System generiert eine Vorschau der Auswertung am UI", new string[] {
                        "Manuell"});
#line 46
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Das System liefert eine Grafik mit Zustandsspiegel pro Belastungskategorie des St" +
            "rassennetzes des Mandanten")]
        [NUnit.Framework.CategoryAttribute("Manuell")]
        public virtual void DasSystemLiefertEineGrafikMitZustandsspiegelProBelastungskategorieDesStrassennetzesDesMandanten()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Das System liefert eine Grafik mit Zustandsspiegel pro Belastungskategorie des St" +
                    "rassennetzes des Mandanten", new string[] {
                        "Manuell"});
#line 52
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 53
testRunner.When("ich einen Jahresabschluss für das Jahr \'2010\' durchführe", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Wenn ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Filter",
                        "Filter Wert"});
            table6.AddRow(new string[] {
                        "Erfassungsperiode",
                        "2010"});
#line 54
testRunner.And("ich die Grafik mit Zustandsspiegel pro Belastungskategorie für mein ganzes Netz g" +
                    "eneriere", ((string)(null)), table6, "Und ");
#line 57
testRunner.Then("ist das Ergebnis das gleiche wie in der Referenz Auswertung \'W3.2_Strassennamen_M" +
                    "andant1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dann ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Belastungskategorie",
                        "Zustandindex",
                        "FlächeFahrbahn",
                        "FlächeTrottoirLinks",
                        "FlächeTrottoirRechts"});
            table7.AddRow(new string[] {
                        "IA",
                        "0.0 - 0.9",
                        "0%",
                        "100%",
                        "-"});
            table7.AddRow(new string[] {
                        "IA",
                        "1.0 - 1.9",
                        "74%",
                        "0%",
                        "-"});
            table7.AddRow(new string[] {
                        "IA",
                        "2.0 - 2.9",
                        "26%",
                        "0%",
                        "-"});
            table7.AddRow(new string[] {
                        "IA",
                        "3.0 - 3.9",
                        "0%",
                        "0%",
                        "-"});
            table7.AddRow(new string[] {
                        "IA",
                        "4.0 - 5.0",
                        "0%",
                        "0%",
                        "-"});
            table7.AddRow(new string[] {
                        "IA",
                        "Anteil des erfassten Netzes",
                        "100%",
                        "74%",
                        "-"});
            table7.AddRow(new string[] {
                        "IB",
                        "0.0 - 0.9",
                        "0%",
                        "-",
                        "-"});
            table7.AddRow(new string[] {
                        "IB",
                        "1.0 - 1.9",
                        "96%",
                        "-",
                        "-"});
            table7.AddRow(new string[] {
                        "IB",
                        "2.0 - 2.9",
                        "4%",
                        "-",
                        "-"});
            table7.AddRow(new string[] {
                        "IB",
                        "3.0 - 3.9",
                        "0%",
                        "-",
                        "-"});
            table7.AddRow(new string[] {
                        "IB",
                        "4.0 - 5.0",
                        "0%",
                        "-",
                        "-"});
            table7.AddRow(new string[] {
                        "IB",
                        "Anteil des erfassten Netzes",
                        "100%",
                        "",
                        ""});
            table7.AddRow(new string[] {
                        "IC",
                        "0.0 - 0.9",
                        "0%",
                        "-",
                        "-"});
            table7.AddRow(new string[] {
                        "IC",
                        "1.0 - 1.9",
                        "0%",
                        "-",
                        "-"});
            table7.AddRow(new string[] {
                        "IC",
                        "2.0 - 2.9",
                        "0%",
                        "-",
                        "-"});
            table7.AddRow(new string[] {
                        "IC",
                        "3.0 - 3.9",
                        "100%",
                        "-",
                        "-"});
            table7.AddRow(new string[] {
                        "IC",
                        "4.0 - 5.0",
                        "0%",
                        "-",
                        "-"});
            table7.AddRow(new string[] {
                        "IC",
                        "Anteil des erfassten Netzes",
                        "100%",
                        "",
                        ""});
            table7.AddRow(new string[] {
                        "II",
                        "0.0 - 0.9",
                        "0%",
                        "0%",
                        "0%"});
            table7.AddRow(new string[] {
                        "II",
                        "1.0 - 1.9",
                        "0%",
                        "0%",
                        "100%"});
            table7.AddRow(new string[] {
                        "II",
                        "2.0 - 2.9",
                        "0%",
                        "100%",
                        "0%"});
            table7.AddRow(new string[] {
                        "II",
                        "3.0 - 3.9",
                        "100%",
                        "0%",
                        "0%"});
            table7.AddRow(new string[] {
                        "II",
                        "4.0 - 5.0",
                        "0%",
                        "0%",
                        "0%"});
            table7.AddRow(new string[] {
                        "II",
                        "Anteil des erfassten Netzes",
                        "100%",
                        "100%",
                        "100%"});
#line 58
testRunner.Then("zeigen die Grafiken folgende Verteilung: (manuell)", ((string)(null)), table7, "Dann ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Der Data-Reader kann die Graphik filtern")]
        [NUnit.Framework.CategoryAttribute("Manuell")]
        public virtual void DerData_ReaderKannDieGraphikFiltern()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Reader kann die Graphik filtern", new string[] {
                        "Manuell"});
#line 88
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Der Data-Reader kann die Vorschau als PDF exportieren")]
        [NUnit.Framework.CategoryAttribute("Manuell")]
        public virtual void DerData_ReaderKannDieVorschauAlsPDFExportieren()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Reader kann die Vorschau als PDF exportieren", new string[] {
                        "Manuell"});
#line 93
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion