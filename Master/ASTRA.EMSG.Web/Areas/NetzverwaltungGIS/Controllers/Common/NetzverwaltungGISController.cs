using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.Services.EntityServices;
using ASTRA.EMSG.Business.Models;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Master.GeoJSON;
using ASTRA.EMSG.Web.Infrastructure.Security;

namespace ASTRA.EMSG.Web.Areas.NetzverwaltungGIS.Controllers.Common
{
    public class NetzverwaltungGISController : Controller
    {
        private readonly IStrassenabschnittGISService strassenabschnittGISService;
      

        public NetzverwaltungGISController(IStrassenabschnittGISService strassenabschnittGISService)
        {
            this.strassenabschnittGISService = strassenabschnittGISService;
            
        }
        [HttpGet]
        public ActionResult GetStrassenabschnittAt(double x, double y, double tolerance)
        {
            try
            {
                StrassenabschnittGISModel strassenabschnitt = strassenabschnittGISService.GetCurrentStrassenabschnittAt(x, y, tolerance);
                return Content(strassenabschnitt.FeatureGeoJSONString, "application/json");
            }
            catch (Exception exc)
            {
                return Content(GeoJSONStrings.GeoJSONFailure(exc.Message), "application/json");
            }
        }

        [HttpGet]
        public ActionResult GetStrassenabschnittByID(string id)
        {
            try
            {
                StrassenabschnittGISModel strassenabschnitt = strassenabschnittGISService.GetById(Guid.Parse(id));
                return Content(strassenabschnitt.FeatureGeoJSONString, "application/json");
            }
            catch (Exception exc)
            {
                return Content(GeoJSONStrings.GeoJSONFailure(exc.Message), "application/json");
            }
        }
    }
}
