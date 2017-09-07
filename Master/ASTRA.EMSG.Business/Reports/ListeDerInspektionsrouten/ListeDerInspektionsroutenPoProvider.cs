using System;
using System.Collections.Generic;
using System.Net;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using ASTRA.EMSG.Common.Utils;
using System.Linq;
using ASTRA.EMSG.Business.Services.GIS;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Web.Mvc;

namespace ASTRA.EMSG.Business.Reports.ListeDerInspektionsrouten
{
    public interface IListeDerInspektionsroutenPoProvider : IPoProvider
    {
    }

    [ReportInfo(AuswertungTyp.W1_3, NetzErfassungsmodus = NetzErfassungsmodus.Gis)]
    public class ListeDerInspektionsroutenPoProvider : EmsgGisTablePoProviderBase<ListeDerInspektionsroutenParameter, ListeDerInspektionsroutenPo, StrassenabschnittGIS>, IListeDerInspektionsroutenPoProvider
    {
        private readonly IListeDerInspektionsroutenMapProvider listeDerInspektionsroutenMapProvider;
        private readonly ITransactionScopeProvider transactionScopeProvider;
        private readonly IFiltererFactory filtererFactory;
        private readonly ILegendService legendService;
        private readonly IServerConfigurationProvider serverConfigurationProvider;

        public ListeDerInspektionsroutenPoProvider(
            IListeDerInspektionsroutenMapProvider listeDerInspektionsroutenMapProvider,
            ITransactionScopeProvider transactionScopeProvider,
            IFiltererFactory filtererFactory,
            ILegendService legendService,
            IServerConfigurationProvider serverConfigurationProvider
            )
        {
            this.listeDerInspektionsroutenMapProvider = listeDerInspektionsroutenMapProvider;
            this.transactionScopeProvider = transactionScopeProvider;
            this.filtererFactory = filtererFactory;
            this.legendService = legendService;
            this.serverConfigurationProvider = serverConfigurationProvider;
        }

        protected override List<ListeDerInspektionsroutenPo> GetPresentationObjectListForSummarisch(ListeDerInspektionsroutenParameter parameter) { return NotSupported(); }
        protected override List<ListeDerInspektionsroutenPo> GetPresentationObjectListForTabellarisch(ListeDerInspektionsroutenParameter parameter) { return NotSupported(); }

        protected override List<ListeDerInspektionsroutenPo> GetPresentationObjectListForGis(ListeDerInspektionsroutenParameter parameter)
        {
            var queryOver = transactionScopeProvider.CurrentTransactionScope.Session.QueryOver<StrassenabschnittGIS>();
            queryOver = filtererFactory.CreateFilterer<StrassenabschnittGIS>(parameter).Filter(queryOver);
            queryOver = listeDerInspektionsroutenMapProvider.FilterForBoundingBox(queryOver, parameter);

            //Preload InspektionsRouteGIS entities
            var currentErfassungsperiod = HistorizationService.GetCurrentErfassungsperiod();
            transactionScopeProvider.CurrentTransactionScope.Session
                .QueryOver<InspektionsRouteGIS>()
                .Where(ir => ir.ErfassungsPeriod == currentErfassungsperiod)
                .Where(ir => ir.Mandant == currentErfassungsperiod.Mandant)
                .List<InspektionsRouteGIS>();

            //Fetch InspektionsRtStrAbschnitte and Belastungskategorie
            IEnumerable<StrassenabschnittGIS> list = queryOver
                .Fetch(sa => sa.InspektionsRtStrAbschnitte).Eager
                .Fetch(sa => sa.Belastungskategorie).Eager
                .List<StrassenabschnittGIS>()
                .Where(sa => sa.InspektionsRtStrAbschnitte.Any());

            //Filtering in memory
            if (parameter.Inspektionsroutename.HasText())
                list = list.Where(sa => sa.InspektionsRtStrAbschnitte.Any(ira => (ira.InspektionsRouteGIS.Bezeichnung ?? "").ToLower().Contains(parameter.Inspektionsroutename.ToLower())));

            if (parameter.InspektionsrouteInInspektionBei.HasText())
                list = list.Where(sa => sa.InspektionsRtStrAbschnitte.Any(ira => (ira.InspektionsRouteGIS.InInspektionBei ?? "").ToLower().Contains(parameter.InspektionsrouteInInspektionBei.ToLower())));

            if (parameter.InspektionsrouteInInspektionBisVon.HasValue)
                list = list.Where(sa => sa.InspektionsRtStrAbschnitte.Any(ira => ira.InspektionsRouteGIS.InInspektionBis >= parameter.InspektionsrouteInInspektionBisVon.Value));

            if (parameter.InspektionsrouteInInspektionBisBis.HasValue)
                list = list.Where(sa => sa.InspektionsRtStrAbschnitte.Any(ira => ira.InspektionsRouteGIS.InInspektionBis <= parameter.InspektionsrouteInInspektionBisBis.Value));

            return list.OrderBy(sa => sa.InspektionsRtStrAbschnitte.Single().InspektionsRouteGIS.Bezeichnung).ThenBy(sa => sa.InspektionsRtStrAbschnitte.Single().Reihenfolge).Select(sa => CreatePo(sa, parameter.LegendImageBaseUrl, parameter.IsPreview)).ToList();
        }

