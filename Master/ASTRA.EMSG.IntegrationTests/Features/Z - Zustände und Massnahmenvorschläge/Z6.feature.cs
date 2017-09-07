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
namespace ASTRA.EMSG.IntegrationTests.Features.Z_ZustandeUndMassnahmenvorschlage
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [TechTalk.SpecRun.FeatureAttribute("Inspektionsresultate aus EMSG-Mobile in EMSG-Master importieren", Description="\tAls Data-Manager,\r\n\twill ich Inspektionsresultate aus EMSG-Mobile in EMSG-Master" +
        " importieren\r\n\tdamit ich die Daten, die ich mit EMSG-Mobile erfasst habe in EMSG" +
        "-Master zur Verfügung habe", SourceFile="Features\\Z - Zustände und Massnahmenvorschläge\\Z6.feature", SourceLine=0)]
    public partial class InspektionsresultateAusEMSG_MobileInEMSG_MasterImportierenFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Z6.feature"
#line hidden
        
        [TechTalk.SpecRun.FeatureInitialize()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("de-AT"), "Inspektionsresultate aus EMSG-Mobile in EMSG-Master importieren", "\tAls Data-Manager,\r\n\twill ich Inspektionsresultate aus EMSG-Mobile in EMSG-Master" +
                    " importieren\r\n\tdamit ich die Daten, die ich mit EMSG-Mobile erfasst habe in EMSG" +
                    "-Master zur Verfügung habe", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        
        [TechTalk.SpecRun.ScenarioAttribute("Der Data-Manager kann auf dem EMSG-Mobile eine Änderungsdatei (ChangeLog) erzeuge" +
            "n und diese lokal abspeichern.", new string[] {
                "Manuell"}, SourceLine=8)]
        public virtual void DerData_ManagerKannAufDemEMSG_MobileEineAnderungsdateiChangeLogErzeugenUndDieseLokalAbspeichern_()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Manager kann auf dem EMSG-Mobile eine Änderungsdatei (ChangeLog) erzeuge" +
                    "n und diese lokal abspeichern.", new string[] {
                        "Manuell"});
#line 9
this.ScenarioSetup(scenarioInfo);
#line 10
testRunner.Given("einen beliebige Manipulation nach Z3.1 von Zustandsabschnitten", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Gegeben sei ");
#line 11
testRunner.And("ich wähle im EMSG Mobile \'Änderungsdatei / ChangeLog erzeugen\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 12
testRunner.And("ich wähle einen gültigen Pfad im Dateidialog", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 13
testRunner.Then("werden alle Änderungen in die Änderungsdatei geschrieben und gespeichert", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dann ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [TechTalk.SpecRun.ScenarioAttribute("Der Data-Manager kann das ChangeLog auf den EMSG-Master hochladen", new string[] {
                "Manuell"}, SourceLine=17)]
        public virtual void DerData_ManagerKannDasChangeLogAufDenEMSG_MasterHochladen()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Manager kann das ChangeLog auf den EMSG-Master hochladen", new string[] {
                        "Manuell"});
#line 18
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Mandant",
                        "Modus"});
            table1.AddRow(new string[] {
                        "Mandant_1",
                        "gis"});
#line 19
testRunner.Given("folgende Einstellungen existieren:", ((string)(null)), table1, "Gegeben sei ");
#line 23
testRunner.And("ich bin Data-Manager von \'Mandant_1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 24
testRunner.And("ich wähle die Seite \'Insepktionsergebnisse hochladen\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 25
testRunner.And("ich wähle eine gültige \'Änderungsdatei/ChangeLog\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 26
testRunner.Then("wird diese \'Änderungsdatei/ChangeLog\' auf den Server hochgeladen", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dann ");
#line 27
testRunner.And("der Import gestartet", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [TechTalk.SpecRun.ScenarioAttribute("Hochgeladene ChangeLogs werden automatisch in EMSG importiert", new string[] {
                "Manuell"}, SourceLine=31)]
        public virtual void HochgeladeneChangeLogsWerdenAutomatischInEMSGImportiert()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Hochgeladene ChangeLogs werden automatisch in EMSG importiert", new string[] {
                        "Manuell"});
#line 32
this.ScenarioSetup(scenarioInfo);
#line hidden
            this.ScenarioCleanup();
        }
        
        [TechTalk.SpecRun.ScenarioAttribute("Der Data-Manager kann im EMSG-Master die (zuvor ausgecheckten) Daten wieder bearb" +
            "eiten", new string[] {
                "Manuell"}, SourceLine=36)]
        public virtual void DerData_ManagerKannImEMSG_MasterDieZuvorAusgechecktenDatenWiederBearbeiten()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Manager kann im EMSG-Master die (zuvor ausgecheckten) Daten wieder bearb" +
                    "eiten", new string[] {
                        "Manuell"});
#line 37
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Mandant",
                        "Modus"});
            table2.AddRow(new string[] {
                        "Mandant_1",
                        "gis"});
