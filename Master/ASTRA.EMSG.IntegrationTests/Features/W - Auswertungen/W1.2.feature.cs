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
    [TechTalk.SpecRun.FeatureAttribute("W1.2 - Eine Liste der Strassenabschnitte erhalten", Description="\tAls Data-Reader\r\n\twill ich eine Liste der Strassenabschnitte erhalten\r\n\tdamit ic" +
        "h für Entscheidungsträger meiner Gemeinde eine Informationsbasis habe und einen " +
        "Überblick zu meinem Inventar erhalte", SourceFile="Features\\W - Auswertungen\\W1.2.feature", SourceLine=0)]
    public partial class W1_2_EineListeDerStrassenabschnitteErhaltenFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "W1.2.feature"
#line hidden
        
        [TechTalk.SpecRun.FeatureInitialize()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("de-AT"), "W1.2 - Eine Liste der Strassenabschnitte erhalten", "\tAls Data-Reader\r\n\twill ich eine Liste der Strassenabschnitte erhalten\r\n\tdamit ic" +
                    "h für Entscheidungsträger meiner Gemeinde eine Informationsbasis habe und einen " +
                    "Überblick zu meinem Inventar erhalte", ProgrammingLanguage.CSharp, ((string[])(null)));
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
                        "Abschnittsnummer",
                        "BezeichnungVon",
                        "BezeichnungBis",
                        "Strasseneigentümer",
                        "Belastungskategorie",
                        "Trottoir",
                        "Länge",
                        "Breite Fahrbahn",
                        "BreiteTrottoirLinks",
                        "BreiteTrottoirRechts"});
            table2.AddRow(new string[] {
                        "1",
                        "Kantonsstrasse",
                        "1",
                        "0.2",
                        "2.0",
                        "Gemeinde",
                        "III",
                        "kein Trottoir",
                        "1800",
                        "19.56",
                        "0",
                        "0"});
            table2.AddRow(new string[] {
                        "2",
                        "Kantonsstrasse",
                        "2",
                        "2.0",
                        "7.0",
                        "Gemeinde",
                        "IV",
                        "links",
                        "5000",
                        "19.00",
                        "2.32",
                        "0"});
            table2.AddRow(new string[] {
                        "3",
                        "Landstrasse",
                        "1",
                        "0.0",
                        "2.5",
                        "Privat",
                        "III",
                        "rechts",
                        "2500",
                        "20.00",
                        "0",
                        "1.9"});
            table2.AddRow(new string[] {
                        "4",
                        "Hauptstrasse",
                        "5",
                        "1.2",
                        "2.5",
                        "Gemeinde",
                        "II",
                        "beide",
                        "1300",
                        "15.00",
                        "1.4",
                        "1.35"});
            table2.AddRow(new string[] {
                        "5",
                        "Hauptstrasse",
                        "4",
                        "0.4",
                        "1.2",
                        "Korporation",
                        "IC",
                        "Noch nicht erfasst",
                        "800",
                        "8.45",
                        "",
                        ""});
            table2.AddRow(new string[] {
                        "6",
                        "Hauptstrasse",
                        "1",
                        "0.0",
                        "0.1",
                        "Korporation",
                        "IB",
                        "kein Trottoir",
                        "100",
                        "12.87",
                        "0",
                        "0"});
            table2.AddRow(new string[] {
                        "7",
                        "Hauptstrasse",
                        "2",
                        "0.1",
                        "0.2",
                        "Korporation",
                        "IB",
                        "rechts",
                        "100",
                        "12.87",
                        "0",
                        "1.1"});
            table2.AddRow(new string[] {
                        "8",
                        "Hauptstrasse",
                        "3",
                        "0.2",
                        "0.4",
                        "Korporation",
                        "IB",
                        "links",
                        "200",
                        "12.87",
                        "1.2",
                        "0"});
#line 11
testRunner.And("folgende Netzinformationen existieren:", ((string)(null)), table2, "Und ");
#line hidden
        }
        
        [TechTalk.SpecRun.ScenarioAttribute("Der Data-Reader kann die gewünschte Auswertung selektieren", new string[] {
                "Manuell"}, SourceLine=24)]
        public virtual void DerData_ReaderKannDieGewunschteAuswertungSelektieren()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Reader kann die gewünschte Auswertung selektieren", new string[] {
                        "Manuell"});
