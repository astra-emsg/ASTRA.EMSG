using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Infrastructure.Xlsx;
using ASTRA.EMSG.Business.Models.Strassennamen;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.EntityServices.Strassennamen;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;
using FluentValidation;

namespace ASTRA.EMSG.Business.Services.Import
{
    public interface IStrassenabschnittXlsxImportService : IService
    {
        ImportResultModel<StrassenabschnittImportModel, StrassenabschnittImportOverviewModel> ImportStrassenabschnitte(Stream stream);
        Stream CreateImportTemplateForStrassenabschnittImportModels();
        Stream GetImportTemplateForStrassenabschnittImportModels();
        ImportResultModel<StrassenabschnittImportModel, StrassenabschnittImportOverviewModel> CommitStrassenabschnittImport();
    }

    public class StrassenabschnittXlsxImportService : ImportServiceBase, IStrassenabschnittXlsxImportService
    {
        private readonly IStrassenabschnittImportService strassenabschnittImportService;
        private readonly IBelastungskategorieService belastungskategorieService;
        private readonly ISessionService sessionService;

        private readonly string[] strassenabschnittImportModelProperties = new[]
                {
                    GetPropertyName(s => s.Strassenname),
                    GetPropertyName(s => s.BezeichnungVon),
                    GetPropertyName(s => s.BezeichnungBis),
                    GetPropertyName(s => s.ExternalId),
                    GetPropertyName(s => s.Abschnittsnummer),
                    GetPropertyName(s => s.Strasseneigentuemer),
                    GetPropertyName(s => s.Ortsbezeichnung),
                    GetPropertyName(s => s.BelastungskategorieTyp),
                    GetPropertyName(s => s.Belag),
                    GetPropertyName(s => s.BreiteFahrbahn),
                    GetPropertyName(s => s.Laenge),
                    GetPropertyName(s => s.Trottoir),
                    GetPropertyName(s => s.BreiteTrottoirLinks),
                    GetPropertyName(s => s.BreiteTrottoirRechts)
                };

        public StrassenabschnittXlsxImportService(
            IValidatorFactory validatorFactory, 
            ILocalizationService localizationService, 
            IStrassenabschnittImportService strassenabschnittImportService, 
            IBelastungskategorieService belastungskategorieService, 
            IEreignisLogService ereignisLogService,
            ISessionService sessionService,
            IEntityServiceMappingEngine entityServiceMappingEngine,
            ICookieService cookieService,
            IServerPathProvider serverPathProvider) 
            : base(localizationService, validatorFactory, ereignisLogService, entityServiceMappingEngine, cookieService, serverPathProvider)
        {
            this.strassenabschnittImportService = strassenabschnittImportService;
            this.belastungskategorieService = belastungskategorieService;
            this.sessionService = sessionService;
        }

        private static string GetPropertyName<TProperty>(Expression<Func<StrassenabschnittImportModel, TProperty>> expression)
        {
            return ExpressionHelper.GetPropertyName(expression);
        }

        public Stream CreateImportTemplateForStrassenabschnittImportModels()
        {
            return CreateTemplateFor<StrassenabschnittImportModel>(strassenabschnittImportModelProperties);
        }

        public Stream GetImportTemplateForStrassenabschnittImportModels()
        {
            return GetImportTemplateForImportModels(LocalizationService.GetLocalizedText("StrassenabschnittImportTemplateFileName"));
        }

