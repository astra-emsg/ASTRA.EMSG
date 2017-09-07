using System;
using System.Collections.Generic;
using System.IO;
using System.Collections.Specialized;
using System.Reflection;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Models;
using ASTRA.EMSG.Business.Models.Strassennamen;
using ASTRA.EMSG.Business.Services;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices;
using ASTRA.EMSG.Business.Services.Import;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.IntegrationTests.Support;
using ASTRA.EMSG.IntegrationTests.Support.MvcTesting;
using ASTRA.EMSG.IntegrationTests.Support.ObjectReader;
using ASTRA.EMSG.Tests.Common;
using ASTRA.EMSG.Web.Areas.Common.GridCommands;
using ASTRA.EMSG.Web.Areas.NetzverwaltungStrassennamen.Controllers;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using Autofac;
using ClosedXML.Excel;
using NUnit.Framework;
using TechTalk.SpecFlow;
using NHibernate.Linq;
using System.Linq;
using Kendo.Mvc.UI;
using TechTalk.SpecFlow.Assist;

namespace ASTRA.EMSG.IntegrationTests.StepDefinitions
{
    [Binding]
    public class StrassenabschnittSteps : StepsBase
    {
        public static Dictionary<int, Guid> StrassenabschnittIds
        {
            get { return (Dictionary<int, Guid>)ScenarioContext.Current["StrassenabschnittIds"]; }
            set { ScenarioContext.Current["StrassenabschnittIds"] = value; }
        }

        public static Dictionary<Guid, Strassenabschnitt> Strassenabschnitten
        {
            get { return (Dictionary<Guid, Strassenabschnitt>)ScenarioContext.Current["Strassenabschnitten"]; }
            set { ScenarioContext.Current["Strassenabschnitten"] = value; }
        }

        public static Stream StrassenabschnittImportFileStream
        {
            get { return (Stream)ScenarioContext.Current["StrassenabschnittImportFileStream"]; }
            set { ScenarioContext.Current["StrassenabschnittImportFileStream"] = value; }
        }

        public static IContainer SpecFlowTestConatainer
        {
            get { return (IContainer)ScenarioContext.Current["SpecFlowTestConatainer"]; }
            set { ScenarioContext.Current["SpecFlowTestConatainer"] = value; }
        }

        private DataSourceRequest dataSourceRequest = new DataSourceRequest();

        public StrassenabschnittSteps(BrowserDriver browserDriver)
            : base(browserDriver)
        {
            StrassenabschnittIds = new Dictionary<int, Guid>();
            Strassenabschnitten = new Dictionary<Guid, Strassenabschnitt>();

            SpecFlowTestConatainer = new SpecFlowTestConatainerSetup().BuildContainer();
        }

        [Given(@"für Mandant '(.+)' existieren folgende Netzinformationen:")]
        public void AngenommenFurMandantExistierenFolgendeNetzinformationen(string mandant, Table table)
        {
            using (NHibernateSpecflowScope scope = new NHibernateSpecflowScope())
            {
                var strassenabschnittReader = GetStrassenabschnittReader();
                var strassenabschnitten = strassenabschnittReader.GetObjectList<Strassenabschnitt>(table);

                foreach (var strassenabschnitt in strassenabschnitten)
                {
                    strassenabschnitt.Mandant = scope.GetMandant(mandant);
                    strassenabschnitt.ErfassungsPeriod = scope.GetCurrentErfassungsperiod(mandant);
                    scope.Session.Save(strassenabschnitt);
                }
            }
        }

        [When(@"ich folgende Netzinformationen für ID '(.+)' eingebe:")]
        public void WennIchFolgendeNetzinformationenFurIdEingebe(int id, Table table)
        {
            StrassenabschnittModel strassenabschnittModel;
            using (new NHibernateSpecflowScope())
            {
                strassenabschnittModel = GetStrassenabschnittModelReader().GetObject<StrassenabschnittModel>(table);
            }

            strassenabschnittModel.Id = StrassenabschnittIds[id];

            BrowserDriver.InvokePostAction<NetzdefinitionUndStrassenabschnittController, StrassenabschnittModel>((c, r) => c.Update(r), strassenabschnittModel);

            if (!BrowserDriver.IsLastResultEmptyResult())
                BrowserDriver.GetRequestResult<TestPartialViewResult>();
        }

        [Then(@"sind folgende Netzinformationen im System:")]
        public void DannSindFolgendeNetzinformationenImSystem(Table table)
        {
            if (HasFieldValidationError())
                return;

            using (NHibernateSpecflowScope nHibernateSpecflowScope = new NHibernateSpecflowScope())
            {
                var strassenabschnittReader = GetStrassenabschnittReader();
                var strassenabschnitts = nHibernateSpecflowScope.Session.Query<Strassenabschnitt>().ToList();
                var areObjectListWithTableEqual = strassenabschnittReader.AreObjectListWithTableEqual(strassenabschnitts, table);

                Assert.IsTrue(areObjectListWithTableEqual);
            }
        }