#line 25
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            this.ScenarioCleanup();
        }
        
        [TechTalk.SpecRun.ScenarioAttribute("Der Data-Reader kann ein Jahr auswählen, für das vom System eine Inventarauswertu" +
            "ng generiert werden soll", new string[] {
                "Manuell"}, SourceLine=29)]
        public virtual void DerData_ReaderKannEinJahrAuswahlenFurDasVomSystemEineInventarauswertungGeneriertWerdenSoll()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Reader kann ein Jahr auswählen, für das vom System eine Inventarauswertu" +
                    "ng generiert werden soll", new string[] {
                        "Manuell"});
#line 30
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            this.ScenarioCleanup();
        }
        
        [TechTalk.SpecRun.ScenarioAttribute("Vom System wird das Jahr des letzen Jahresabschlusses als default Wert vorselekti" +
            "ert", new string[] {
                "Manuell"}, SourceLine=34)]
        public virtual void VomSystemWirdDasJahrDesLetzenJahresabschlussesAlsDefaultWertVorselektiert()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Vom System wird das Jahr des letzen Jahresabschlusses als default Wert vorselekti" +
                    "ert", new string[] {
                        "Manuell"});
#line 35
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            this.ScenarioCleanup();
        }
        
        [TechTalk.SpecRun.ScenarioAttribute("Das System liefert eine Liste aller Strassenabschnitte des Mandanten", new string[] {
                "Manuell"}, SourceLine=39)]
        public virtual void DasSystemLiefertEineListeAllerStrassenabschnitteDesMandanten()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Das System liefert eine Liste aller Strassenabschnitte des Mandanten", new string[] {
                        "Manuell"});
#line 40
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 41
testRunner.When("ich die Liste aller Strassenabschnitte des Mandanten generiere", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Wenn ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Id",
                        "Strassenname",
                        "kmVon",
                        "kmBis",
                        "Strasseneigentümer",
                        "Belastungskategorie",
                        "Troittoir",
                        "FlächeFahrbahn",
                        "FlächeTrottoirLinks",
                        "FlächeTrottoirRechts"});
            table3.AddRow(new string[] {
                        "1",
                        "Kantonsstrasse",
                        "0.2",
                        "2.0",
                        "Gemeinde",
                        "III",
                        "kein Trottoir",
                        "35208",
                        "0",
                        "0"});
            table3.AddRow(new string[] {
                        "2",
                        "Kantonsstrasse",
                        "2.0",
                        "7.0",
                        "Gemeinde",
                        "IV",
                        "links",
                        "95000",
                        "11600",
                        "0"});
            table3.AddRow(new string[] {
                        "3",
                        "Landstrasse",
                        "0.0",
                        "2.5",
                        "Privat",
                        "III",
                        "rechts",
                        "50000",
                        "0",
                        "4750"});
            table3.AddRow(new string[] {
                        "4",
                        "Hauptstrasse",
                        "1.2",
                        "2.5",
                        "Gemeinde",
                        "II",
                        "beide",
                        "19500",
                        "1820",
                        "1755"});
            table3.AddRow(new string[] {
                        "5",
                        "Hauptstrasse",
                        "0.4",
                        "1.2",
                        "Korporation",
                        "IC",
                        "Noch nicht erfasst",
                        "6760",
                        "0",
                        "0"});
            table3.AddRow(new string[] {
                        "6",
                        "Hauptstrasse",
                        "0.0",
                        "0.1",
                        "Korporation",
                        "IB",
                        "kein Trottoir",
                        "1287",
                        "0",
                        "0"});
            table3.AddRow(new string[] {
                        "7",
                        "Hauptstrasse",
                        "0.1",
                        "0.2",
                        "Korporation",
                        "IB",
                        "rechts",
                        "1287",
                        "0",
                        "110"});
            table3.AddRow(new string[] {
                        "8",
                        "Hauptstrasse",
                        "0.2",
                        "0.4",
                        "Korporation",
                        "IB",
                        "links",
                        "2574",
                        "240",
                        "0"});
#line 42
testRunner.Then("zeigt die Liste folgende Daten:", ((string)(null)), table3, "Dann ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [TechTalk.SpecRun.ScenarioAttribute("Die Liste ist nach Strassenname und Abschnittsnummer sortiert", new string[] {
                "Manuell"}, SourceLine=55)]
        public virtual void DieListeIstNachStrassennameUndAbschnittsnummerSortiert()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Die Liste ist nach Strassenname und Abschnittsnummer sortiert", new string[] {
                        "Manuell"});
