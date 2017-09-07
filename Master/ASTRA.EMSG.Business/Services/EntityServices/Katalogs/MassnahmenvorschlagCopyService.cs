using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.Historization;

namespace ASTRA.EMSG.Business.Services.EntityServices.Katalogs
{
    public interface IMassnahmenvorschlagCopyService : IService
    {
        void CopyMassnahmenvorschlagen(ZustandsabschnittBase copiedZustandsabschnitt, ZustandsabschnittBase zustandsabschnittToCopy);
    }

    public class MassnahmenvorschlagCopyService : IMassnahmenvorschlagCopyService
    {
        private readonly IKatalogCopyService katalogCopyService;

        public MassnahmenvorschlagCopyService(IKatalogCopyService katalogCopyService)
        {
            this.katalogCopyService = katalogCopyService;
        }

        public void CopyMassnahmenvorschlagen(ZustandsabschnittBase copiedZustandsabschnitt, ZustandsabschnittBase zustandsabschnittToCopy)
        {
            if (zustandsabschnittToCopy.MassnahmenvorschlagFahrbahn != null)
                copiedZustandsabschnitt.MassnahmenvorschlagFahrbahn = katalogCopyService.GetKatalogCopy<MassnahmenvorschlagKatalog>(zustandsabschnittToCopy.MassnahmenvorschlagFahrbahn.Id);
            if (zustandsabschnittToCopy.MassnahmenvorschlagTrottoirLinks != null)
                copiedZustandsabschnitt.MassnahmenvorschlagTrottoirLinks = katalogCopyService.GetKatalogCopy<MassnahmenvorschlagKatalog>(zustandsabschnittToCopy.MassnahmenvorschlagTrottoirLinks.Id);
            if (zustandsabschnittToCopy.MassnahmenvorschlagTrottoirRechts != null)
                copiedZustandsabschnitt.MassnahmenvorschlagTrottoirRechts = katalogCopyService.GetKatalogCopy<MassnahmenvorschlagKatalog>(zustandsabschnittToCopy.MassnahmenvorschlagTrottoirRechts.Id);
        }
    }
}