        public ImportResultModel<StrassenabschnittImportModel, StrassenabschnittImportOverviewModel> ImportStrassenabschnitte(Stream stream)
        {
            var importResult = ImportModels<StrassenabschnittImportModel>(stream, strassenabschnittImportModelProperties, ReadStrassenabschnittImportModel);

            bool isThereBelagBelastungskategorieMissmatch =
                importResult.ModelInfos.Any(mi => mi.Model.Belastungskategorie.HasValue &&
                    !belastungskategorieService.GetBelastungskategorie(mi.Model.Belastungskategorie).AllowedBelagList.Contains(mi.Model.Belag));

            var warnings = isThereBelagBelastungskategorieMissmatch ? new List<string> { LocalizationService.GetLocalizedError(ValidationError.BelagBelastungskategorieMissmatch) } : new List<string>();

            Func<StrassenabschnittImportModel, StrassenabschnittImportModel, bool> areEqual =
                (m1, m2) => m1.Strassenname == m2.Strassenname 
                    && m1.BezeichnungVon == m2.BezeichnungVon 
                    && m1.BezeichnungBis == m2.BezeichnungBis;

            Func<StrassenabschnittImportModel, StrassenabschnittImportModel, bool> isExternalIdEqual =
                (m1, m2) => !string.IsNullOrEmpty(m1.ExternalId) && !string.IsNullOrEmpty(m2.ExternalId) && m1.ExternalId == m2.ExternalId;

            var importResultModel = GetImportResultModel<StrassenabschnittImportModel, StrassenabschnittImportOverviewModel>(importResult, strassenabschnittImportService.GetCurrentModels(), areEqual, isExternalIdEqual, warnings);

            sessionService.LastStrassenabschnittImportResult = importResultModel;

            return importResultModel;
        }

        public ImportResultModel<StrassenabschnittImportModel, StrassenabschnittImportOverviewModel> CommitStrassenabschnittImport()
        {
            if (sessionService.LastStrassenabschnittImportResult == null)
                return sessionService.LastStrassenabschnittImportResult = new ImportResultModel<StrassenabschnittImportModel, StrassenabschnittImportOverviewModel>(new List<string> { LocalizationService.GetLocalizedError(ValidationError.SessionTimeOut) });

            return SaveImportResult(sessionService.LastStrassenabschnittImportResult, strassenabschnittImportService.GetCurrentModels(), strassenabschnittImportService.CreateEntity, strassenabschnittImportService.UpdateEntity);
        }

        private StrassenabschnittImportModel ReadStrassenabschnittImportModel(XlsxImportResult<StrassenabschnittImportModel> importResult, Row row)
        {
            var i = 0;
            var model = new StrassenabschnittImportModel
            {
                Id = Guid.NewGuid(),
                Strassenname = ParseNullableString(row.Cells[i++]),
                BezeichnungVon = ParseNullableString(row.Cells[i++]),
                BezeichnungBis = ParseNullableString(row.Cells[i++]),
                ExternalId = ParseNullableString(row.Cells[i++]),
                Abschnittsnummer = TryParseNullableInt(row.Cells[i++], importResult),
                Strasseneigentuemer = TryParseEnum<EigentuemerTyp>(row.Cells[i++], importResult),
                Ortsbezeichnung = ParseNullableString(row.Cells[i++]),
                Belastungskategorie = TryGetBelastungskategorie(row.Cells[i++], importResult),
                Belag = TryParseEnum<BelagsTyp>(row.Cells[i++], importResult),
                BreiteFahrbahn = TryParseDecimal(row.Cells[i++], importResult),
                Laenge = TryParseDecimal(row.Cells[i++], importResult),
                Trottoir = TryParseEnum<TrottoirTyp>(row.Cells[i++], importResult),
                BreiteTrottoirLinks = TryParseNullableDecimal(row.Cells[i++], importResult),
                BreiteTrottoirRechts = TryParseNullableDecimal(row.Cells[i], importResult)
            };

            return model;
        }

        protected Guid TryGetBelastungskategorie(Cell cell, IXlsxImportResult xlsxImportResult)
        {
            var belastungskategorie = belastungskategorieService
                .AllBelastungskategorieModel
                .SingleOrDefault(bkm => LocalizationService.GetLocalizedBelastungskategorieTyp(bkm.Typ) == cell.Value);

            if (belastungskategorie == null)
            {
                xlsxImportResult.ErrorList.Add(string.Format(LocalizationService.GetLocalizedError(ValidationError.WrongDataFormat), cell.Id));
                return Guid.Empty;
            }

            return belastungskategorie.Id;
        }
    }
}