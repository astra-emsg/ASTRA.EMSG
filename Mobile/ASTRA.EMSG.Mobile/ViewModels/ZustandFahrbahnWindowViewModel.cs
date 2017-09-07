using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;
using ASTRA.EMSG.Common.DataTransferObjects;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Services.SchadenMetadaten;
using ASTRA.EMSG.Mobile.Services;
using ASTRA.EMSG.Mobile.Utils;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Mobile.ViewModels
{
    public class ZustandFahrbahnWindowViewModel : EditableViewModel, ISaveable
    {
        private readonly ZustandsabschnittGISDTO zustandsabschnittGisdto;
        private readonly StrassenabschnittGISDTO strassenabschnittGisdto;
        private readonly IDTOService dtoService;
        private readonly ISchadenMetadatenService schadenMetadatenService;
        private readonly IMessageBoxService messageBoxService;

        public event EventHandler Saved;
        public event EventHandler Closed;

        public void OnSaved() { EventHandler handler = Saved; if (handler != null) handler(this, EventArgs.Empty); }
        public void OnClosed() { EventHandler handler = Closed; if (handler != null) handler(this, EventArgs.Empty); }

        private string windowTitle;
        public string WindowTitle { get { return windowTitle; } private set { windowTitle = value; Notify(() => WindowTitle); } }

        private BelagsTyp belagsTyp;
        private List<SchadengruppeDTO> schadengruppeDtos = new List<SchadengruppeDTO>();
        private List<SchadendetailDTO> schadendetailDtos = new List<SchadendetailDTO>();

        private bool hasNoChanges;
        public bool HasNoChanges
        {
            get { return hasNoChanges; }
            set { hasNoChanges = value; Notify(() => HasNoChanges); }
        }

        public ZustandFahrbahnWindowViewModel(
            ZustandsabschnittGISDTO zustandsabschnittGisdto,
            StrassenabschnittGISDTO strassenabschnittGisdto,
            IDTOService dtoService,
            ISchadenMetadatenService schadenMetadatenService,
            IWindowService windowService,
            IMessageBoxService messageBoxService)
        {
            this.zustandsabschnittGisdto = zustandsabschnittGisdto;
            this.strassenabschnittGisdto = strassenabschnittGisdto;
            this.dtoService = dtoService;
            this.schadenMetadatenService = schadenMetadatenService;
            this.messageBoxService = messageBoxService;

            OkCommand = new DelegateCommand(Ok);
            CancelCommand = new DelegateCommand(Cancel);

            RegisterValidation(m => m.Zustandsindex, () => IsValidDecimalWithDecimalPlaces(Zustandsindex, 2), string.Format(MobileLocalization.InvalidDecimalPlacesValidationError, 2));
            RegisterValidation(m => m.Zustandsindex, () => RangeValidator(Zustandsindex, 0, 5), RangeValidationMessage(0, 5));

            GrobItemViewModels = new ObservableCollection<GrobItemViewModel>();
            DetailGroupItemViewModels = new ObservableCollection<DetailGroupItemViewModel>();

            Load(zustandsabschnittGisdto);
            PropertyChanged += (sender, args) =>
                                   {
                                       if (args.PropertyName != ExpressionHelper.GetPropertyName(() => IsValid) &&
                                           args.PropertyName != ExpressionHelper.GetPropertyName(() => ValidationErrorStrings) &&
                                           args.PropertyName != ExpressionHelper.GetPropertyName(() => IsGrobInitializiert) &&
                                           args.PropertyName != ExpressionHelper.GetPropertyName(() => IsDetailInitializiert) &&
                                           args.PropertyName != ExpressionHelper.GetPropertyName(() => HasNoChanges)
                                           )
                                           HasNoChanges = false;
                                   };
            HasNoChanges = true;
        }

        private readonly List<SchadengruppeDTO> temporallySavedSchadengruppeDtos = new List<SchadengruppeDTO>();
        private readonly List<SchadendetailDTO> temporallySavedSchadendetailDtos = new List<SchadendetailDTO>();

        private bool isGrobInitializiert;
        public bool IsGrobInitializiert { get { return isGrobInitializiert; } set { isGrobInitializiert = value; Notify(() => IsGrobInitializiert); } }

        private bool isDetailInitializiert;
        public bool IsDetailInitializiert { get { return isDetailInitializiert; } set { isDetailInitializiert = value; Notify(() => IsDetailInitializiert); } }

        private bool isCloseButtonClicked = true;

        public void Closing()
        {
            if (!isCloseButtonClicked)
                return;

            CancelInternal();
        }

        private void Ok()
        {
            temporallySavedSchadengruppeDtos.Clear();
            temporallySavedSchadendetailDtos.Clear();

            if (ZustandsErfassungsmodus == ZustandsErfassungsmodus.Grob)
            {
                foreach (var grobItemViewModel in GrobItemViewModels)
                    temporallySavedSchadengruppeDtos.Add(CreateSchadengruppeDto(grobItemViewModel));

                IsGrobInitializiert = true;
            }
            else if (ZustandsErfassungsmodus == ZustandsErfassungsmodus.Detail)
            {
                foreach (var groupItem in DetailGroupItemViewModels)
                    foreach (var detailItem in groupItem.DetailItemViewModels)
                        temporallySavedSchadendetailDtos.Add(CreateSchadendetailDto(detailItem));

                IsDetailInitializiert = true;
            }

            isCloseButtonClicked = false;
            OnClosed();
            isCloseButtonClicked = true;
        }

        public void Cancel()
        {
            CancelInternal();
            isCloseButtonClicked = false;
            OnClosed();
            isCloseButtonClicked = true;
            HasNoChanges = true;
        }

        private void CancelInternal()
        {
            if (ZustandsErfassungsmodus == ZustandsErfassungsmodus.Grob)
            {
                schadengruppeDtos.Clear();
                schadengruppeDtos.AddRange(temporallySavedSchadengruppeDtos);
                ChangeToGrob();
            }
            else if (ZustandsErfassungsmodus == ZustandsErfassungsmodus.Detail)
            {
                schadendetailDtos.Clear();
                schadendetailDtos.AddRange(temporallySavedSchadendetailDtos);
                ChangeToDetail();
            }
        }

        public void Init(ZustandsErfassungsmodus zem)
        {
            ZustandsErfassungsmodus = zem;

            RefreshCalculations();
            Notify(() => GrobItemViewModels);
            Notify(() => DetailGroupItemViewModels);
        }

        protected void RegisterValidation<TProperty>(Expression<Func<ZustandFahrbahnWindowViewModel, TProperty>> property, Func<bool> isValidMethod, string validationMessage)
        {
            RegisterValidationGeneric(property, isValidMethod, validationMessage);
        }

        private static bool IsValidDecimalWithDecimalPlaces(decimal? property, int decimalPlaces)
        {
            if (!property.HasValue)
                return true;

            decimal value = property.Value * Convert.ToInt64(Math.Pow(10, decimalPlaces));

            if (value > Int64.MaxValue)
                return false;

            return Convert.ToInt64(value) == value;
        }

        private ZustandsErfassungsmodus zustandsErfassungsmodus;
        public ZustandsErfassungsmodus ZustandsErfassungsmodus
        {
            get { return zustandsErfassungsmodus; }
            set
            {
                if (ZustandsErfassungsmodus != value)
                {
                    zustandsErfassungsmodus = value;

                    switch (ZustandsErfassungsmodus)
                    {
                        case ZustandsErfassungsmodus.Manuel:
                            ChangeToManuel();
                            break;
                        case ZustandsErfassungsmodus.Grob:
                            ChangeToGrob();
                            break;
                        case ZustandsErfassungsmodus.Detail:
                            ChangeToDetail();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("ZustandsErfassungsmodus");
                    }

                    Zustandsindex = zustandsabschnittGisdto.Erfassungsmodus == ZustandsErfassungsmodus ? zustandsabschnittGisdto.Zustandsindex : 0.0m;

                    Notify(() => IsManuel);
                    Notify(() => IsGrob);
                    Notify(() => IsDetail);
                }
            }
        }

        private void ChangeToDetail()
        {
            var schadengruppeMetadatens = schadenMetadatenService.GetSchadengruppeMetadaten(belagsTyp);

            foreach (var detailGroupItemViewModel in DetailGroupItemViewModels)
                detailGroupItemViewModel.PropertyChanged -= ItemViewModelOnPropertyChanged;

            DetailGroupItemViewModels.Clear();
            foreach (var schadengruppeMetadaten in schadengruppeMetadatens)
            {
                var detailGroupItemViewModel = new DetailGroupItemViewModel(schadengruppeMetadaten, schadendetailDtos);
                DetailGroupItemViewModels.Add(detailGroupItemViewModel);
                detailGroupItemViewModel.PropertyChanged += ItemViewModelOnPropertyChanged;
            }

            RefreshCalculations();
        }

        private void ChangeToGrob()
        {
            var schadengruppeMetadatens = schadenMetadatenService.GetSchadengruppeMetadaten(belagsTyp);

            foreach (var grobItemViewModel in GrobItemViewModels)
                grobItemViewModel.PropertyChanged -= ItemViewModelOnPropertyChanged;

            GrobItemViewModels.Clear();
            int rowNumber = 0;
            foreach (var schadengruppeMetadaten in schadengruppeMetadatens)
            {
                var schadengruppeDto = schadengruppeDtos.SingleOrDefault(sg => sg.SchadengruppeTyp == schadengruppeMetadaten.SchadengruppeTyp);
                var grobItemViewModel = new GrobItemViewModel(schadengruppeMetadaten, schadengruppeDto, rowNumber % 2 != 0);
                GrobItemViewModels.Add(grobItemViewModel);
                grobItemViewModel.PropertyChanged += ItemViewModelOnPropertyChanged;
                rowNumber++;
            }

            RefreshCalculations();
        }

        private void ItemViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            RefreshCalculations();
        }

        private void RefreshCalculations()
        {
            if (ZustandsErfassungsmodus == ZustandsErfassungsmodus.Grob)
                Schadensumme = GrobItemViewModels.Sum(ivm => ivm.Bewertung);
            else if (ZustandsErfassungsmodus == ZustandsErfassungsmodus.Detail)
                Schadensumme = DetailGroupItemViewModels.Sum(ivm => ivm.Bewertung);

            Zustandsindex = Math.Min(5, Schadensumme * 0.1m);
        }

        private void ChangeToManuel()
        {
        }

        public bool IsManuel
        {
            get { return ZustandsErfassungsmodus == ZustandsErfassungsmodus.Manuel; }
            set
            {
                if (value)
                    ZustandsErfassungsmodus = ZustandsErfassungsmodus.Manuel;
                Notify(() => IsManuel);
            }
        }

        public bool IsGrob
        {
            get { return ZustandsErfassungsmodus == ZustandsErfassungsmodus.Grob; }
            set
            {
                if (value)
                    ZustandsErfassungsmodus = ZustandsErfassungsmodus.Grob;
                Notify(() => IsGrob);
            }
        }

        public bool IsDetail
        {
            get { return ZustandsErfassungsmodus == ZustandsErfassungsmodus.Detail; }
            set
            {
                if (value)
                    ZustandsErfassungsmodus = ZustandsErfassungsmodus.Detail;
                Notify(() => IsDetail);
            }
        }

        private void Load(ZustandsabschnittGISDTO za)
        {
            if (string.IsNullOrEmpty(za.BezeichnungVon) && string.IsNullOrEmpty(za.BezeichnungBis))
                WindowTitle = string.Format(MobileLocalization.ZustandFahrbahnShortWindowTitle, strassenabschnittGisdto.Strassenname);
            else
                WindowTitle = string.Format(MobileLocalization.ZustandFahrbahnWindowTitle, strassenabschnittGisdto.Strassenname, za.BezeichnungVon, za.BezeichnungBis);

            belagsTyp = strassenabschnittGisdto.Belag;
            schadengruppeDtos = dtoService.Get<SchadengruppeDTO>().Where(sg => sg.ZustandsabschnittId == za.Id).ToList();
            schadendetailDtos = dtoService.Get<SchadendetailDTO>().Where(sg => sg.ZustandsabschnittId == za.Id).ToList();

            temporallySavedSchadendetailDtos.Clear();
            temporallySavedSchadendetailDtos.AddRange(schadendetailDtos);

            temporallySavedSchadengruppeDtos.Clear();
            temporallySavedSchadengruppeDtos.AddRange(schadengruppeDtos);

            ZustandsErfassungsmodus = zustandsabschnittGisdto.Erfassungsmodus;
        }

        public void Save()
        {
            zustandsabschnittGisdto.Zustandsindex = Zustandsindex ?? 0;
            zustandsabschnittGisdto.Erfassungsmodus = ZustandsErfassungsmodus;

            switch (ZustandsErfassungsmodus)
            {
                case ZustandsErfassungsmodus.Manuel:
                    SaveManualMode();
                    break;
                case ZustandsErfassungsmodus.Grob:
                    SaveGrobMode();
                    break;
                case ZustandsErfassungsmodus.Detail:
                    SaveDetailMode();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("ZustandsErfassungsmodus");
            }

            dtoService.CreateOrReplaceDTO(zustandsabschnittGisdto);

            schadengruppeDtos = dtoService.Get<SchadengruppeDTO>().Where(sg => sg.ZustandsabschnittId == zustandsabschnittGisdto.Id).ToList();
            schadendetailDtos = dtoService.Get<SchadendetailDTO>().Where(sg => sg.ZustandsabschnittId == zustandsabschnittGisdto.Id).ToList();

            OnSaved();
        }

        private void SaveManualMode()
        {
            ClearSchadenData();
        }

        private void SaveGrobMode()
        {
            ClearSchadenData();

            foreach (var groupItem in GrobItemViewModels)
            {
                var schadengruppeDto = CreateSchadengruppeDto(groupItem);
                dtoService.CreateOrReplaceDTO(schadengruppeDto);
            }
        }

        private void SaveDetailMode()
        {
            ClearSchadenData();

            foreach (var groupItem in DetailGroupItemViewModels)
            {
                foreach (var detailItem in groupItem.DetailItemViewModels)
                {
                    var schadendetailDto = CreateSchadendetailDto(detailItem);
                    dtoService.CreateOrReplaceDTO(schadendetailDto);
                }
            }
        }

        private SchadendetailDTO CreateSchadendetailDto(DetailItemViewModel detailItem)
        {
            return new SchadendetailDTO
                       {
                           SchadenausmassTyp = detailItem.SchadenausmassTyp,
                           SchadendetailTyp = detailItem.SchadendetailTyp,
                           SchadenschwereTyp = detailItem.SchadenschwereTyp,
                           ZustandsabschnittId = zustandsabschnittGisdto.Id
                       };
        }

        private SchadengruppeDTO CreateSchadengruppeDto(GrobItemViewModel groupItem)
        {
            return new SchadengruppeDTO
                       {
                           Gewicht = groupItem.Gewicht,
                           SchadenausmassTyp = groupItem.SchadenausmassTyp,
                           SchadengruppeTyp = groupItem.SchadengruppeTyp,
                           SchadenschwereTyp = groupItem.SchadenschwereTyp,
                           ZustandsabschnittId = zustandsabschnittGisdto.Id
                       };
        }

        private void ClearSchadenData()
        {
            foreach (var schadengruppeDto in schadengruppeDtos)
                dtoService.DeleteDTO(schadengruppeDto);

            foreach (var schadendetailDto in schadendetailDtos)
                dtoService.DeleteDTO(schadendetailDto);
        }

        public ICommand OkCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        private decimal schadensumme;
        public decimal Schadensumme { get { return schadensumme; } set { schadensumme = value; Notify(() => Schadensumme); } }

        private decimal? zustandsindex;
        public decimal? Zustandsindex { get { return zustandsindex; } set { zustandsindex = value; Notify(() => Zustandsindex); } }

        private ObservableCollection<GrobItemViewModel> grobItemViewModels;
        public ObservableCollection<GrobItemViewModel> GrobItemViewModels { get { return grobItemViewModels; } set { grobItemViewModels = value; Notify(() => GrobItemViewModels); } }

        private ObservableCollection<DetailGroupItemViewModel> detailGroupItemViewModels;
        public ObservableCollection<DetailGroupItemViewModel> DetailGroupItemViewModels { get { return detailGroupItemViewModels; } set { detailGroupItemViewModels = value; Notify(() => DetailGroupItemViewModels); } }
    }
}