        [When(@"ich folgende Netzinformationen eingebe:")]
        public void WennIchFolgendeNetzinformationenEingebe(Table table)
        {
            StrassenabschnittModel strassenabschnittModel;
            using (new NHibernateSpecflowScope())
            {
                strassenabschnittModel = GetStrassenabschnittModelReader().GetObject<StrassenabschnittModel>(table);
            }

            BrowserDriver.InvokePostAction<NetzdefinitionUndStrassenabschnittController, StrassenabschnittModel>((c, r) => c.Insert(r), strassenabschnittModel);
        }

        [When(@"ich Netzinformationen für ID '(.+)' lösche")]
        public void WennIchNetzinformationenFurIdLosche(int id)
        {
            BrowserDriver.InvokePostAction<NetzdefinitionUndStrassenabschnittController, object>((c, r) => c.Delete(dataSourceRequest, Guid.Empty), new { id = StrassenabschnittIds[id] });
        }

        private ObjectReader GetStrassenabschnittModelReader()
        {
            return GetObjectReaderConfigurationFor<StrassenabschnittModel>()
                .ConverterFor(e => e.Belastungskategorie, ConvertBelastungskategorieId)
                .PropertyAliasFor(e => e.Laenge, "Gesamtlänge")
                .GetObjectReader();
        }

        private ObjectReader GetStrassenabschnittReader()
        {
            return GetObjectReaderConfigurationFor<Strassenabschnitt>()
                .ConverterFor(e => e.Id, (s, propertyInfo) => ConvertId(s, StrassenabschnittIds))
                .ConverterFor(e => e.Belastungskategorie, (s, p) => ConvertBelastungskategorie(s))
                .PropertyAliasFor(e => e.Laenge, "Gesamtlänge")
                .GetObjectReader();
        }

        [When(@"ich nach Filterkriterium '(.+)' suche")]
        public void WennIchNachFilterkriteriumBahnSuche(string strassennameFilter)
        {
            var strassenabschnittGridCommand = new StrassenabschnittGridCommand { StrassennameFilter = strassennameFilter };
            BrowserDriver.InvokePostAction<NetzdefinitionUndStrassenabschnittController, StrassenabschnittGridCommand>((c, r) => c.GetAll(dataSourceRequest, r), strassenabschnittGridCommand);
        }

        [Then(@"existieren folgende Netzinformationen in der Übersichtstabelle: '(.+)'")]
        public void DannExistierenFolgendeNetzinformationenInDerUbersichtstabelle(string idList)
        {
            var serializableGridModel = BrowserDriver.GetCurrentModel<SerializableDataSourceResult>();

            var models = serializableGridModel.Data.OfType<StrassenabschnittOverviewModel>().ToList();

            if (idList.IsNull())
            {
                Assert.AreEqual(0, models.Count());
                return;
            }

            var ids = idList.Split(',').Select(s => s.Trim()).ToList();

            int resultCount = ids.Count();
            Assert.AreEqual(resultCount, models.Count());

            foreach (var id in ids)
                Assert.IsNotNull(models.SingleOrDefault(s => s.Id == StrassenabschnittIds[id.ParseInt()]));
        }

        [Then(@"ist folgende FlächeFahrbahn im System:")]
        [Then(@"ist folgende FlächeTrottoirLinks im System:")]
        [Then(@"ist folgende FlächeTrottoirRechts im System:")]
        [Then(@"ist folgende FlächeTrottoir im System:")]
        public void DannIstFolgendeCalculatedFieldsImSystem(Table table)
        {
            CheckStrassenabschnittCalculatedFields(table);
        }

        private static void CheckStrassenabschnittCalculatedFields(Table table)
        {
            var netzinformationCalculatedValuesRows = table.CreateSet<NetzinformationCalculatedValuesRow>();
            foreach (var row in netzinformationCalculatedValuesRows)
            {
                Guid strassenabschnittId = StrassenabschnittIds[row.Id];
                Strassenabschnitt strassenabschnitt = Strassenabschnitten[strassenabschnittId];

                if (!string.IsNullOrEmpty(row.FlächeFahrbahn))
                    Assert.AreEqual(row.FlächeFahrbahnValue, strassenabschnitt.FlaecheFahrbahn);
                if (!string.IsNullOrEmpty(row.FlächeTrottoirLinks))
                    Assert.AreEqual(row.FlächeTrottoirLinksValue, strassenabschnitt.FlaecheTrottoirLinks);
                if (!string.IsNullOrEmpty(row.FlächeTrottoirRechts))
                    Assert.AreEqual(row.FlächeTrottoirRechtsValue, strassenabschnitt.FlaecheTrottoirRechts);
                if (!string.IsNullOrEmpty(row.FlächeTrottoir))
                    Assert.AreEqual(row.FlächeTrottoirValue, strassenabschnitt.FlaecheTrottoir);
            }
        }