#line 56
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 57
testRunner.When("ich die Liste aller Strassenabschnitte des Mandanten generiere", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Wenn ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Id",
                        "Strassenname",
                        "Abschnittsnummer"});
            table4.AddRow(new string[] {
                        "6",
                        "Hauptstrasse",
                        "1"});
            table4.AddRow(new string[] {
                        "7",
                        "Hauptstrasse",
                        "2"});
            table4.AddRow(new string[] {
                        "8",
                        "Hauptstrasse",
                        "3"});
            table4.AddRow(new string[] {
                        "5",
                        "Hauptstrasse",
                        "4"});
            table4.AddRow(new string[] {
                        "4",
                        "Hauptstrasse",
                        "5"});
            table4.AddRow(new string[] {
                        "1",
                        "Kantonsstrasse",
                        "1"});
            table4.AddRow(new string[] {
                        "2",
                        "Kantonsstrasse",
                        "2"});
            table4.AddRow(new string[] {
                        "3",
                        "Landstrasse",
                        "1"});
#line 58
testRunner.Then("zeigt die Liste folgende Daten in dieser Reihenfolge:", ((string)(null)), table4, "Dann ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [TechTalk.SpecRun.ScenarioAttribute("Die Liste kann nach Strasseneigentümer gefiltert werden", new string[] {
                "Manuell"}, SourceLine=71)]
        public virtual void DieListeKannNachStrasseneigentumerGefiltertWerden()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Die Liste kann nach Strasseneigentümer gefiltert werden", new string[] {
                        "Manuell"});
#line 72
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 73
testRunner.When("ich die Liste aller Strassenabschnitte des Mandanten für Strasseneigentümer \'Geme" +
                    "inde\' generiere", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Wenn ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Id",
                        "Strassenname",
                        "kmVon",
                        "kmBis",
                        "Strasseneigentümer",
                        "Belastungskategorie",
                        "Troittoir",
                        "FlächeFahrbahn",
                        "FlächeTrottoirLinks",
                        "FlächeTrottoirRechts"});
            table5.AddRow(new string[] {
                        "1",
                        "Kantonsstrasse",
                        "0.2",
                        "2.0",
                        "Gemeinde",
                        "III",
                        "kein Trottoir",
                        "35208",
                        "0",
                        "0"});
            table5.AddRow(new string[] {
                        "2",
                        "Kantonsstrasse",
                        "2.0",
                        "7.0",
                        "Gemeinde",
                        "IV",
                        "links",
                        "95000",
                        "11600",
                        "0"});
            table5.AddRow(new string[] {
                        "4",
                        "Hauptstrasse",
                        "1.2",
                        "2.5",
                        "Gemeinde",
                        "II",
                        "beide",
                        "19500",
                        "1820",
                        "1755"});
#line 74
testRunner.Then("zeigt die Liste folgende Daten:", ((string)(null)), table5, "Dann ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [TechTalk.SpecRun.ScenarioAttribute("Der Data-Reader kann die Liste als Excel-File exportieren", new string[] {
                "Manuell"}, SourceLine=82)]
        public virtual void DerData_ReaderKannDieListeAlsExcel_FileExportieren()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Reader kann die Liste als Excel-File exportieren", new string[] {
                        "Manuell"});
#line 83
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
    [NUnit.Framework.DescriptionAttribute("W1.2 - Eine Liste der Strassenabschnitte erhalten")]
    public partial class W1_2_EineListeDerStrassenabschnitteErhaltenFeature_NUnit
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "W1.2.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("de-AT"), "W1.2 - Eine Liste der Strassenabschnitte erhalten", "\tAls Data-Reader\r\n\twill ich eine Liste der Strassenabschnitte erhalten\r\n\tdamit ic" +
                    "h für Entscheidungsträger meiner Gemeinde eine Informationsbasis habe und einen " +
                    "Überblick zu meinem Inventar erhalte", ProgrammingLanguage.CSharp, ((string[])(null)));
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
                        "Abschnittsnummer",
                        "BezeichnungVon",
                        "BezeichnungBis",
                        "Strasseneigentümer",
                        "Belastungskategorie",
                        "Trottoir",
                        "Länge",
                        "Breite Fahrbahn",
                        "BreiteTrottoirLinks",
                        "BreiteTrottoirRechts"});
            table2.AddRow(new string[] {
                        "1",
                        "Kantonsstrasse",
                        "1",
                        "0.2",
                        "2.0",
                        "Gemeinde",
                        "III",
                        "kein Trottoir",
                        "1800",
                        "19.56",
                        "0",
                        "0"});
            table2.AddRow(new string[] {
                        "2",
                        "Kantonsstrasse",
                        "2",
                        "2.0",
                        "7.0",
                        "Gemeinde",
                        "IV",
                        "links",
                        "5000",
                        "19.00",
                        "2.32",
                        "0"});
            table2.AddRow(new string[] {
                        "3",
                        "Landstrasse",
                        "1",
                        "0.0",
                        "2.5",
                        "Privat",
                        "III",
                        "rechts",
                        "2500",
                        "20.00",
                        "0",
                        "1.9"});
            table2.AddRow(new string[] {
                        "4",
                        "Hauptstrasse",
                        "5",
                        "1.2",
                        "2.5",
                        "Gemeinde",
                        "II",
                        "beide",
                        "1300",
                        "15.00",
                        "1.4",
                        "1.35"});
            table2.AddRow(new string[] {
                        "5",
                        "Hauptstrasse",
                        "4",
                        "0.4",
                        "1.2",
                        "Korporation",
                        "IC",
                        "Noch nicht erfasst",
                        "800",
                        "8.45",
                        "",
                        ""});
            table2.AddRow(new string[] {
                        "6",
                        "Hauptstrasse",
                        "1",
                        "0.0",
                        "0.1",
                        "Korporation",
                        "IB",
                        "kein Trottoir",
                        "100",
                        "12.87",
                        "0",
                        "0"});
            table2.AddRow(new string[] {
                        "7",
                        "Hauptstrasse",
                        "2",
                        "0.1",
                        "0.2",
                        "Korporation",
                        "IB",
                        "rechts",
                        "100",
                        "12.87",
                        "0",
                        "1.1"});
            table2.AddRow(new string[] {
                        "8",
                        "Hauptstrasse",
                        "3",
                        "0.2",
                        "0.4",
                        "Korporation",
                        "IB",
                        "links",
                        "200",
                        "12.87",
                        "1.2",
                        "0"});
