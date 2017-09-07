using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Business.Services.GIS.WMS.WMSObjects;
using ASTRA.EMSG.Business.Services.GIS.WMS;
using ASTRA.EMSG.Common.Master.HttpRequest;
using System.Runtime.Serialization.Json;
using ASTRA.EMSG.Common.Utils;
using System.IO;
using System.Drawing;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Common.Master.Logging;
using NLog;

namespace ASTRA.EMSG.Business.Services.GIS.WMS
{
    public interface ICreateWMSRequest : IService
    {
        HttpResponseObject WMSRequest(WMSRequestType wmsRequestType, WMSParamBase wms);
        Stream LegendEMSGRequest(string layer);
    }


    public class CreateWMSRequest:ICreateWMSRequest
    {
        private const int REPORT_IMAGE_RESOLUTION = 300;
        private const int WEB_IMAGE_RESOLUTION = 96;
        private readonly ISecurityService securityService;
        private readonly ILegendService legendService;

        public CreateWMSRequest(ISecurityService securityService, ILegendService legendService)
        {
            this.securityService = securityService;
            this.legendService = legendService;
            
        }

        public HttpResponseObject WMSRequest(WMSRequestType wmsRequestType, WMSParamBase wms)
        {
            if (string.IsNullOrEmpty(wms.LAYERS))
                return null;
            
            string query;
            switch (wmsRequestType)
            {
                case WMSRequestType.OverlayEMSGLayer:
                    query = new CreateWMSQuery().CreateMapRequestString((WMSRestParameter)wms, securityService.GetCurrentMandant().Id);
                    return SendHttpRequest.WmsRequest(new HttpRequestObject(new ServerConfigurationProvider().WMSUrl, query, contentType: wms.FORMAT));
                case WMSRequestType.BackgroundLayer:
                    //((WMSParameter)wms).CRS = "EPSG:21781";
                    query = new CreateWMSQuery().CreateBackgroundRequestString((WMSParameter)wms);
                     return SendHttpRequest.WmsRequest( new HttpRequestObject(new ServerConfigurationProvider().WMSUrlBaseLayer, query, contentType: wms.FORMAT));
                case WMSRequestType.AVLayer:
                    //((WMSParameter)wms).SRS = "EPSG:21781";
                    query = new CreateWMSQuery().CreateBackgroundRequestString((WMSParameter)wms);
                     return SendHttpRequest.WmsRequest( new HttpRequestObject(new ServerConfigurationProvider().WMSAVUrl, query, contentType: wms.FORMAT));
                case WMSRequestType.AVCLayer:
                     //((WMSParameter)wms).SRS = "EPSG:21781";
                     query = new CreateWMSQuery().CreateBackgroundRequestString((WMSParameter)wms);
                     return SendHttpRequest.WmsRequest(new HttpRequestObject(new ServerConfigurationProvider().SWMSAVUrl, query, contentType: wms.FORMAT,userName:new ServerConfigurationProvider().SWMSAVUserName, password:new ServerConfigurationProvider().SWMSAVPassword));
                case WMSRequestType.WMTSLayer:
                    //todo wenn anpassen wenn änderungsantrag durch ist
                    break;               
                default:
                    throw new NotImplementedException();
            }

            return null;
        }
           

        public Stream LegendEMSGRequest(string layer) 
        {
            try
            {
                return legendService.GetLegendStream(layer);
            }
            catch (Exception ex)
            {
                Loggers.ApplicationLogger.ErrorFormat(ex.Message, ex);
                return new MemoryStream();
            }
        }       
    }
}
