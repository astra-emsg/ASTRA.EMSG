using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Administration;
using ASTRA.EMSG.Web.Areas.Auswertungen;
using ASTRA.EMSG.Web.Areas.Benchmarking;
using ASTRA.EMSG.Web.Areas.NetzverwaltungGIS;
using ASTRA.EMSG.Web.Areas.NetzverwaltungStrassennamen;
using ASTRA.EMSG.Web.Areas.NetzverwaltungSummarisch;
using ASTRA.EMSG.Web.Controllers;

namespace ASTRA.EMSG.Web.Infrastructure.Security
{
    public interface IAvailabilityService
    {
        Availability CheckAvailability();
    }

    public class AvailabiltyService : IAvailabilityService
    {
        private readonly IErfassungsPeriodService erfassungsPeriodService;

        private readonly Dictionary<string, List<NetzErfassungsmodus>> areaModusAssociationDictionary = new Dictionary<string, List<NetzErfassungsmodus>>();
        private readonly Dictionary<string, List<NetzErfassungsmodus>> controllerModusAssociationDictionary = new Dictionary<string, List<NetzErfassungsmodus>>();

        public AvailabiltyService(IErfassungsPeriodService erfassungsPeriodService)
        {
            this.erfassungsPeriodService = erfassungsPeriodService;
            Init();
        }

        private void Init()
        {
            areaModusAssociationDictionary.Add(String.Empty, new List<NetzErfassungsmodus>{NetzErfassungsmodus.Summarisch, NetzErfassungsmodus.Tabellarisch,NetzErfassungsmodus.Gis});

            SetUpAreaModes<AdministrationAreaRegistration>(new List<NetzErfassungsmodus> { NetzErfassungsmodus.Summarisch, NetzErfassungsmodus.Tabellarisch, NetzErfassungsmodus.Gis });
            SetUpAreaModes<NetzverwaltungSummarischAreaRegistration>(new List<NetzErfassungsmodus> { NetzErfassungsmodus.Summarisch });
            SetUpAreaModes<NetzverwaltungStrassennamenAreaRegistration>(new List<NetzErfassungsmodus> { NetzErfassungsmodus.Tabellarisch });
            SetUpAreaModes<NetzverwaltungGISAreaRegistration>(new List<NetzErfassungsmodus> { NetzErfassungsmodus.Gis });
            SetUpAreaModes<AuswertungenAreaRegistration>(new List<NetzErfassungsmodus>());
            SetUpAreaModes<BenchmarkingAreaRegistration>(new List<NetzErfassungsmodus> { NetzErfassungsmodus.Summarisch, NetzErfassungsmodus.Tabellarisch, NetzErfassungsmodus.Gis });

            foreach (Type controllerType in typeof(HomeController).Assembly.FindDerivedTypesFromAssembly(typeof(Controller), true))
            {
                var controllerAvailabilityAttribute = controllerType.GetCustomAttributes(typeof(AllowedModesAttribute), false).SingleOrDefault() as AllowedModesAttribute;
                if (controllerAvailabilityAttribute != null)
                    controllerModusAssociationDictionary.Add(controllerType.GetControllerKey(), controllerAvailabilityAttribute.NetzErfassungsmoduses);
            }
        }

        private void SetUpAreaModes<TAreaRegistration>(List<NetzErfassungsmodus> modes)
        {
            areaModusAssociationDictionary.Add(RouteDataKeyHelper.GetAreaKey<TAreaRegistration>(), modes);
        }

        public virtual Availability CheckAvailability()
        {
            RouteData routeData = HttpContext.Current.Request.RequestContext.RouteData;

            return CheckAvailability(routeData);
        }

        private Availability CheckAvailability(RouteData routeData)
        {
            return CheckAvailability(routeData.GetAreaKey(), routeData.GetControllerKey());
        }

        private Availability CheckAvailability(string areaKey, string controllerKey)
        {
            List<NetzErfassungsmodus> supportedControllerModes;
            controllerModusAssociationDictionary.TryGetValue(controllerKey, out supportedControllerModes);

            if (supportedControllerModes == null)
            {
                List<NetzErfassungsmodus> supportedAreaModes;
                areaModusAssociationDictionary.TryGetValue(areaKey, out supportedAreaModes);

                if (supportedAreaModes != null)
                {
                    if (supportedAreaModes.Distinct().Count() == Enum.GetNames(typeof(NetzErfassungsmodus)).Length)
                        return Availability.Available;
                    if (supportedAreaModes.Contains(erfassungsPeriodService.GetCurrentErfassungsPeriod().NetzErfassungsmodus))
                        return Availability.Available;
                }

                return Availability.Unavailable;
            }

            if (supportedControllerModes.Distinct().Count() == Enum.GetNames(typeof(NetzErfassungsmodus)).Length)
                return Availability.Available;
            if (supportedControllerModes.Contains(erfassungsPeriodService.GetCurrentErfassungsPeriod().NetzErfassungsmodus))
                return Availability.Available;

            return Availability.Unavailable;
        }
    }
}