        private ListeDerInspektionsroutenPo CreatePo(StrassenabschnittGIS strassenabschnittGIS, string baseUrl, bool isPreview)
        {
            var inspektionsRtStrAbschnitte = strassenabschnittGIS.InspektionsRtStrAbschnitte.Single();
            var inspektionsRouteGIS = inspektionsRtStrAbschnitte.InspektionsRouteGIS;

            var po = CreatePoFromEntityWithCopyingMatchingProperties(strassenabschnittGIS);

            po.BelastungskategorieBezeichnung = LocalizationService.GetLocalizedBelastungskategorieTyp(po.BelastungskategorieTyp);
            po.StrasseneigentuemerBezeichnung = LocalizationService.GetLocalizedEnum(po.Strasseneigentuemer);
            po.InInspektionBei = inspektionsRouteGIS.InInspektionBei;
            po.InInspektionBis = inspektionsRouteGIS.InInspektionBis;
            po.Inspektionsroutename = inspektionsRouteGIS.Bezeichnung;
            po.Reihenfolge = inspektionsRtStrAbschnitte.Reihenfolge;
            if (inspektionsRouteGIS.LegendNumber != null)
            {
                po.ImageUrl = legendService.GetInspektionsRouteLegendImageUrl((int)inspektionsRouteGIS.LegendNumber, baseUrl);
                if (!isPreview)
                {
                    string base64 = string.Empty;

                    using (Stream s = ((FileStreamResult)legendService.GetInspektionsRouteLegendImage((int)inspektionsRouteGIS.LegendNumber)).FileStream)
                    {
                        using (Image image = Image.FromStream(s))
                        {
                            base64 = this.ImageToBase64(image, ImageFormat.Bmp);
                        }
                    }

                    po.ImageContent = base64;
                }
            }

            return po;
        }
        private string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        protected override PaperType PaperType { get { return Reporting.PaperType.A4Landscape; } }

        protected override void BuildFilterList(IFilterListBuilder<ListeDerInspektionsroutenParameter> filterListBuilder)
        {
            base.BuildFilterList(filterListBuilder);
            AddErfassungsPeriodFilterListItem(filterListBuilder);
            filterListBuilder.AddFilterListItem(p => p.Eigentuemer);
            filterListBuilder.AddFilterListItem(p => p.Inspektionsroutename);
            filterListBuilder.AddFilterListItem(p => p.Strassenname);
            filterListBuilder.AddFilterListItem(p => p.InspektionsrouteInInspektionBei);
            filterListBuilder.AddFilterListItem(p => p.InspektionsrouteInInspektionBisVon);
            filterListBuilder.AddFilterListItem(p => p.InspektionsrouteInInspektionBisBis);
        }
    }
}
