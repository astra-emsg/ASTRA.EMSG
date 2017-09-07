using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;

namespace ASTRA.EMSG.Business.Services.GIS
{
    public interface IAbschnittGisValidationService : IService
    {
        bool ValidateOverlap(IAbschnittGISModelBase abschnitt);
    }
   public  class AbschnittGisValidationService:IAbschnittGisValidationService
    {
       private readonly IAchsenReferenzService achsenReferenzService;
       private readonly IGISService gisService;

       public AbschnittGisValidationService(
           IAchsenReferenzService achsenReferenzService,
           IGISService gisService)
       {
           this.achsenReferenzService = achsenReferenzService;
           this.gisService = gisService;
       }
       
       //check for overlaps with other strabs or zabs on the same achssegment returns false if overlaps
       public bool ValidateOverlap(IAbschnittGISModelBase abschnitt)
       {
           IList<AchsenReferenzModel> neue_achsenreferenzen = abschnitt.ReferenzGruppeModel.AchsenReferenzenModel;

           foreach (var neue_achsenreferenz in neue_achsenreferenzen)
           {
               Guid achsensegmentID = neue_achsenreferenz.AchsenSegmentModel.Id;

               IList<AchsenReferenz> alteAchsenreferenzen = achsenReferenzService.GetCurrentEntities()
               .Where(r => r.AchsenSegment.Id == achsensegmentID).ToList();

               if (abschnitt.GetType() == typeof(ZustandsabschnittGISModel))
               {
                   alteAchsenreferenzen = alteAchsenreferenzen.Where(r => r.ReferenzGruppe.ZustandsabschnittGIS != null
                       && r.ReferenzGruppe.ZustandsabschnittGIS.ErfassungsPeriod.IsClosed == false
                       && r.ReferenzGruppe.ZustandsabschnittGIS.Id != abschnitt.Id).ToList();
               }
               if (abschnitt.GetType() == typeof(StrassenabschnittGISModel))
               {
                   alteAchsenreferenzen = alteAchsenreferenzen.Where(r => r.ReferenzGruppe.StrassenabschnittGIS != null
                       && r.ReferenzGruppe.StrassenabschnittGIS.ErfassungsPeriod.IsClosed == false
                       && r.ReferenzGruppe.StrassenabschnittGIS.Id != abschnitt.Id).ToList();
               }



               if (!gisService.CheckOverlapp(alteAchsenreferenzen.ToList(), neue_achsenreferenz.Shape))
               {
                   return false;
               }
           }
           return true;
       }
    }
}
