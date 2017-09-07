using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Windows.Input;
using ASTRA.EMSG.Common.DataTransferObjects;
using ASTRA.EMSG.Common.DataTransferObjects.EventArgs;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Services.SchadenMetadaten;

using ASTRA.EMSG.Mobile.Services;
using ASTRA.EMSG.Mobile.Utils;
using System.Linq;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Map.Services;
using GeoAPI.Geometries;

namespace ASTRA.EMSG.Mobile.ViewModels
{
    public class ZustandsabschnittViewModel : EditableViewModel
    {
        private readonly ZustandsabschnittGISDTO zustandsabschnittGisDto;
        private readonly StrassenabschnittGISDTO strassenabschnittGisdto;

        private readonly IDTOService dtoService;
        private readonly IFormService formService;
        private readonly IMessageBoxService messageBoxService;

        private readonly IGeoJsonService geoJsonService;

        private IZustandsabschnittTabViewModel selectedTabViewModel;

        public event EventHandler Cancelled;
        public event EventHandler<SaveEventArgs> Saved;
        public event EventHandler Deleted;

        private void OnCancelled() { var handler = Cancelled; if (handler != null) handler(this, EventArgs.Empty); }
        private void OnSaved(bool closeWindow = true) { var handler = Saved; if (handler != null) handler(this, new SaveEventArgs(closeWindow)); }
        private void OnDeleted() { var handler = Deleted; if (handler != null) handler(this, EventArgs.Empty); }

        public ZustandsabschnittDetailsViewModel ZustandsabschnittDetailsViewModel { get; set; }
        public ZustandsErfassungsmodusChooserViewModel ZustandsErfassungsmodusChooserViewModel { get; set; }
        public ZustandTrottoirViewModel ZustandTrottoirViewModel { get; set; }

        public ZustandsabschnittViewModel(
            ZustandsabschnittGISDTO zustandsabschnittGisDto,
            StrassenabschnittGISDTO strassenabschnittGisdto,
            IDTOService dtoService,
            IWindowService windowService,
            ISchadenMetadatenService schadenMetadatenService,
            IFormService formService,
            IMessageBoxService messageBoxService,
            IGeoJsonService geoJsonService,
            bool isNew = false)
        {
            this.zustandsabschnittGisDto = zustandsabschnittGisDto;
            this.strassenabschnittGisdto = strassenabschnittGisdto;
            this.dtoService = dtoService;
            this.formService = formService;
            this.messageBoxService = messageBoxService;
            this.geoJsonService = geoJsonService;

            ZustandsabschnittDetailsViewModel = new ZustandsabschnittDetailsViewModel(zustandsabschnittGisDto, strassenabschnittGisdto, dtoService, windowService);
            ZustandsErfassungsmodusChooserViewModel = new ZustandsErfassungsmodusChooserViewModel(zustandsabschnittGisDto, strassenabschnittGisdto, dtoService, windowService, schadenMetadatenService, messageBoxService, isNew);

            TabViewModels = new ObservableCollection<IZustandsabschnittTabViewModel>
                {
                    ZustandsabschnittDetailsViewModel,
                    ZustandsErfassungsmodusChooserViewModel
                };

            if (strassenabschnittGisdto.Trottoir != TrottoirTyp.KeinTrottoir && strassenabschnittGisdto.Trottoir != TrottoirTyp.NochNichtErfasst)
            {
                ZustandTrottoirViewModel = new ZustandTrottoirViewModel(zustandsabschnittGisDto, strassenabschnittGisdto, dtoService, windowService);
                TabViewModels.Add(ZustandTrottoirViewModel);
            }

            SaveCommand = new DelegateCommand(Save);
            ApplyCommand = new DelegateCommand(Apply);
            CancelCommand = new DelegateCommand(Cancel);
            DeleteCommand = new DelegateCommand(Delete);
            OpenHelpWindowCommand = new DelegateCommand(() => { if (SelectedTabViewModel != null) SelectedTabViewModel.OpenHelp(); });

            foreach (var tabViewModel in TabViewModels)
            {
                tabViewModel.PropertyChanged += (sender, args) =>
                    {
                        if (args.PropertyName == ExpressionHelper.GetPropertyName(() => IsValid))
                            IsChildsValid = TabViewModels.All(tvm => tvm.IsValid);
                    };

                tabViewModel.Changed += (sender, args) => { HasChanges = true; };
            }

            IsNew = isNew;

            RegisterValidation(vm => vm.BezeichnungVon, () => LenghtValidator(BezeichnungVon), LengthValidationMessage());
            RegisterValidation(vm => vm.BezeichnungBis, () => LenghtValidator(BezeichnungBis), LengthValidationMessage());

            Load();

            PropertyChanged += (sender, args) =>
                                   {
                                       if (
                                           args.PropertyName == ExpressionHelper.GetPropertyName(() => BezeichnungVon) ||
                                           args.PropertyName == ExpressionHelper.GetPropertyName(() => BezeichnungBis)
                                           )
                                           HasChanges = true;
                                   };
        }

        private bool hasChanges;
        public bool HasChanges { get { return hasChanges; } set { hasChanges = value; Notify(() => HasChanges); } }

        protected void RegisterValidation<TProperty>(Expression<Func<ZustandsabschnittViewModel, TProperty>> property, Func<bool> isValidMethod, string validationMessage)
        {
            RegisterValidationGeneric(property, isValidMethod, validationMessage);
        }