#line 11
testRunner.And("folgende Netzinformationen existieren:", ((string)(null)), table2, "Und ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Der Data-Reader kann die gewünschte Auswertung selektieren")]
        [NUnit.Framework.CategoryAttribute("Manuell")]
        public virtual void DerData_ReaderKannDieGewunschteAuswertungSelektieren()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Reader kann die gewünschte Auswertung selektieren", new string[] {
                        "Manuell"});
#line 25
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Der Data-Reader kann ein Jahr auswählen, für das vom System eine Inventarauswertu" +
            "ng generiert werden soll")]
        [NUnit.Framework.CategoryAttribute("Manuell")]
        public virtual void DerData_ReaderKannEinJahrAuswahlenFurDasVomSystemEineInventarauswertungGeneriertWerdenSoll()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Reader kann ein Jahr auswählen, für das vom System eine Inventarauswertu" +
                    "ng generiert werden soll", new string[] {
                        "Manuell"});
#line 30
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Vom System wird das Jahr des letzen Jahresabschlusses als default Wert vorselekti" +
            "ert")]
        [NUnit.Framework.CategoryAttribute("Manuell")]
        public virtual void VomSystemWirdDasJahrDesLetzenJahresabschlussesAlsDefaultWertVorselektiert()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Vom System wird das Jahr des letzen Jahresabschlusses als default Wert vorselekti" +
                    "ert", new string[] {
                        "Manuell"});
#line 35
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Das System liefert eine Liste aller Strassenabschnitte des Mandanten")]
        [NUnit.Framework.CategoryAttribute("Manuell")]
        public virtual void DasSystemLiefertEineListeAllerStrassenabschnitteDesMandanten()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Das System liefert eine Liste aller Strassenabschnitte des Mandanten", new string[] {
                        "Manuell"});
#line 40
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 41
testRunner.When("ich die Liste aller Strassenabschnitte des Mandanten generiere", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Wenn ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Id",
                        "Strassenname",
                        "kmVon",
                        "kmBis",
                        "Strasseneigentümer",
                        "Belastungskategorie",
                        "Troittoir",
                        "FlächeFahrbahn",
                        "FlächeTrottoirLinks",
                        "FlächeTrottoirRechts"});
            table3.AddRow(new string[] {
                        "1",
                        "Kantonsstrasse",
                        "0.2",
                        "2.0",
                        "Gemeinde",
                        "III",
                        "kein Trottoir",
                        "35208",
                        "0",
                        "0"});
            table3.AddRow(new string[] {
                        "2",
                        "Kantonsstrasse",
                        "2.0",
                        "7.0",
                        "Gemeinde",
                        "IV",
                        "links",
                        "95000",
                        "11600",
                        "0"});
            table3.AddRow(new string[] {
                        "3",
                        "Landstrasse",
                        "0.0",
                        "2.5",
                        "Privat",
                        "III",
                        "rechts",
                        "50000",
                        "0",
                        "4750"});
            table3.AddRow(new string[] {
                        "4",
                        "Hauptstrasse",
                        "1.2",
                        "2.5",
                        "Gemeinde",
                        "II",
                        "beide",
                        "19500",
                        "1820",
                        "1755"});
            table3.AddRow(new string[] {
                        "5",
                        "Hauptstrasse",
                        "0.4",
                        "1.2",
                        "Korporation",
                        "IC",
                        "Noch nicht erfasst",
                        "6760",
                        "0",
                        "0"});
            table3.AddRow(new string[] {
                        "6",
                        "Hauptstrasse",
                        "0.0",
                        "0.1",
                        "Korporation",
                        "IB",
                        "kein Trottoir",
                        "1287",
                        "0",
                        "0"});
            table3.AddRow(new string[] {
                        "7",
                        "Hauptstrasse",
                        "0.1",
                        "0.2",
                        "Korporation",
                        "IB",
                        "rechts",
                        "1287",
                        "0",
                        "110"});
            table3.AddRow(new string[] {
                        "8",
                        "Hauptstrasse",
                        "0.2",
                        "0.4",
                        "Korporation",
                        "IB",
                        "links",
                        "2574",
                        "240",
                        "0"});
