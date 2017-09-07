using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Validators.GIS;
using ASTRA.EMSG.Common.DataTransferObjects;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.EntityServices;
using ASTRA.EMSG.Business.Services.GIS;

namespace ASTRA.EMSG.Business.Services.DTOServices
{
    public interface IZustandsabschnittGISDTOService : IDTOServiceBase<ZustandsabschnittGIS, ZustandsabschnittGISDTO>
    {
        IList<string> Validate(ZustandsabschnittGISDTO zustandsabschnittGISDTO, IList<Guid> deletedZustandsabschnitte, IList<ZustandsabschnittGISDTO> zustandsabschnitteToValidate);
        ZustandsabschnittGIS GetZustandsabschnittById(Guid id);
        
    }
    class ZustandsabschnittGISDTOService : DTOServiceBase<ZustandsabschnittGIS, ZustandsabschnittGISDTO>, IZustandsabschnittGISDTOService
    {
        private readonly IReferenzGruppeDTOService referenzGruppeDTOService;
        private readonly IMassnahmenvorschlagKatalogDTOService massnahmenvorschlagKatalogDTOService;
        private readonly IAchsenReferenzService achsenReferenzService;
        private readonly IGISService gisService;
        private readonly ILocalizationService localizationService;
        private readonly IZustandsabschnittGISService zustandsabschnittGISService;
        private readonly IStrassenabschnittGISService strassenabschnittGISService;
        private readonly IMassnahmenvorschlagKatalogService massnahmenvorschlagKatalogService;
        private readonly IDataTransferObjectServiceMappingEngine dataTransferObjectServiceMappingEngine;
       

        public ZustandsabschnittGISDTOService(ITransactionScopeProvider transactionScopeProvider, 
            IDataTransferObjectServiceMappingEngine dataTransferObjectServiceMappingEngine,
            IReferenzGruppeDTOService referenzGruppeDTOService, 
            IMassnahmenvorschlagKatalogDTOService massnahmenvorschlagKatalogDTOService,
            IAchsenReferenzService achsenReferenzService,
            IGISService gisService,
            ILocalizationService localizationService,
            IZustandsabschnittGISService zustandsabschnittGISService,
            IMassnahmenvorschlagKatalogService massnahmenvorschlagKatalogService, 
            IStrassenabschnittGISService strassenabschnittGISService)
            : base(transactionScopeProvider, dataTransferObjectServiceMappingEngine)
        {
            this.dataTransferObjectServiceMappingEngine = dataTransferObjectServiceMappingEngine;
            this.massnahmenvorschlagKatalogService = massnahmenvorschlagKatalogService;
            this.strassenabschnittGISService = strassenabschnittGISService;
            this.zustandsabschnittGISService = zustandsabschnittGISService;
            this.localizationService = localizationService;
            this.gisService = gisService;
            this.achsenReferenzService = achsenReferenzService;
            this.massnahmenvorschlagKatalogDTOService = massnahmenvorschlagKatalogDTOService;
            this.referenzGruppeDTOService = referenzGruppeDTOService;
        }

        protected override ZustandsabschnittGISDTO CreateModel(ZustandsabschnittGIS entity)
        {
            var DTO = base.CreateModel(entity);
            DTO.ReferenzGruppeDTO = referenzGruppeDTOService.GetDTOByID(entity.ReferenzGruppe.Id);
            if (entity.MassnahmenvorschlagTrottoirLinks != null)
            {
                DTO.MassnahmenvorschlagLinks = new MassnahmenvorschlagDTO();
                DTO.MassnahmenvorschlagLinks.Dringlichkeit = entity.DringlichkeitTrottoirLinks;
                DTO.MassnahmenvorschlagLinks.Kosten = entity.KostenMassnahmenvorschlagTrottoirLinks;
                DTO.MassnahmenvorschlagLinks.Typ = entity.MassnahmenvorschlagTrottoirLinks != null ? entity.MassnahmenvorschlagTrottoirLinks.Id : (Guid?)null;
            } 
            if (entity.MassnahmenvorschlagTrottoirRechts != null)
            {
                DTO.MassnahmenvorschlagRechts = new MassnahmenvorschlagDTO();
                DTO.MassnahmenvorschlagRechts.Dringlichkeit = entity.DringlichkeitTrottoirRechts;
                DTO.MassnahmenvorschlagRechts.Kosten = entity.KostenMassnahmenvorschlagTrottoirRechts;
                DTO.MassnahmenvorschlagRechts.Typ = entity.MassnahmenvorschlagTrottoirRechts != null ? entity.MassnahmenvorschlagTrottoirRechts.Id : (Guid?)null;
            }
            if (entity.MassnahmenvorschlagFahrbahn != null)
            {
                DTO.MassnahmenvorschlagFahrbahnDTO = new MassnahmenvorschlagDTO();
                DTO.MassnahmenvorschlagFahrbahnDTO.Dringlichkeit = entity.DringlichkeitFahrbahn;
                DTO.MassnahmenvorschlagFahrbahnDTO.Kosten = entity.KostenMassnahmenvorschlagFahrbahn;
                DTO.MassnahmenvorschlagFahrbahnDTO.Typ = entity.MassnahmenvorschlagFahrbahn != null ? entity.MassnahmenvorschlagFahrbahn.Id : (Guid?)null;
            }
            return DTO;
        }

