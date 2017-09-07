using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Entities.Summarisch;
using ASTRA.EMSG.Business.Infrastructure.MappingRules;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.Models.Katalogs;
using ASTRA.EMSG.Business.Models.Reports;
using ASTRA.EMSG.Business.Models.Strassennamen;
using ASTRA.EMSG.Business.Models.Summarisch;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.ReflectionMapper;
using ASTRA.EMSG.Common.ReflectionMapper.Interfaces;

namespace ASTRA.EMSG.Business.ReflectionMappingConfiguration
{
    public interface IEntityServiceMappingConfiguration : IMappingConfiguration
    {
    }

    public class EntityServiceMappingConfiguration : MappingConfiguration, IEntityServiceMappingConfiguration
    {
        private readonly IMappingRule[] mappingRules;

        public EntityServiceMappingConfiguration(IIgnoreReadonlyPropertiesMappingRule ignoreReadonlyPropertiesMappingRule, IIdToEntityMappingRule idToEntityMappingRule,
            IEntityToIdMappingRule entityToIdMappingRule, INullableToNonNullableMappingRule nullableToNonNullableMappingRule)
        {
            mappingRules = new IMappingRule[] { nullableToNonNullableMappingRule, idToEntityMappingRule, entityToIdMappingRule, ignoreReadonlyPropertiesMappingRule };

            RegisterTwoDirectionMapping<NetzSummarischModel, NetzSummarisch>();
            RegisterTwoDirectionMapping<NetzSummarischDetailModel, NetzSummarischDetail>();
            RegisterStrassenabschnittMappings();
            RegisterTwoDirectionMapping<StrassenabschnittImportModel, Strassenabschnitt>();
            Register(new RuleDrivenReflectingMapper<StrassenabschnittImportModel, StrassenabschnittImportOverviewModel>(mappingRules));
            RegisterTwoDirectionMapping<StrassenabschnittOverviewModel, Strassenabschnitt>();
            RegisterTwoDirectionMapping<StrassenabschnittOverviewGISModel, StrassenabschnittGIS>();
            RegisterTwoDirectionMapping<AchsenReferenzModel, AchsenReferenz>();
            RegisterTwoDirectionMapping<AchsenSegmentModel, AchsenSegment>();
            RegisterTwoDirectionMapping<ReferenzGruppeModel, ReferenzGruppe>();
            RegisterTwoDirectionMapping<ZustandsabschnittModel, Zustandsabschnitt>();
            RegisterTwoDirectionMapping<ZustandsabschnittImportModel, Zustandsabschnitt>();
            Register(new RuleDrivenReflectingMapper<ZustandsabschnittImportModel, ZustandsabschnittImportOverviewModel>(mappingRules));
            RegisterTwoDirectionMapping<ZustandsabschnittOverviewModel, Zustandsabschnitt>();
            RegisterTwoDirectionMapping<ZustandsabschnittGISModel, ZustandsabschnittGIS>();
            RegisterTwoDirectionMapping<ZustandsabschnittOverviewGISModel, ZustandsabschnittGIS>();
            RegisterTwoDirectionMapping<SchadengruppeModel, Schadengruppe>();
            RegisterTwoDirectionMapping<SchadendetailModel, Schadendetail>();
            RegisterTwoDirectionMapping<ErfassungsPeriodModel, ErfassungsPeriod>();
            RegisterTwoDirectionMapping<EreignisLogOverviewModel, EreignisLog>();
            RegisterTwoDirectionMapping<MandantModel, Mandant>();
            RegisterTwoDirectionMapping<MandantLogoModel, MandantLogo>();
            RegisterTwoDirectionMapping<MassnahmenvorschlagKatalogModel, MassnahmenvorschlagKatalog>();
            RegisterTwoDirectionMapping<MassnahmenvorschlagKatalogEditModel, MassnahmenvorschlagKatalog>();
            RegisterTwoDirectionMapping<MassnahmenvorschlagKatalogEditModel, GlobalMassnahmenvorschlagKatalog>();
            RegisterTwoDirectionMapping<WiederbeschaffungswertKatalogModel, WiederbeschaffungswertKatalog>();
            RegisterTwoDirectionMapping<WiederbeschaffungswertKatalogEditModel, WiederbeschaffungswertKatalog>();
            RegisterTwoDirectionMapping<WiederbeschaffungswertKatalogEditModel, GlobalWiederbeschaffungswertKatalog>();
            RegisterTwoDirectionMapping<RealisierteMassnahmeSummarsichModel, RealisierteMassnahmeSummarsich>();
            RegisterTwoDirectionMapping<RealisierteMassnahmeSummarsichOverviewModel, RealisierteMassnahmeSummarsich>();
            RegisterTwoDirectionMapping<RealisierteMassnahmeModel, RealisierteMassnahme>();
            RegisterTwoDirectionMapping<RealisierteMassnahmeOverviewModel, RealisierteMassnahme>();
            RegisterTwoDirectionMapping<KoordinierteMassnahmeGISOverviewModel, KoordinierteMassnahmeGIS>();
            var koordinierteMassnahmeGISModel = new RuleDrivenReflectingMapper<KoordinierteMassnahmeGISModel, KoordinierteMassnahmeGIS>(mappingRules);
            koordinierteMassnahmeGISModel.SetValueFrom(d => d.BeteiligteSysteme, s => s.BeteiligteSysteme);
            Register(new EnumMapper<TeilsystemTyp, TeilsystemTyp>());
            Register(koordinierteMassnahmeGISModel);
            RegisterTwoDirectionMapping<RealisierteMassnahmeGISOverviewModel, RealisierteMassnahmeGIS>();
            var realisierteMassnahmeGISModel = new RuleDrivenReflectingMapper<RealisierteMassnahmeGISModel, RealisierteMassnahmeGIS>(mappingRules);
            realisierteMassnahmeGISModel.SetValueFrom(d => d.BeteiligteSysteme, s => s.BeteiligteSysteme);
            Register(realisierteMassnahmeGISModel);
            Register(new RuleDrivenReflectingMapper<KoordinierteMassnahmeGIS, KoordinierteMassnahmeGISModel>(mappingRules));
            Register(new RuleDrivenReflectingMapper<RealisierteMassnahmeGIS, RealisierteMassnahmeGISModel>(mappingRules));
            RegisterTwoDirectionMapping<MassnahmenvorschlagTeilsystemeGISModel, MassnahmenvorschlagTeilsystemeGIS>();
            RegisterTwoDirectionMapping<MassnahmenvorschlagTeilsystemeGISOverviewModel, MassnahmenvorschlagTeilsystemeGIS>();
            RegisterTwoDirectionMapping<CheckOutsGISModel, CheckOutsGIS>();
            RegisterTwoDirectionMapping<InspektionsRouteGISModel, InspektionsRouteGIS>();
            RegisterTwoDirectionMapping<InspektionsRouteGISOverviewModel, InspektionsRouteGIS>();
            RegisterTwoDirectionMapping<InspektionsRtStrAbschnitteModel, InspektionsRtStrAbschnitte>();
            RegisterBelastungskategorieMapping();
            Register(new EnumMapper<BelagsTyp, BelagsTyp>());
            RegisterTwoDirectionMapping<InspektionsRouteStatusverlaufModel, InspektionsRouteStatusverlauf>();
            RegisterTwoDirectionMapping<AusgefuellteErfassungsformulareFuerOberflaechenschaeden, Zustandsabschnitt>();
            RegisterTwoDirectionMapping<AusgefuellteErfassungsformulareFuerOberflaechenschaeden, ZustandsabschnittGIS>();
            RegisterTwoDirectionMapping<KenngroessenFruehererJahreDetailModel, KenngroessenFruehererJahreDetail>();
            RegisterTwoDirectionMapping<KenngroessenFruehererJahreDetailOverviewModel, KenngroessenFruehererJahreDetail>();
            RegisterTwoDirectionMapping<KenngroessenFruehererJahreModel, KenngroessenFruehererJahre>();
            RegisterTwoDirectionMapping<KenngroessenFruehererJahreOverviewModel, KenngroessenFruehererJahre>();
            RegisterTwoDirectionMapping<AchsenImportModel, AchsenUpdateLog>();
            RegisterTwoDirectionMapping<MandantDetailsModel, MandantDetails>();
            RegisterTwoDirectionMapping<GemeindeKatalogModel, GemeindeKatalog>();
            RegisterTwoDirectionMapping<OeffentlicheVerkehrsmittelKatalogModel, OeffentlicheVerkehrsmittelKatalog>();

            RegisterGlobalToMandantSpecificMapping(ignoreReadonlyPropertiesMappingRule);

            RegisterCopyMappers(ignoreReadonlyPropertiesMappingRule);

            RegisterBenchmarkingDataCopyMappers(ignoreReadonlyPropertiesMappingRule);
        }

