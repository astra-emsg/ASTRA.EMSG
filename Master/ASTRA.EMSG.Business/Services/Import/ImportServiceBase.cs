using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Xlsx;
using ASTRA.EMSG.Business.Models.Strassennamen;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Utils;
using FluentValidation;
using FluentValidation.Results;
using ASTRA.EMSG.Web.Infrastructure;

namespace ASTRA.EMSG.Business.Services.Import
{
    public abstract class ImportServiceBase
    {
        protected ILocalizationService LocalizationService { get; set; }

        private readonly IValidatorFactory validatorFactory;
        private readonly IEreignisLogService ereignisLogService;
        private readonly IEntityServiceMappingEngine entityServiceMappingEngine;
        private readonly ICookieService cookieService;
        private readonly IServerPathProvider serverPathProvider;

        protected ImportServiceBase(
            ILocalizationService localizationService, 
            IValidatorFactory validatorFactory, 
            IEreignisLogService ereignisLogService,
            IEntityServiceMappingEngine entityServiceMappingEngine,
            ICookieService cookieService,
            IServerPathProvider serverPathProvider)
        {
            LocalizationService = localizationService;
            this.validatorFactory = validatorFactory;
            this.ereignisLogService = ereignisLogService;
            this.entityServiceMappingEngine = entityServiceMappingEngine;
            this.cookieService = cookieService;
            this.serverPathProvider = serverPathProvider;
        }

        protected XlsxImportResult<TModel> ImportModels<TModel>(Stream stream, string[] prop, Func<XlsxImportResult<TModel>, Row, TModel> readModel)
        {
            var importResult = new XlsxImportResult<TModel>();

            var xlsxDoc = new XlsxDoc(prop.Length);
            if (!xlsxDoc.TryLoad(stream))
                return CreateXlsxImportResult<TModel>(LocalizationService.GetLocalizedError(ValidationError.WrongFileFormat));

            if (xlsxDoc.HeaderRow.Cells.Any(c => c.IsEmpty))
                return CreateXlsxImportResult<TModel>(LocalizationService.GetLocalizedError(ValidationError.MissignHeader));

            XlsxImportResult<TModel> headerRowError = GetModelHeaderRowError<TModel>(xlsxDoc, prop);
            if (headerRowError != null)
                return headerRowError;

            foreach (var row in xlsxDoc.DataMatrix)
                importResult.ModelInfos.Add(new ModelInfo<TModel>(readModel(importResult, row), row.RowNumber));

            if (importResult.HasError)
                importResult.ModelInfos.Clear();

            return importResult;
        }

        protected XlsxImportResult<TModel> GetModelHeaderRowError<TModel>(XlsxDoc xlsxDoc, string[] prop)
        {
            if (xlsxDoc.IsThereExtraColumns)
                return CreateXlsxImportResult<TModel>(LocalizationService.GetLocalizedError(ValidationError.IncorrectNumberOfColumns));

            for (int index = 0; index < prop.Length; index++)
                if (CheckHeader(xlsxDoc, index, LocalizationService.GetLocalizedModelPropertyText<TModel>(prop[index])))
                    return CreateIncorrectHeaderTextError<TModel>(index + 1);

            return null;
        }

        protected Stream CreateTemplateFor<TModel>(string[] prop)
        {
            var xlsxDoc = new XlsxDoc(prop.Length);
            var cells = xlsxDoc.HeaderRow.Cells;

            for (int index = 0; index < prop.Length; index++)
                cells[index].Value = LocalizationService.GetLocalizedModelPropertyText<TModel>(prop[index]);

            Stream stream = xlsxDoc.Save();
            stream.Seek(0, 0);
            return stream;
        }
        
