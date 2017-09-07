using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Entities.Summarisch;
using ASTRA.EMSG.Business.Infrastructure.MappingRules;
using ASTRA.EMSG.Business.Reports.EineListeVonKoordiniertenMassnahmen;
using ASTRA.EMSG.Business.Reports.EineListeVonMassnahmenGegliedertNachTeilsystemen;
using ASTRA.EMSG.Business.Reports.EineListeVonRealisiertenMassnahmenGeordnetNachJahren;
using ASTRA.EMSG.Business.Reports.ListeDerInspektionsrouten;
using ASTRA.EMSG.Business.Reports.MassnahmenvorschlagProZustandsabschnitt;
using ASTRA.EMSG.Business.Reports.MengeProBelastungskategorie;
using ASTRA.EMSG.Business.Reports.NochNichtInspizierteStrassenabschnitte;
using ASTRA.EMSG.Business.Reports.StrassenabschnitteListe;
using ASTRA.EMSG.Business.Reports.StrassenabschnitteListeOhneInspektionsroute;
using ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustProStrassenabschnitt;
using ASTRA.EMSG.Business.Reports.ZustandProZustandsabschnitt;
using ASTRA.EMSG.Common.ReflectionMapper;
using ASTRA.EMSG.Common.ReflectionMapper.Interfaces;

namespace ASTRA.EMSG.Business.ReflectionMappingConfiguration
{
    public interface IReportingMappingConfiguration : IMappingConfiguration
    {
    }

    public class ReportingMappingConfiguration : MappingConfiguration, IReportingMappingConfiguration
    {
        private readonly IMappingRule[] mappingRules;

        public ReportingMappingConfiguration(IIgnoreReadonlyPropertiesMappingRule ignoreReadonlyPropertiesMappingRule, IEntityToIdMappingRule entityToIdMappingRule, INullableToNonNullableMappingRule nullableToNonNullableMappingRule)
        {
            mappingRules = new IMappingRule[] { entityToIdMappingRule, nullableToNonNullableMappingRule, ignoreReadonlyPropertiesMappingRule };

            Register(new RuleDrivenReflectingMapper<NetzSummarischDetail, MengeProBelastungskategoriePo>(mappingRules));
            Register(new RuleDrivenReflectingMapper<StrassenabschnittBase, StrassenabschnitteListePo>(mappingRules));
            Register(new RuleDrivenReflectingMapper<StrassenabschnittBase, NochNichtInspizierteStrassenabschnittePo>(mappingRules));
            Register(new RuleDrivenReflectingMapper<StrassenabschnittBase, WiederbeschaffungswertUndWertverlustProStrassenabschnittPo>(mappingRules));
            Register(new RuleDrivenReflectingMapper<StrassenabschnittGIS, ListeDerInspektionsroutenPo>(mappingRules));
            Register(new RuleDrivenReflectingMapper<ZustandsabschnittBase, ZustandProZustandsabschnittPo>(mappingRules));
            Register(new RuleDrivenReflectingMapper<ZustandsabschnittBase, MassnahmenvorschlagProZustandsabschnittPo>(mappingRules));
            Register(new RuleDrivenReflectingMapper<StrassenabschnittGIS, StrassenabschnitteListeOhneInspektionsroutePo>(mappingRules));
            Register(new RuleDrivenReflectingMapper<KoordinierteMassnahmeGIS, EineListeVonKoordiniertenMassnahmenPo>(mappingRules));
            Register(new RuleDrivenReflectingMapper<MassnahmenvorschlagTeilsystemeGIS, EineListeVonMassnahmenGegliedertNachTeilsystemenPo>(mappingRules));
            var realisierteMassnahmeSummarsichSummarsichMapper = new RuleDrivenReflectingMapper<RealisierteMassnahmeSummarsich, EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischPo>(mappingRules);
            realisierteMassnahmeSummarsichSummarsichMapper.SetValueFrom(d => d.KostenFahrbahn, s => s.KostenFahrbahn.HasValue ? (decimal)s.KostenFahrbahn.Value : 0m);
            Register(realisierteMassnahmeSummarsichSummarsichMapper);
            Register(new RuleDrivenReflectingMapper<RealisierteMassnahme, EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischPo>(mappingRules));
            Register(new RuleDrivenReflectingMapper<RealisierteMassnahmeGIS, EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischPo>(mappingRules));
            var realisierteMassnahmeSummarsichGISMapper = new RuleDrivenReflectingMapper<RealisierteMassnahmeSummarsich, EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISPo>(mappingRules);
            realisierteMassnahmeSummarsichGISMapper.SetValueFrom(d => d.KostenFahrbahn, s => s.KostenFahrbahn.HasValue ? (decimal)s.KostenFahrbahn.Value : 0m);
            Register(realisierteMassnahmeSummarsichGISMapper);
            Register(new RuleDrivenReflectingMapper<RealisierteMassnahme, EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISPo>(mappingRules));
            Register(new RuleDrivenReflectingMapper<RealisierteMassnahmeGIS, EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISPo>(mappingRules));
            var realisierteMassnahmeSummarsichMapper = new RuleDrivenReflectingMapper<RealisierteMassnahmeSummarsich, EineListeVonRealisiertenMassnahmenGeordnetNachJahrenPo>(mappingRules);
            realisierteMassnahmeSummarsichMapper.SetValueFrom(d => d.KostenFahrbahn, s => s.KostenFahrbahn.HasValue ? (decimal)s.KostenFahrbahn.Value : 0m);
            Register(realisierteMassnahmeSummarsichMapper);
            Register(new RuleDrivenReflectingMapper<RealisierteMassnahme, EineListeVonRealisiertenMassnahmenGeordnetNachJahrenPo>(mappingRules));
            Register(new RuleDrivenReflectingMapper<RealisierteMassnahmeGIS, EineListeVonRealisiertenMassnahmenGeordnetNachJahrenPo>(mappingRules));
        }
    }
}