#line 38
testRunner.Given("folgende Einstellungen existieren:", ((string)(null)), table2, "Gegeben sei ");
#line 41
testRunner.And("ich bin Data-Manager von \'Mandant_1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 42
testRunner.And("ich exportiere eine Inspektionsroute", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 43
testRunner.And("ich importiere die Änderungen für diese Inspektionsroute wieder am EMSG Master", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 44
testRunner.And("ich wähle einen Zustandsabschnitt, welcher Teil dieser Inspektionsroute ist", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 45
testRunner.Then("kann ich diesen nach Z3.2 manipulieren", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dann ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [TechTalk.SpecRun.ScenarioAttribute("Der Data-Manager sieht die Änderungen nach erfolgreichem Check-In auf der Karte u" +
            "nd in den Auswertungen", new string[] {
                "Manuell"}, SourceLine=49)]
        public virtual void DerData_ManagerSiehtDieAnderungenNachErfolgreichemCheck_InAufDerKarteUndInDenAuswertungen()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Manager sieht die Änderungen nach erfolgreichem Check-In auf der Karte u" +
                    "nd in den Auswertungen", new string[] {
                        "Manuell"});
#line 50
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Mandant",
                        "Modus"});
            table3.AddRow(new string[] {
                        "Mandant_1",
                        "gis"});
#line 51
testRunner.Given("folgende Einstellungen existieren:", ((string)(null)), table3, "Gegeben sei ");
#line 54
testRunner.And("ich bin Data-Manager von \'Mandant_1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 55
testRunner.And("ich exportiere eine Inspektionsroute", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 56
testRunner.And("ich erstelle einen neuen Zustandabschnitt am EMSG Mobile (nach Z3.1)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 57
testRunner.And("ich importiere die Änderungen im EMSG Master", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 58
testRunner.Then("wird mit der Zustandabschnitt auf der Karte visualisiert und in der Liste angezei" +
                    "gt und ich kann diesen nach Z3.2 bearbeiten", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dann ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [TechTalk.SpecRun.ScenarioAttribute("Der Data-Manager importiert das ChangeLog nur einmal in EMSG-Master", new string[] {
                "Manuell"}, SourceLine=62)]
        public virtual void DerData_ManagerImportiertDasChangeLogNurEinmalInEMSG_Master()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Manager importiert das ChangeLog nur einmal in EMSG-Master", new string[] {
                        "Manuell"});
#line 63
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Mandant",
                        "Modus"});
            table4.AddRow(new string[] {
                        "Mandant_1",
                        "gis"});
#line 64
testRunner.Given("folgende Einstellungen existieren:", ((string)(null)), table4, "Gegeben sei ");
#line 68
testRunner.And("ich bin Data-Manager von \'Mandant_1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 69
testRunner.And("es liegt ein ChangLog vor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 70
testRunner.And("ich lade und importiere dieses erfolgreich am EMSG Master", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 71
testRunner.And("ich versuche dieses nochmals hochzuladen und zu importieren", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 72
testRunner.Then("erhalte ich eine Fehlermeldung", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dann ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [TechTalk.SpecRun.ScenarioAttribute("Der Data-Manager erhält eine Erfolgsmeldung wenn die Check-In der Daten erfolgrei" +
            "ch war", new string[] {
                "Manuell"}, SourceLine=77)]
        public virtual void DerData_ManagerErhaltEineErfolgsmeldungWennDieCheck_InDerDatenErfolgreichWar()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Manager erhält eine Erfolgsmeldung wenn die Check-In der Daten erfolgrei" +
                    "ch war", new string[] {
                        "Manuell"});
#line 78
this.ScenarioSetup(scenarioInfo);
#line hidden
            this.ScenarioCleanup();
        }
        
        [TechTalk.SpecRun.ScenarioAttribute("Der Data-Manager erhält eine Fehlermeldung wenn ein Fehler während des Check-In\'s" +
            " aufgetreten ist.", new string[] {
                "Manuell"}, SourceLine=82)]
        public virtual void DerData_ManagerErhaltEineFehlermeldungWennEinFehlerWahrendDesCheck_InSAufgetretenIst_()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Manager erhält eine Fehlermeldung wenn ein Fehler während des Check-In\'s" +
                    " aufgetreten ist.", new string[] {
                        "Manuell"});
#line 83
this.ScenarioSetup(scenarioInfo);
#line hidden
            this.ScenarioCleanup();
        }
        
        [TechTalk.SpecRun.ScenarioAttribute("Der Data-Manager importiert immer nur alle getätigten Änderungen vom EMSG-Mobile " +
            "in EMSG-Master", new string[] {
                "Manuell"}, SourceLine=87)]
        public virtual void DerData_ManagerImportiertImmerNurAlleGetatigtenAnderungenVomEMSG_MobileInEMSG_Master()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Manager importiert immer nur alle getätigten Änderungen vom EMSG-Mobile " +
                    "in EMSG-Master", new string[] {
                        "Manuell"});
#line 88
this.ScenarioSetup(scenarioInfo);
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
    [NUnit.Framework.DescriptionAttribute("Inspektionsresultate aus EMSG-Mobile in EMSG-Master importieren")]
    public partial class InspektionsresultateAusEMSG_MobileInEMSG_MasterImportierenFeature_NUnit
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Z6.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("de-AT"), "Inspektionsresultate aus EMSG-Mobile in EMSG-Master importieren", "\tAls Data-Manager,\r\n\twill ich Inspektionsresultate aus EMSG-Mobile in EMSG-Master" +
                    " importieren\r\n\tdamit ich die Daten, die ich mit EMSG-Mobile erfasst habe in EMSG" +
                    "-Master zur Verfügung habe", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Der Data-Manager kann auf dem EMSG-Mobile eine Änderungsdatei (ChangeLog) erzeuge" +
            "n und diese lokal abspeichern.")]
        [NUnit.Framework.CategoryAttribute("Manuell")]
        public virtual void DerData_ManagerKannAufDemEMSG_MobileEineAnderungsdateiChangeLogErzeugenUndDieseLokalAbspeichern_()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Manager kann auf dem EMSG-Mobile eine Änderungsdatei (ChangeLog) erzeuge" +
                    "n und diese lokal abspeichern.", new string[] {
                        "Manuell"});