        private void RegisterBelastungskategorieMapping()
        {
            var belastungskategorieMapper = new RuleDrivenReflectingMapper<Belastungskategorie, BelastungskategorieModel>(mappingRules);
            belastungskategorieMapper.SetValueFrom(m => m.AllowedBelagList, e => e.AllowedBelagList);
            Register(belastungskategorieMapper);
            Register(new RuleDrivenReflectingMapper<BelastungskategorieModel, Belastungskategorie>(mappingRules));
        }

        private void RegisterGlobalToMandantSpecificMapping(IIgnoreReadonlyPropertiesMappingRule ignoreReadonlyPropertiesMappingRule)
        {
            var globalWiederbeschaffungswertKatalogMapper = new RuleDrivenReflectingMapper<GlobalWiederbeschaffungswertKatalog, WiederbeschaffungswertKatalog>(new IMappingRule[] { ignoreReadonlyPropertiesMappingRule });
            globalWiederbeschaffungswertKatalogMapper.Ignore(wbk => wbk.Id);
            Register(globalWiederbeschaffungswertKatalogMapper);

            var massnahmenvorschlagKatalogMapper = new RuleDrivenReflectingMapper<GlobalMassnahmenvorschlagKatalog, MassnahmenvorschlagKatalog>(new IMappingRule[] { ignoreReadonlyPropertiesMappingRule });
            massnahmenvorschlagKatalogMapper.Ignore(rmk => rmk.Id);
            Register(massnahmenvorschlagKatalogMapper);
        }

