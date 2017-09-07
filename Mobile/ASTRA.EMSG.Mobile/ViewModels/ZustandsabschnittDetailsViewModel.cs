using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ASTRA.EMSG.Common.DataTransferObjects;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Mobile.Services;
using System.Linq;
using ASTRA.EMSG.Mobile.Utils;
using ASTRA.EMSG.Common;
using System.Windows.Input;

namespace ASTRA.EMSG.Mobile.ViewModels
{
    public class ZustandsabschnittDetailsViewModel : EditableViewModel, IZustandsabschnittTabViewModel
    {
        private readonly ZustandsabschnittGISDTO zustandsabschnittGisdto;
        private readonly StrassenabschnittGISDTO strassenabschnittGisdto;
        private readonly IDTOService dtoService;
        private readonly IWindowService windowService;

        public event EventHandler Changed;

        public void OnChanged()
        {
            EventHandler handler = Changed;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public string HeaderText { get { return MobileLocalization.AbschnittTabTitle; } }
        public bool HasError { get { return !IsValid; } }
        public void Init()
        {
        }

        public void OpenHelp()
        {
            windowService.OpenHelpWindow(HelpFileNames.ZustandsabschnittFormHelpPage);
        }

        public ZustandsabschnittDetailsViewModel(
            ZustandsabschnittGISDTO zustandsabschnittGisdto,
            StrassenabschnittGISDTO strassenabschnittGisdto,
            IDTOService dtoService,
            IWindowService windowService)
        {
            this.zustandsabschnittGisdto = zustandsabschnittGisdto;
            this.strassenabschnittGisdto = strassenabschnittGisdto;
            this.dtoService = dtoService;
            this.windowService = windowService;

            RegisterValidation(vm => vm.Aufnahmeteam, () => LenghtValidator(Aufnahmeteam), LengthValidationMessage());
            RegisterValidation(vm => vm.Bemerkung, () => LenghtValidator(Bemerkung, 8000), LengthValidationMessage(8000));

            RegisterValidation(vm => vm.Aufnahmedatum, () => DateTimeValidator(Aufnahmedatum), DateTimeValidationMessage());
            RegisterValidation(vm => vm.Aufnahmedatum, () => RequiredValidator(Aufnahmedatum), MobileLocalization.RequiredValidationError);

            RegisterValidation(vm => vm.Abschnittsnummer, () => RangeValidator(Abschnittsnummer), RangeValidationMessage());

            RegisterValidation(vm => vm.Laenge, () => RequiredValidator(Laenge), MobileLocalization.GeometryShouldBeNotNull);
            RegisterValidation(vm => vm.Laenge, () => RangeValidator(Laenge, 1), RangeValidationMessage(1));
            RegisterValidation(vm => vm.Laenge, ValidateZustandsabschnittStrassenabschnittLaenge, ZustandsabschnittStrassenabschnittLaengeValidationMessage());

            Load(zustandsabschnittGisdto, strassenabschnittGisdto);

            DelegateEvent(() => IsValid, () => HasError);
            

            PropertyChanged += (sender, args) =>
                                   {
                                       if (
                                           args.PropertyName == ExpressionHelper.GetPropertyName(() => Aufnahmedatum) ||
                                           args.PropertyName == ExpressionHelper.GetPropertyName(() => Laenge) ||
                                           args.PropertyName == ExpressionHelper.GetPropertyName(() => Aufnahmeteam) ||
                                           args.PropertyName == ExpressionHelper.GetPropertyName(() => Bemerkung) ||
                                           args.PropertyName == ExpressionHelper.GetPropertyName(() => Wetter)
                                           )
                                           OnChanged();
                                   };
        }

        private string ZustandsabschnittStrassenabschnittLaengeValidationMessage()
        {
            return string.Format(MobileLocalization.StrassenabschnittZustandsabschnittLaengeError, strassenabschnittGisdto.Laenge);
        }

        private bool ValidateZustandsabschnittStrassenabschnittLaenge()
        {
            System.Console.WriteLine("Anzahl DTOS, zur Berechnung: " + dtoService.Get<ZustandsabschnittGISDTO>()
                .Where(za => za.StrassenabschnittGIS == strassenabschnittGisdto.Id && za.Id != zustandsabschnittGisdto.Id)
                .Count());

            var zustandsabschnittLaengeSum = dtoService.Get<ZustandsabschnittGISDTO>()
                .Where(za => za.StrassenabschnittGIS == strassenabschnittGisdto.Id && za.Id != zustandsabschnittGisdto.Id)
                .Sum(za => za.Laenge);

            //compensation for rounding differences
            return strassenabschnittGisdto.Laenge + (decimal)0.5 >= zustandsabschnittLaengeSum + (Laenge ?? 0);
        }

        private void Load(ZustandsabschnittGISDTO za, StrassenabschnittGISDTO sa)
        {
            Laenge = za.Laenge;
            Aufnahmedatum = za.Aufnahmedatum;
            Aufnahmeteam = za.Aufnahmeteam;
            Abschnittsnummer = za.Abschnittsnummer;
            Wetter = za.Wetter;
            Bemerkung = za.Bemerkung;
            FlaecheFahrbahn = za.FlaecheFahrbahn;
            FlaecheTrottoirLinks = za.FlaceheTrottoirLinks;
            FlaecheTrottoirRechts = za.FlaceheTrottoirRechts;
            HasTrottoir = sa.Trottoir != TrottoirTyp.NochNichtErfasst && sa.Trottoir != TrottoirTyp.KeinTrottoir;
        }

        protected void RegisterValidation<TProperty>(Expression<Func<ZustandsabschnittDetailsViewModel, TProperty>> property, Func<bool> isValidMethod, string validationMessage)
        {
            RegisterValidationGeneric(property, isValidMethod, validationMessage);
        }
        
        public void Save()
        {
            if (IsValid)
            {
                zustandsabschnittGisdto.Laenge = Laenge;
                zustandsabschnittGisdto.Aufnahmedatum = Aufnahmedatum;
                zustandsabschnittGisdto.Aufnahmeteam = Aufnahmeteam;
                zustandsabschnittGisdto.Abschnittsnummer = Abschnittsnummer;
                zustandsabschnittGisdto.Wetter = Wetter;
                zustandsabschnittGisdto.Bemerkung = Bemerkung;
                zustandsabschnittGisdto.FlaecheFahrbahn = FlaecheFahrbahn;
                zustandsabschnittGisdto.FlaceheTrottoirLinks = FlaecheTrottoirLinks;
                zustandsabschnittGisdto.FlaceheTrottoirRechts = FlaecheTrottoirRechts;
                var strab = dtoService.GetDTOByID<StrassenabschnittGISDTO>(zustandsabschnittGisdto.StrassenabschnittGIS);
                if(!strab.ZustandsabschnittenId.Any(s => s.Equals(zustandsabschnittGisdto.Id)))
                {
                    strab.ZustandsabschnittenId.Add(zustandsabschnittGisdto.Id);
                }
                dtoService.CreateOrReplaceDTO(zustandsabschnittGisdto);
                dtoService.CreateOrReplaceDTO(strab);
            }
        }

        private bool hasTrottoir;
        public bool HasTrottoir { get { return hasTrottoir; } set { hasTrottoir = value; Notify(() => HasTrottoir); } }

        private decimal? laenge;
        public decimal? Laenge
        {
            get { return laenge; }
            set
            {
                laenge = value.HasValue ? Math.Round(value.Value, 2) : (decimal?)null;

                FlaecheFahrbahn = Math.Round((strassenabschnittGisdto.BreiteFahrbahn ?? 0) * (Laenge ?? 0), 2);
                FlaecheTrottoirLinks = Math.Round((strassenabschnittGisdto.BreiteTrottoirLinks ?? 0) * (Laenge ?? 0), 2);
                FlaecheTrottoirRechts = Math.Round((strassenabschnittGisdto.BreiteTrottoirRechts ?? 0) * (Laenge ?? 0), 2);

                Notify(() => Laenge);
            }
        }

        private decimal? flaecheFahrbahn;
        public decimal? FlaecheFahrbahn { get { return flaecheFahrbahn; } set { flaecheFahrbahn = value; Notify(() => FlaecheFahrbahn); } }

        private decimal? flaecheTrottoirLinks;
        public decimal? FlaecheTrottoirLinks { get { return flaecheTrottoirLinks; } set { flaecheTrottoirLinks = value; Notify(() => FlaecheTrottoirLinks); } }

        private decimal? flaecheTrottoirRechts;
        public decimal? FlaecheTrottoirRechts { get { return flaecheTrottoirRechts; } set { flaecheTrottoirRechts = value; Notify(() => FlaecheTrottoirRechts); } }

        private DateTime? aufnahmedatum;
        public DateTime? Aufnahmedatum { get { return aufnahmedatum; } set { aufnahmedatum = value; Notify(() => Aufnahmedatum); } }

        private string aufnahmeteam;
        public string Aufnahmeteam { get { return aufnahmeteam; } set { aufnahmeteam = value; Notify(() => Aufnahmeteam); } }

        private int? abschnittsnummer;
        public int? Abschnittsnummer { get { return abschnittsnummer; } set { abschnittsnummer = value; Notify(() => Abschnittsnummer); } }

        private WetterTyp wetter;
        public WetterTyp Wetter { get { return wetter; } set { wetter = value; Notify(() => Wetter); } }

        public List<NameValueItemViewModel<WetterTyp>> WetterList
        {
            get
            {
                return new List<NameValueItemViewModel<WetterTyp>>
                             {
                                 new NameValueItemViewModel<WetterTyp>(MobileLocalization.WetterTypKeinRegen, WetterTyp.KeinRegen),
                                 new NameValueItemViewModel<WetterTyp>(MobileLocalization.WetterTypRegen, WetterTyp.Regen),
                             };
            }
        }

        private string bemerkung;
        public string Bemerkung { get { return bemerkung; } set { bemerkung = value; Notify(() => Bemerkung); } }
    }
}
