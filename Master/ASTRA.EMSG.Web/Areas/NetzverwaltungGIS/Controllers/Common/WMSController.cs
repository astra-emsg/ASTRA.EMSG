using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Configuration;
using System.IO;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Business.Services.EntityServices;
using ASTRA.EMSG.Common.Master.Logging;
using ASTRA.EMSG.Business.Services.GIS;
using ASTRA.EMSG.Business.Services.GIS.WMS;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;
using System.Runtime;
using System.Collections;
using ASTRA.EMSG.Common.Master.GeoJSON;
using System.Runtime.Serialization.Json;
using ASTRA.EMSG.Common.Utils;
using System.Text;
using ASTRA.EMSG.Common.Master.HttpRequest;
using ASTRA.EMSG.Business.Services.GIS.WMS.WMSObjects;
using ASTRA.EMSG.Web.Infrastructure.Security;


namespace ASTRA.EMSG.Web.Areas.NetzverwaltungGIS.Controllers.Common
{
    //set sessionstate to readonly for making parallel wms requests possible
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class WMSController : Controller
    {
        private readonly ISecurityService securityService;
        private readonly IAchsenSegmentService achsenSegmentService;
        private readonly ICreateWMSRequest createWMSRequest;
        private readonly ILegendService legendService;
        private readonly IServerConfigurationProvider serverConfigurationProvider;

        public WMSController(ISecurityService securityService, 
            IAchsenSegmentService achsenSegmentService, 
            ICreateWMSRequest createWMSRequest, 
            ILegendService legendService,
            IServerConfigurationProvider serverConfigurationProvider)
        {           
            this.securityService = securityService;
            this.achsenSegmentService = achsenSegmentService;
            this.createWMSRequest = createWMSRequest;
            this.legendService = legendService;
            this.serverConfigurationProvider = serverConfigurationProvider;
        }

        // get the legend using rest api and /Legend service
        [HttpGet]
        public ActionResult Legend(string layer)
        {
            Stream legendStream = createWMSRequest.LegendEMSGRequest(layer);

            return base.File(legendStream, "image/png");
        }
       
        // get an image using rest api and /export service
        [HttpGet]
        public ActionResult Export(string f,
              string layers,
              string transparent,
              string format,
              string bbox,
              string size,
              string bboxsr,
              string imagesr,
              string layerdefs)
        {
            WMSRestParameter wmsParameter = new WMSRestParameter(layers, bbox, size, transparent, format, bboxsr, GisConstants.SRS, layerdefs, f);
       
            return RequestHandler(createWMSRequest.WMSRequest(WMSRequestType.OverlayEMSGLayer, wmsParameter));
        }

        [HttpGet]
        public ActionResult GetMap(
          string LAYERS,
          string BBOX,
          string FORMAT = "image/png",
          string CRS = "EPSG:21781",
          string WIDTH = "256",
          string HEIGHT = "256",
          string REQUEST = "GetMap",
          string VERSION = "1.3.0",
          string SERVICE = "WMS")
        {
            WMSParameter wmsParameter = new WMSParameter(LAYERS: LAYERS, BBOX: BBOX, FORMAT: FORMAT, CRS: CRS, WIDTH: WIDTH, HEIGHT: HEIGHT, REQUEST: REQUEST, VERSION: VERSION, SERVICE: SERVICE);

            return RequestHandler(createWMSRequest.WMSRequest(WMSRequestType.BackgroundLayer, wmsParameter));
        }

        [HttpGet]
        [Obsolete("For WMTS - proof-of-concept", false)]
        public ActionResult GetWmts(String query)
        {
            #pragma warning disable 612, 618 
            return RequestHandler(serverConfigurationProvider.WMTSUrlBaseLayer +"/"+ query,
                "",
                "http://astra.admin.ch",
                userName:serverConfigurationProvider.WMTSUserName,
                password:serverConfigurationProvider.WMTSPassword
                );
            #pragma warning restore 612, 618 
        }

       

        [Obsolete("For WMTS - proof-of-concept", false)]
        private ActionResult RequestHandler(string requestURI, string contentType = "", string referer = "", string userName = "", string password = "")
        {
            HttpRequestObject requestObject = new HttpRequestObject();
            requestObject.requestURI = requestURI;
            requestObject.contentType = contentType;
            requestObject.referer = referer;
            requestObject.userName = userName;
            requestObject.password = password;

            HttpResponseObject responseObject = SendHttpRequest.WmsRequest(requestObject);

            return new FileStreamResult(responseObject.responseStream, responseObject.contentType);
        }
       

        [HttpGet]
        public ActionResult GetAV(string LAYERS,
          string BBOX,
          string FORMAT = "image/png",
          string SRS = "EPSG:21781",
          string WIDTH = "256",
          string HEIGHT = "256",
          string REQUEST = "GetMap",
          string VERSION = "1.3.0",
          string SERVICE = "WMS")
        {
            WMSParameter wmsParameter = new WMSParameter(LAYERS: LAYERS, BBOX: BBOX, FORMAT: FORMAT, SRS: SRS, WIDTH: WIDTH, HEIGHT:HEIGHT, REQUEST:REQUEST, VERSION:VERSION, SERVICE:SERVICE);
            return RequestHandler(createWMSRequest.WMSRequest(WMSRequestType.AVLayer, wmsParameter));
        }

        [HttpGet]
        public ActionResult GetAVC(string LAYERS,
          string BBOX,
          string FORMAT = "image/png",
          string SRS = "EPSG:21781",
          string WIDTH = "256",
          string HEIGHT = "256",
          string REQUEST = "GetMap",
          string VERSION = "1.3.0",
          string SERVICE = "WMS")
        {
            WMSParameter wmsParameter = new WMSParameter(LAYERS: LAYERS, BBOX: BBOX, FORMAT: FORMAT, SRS: SRS, WIDTH: WIDTH, HEIGHT: HEIGHT, REQUEST: REQUEST, VERSION: VERSION, SERVICE: SERVICE);
            return RequestHandler(createWMSRequest.WMSRequest(WMSRequestType.AVCLayer, wmsParameter));
        }

        private ActionResult RequestHandler(HttpResponseObject response)
        {
            if (response == null || string.IsNullOrEmpty(response.contentType))
                return Content("wms response is emtpy");
            return new FileStreamResult(response.responseStream, response.contentType);
        }


        [HttpGet]
        public ActionResult GetMandantAxisEnvelope()
        {
            return Content("[" + achsenSegmentService.GetMandantAxisEnvelope() + "]", "application/json");
        }

        //Dummy Method for Development
        [HttpGet]
        public ActionResult GetCapabilities()
        {
            var request = (HttpWebRequest)WebRequest.Create("http://wmts.geo.admin.ch/1.0.0/WMTSCapabilities.xml");
            request.Referer = "http://astra.admin.ch";
            WebResponse resp = request.GetResponse();
            return new FileStreamResult(resp.GetResponseStream(), resp.ContentType);
        }
    }
}
