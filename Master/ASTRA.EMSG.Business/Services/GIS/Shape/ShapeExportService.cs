using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Services.Historization;
using NHibernate.Criterion;
using NHibernate;
using ASTRA.EMSG.Business.Entities.GIS;
using NetTopologySuite.Features;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using System.IO;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Models.Katalogs;
using ASTRA.EMSG.Common;
using NetTopologySuite.IO;
using GeoAPI.Geometries;
using ASTRA.EMSG.Common.Utils;

namespace ASTRA.EMSG.Business.Services.GIS.Shape
{
    public interface IShapeExportService : IService
    {
        Stream Export(ErfassungsPeriod periode, ShapeExportType type);
    }
    public class ShapeExportService: IShapeExportService
    {
        private readonly ITransactionScopeProvider transactionScopeProvider;
        private readonly IHistorizationService historizationService;
        private readonly IServerConfigurationProvider serverConfigurationProvider;
        private readonly IShpShxSerializeService shpShxSerializeService;
        private readonly IWiederbeschaffungswertKatalogService wiederbeschaffungswertKatalogService;
        private readonly int batchSize = 500;

        private const int DoubleLength = 18;
        private const int DoubleDecimals = 8;
        private const int IntLength = 10;
        private const int IntDecimals = 0;
        private const int StringLength = 254;
        private const int StringDecimals = 0;
        private const int BoolLength = 1;
        private const int BoolDecimals = 0;
        private const int DateLength = 8;
        private const int DateDecimals = 0;