#line 9
this.ScenarioSetup(scenarioInfo);
#line 10
testRunner.Given("einen beliebige Manipulation nach Z3.1 von Zustandsabschnitten", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Gegeben sei ");
#line 11
testRunner.And("ich wähle im EMSG Mobile \'Änderungsdatei / ChangeLog erzeugen\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 12
testRunner.And("ich wähle einen gültigen Pfad im Dateidialog", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 13
testRunner.Then("werden alle Änderungen in die Änderungsdatei geschrieben und gespeichert", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dann ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Der Data-Manager kann das ChangeLog auf den EMSG-Master hochladen")]
        [NUnit.Framework.CategoryAttribute("Manuell")]
        public virtual void DerData_ManagerKannDasChangeLogAufDenEMSG_MasterHochladen()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Manager kann das ChangeLog auf den EMSG-Master hochladen", new string[] {
                        "Manuell"});
#line 18
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Mandant",
                        "Modus"});
            table1.AddRow(new string[] {
                        "Mandant_1",
                        "gis"});
#line 19
testRunner.Given("folgende Einstellungen existieren:", ((string)(null)), table1, "Gegeben sei ");
#line 23
testRunner.And("ich bin Data-Manager von \'Mandant_1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 24
testRunner.And("ich wähle die Seite \'Insepktionsergebnisse hochladen\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 25
testRunner.And("ich wähle eine gültige \'Änderungsdatei/ChangeLog\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 26
testRunner.Then("wird diese \'Änderungsdatei/ChangeLog\' auf den Server hochgeladen", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dann ");
#line 27
testRunner.And("der Import gestartet", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Hochgeladene ChangeLogs werden automatisch in EMSG importiert")]
        [NUnit.Framework.CategoryAttribute("Manuell")]
        public virtual void HochgeladeneChangeLogsWerdenAutomatischInEMSGImportiert()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Hochgeladene ChangeLogs werden automatisch in EMSG importiert", new string[] {
                        "Manuell"});
#line 32
this.ScenarioSetup(scenarioInfo);
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Der Data-Manager kann im EMSG-Master die (zuvor ausgecheckten) Daten wieder bearb" +
            "eiten")]
        [NUnit.Framework.CategoryAttribute("Manuell")]
        public virtual void DerData_ManagerKannImEMSG_MasterDieZuvorAusgechecktenDatenWiederBearbeiten()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Manager kann im EMSG-Master die (zuvor ausgecheckten) Daten wieder bearb" +
                    "eiten", new string[] {
                        "Manuell"});
#line 37
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Mandant",
                        "Modus"});
            table2.AddRow(new string[] {
                        "Mandant_1",
                        "gis"});
