using System.Collections.Generic;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Models.Administration
{
    public class HomeModel : IModel
    {
        public HomeModel()
        {
            Rollen = new List<Rolle>();
            NotInitialisedMandanten = new List<string>();
        }

        public NetzErfassungsmodus? NetzErfassungsmodus { get; set; }
        public string NetzErfassungsmodusBezeichnung { get; set; }

        public List<Rolle> Rollen { get; set; }
        public string RolleBezeichnungen { get; set; }

        public string MandantName { get; set; }
        public string MandantBezeichnung { get; set; }

        public List<string> NotInitialisedMandanten { get; set; }
        public string NotInitialisedMandantenBezeichnung { get; set; }

        public ApplicationMode AppMode { get; set; }
    }
}