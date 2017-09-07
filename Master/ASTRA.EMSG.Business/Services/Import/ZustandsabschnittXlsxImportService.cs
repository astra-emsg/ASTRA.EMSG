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
    public interface IZustandsabschnittXlsxImportService : IService
    {
        ImportResultModel<ZustandsabschnittImportModel, ZustandsabschnittImportOverviewModel> ImportZustandsabschnitte(Stream stream);
        Stream CreateImportTemplateForZustandsabschnittImportModels();
        ImportResultModel<ZustandsabschnittImportModel, ZustandsabschnittImportOverviewModel> CommitZustandsabschnittImport();
        Stream GetImportTemplateForZustandsabschnittImportModels();
    }

    public class ZustandsabschnittXlsxImportService : ImportServiceBase, IZustandsabschnittXlsxImportService
    {
        private readonly IStrassenabschnittService strassenabschnittService;
        private readonly IZustandsabschnittImportService zustandsabschnittImportService;
        private readonly IMassnahmenvorschlagKatalogService massnahmenvorschlagKatalogService;
        private readonly ISessionService sessionService;

        private readonly string[] zustandsabschnittImportModelProperties = new[]
                {
                    GetPropertyName(z => z.Strassenname),
                    GetPropertyName(z => z.StrassennameBezeichnungVon),
                    GetPropertyName(z => z.StrassennameBezeichnungBis),
                    GetPropertyName(z => z.ExternalId),
                    GetPropertyName(z => z.Abschnittsnummer),
                    GetPropertyName(z => z.BezeichnungVon),
                    GetPropertyName(z => z.BezeichnungBis),
                    GetPropertyName(z => z.Laenge),
                    GetPropertyName(z => z.Zustandsindex),
                    GetPropertyName(z => z.ZustandsindexTrottoirLinks),
                    GetPropertyName(z => z.ZustandsindexTrottoirRechts),
                    GetPropertyName(z => z.Aufnahmedatum),
                    GetPropertyName(z => z.Aufnahmeteam),
                    GetPropertyName(z => z.Wetter),
                    GetPropertyName(z => z.Bemerkung),
                    GetPropertyName(z => z.MassnahmenvorschlagFahrbahnId),
                    GetPropertyName(z => z.DringlichkeitFahrbahn),
                    GetPropertyName(z => z.MassnahmenvorschlagTrottoirLinksId),
                    GetPropertyName(z => z.DringlichkeitTrottoirLinks),
                    GetPropertyName(z => z.MassnahmenvorschlagTrottoirRechtsId),
                    GetPropertyName(z => z.DringlichkeitTrottoirRechts)
                };

        private static string GetPropertyName<TProperty>(Expression<Func<ZustandsabschnittImportModel, TProperty>> expression)
        {
            return ExpressionHelper.GetPropertyName(expression);
        }

        public ZustandsabschnittXlsxImportService(
            ILocalizationService localizationService,
            IStrassenabschnittService strassenabschnittService,
            IMassnahmenvorschlagKatalogService massnahmenvorschlagKatalogService, 
            IValidatorFactory validatorFactory, 
            IZustandsabschnittImportService zustandsabschnittImportService, 
            IEreignisLogService ereignisLogService,
            IEntityServiceMappingEngine entityServiceMappingEngine,
            ISessionService sessionService,
            ICookieService cookieService,
            IServerPathProvider serverPathProvider
            )
            : base(localizationService, validatorFactory, ereignisLogService, entityServiceMappingEngine, cookieService, serverPathProvider)
        {
            this.strassenabschnittService = strassenabschnittService;
            this.massnahmenvorschlagKatalogService = massnahmenvorschlagKatalogService;
            this.zustandsabschnittImportService = zustandsabschnittImportService;
            this.sessionService = sessionService;
        }

        public Stream CreateImportTemplateForZustandsabschnittImportModels()
        {
            return CreateTemplateFor<ZustandsabschnittImportModel>(zustandsabschnittImportModelProperties);
        }

        public Stream GetImportTemplateForZustandsabschnittImportModels()
        {
            return GetImportTemplateForImportModels(LocalizationService.GetLocalizedText("ZustandsabschnittImportTemplateFileName"));
        }

        public ImportResultModel<ZustandsabschnittImportModel, ZustandsabschnittImportOverviewModel> ImportZustandsabschnitte(Stream stream)
        {
            var importResult = ImportModels<ZustandsabschnittImportModel>(stream, zustandsabschnittImportModelProperties, ReadZustandsabschnittImportModel);

            Func<ZustandsabschnittImportModel, ZustandsabschnittImportModel, bool> areEqual =
                (m1, m2) => m1.Strassenabschnitt == m2.Strassenabschnitt
                            && m1.StrassennameBezeichnungVon == m2.StrassennameBezeichnungVon
                            && m1.StrassennameBezeichnungBis == m2.StrassennameBezeichnungBis
                            && m1.BezeichnungVon == m2.BezeichnungVon
                            && m1.BezeichnungBis == m2.BezeichnungBis;


            Func<ZustandsabschnittImportModel, ZustandsabschnittImportModel, bool> isExternalIdEqual =
                (m1, m2) => !string.IsNullOrEmpty(m1.ExternalId) && !string.IsNullOrEmpty(m2.ExternalId) && m1.ExternalId == m2.ExternalId;

            var importResultModel = GetImportResultModel<ZustandsabschnittImportModel, ZustandsabschnittImportOverviewModel>(importResult, zustandsabschnittImportService.GetCurrentModels(), areEqual, isExternalIdEqual, null);

            sessionService.LastZustandsabschnittImportResult = importResultModel;

            return importResultModel;
        }

        public ImportResultModel<ZustandsabschnittImportModel, ZustandsabschnittImportOverviewModel> CommitZustandsabschnittImport()
        {
            if (sessionService.LastZustandsabschnittImportResult == null)
                return sessionService.LastZustandsabschnittImportResult = new ImportResultModel<ZustandsabschnittImportModel, ZustandsabschnittImportOverviewModel>(new List<string> { LocalizationService.GetLocalizedError(ValidationError.SessionTimeOut) });

            return SaveImportResult(sessionService.LastZustandsabschnittImportResult, zustandsabschnittImportService.GetCurrentModels(), zustandsabschnittImportService.CreateEntity, zustandsabschnittImportService.UpdateEntity);
        }

        protected ZustandsabschnittImportModel ReadZustandsabschnittImportModel(XlsxImportResult<ZustandsabschnittImportModel> importResult, Row row)
        {
            var model = new ZustandsabschnittImportModel
            {
                Id = Guid.NewGuid(),
                Strassenname = ParseNullableString(row.Cells[0]),
                StrassennameBezeichnungVon = ParseNullableString(row.Cells[1]),
                StrassennameBezeichnungBis = ParseNullableString(row.Cells[2]),
                ExternalId = ParseNullableString(row.Cells[3]),
                Abschnittsnummer = TryParseNullableInt(row.Cells[4], importResult),
                BezeichnungVon = ParseNullableString(row.Cells[5]),
                BezeichnungBis = ParseNullableString(row.Cells[6]),
                Laenge = TryParseDecimal(row.Cells[7], importResult),
                Zustandsindex = TryParseDecimal(row.Cells[8], importResult),
                ZustandsindexTrottoirLinks = TryParseEnum<ZustandsindexTyp>(row.Cells[9], importResult),
                ZustandsindexTrottoirRechts = TryParseEnum<ZustandsindexTyp>(row.Cells[10], importResult),
                Aufnahmedatum = TryParseDateTime(row.Cells[11], importResult),
                Aufnahmeteam = ParseNullableString(row.Cells[12]),
                Wetter = TryParseEnum<WetterTyp>(row.Cells[13], importResult),
                Bemerkung = ParseNullableString(row.Cells[14]),
                DringlichkeitFahrbahn = TryParseEnum<DringlichkeitTyp>(row.Cells[16], importResult),
                DringlichkeitTrottoirLinks = TryParseEnum<DringlichkeitTyp>(row.Cells[18], importResult),
                DringlichkeitTrottoirRechts = TryParseEnum<DringlichkeitTyp>(row.Cells[20], importResult),
            };

            var strassenabschnitt = strassenabschnittService.GetStrassenabschnitt(model.Strassenname, model.StrassennameBezeichnungVon, model.StrassennameBezeichnungBis);
            if (strassenabschnitt == null)
            {
                importResult.ErrorList.Add(string.Format(LocalizationService.GetLocalizedError(ValidationError.StrassenabschnittDoesNotExistsError), model.Strassenname, model.StrassennameBezeichnungVon, model.StrassennameBezeichnungBis, row.RowNumber));
                return model;
            }
            
            var belastungskategorieTyp = strassenabschnitt.BelastungskategorieTyp;

            model.Strassenabschnitt = strassenabschnitt.Id;
            model.MassnahmenvorschlagFahrbahnId = TryGetMassnahmenvorschlagKatalog(row.Cells[15], MassnahmenvorschlagKatalogTyp.Fahrbahn, belastungskategorieTyp, importResult);
            model.MassnahmenvorschlagTrottoirLinksId = TryGetMassnahmenvorschlagKatalog(row.Cells[17], MassnahmenvorschlagKatalogTyp.Trottoir, belastungskategorieTyp, importResult);
            model.MassnahmenvorschlagTrottoirRechtsId = TryGetMassnahmenvorschlagKatalog(row.Cells[19], MassnahmenvorschlagKatalogTyp.Trottoir, belastungskategorieTyp, importResult);

            return model;
        }

        protected Guid? TryGetMassnahmenvorschlagKatalog(Cell cell, MassnahmenvorschlagKatalogTyp massnahmenvorschlagKatalogTyp, string belastungskategorieTyp, IXlsxImportResult xlsxImportResult)
        {
            if (string.IsNullOrEmpty(cell.Value))
                return null;

            var massnahmenvorschlagKatalogModel = massnahmenvorschlagKatalogService
                .GetMassnahmenvorschlagKatalogModelList(massnahmenvorschlagKatalogTyp, belastungskategorieTyp)
                .SingleOrDefault(mkm => mkm.TypBezeichnung == cell.Value);

            if (massnahmenvorschlagKatalogModel == null)
            {
                xlsxImportResult.ErrorList.Add(string.Format(LocalizationService.GetLocalizedError(ValidationError.WrongDataFormat), cell.Id));
                return Guid.Empty;
            }

            return massnahmenvorschlagKatalogModel.Id;
        }
    }
}