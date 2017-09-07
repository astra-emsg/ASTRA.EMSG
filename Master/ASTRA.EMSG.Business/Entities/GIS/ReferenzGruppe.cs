using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Mapping;

namespace ASTRA.EMSG.Business.Entities.GIS
{
    [TableShortName("RFG")]
    public class ReferenzGruppe : Entity
    {
        public ReferenzGruppe()
        {
            AchsenReferenzen = new List<AchsenReferenz>();
            StrassenabschnittGISList = new List<StrassenabschnittGIS>();
            ZustandsabschnittGISList = new List<ZustandsabschnittGIS>();
            KoordinierteMassnahmeGISList = new List<KoordinierteMassnahmeGIS>();
            MassnahmenvorschlagTeilsystemeGISList = new List<MassnahmenvorschlagTeilsystemeGIS>();
            RealisierteMassnahmeGISList = new List<RealisierteMassnahmeGIS>();
        }

        public virtual IList<AchsenReferenz> AchsenReferenzen { get; set; }

        //Should be HasOne (was not working), therefore workaround
        public virtual IList<StrassenabschnittGIS> StrassenabschnittGISList { get; set; }
        public virtual IList<ZustandsabschnittGIS> ZustandsabschnittGISList { get; set; }
        public virtual IList<KoordinierteMassnahmeGIS> KoordinierteMassnahmeGISList { get; set; }
        public virtual IList<MassnahmenvorschlagTeilsystemeGIS> MassnahmenvorschlagTeilsystemeGISList { get; set; }
        public virtual IList<RealisierteMassnahmeGIS> RealisierteMassnahmeGISList { get; set; }

       
        public virtual void AddAchsenReferenz(AchsenReferenz achsenReferenz)
        {
            AchsenReferenzen.Add(achsenReferenz);
            achsenReferenz.ReferenzGruppe = this;
        }

        public virtual void RemoveAchsenReferenz(AchsenReferenz achsenReferenz)
        {
            AchsenReferenzen.Remove(achsenReferenz);
            achsenReferenz.ReferenzGruppe = null;
        }
      
        public virtual Mandant Mandant
        {
            get
            {
                if (StrassenabschnittGIS != null) return StrassenabschnittGIS.Mandant;
                if (ZustandsabschnittGIS != null) return ZustandsabschnittGIS.Mandant;
                if (KoordinierteMassnahmeGIS != null) return KoordinierteMassnahmeGIS.Mandant;
                if (MassnahmenvorschlagTeilsystemeGIS != null) return MassnahmenvorschlagTeilsystemeGIS.Mandant;
                if (RealisierteMassnahmeGIS != null) return RealisierteMassnahmeGIS.Mandant;
                return null;
            }
            set { }
        }

        public virtual ErfassungsPeriod Erfassungsperiod
        {
            get
            {
                if (StrassenabschnittGIS != null) return StrassenabschnittGIS.ErfassungsPeriod;
                if (ZustandsabschnittGIS != null) return ZustandsabschnittGIS.ErfassungsPeriod;
                return null;
            }
            set { }
        }

        #region geometries functions
        
        public virtual StrassenabschnittGIS StrassenabschnittGIS
        {
            get { return StrassenabschnittGISList.Count > 0 ? StrassenabschnittGISList[0] : null; }
            set { StrassenabschnittGISList[0] = value; }
        }

        public virtual ZustandsabschnittGIS ZustandsabschnittGIS
        {
            get { return ZustandsabschnittGISList.Count > 0 ? ZustandsabschnittGISList[0] : null; }
            set { ZustandsabschnittGISList[0] = value; }
        }

        public virtual KoordinierteMassnahmeGIS KoordinierteMassnahmeGIS
        {
            get { return KoordinierteMassnahmeGISList.Count > 0 ? KoordinierteMassnahmeGISList[0] : null; }
            set { KoordinierteMassnahmeGISList[0] = value; }
        }

        public virtual MassnahmenvorschlagTeilsystemeGIS MassnahmenvorschlagTeilsystemeGIS
        {
            get { return MassnahmenvorschlagTeilsystemeGISList.Count > 0 ? MassnahmenvorschlagTeilsystemeGISList[0] : null; }
            set { MassnahmenvorschlagTeilsystemeGISList[0] = value; }
        }

        public virtual RealisierteMassnahmeGIS RealisierteMassnahmeGIS
        {
            get { return RealisierteMassnahmeGISList.Count > 0 ? RealisierteMassnahmeGISList[0] : null; }
            set { RealisierteMassnahmeGISList[0] = value; }
        }
        #endregion operational geometries

        public virtual ReferenzGruppe CopiedFrom { get; set; }
    }
}