using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common;
using System.IO;
using System.Runtime.Serialization;
using ASTRA.EMSG.Common.Utils;
using ASTRA.EMSG.Business.Models.GIS;
using GeoAPI.Geometries;
using ASTRA.EMSG.Common.DataTransferObjects;
using NetTopologySuite.Features;
using ASTRA.EMSG.Business.Entities.Common;

using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Business.Services.DTOServices;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;
using System.Configuration;
using System.Globalization;
using System.Xml.Serialization;
using ASTRA.EMSG.Common.EMSGBruTile;
using BruTile.Wmsc;
using BruTile;
using ASTRA.EMSG.Common.Master.Logging;
using System.Diagnostics;

namespace ASTRA.EMSG.Business.Services.GIS
{
    public interface ICheckOutService : IService
    {
        CheckOutGISStreams CheckOutData(IList<Guid> ids, bool exportBackground);
        CheckOutGISStreams CheckOutData(Guid id, bool exportBackground);
    }
    public class CheckOutService : ICheckOutService
    {
       
        private readonly IInspektionsRouteGISService inspektionsRouteGISService;
        private readonly IInspektionsRouteStatusverlaufService inspektionsRouteStatusverlaufService;
        private readonly IInspektionsRtStrAbschnitteService inspektionsRtStrAbschnitteService;
        private readonly IStrassenabschnittGISDTOService strassenabschnittGISDTOService;
        private readonly IMassnahmenvorschlagKatalogService massnahmenvorschlagKatalogService;
        private readonly IMassnahmenvorschlagKatalogDTOService massnahmenvorschlagKatalogDTOService;
        private readonly IAchsenReferenzService achsenReferenzService;
        private readonly IZustandsabschnittGISDTOService zustandsabschnittGISDTOService;
        private readonly ISchadendetailDTOService schadendetailDTOService;
        private readonly ISchadengruppeDTOService schadengruppeDTOService;
        private readonly IAchsenSegmentService achsenSegmentService;
        private readonly IAchsenSegmentDTOService achsenSegmentDTOService;
        private readonly IBelastungskategorieService belastungskategorieService;
        private readonly IBelastungskategorieDTOService belastungskategorieDTOService;
        private readonly IServerConfigurationProvider serverConfigurationProvider;
        private readonly ILegendService legendService;

        public CheckOutService
            (
            IInspektionsRouteGISService inspektionsRouteGISService,
            IInspektionsRouteStatusverlaufService inspektionsRouteStatusverlaufService,
            IInspektionsRtStrAbschnitteService inspektionsRtStrAbschnitteService,
            IStrassenabschnittGISDTOService strassenabschnittGISDTOService,
            IMassnahmenvorschlagKatalogService massnahmenvorschlagKatalogService,
            IMassnahmenvorschlagKatalogDTOService massnahmenvorschlagKatalogDTOService,
            IAchsenReferenzService achsenReferenzService,
            IZustandsabschnittGISDTOService zustandsabschnittGISDTOService,
            ISchadendetailDTOService schadendetailDTOService,
            ISchadengruppeDTOService schadengruppeDTOService,
            IAchsenSegmentService achsenSegmentService,
            IAchsenSegmentDTOService achsenSegmentDTOService,
            IBelastungskategorieService belastungskategorieService,
            IBelastungskategorieDTOService belastungskategorieDTOService,
            IServerConfigurationProvider serverConfigurationProvider,
            ILegendService legendService
            )
        {
            this.inspektionsRouteGISService = inspektionsRouteGISService;
            this.inspektionsRouteStatusverlaufService = inspektionsRouteStatusverlaufService;
            this.inspektionsRtStrAbschnitteService = inspektionsRtStrAbschnitteService;
            this.strassenabschnittGISDTOService = strassenabschnittGISDTOService;
            this.massnahmenvorschlagKatalogService = massnahmenvorschlagKatalogService;
            this.massnahmenvorschlagKatalogDTOService = massnahmenvorschlagKatalogDTOService;
            this.achsenReferenzService = achsenReferenzService;
            this.zustandsabschnittGISDTOService = zustandsabschnittGISDTOService;
            this.schadendetailDTOService = schadendetailDTOService;
            this.schadengruppeDTOService = schadengruppeDTOService;
            this.achsenSegmentService = achsenSegmentService;
            this.achsenSegmentDTOService = achsenSegmentDTOService;
            this.belastungskategorieService = belastungskategorieService;
            this.belastungskategorieDTOService = belastungskategorieDTOService;
            this.serverConfigurationProvider = serverConfigurationProvider;
            this.legendService = legendService;
        }

