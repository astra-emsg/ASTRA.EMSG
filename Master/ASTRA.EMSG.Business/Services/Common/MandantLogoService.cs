using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Security;
using System.Linq;
using ASTRA.EMSG.Common.Utils;

namespace ASTRA.EMSG.Business.Services.Common
{
    public interface IMandantLogoService
    {
        MandantLogoModel GetMandantLogo();
        List<string> UploadMandantLogo(Stream inputStream);
    }

    public class MandantLogoService : MandantDependentEntityServiceBase<MandantLogo, MandantLogoModel>, IMandantLogoService
    {
        private readonly ILocalizationService localizationService;

        private readonly MandantLogoModel defaultMandantLogoModel;

        public MandantLogoService(
            ITransactionScopeProvider transactionScopeProvider, 
            IEntityServiceMappingEngine entityServiceMappingEngine, 
            ISecurityService securityService,
            ILocalizationService localizationService,
            IServerPathProvider serverPathProvider)
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService)
        {
            this.localizationService = localizationService;

            var imagePath = serverPathProvider.MapPath(@"~/App_Data/DefaultLogo.png");
            var image = Image.FromFile(imagePath);

            defaultMandantLogoModel = new MandantLogoModel
                {
                   Logo = File.ReadAllBytes(imagePath),
                   Height = image.Height,
                   Width = image.Width
                }; 
        }

        public virtual MandantLogoModel GetMandantLogo()
        {
            var mandantLogoModel = GetCurrentMandantLogo();
            if (mandantLogoModel == null || mandantLogoModel.Logo == null || mandantLogoModel.Logo.IsEmpty())
                return defaultMandantLogoModel;

            return mandantLogoModel;
        }

        public List<string> UploadMandantLogo(Stream inputStream)
        {
            if(inputStream.Length > 2000000)
                return new List<string> {localizationService.GetLocalizedError(ValidationError.TooBigFile)};

            Image img;
            try
            {
                img = Image.FromStream(inputStream);
            }
            catch (Exception)
            {
                return GetInvalidFileFormatError();
            }

            if (IsNotImageFormat(img, ImageFormat.Jpeg) && IsNotImageFormat(img, ImageFormat.Png) && IsNotImageFormat(img, ImageFormat.Bmp))
                return GetInvalidFileFormatError();

            var memoryStream = new MemoryStream();
            img.Save(memoryStream, ImageFormat.Png);
            memoryStream.Seek(0, 0);

            var mandantLogoModel = GetCurrentMandantLogo();
            if (mandantLogoModel != null)
            {
                UpdateMandantLogoModel(mandantLogoModel, memoryStream, img);
                UpdateEntity(mandantLogoModel);
            }
            else
            {
                mandantLogoModel = UpdateMandantLogoModel(new MandantLogoModel {Id = Guid.NewGuid()}, memoryStream, img);
                CreateEntity(mandantLogoModel);
            }

            return new List<string>();
        }

        private static MandantLogoModel UpdateMandantLogoModel(MandantLogoModel mandantLogoModel, MemoryStream memoryStream, Image img)
        {
            mandantLogoModel.Logo = memoryStream.ToArray();
            mandantLogoModel.Height = img.Height;
            mandantLogoModel.Width = img.Width;

            return mandantLogoModel;
        }

        private List<string> GetInvalidFileFormatError()
        {
            return new List<string> { localizationService.GetLocalizedError(ValidationError.WrongFileFormat) };
        }

        private static bool IsNotImageFormat(Image img, ImageFormat imageFormat)
        {
            return !img.RawFormat.Equals(imageFormat);
        }

        private MandantLogoModel GetCurrentMandantLogo()
        {
            return GetCurrentModels().SingleOrDefault();
        }

        protected override Expression<Func<MandantLogo, Mandant>> MandantExpression { get { return ml => ml.Mandant; } }
    }
}