        public ShapeExportService(ITransactionScopeProvider transactionScopeProvider, 
            IHistorizationService historizationService, 
            IServerConfigurationProvider serverConfigurationProvider, 
            IShpShxSerializeService shpShxSerializeService,
            IWiederbeschaffungswertKatalogService wiederbeschaffungswertKatalogService)
        {
            this.wiederbeschaffungswertKatalogService = wiederbeschaffungswertKatalogService;
            this.shpShxSerializeService = shpShxSerializeService;
            this.serverConfigurationProvider = serverConfigurationProvider;
            this.transactionScopeProvider = transactionScopeProvider;
            this.historizationService = historizationService;
        }
        public Stream Export(ErfassungsPeriod periode, ShapeExportType type)
        {            
           

            switch (type)
            {
                case ShapeExportType.Zustandsabschnitt:
                    return this.ExportZabs("Zustandsabschnitte" + DateTime.Now.Date.ToString("yyyyMMdd"), periode);
                case ShapeExportType.Strassenabschnitt:
                    return this.ExportStrabs("Strassenabschnitte" + DateTime.Now.Date.ToString("yyyyMMdd"), periode);
                case ShapeExportType.Trottoir:
                    return this.ExportTrottoir("Trottoir" + DateTime.Now.Date.ToString("yyyyMMdd"), periode);
                default:
                    throw new NotImplementedException();
            }
        }
        public Stream ExportStrabs(string fileName, ErfassungsPeriod periode)
        {
            //IStatelessSession session = transactionScopeProvider.CurrentTransactionScope.Session.SessionFactory.OpenStatelessSession();
            ISession session = transactionScopeProvider.CurrentTransactionScope.Session;
            IQueryOver<StrassenabschnittGIS> query = session.QueryOver<StrassenabschnittGIS>().Where(s => s.ErfassungsPeriod.Id == periode.Id)
                .OrderBy(z => z.Id).Asc
                .Fetch(s => s.Belastungskategorie).Eager();
            //ICriteria crit = session.CreateCriteria<StrassenabschnittGIS>().SetFetchMode(ExpressionHelper.GetPropertyName<StrassenabschnittGIS, Belastungskategorie>(s => s.Belastungskategorie), FetchMode.Join)
            //    .CreateAlias(ExpressionHelper.GetPropertyName<StrassenabschnittGIS, ErfassungsPeriod>(s => s.ErfassungsPeriod), "erf")
            //    .Add(Restrictions.Eq("erf."+ExpressionHelper.GetPropertyName<Belastungskategorie, Guid>(s => s.Id), periode.Id));

            return writeEntitiesBatched<StrassenabschnittGIS>(query, writeStrabs, fileName, this.getStrabsHeader());

        }
        public Stream ExportZabs(string fileName, ErfassungsPeriod periode)
        {
            ISession session = transactionScopeProvider.CurrentTransactionScope.Session;
            IQueryOver<ZustandsabschnittGIS> query = session.QueryOver<ZustandsabschnittGIS>()
                .JoinQueryOver(z => z.StrassenabschnittGIS).Where(s => s.ErfassungsPeriod.Id == periode.Id)
                .OrderBy(z => z.Id).Asc
                .Fetch(z => z.StrassenabschnittGIS).Eager()
                .Fetch(z => z.MassnahmenvorschlagFahrbahn).Eager()
                .Fetch(z => z.MassnahmenvorschlagTrottoirLinks).Eager()
                .Fetch(z => z.MassnahmenvorschlagTrottoirRechts).Eager();

            return writeEntitiesBatched<ZustandsabschnittGIS>(query, writeZabs, fileName, this.getZabsHeader());

        }
        public Stream ExportTrottoir(string fileName, ErfassungsPeriod periode)
        {
            ISession session = transactionScopeProvider.CurrentTransactionScope.Session;
            IQueryOver<ZustandsabschnittGIS> query = session.QueryOver<ZustandsabschnittGIS>()
                .JoinQueryOver(z => z.StrassenabschnittGIS).Where(s => s.ErfassungsPeriod.Id == periode.Id)
                .OrderBy(z => z.Id).Asc
                .Fetch(z => z.StrassenabschnittGIS).Eager()
                .Fetch(z => z.MassnahmenvorschlagFahrbahn).Eager()
                .Fetch(z => z.MassnahmenvorschlagTrottoirLinks).Eager()
                .Fetch(z => z.MassnahmenvorschlagTrottoirRechts).Eager();

            return writeEntitiesBatched<ZustandsabschnittGIS>(query, writeTrottoir, fileName, this.getTrottoirHeader());

        }
        public IList<Feature> writeStrabs(IList<StrassenabschnittGIS> strabs)
        {
            IList<Feature> features = new List<Feature>();
            foreach (StrassenabschnittGIS strab in strabs)
            {
                //The service uses a cache (httpRequestCacheService) for loading  WiederbeschaffungswertKatalogModels so there should be no performance problem looking this up for every strab
                WiederbeschaffungswertKatalogModel wbwkatalog = this.wiederbeschaffungswertKatalogService.GetWiederbeschaffungswertKatalogModel(strab.Belastungskategorie);

                Feature feature = new Feature();
                IAttributesTable attributes = new AttributesTable();

                feature.Geometry = strab.Shape;

                double areaTrottLeft = strab.BreiteTrottoirLinks != null ? (double)(strab.BreiteTrottoirLinks*strab.Laenge) : 0d;
                double areaTrottRight = strab.BreiteTrottoirRechts != null ? (double)(strab.BreiteTrottoirRechts*strab.Laenge) : 0d;
                double areaStreet = (double)(strab.Laenge * strab.BreiteFahrbahn);
                double wbw = areaStreet * (double)wbwkatalog.FlaecheFahrbahn + (double)wbwkatalog.FlaecheTrottoir * (areaTrottLeft + areaTrottRight);

                attributes.AddAttribute(StrabShapeFileConstants.ID, this.guidToOracleRaw(strab.Id));
                attributes.AddAttribute(StrabShapeFileConstants.Belastungskategorie, StrabShapeFileConstants.getBelastungsKategorie(strab.Belastungskategorie.Typ));
                attributes.AddAttribute(StrabShapeFileConstants.Laenge, (double)strab.Laenge);
                attributes.AddAttribute(StrabShapeFileConstants.FlaecheFahrbahn, areaStreet);
                attributes.AddAttribute(StrabShapeFileConstants.Bezeichnungbis, strab.BezeichnungBis != null ? strab.BezeichnungBis : string.Empty);
                attributes.AddAttribute(StrabShapeFileConstants.Bezeichnungvon, strab.BezeichnungVon != null ? strab.BezeichnungVon : string.Empty);
                attributes.AddAttribute(StrabShapeFileConstants.Strassenname, strab.Strassenname != null ? strab.Strassenname : string.Empty);
                attributes.AddAttribute(StrabShapeFileConstants.Eigentuemer, StrabShapeFileConstants.getOwner(strab.Strasseneigentuemer));
                attributes.AddAttribute(StrabShapeFileConstants.Ortsbezeichnung, strab.Ortsbezeichnung != null ? strab.Ortsbezeichnung : string.Empty);
                attributes.AddAttribute(StrabShapeFileConstants.Belag, StrabShapeFileConstants.getBelag(strab.Belag));
                attributes.AddAttribute(StrabShapeFileConstants.BreiteFahrbahn, (double)strab.BreiteFahrbahn);
                attributes.AddAttribute(StrabShapeFileConstants.Trottoir, StrabShapeFileConstants.getTrottoirTyp(strab.Trottoir));
                attributes.AddAttribute(StrabShapeFileConstants.BreiteTrottoirlinks, strab.BreiteTrottoirLinks != null ? (double)strab.BreiteTrottoirLinks : 0d);
                attributes.AddAttribute(StrabShapeFileConstants.FlaecheTrottoirlinks, areaTrottLeft);
                attributes.AddAttribute(StrabShapeFileConstants.BreiteTrottoirrechts, strab.BreiteTrottoirRechts != null ? (double)strab.BreiteTrottoirRechts : 0d);
                attributes.AddAttribute(StrabShapeFileConstants.FlaecheTrottoirrechts, areaTrottRight);
                attributes.AddAttribute(StrabShapeFileConstants.FlaecheTrottoir, areaTrottRight+areaTrottLeft);
                attributes.AddAttribute(StrabShapeFileConstants.Wiederbeschaffungswert, wbw);
                attributes.AddAttribute(StrabShapeFileConstants.AlterungsbeiwertI, (double)wbwkatalog.AlterungsbeiwertI);
                attributes.AddAttribute(StrabShapeFileConstants.WertverlustI, (double)((double)wbwkatalog.AlterungsbeiwertI * wbw / 100));
                attributes.AddAttribute(StrabShapeFileConstants.AlterungsbeiwertII, (double)wbwkatalog.AlterungsbeiwertII);
                attributes.AddAttribute(StrabShapeFileConstants.WertverlustII, (double)((double)wbwkatalog.AlterungsbeiwertII * wbw / 100));

                feature.Attributes = attributes;
                features.Add(feature);
            }
            return features;
            //shpShxSerializeService.WriteShpShxDbf(features, fileName);
        }
        public IList<Feature> writeZabs(IList<ZustandsabschnittGIS> zabs)
        {
            IList<Feature> features = new List<Feature>();
            foreach (ZustandsabschnittGIS zab in zabs)
            {
                Feature feature = new Feature();
                IAttributesTable attributes = new AttributesTable();

                feature.Geometry = zab.Shape;


                double areafb = (double)(zab.FlaecheFahrbahn!=null ? zab.FlaecheFahrbahn : 0m);
                double areatrl = (double)(zab.FlaceheTrottoirLinks != null ? zab.FlaceheTrottoirLinks : 0m);
                double areatrr = (double)(zab.FlaceheTrottoirRechts != null ? zab.FlaceheTrottoirRechts : 0m);

                double fbkosten = (double)(zab.KostenMassnahmenvorschlagFahrbahn == null
                    ? (zab.MassnahmenvorschlagFahrbahn != null ? zab.MassnahmenvorschlagFahrbahn.DefaultKosten : 0m)
                    : zab.KostenMassnahmenvorschlagFahrbahn);
               double trrkosten = (double)(zab.KostenMassnahmenvorschlagTrottoirRechts == null
                    ? (zab.MassnahmenvorschlagTrottoirRechts != null ? zab.MassnahmenvorschlagTrottoirRechts.DefaultKosten : 0m)
                    : zab.KostenMassnahmenvorschlagTrottoirRechts);
                double trlkosten = (double)(zab.KostenMassnahmenvorschlagTrottoirLinks == null
                    ? (zab.MassnahmenvorschlagTrottoirLinks != null ? zab.MassnahmenvorschlagTrottoirLinks.DefaultKosten : 0m)
                    : zab.KostenMassnahmenvorschlagTrottoirLinks);

                attributes.AddAttribute(ZabShapeFileConstants.StrassenabschnittID, this.guidToOracleRaw(zab.StrassenabschnittGIS.Id));
                attributes.AddAttribute(ZabShapeFileConstants.Strassenname, zab.StrassenabschnittGIS.Strassenname);
                attributes.AddAttribute(ZabShapeFileConstants.StrassenabschnittBezeichnungvon, zab.StrassenabschnittGIS.BezeichnungVon != null ? zab.StrassenabschnittGIS.BezeichnungVon : string.Empty);
                attributes.AddAttribute(ZabShapeFileConstants.StrassenabschnittBezeichnungbis, zab.StrassenabschnittGIS.BezeichnungBis != null ? zab.StrassenabschnittGIS.BezeichnungBis : string.Empty);
                attributes.AddAttribute(ZabShapeFileConstants.Eigentuemer, StrabShapeFileConstants.getOwner(zab.StrassenabschnittGIS.Strasseneigentuemer));
                attributes.AddAttribute(ZabShapeFileConstants.Ortsbezeichnung, zab.StrassenabschnittGIS.Ortsbezeichnung != null ? zab.StrassenabschnittGIS.Ortsbezeichnung : string.Empty);

                attributes.AddAttribute(ZabShapeFileConstants.ID, this.guidToOracleRaw(zab.Id));
                attributes.AddAttribute(ZabShapeFileConstants.Bezeichnungvon, zab.BezeichnungVon != null ? zab.BezeichnungVon : string.Empty);
                attributes.AddAttribute(ZabShapeFileConstants.Bezeichnungbis, zab.BezeichnungBis != null ? zab.BezeichnungBis : string.Empty);
                attributes.AddAttribute(ZabShapeFileConstants.Laenge, (double)zab.Laenge);
                attributes.AddAttribute(ZabShapeFileConstants.FlaecheFahrbahn, areafb);
                attributes.AddAttribute(ZabShapeFileConstants.FlaecheTrottoirlinks, areatrl);
                attributes.AddAttribute(ZabShapeFileConstants.FlaecheTrottoirrechts, areatrr);
                attributes.AddAttribute(ZabShapeFileConstants.Aufnahmedatum, zab.Aufnahmedatum);
                attributes.AddAttribute(ZabShapeFileConstants.Aufnahmeteam, zab.Aufnahmeteam != null ? zab.Aufnahmeteam : string.Empty);

                attributes.AddAttribute(ZabShapeFileConstants.FBZustandsindex, (double)zab.Zustandsindex);
                attributes.AddAttribute(ZabShapeFileConstants.FBMassnahmenvorschlag, zab.MassnahmenvorschlagFahrbahn != null ? zab.MassnahmenvorschlagFahrbahn.Typ : string.Empty);
                attributes.AddAttribute(ZabShapeFileConstants.FBKosten, fbkosten);
                attributes.AddAttribute(ZabShapeFileConstants.FBDringlichkeit, ZabShapeFileConstants.getDringlichkeit(zab.DringlichkeitFahrbahn));              
                attributes.AddAttribute(ZabShapeFileConstants.FBGesamtkosten, fbkosten * areafb);

                attributes.AddAttribute(ZabShapeFileConstants.TRRZustandsindex, ZabShapeFileConstants.getTrottoirZustand(zab.ZustandsindexTrottoirRechts));
                attributes.AddAttribute(ZabShapeFileConstants.TRRMassnahmenvorschlag, zab.MassnahmenvorschlagTrottoirRechts != null ? zab.MassnahmenvorschlagTrottoirRechts.Typ : string.Empty);
                attributes.AddAttribute(ZabShapeFileConstants.TRRKosten, trrkosten);
                attributes.AddAttribute(ZabShapeFileConstants.TRRDringlichkeit, ZabShapeFileConstants.getDringlichkeit(zab.DringlichkeitTrottoirRechts));
                attributes.AddAttribute(ZabShapeFileConstants.TRRGesamtkosten, trrkosten * areatrr);

                attributes.AddAttribute(ZabShapeFileConstants.TRLZustandsindex, ZabShapeFileConstants.getTrottoirZustand(zab.ZustandsindexTrottoirLinks));
                attributes.AddAttribute(ZabShapeFileConstants.TRLMassnahmenvorschlag, zab.MassnahmenvorschlagTrottoirLinks != null ? zab.MassnahmenvorschlagTrottoirLinks.Typ : string.Empty);
                attributes.AddAttribute(ZabShapeFileConstants.TRLKosten, trlkosten);
                attributes.AddAttribute(ZabShapeFileConstants.TRLDringlichkeit, ZabShapeFileConstants.getDringlichkeit(zab.DringlichkeitTrottoirLinks));
                attributes.AddAttribute(ZabShapeFileConstants.TRLGesamtkosten, trlkosten * areatrl);



                feature.Attributes = attributes;
                features.Add(feature);
            }
            return features;
            //shpShxSerializeService.WriteShpShxDbf(features, fileName);
        }
        public IList<Feature> writeTrottoir(IList<ZustandsabschnittGIS> zabs)
        {
            IList<Feature> features = new List<Feature>();
            foreach (ZustandsabschnittGIS zab in zabs)
            {
                double areafb = (double)(zab.FlaecheFahrbahn != null ? zab.FlaecheFahrbahn : 0m);
                double areatrl = (double)(zab.FlaceheTrottoirLinks != null ? zab.FlaceheTrottoirLinks : 0m);
                double areatrr = (double)(zab.FlaceheTrottoirRechts != null ? zab.FlaceheTrottoirRechts : 0m);

                double fbkosten = (double)(zab.KostenMassnahmenvorschlagFahrbahn == null
                    ? (zab.MassnahmenvorschlagFahrbahn != null ? zab.MassnahmenvorschlagFahrbahn.DefaultKosten : 0m)
                    : zab.KostenMassnahmenvorschlagFahrbahn);
                double trrkosten = (double)(zab.KostenMassnahmenvorschlagTrottoirRechts == null
                     ? (zab.MassnahmenvorschlagTrottoirRechts != null ? zab.MassnahmenvorschlagTrottoirRechts.DefaultKosten : 0m)
                     : zab.KostenMassnahmenvorschlagTrottoirRechts);
                double trlkosten = (double)(zab.KostenMassnahmenvorschlagTrottoirLinks == null
                    ? (zab.MassnahmenvorschlagTrottoirLinks != null ? zab.MassnahmenvorschlagTrottoirLinks.DefaultKosten : 0m)
                    : zab.KostenMassnahmenvorschlagTrottoirLinks);

                

                if (zab.StrassenabschnittGIS.Trottoir == EMSG.Common.Enums.TrottoirTyp.Links || zab.StrassenabschnittGIS.Trottoir == EMSG.Common.Enums.TrottoirTyp.BeideSeiten)
                {
                    Feature feature = new Feature();
                    IAttributesTable attributes = new AttributesTable();

                    decimal? trottoirCenterDistance = zab.StrassenabschnittGIS.BreiteFahrbahn / 2 + zab.StrassenabschnittGIS.BreiteTrottoirLinks / 2;

                    List<ILineString> lines = new List<ILineString>();
                    foreach(AchsenReferenz achsref in zab.ReferenzGruppe.AchsenReferenzen)
                    {
                        bool reverse = achsref.AchsenSegment.IsInverted;
                        if (reverse)
                        {
                            trottoirCenterDistance *= -1;
                        }
                        lines.Add(GeometryUtils.createOffsetLineNew(achsref.Shape.Factory, (ILineString)achsref.Shape, (double)trottoirCenterDistance));
                    }

                    IGeometry shape = zab.Shape.Factory.CreateMultiLineString(lines.ToArray());

                    feature.Geometry = shape;

                    writeBaseTroittoir(attributes, zab);
                    attributes.AddAttribute(TrottoirShapeFileConstants.Breite, zab.StrassenabschnittGIS.BreiteTrottoirLinks);
                    attributes.AddAttribute(TrottoirShapeFileConstants.FlaecheTrottoir, areatrl);
                    attributes.AddAttribute(TrottoirShapeFileConstants.Lage, TrottoirShapeFileConstants.Left);

                    attributes.AddAttribute(TrottoirShapeFileConstants.Zustandsindex, TrottoirShapeFileConstants.getTrottoirZustand(zab.ZustandsindexTrottoirLinks));
                    attributes.AddAttribute(TrottoirShapeFileConstants.Massnahmenvorschlag, zab.MassnahmenvorschlagTrottoirLinks != null ? zab.MassnahmenvorschlagTrottoirLinks.Typ : string.Empty);
                    attributes.AddAttribute(TrottoirShapeFileConstants.Kosten, trlkosten);
                    attributes.AddAttribute(TrottoirShapeFileConstants.Dringlichkeit, TrottoirShapeFileConstants.getDringlichkeit(zab.DringlichkeitTrottoirLinks));
                    attributes.AddAttribute(TrottoirShapeFileConstants.Gesamtkosten, trlkosten * areatrl);

                    feature.Attributes = attributes;
                    features.Add(feature);
                }
                if (zab.StrassenabschnittGIS.Trottoir == EMSG.Common.Enums.TrottoirTyp.Rechts || zab.StrassenabschnittGIS.Trottoir == EMSG.Common.Enums.TrottoirTyp.BeideSeiten)
                {
                    Feature feature = new Feature();
                    IAttributesTable attributes = new AttributesTable();

                    decimal? trottoirCenterDistance = zab.StrassenabschnittGIS.BreiteFahrbahn / 2 + zab.StrassenabschnittGIS.BreiteTrottoirRechts / 2;

                    List<ILineString> lines = new List<ILineString>();
                    foreach (AchsenReferenz achsref in zab.ReferenzGruppe.AchsenReferenzen)
                    {
                        bool reverse = achsref.AchsenSegment.IsInverted;
                        if (!reverse)
                        {
                            trottoirCenterDistance *= -1;
                        }
                        lines.Add(GeometryUtils.createOffsetLineNew(achsref.Shape.Factory, (ILineString)achsref.Shape, (double)trottoirCenterDistance));
                    }

                    IGeometry shape = zab.Shape.Factory.CreateMultiLineString(lines.ToArray());

                    feature.Geometry = shape;

                    writeBaseTroittoir(attributes, zab);
                    attributes.AddAttribute(TrottoirShapeFileConstants.Breite, zab.StrassenabschnittGIS.BreiteTrottoirRechts);
                    attributes.AddAttribute(TrottoirShapeFileConstants.FlaecheTrottoir, areatrr);
                    attributes.AddAttribute(TrottoirShapeFileConstants.Lage, TrottoirShapeFileConstants.Right);

                    attributes.AddAttribute(TrottoirShapeFileConstants.Zustandsindex, TrottoirShapeFileConstants.getTrottoirZustand(zab.ZustandsindexTrottoirRechts));
                    attributes.AddAttribute(TrottoirShapeFileConstants.Massnahmenvorschlag, zab.MassnahmenvorschlagTrottoirRechts != null ? zab.MassnahmenvorschlagTrottoirRechts.Typ : string.Empty);
                    attributes.AddAttribute(TrottoirShapeFileConstants.Kosten, trrkosten);
                    attributes.AddAttribute(TrottoirShapeFileConstants.Dringlichkeit, TrottoirShapeFileConstants.getDringlichkeit(zab.DringlichkeitTrottoirRechts));
                    attributes.AddAttribute(TrottoirShapeFileConstants.Gesamtkosten, trrkosten * areatrr);

                    feature.Attributes = attributes;
                    features.Add(feature);
                }
            }
            return features;
            //shpShxSerializeService.WriteShpShxDbf(features, fileName);
        }

