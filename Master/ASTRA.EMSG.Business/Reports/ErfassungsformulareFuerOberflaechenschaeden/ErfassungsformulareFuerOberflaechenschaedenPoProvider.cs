using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.AusgefuellteErfassungsformulareFuerOberflaechenschaeden;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Services.SchadenMetadaten;

namespace ASTRA.EMSG.Business.Reports.ErfassungsformulareFuerOberflaechenschaeden
{
    public interface IErfassungsformulareFuerOberflaechenschaedenPoProvider
    {
    }

    [ReportInfo(AuswertungTyp.W3_1)]
    public class ErfassungsformulareFuerOberflaechenschaedenPoProvider : EmsgModeDependentPoProviderBase<ErfassungsformulareFuerOberflaechenschaedenParameter, AusgefuellteErfassungsformulareFuerOberflaechenschaedenPo>, IErfassungsformulareFuerOberflaechenschaedenPoProvider
    {
        private readonly ILocalizationService localizationService;
        private readonly ISchadenMetadatenService schadenMetadatenService;

        public ErfassungsformulareFuerOberflaechenschaedenPoProvider(ILocalizationService localizationService, ISchadenMetadatenService schadenMetadatenService)
        {
            this.localizationService = localizationService;
            this.schadenMetadatenService = schadenMetadatenService;
        }

        public override string ReportDefinitionResourceName
        {
            get
            {
                return GetRdlcResourceName(typeof(AusgefuellteErfassungsformulareFuerOberflaechenschaedenPoProvider));
            }
        }

        protected override List<AusgefuellteErfassungsformulareFuerOberflaechenschaedenPo> GetPresentationObjectListForSummarisch(ErfassungsformulareFuerOberflaechenschaedenParameter parameter)
        {
            return GetPos(parameter.BelagsTyp);
        }

        protected override List<AusgefuellteErfassungsformulareFuerOberflaechenschaedenPo> GetPresentationObjectListForTabellarisch(ErfassungsformulareFuerOberflaechenschaedenParameter parameter)
        {
            return GetPos(parameter.BelagsTyp);
        }

        protected override List<AusgefuellteErfassungsformulareFuerOberflaechenschaedenPo> GetPresentationObjectListForGis(ErfassungsformulareFuerOberflaechenschaedenParameter parameter)
        {
            return GetPos(parameter.BelagsTyp);
        }

        private List<AusgefuellteErfassungsformulareFuerOberflaechenschaedenPo> GetPos(BelagsTyp belagsTyp)
        {
            return schadenMetadatenService
                .GetSchadengruppeMetadaten(belagsTyp)
                .SelectMany(sgm => sgm.Schadendetails.Select(sdm => CreatePo(sgm, sdm)))
                .OrderBy(po => po.SchadengruppeReihung)
                .ThenBy(po => po.SchadendetailReihung)
                .ToList();
        }

        private AusgefuellteErfassungsformulareFuerOberflaechenschaedenPo CreatePo(SchadengruppeMetadaten sgm, SchadendetailMetadaten sdm)
        {
            return new AusgefuellteErfassungsformulareFuerOberflaechenschaedenPo
                       {
                           Strassenname = string.Empty,
                           ZustandsabschnittBezeichnungVon = string.Empty,
                           ZustandsabschnittBezeichnungBis = string.Empty,

                           Laenge = null,
                           FlaecheFahrbahn = null,
                           AufnahmeDatum = null,
                           Aufnahmeteam = string.Empty,
                           Wetter = null,
                           WetterBezeichnung = string.Empty,
                           Bemerkung = string.Empty,

                           SchadengruppeTyp = sgm.SchadengruppeTyp,
                           SchadengruppeBezeichnung = localizationService.GetLocalizedEnum(sgm.SchadengruppeTyp),
                           SchadengruppeReihung = sgm.Reihung,

                           SchadendetailTyp = sdm.SchadendetailTyp,
                           SchadendetailBezeichnung = localizationService.GetLocalizedEnum(sdm.SchadendetailTyp),
                           SchadendetailReihung = sdm.Reihung,

                           Gewicht = sgm.Gewicht,
                           Bewertung = null,
                           Matrix = null,

                           SchadenschwereS1 = string.Empty,
                           SchadenschwereS2 = string.Empty,
                           SchadenschwereS3 = string.Empty,

                           SchadenausmassA0 = string.Empty,
                           SchadenausmassA1 = string.Empty,
                           SchadenausmassA2 = string.Empty,
                           SchadenausmassA3 = string.Empty
                       };
        }

        protected override void SetReportParameters(ErfassungsformulareFuerOberflaechenschaedenParameter parameter)
        {
            switch (parameter.BelagsTyp)
            {
                case BelagsTyp.Asphalt:
                    ReportFileNamePostfix = "Asphaltbelag";
                    break;
                case BelagsTyp.Beton:
                    ReportFileNamePostfix = "Betonbelag";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("parameter.BelagsTyp", string.Format("Belag {0} is not supported", parameter.BelagsTyp));
            }

            base.SetReportParameters(parameter);

            AddReportParameter("IsEmpty", true);
            AddReportParameter("TableHeaderBackgroundColor", "#DCE6F1");
            AddReportParameter("TableFooterBackgroundColor", "#F2F2F2");
            AddReportParameter("TableAlternatingRowBackgroungColor", "#EBF5F5");
            AddReportParameter("TableHorizontalBorderColor", "#587BA5");
            AddReportParameter("TableVerticalBorderColor", "#C2D3E8");
        }

        public override bool AvailableInCurrentErfassungPeriod { get { return true; } }
    }
}
