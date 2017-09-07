using System;
using System.Linq;
using System.Windows.Input;
using ASTRA.EMSG.Common.DataTransferObjects;
using ASTRA.EMSG.Common.DataTransferObjects.EventArgs;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Services.SchadenMetadaten;

using ASTRA.EMSG.Mobile.Services;
using ASTRA.EMSG.Mobile.Utils;
using ASTRA.EMSG.Map.Services;
using GeoJSON;
using System.IO;
using ASTRA.EMSG.Common.Utils;

namespace ASTRA.EMSG.Mobile.ViewModels
{
    public interface IFormViewModel : IViewModel
    {
    }

    public class FormViewModel : ViewModel, IFormViewModel
    {
        private readonly IWindowService windowService;
        private readonly IFormService formService;
        private readonly IDTOService dtoService;
        private readonly ISchadenMetadatenService schadenMetadatenService;
        private readonly IMessageBoxService messageBoxService;
        private readonly IGeoJsonService geoJsonService;

        public FormViewModel(
            IWindowService windowService, 
            IMapService mapService, 
            IDTOService dtoService, 
            ISchadenMetadatenService schadenMetadatenService, 
            IFormService formService, 
            IMessageBoxService messageBoxService,
            IGeoJsonService geoJsonService)
        {
            this.windowService = windowService;
            this.dtoService = dtoService;
            this.schadenMetadatenService = schadenMetadatenService;
            this.formService = formService;
            this.messageBoxService = messageBoxService;
            this.geoJsonService = geoJsonService;

            mapService.ZustandsabschnittSelected += MapServiceOnZustandsabschnittSelected;
            mapService.ZustandsabschnittChanged += MapServiceOnZustandsabschnittChanged;
            mapService.ZustandsabschnittCreated += MapServiceOnZustandsabschnittCreated;
            mapService.ZustandsabschnittCancelled += MapServiceOnZustandsabschnittCancelled;
            mapService.StrassenabschnittSelected += MapServiceOnStrassenabschnittSelected;
            mapService.ZustandsabschnittDeleted += MapServiceOnZustandsabschnittDeleted;
            IsVisible = false;

            formService.GettingFormHasChanges += (sender, args) =>
                                                     {
                                                         args.HasFormChanges = ZustandsabschnittViewModel != null && ZustandsabschnittViewModel.HasChanges;
                                                     };
        }

        private void MapServiceOnZustandsabschnittSelected(object sender, SelectZustandsabschnittDataTransferEventArgs dataTransferEventArgs)
        {
            var zustandsabschnittGisdto = dtoService.GetDTOByID<ZustandsabschnittGISDTO>(dataTransferEventArgs.Id);
            var strassenabschnittGisdto = dtoService.Get<StrassenabschnittGISDTO>().Single(s => s.Id == zustandsabschnittGisdto.StrassenabschnittGIS);

            ZustandsabschnittViewModel = new ZustandsabschnittViewModel(zustandsabschnittGisdto, strassenabschnittGisdto, dtoService, windowService, schadenMetadatenService, formService, messageBoxService, geoJsonService);
            IsVisible = true;
        }

        private void MapServiceOnZustandsabschnittChanged(object sender, EditZustandsabschnittDataTransferEventArgs dataTransferEventArgs)
        {
            ZustandsabschnittViewModel.ZustandsabschnittDetailsViewModel.Laenge = dataTransferEventArgs.Length;
            this.ApplyGeojson(dataTransferEventArgs.GeoJson);
        }

        private void MapServiceOnZustandsabschnittCreated(object sender, CreateZustandsabschnittDataTransferEventArgs dataTransferEventArgs)
        {
            var zustandsabschnittGisdto = new ZustandsabschnittGISDTO
                                              {
                                                  Id = dataTransferEventArgs.Id,
                                                  StrassenabschnittGIS = dataTransferEventArgs.StrassenabschnittId,
                                                  Erfassungsmodus = ZustandsErfassungsmodus.Manuel,
                                                  Aufnahmedatum = DateTime.Now
                                              };
            //should only be saved if Save or Apply is pressed
            //dtoService.CreateOrReplaceDTO(zustandsabschnittGisdto);

            var strassenabschnittGisdto = dtoService.Get<StrassenabschnittGISDTO>().Single(s => s.Id == zustandsabschnittGisdto.StrassenabschnittGIS);

            ZustandsabschnittViewModel = new ZustandsabschnittViewModel(zustandsabschnittGisdto, strassenabschnittGisdto, dtoService, windowService, schadenMetadatenService, formService, messageBoxService, geoJsonService, true);
            IsVisible = true;
        }

        private void MapServiceOnStrassenabschnittSelected(object sender, SelectStrassenabschnittDataTransferEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MapServiceOnZustandsabschnittCancelled(object sender, EventArgs e)
        {
            IsVisible = false;
            if (zustandsabschnittViewModel != null)
            {
                zustandsabschnittViewModel.HasChanges = false;
            }            
        }

        private void MapServiceOnZustandsabschnittDeleted(object sender, DeleteZustandsabschnittDataTransferEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void ApplyGeojson(string geoJson)
        {
            if (string.IsNullOrEmpty(geoJson))
            {
                zustandsabschnittViewModel.ReferenzGruppe = null;
                zustandsabschnittViewModel.Shape = null;
            }
            else
            {
                zustandsabschnittViewModel.ReferenzGruppe = geoJsonService.GenerateReferenzGruppeFromGeoJson(geoJson);
                zustandsabschnittViewModel.Shape = GeoJSONReader.ReadFeatureWithID(new StringReader(geoJson)).Geometry;
                zustandsabschnittViewModel.Shape.SRID = GisConstants.SRID;
                if (zustandsabschnittViewModel.Shape.IsEmpty)
                {
                    zustandsabschnittViewModel.Shape = null;
                }
            }

        }

        private ZustandsabschnittViewModel zustandsabschnittViewModel;
        public ZustandsabschnittViewModel ZustandsabschnittViewModel
        {
            get { return zustandsabschnittViewModel; }
            set
            {
                UnSubscribe();
                zustandsabschnittViewModel = value;
                Subscribe();

                Notify(() => ZustandsabschnittViewModel);
            }
        }

        private void Subscribe()
        {
            if (ZustandsabschnittViewModel == null)
                return;

            ZustandsabschnittViewModel.Saved += ZustandsabschnittViewModelOnSaved;
            ZustandsabschnittViewModel.Cancelled += ZustandsabschnittViewModelOnCancelled;
            ZustandsabschnittViewModel.Deleted += ZustandsabschnittViewModelOnDeleted;
        }

        private void UnSubscribe()
        {
            if (ZustandsabschnittViewModel == null)
                return;

            ZustandsabschnittViewModel.Saved -= ZustandsabschnittViewModelOnSaved;
            ZustandsabschnittViewModel.Cancelled -= ZustandsabschnittViewModelOnCancelled;
            ZustandsabschnittViewModel.Deleted -= ZustandsabschnittViewModelOnDeleted;
        }

        private void ZustandsabschnittViewModelOnDeleted(object sender, EventArgs eventArgs) { IsVisible = false; }
        private void ZustandsabschnittViewModelOnCancelled(object sender, EventArgs eventArgs) { IsVisible = false; }
        private void ZustandsabschnittViewModelOnSaved(object sender, SaveEventArgs eventArgs) { if (eventArgs.CloseWindow) IsVisible = false; }

        public ICommand OpenHelpWindowCommand { get; set; }
    }
}