#line 38
testRunner.Given("folgende Einstellungen existieren:", ((string)(null)), table2, "Gegeben sei ");
#line 41
testRunner.And("ich bin Data-Manager von \'Mandant_1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 42
testRunner.And("ich exportiere eine Inspektionsroute", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 43
testRunner.And("ich importiere die Änderungen für diese Inspektionsroute wieder am EMSG Master", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 44
testRunner.And("ich wähle einen Zustandsabschnitt, welcher Teil dieser Inspektionsroute ist", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 45
testRunner.Then("kann ich diesen nach Z3.2 manipulieren", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dann ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Der Data-Manager sieht die Änderungen nach erfolgreichem Check-In auf der Karte u" +
            "nd in den Auswertungen")]
        [NUnit.Framework.CategoryAttribute("Manuell")]
        public virtual void DerData_ManagerSiehtDieAnderungenNachErfolgreichemCheck_InAufDerKarteUndInDenAuswertungen()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Manager sieht die Änderungen nach erfolgreichem Check-In auf der Karte u" +
                    "nd in den Auswertungen", new string[] {
                        "Manuell"});
#line 50
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Mandant",
                        "Modus"});
            table3.AddRow(new string[] {
                        "Mandant_1",
                        "gis"});
#line 51
testRunner.Given("folgende Einstellungen existieren:", ((string)(null)), table3, "Gegeben sei ");
#line 54
testRunner.And("ich bin Data-Manager von \'Mandant_1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 55
testRunner.And("ich exportiere eine Inspektionsroute", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 56
testRunner.And("ich erstelle einen neuen Zustandabschnitt am EMSG Mobile (nach Z3.1)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 57
testRunner.And("ich importiere die Änderungen im EMSG Master", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 58
testRunner.Then("wird mit der Zustandabschnitt auf der Karte visualisiert und in der Liste angezei" +
                    "gt und ich kann diesen nach Z3.2 bearbeiten", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dann ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Der Data-Manager importiert das ChangeLog nur einmal in EMSG-Master")]
        [NUnit.Framework.CategoryAttribute("Manuell")]
        public virtual void DerData_ManagerImportiertDasChangeLogNurEinmalInEMSG_Master()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Manager importiert das ChangeLog nur einmal in EMSG-Master", new string[] {
                        "Manuell"});
#line 63
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Mandant",
                        "Modus"});
            table4.AddRow(new string[] {
                        "Mandant_1",
                        "gis"});
#line 64
testRunner.Given("folgende Einstellungen existieren:", ((string)(null)), table4, "Gegeben sei ");
#line 68
testRunner.And("ich bin Data-Manager von \'Mandant_1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 69
testRunner.And("es liegt ein ChangLog vor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 70
testRunner.And("ich lade und importiere dieses erfolgreich am EMSG Master", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 71
testRunner.And("ich versuche dieses nochmals hochzuladen und zu importieren", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 72
testRunner.Then("erhalte ich eine Fehlermeldung", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dann ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Der Data-Manager erhält eine Erfolgsmeldung wenn die Check-In der Daten erfolgrei" +
            "ch war")]
        [NUnit.Framework.CategoryAttribute("Manuell")]
        public virtual void DerData_ManagerErhaltEineErfolgsmeldungWennDieCheck_InDerDatenErfolgreichWar()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Manager erhält eine Erfolgsmeldung wenn die Check-In der Daten erfolgrei" +
                    "ch war", new string[] {
                        "Manuell"});
#line 78
this.ScenarioSetup(scenarioInfo);
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Der Data-Manager erhält eine Fehlermeldung wenn ein Fehler während des Check-In\'s" +
            " aufgetreten ist.")]
        [NUnit.Framework.CategoryAttribute("Manuell")]
        public virtual void DerData_ManagerErhaltEineFehlermeldungWennEinFehlerWahrendDesCheck_InSAufgetretenIst_()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Manager erhält eine Fehlermeldung wenn ein Fehler während des Check-In\'s" +
                    " aufgetreten ist.", new string[] {
                        "Manuell"});
#line 83
this.ScenarioSetup(scenarioInfo);
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Der Data-Manager importiert immer nur alle getätigten Änderungen vom EMSG-Mobile " +
            "in EMSG-Master")]
        [NUnit.Framework.CategoryAttribute("Manuell")]
        public virtual void DerData_ManagerImportiertImmerNurAlleGetatigtenAnderungenVomEMSG_MobileInEMSG_Master()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Der Data-Manager importiert immer nur alle getätigten Änderungen vom EMSG-Mobile " +
                    "in EMSG-Master", new string[] {
                        "Manuell"});
#line 88
this.ScenarioSetup(scenarioInfo);
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
