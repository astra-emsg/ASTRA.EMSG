using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using NHibernate;
using NHibernate.Linq;
using ASTRA.EMSG.Business.Entities;
using System.IO.Compression;
using System.IO;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Tests.Common.Utils
{
    public class AxisDataUtils
    {

        public static void ClearUpdateLog(ISession session, Mandant testMandant, ErfassungsPeriod erfPeriod)
        {
            var allEntries = session.QueryOver<AchsenUpdateLog>()
                .Where(o => o.ErfassungsPeriod == erfPeriod)
                .Where(o => o.Mandant == testMandant)
                .List();
            allEntries.ToList().ForEach(o => session.Delete(o));
        }

        public static void GUnZipFile(string inFile, string outFile)
        {
            var inStream = new GZipStream(new FileStream(inFile, FileMode.Open, FileAccess.Read, FileShare.Read), CompressionMode.Decompress);

            var outStream = new FileStream(outFile, FileMode.Create, FileAccess.Write);

            byte[] buffer = new byte[256];

            int count;

            while ((count = inStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                outStream.Write(buffer, 0, count);
            }

            inStream.Close();
            outStream.Close();
        }

        public static void ClearAxisDataAndReferencesForMandant(ISession session, Guid mandantId)
        {
            var selectedMandant = session.Query<Mandant>().Single(m => m.Id == mandantId);
            if (selectedMandant != null)
            {
                TestDataUtils.DeleteMandantData(session, selectedMandant);
            }
            //var segments = new List<AchsenSegment>(session.QueryOver<AchsenSegment>().Where(o => o.Mandant.Id == mandantId).List());

            //foreach (var segment in segments)
            //{
            //    var achsRefs = new List<AchsenReferenz>(segment.AchsenReferenzen);

            //    foreach (AchsenReferenz achsRef in achsRefs)
            //    {
            //        segment.AchsenReferenzen.Remove(achsRef);

            //        ReferenzGruppe refGroup = achsRef.ReferenzGruppe;

            //        achsRef.ReferenzGruppe = null;
            //        refGroup.AchsenReferenzen.Remove(achsRef);

            //        foreach (ZustandsabschnittGIS entity in new List<ZustandsabschnittGIS>(refGroup.ZustandsabschnittGISList))
            //        {
            //            refGroup.ZustandsabschnittGISList.Remove(entity);
            //            entity.ReferenzGruppe = null;

            //            session.Delete(entity);
            //        }
            //        foreach (StrassenabschnittGIS strabs in new List<StrassenabschnittGIS>(refGroup.StrassenabschnittGISList))
            //        {
            //            refGroup.StrassenabschnittGISList.Remove(strabs);
            //            strabs.ReferenzGruppe = null;

            //            session.Delete(strabs);
            //        }
            //        foreach (KoordinierteMassnahmeGIS entity in new List<KoordinierteMassnahmeGIS>(refGroup.KoordinierteMassnahmeGISList))
            //        {
            //            refGroup.KoordinierteMassnahmeGISList.Remove(entity);
            //            entity.ReferenzGruppe = null;

            //            session.Delete(entity);
            //        }
            //        foreach (RealisierteMassnahmeGIS entity in new List<RealisierteMassnahmeGIS>(refGroup.RealisierteMassnahmeGISList))
            //        {
            //            refGroup.RealisierteMassnahmeGISList.Remove(entity);
            //            entity.ReferenzGruppe = null;

            //            session.Delete(entity);
            //        }
            //        foreach (MassnahmenvorschlagTeilsystemeGIS entity in new List<MassnahmenvorschlagTeilsystemeGIS>(refGroup.MassnahmenvorschlagTeilsystemeGISList))
            //        {
            //            refGroup.MassnahmenvorschlagTeilsystemeGISList.Remove(entity);
            //            entity.ReferenzGruppe = null;

            //            session.Delete(entity);
            //        }


            //        session.Delete(refGroup);

            //        achsRef.AchsenSegment = null;

            //        session.Delete(achsRef);
            //    }
            //}
            //session.Flush();

            //ClearAxisDataForMandant(session, mandantId);
        }

        //public static void ClearAxisDataForMandant(ISession session, Guid mandantId)
        //{
        //    var mandantIdPropertyName = ExpressionHelper.GetPropertyName<Mandant, Guid>(m => m.Id);
        //    var achsenSegmentTypeName = typeof(AchsenSegment).Name;
        //    var achsenSegmentMandantPropertyName = ExpressionHelper.GetPropertyName<AchsenSegment, Mandant>(a => a.Mandant);
        //    {
        //        var achsenUpdateLogTypeName = typeof(AchsenUpdateLog).Name;
        //        var achsenUpdateLogMandantPropertyName = ExpressionHelper.GetPropertyName<AchsenUpdateLog, Mandant>(m => m.Mandant);
        //        var qry = session.CreateQuery(string.Format("delete {0} achsenUpdateLog where achsenUpdateLog.{1}.{2} = :mandant",
        //            new string[]{achsenUpdateLogTypeName, achsenUpdateLogMandantPropertyName, mandantIdPropertyName} ));
        //        qry.SetParameter("mandant", mandantId);
        //        qry.ExecuteUpdate();
        //    }
        //    {
        //        var sektorTypeName = typeof(Sektor).Name;
        //        var sektorAchsenSegmentPropertyName = ExpressionHelper.GetPropertyName<Sektor, AchsenSegment>(a => a.AchsenSegment);
        //        var achsenSegmentIdPropertyName = ExpressionHelper.GetPropertyName<AchsenSegment, Guid>(a => a.Id);
        //        var qry = session.CreateQuery(string.Format("delete {0} sektor where sektor.{1}.{2} in (select {2} from {3} segment where segment.{4}.{5} = :mandant)", 
        //           new string[]{sektorTypeName, sektorAchsenSegmentPropertyName, achsenSegmentIdPropertyName, achsenSegmentTypeName, achsenSegmentMandantPropertyName, mandantIdPropertyName}));
        //        qry.SetParameter("mandant", mandantId);
        //        qry.ExecuteUpdate();
        //    }
        //    {
        //        var qry = session.CreateQuery(string.Format("delete {0} segment where segment.{1}.{2} = :mandant",
        //            new string[]{achsenSegmentTypeName, achsenSegmentMandantPropertyName, mandantIdPropertyName } ));
        //        qry.SetParameter("mandant", mandantId);
        //        qry.ExecuteUpdate();
        //    }
        //    {
        //        var achseTypeName = typeof(Achse).Name;
        //        var achseMandantPropertyName = ExpressionHelper.GetPropertyName<Achse, Mandant>(a => a.Mandant);
        //        var qry = session.CreateQuery(string.Format("delete {0} achse where achse.{1}.{2} = :mandant",new string[]{ achseTypeName, achseMandantPropertyName, mandantIdPropertyName} ));
        //        qry.SetParameter("mandant", mandantId);
        //        qry.ExecuteUpdate();
        //    }
        //}
    }
}