        private IAttributesTable writeBaseTroittoir(IAttributesTable attributes, ZustandsabschnittGIS zab)
        {
            attributes.AddAttribute(TrottoirShapeFileConstants.StrassenabschnittID, this.guidToOracleRaw(zab.StrassenabschnittGIS.Id));
            attributes.AddAttribute(TrottoirShapeFileConstants.Strassenname, zab.StrassenabschnittGIS.Strassenname);
            attributes.AddAttribute(TrottoirShapeFileConstants.StrassenabschnittBezeichnungvon, zab.StrassenabschnittGIS.BezeichnungVon != null ? zab.StrassenabschnittGIS.BezeichnungVon : string.Empty);
            attributes.AddAttribute(TrottoirShapeFileConstants.StrassenabschnittBezeichnungbis, zab.StrassenabschnittGIS.BezeichnungBis != null ? zab.StrassenabschnittGIS.BezeichnungBis : string.Empty);
            attributes.AddAttribute(TrottoirShapeFileConstants.Eigentuemer, StrabShapeFileConstants.getOwner(zab.StrassenabschnittGIS.Strasseneigentuemer));
            attributes.AddAttribute(TrottoirShapeFileConstants.Ortsbezeichnung, zab.StrassenabschnittGIS.Ortsbezeichnung != null ? zab.StrassenabschnittGIS.Ortsbezeichnung : string.Empty);

            attributes.AddAttribute(TrottoirShapeFileConstants.ID, this.guidToOracleRaw(Guid.NewGuid()));
            attributes.AddAttribute(TrottoirShapeFileConstants.ZustandsAbschnittID, this.guidToOracleRaw(zab.Id));
            attributes.AddAttribute(TrottoirShapeFileConstants.Bezeichnungvon, zab.BezeichnungVon != null ? zab.BezeichnungVon : string.Empty);
            attributes.AddAttribute(TrottoirShapeFileConstants.Bezeichnungbis, zab.BezeichnungBis != null ? zab.BezeichnungBis : string.Empty);
            attributes.AddAttribute(TrottoirShapeFileConstants.Laenge, (double)zab.Laenge);
            attributes.AddAttribute(TrottoirShapeFileConstants.Aufnahmedatum, zab.Aufnahmedatum);
            attributes.AddAttribute(TrottoirShapeFileConstants.Aufnahmeteam, zab.Aufnahmeteam != null ? zab.Aufnahmeteam : string.Empty);
            return attributes;
        }