        public CheckOutGISStreams CheckOutData(IList<Guid> ids, bool exportBackground)
        {
            CheckOutGISStreams checkoutGISStreams = new CheckOutGISStreams();


            DTOContainer dtosToExport = new DTOContainer();
            Mandant mandant = null;
            IGeometry bbox = null;
            foreach (Guid id in ids)
            {
                InspektionsRouteGIS inspektionsroute = inspektionsRouteGISService.GetInspektionsRouteById(id);
                mandant = inspektionsroute.Mandant;
                if (bbox != null)
                {
                    bbox = bbox.Union(inspektionsroute.Shape.Envelope).Envelope;
                }
                else
                {
                    bbox = inspektionsroute.Shape.Envelope;
                }
                IList<InspektionsRtStrAbschnitte> inspektionsroutenAbschnitte = inspektionsRtStrAbschnitteService.GetCurrentEntities().Where(ira => ira.InspektionsRouteGIS.Id == inspektionsroute.Id).ToList();

                inspektionsroutenAbschnitte.OrderBy(ira => ira.Reihenfolge);

                //Strassenabschnitte
                foreach (InspektionsRtStrAbschnitte inspektionsroutenAbschnitt in inspektionsroutenAbschnitte)
                {

                    StrassenabschnittGISDTO strassenabschnittGISDTO = strassenabschnittGISDTOService.GetDTOByID(inspektionsroutenAbschnitt.StrassenabschnittGIS.Id);
                    dtosToExport.DataTransferObjects.Add(strassenabschnittGISDTO);

                    //Zustandsabschnitte
                    foreach (ZustandsabschnittGIS zustandsabschnitt in inspektionsroutenAbschnitt.StrassenabschnittGIS.Zustandsabschnitten)
                    {

                        ZustandsabschnittGISDTO zustandsabschnittGISDTO = zustandsabschnittGISDTOService.GetDTOByID(zustandsabschnitt.Id);
                        dtosToExport.DataTransferObjects.Add(zustandsabschnittGISDTO);


                        foreach (Schadendetail schaden in zustandsabschnitt.Schadendetails)
                        {
                            SchadendetailDTO schadendto = schadendetailDTOService.GetDTOByID(schaden.Id);
                            schadendto.ZustandsabschnittId = zustandsabschnitt.Id;
                            dtosToExport.DataTransferObjects.Add(schadendto);
                        }
                        foreach (Schadengruppe schadengruppe in zustandsabschnitt.Schadengruppen)
                        {
                            SchadengruppeDTO schadengruppedto = schadengruppeDTOService.GetDTOByID(schadengruppe.Id);
                            schadengruppedto.ZustandsabschnittId = zustandsabschnitt.Id;
                            dtosToExport.DataTransferObjects.Add(schadengruppedto);
                        }
                    }
                }
            }
            IGeometry backgroundBbox = null;

            //Achsensegmente
            foreach (AchsenSegment achsensegment in achsenSegmentService.GetCurrentBySpatialFilter(bbox).Where(a => a.Mandant == mandant))
            {
                dtosToExport.DataTransferObjects.Add(achsenSegmentDTOService.GetDTOByID(achsensegment.Id));
                if (backgroundBbox != null)
                {
                    backgroundBbox = backgroundBbox.Envelope.Union(achsensegment.Shape.Envelope);
                }
                else
                {
                    backgroundBbox = achsensegment.Shape.Envelope;
                }
            }

            List<Belastungskategorie> belastungskategorien = belastungskategorieService.AlleBelastungskategorie;
            foreach (Belastungskategorie bk in belastungskategorien)
            {
                dtosToExport.DataTransferObjects.Add(belastungskategorieDTOService.GetDTOByID(bk.Id));
            }
            List<MassnahmenvorschlagKatalog> mvklist = massnahmenvorschlagKatalogService.GetCurrentEntities().Where(mvk => mvk.Mandant.Id == mandant.Id && mvk.ErfassungsPeriod.IsClosed == false).ToList();

            foreach (MassnahmenvorschlagKatalog mvk in mvklist)
            {
                dtosToExport.DataTransferObjects.Add(massnahmenvorschlagKatalogDTOService.GetDTOByID(mvk.Id));
            }

            IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            MemoryStream serializedModelsStream = new MemoryStream();
            formatter.Serialize(serializedModelsStream, dtosToExport);
            serializedModelsStream.Seek(0, 0);


            checkoutGISStreams.Bezeichnung = "export";

            LayerCollection exportLayer = serverConfigurationProvider.ExportLayer;
            int count = exportLayer.Count;
            string filepath = serverConfigurationProvider.WMSCacheFolderPath;
            for (int i = 0; i < count; i++)
            {

                ITileSourceFactory fact = null;
                var layer = exportLayer[i];
                switch (layer.ServiceType)
                {
                    case ServiceType.WMS:
                        fact = new WmsTileSourceFactory(layer);
                        break;
                    case ServiceType.WMTS:
                        fact = new WmtsTileSourceFactory(layer);
                        break;
                    default:
                        break;
                }
                TileLoader loader = new TileLoader(fact.GetTileSource());

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                checkoutGISStreams.Tiles.Add(loader.GetTiles(layer, bbox, filepath, serverConfigurationProvider.UseWMSCaching, fact.Format, serverConfigurationProvider.ExportBackgroundMapBuffer, serverConfigurationProvider.ExportTileLimit, download: exportBackground));
                stopwatch.Stop();
                Loggers.PeformanceLogger.DebugFormat("Layer {0} downloaded in {1}", layer.Name, stopwatch.Elapsed);

            }
            checkoutGISStreams.ModelsToExport = serializedModelsStream;
            
            checkoutGISStreams.LegendStreams.Add(FileNameConstants.AchsenSegmentLayerLegendFilename, legendService.GetLegendStream("AchsenSegmentLayer"));
            checkoutGISStreams.LegendStreams.Add(FileNameConstants.StrassenabschnittLayerLegendFilename, legendService.GetLegendStream("StrassenabschnittLayer"));
            checkoutGISStreams.LegendStreams.Add(FileNameConstants.ZustandsabschnittLayerLegendFilename, legendService.GetLegendStream("ZustandsabschnittLayer"));
            checkoutGISStreams.LegendStreams.Add(FileNameConstants.ZustandsabschnittLayerTrottoirLegendFilename, legendService.GetLegendStream("ZustandsabschnittLayer_Trottoir"));
            return checkoutGISStreams;
        }

        public CheckOutGISStreams CheckOutData(Guid id, bool exportBackground)
        {

            List<Guid> ids = new List<Guid> { id };
            return CheckOutData(ids, exportBackground);
        }
    }
}