        private void RegisterStrassenabschnittMappings()
        {
            Register(new RuleDrivenReflectingMapper<StrassenabschnittModel, Strassenabschnitt>(mappingRules));
            var strassenabschnittToModelMapper = new RuleDrivenReflectingMapper<Strassenabschnitt, StrassenabschnittModel>(mappingRules);
            strassenabschnittToModelMapper.SetValueFrom(d => d.FlaecheFahrbahn, s => s.FlaecheFahrbahn == 0 ? (object)null : s.FlaecheFahrbahn);
            strassenabschnittToModelMapper.SetValueFrom(d => d.FlaecheTrottoir, s => s.FlaecheTrottoir == 0 ? (object)null : s.FlaecheTrottoir);
            strassenabschnittToModelMapper.SetValueFrom(d => d.FlaecheTrottoirLinks, s => s.FlaecheTrottoirLinks == 0 ? (object)null : s.FlaecheTrottoirLinks);
            strassenabschnittToModelMapper.SetValueFrom(d => d.FlaecheTrottoirRechts, s => s.FlaecheTrottoirRechts == 0 ? (object)null : s.FlaecheTrottoirRechts);
            Register(strassenabschnittToModelMapper);

            Register(new RuleDrivenReflectingMapper<StrassenabschnittGISModel, StrassenabschnittGIS>(mappingRules));
            var strassenabschnittGisToModelMapper = new RuleDrivenReflectingMapper<StrassenabschnittGIS, StrassenabschnittGISModel>(mappingRules);
            strassenabschnittGisToModelMapper.SetValueFrom(d => d.FlaecheFahrbahn, s => s.FlaecheFahrbahn == 0 ? (object)null : s.FlaecheFahrbahn);
            strassenabschnittGisToModelMapper.SetValueFrom(d => d.FlaecheTrottoir, s => s.FlaecheTrottoir == 0 ? (object)null : s.FlaecheTrottoir);
            strassenabschnittGisToModelMapper.SetValueFrom(d => d.FlaecheTrottoirLinks, s => s.FlaecheTrottoirLinks == 0 ? (object)null : s.FlaecheTrottoirLinks);
            strassenabschnittGisToModelMapper.SetValueFrom(d => d.FlaecheTrottoirRechts, s => s.FlaecheTrottoirRechts == 0 ? (object)null : s.FlaecheTrottoirRechts);
            Register(strassenabschnittGisToModelMapper);
        }

