using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.Mapping;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Common.Enums;
using GeoAPI.Geometries;

namespace ASTRA.EMSG.Business.Entities.GIS
{
    [TableShortName("RMG")]
    public class RealisierteMassnahmeGIS : RealisierteMassnahmeBase, IAbschnittGISBase
    {
        public virtual IList<TeilsystemTyp> BeteiligteSysteme { get; set; }

        public virtual decimal? KostenGesamtprojekt { get; set; }

        public virtual string LeitendeOrganisation { get; set; }

        public virtual IGeometry Shape { get; set; }
        
        //public List<AchsenReferenz> AchsenReferenzList { get; set; }
        public virtual ReferenzGruppe ReferenzGruppe { get; set; }

        public virtual string DisplayName
        {
            get { return Projektname + " (" + BezeichnungVon + " - " + BezeichnungBis + ")"; }
        }
    }
}