        public Stream writeEntitiesBatched<T>(IQueryOver<T> query, Func<IList<T>, IList<Feature>> action, string fileName, DbaseFileHeader header = null)
        {
            int i = 0;
            query.CacheMode(CacheMode.Ignore);            
            IEnumerable<Feature> features = new List<Feature>();
            IList<T> batch;
            do
            {
                batch = query.Skip(this.batchSize * i).Take(batchSize).List();
                //batch = query.List();
                features = features.Concat(action.Invoke(batch));
                i++;
               
            } while (batch.Any());
            if (header != null)
            {
                header.NumRecords = features.Count();
            }
            return shpShxSerializeService.WriteShape(features.ToList(), fileName, header);
        }

        public Stream writeEntitiesBatched<T>(ICriteria crit, Func<IList<T>, IList<Feature>> action, string fileName)
        {
            int i = 0;
            //query.CacheMode(CacheMode.Get);
            IEnumerable<Feature> features = new List<Feature>();
            IList<T> batch;
            do
            {
                crit.SetFirstResult(this.batchSize * i);
                crit.SetMaxResults(batchSize);                
                var temp = crit.List();
                batch = (List<T>)crit.List();
                features = features.Concat(action.Invoke(batch));
                i++;

            } while (batch.Any());
            return shpShxSerializeService.WriteShape(features.ToList(), fileName);
        }