        [When(@"ich folgende Netzinformationen speichere")]
        public void WennIchFolgendeNetzinformationenSpeichere(Table table)
        {
            var strassenabschnittReader = GetObjectReaderConfigurationFor<Strassenabschnitt>()
                .ConverterFor(s => s.Id, (s, propertyInfo) => ConvertId(s, StrassenabschnittIds))
                .GetObjectReader();

            var strassenabschnitten = strassenabschnittReader.GetObjectList<Strassenabschnitt>(table);

            foreach (var strassenabschnitt in strassenabschnitten)
                Strassenabschnitten.Add(strassenabschnitt.Id, strassenabschnitt);
        }

        [Given(@"eine XLSX-Datei mit folgenden Zeilen \(im ersten Tab\):")]
        public void AngenommenEineXLSX_DateiMitFolgendenZeilenImErstenTab(Table table)
        {
            XLWorkbook templateXlWorkbook = GetTemplateXlWorkbook();
            IXLWorksheet xlWorksheet = templateXlWorkbook.Worksheets.First();

            var strassenabschnittExcelImportRows = table.CreateSet<StrassenabschnittExcelImportRow>();

            int rowNumber = 2;
            foreach (var row in strassenabschnittExcelImportRows)
            {
                WriteRow(xlWorksheet, rowNumber, new[]
                                             {
                                                 row.Strassenname,        //Strassenname
                                                 row.BezeichnungVon,      //BezeichnungVon
                                                 row.BezeichnungBis,      //BezeichnungBis
                                                 row.ExternalId,          //ExternalId
                                                 row.Abschnittsnummer,    //Abschnittsnummer
                                                 row.Strasseneigentümer,  //Strasseneigentuemer
                                                 row.Ortsbezeichnung,     //Ortsbezeichnung
                                                 row.Belastungskategorie, //BelastungskategorieTyp
                                                 row.Belag,               //Belag
                                                 row.BreiteFahrbahn,      //BreiteFahrbahn
                                                 row.Länge,               //Gesamtlaenge
                                                 row.Trottoir,            //Trottoir
                                                 row.BreiteTrottoirLinks, //BreiteTrottoirLinks
                                                 row.BreiteTrottoirRechts //BreiteTrottoirRechts
                                             });

                rowNumber++;
            }

            StrassenabschnittImportFileStream = new MemoryStream();
            templateXlWorkbook.SaveAs(StrassenabschnittImportFileStream);
            StrassenabschnittImportFileStream.Seek(0, 0);
        }

        private void WriteRow(IXLWorksheet ws, int rowNumber, string[] row)
        {
            if (row.Length != 14)
                throw new NotSupportedException();

            for (int i = 1; i <= 14; i++)
                ws.Cell(rowNumber, i).Value = row[i - 1];
        }

        private static XLWorkbook GetTemplateXlWorkbook()
        {
            var xlWorkbook = new XLWorkbook();
            IXLWorksheet ws = xlWorkbook.Worksheets.Add("Import");

            var localizationService = new StubLocalizationService();

            ws.Cell(1, 1).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, string>(s => s.Strassenname);
            ws.Cell(1, 2).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, string>(s => s.BezeichnungVon);
            ws.Cell(1, 3).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, string>(s => s.BezeichnungBis);
            ws.Cell(1, 4).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, string>(s => s.ExternalId);
            ws.Cell(1, 5).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, int?>(s => s.Abschnittsnummer);
            ws.Cell(1, 6).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, EigentuemerTyp>(s => s.Strasseneigentuemer);
            ws.Cell(1, 7).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, string>(s => s.Ortsbezeichnung);
            ws.Cell(1, 8).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, string>(s => s.BelastungskategorieTyp);
            ws.Cell(1, 9).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, BelagsTyp>(s => s.Belag);
            ws.Cell(1, 10).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, decimal?>(s => s.BreiteFahrbahn);
            ws.Cell(1, 11).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, decimal?>(s => s.Laenge);
            ws.Cell(1, 12).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, TrottoirTyp>(s => s.Trottoir);
            ws.Cell(1, 13).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, decimal?>(s => s.BreiteTrottoirLinks);
            ws.Cell(1, 14).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, decimal?>(s => s.BreiteTrottoirRechts);

            return xlWorkbook;
        }

