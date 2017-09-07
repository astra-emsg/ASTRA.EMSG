using System.IO;
using System.Linq;
using ASTRA.EMSG.Business.Models.Strassennamen;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.EntityServices.Strassennamen;
using ASTRA.EMSG.Business.Services.Import;
using ASTRA.EMSG.Business.Validators.Strassennamen;
using ASTRA.EMSG.Web.Infrastructure;
using ClosedXML.Excel;
using FluentValidation;
using NUnit.Framework;
using Moq;

namespace ASTRA.EMSG.Tests.XlsxImportTests
{
    [TestFixture]
    public class StrassenabschnittXlsxImportServiceTests
    {
        [Test]
        public void TestXlsxWhitMissingSheet()
        {
            ILocalizationService localizationService = new StubLocalizationService();
            var strassenabschnittXlsxImportService = GetStrassenabschnittXlsxImportService(localizationService);

            Stream memoryStream = new MemoryStream();
            var xlWorkbook = new XLWorkbook();
            xlWorkbook.SaveAs(memoryStream);

            var importResultModel = strassenabschnittXlsxImportService.ImportStrassenabschnitte(memoryStream);

            Assert.AreEqual(1, importResultModel.Errors.Count);
            Assert.AreEqual(localizationService.GetLocalizedError(ValidationError.WrongFileFormat), importResultModel.Errors.First());
        }

        [Test]
        public void TestEmptyStream()
        {
            ILocalizationService localizationService = new StubLocalizationService();
            var strassenabschnittXlsxImportService = GetStrassenabschnittXlsxImportService(localizationService);

            var importResultModel = strassenabschnittXlsxImportService.ImportStrassenabschnitte(new MemoryStream());

            Assert.AreEqual(1, importResultModel.Errors.Count);
            Assert.AreEqual(localizationService.GetLocalizedError(ValidationError.WrongFileFormat), importResultModel.Errors.First());
        }

        [Test]
        public void TestLessHeaderThanExpected()
        {
            ILocalizationService localizationService = new StubLocalizationService();
            var strassenabschnittXlsxImportService = GetStrassenabschnittXlsxImportService(localizationService);

            Stream memoryStream = new MemoryStream();
            var xlWorkbook = new XLWorkbook();
            IXLWorksheet ws = xlWorkbook.Worksheets.Add("Import");
            var i = 0;
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.Strassenname);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.BezeichnungVon);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.BezeichnungBis);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.ExternalId);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.Abschnittsnummer);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.Strasseneigentuemer);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.Ortsbezeichnung);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.BelastungskategorieTyp);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.Belag);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.BreiteFahrbahn);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.Laenge);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.Trottoir);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.BreiteTrottoirLinks);
            xlWorkbook.SaveAs(memoryStream);
            Stream xlsxTemplateStream = memoryStream;

            var importResultModel = strassenabschnittXlsxImportService.ImportStrassenabschnitte(xlsxTemplateStream);

            Assert.AreEqual(1, importResultModel.Errors.Count);
            Assert.AreEqual(localizationService.GetLocalizedError(ValidationError.MissignHeader), importResultModel.Errors.First());
        }

        [Test]
        public void TestIncorrectHeaderOrder()
        {
            ILocalizationService localizationService = new StubLocalizationService();
            var strassenabschnittXlsxImportService = GetStrassenabschnittXlsxImportService(localizationService);

            Stream memoryStream = new MemoryStream();
            var xlWorkbook = new XLWorkbook();
            IXLWorksheet ws = xlWorkbook.Worksheets.Add("Import");
            var i = 0;
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.Strassenname);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.BezeichnungVon);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.BezeichnungBis);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.ExternalId);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.Abschnittsnummer);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.Strasseneigentuemer);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.Ortsbezeichnung);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.BelastungskategorieTyp);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.Belag);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.BreiteFahrbahn);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.Laenge);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.Trottoir);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.BreiteTrottoirRechts);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.BreiteTrottoirLinks);

            xlWorkbook.SaveAs(memoryStream);
            Stream xlsxTemplateStream = memoryStream;

            var importResultModel = strassenabschnittXlsxImportService.ImportStrassenabschnitte(xlsxTemplateStream);

            Assert.AreEqual(1, importResultModel.Errors.Count);
            Assert.AreEqual(string.Format(localizationService.GetLocalizedError(ValidationError.IncorrectHeaderText), 12), importResultModel.Errors.First());
        }

        [Test]
        public void TestCorrectEmptyFile()
        {
            var strassenabschnittXlsxImportService = GetStrassenabschnittXlsxImportService(new StubLocalizationService());

            Stream xlsxTemplateStream = GetXlsxTemplateStream(new MemoryStream());
            var importResultModel = strassenabschnittXlsxImportService.ImportStrassenabschnitte(xlsxTemplateStream);

            Assert.AreEqual(0, importResultModel.Errors.Count);
        }
        
        [Test]
        public void TestGeneratedTemplateFile()
        {
            var strassenabschnittXlsxImportService = GetStrassenabschnittXlsxImportService(new StubLocalizationService());

            Stream xlsxTemplateStream = strassenabschnittXlsxImportService.CreateImportTemplateForStrassenabschnittImportModels();
            var importResultModel = strassenabschnittXlsxImportService.ImportStrassenabschnitte(xlsxTemplateStream);

            Assert.AreEqual(0, importResultModel.Errors.Count);
        }
        
        private static Stream GetXlsxTemplateStream(Stream memoryStream)
        {
            var xlWorkbook = GetTemplateXlWorkbook();

            xlWorkbook.SaveAs(memoryStream);
            return memoryStream;
        }

        private static XLWorkbook GetTemplateXlWorkbook()
        {
            var xlWorkbook = new XLWorkbook();
            IXLWorksheet ws = xlWorkbook.Worksheets.Add("Import");

            var localizationService = new StubLocalizationService();
            var i = 0;
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.Strassenname);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.BezeichnungVon);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.BezeichnungBis);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.ExternalId);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.Abschnittsnummer);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.Strasseneigentuemer);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.Ortsbezeichnung);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.BelastungskategorieTyp);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.Belag);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.BreiteFahrbahn);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.Laenge);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.Trottoir);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.BreiteTrottoirLinks);
            ws.Cell(1, ++i).Value = localizationService.GetLocalizedModelPropertyText<StrassenabschnittImportModel, object>(s => s.BreiteTrottoirRechts);
            return xlWorkbook;
        }

        private static StrassenabschnittXlsxImportService GetStrassenabschnittXlsxImportService(ILocalizationService localizationService)
        {
            var validatorFactoryMock = new Mock<IValidatorFactory>();
            validatorFactoryMock.Setup(m => m.GetValidator<StrassenabschnittImportModel>()).Returns(new StrassenabschnittImportModelValidator(localizationService, null));

            return new StrassenabschnittXlsxImportService(validatorFactoryMock.Object, localizationService, new Mock<IStrassenabschnittImportService>().Object, new Mock<IBelastungskategorieService>().Object, new Mock<IEreignisLogService>().Object, new Mock<ISessionService>().Object, new Mock<IEntityServiceMappingEngine>().Object, new Mock<ICookieService>().Object, new Mock<IServerPathProvider>().Object);
        }
    }
}
