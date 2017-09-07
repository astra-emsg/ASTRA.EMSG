using System.Drawing;
using System.IO;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Security;

namespace ASTRA.EMSG.Business.Services.Common
{
    public class TestMandantLogoService : MandantLogoService
    {
        private readonly IServerPathProvider serverPathProvider;

        public TestMandantLogoService(
            ITransactionScopeProvider transactionScopeProvider,
            IEntityServiceMappingEngine entityServiceMappingEngine,
            ISecurityService securityService,
            ILocalizationService localizationService,
            IServerPathProvider serverPathProvider)
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService, localizationService, serverPathProvider)
        {
            this.serverPathProvider = serverPathProvider;
        }

        public override MandantLogoModel GetMandantLogo()
        {
            var imagePath = serverPathProvider.MapPath(@"~/App_Data/testLogo.png");
            var image = Image.FromFile(imagePath);

            return new MandantLogoModel
            {
                Logo = File.ReadAllBytes(imagePath),
                Height = image.Height,
                Width = image.Width
            };
        }
    }
}