        private void RegisterCopyMappers(IIgnoreReadonlyPropertiesMappingRule ignoreReadonlyPropertiesMappingRule)
        {
            var achsenMapper = new RuleDrivenReflectingMapper<Achse, Achse>(ignoreReadonlyPropertiesMappingRule);
            achsenMapper.Ignore(m => m.AchsenSegmente);
            achsenMapper.Ignore(m => m.Mandant);
            achsenMapper.Ignore(m => m.ErfassungsPeriod);
            achsenMapper.Ignore(m => m.Id);
            Register(achsenMapper);

            var sektorMapper = new RuleDrivenReflectingMapper<Sektor, Sektor>(ignoreReadonlyPropertiesMappingRule);
            sektorMapper.Ignore(m => m.Id);
            Register(sektorMapper);

            var achsensegmentMapper = new RuleDrivenReflectingMapper<AchsenSegment, AchsenSegment>(ignoreReadonlyPropertiesMappingRule);
            achsensegmentMapper.Ignore(m => m.Sektoren);
            achsensegmentMapper.Ignore(m => m.AchsenReferenzen);
            achsensegmentMapper.Ignore(m => m.Mandant);
            achsensegmentMapper.Ignore(m => m.ErfassungsPeriod);
            achsensegmentMapper.Ignore(m => m.Id);
            Register(achsensegmentMapper);

            var strassenabschnittMapper = new RuleDrivenReflectingMapper<Strassenabschnitt, Strassenabschnitt>(ignoreReadonlyPropertiesMappingRule);
            strassenabschnittMapper.Ignore(m => m.Zustandsabschnitten);
            strassenabschnittMapper.Ignore(m => m.Mandant);
            strassenabschnittMapper.Ignore(m => m.ErfassungsPeriod);
            strassenabschnittMapper.Ignore(m => m.Id);
            strassenabschnittMapper.SetValueFrom(d => d.Belastungskategorie, s => s.Belastungskategorie);
            Register(strassenabschnittMapper);

            var strassenabschnittGISMapper = new RuleDrivenReflectingMapper<StrassenabschnittGIS, StrassenabschnittGIS>(ignoreReadonlyPropertiesMappingRule);
            strassenabschnittGISMapper.Ignore(m => m.ReferenzGruppe);
            strassenabschnittGISMapper.Ignore(m => m.Zustandsabschnitten);
            strassenabschnittGISMapper.Ignore(m => m.Mandant);
            strassenabschnittGISMapper.Ignore(m => m.ErfassungsPeriod);
            strassenabschnittGISMapper.Ignore(m => m.Id);
            strassenabschnittGISMapper.Ignore(m => m.InspektionsRtStrAbschnitte);
            strassenabschnittGISMapper.Ignore(m => m.IsLocked);
            strassenabschnittGISMapper.SetValueFrom(d => d.Belastungskategorie, s => s.Belastungskategorie);
            Register(strassenabschnittGISMapper);

            var achsenreferenzMapper = new RuleDrivenReflectingMapper<AchsenReferenz, AchsenReferenz>(ignoreReadonlyPropertiesMappingRule);
            achsenreferenzMapper.Ignore(m => m.Id);
            achsenreferenzMapper.Ignore(m => m.Erfassungsperiod);
            achsenreferenzMapper.Ignore(m => m.Mandandt);
            achsenreferenzMapper.Ignore(m => m.ReferenzGruppe);
            Register(achsenreferenzMapper);

            var zustandsabschnittMapper = new RuleDrivenReflectingMapper<Zustandsabschnitt, Zustandsabschnitt>(ignoreReadonlyPropertiesMappingRule);
            zustandsabschnittMapper.Ignore(m => m.Id);
            zustandsabschnittMapper.Ignore(m => m.Strassenabschnitt);
            zustandsabschnittMapper.Ignore(m => m.Schadendetails);
            zustandsabschnittMapper.Ignore(m => m.Schadengruppen);
            zustandsabschnittMapper.Ignore(m => m.MassnahmenvorschlagFahrbahn);
            zustandsabschnittMapper.Ignore(m => m.MassnahmenvorschlagTrottoirLinks);
            zustandsabschnittMapper.Ignore(m => m.MassnahmenvorschlagTrottoirRechts);
            Register(zustandsabschnittMapper);

            var zustandsabschnittGISMapper = new RuleDrivenReflectingMapper<ZustandsabschnittGIS, ZustandsabschnittGIS>(ignoreReadonlyPropertiesMappingRule);
            zustandsabschnittGISMapper.Ignore(m => m.Id);
            zustandsabschnittGISMapper.Ignore(m => m.StrassenabschnittGIS);
            zustandsabschnittGISMapper.Ignore(m => m.Schadendetails);
            zustandsabschnittGISMapper.Ignore(m => m.Schadengruppen);
            zustandsabschnittGISMapper.Ignore(m => m.ReferenzGruppe);
            zustandsabschnittGISMapper.Ignore(m => m.MassnahmenvorschlagFahrbahn);
            zustandsabschnittGISMapper.Ignore(m => m.MassnahmenvorschlagTrottoirLinks);
            zustandsabschnittGISMapper.Ignore(m => m.MassnahmenvorschlagTrottoirRechts);
            Register(zustandsabschnittGISMapper);

            var schadengruppeMapper = new RuleDrivenReflectingMapper<Schadengruppe, Schadengruppe>(ignoreReadonlyPropertiesMappingRule);
            schadengruppeMapper.Ignore(m => m.Id);
            Register(schadengruppeMapper);

            var schadendetailMapper = new RuleDrivenReflectingMapper<Schadendetail, Schadendetail>(ignoreReadonlyPropertiesMappingRule);
            schadendetailMapper.Ignore(m => m.Id);
            Register(schadendetailMapper);

            var wiederbeschaffungswertKatalogMapper = new RuleDrivenReflectingMapper<WiederbeschaffungswertKatalog, WiederbeschaffungswertKatalog>(ignoreReadonlyPropertiesMappingRule);
            wiederbeschaffungswertKatalogMapper.Ignore(m => m.Id);
            wiederbeschaffungswertKatalogMapper.Ignore(m => m.Mandant);
            wiederbeschaffungswertKatalogMapper.Ignore(m => m.ErfassungsPeriod);
            wiederbeschaffungswertKatalogMapper.SetValueFrom(d => d.Belastungskategorie, s => s.Belastungskategorie);
            Register(wiederbeschaffungswertKatalogMapper);

            var massnahmenvorschlagKatalogMapper = new RuleDrivenReflectingMapper<MassnahmenvorschlagKatalog, MassnahmenvorschlagKatalog>(ignoreReadonlyPropertiesMappingRule);
            massnahmenvorschlagKatalogMapper.Ignore(m => m.Id);
            massnahmenvorschlagKatalogMapper.Ignore(m => m.Mandant);
            massnahmenvorschlagKatalogMapper.Ignore(m => m.ErfassungsPeriod);
            massnahmenvorschlagKatalogMapper.SetValueFrom(d => d.Belastungskategorie, s => s.Belastungskategorie);
            Register(massnahmenvorschlagKatalogMapper);
            
            var mandantDetailsMapper = new RuleDrivenReflectingMapper<MandantDetails, MandantDetails>(ignoreReadonlyPropertiesMappingRule);
            mandantDetailsMapper.Ignore(m => m.Id);
            mandantDetailsMapper.Ignore(m => m.NetzLaenge);
            mandantDetailsMapper.Ignore(m => m.Mandant);
            mandantDetailsMapper.Ignore(m => m.ErfassungsPeriod);
            Register(mandantDetailsMapper);
        }

