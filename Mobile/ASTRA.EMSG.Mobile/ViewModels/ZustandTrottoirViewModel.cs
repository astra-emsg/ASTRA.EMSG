using System;
using System.Collections.Generic;
using System.Windows.Input;
using ASTRA.EMSG.Common.DataTransferObjects;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Mobile.Services;
using ASTRA.EMSG.Mobile.Utils;
using System.Linq;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Mobile.ViewModels
{
    public class ZustandTrottoirViewModel : EditableViewModel, IZustandsabschnittTabViewModel
    {
        private readonly ZustandsabschnittGISDTO zustandsabschnittGisdto;
        private readonly StrassenabschnittGISDTO strassenabschnittGisdto;
        private readonly IDTOService dtoService;
        private readonly IWindowService windowService;

        private string windowTitle;
        public string WindowTitle { get { return windowTitle; } private set { windowTitle = value; Notify(() => WindowTitle); } }

        public string HeaderText { get { return MobileLocalization.TrottoirTabTitle; } }
        public bool HasError { get { return false; } }

        public event EventHandler Changed;

        public void OnChanged()
        {
            EventHandler handler = Changed;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public void Init()
        {
        }

        public void OpenHelp()
        {
            windowService.OpenHelpWindow(HelpFileNames.ZustandsabschnittTrottoirZustandHelpPage);
        }

        public ZustandTrottoirViewModel(
            ZustandsabschnittGISDTO zustandsabschnittGisdto,
            StrassenabschnittGISDTO strassenabschnittGisdto,
            IDTOService dtoService,
            IWindowService windowService)
        {
            this.zustandsabschnittGisdto = zustandsabschnittGisdto;
            this.strassenabschnittGisdto = strassenabschnittGisdto;
            this.dtoService = dtoService;
            this.windowService = windowService;

            Load(zustandsabschnittGisdto);

            PropertyChanged += (sender, args) =>
            {
                if (
                    args.PropertyName == ExpressionHelper.GetPropertyName(() => ZustandsindexTrottoirLinks) ||
                    args.PropertyName == ExpressionHelper.GetPropertyName(() => ZustandsindexTrottoirRechts) ||
                    args.PropertyName == ExpressionHelper.GetPropertyName(() => DringlichkeitTrottoirLinks) ||
                    args.PropertyName == ExpressionHelper.GetPropertyName(() => DringlichkeitTrottoirRechts) ||
                    args.PropertyName == ExpressionHelper.GetPropertyName(() => MassnahmenvorschlagTrottoirLinks) ||
                    args.PropertyName == ExpressionHelper.GetPropertyName(() => MassnahmenvorschlagTrottoirRechts)
                    )
                    OnChanged();
            };
        }

        private bool hasTrottoirLinks;
        public bool HasTrottoirLinks { get { return hasTrottoirLinks; } set { hasTrottoirLinks = value; Notify(() => HasTrottoirLinks); } }

        private bool hasTrottoirRechts;
        public bool HasTrottoirRechts { get { return hasTrottoirRechts; } set { hasTrottoirRechts = value; Notify(() => HasTrottoirRechts); } }

        private bool hasTrottoirBeideSeiten;
        public bool HasTrottoirBeideSeiten { get { return hasTrottoirBeideSeiten; } set { hasTrottoirBeideSeiten = value; Notify(() => HasTrottoirBeideSeiten); } }

        private void Load(ZustandsabschnittGISDTO za)
        {
            var strassenabschnitt = dtoService.GetDTOByID<StrassenabschnittGISDTO>(za.StrassenabschnittGIS);
            var massnahmenvorschlagKatalogDtos = dtoService
                .Get<MassnahmenvorschlagKatalogDTO>()
                .Where(mvk => mvk.KatalogTyp == MassnahmenvorschlagKatalogTyp.Trottoir && mvk.Belastungskategorie == strassenabschnitt.Belastungskategorie);

            MassnahmenvorschlagList = new List<NameValueItemViewModel<Guid?>> { new NameValueItemViewModel<Guid?>(string.Empty, null) }
                    .Concat(massnahmenvorschlagKatalogDtos
                    .Select(mvk => new NameValueItemViewModel<Guid?>(MobileLocalization.GetLocalizedMassnahmenvorschlag(mvk.Typ), mvk.Id)))
                    .ToList();

            if (string.IsNullOrEmpty(za.BezeichnungVon) && string.IsNullOrEmpty(za.BezeichnungBis))
                WindowTitle = string.Format(MobileLocalization.ZustandTrottoirShortWindowTitle, strassenabschnittGisdto.Strassenname);
            else
                WindowTitle = string.Format(MobileLocalization.ZustandTrottoirtWindowTitle, strassenabschnittGisdto.Strassenname, za.BezeichnungVon, za.BezeichnungBis);

            HasTrottoirBeideSeiten = strassenabschnittGisdto.Trottoir == TrottoirTyp.BeideSeiten;

            HasTrottoirLinks = strassenabschnittGisdto.Trottoir == TrottoirTyp.Links ||
                               strassenabschnittGisdto.Trottoir == TrottoirTyp.BeideSeiten;

            HasTrottoirRechts = strassenabschnittGisdto.Trottoir == TrottoirTyp.Rechts ||
                               strassenabschnittGisdto.Trottoir == TrottoirTyp.BeideSeiten;

            ZustandsindexTrottoirLinks = za.ZustandsindexTrottoirLinks;
            ZustandsindexTrottoirRechts = za.ZustandsindexTrottoirRechts;

            DringlichkeitTrottoirLinks = za.MassnahmenvorschlagLinks == null ? DringlichkeitTyp.Unbekannt : za.MassnahmenvorschlagLinks.Dringlichkeit;
            DringlichkeitTrottoirRechts = za.MassnahmenvorschlagRechts == null ? DringlichkeitTyp.Unbekannt : za.MassnahmenvorschlagRechts.Dringlichkeit;

            MassnahmenvorschlagTrottoirLinks = za.MassnahmenvorschlagLinks == null ? null : za.MassnahmenvorschlagLinks.Typ;
            MassnahmenvorschlagTrottoirRechts = za.MassnahmenvorschlagRechts == null ? null : za.MassnahmenvorschlagRechts.Typ;
        }

        public void Save()
        {
            zustandsabschnittGisdto.ZustandsindexTrottoirLinks = ZustandsindexTrottoirLinks;
            zustandsabschnittGisdto.ZustandsindexTrottoirRechts = ZustandsindexTrottoirRechts;

            if (zustandsabschnittGisdto.MassnahmenvorschlagLinks == null)
                zustandsabschnittGisdto.MassnahmenvorschlagLinks = new MassnahmenvorschlagDTO();

            zustandsabschnittGisdto.MassnahmenvorschlagLinks.Dringlichkeit = DringlichkeitTrottoirLinks;
            zustandsabschnittGisdto.MassnahmenvorschlagLinks.Typ = MassnahmenvorschlagTrottoirLinks;


            if (zustandsabschnittGisdto.MassnahmenvorschlagRechts == null)
                zustandsabschnittGisdto.MassnahmenvorschlagRechts = new MassnahmenvorschlagDTO();

            zustandsabschnittGisdto.MassnahmenvorschlagRechts.Dringlichkeit = DringlichkeitTrottoirRechts;
            zustandsabschnittGisdto.MassnahmenvorschlagRechts.Typ = MassnahmenvorschlagTrottoirRechts;
        }

        private ZustandsindexTyp zustandsindexTrottoirLinks;
        public ZustandsindexTyp ZustandsindexTrottoirLinks { get { return zustandsindexTrottoirLinks; } set { zustandsindexTrottoirLinks = value; Notify(() => ZustandsindexTrottoirLinks); } }

        private ZustandsindexTyp zustandsindexTrottoirRechts;
        public ZustandsindexTyp ZustandsindexTrottoirRechts { get { return zustandsindexTrottoirRechts; } set { zustandsindexTrottoirRechts = value; Notify(() => ZustandsindexTrottoirRechts); } }

        private DringlichkeitTyp dringlichkeitTrottoirLinks;
        public DringlichkeitTyp DringlichkeitTrottoirLinks { get { return dringlichkeitTrottoirLinks; } set { dringlichkeitTrottoirLinks = value; Notify(() => DringlichkeitTrottoirLinks); } }

        private DringlichkeitTyp dringlichkeitTrottoirRechts;
        public DringlichkeitTyp DringlichkeitTrottoirRechts { get { return dringlichkeitTrottoirRechts; } set { dringlichkeitTrottoirRechts = value; Notify(() => DringlichkeitTrottoirRechts); } }

        private Guid? massnahmenvorschlagTrottoirLinks;
        public Guid? MassnahmenvorschlagTrottoirLinks { get { return massnahmenvorschlagTrottoirLinks; } set { massnahmenvorschlagTrottoirLinks = value; UpdateKostenLinks(); Notify(() => MassnahmenvorschlagTrottoirLinks); } }

        private Guid? massnahmenvorschlagTrottoirRechts;
        public Guid? MassnahmenvorschlagTrottoirRechts { get { return massnahmenvorschlagTrottoirRechts; } set { massnahmenvorschlagTrottoirRechts = value; UpdateKostenRechts(); Notify(() => MassnahmenvorschlagTrottoirRechts); } }

        private void UpdateKostenLinks()
        {
            if (MassnahmenvorschlagTrottoirLinks.HasValue)
            {
                var massnahmenvorschlagKatalogDto = dtoService.GetDTOByID<MassnahmenvorschlagKatalogDTO>(MassnahmenvorschlagTrottoirLinks.Value);
                KostenTrottoirLinks = massnahmenvorschlagKatalogDto.DefaultKosten;

                GesamtkostenTrottoirLinks = Math.Round((zustandsabschnittGisdto.Laenge ?? 0) * (strassenabschnittGisdto.BreiteTrottoirLinks ?? 0) * (KostenTrottoirLinks ?? 0), 2);
            }
            else
            {
                KostenTrottoirLinks = null;
                GesamtkostenTrottoirLinks = null;
            }
        }

        private void UpdateKostenRechts()
        {
            if (MassnahmenvorschlagTrottoirRechts.HasValue)
            {
                var massnahmenvorschlagKatalogDto = dtoService.GetDTOByID<MassnahmenvorschlagKatalogDTO>(MassnahmenvorschlagTrottoirRechts.Value);
                KostenTrottoirRechts = massnahmenvorschlagKatalogDto.DefaultKosten;

                GesamtkostenTrottoirRechts = Math.Round((zustandsabschnittGisdto.Laenge ?? 0) * (strassenabschnittGisdto.BreiteTrottoirRechts ?? 0) * (KostenTrottoirRechts ?? 0), 2);
            }
            else
            {
                KostenTrottoirRechts = null;
                GesamtkostenTrottoirRechts = null;
            }
        }

        private decimal? kostenTrottoirLinks;
        public decimal? KostenTrottoirLinks { get { return kostenTrottoirLinks; } set { kostenTrottoirLinks = value; Notify(() => KostenTrottoirLinks); } }

        private decimal? kostenTrottoirRechts;
        public decimal? KostenTrottoirRechts { get { return kostenTrottoirRechts; } set { kostenTrottoirRechts = value; Notify(() => KostenTrottoirRechts); } }

        private decimal? gesamtkostenTrottoirLinks;
        public decimal? GesamtkostenTrottoirLinks { get { return gesamtkostenTrottoirLinks; } set { gesamtkostenTrottoirLinks = value; Notify(() => GesamtkostenTrottoirLinks); } }

        private decimal? gesamtkostenTrottoirRechts;
        public decimal? GesamtkostenTrottoirRechts { get { return gesamtkostenTrottoirRechts; } set { gesamtkostenTrottoirRechts = value; Notify(() => GesamtkostenTrottoirRechts); } }

        private List<NameValueItemViewModel<Guid?>> massnahmenvorschlagList;
        public List<NameValueItemViewModel<Guid?>> MassnahmenvorschlagList { get { return massnahmenvorschlagList; } set { massnahmenvorschlagList = value; Notify(() => MassnahmenvorschlagList); } }

        public List<NameValueItemViewModel<ZustandsindexTyp>> ZustandsindexTrottoirList
        {
            get
            {
                return new List<NameValueItemViewModel<ZustandsindexTyp>>
                           {
                               new NameValueItemViewModel<ZustandsindexTyp>(MobileLocalization.ZustandsindexTrottoirTypUnbekannt, ZustandsindexTyp.Unbekannt),
                               new NameValueItemViewModel<ZustandsindexTyp>(MobileLocalization.ZustandsindexTrottoirTypGut, ZustandsindexTyp.Gut),
                               new NameValueItemViewModel<ZustandsindexTyp>(MobileLocalization.ZustandsindexTrottoirTypMittel, ZustandsindexTyp.Mittel),
                               new NameValueItemViewModel<ZustandsindexTyp>(MobileLocalization.ZustandsindexTrottoirTypAusreichend, ZustandsindexTyp.Ausreichend),
                               new NameValueItemViewModel<ZustandsindexTyp>(MobileLocalization.ZustandsindexTrottoirTypKritisch, ZustandsindexTyp.Kritisch),
                               new NameValueItemViewModel<ZustandsindexTyp>(MobileLocalization.ZustandsindexTrottoirTypSchlecht, ZustandsindexTyp.Schlecht),
                           };
            }
        }

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
    }
}
