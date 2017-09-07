using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.GIS;

namespace ASTRA.EMSG.Business.AchsenUpdate
{
    public class ReferenceUpdaterStatistics
    {
        public int UpdatedReferences = 0;
        public int DeletedReferences = 0;
        
        public int UpdatedStrassenabschnitts = 0;
        public int DeletedStrassenabschnitts = 0;

        public int UpdatedZustandsabschnitts = 0;
        public int DeletedZustandsabschnitts = 0;

        public int UpdatedKoordinierteMassnahmen = 0;
        public int DeletedKoordinierteMassnahmen = 0;

        public int UpdatedMassnahmenvorschlagTeilsysteme = 0;
        public int DeletedMassnahmenvorschlagTeilsysteme = 0;


        public void ReportRemoved(IAbschnittGISBase item)
        {
            if (item is StrassenabschnittGIS) DeletedStrassenabschnitts++;
            if (item is ZustandsabschnittGIS) DeletedZustandsabschnitts++;
            if (item is KoordinierteMassnahmeGIS) DeletedKoordinierteMassnahmen++;
            if (item is MassnahmenvorschlagTeilsystemeGIS) DeletedMassnahmenvorschlagTeilsysteme++;
        }

        public void ReportUpdated(IAbschnittGISBase item)
        {
            if (item is StrassenabschnittGIS) UpdatedStrassenabschnitts++;
            if (item is ZustandsabschnittGIS) UpdatedZustandsabschnitts++;
            if (item is KoordinierteMassnahmeGIS) UpdatedKoordinierteMassnahmen++;
            if (item is MassnahmenvorschlagTeilsystemeGIS) UpdatedMassnahmenvorschlagTeilsysteme++;
        }

        public override string ToString()
        {
            return
                  " #UpdRefs: " + UpdatedReferences
                + " #RemRefs: " + DeletedReferences
                + " #UpdStra: " + UpdatedStrassenabschnitts
                + " #RemStra: " + DeletedStrassenabschnitts
                + " #UpdZust: " + UpdatedZustandsabschnitts
                + " #RemZust: " + DeletedZustandsabschnitts
                + " #UpdKoord: " + UpdatedKoordinierteMassnahmen
                + " #RemKoord: " + DeletedKoordinierteMassnahmen
                + " #UpdMVorschl: " + UpdatedMassnahmenvorschlagTeilsysteme
                + " #RemMVorschl: " + DeletedMassnahmenvorschlagTeilsysteme;
        }

    }
}