        private void RegisterBenchmarkingDataCopyMappers(IIgnoreReadonlyPropertiesMappingRule ignoreReadonlyPropertiesMappingRule)
        {
            var benchmarkingDataMapper = new RuleDrivenReflectingMapper<BenchmarkingData, BenchmarkingData>(ignoreReadonlyPropertiesMappingRule);
            benchmarkingDataMapper.Ignore(m => m.Id);
            benchmarkingDataMapper.Ignore(m => m.Mandant);
            benchmarkingDataMapper.Ignore(m => m.ErfassungsPeriod);
            benchmarkingDataMapper.Ignore(m => m.CalculatedAt);
            benchmarkingDataMapper.Ignore(m => m.BenchmarkingDataDetails);
            Register(benchmarkingDataMapper);

            var benchmarkingDataDetailMapper = new RuleDrivenReflectingMapper<BenchmarkingDataDetail, BenchmarkingDataDetail>(ignoreReadonlyPropertiesMappingRule);
            benchmarkingDataDetailMapper.Ignore(m => m.Id);
            benchmarkingDataDetailMapper.Ignore(m => m.Belastungskategorie);
            benchmarkingDataDetailMapper.Ignore(m => m.BenchmarkingData);
            Register(benchmarkingDataDetailMapper);
        }

        private void RegisterTwoDirectionMapping<TModel, TEntity>()
            where TModel : IModel
            where TEntity : IEntity
        {
            Register(new RuleDrivenReflectingMapper<TModel, TEntity>(mappingRules));
            Register(new RuleDrivenReflectingMapper<TEntity, TModel>(mappingRules));
        }
    }
}
