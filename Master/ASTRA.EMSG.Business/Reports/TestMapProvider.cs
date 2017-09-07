using System.Drawing;
using System.Web;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.EineListeVonKoordiniertenMassnahmen;
using ASTRA.EMSG.Business.Reports.EineListeVonMassnahmenGegliedertNachTeilsystemen;
using ASTRA.EMSG.Business.Reports.ListeDerInspektionsrouten;
using ASTRA.EMSG.Business.Reports.MassnahmenvorschlagProZustandsabschnitt;
using ASTRA.EMSG.Business.Reports.StrassenabschnitteListe;
using ASTRA.EMSG.Business.Reports.ZustandProZustandsabschnitt;
using NHibernate;
using System.IO;

namespace ASTRA.EMSG.Business.Reports
{
    public abstract class TestMapProvider<TReportParameter, TMapReportParameter, TEntity> : IMapInfoProviderBase<TMapReportParameter>, IBoundingBoxFiltererBase<TReportParameter, TEntity>
        where TEntity : Entity
        where TReportParameter : EmsgGisReportParameter
        where TMapReportParameter : TReportParameter
    {
        public string GetMapImageInfo(TMapReportParameter parameter)
        {
            string tempPath = Path.GetTempFileName() + ".png";
            File.Copy(HttpContext.Current.Server.MapPath(@"~/Content/MapPlaceHolder.png"),
                tempPath);
            parameter.ReportImagePath = tempPath;
            return tempPath;
        }

        public IQueryOver<TEntity, TEntity> FilterForBoundingBox(IQueryOver<TEntity, TEntity> queryOver, TReportParameter parameter)
        {
            return queryOver;
        }
    }

    public class TestListeDerInspektionsroutenMapProvider : TestMapProvider<ListeDerInspektionsroutenParameter,ListeDerInspektionsroutenMapParameter, StrassenabschnittGIS>, IListeDerInspektionsroutenMapProvider { }
    public class TestStrassenabschnitteListeMapProvider : TestMapProvider<StrassenabschnitteListeParameter, StrassenabschnitteListeMapParameter, StrassenabschnittGIS>, IStrassenabschnitteListeMapProvider { }
    public class TestZustandProZustandsabschnittMapProvider : TestMapProvider<ZustandProZustandsabschnittParameter,ZustandProZustandsabschnittMapParameter, ZustandsabschnittGIS>, IZustandProZustandsabschnittMapProvider { }
    public class TestMassnahmenvorschlagProZustandsabschnittMapProvider : TestMapProvider<MassnahmenvorschlagProZustandsabschnittParameter, MassnahmenvorschlagProZustandsabschnittMapParameter, ZustandsabschnittGIS>, IMassnahmenvorschlagProZustandsabschnittMapProvider { }
    public class TestEineListeVonKoordiniertenMassnahmenMapProvider : TestMapProvider<EineListeVonKoordiniertenMassnahmenParameter,EineListeVonKoordiniertenMassnahmenMapParameter, KoordinierteMassnahmeGIS>, IEineListeVonKoordiniertenMassnahmenMapProvider { }
    public class TestEineListeVonMassnahmenGegliedertNachTeilsystemenMapProvider : TestMapProvider<EineListeVonMassnahmenGegliedertNachTeilsystemenParameter, EineListeVonMassnahmenGegliedertNachTeilsystemenMapParameter, MassnahmenvorschlagTeilsystemeGIS>, IEineListeVonMassnahmenGegliedertNachTeilsystemenMapProvider { }

}