        [Then(@"liefert der Import einen Validationsfehler")]
        public void DannLiefertDerImportEinenValidationsfehler()
        {
            var sessionService = SpecFlowTestConatainer.Resolve<ISessionService>();
            Assert.AreEqual(1, sessionService.LastStrassenabschnittImportResult.Errors.Count);
        }

        [Then(@"liefert der Import drei Validationsfehler")]
        public void DannLiefertDerImportDreiValidationsfehler()
        {
            var sessionService = SpecFlowTestConatainer.Resolve<ISessionService>();
            Assert.AreEqual(3, sessionService.LastStrassenabschnittImportResult.Errors.Count);
        }

        [When(@"ich die XLSX-Datei importiere")]
        public void WennIchDieXLSX_DateiImportiere()
        {
            using (var scope = new NHibernateSpecflowScope())
            {
                var strassenabschnittService = SpecFlowTestConatainer.Resolve<IStrassenabschnittXlsxImportService>();

                strassenabschnittService.ImportStrassenabschnitte(StrassenabschnittImportFileStream);
                strassenabschnittService.CommitStrassenabschnittImport();
            }
        }

        [When(@"ich den Strassenabschnitt mit der Id '(.+)' auf '(.+)' Teilabschnitte aufteile")]
        public void WennIchDenStrassenabschnittMitDerId1Auf3TeilabschnitteAufteile(string id, int parts)
        {
            BrowserDriver.InvokePostAction<NetzdefinitionUndStrassenabschnittController, SplitStrassenabschnittModel>(
                (c, r) => c.Split(r), new SplitStrassenabschnittModel() { Count = parts, StrassenabschnittId = StrassenabschnittIds[int.Parse(id)] }, false);
            Assert.IsNotInstanceOf<TestErrorResult>(BrowserDriver.GetRequestResult<TestResult>());
        }


        [When(@"ich für die Teilabschnitte folgende Daten eingebe:")]
        public void WennIchFurDieTeilabschnitteFolgendeDatenEingebe(Table table)
        {
            List<StrassenabschnittModel> l = null;

            using (new NHibernateSpecflowScope())
            {
                l = GetStrassenabschnittModelReader().GetObjectList<StrassenabschnittModel>(table);
            }

            var nameValueCollection = new NameValueCollection();
            int index = 0;
            foreach (var strassenabschnittModel in l)
            {
                nameValueCollection.Add(string.Format("[{0}].Id", index), StrassenabschnittIds.Values.First().ToString());
                foreach (var propertyInfo in typeof(StrassenabschnittModel).GetProperties().Where(p => p.Name != "Id"))
                {
                    object value = propertyInfo.GetValue(strassenabschnittModel, null);
                    nameValueCollection.Add(string.Format("[{0}].{1}", index, propertyInfo.Name), value != null ? value.ToString() : "");
                }
                index++;
            }

            BrowserDriver.InvokePostAction<NetzdefinitionUndStrassenabschnittController, List<StrassenabschnittSplitModel>>(
                (c, r) => c.InsertStrassenabschnitten(r), nameValueCollection);
            Assert.IsNotInstanceOf<TestErrorResult>(BrowserDriver.GetRequestResult<TestResult>());
        }
    }

    public class NetzinformationCalculatedValuesRow
    {
        public int Id { get; set; }

        public string FlächeFahrbahn { get; set; }
        public decimal? FlächeFahrbahnValue { get { return FlächeFahrbahn.ParseNullableDecimal(); } }

        public string FlächeTrottoirLinks { get; set; }
        public decimal? FlächeTrottoirLinksValue { get { return FlächeTrottoirLinks.ParseNullableDecimal(); } }

        public string FlächeTrottoirRechts { get; set; }
        public decimal? FlächeTrottoirRechtsValue { get { return FlächeTrottoirRechts.ParseNullableDecimal(); } }

        public string FlächeTrottoir { get; set; }
        public decimal? FlächeTrottoirValue { get { return FlächeTrottoir.ParseNullableDecimal(); } }
    }

    public class StrassenabschnittExcelImportRow
    {
        public string Strassenname { get; set; }
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }
        public string ExternalId { get; set; }
        public string Abschnittsnummer { get; set; }
        public string Strasseneigentümer { get; set; }
        public string Ortsbezeichnung { get; set; }
        public string Belastungskategorie { get; set; }
        public string Belag { get; set; }
        public string BreiteFahrbahn { get; set; }
        public string Länge { get; set; }
        public string Trottoir { get; set; }
        public string BreiteTrottoirLinks { get; set; }
        public string BreiteTrottoirRechts { get; set; }
    }
}