#line 42
testRunner.Then("zeigt die Liste folgende Daten:", ((string)(null)), table3, "Dann ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Die Liste ist nach Strassenname und Abschnittsnummer sortiert")]
        [NUnit.Framework.CategoryAttribute("Manuell")]
        public virtual void DieListeIstNachStrassennameUndAbschnittsnummerSortiert()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Die Liste ist nach Strassenname und Abschnittsnummer sortiert", new string[] {
                        "Manuell"});
#line 56
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 57
testRunner.When("ich die Liste aller Strassenabschnitte des Mandanten generiere", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Wenn ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Id",
                        "Strassenname",
                        "Abschnittsnummer"});
            table4.AddRow(new string[] {
                        "6",
                        "Hauptstrasse",
                        "1"});
            table4.AddRow(new string[] {
                        "7",
                        "Hauptstrasse",
                        "2"});
            table4.AddRow(new string[] {
                        "8",
                        "Hauptstrasse",
                        "3"});
            table4.AddRow(new string[] {
                        "5",
                        "Hauptstrasse",
                        "4"});
            table4.AddRow(new string[] {
                        "4",
                        "Hauptstrasse",
                        "5"});
            table4.AddRow(new string[] {
                        "1",
                        "Kantonsstrasse",
                        "1"});
            table4.AddRow(new string[] {
                        "2",
                        "Kantonsstrasse",
                        "2"});
            table4.AddRow(new string[] {
                        "3",
                        "Landstrasse",
                        "1"});
#line 58
testRunner.Then("zeigt die Liste folgende Daten in dieser Reihenfolge:", ((string)(null)), table4, "Dann ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Die Liste kann nach Strasseneigentümer gefiltert werden")]
        [NUnit.Framework.CategoryAttribute("Manuell")]
        public virtual void DieListeKannNachStrasseneigentumerGefiltertWerden()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Die Liste kann nach Strasseneigentümer gefiltert werden", new string[] {
                        "Manuell"});
#line 72
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 73
testRunner.When("ich die Liste aller Strassenabschnitte des Mandanten für Strasseneigentümer \'Geme" +
                    "inde\' generiere", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Wenn ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Id",
                        "Strassenname",
                        "kmVon",
                        "kmBis",
                        "Strasseneigentümer",
                        "Belastungskategorie",
                        "Troittoir",
                        "FlächeFahrbahn",
                        "FlächeTrottoirLinks",
                        "FlächeTrottoirRechts"});
            table5.AddRow(new string[] {
                        "1",
                        "Kantonsstrasse",
                        "0.2",
                        "2.0",
                        "Gemeinde",
                        "III",
                        "kein Trottoir",
                        "35208",
                        "0",
                        "0"});
            table5.AddRow(new string[] {
                        "2",
                        "Kantonsstrasse",
                        "2.0",
                        "7.0",
                        "Gemeinde",
                        "IV",
                        "links",
                        "95000",
                        "11600",
                        "0"});
            table5.AddRow(new string[] {
                        "4",
                        "Hauptstrasse",
                        "1.2",
                        "2.5",
                        "Gemeinde",
                        "II",
                        "beide",
                        "19500",
                        "1820",
                        "1755"});
#line 74
testRunner.Then("zeigt die Liste folgende Daten:", ((string)(null)), table5, "Dann ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Der Data-Reader kann die Liste als Excel-File exportieren")]
        [NUnit.Framework.CategoryAttribute("Manuell")]
        public virtual void DerData_ReaderKannDieListeAlsExcel_FileExportieren()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Reader kann die Liste als Excel-File exportieren", new string[] {
                        "Manuell"});
#line 83
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