        //Returns a string which is equivalent to the Id stored in the DB
        private string guidToOracleRaw(Guid id)
        {           
            return BitConverter.ToString(id.ToByteArray()).Replace("-", "");
        }

        //inverse of guidToOracleRaw not yet required but included for potential later use
        private Guid oracleRawToGuid(string id)
        {
            byte[] bytes = new byte[id.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(id.Substring(i * 2, 2), 16);
            }
            return new Guid(bytes);
        }

        private DbaseFileHeader getZabsHeader()
        {
            DbaseFileHeader header = new DbaseFileHeader();
            header.AddColumn(ZabShapeFileConstants.StrassenabschnittID, 'C', StringLength, StringDecimals);
            header.AddColumn(ZabShapeFileConstants.Strassenname, 'C', StringLength, StringDecimals);
            header.AddColumn(ZabShapeFileConstants.StrassenabschnittBezeichnungvon, 'C', StringLength, StringDecimals);
            header.AddColumn(ZabShapeFileConstants.StrassenabschnittBezeichnungbis, 'C', StringLength, StringDecimals);
            header.AddColumn(ZabShapeFileConstants.Eigentuemer, 'C', StringLength, StringDecimals);
            header.AddColumn(ZabShapeFileConstants.Ortsbezeichnung, 'C', StringLength, StringDecimals);

            header.AddColumn(ZabShapeFileConstants.ID, 'C', StringLength, StringDecimals);
            header.AddColumn(ZabShapeFileConstants.Bezeichnungvon, 'C', StringLength, StringDecimals);
            header.AddColumn(ZabShapeFileConstants.Bezeichnungbis, 'C', StringLength, StringDecimals);
            header.AddColumn(ZabShapeFileConstants.Laenge, 'N', DoubleLength, DoubleDecimals);
            header.AddColumn(ZabShapeFileConstants.FlaecheFahrbahn, 'N', DoubleLength, DoubleDecimals);
            header.AddColumn(ZabShapeFileConstants.FlaecheTrottoirlinks, 'N', DoubleLength, DoubleDecimals);
            header.AddColumn(ZabShapeFileConstants.FlaecheTrottoirrechts, 'N', DoubleLength, DoubleDecimals);
            header.AddColumn(ZabShapeFileConstants.Aufnahmedatum, 'D', DateLength, DateDecimals);
            header.AddColumn(ZabShapeFileConstants.Aufnahmeteam, 'C', StringLength, StringDecimals);

            header.AddColumn(ZabShapeFileConstants.FBZustandsindex, 'C', StringLength, StringDecimals);
            header.AddColumn(ZabShapeFileConstants.FBMassnahmenvorschlag, 'C', StringLength, StringDecimals);
            header.AddColumn(ZabShapeFileConstants.FBKosten, 'N', DoubleLength, DoubleDecimals);
            header.AddColumn(ZabShapeFileConstants.FBDringlichkeit, 'C', StringLength, StringDecimals);
            header.AddColumn(ZabShapeFileConstants.FBGesamtkosten, 'N', DoubleLength, DoubleDecimals);

            header.AddColumn(ZabShapeFileConstants.TRRZustandsindex, 'C', StringLength, StringDecimals);
            header.AddColumn(ZabShapeFileConstants.TRRMassnahmenvorschlag, 'C', StringLength, StringDecimals);
            header.AddColumn(ZabShapeFileConstants.TRRKosten, 'N', DoubleLength, DoubleDecimals);
            header.AddColumn(ZabShapeFileConstants.TRRDringlichkeit, 'C', StringLength, StringDecimals);
            header.AddColumn(ZabShapeFileConstants.TRRGesamtkosten, 'N', DoubleLength, DoubleDecimals);

            header.AddColumn(ZabShapeFileConstants.TRLZustandsindex, 'C', StringLength, StringDecimals);
            header.AddColumn(ZabShapeFileConstants.TRLMassnahmenvorschlag, 'C', StringLength, StringDecimals);
            header.AddColumn(ZabShapeFileConstants.TRLKosten, 'N', DoubleLength, DoubleDecimals);
            header.AddColumn(ZabShapeFileConstants.TRLDringlichkeit, 'C', StringLength, StringDecimals);
            header.AddColumn(ZabShapeFileConstants.TRLGesamtkosten, 'N', DoubleLength, DoubleDecimals);

            return header;
        }
        private DbaseFileHeader getStrabsHeader()
        {
            DbaseFileHeader header = new DbaseFileHeader();

            header.AddColumn(StrabShapeFileConstants.ID, 'C', StringLength, StringDecimals);
            header.AddColumn(StrabShapeFileConstants.Strassenname, 'C', StringLength, StringDecimals);
            header.AddColumn(StrabShapeFileConstants.Bezeichnungbis, 'C', StringLength, StringDecimals);
            header.AddColumn(StrabShapeFileConstants.Bezeichnungvon, 'C', StringLength, StringDecimals);
            header.AddColumn(StrabShapeFileConstants.Eigentuemer, 'C', StringLength, StringDecimals);
            header.AddColumn(StrabShapeFileConstants.Ortsbezeichnung, 'C', StringLength, StringDecimals);
            header.AddColumn(StrabShapeFileConstants.Belastungskategorie, 'C', StringLength, StringDecimals);
            header.AddColumn(StrabShapeFileConstants.Belag, 'C', StringLength, StringDecimals);
            header.AddColumn(StrabShapeFileConstants.BreiteFahrbahn, 'N', DoubleLength, DoubleDecimals);
            header.AddColumn(StrabShapeFileConstants.Laenge, 'N', DoubleLength, DoubleDecimals);
            header.AddColumn(StrabShapeFileConstants.FlaecheFahrbahn, 'N', DoubleLength, DoubleDecimals);
            header.AddColumn(StrabShapeFileConstants.Trottoir, 'C', StringLength, StringDecimals);
            header.AddColumn(StrabShapeFileConstants.BreiteTrottoirlinks, 'N', DoubleLength, DoubleDecimals);
            header.AddColumn(StrabShapeFileConstants.BreiteTrottoirrechts, 'N', DoubleLength, DoubleDecimals);
            header.AddColumn(StrabShapeFileConstants.FlaecheTrottoirlinks, 'N', DoubleLength, DoubleDecimals);            
            header.AddColumn(StrabShapeFileConstants.FlaecheTrottoirrechts, 'N', DoubleLength, DoubleDecimals);
            header.AddColumn(StrabShapeFileConstants.FlaecheTrottoir, 'N', DoubleLength, DoubleDecimals);
            header.AddColumn(StrabShapeFileConstants.Wiederbeschaffungswert, 'N', DoubleLength, DoubleDecimals);
            header.AddColumn(StrabShapeFileConstants.AlterungsbeiwertI, 'N', DoubleLength, DoubleDecimals);
            header.AddColumn(StrabShapeFileConstants.WertverlustI, 'N', DoubleLength, DoubleDecimals);
            header.AddColumn(StrabShapeFileConstants.AlterungsbeiwertII, 'N', DoubleLength, DoubleDecimals);
            header.AddColumn(StrabShapeFileConstants.WertverlustII, 'N', DoubleLength, DoubleDecimals);
            return header;
        }
        private DbaseFileHeader getTrottoirHeader()
        {
            DbaseFileHeader header = new DbaseFileHeader();
            header.AddColumn(TrottoirShapeFileConstants.StrassenabschnittID, 'C', StringLength, StringDecimals);
            header.AddColumn(TrottoirShapeFileConstants.Strassenname, 'C', StringLength, StringDecimals);
            header.AddColumn(TrottoirShapeFileConstants.StrassenabschnittBezeichnungvon, 'C', StringLength, StringDecimals);
            header.AddColumn(TrottoirShapeFileConstants.StrassenabschnittBezeichnungbis, 'C', StringLength, StringDecimals);
            header.AddColumn(TrottoirShapeFileConstants.Eigentuemer, 'C', StringLength, StringDecimals);
            header.AddColumn(TrottoirShapeFileConstants.Ortsbezeichnung, 'C', StringLength, StringDecimals);

            header.AddColumn(TrottoirShapeFileConstants.ID, 'C', StringLength, StringDecimals);
            header.AddColumn(TrottoirShapeFileConstants.ZustandsAbschnittID, 'C', StringLength, StringDecimals);
            header.AddColumn(TrottoirShapeFileConstants.Bezeichnungvon, 'C', StringLength, StringDecimals);
            header.AddColumn(TrottoirShapeFileConstants.Bezeichnungbis, 'C', StringLength, StringDecimals);
            header.AddColumn(TrottoirShapeFileConstants.Laenge, 'N', DoubleLength, DoubleDecimals);
            header.AddColumn(TrottoirShapeFileConstants.Breite, 'N', DoubleLength, DoubleDecimals);

            
            header.AddColumn(TrottoirShapeFileConstants.FlaecheTrottoir, 'N', DoubleLength, DoubleDecimals);
            header.AddColumn(TrottoirShapeFileConstants.Lage, 'C', StringLength, StringDecimals);
            header.AddColumn(TrottoirShapeFileConstants.Aufnahmedatum, 'D', DateLength, DateDecimals);
            header.AddColumn(TrottoirShapeFileConstants.Aufnahmeteam, 'C', StringLength, StringDecimals);

            header.AddColumn(TrottoirShapeFileConstants.Zustandsindex, 'C', StringLength, StringDecimals);
            header.AddColumn(TrottoirShapeFileConstants.Massnahmenvorschlag, 'C', StringLength, StringDecimals);
            header.AddColumn(TrottoirShapeFileConstants.Kosten, 'N', DoubleLength, DoubleDecimals);
            header.AddColumn(TrottoirShapeFileConstants.Dringlichkeit, 'C', StringLength, StringDecimals);
            header.AddColumn(TrottoirShapeFileConstants.Gesamtkosten, 'N', DoubleLength, DoubleDecimals);


            return header;
        }
    }
}