        protected Stream GetImportTemplateForImportModels(string importTemplateName)
        {
            var memoryStream = new MemoryStream();
            using (var fs = File.OpenRead(serverPathProvider.MapPath(string.Format("~/ImportTemplates/{0}/{1}.xlsx", cookieService.EmsgLanguage.ToCultureInfo().TwoLetterISOLanguageName, importTemplateName))))
            {
                var bytes = fs.ReadAllByte();
                memoryStream.Write(bytes, 0, bytes.Length);
            }

            memoryStream.Seek(0, 0);
            return memoryStream;
        }
        
        protected static string ParseNullableString(Cell cell)
        {
            return string.IsNullOrEmpty(cell.Value) ? null : cell.Value;
        }

        protected decimal? TryParseNullableDecimal(Cell cell, IXlsxImportResult xlsxImportResult)
        {
            if (string.IsNullOrEmpty(cell.Value))
                return null;
            return TryParseDecimal(cell, xlsxImportResult);
        }

        protected decimal TryParseDecimal(Cell cell, IXlsxImportResult xlsxImportResult)
        {
            decimal result;
            if (!decimal.TryParse(cell.Value, out result))
                xlsxImportResult.ErrorList.Add(string.Format(LocalizationService.GetLocalizedError(ValidationError.WrongDataFormat), cell.Id));

            return result;
        }

        protected DateTime TryParseDateTime(Cell cell, IXlsxImportResult xlsxImportResult)
        {
            DateTime result;
            if (!DateTime.TryParse(cell.Value, out result))
                xlsxImportResult.ErrorList.Add(string.Format(LocalizationService.GetLocalizedError(ValidationError.WrongDataFormat), cell.Id));

            return result;
        }

        protected int? TryParseNullableInt(Cell cell, IXlsxImportResult xlsxImportResult)
        {
            if (string.IsNullOrEmpty(cell.Value))
                return null;

            int result;
            if (!int.TryParse(cell.Value, out result))
                xlsxImportResult.ErrorList.Add(string.Format(LocalizationService.GetLocalizedError(ValidationError.WrongDataFormat), cell.Id));

            return result;
        }

        protected TEnum TryParseEnum<TEnum>(Cell cell, IXlsxImportResult xlsxImportResult) where TEnum : struct
        {
            if (!LocalizationService.IsEnumExistsForLocalisedText<TEnum>(cell.Value))
            {
                xlsxImportResult.ErrorList.Add(string.Format(LocalizationService.GetLocalizedError(ValidationError.WrongDataFormat), cell.Id));
                return default(TEnum);
            }

            return LocalizationService.GetEnumFromLocalizedText<TEnum>(cell.Value);
        }

        protected bool CheckHeader(XlsxDoc xlsxDoc, int columnNumber, string headerText)
        {
            return xlsxDoc.HeaderRow.Cells[columnNumber].Value != headerText;
        }

        protected XlsxImportResult<TModel> CreateIncorrectHeaderTextError<TModel>(int columnNumber)
        {
            return CreateXlsxImportResult<TModel>(string.Format(LocalizationService.GetLocalizedError(ValidationError.IncorrectHeaderText), columnNumber));
        }

        protected XlsxImportResult<TModel> CreateXlsxImportResult<TModel>(string errorMessage)
        {
            var xlsxImportResult = new XlsxImportResult<TModel>();
            xlsxImportResult.ErrorList.Add(errorMessage);
            return xlsxImportResult;
        }

