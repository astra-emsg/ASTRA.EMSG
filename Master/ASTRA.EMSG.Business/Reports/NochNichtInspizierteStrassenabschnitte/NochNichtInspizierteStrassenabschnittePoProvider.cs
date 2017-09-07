using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.EntityServices;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Business.Services.EntityServices.Strassennamen;
using ASTRA.EMSG.Common.Enums;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Reports.NochNichtInspizierteStrassenabschnitte
{
    public interface INochNichtInspizierteStrassenabschnittePoProvider : IPoProvider
    {
    }

    [ReportInfo(AuswertungTyp.W3_4, NetzErfassungsmodus = NetzErfassungsmodus.Tabellarisch)]
    [ReportInfo(AuswertungTyp.W3_4, NetzErfassungsmodus = NetzErfassungsmodus.Gis)]
    public class NochNichtInspizierteStrassenabschnittePoProvider : EmsgTablePoProviderBase<NochNichtInspizierteStrassenabschnitteParameter, NochNichtInspizierteStrassenabschnittePo>, INochNichtInspizierteStrassenabschnittePoProvider
    {
        private readonly IStrassenabschnittService strassenabschnittService;
        private readonly IStrassenabschnittGISService strassenabschnittGisService;

        public NochNichtInspizierteStrassenabschnittePoProvider(IStrassenabschnittService strassenabschnittService, IStrassenabschnittGISService strassenabschnittGisService)
        {
            this.strassenabschnittService = strassenabschnittService;
            this.strassenabschnittGisService = strassenabschnittGisService;
        }

        protected override List<NochNichtInspizierteStrassenabschnittePo> GetPresentationObjectListForSummarisch(NochNichtInspizierteStrassenabschnitteParameter parameter)
        {
            return NotSupported();
        }

        protected override List<NochNichtInspizierteStrassenabschnittePo> GetPresentationObjectListForTabellarisch(NochNichtInspizierteStrassenabschnitteParameter parameter)
        {
            var strassenabschnitten = strassenabschnittService.GetEntitiesBy(GetErfassungsPeriod(parameter.ErfassungsPeriodId));
               
            return FilterStrassenabschnittBase(strassenabschnitten, parameter)
                .Fetch(s => s.Belastungskategorie)
                .FetchMany(z => z.Zustandsabschnitten)
                .ToArray()
                .Where(s => !s.Zustandsabschnitten.Any() || s.Laenge > s.Zustandsabschnitten.Sum(z => z.Laenge))
                .OrderBy(s => s.Strassenname).ThenBy(s => s.Abschnittsnummer)
                .Select(CreateBasePo)
                .ToList();
        }

        protected override List<NochNichtInspizierteStrassenabschnittePo> GetPresentationObjectListForGis(NochNichtInspizierteStrassenabschnitteParameter parameter)
        {
            return FilterStrassenabschnittBase(strassenabschnittGisService.GetEntitiesBy(GetErfassungsPeriod(parameter.ErfassungsPeriodId)), parameter)
                .Fetch(s => s.Belastungskategorie)
                .FetchMany(z => z.Zustandsabschnitten)
                .ToArray()
                .Where(s => !s.Zustandsabschnitten.Any() || s.Laenge > s.Zustandsabschnitten.Sum(z => z.Laenge)).ToArray()
                .OrderBy(s => s.Strassenname)
                .Select(CreateBasePo)
                .ToList();
        }

        private static IQueryable<T> FilterStrassenabschnittBase<T>(IQueryable<T> strassenabschnitten, NochNichtInspizierteStrassenabschnitteParameter parameter)
            where T : StrassenabschnittBase
        {
            if (parameter.Eigentuemer.HasValue)
                strassenabschnitten = strassenabschnitten.Where(s => s.Strasseneigentuemer == parameter.Eigentuemer.Value);

            if (!string.IsNullOrWhiteSpace(parameter.Strassenname))
                strassenabschnitten = strassenabschnitten.Where(s => s.Strassenname.ToLower().Contains(parameter.Strassenname.ToLower()));

            return strassenabschnitten;
        }

        private NochNichtInspizierteStrassenabschnittePo CreateBasePo(StrassenabschnittBase strassenabschnittBase)
        {
            var result = CreatePoFromEntityWithCopyingMatchingProperties(strassenabschnittBase);
            result.StrasseneigentuemerBezeichnung = LocalizationService.GetLocalizedEnum(strassenabschnittBase.Strasseneigentuemer);
            result.BelastungskategorieBezeichnung = LocalizationService.GetLocalizedBelastungskategorieTyp(strassenabschnittBase.BelastungskategorieTyp);

            return result;
        }

        public override bool AvailableInCurrentErfassungPeriod { get { return true; } }

        protected override void BuildFilterList(IFilterListBuilder<NochNichtInspizierteStrassenabschnitteParameter> builder)
        {
            base.BuildFilterList(builder);
            AddErfassungsPeriodFilterListItem(builder);
            builder.AddFilterListItem(p => p.Eigentuemer);
            builder.AddFilterListItem(p => p.Strassenname);
        }

        protected override PaperType PaperType { get { return PaperType.A4Landscape; } }
    }
}