        private void Load()
        {
            Strassenname = strassenabschnittGisdto.Strassenname;
            StrassennameBezeichnungVon = strassenabschnittGisdto.BezeichnungVon;
            StrassennameBezeichnungBis = strassenabschnittGisdto.BezeichnungBis;
            BezeichnungVon = zustandsabschnittGisDto.BezeichnungVon;
            BezeichnungBis = zustandsabschnittGisDto.BezeichnungBis;
            ReferenzGruppe = zustandsabschnittGisDto.ReferenzGruppeDTO;
            Shape = zustandsabschnittGisDto.Shape;

            Notify(() => StrasseBezeichnung);

            HasChanges = false;
        }

        public ICommand OpenHelpWindowCommand { get; private set; }

        public ObservableCollection<IZustandsabschnittTabViewModel> TabViewModels { get; set; }

        public IZustandsabschnittTabViewModel SelectedTabViewModel { get { return selectedTabViewModel; } set { selectedTabViewModel = value; Notify(() => SelectedTabViewModel); } }

        private void Delete()
        {
            if (!messageBoxService.Comfirm(MobileLocalization.DeleteConfirmation))
                return;

            dtoService.DeleteZustandsabschnitt(zustandsabschnittGisDto);

            formService.OnZustandsabschnittDeleted(new DeleteZustandsabschnittDataTransferEventArgs(zustandsabschnittGisDto.Id));

            OnDeleted();

            HasChanges = false;
        }

        private void Cancel()
        {
            if (HasChanges && !messageBoxService.Comfirm(MobileLocalization.DiscardChanges))
                return;

            formService.OnZustandsabschnittCancelled();
            OnCancelled();

            HasChanges = false;
        }

        public void Save()
        {
            SaveInternal(true);
        }

        public void Apply()
        {
            SaveInternal(false);
        }

        private void SaveInternal(bool closeWindow)
        {
            foreach (var tabViewModel in TabViewModels)
                tabViewModel.RefreshValidation();

            if (IsValid)
            {
                foreach (var tabViewModel in TabViewModels)
                    tabViewModel.Save();

                zustandsabschnittGisDto.Shape = Shape;
                zustandsabschnittGisDto.BezeichnungVon = BezeichnungVon;
                zustandsabschnittGisDto.BezeichnungBis = BezeichnungBis;
                zustandsabschnittGisDto.ReferenzGruppeDTO = ReferenzGruppe;

                if (closeWindow)
                {
                    formService.OnZustandsabschnittSaved(new SaveZustandsabschnittDataTransferEventArgs(zustandsabschnittGisDto.Id, zustandsabschnittGisDto.Zustandsindex, geoJsonService.GenerateGeoJsonStringFromEntity(zustandsabschnittGisDto)));
                }
                else
                {
                    formService.OnZustandsabschnittApplySave(new SaveZustandsabschnittDataTransferEventArgs(zustandsabschnittGisDto.Id, zustandsabschnittGisDto.Zustandsindex, geoJsonService.GenerateGeoJsonStringFromEntity(zustandsabschnittGisDto)));
                    IsNew = false;
                }
                dtoService.CreateOrReplaceDTO(zustandsabschnittGisDto);

                OnSaved(closeWindow);

                HasChanges = false;
            }
        }

        public ICommand SaveCommand { get; private set; }
        public ICommand ApplyCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        private bool isNew;
        public bool IsNew { get { return isNew; } private set { isNew = value; Notify(() => IsNew); } }

        public string StrasseBezeichnung
        {
            get
            {
                var comma = (string.IsNullOrWhiteSpace(StrassennameBezeichnungVon) && string.IsNullOrWhiteSpace(StrassennameBezeichnungBis)) ? string.Empty : ",";
                var von = string.IsNullOrWhiteSpace(StrassennameBezeichnungVon) ? string.Empty : string.Format("{0} {1}", MobileLocalization.From, StrassennameBezeichnungVon);
                var bis = string.IsNullOrWhiteSpace(StrassennameBezeichnungBis) ? string.Empty : string.Format("{0} {1}", MobileLocalization.To, StrassennameBezeichnungBis);
                return string.Format("{0}{1} {2} {3}", Strassenname, comma, von, bis);
            }
        }

        private string strassenname;
        public string Strassenname { get { return strassenname; } set { strassenname = value; Notify(() => Strassenname); } }

        private string strassennameBezeichnungVon;
        public string StrassennameBezeichnungVon { get { return strassennameBezeichnungVon; } set { strassennameBezeichnungVon = value; Notify(() => StrassennameBezeichnungVon); } }

        private string strassennameBezeichnungBis;
        public string StrassennameBezeichnungBis { get { return strassennameBezeichnungBis; } set { strassennameBezeichnungBis = value; Notify(() => StrassennameBezeichnungBis); } }

        private string bezeichnungVon;
        public string BezeichnungVon { get { return bezeichnungVon; } set { bezeichnungVon = value; Notify(() => BezeichnungVon); } }

        private string bezeichnungBis;
        public string BezeichnungBis { get { return bezeichnungBis; } set { bezeichnungBis = value; Notify(() => BezeichnungBis); } }

        public ReferenzGruppeDTO ReferenzGruppe { get; set; }
        public IGeometry Shape { get; set; }
    }
}
