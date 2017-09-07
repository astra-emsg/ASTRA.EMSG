using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Summarisch;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Reporting;
using System.Linq;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.EntityServices.Strassennamen;
using ASTRA.EMSG.Business.Services.EntityServices.Summarisch;
using ASTRA.EMSG.Common.Enums;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Reports.MengeProBelastungskategorie
{
    public interface IMengeProBelastungskategoriePoProvider : IPoProvider
    {
    }
    [ReportInfo(AuswertungTyp.W1_7)]
    public class MengeProBelastungskategoriePoProvider : EmsgTablePoProviderBase<MengeProBelastungskategorieParameter, MengeProBelastungskategoriePo>, IMengeProBelastungskategoriePoProvider
    {
        private readonly INetzSummarischDetailService netzSummarischDetailService;
        private readonly IStrassenabschnittService strassenabschnittService;
        private readonly IStrassenabschnittGISService strassenabschnittGISService;
        private readonly IBelastungskategorieService belastungskategorieService;

        public MengeProBelastungskategoriePoProvider(
            INetzSummarischDetailService netzSummarischDetailService,
            IStrassenabschnittService strassenabschnittService,
            IStrassenabschnittGISService strassenabschnittGISService,
            IBelastungskategorieService belastungskategorieService)
        {
            this.netzSummarischDetailService = netzSummarischDetailService;
            this.strassenabschnittService = strassenabschnittService;
            this.strassenabschnittGISService = strassenabschnittGISService;
            this.belastungskategorieService = belastungskategorieService;
        }

        protected override List<MengeProBelastungskategoriePo> GetPresentationObjectListForSummarisch(MengeProBelastungskategorieParameter parameter)
        {
            var netzSummarischDetails = netzSummarischDetailService.GetEntitiesBy(GetErfassungsPeriod(parameter.ErfassungsPeriodId)).Fetch(nsd => nsd.Belastungskategorie);
            return netzSummarischDetails.Select(CreatePo).OrderBy(m => m.BelastungskategorieReihenfolge).ToList();
        }

        protected override List<MengeProBelastungskategoriePo> GetPresentationObjectListForTabellarisch(MengeProBelastungskategorieParameter parameter)
        {
            var strassenabschnitten = FilterStrassenabschnitten(strassenabschnittService.GetEntitiesBy(GetErfassungsPeriod(parameter.ErfassungsPeriodId)), parameter).ToList();
            return GetMengeProBelastungskategoriePos(strassenabschnitten);
        }

        protected override List<MengeProBelastungskategoriePo> GetPresentationObjectListForGis(MengeProBelastungskategorieParameter parameter)
        {
            var strassenabschnitten = FilterStrassenabschnitten(strassenabschnittGISService.GetEntitiesBy(GetErfassungsPeriod(parameter.ErfassungsPeriodId)), parameter).ToList();
            return GetMengeProBelastungskategoriePos(strassenabschnitten);
        }

        private IQueryable<StrassenabschnittBase> FilterStrassenabschnitten(IQueryable<StrassenabschnittBase> strassenabschnittBases, MengeProBelastungskategorieParameter parameter)
        {
            if (parameter.Eigentuemer.HasValue)
                strassenabschnittBases = strassenabschnittBases.Where(s => s.Strasseneigentuemer == parameter.Eigentuemer.Value);

            return strassenabschnittBases.Fetch(sa => sa.Belastungskategorie);
        }

        private List<MengeProBelastungskategoriePo> GetMengeProBelastungskategoriePos(List<StrassenabschnittBase> strassenabschnittBaseList)
        {
            List<MengeProBelastungskategoriePo> mengeProBelastungskategoriePos = strassenabschnittBaseList.GroupBy(s => s.Belastungskategorie)
                .Select(g => new MengeProBelastungskategoriePo
                                 {
                                     BelastungskategorieKurzBezeichnung = LocalizationService.GetLocalizedBelastungskategorieTyp(g.Key.Typ),
                                     BelastungskategorieTyp = g.Key.Typ,
                                     BelastungskategorieReihenfolge = g.Key.Reihenfolge,
                                     Fahrbahnflaeche = (int)Math.Round(g.Sum(s => s.FlaecheFahrbahn)),
                                     Trottoirflaeche = (int)Math.Round(g.Sum(s => s.FlaecheTrottoir)),
                                 }).ToList();

            foreach (var bk in belastungskategorieService.AllBelastungskategorieModel)
                if(mengeProBelastungskategoriePos.All(po => po.BelastungskategorieTyp != bk.Typ))
                    mengeProBelastungskategoriePos.Add(new MengeProBelastungskategoriePo { BelastungskategorieTyp = bk.Typ, BelastungskategorieKurzBezeichnung = LocalizationService.GetLocalizedBelastungskategorieTyp(bk.Typ), BelastungskategorieReihenfolge = bk.Reihenfolge });

            return mengeProBelastungskategoriePos.OrderBy(po => po.BelastungskategorieReihenfolge).ToList();
        }

        public override bool AvailableInCurrentErfassungPeriod { get { return true; } }

        protected override void SetReportParameters(MengeProBelastungskategorieParameter parameter)
        {
            base.SetReportParameters(parameter);
            
            var netzErfassungsmodus = GetNetzErfassungsmodus(parameter);
            
            AddReportParameter("IsSummarischeModus", netzErfassungsmodus == NetzErfassungsmodus.Summarisch);
        }

        private MengeProBelastungskategoriePo CreatePo(NetzSummarischDetail netzSummarischDetail)
        {
            var po = CreatePoFromEntityWithCopyingMatchingProperties(netzSummarischDetail);
            po.BelastungskategorieKurzBezeichnung = LocalizationService.GetLocalizedBelastungskategorieTyp(netzSummarischDetail.BelastungskategorieTyp);
            po.BelastungskategorieReihenfolge = netzSummarischDetail.Belastungskategorie.Reihenfolge;
            return po;
        }

        protected override void BuildFilterList(IFilterListBuilder<MengeProBelastungskategorieParameter> builder)
        {
            base.BuildFilterList(builder);
            AddErfassungsPeriodFilterListItem(builder);
            builder.AddFilterListItem(p => p.Eigentuemer);
        }

        protected override PaperType PaperType { get { return PaperType.A4Portrait; } }
    }
}