        protected ImportResultModel<TImportModel, TImportOverviewModel> GetImportResultModel<TImportModel, TImportOverviewModel>(XlsxImportResult<TImportModel> importResult, List<TImportModel> allImportModel, Func<TImportModel, TImportModel, bool> areEqual, Func<TImportModel, TImportModel, bool> isExternalIdEqual, List<string> warnings)
            where TImportModel : IExternalIdHolder
        {
            if (importResult.HasError)
                return new ImportResultModel<TImportModel, TImportOverviewModel>(importResult.ErrorList, warnings);

            var validator = validatorFactory.GetValidator<TImportModel>();

            var errorList = new List<string>();
            foreach (var modelInfo in importResult.ModelInfos)
            {
                ValidationResult validationResult = validator.Validate(modelInfo.Model);
                errorList.AddRange(validationResult.Errors.Select(e => e.ErrorMessage + string.Format(LocalizationService.GetLocalizedError(ValidationError.RowN), modelInfo.RowNumber)));
            }

            LogImportEreigniss(errorList.Count);

            if (errorList.Any())
                return new ImportResultModel<TImportModel, TImportOverviewModel>(errorList, warnings);

            var resultModel = new ImportResultModel<TImportModel, TImportOverviewModel>(null, warnings);

            foreach (var importModel in importResult.ModelInfos.ToList())
            {
                var model = importModel.Model;
                //case a ExtrnalID matches -> Update
                if ((allImportModel.FirstOrDefault(m => isExternalIdEqual(m, model))) != null )
                {
                    model.Id = allImportModel.FirstOrDefault(m => isExternalIdEqual(m, model)).Id;

                    resultModel.UpdateImportModels.Add(model);
                    resultModel.UpdateImportOverviewModels.Add(Translate<TImportModel, TImportOverviewModel>(model));
                }
                else
                {
                    var firstOrDefaultImprotModel = allImportModel.FirstOrDefault(m => areEqual(m, model));

                    //we found an entity with
                    if (firstOrDefaultImprotModel != null)
                    {
                        if (string.IsNullOrEmpty(model.ExternalId) ||
                            string.IsNullOrEmpty(firstOrDefaultImprotModel.ExternalId))
                        {
                            model.Id = firstOrDefaultImprotModel.Id;

                            resultModel.UpdateImportModels.Add(model);
                            resultModel.UpdateImportOverviewModels.Add(
                                Translate<TImportModel, TImportOverviewModel>(model));
                        }
                            //non of the ExternalIds is null but they are not qual B/2 case -> error
                        else
                        {
                            errorList.Add(
                                string.Format(
                                    LocalizationService.GetLocalizedError(ValidationError.NonMatchingExternalId),
                                    importModel.RowNumber));
                        }
                    }
                        //we cannot find it by enternalid or by natural key case C -> create
                    else
                    {
                        allImportModel.Add(model);
                        resultModel.CreateImportModels.Add(model);
                        resultModel.CreateImportOverviewModels.Add(Translate<TImportModel, TImportOverviewModel>(model));
                    }
                }
            }

            if (errorList.Any())
                return new ImportResultModel<TImportModel, TImportOverviewModel>(errorList, warnings);

            return resultModel;
        }

        protected TImportOverviewModel Translate<TImportModel, TImportOverviewModel>(TImportModel model)
            where TImportModel : IExternalIdHolder
        {
            return entityServiceMappingEngine.Translate<TImportModel, TImportOverviewModel>(model);
        }

        protected ImportResultModel<TImportModel, TImportOverviewModel> SaveImportResult<TImportModel, TImportOverviewModel>(ImportResultModel<TImportModel, TImportOverviewModel> importResultModel, List<TImportModel> allImportModel, Func<TImportModel, TImportModel> create, Func<TImportModel, TImportModel> update)
            where TImportModel : IExternalIdHolder
        {
            foreach (var updateImportModel in importResultModel.UpdateImportModels)
            {
                if (allImportModel.Any(m => m.Id == updateImportModel.Id))
                {
                    update(updateImportModel);
                }
                else
                {
                    //We will recreate Deleted Items with a different ID (Deleted between Import and Commit)
                    updateImportModel.Id = Guid.NewGuid();
                    create(updateImportModel);
                }
            }

            foreach (var updateImportModel in importResultModel.CreateImportModels)
                create(updateImportModel);

            return importResultModel;
        }

        protected void LogImportEreigniss(int errorCount)
        {
            ereignisLogService.LogEreignis(EreignisTyp.XmlImport, new Dictionary<string, object> { { "fehleranzahl", errorCount } });
        }
    }
}