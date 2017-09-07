using System.Collections.Generic;
using System.Collections.ObjectModel;
using ASTRA.EMSG.Common.DataTransferObjects;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Services.SchadenMetadaten;
using System.Linq;
using ASTRA.EMSG.Localization;

namespace ASTRA.EMSG.Mobile.ViewModels
{
    public class DetailGroupItemViewModel : ViewModel
    {
        public DetailGroupItemViewModel(SchadengruppeMetadaten schadengruppeMetadaten, List<SchadendetailDTO> schadendetailDtos)
        {
            DetailItemViewModels = new ObservableCollection<DetailItemViewModel>();

            int rowNumber = 0;
            foreach (var schadendetailMetadaten in schadengruppeMetadaten.Schadendetails)
            {
                var schadendetailDto = schadendetailDtos == null
                                           ? null
                                           : schadendetailDtos
                                           .SingleOrDefault(sd => sd.SchadendetailTyp == schadendetailMetadaten.SchadendetailTyp);

                var detailItemViewModel = new DetailItemViewModel(schadendetailMetadaten, schadendetailDto, rowNumber % 2 != 0);
                DetailItemViewModels.Add(detailItemViewModel);
                detailItemViewModel.PropertyChanged += (sender, args) => Recalculate();
                rowNumber++;
            }

            Gewicht = schadengruppeMetadaten.Gewicht;
            SchadengruppeTyp = schadengruppeMetadaten.SchadengruppeTyp;

            SchadengruppeBezeichnung = LocalizationLocator.MobileLocalization.GetSchadengruppeBezeichnung(schadengruppeMetadaten.SchadengruppeTyp);
        }

        private void Recalculate()
        {
            Notify(() => Matrix);
            Notify(() => Bewertung);
        }

        private SchadengruppeTyp schadengruppeTyp;
        public SchadengruppeTyp SchadengruppeTyp { get { return schadengruppeTyp; } set { schadengruppeTyp = value; Notify(() => SchadengruppeTyp); } }

        private string schadengruppeBezeichnung;
        public string SchadengruppeBezeichnung { get { return schadengruppeBezeichnung; } set { schadengruppeBezeichnung = value; Notify(() => SchadengruppeBezeichnung); } }

        private ObservableCollection<DetailItemViewModel> detailItemViewModels;
        public ObservableCollection<DetailItemViewModel> DetailItemViewModels { get { return detailItemViewModels; } set { detailItemViewModels = value; Notify(() => DetailItemViewModels); } }

        public int Matrix { get { return DetailItemViewModels.Max(ivm => ivm.Matrix); } }

        private int gewicht;
        public int Gewicht { get { return gewicht; } set { gewicht = value; Notify(() => Gewicht); } }

        public int Bewertung { get { return Matrix * Gewicht; } }
    }
}