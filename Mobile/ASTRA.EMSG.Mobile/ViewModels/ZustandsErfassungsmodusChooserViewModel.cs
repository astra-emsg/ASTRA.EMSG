using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ASTRA.EMSG.Common.DataTransferObjects;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Services.SchadenMetadaten;
using ASTRA.EMSG.Mobile.Services;
using ASTRA.EMSG.Mobile.Utils;
using ICommand = System.Windows.Input.ICommand;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Mobile.ViewModels
{
    public class ZustandsErfassungsmodusChooserViewModel : EditableViewModel, IZustandsabschnittTabViewModel
    {
        private readonly ZustandsabschnittGISDTO zustandsabschnittGisDto;
        private readonly StrassenabschnittGISDTO strassenabschnittGisdto;
        private readonly IDTOService dtoService;
        private readonly IWindowService windowService;
        private readonly ISchadenMetadatenService schadenMetadatenService;
        private readonly IMessageBoxService messageBoxService;
        private readonly bool isNew;

        public string HeaderText { get { return MobileLocalization.FahrbahnTabTitle; } }
        public bool HasError { get { return !IsValid; } }

        public event EventHandler Changed;

        public void OnChanged()
        {
            EventHandler handler = Changed;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public void Init()
        {
        }

        private ZustandFahrbahnWindowViewModel zustandFahrbahnWindowViewModel;

        public ICommand OpenZustandsFahrbahnWindowCommand { get; set; }

        public ZustandsErfassungsmodusChooserViewModel(
            ZustandsabschnittGISDTO zustandsabschnittGisDto,
            StrassenabschnittGISDTO strassenabschnittGisdto,
            IDTOService dtoService,
            IWindowService windowService,
            ISchadenMetadatenService schadenMetadatenService,
            IMessageBoxService messageBoxService,
            bool isNew)
        {
            this.zustandsabschnittGisDto = zustandsabschnittGisDto;
            this.strassenabschnittGisdto = strassenabschnittGisdto;
            this.dtoService = dtoService;
            this.windowService = windowService;
            this.schadenMetadatenService = schadenMetadatenService;
            this.messageBoxService = messageBoxService;
            this.isNew = isNew;

            OpenZustandsFahrbahnWindowCommand = new DelegateCommand(OpenZustandsFahrbahnWindow);

            RegisterValidation(m => m.Zustandsindex, () => IsValidDecimalWithDecimalPlaces(Zustandsindex, 2), string.Format(MobileLocalization.InvalidDecimalPlacesValidationError, 2));
            RegisterValidation(m => m.Zustandsindex, () => RangeValidator(Zustandsindex, 0, 5), RangeValidationMessage(0, 5));
            RegisterValidation(m => m.Zustandsindex, IsZustandsindexValid, RangeValidationMessage(0, 5));
            RegisterValidation(m => m.IsGrobInitializiert, () => ZustandsErfassungsmodus != ZustandsErfassungsmodus.Grob || IsGrobInitializiert, MobileLocalization.GrobFormIsNotinitialized);
            RegisterValidation(m => m.IsDetailInitializiert, () => ZustandsErfassungsmodus != ZustandsErfassungsmodus.Detail || IsDetailInitializiert, MobileLocalization.DetailFormIsNotinitialized);

            ReCreateZustabdFahrbahnWindowViewModel();

            Load(zustandsabschnittGisDto, isNew);

            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == ExpressionHelper.GetPropertyName(() => Zustandsindex))
                    HasNoChanges = false;
            };

            DelegateEvent(() => IsValid, () => HasError);
            DelegateEvent(() => Zustandsindex, () => ZustandsindexCalculated);

            PropertyChanged += (sender, args) =>
            {
                if (
                    args.PropertyName == ExpressionHelper.GetPropertyName(() => Zustandsindex) ||
                    args.PropertyName == ExpressionHelper.GetPropertyName(() => IsGrobInitializiert) ||
                    args.PropertyName == ExpressionHelper.GetPropertyName(() => IsDetailInitializiert) ||
                    args.PropertyName == ExpressionHelper.GetPropertyName(() => Massnahmenvorschlag) ||
                    args.PropertyName == ExpressionHelper.GetPropertyName(() => ZustandsErfassungsmodus) ||
                    args.PropertyName == ExpressionHelper.GetPropertyName(() => Dringlichkeit)
                    )
                    OnChanged();
            };
        }

        private bool IsZustandsindexValid()
        {
            if (isNew)
            {
                if (HasZustandsIndexChanges)
                    return RequiredValidator(Zustandsindex);
                return true;
            }

            return RequiredValidator(Zustandsindex);
        }

        private bool isGrobDetailZustandserfassungSupported;
        public bool IsGrobDetailZustandserfassungSupported { get { return isGrobDetailZustandserfassungSupported; } set { isGrobDetailZustandserfassungSupported = value; Notify(() => IsGrobDetailZustandserfassungSupported); } }

        protected void RegisterValidation<TProperty>(Expression<Func<ZustandsErfassungsmodusChooserViewModel, TProperty>> property, Func<bool> isValidMethod, string validationMessage)
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


        private void Load(ZustandsabschnittGISDTO dto, bool isNew)
        {
            if(!isNew)
                Zustandsindex = dto.Zustandsindex;

            var massnahmenvorschlagKatalogDtos = dtoService
                .Get<MassnahmenvorschlagKatalogDTO>()
                .Where(mvk => mvk.KatalogTyp == MassnahmenvorschlagKatalogTyp.Fahrbahn && mvk.Belastungskategorie == strassenabschnittGisdto.Belastungskategorie);

            MassnahmenvorschlagList = new List<NameValueItemViewModel<Guid?>> { new NameValueItemViewModel<Guid?>(string.Empty, null) }
                    .Concat(massnahmenvorschlagKatalogDtos
                    .Select(mvk => new NameValueItemViewModel<Guid?>(MobileLocalization.GetLocalizedMassnahmenvorschlag(mvk.Typ), mvk.Id)))
                    .ToList();

            ZustandsErfassungsmodus = zustandsabschnittGisDto.Erfassungsmodus;

            Dringlichkeit = dto.MassnahmenvorschlagFahrbahnDTO == null ? DringlichkeitTyp.Unbekannt : dto.MassnahmenvorschlagFahrbahnDTO.Dringlichkeit;
            Massnahmenvorschlag = dto.MassnahmenvorschlagFahrbahnDTO == null ? null : dto.MassnahmenvorschlagFahrbahnDTO.Typ;

            IsGrobDetailZustandserfassungSupported = strassenabschnittGisdto.Belag != BelagsTyp.Chaussierung && strassenabschnittGisdto.Belag != BelagsTyp.Pflaesterung;

            HasZustandsIndexChanges = false;
            Notify(() => Zustandsindex);
        }

        public void Save()
        {
            if (zustandsabschnittGisDto.MassnahmenvorschlagFahrbahnDTO == null)
                zustandsabschnittGisDto.MassnahmenvorschlagFahrbahnDTO = new MassnahmenvorschlagDTO();

            zustandsabschnittGisDto.MassnahmenvorschlagFahrbahnDTO.Dringlichkeit = Dringlichkeit;
            zustandsabschnittGisDto.MassnahmenvorschlagFahrbahnDTO.Typ = Massnahmenvorschlag;

            if (IsManuel)
            {
                zustandsabschnittGisDto.Zustandsindex = Zustandsindex ?? 0;
                zustandsabschnittGisDto.Erfassungsmodus = ZustandsErfassungsmodus;

                dtoService.CreateOrReplaceDTO(zustandsabschnittGisDto);

                //Clear Schadendetails
                var schadengruppeDtos = dtoService.Get<SchadengruppeDTO>().Where(sg => sg.ZustandsabschnittId == zustandsabschnittGisDto.Id).ToList();
                foreach (var schadengruppeDto in schadengruppeDtos)
                    dtoService.DeleteDTO(schadengruppeDto);

                var schadendetailDtos = dtoService.Get<SchadendetailDTO>().Where(sd => sd.ZustandsabschnittId == zustandsabschnittGisDto.Id).ToList();
                foreach (var schadendetailDto in schadendetailDtos)
                    dtoService.DeleteDTO(schadendetailDto);
            }
            else
            {
                zustandFahrbahnWindowViewModel.Save();
            }

            HasNoChanges = true;
        }

        private void OpenZustandsFahrbahnWindow()
        {
            zustandFahrbahnWindowViewModel.Init(ZustandsErfassungsmodus);
            HasNoChanges = true;
            windowService.OpenZustandFahrbahnWindow(zustandFahrbahnWindowViewModel, () => { Zustandsindex = zustandFahrbahnWindowViewModel.Zustandsindex; });
        }

        private DringlichkeitTyp dringlichkeit;
        public DringlichkeitTyp Dringlichkeit { get { return dringlichkeit; } set { dringlichkeit = value; Notify(() => Dringlichkeit); } }

        private Guid? massnahmenvorschlag;
        public Guid? Massnahmenvorschlag { get { return massnahmenvorschlag; } set { massnahmenvorschlag = value; UpdateKosten(); Notify(() => Massnahmenvorschlag); } }

        private void UpdateKosten()
        {
            if (Massnahmenvorschlag.HasValue)
            {
                var massnahmenvorschlagKatalogDto = dtoService.GetDTOByID<MassnahmenvorschlagKatalogDTO>(Massnahmenvorschlag.Value);
                Kosten = massnahmenvorschlagKatalogDto.DefaultKosten;

                Gesamtkosten = Math.Round((zustandsabschnittGisDto.Laenge ?? 0) * (strassenabschnittGisdto.BreiteFahrbahn ?? 0) * (Kosten ?? 0), 2);
            }
            else
            {
                Kosten = null;
                Gesamtkosten = null;
            }
        }

        private decimal? kosten;
        public decimal? Kosten { get { return kosten; } set { kosten = value; Notify(() => Kosten); } }

        private decimal? gesamtkosten;
        public decimal? Gesamtkosten { get { return gesamtkosten; } set { gesamtkosten = value; Notify(() => Gesamtkosten); } }

        private List<NameValueItemViewModel<Guid?>> massnahmenvorschlagList;
        public List<NameValueItemViewModel<Guid?>> MassnahmenvorschlagList { get { return massnahmenvorschlagList; } set { massnahmenvorschlagList = value; Notify(() => MassnahmenvorschlagList); } }

        public List<NameValueItemViewModel<DringlichkeitTyp>> DringlichkeitList
        {
            get
            {
                return new List<NameValueItemViewModel<DringlichkeitTyp>>
                           {
                               new NameValueItemViewModel<DringlichkeitTyp>(MobileLocalization.DringlichkeitTypUnbekannt, DringlichkeitTyp.Unbekannt),
                               new NameValueItemViewModel<DringlichkeitTyp>(MobileLocalization.DringlichkeitTypDringlich, DringlichkeitTyp.Dringlich),
                               new NameValueItemViewModel<DringlichkeitTyp>(MobileLocalization.DringlichkeitTypMittelfristig, DringlichkeitTyp.Mittelfristig),
                               new NameValueItemViewModel<DringlichkeitTyp>(MobileLocalization.DringlichkeitTypLangfristig, DringlichkeitTyp.Langfristig),
                           };
            }
        }

        private decimal? zustandsindex;
        public decimal? Zustandsindex
        {
            get { return zustandsindex; }
            set
            {
                zustandsindex = value;
                HasZustandsIndexChanges = true;
                Notify(() => Zustandsindex);
            }
        }

        public decimal? ZustandsindexCalculated
        {
            get
            {
                if (ZustandsErfassungsmodus == ZustandsErfassungsmodus.Grob && !IsGrobInitializiert)
                    return null;

                if (ZustandsErfassungsmodus == ZustandsErfassungsmodus.Detail && !IsDetailInitializiert)
                    return null;

                return Zustandsindex;
            }
        }

        private ZustandsErfassungsmodus zustandsErfassungsmodus;
        public ZustandsErfassungsmodus ZustandsErfassungsmodus
        {
            get { return zustandsErfassungsmodus; }
            set
            {
                if (zustandsErfassungsmodus != value)
                {
                    zustandsErfassungsmodus = value;

                    ReCreateZustabdFahrbahnWindowViewModel();

                    if (ZustandsErfassungsmodus == ZustandsErfassungsmodus.Grob)
                        zustandFahrbahnWindowViewModel.IsGrobInitializiert = zustandsabschnittGisDto.Erfassungsmodus == ZustandsErfassungsmodus;

                    if (ZustandsErfassungsmodus == ZustandsErfassungsmodus.Detail)
                        zustandFahrbahnWindowViewModel.IsDetailInitializiert = zustandsabschnittGisDto.Erfassungsmodus == ZustandsErfassungsmodus;

                    if (ZustandsErfassungsmodus == ZustandsErfassungsmodus.Manuel && isNew)
                    {
                        Zustandsindex = null;
                    }
                    else
                    {
                        Zustandsindex = zustandsabschnittGisDto.Erfassungsmodus == ZustandsErfassungsmodus
                                            ? zustandsabschnittGisDto.Zustandsindex
                                            : (decimal?) null;
                    }

                    Notify(() => IsManuel);
                    Notify(() => IsGrob);
                    Notify(() => IsDetail);
                }
            }
        }

        private void ReCreateZustabdFahrbahnWindowViewModel()
        {
            zustandFahrbahnWindowViewModel = new ZustandFahrbahnWindowViewModel(zustandsabschnittGisDto, strassenabschnittGisdto, dtoService, schadenMetadatenService, windowService, messageBoxService);
            zustandFahrbahnWindowViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == ExpressionHelper.GetPropertyName(() => HasNoChanges))
                    HasNoChanges = zustandFahrbahnWindowViewModel.HasNoChanges;

                if (args.PropertyName == ExpressionHelper.GetPropertyName(() => IsGrobInitializiert))
                    Notify(() => IsGrobInitializiert);

                if (args.PropertyName == ExpressionHelper.GetPropertyName(() => IsDetailInitializiert))
                    Notify(() => IsDetailInitializiert);
            };

            HasNoChanges = true;

            Notify(() => IsGrobInitializiert);
            Notify(() => IsDetailInitializiert);
        }

        public bool IsGrobInitializiert { get { return zustandFahrbahnWindowViewModel.IsGrobInitializiert; } }
        public bool IsDetailInitializiert { get { return zustandFahrbahnWindowViewModel.IsDetailInitializiert; } }

        public bool IsManuel
        {
            get { return ZustandsErfassungsmodus == ZustandsErfassungsmodus.Manuel; }
            set
            {
                if (!Zustandsindex.HasValue || messageBoxService.Comfirm(MobileLocalization.ErfassungsmodusChangedConfirmation))
                {
                    if (value)
                    {
                        ZustandsErfassungsmodus = ZustandsErfassungsmodus.Manuel;
                        Notify(() => IsManuel);
                        HasNoChanges = true;
                    }
                }
            }
        }

        public bool IsGrob
        {
            get { return ZustandsErfassungsmodus == ZustandsErfassungsmodus.Grob; }
            set
            {
                if (!Zustandsindex.HasValue || messageBoxService.Comfirm(MobileLocalization.ErfassungsmodusChangedConfirmation))
                {
                    if (value)
                    {
                        ZustandsErfassungsmodus = ZustandsErfassungsmodus.Grob;
                        Notify(() => IsGrob);
                        HasNoChanges = true;
                    }
                }
            }
        }

        public bool IsDetail
        {
            get { return ZustandsErfassungsmodus == ZustandsErfassungsmodus.Detail; }
            set
            {
                if (!Zustandsindex.HasValue || messageBoxService.Comfirm(MobileLocalization.ErfassungsmodusChangedConfirmation))
                {
                    if (value)
                    {
                        ZustandsErfassungsmodus = ZustandsErfassungsmodus.Detail;
                        Notify(() => IsDetail);
                        HasNoChanges = true;
                    }
                }
            }
        }

        private bool hasNoChanges = true;
        protected bool HasNoChanges
        {
            get { return hasNoChanges; }
            set { hasNoChanges = value; }
        }

        private bool hasZustandsIndexChanges;
        public bool HasZustandsIndexChanges
        {
            get { return hasZustandsIndexChanges; }
            set { hasZustandsIndexChanges = value; }
        }

        public override void RefreshValidation()
        {
            HasZustandsIndexChanges = true;
            Notify(() => Zustandsindex);
            base.RefreshValidation();
        }

        public void OpenHelp()
        {
            windowService.OpenHelpWindow(HelpFileNames.ZustandsabschnittFahrbahnZustandHelpPage);
        }
    }
}
