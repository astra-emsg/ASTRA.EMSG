using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.EntityServices.Strassennamen;
using ASTRA.EMSG.Common.Enums;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustProStrassenabschnitt
{
    public interface IWiederbeschaffungswertUndWertverlustProStrassenabschnittPoProvider
    {
    }
    
    [ReportInfo(AuswertungTyp.W2_2, NetzErfassungsmodus = NetzErfassungsmodus.Gis)]
    [ReportInfo(AuswertungTyp.W2_2, NetzErfassungsmodus = NetzErfassungsmodus.Tabellarisch)]
    public class WiederbeschaffungswertUndWertverlustProStrassenabschnittPoProvider : EmsgTablePoProviderBase<WiederbeschaffungswertUndWertverlustProStrassenabschnittParameter, WiederbeschaffungswertUndWertverlustProStrassenabschnittPo>, IWiederbeschaffungswertUndWertverlustProStrassenabschnittPoProvider
    {
        private readonly IStrassenabschnittService strassenabschnittService;
        private readonly IStrassenabschnittGISService strassenabschnittGISService;
        private readonly IFiltererFactory filtererFactory;
        private readonly IWiederbeschaffungswertKatalogService wiederbeschaffungswertKatalogService;

        public WiederbeschaffungswertUndWertverlustProStrassenabschnittPoProvider(
            IStrassenabschnittService strassenabschnittService, 
            IWiederbeschaffungswertKatalogService wiederbeschaffungswertKatalogService, 
            IStrassenabschnittGISService strassenabschnittGISService,
            IFiltererFactory filtererFactory)
        {
            this.strassenabschnittService = strassenabschnittService;
            this.wiederbeschaffungswertKatalogService = wiederbeschaffungswertKatalogService;
            this.strassenabschnittGISService = strassenabschnittGISService;
            this.filtererFactory = filtererFactory;
        }

        protected override List<WiederbeschaffungswertUndWertverlustProStrassenabschnittPo> GetPresentationObjectListForSummarisch(WiederbeschaffungswertUndWertverlustProStrassenabschnittParameter parameter)
        {
            return NotSupported();
        }

        protected override List<WiederbeschaffungswertUndWertverlustProStrassenabschnittPo> GetPresentationObjectListForTabellarisch(WiederbeschaffungswertUndWertverlustProStrassenabschnittParameter parameter)
        {
            var strassenabschnitten = strassenabschnittService.GetEntitiesBy(GetErfassungsPeriod(parameter.ErfassungsPeriodId));

            return filtererFactory.CreateFilterer<Strassenabschnitt>(parameter).Filter(strassenabschnitten).Fetch(s => s.Belastungskategorie).ToArray()
                .OrderBy(s => s.Strassenname).ThenBy(s => s.Abschnittsnummer)
                .Select(CreatePo)
                .ToList();
        }

        protected override List<WiederbeschaffungswertUndWertverlustProStrassenabschnittPo> GetPresentationObjectListForGis(WiederbeschaffungswertUndWertverlustProStrassenabschnittParameter parameter)
        {
            var strassenabschnitten = strassenabschnittGISService.GetEntitiesBy(GetErfassungsPeriod(parameter.ErfassungsPeriodId));

            return filtererFactory.CreateFilterer<StrassenabschnittGIS>(parameter).Filter(strassenabschnitten).Fetch(s => s.Belastungskategorie).ToArray()
                .OrderBy(s => s.Strassenname)
                .Select(CreatePo)
                .ToList();
        }

        private WiederbeschaffungswertUndWertverlustProStrassenabschnittPo CreatePo(StrassenabschnittBase strassenabschnittBase)
        {
            var result = CreatePoFromEntityWithCopyingMatchingProperties(strassenabschnittBase);
            var wider = wiederbeschaffungswertKatalogService.GetWiederbeschaffungswertKatalogModel(strassenabschnittBase.Belastungskategorie, strassenabschnittBase.ErfassungsPeriod);
            result.BelastungskategorieBezeichnung = LocalizationService.GetLocalizedBelastungskategorieTyp(strassenabschnittBase.BelastungskategorieTyp);
            if (strassenabschnittBase.HasTrottoirInformation)
            {
                result.Wiederbeschaffungswert = strassenabschnittBase.FlaecheFahrbahn * wider.FlaecheFahrbahn 
                    + wider.FlaecheTrottoir * (strassenabschnittBase.FlaecheTrottoirLinks + strassenabschnittBase.FlaecheTrottoirRechts);
            }
            else
            {
                result.Wiederbeschaffungswert = strassenabschnittBase.GesamtFlaeche * wider.GesamtflaecheFahrbahn;
            }
            result.AlterungsbeiwertI = wider.AlterungsbeiwertI;
            result.AlterungsbeiwertII = wider.AlterungsbeiwertII;
            result.StrasseneigentuemerBezeichnung = LocalizationService.GetLocalizedEnum(strassenabschnittBase.Strasseneigentuemer);
            result.WertlustI = result.Wiederbeschaffungswert * result.AlterungsbeiwertI / 100;
            result.WertlustII = result.Wiederbeschaffungswert * result.AlterungsbeiwertII / 100;
            result.TrottoirBezeichnung = LocalizationService.GetLocalizedEnum(strassenabschnittBase.Trottoir);
            result.FlaecheFahrbahn = Math.Round(result.FlaecheFahrbahn ?? 0);
            return result;
        }
        public override bool AvailableInCurrentErfassungPeriod { get { return true; } }

        protected override void BuildFilterList(IFilterListBuilder<WiederbeschaffungswertUndWertverlustProStrassenabschnittParameter> builder)
        {
            base.BuildFilterList(builder);

            AddErfassungsPeriodFilterListItem(builder);
            builder.AddFilterListItem(p => p.Eigentuemer);
            builder.AddFilterListItem(p => p.Strassenname);
            builder.AddFilterListItem(p => p.Ortsbezeichnung);
        }

        protected override PaperType PaperType { get  { return PaperType.A3Landscape; } }
    }
}