        public IList<string> Validate(ZustandsabschnittGISDTO zustandsabschnittGISDTO, IList<Guid> exceptedZustandsabschnitte, IList<ZustandsabschnittGISDTO> zustandsabschnitteToValidate)
        {
            IList<string> errorlist = new List<string>();
            if (!CheckOverlap(zustandsabschnittGISDTO, exceptedZustandsabschnitte, zustandsabschnitteToValidate))
            {
                errorlist.Add(localizationService.GetLocalizedError(ValidationError.GeometryOverlaps));
            }
            if (!IsZustandsabschnittWithinStrassenabschnitt(zustandsabschnittGISDTO))
            {
                errorlist.Add(localizationService.GetLocalizedError(ValidationError.ZustandAbschnittNotWithin));
            }
            ZustandsabschnittGISDTOValidator validator = new ZustandsabschnittGISDTOValidator(localizationService, zustandsabschnittGISService, strassenabschnittGISService);
            var result = validator.Validate(zustandsabschnittGISDTO);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    errorlist.Add(error.ErrorMessage);
                }
            }
            return errorlist;
        }
        protected override void UpdateEntityFieldsInternal(ZustandsabschnittGISDTO model, ZustandsabschnittGIS entity)
        {
            
            if (model.MassnahmenvorschlagLinks != null)
            {
                entity.DringlichkeitTrottoirLinks = model.MassnahmenvorschlagLinks.Dringlichkeit;
                entity.KostenMassnahmenvorschlagTrottoirLinks = model.MassnahmenvorschlagLinks.Kosten;
                entity.MassnahmenvorschlagTrottoirLinks = massnahmenvorschlagKatalogService.GetCurrentEntities().Where(mvk => mvk.Id == model.MassnahmenvorschlagLinks.Typ).SingleOrDefault();
            }
            if (model.MassnahmenvorschlagRechts != null)
            {
                entity.DringlichkeitTrottoirRechts = model.MassnahmenvorschlagRechts.Dringlichkeit;
                entity.KostenMassnahmenvorschlagTrottoirRechts = model.MassnahmenvorschlagRechts.Kosten;
                entity.MassnahmenvorschlagTrottoirRechts = massnahmenvorschlagKatalogService.GetCurrentEntities().Where(mvk => mvk.Id == model.MassnahmenvorschlagRechts.Typ).SingleOrDefault();
            }
            if (model.MassnahmenvorschlagFahrbahnDTO != null)
            {
                entity.DringlichkeitFahrbahn = model.MassnahmenvorschlagFahrbahnDTO.Dringlichkeit;
                entity.KostenMassnahmenvorschlagFahrbahn = model.MassnahmenvorschlagFahrbahnDTO.Kosten;
                entity.MassnahmenvorschlagFahrbahn = massnahmenvorschlagKatalogService.GetCurrentEntities().Where(mvk => mvk.Id == model.MassnahmenvorschlagFahrbahnDTO.Typ).SingleOrDefault();
            }
            //If it is an Update (ID is not empty) -> Delete the References first (Referenzgruppe, Achsenreferenzen)
            if (model.Id != Guid.Empty)
            {
                DeleteReferenzen(model.Id);
            }
            model.ReferenzGruppeDTO = referenzGruppeDTOService.CreateEntity(model.ReferenzGruppeDTO);
            model.ReferenzGruppe = model.ReferenzGruppeDTO.Id;

            if (model.Abschnittsnummer < 0)
                model.Abschnittsnummer = entity.Abschnittsnummer;

            base.UpdateEntityFieldsInternal(model, entity);
        }
        private void DeleteReferenzen(Guid zustandsabschnittID)
        {
            ZustandsabschnittGIS zustandsabschnitt = GetEntityById(zustandsabschnittID);

            if (zustandsabschnitt != null && zustandsabschnitt.ReferenzGruppe.Id != Guid.Empty)
            {
                List<AchsenReferenz> achsenreferenzen = achsenReferenzService.GetAchsenReferenzGruppe(zustandsabschnitt.ReferenzGruppe.Id);

                foreach (AchsenReferenz achsenreferenz in achsenreferenzen)
                {
                    achsenReferenzService.DeleteEntity(achsenreferenz.Id);
                }
            }

            if (zustandsabschnitt != null && zustandsabschnitt.ReferenzGruppe.Id != Guid.Empty)
            {
                referenzGruppeDTOService.DeleteEntity(zustandsabschnitt.ReferenzGruppe.Id);
            }
        }
        private bool CheckOverlap(ZustandsabschnittGISDTO zustandsabschnittGISDTO, IList<Guid> deletedZustandsabschnitte, IList<ZustandsabschnittGISDTO> zustandsabschnitteToValidate)
        {
           
            //check for overlaps with other strabs on the same achssegment
            IList<AchsenReferenzDTO> neue_achsenreferenzen = zustandsabschnittGISDTO.ReferenzGruppeDTO.AchsenReferenzenDTO;
            //var test2 = GeoJSONReader.ReadFeatureWithID(new StringReader(geojsonstring)).Attributes;
            //Guid strabsid = zustandsabschnittGISDTO.StrassenabschnittGIS;
            //StrassenabschnittGIS strab = GetEntityById<StrassenabschnittGIS>(zustandsabschnittGISDTO.StrassenabschnittGIS);
            var other_Zustandsabschnitte = zustandsabschnitteToValidate.Where(za => za.Id != zustandsabschnittGISDTO.Id);
            var other_ZustandsabschnitteIDs = other_Zustandsabschnitte.Select(za => za.Id);
            var other_AchsenreferenzenDTOs = other_Zustandsabschnitte.Select(za => za.ReferenzGruppeDTO.AchsenReferenzenDTO).SelectMany(ar=>ar);
            var other_Achsenreferenzen = new List<AchsenReferenz>();
            foreach (var dto in other_AchsenreferenzenDTOs)
            {
                other_Achsenreferenzen.Add(dataTransferObjectServiceMappingEngine.Translate<AchsenReferenzDTO, AchsenReferenz>(dto));
            }
            foreach (var neue_achsenreferenz in neue_achsenreferenzen)
            {

                Guid achsensegmentID = neue_achsenreferenz.AchsenSegment;


                var alte_achsenreferenzen = achsenReferenzService.GetCurrentEntities()
                .Where(r => r.AchsenSegment.Id == achsensegmentID).ToList();

                alte_achsenreferenzen = alte_achsenreferenzen.Where(r => r.ReferenzGruppe.ZustandsabschnittGIS != null

                    && r.ReferenzGruppe.ZustandsabschnittGIS.ErfassungsPeriod.IsClosed == false
                    //prevent selfcheck
                    && r.ReferenzGruppe.ZustandsabschnittGIS.Id != zustandsabschnittGISDTO.Id
                    //prevent check with zustandsabschnitte that are going to be deleted
                    && !deletedZustandsabschnitte.Contains(r.ReferenzGruppe.ZustandsabschnittGIS.Id)
                    //dont check the old geometries (they are replaced by the Geometries in zustandsabschnittetovalidate)
                    && !other_ZustandsabschnitteIDs.Contains(r.ReferenzGruppe.ZustandsabschnittGIS.Id)
                    ).ToList();
                var allOther_achsenreferenzen = alte_achsenreferenzen.Concat(other_Achsenreferenzen.Where(ar => ar.AchsenSegment.Id == neue_achsenreferenz.AchsenSegment));
                if (!gisService.CheckOverlapp(allOther_achsenreferenzen.ToList(), neue_achsenreferenz.Shape))
                {
                    return false;
                }
            }
            return true;


        }
        public  ZustandsabschnittGIS GetZustandsabschnittById(Guid id)
        {
            return base.GetEntityById(id);
        }
        private bool IsZustandsabschnittWithinStrassenabschnitt(ZustandsabschnittGISDTO zustandsabschnittGISDTO)
        {
            //string geojsonstring = zustandsabschnittGISModel.FeatureGeoJSONString;
            Guid strabsid = zustandsabschnittGISDTO.StrassenabschnittGIS;
            StrassenabschnittGIS strab = GetEntityById<StrassenabschnittGIS>(strabsid);
            IList<AchsenReferenzDTO> neue_achsenreferenzen = zustandsabschnittGISDTO.ReferenzGruppeDTO.AchsenReferenzenDTO;
            return gisService.CheckGeometriesIsInControlGeometry(neue_achsenreferenzen, strab.Shape);
        }
    }
}
