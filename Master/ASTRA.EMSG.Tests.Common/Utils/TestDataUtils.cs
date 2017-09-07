using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Entities.Summarisch;
using FizzWare.NBuilder;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;
using GeoAPI.Geometries;
using NetTopologySuite.LinearReferencing;
using ASTRA.EMSG.Common.Enums;


namespace ASTRA.EMSG.Tests.Common.Utils
{
    public static class TestDataUtils
    {
        private static int RealisierteMassnahmeSummarsichCount = 2;

        private static int StrassenabschnittCount = 50;
        private static int RealisierteMassnahmeCount = 2;

        //private static int StrassenabschnittGISCount = 50;
        private static int MassnahmenTeilsystemeGISCount = 2;
        private static int KoordinierteMassnahmeGISCount = 2;
        private static int InspektionasroutenGISCount = 2;
        private static int RealisierteMassnahmeGISCount = 2;

        //private static int RealisierteMassnahmeSummarsichCount = 2;

        //private static int StrassenabschnittCount = 4;
        //private static int RealisierteMassnahmeCount = 2;

        //private static int StrassenabschnittGISCount = 4;
        //private static int MassnahmenTeilsystemeGISCount = 2;
        //private static int KoordinierteMassnahmeGISCount = 2;
        //private static int InspektionasroutenGISCount = 2;
        //private static int RealisierteMassnahmeGISCount = 2;

        public static void DeleteAndRecreateMandantData(ISession session, string mandantName, bool keepMandantDetails = false)
        {
            var selectedMandant = session.Query<Mandant>().Single(m => m.MandantName == mandantName);
            var madantDetails = session.Query<MandantDetails>()
                .OrderByDescending(m => m.ErfassungsPeriod.Erfassungsjahr)
                .FirstOrDefault(d => d.Mandant == selectedMandant);
            if (madantDetails != null)
                session.Evict(madantDetails);
            else
                keepMandantDetails = false;

            //Note: delete Mandant and then ReCreate
            string ownerId = selectedMandant.OwnerId;
            DeleteMandantData(session, selectedMandant);

            if (keepMandantDetails)
                DbHandlerUtils.CreateMandant(session, mandantName, ownerId,
                                             mandantBezeichnung: selectedMandant.MandantBezeichnung,
                                             mandantDetails: madantDetails);
            else
                DbHandlerUtils.CreateMandant(session, mandantName, ownerId,
                                             mandantBezeichnung: selectedMandant.MandantBezeichnung);
        }

        public static void DeleteMandantData(ISession session, Mandant selectedMandant)
        {
            session.CreateQuery("delete RealisierteMassnahmeSummarsich c where c.Mandant = :mandant").SetParameter("mandant", selectedMandant).ExecuteUpdate();
            session.CreateQuery("delete NetzSummarischDetail c where c.NetzSummarisch.Id in (select n.id from NetzSummarisch n where n.Mandant = :mandant)").SetParameter("mandant", selectedMandant).ExecuteUpdate();
            session.CreateQuery("delete NetzSummarisch c where c.Mandant = :mandant").SetParameter("mandant", selectedMandant).ExecuteUpdate();


            session.CreateQuery("delete RealisierteMassnahme c where c.Mandant = :mandant").SetParameter("mandant", selectedMandant).ExecuteUpdate();
            session.CreateQuery("delete Zustandsabschnitt c where c.Strassenabschnitt.Id in (select n.id from Strassenabschnitt n where n.Mandant = :mandant)").SetParameter("mandant", selectedMandant).ExecuteUpdate();
            session.CreateQuery("delete Strassenabschnitt c where c.Mandant = :mandant").SetParameter("mandant", selectedMandant).ExecuteUpdate();

            session.CreateQuery("delete CheckOutsGIS c where c.Mandant = :mandant").SetParameter("mandant", selectedMandant).ExecuteUpdate();
            session.CreateQuery("delete InspektionsRtStrAbschnitte c where c.StrassenabschnittGIS.Id in (select n.id from StrassenabschnittGIS n where n.Mandant = :mandant)").SetParameter("mandant", selectedMandant).ExecuteUpdate();
            session.CreateQuery("delete InspektionsRouteStatusverlauf c where c.InspektionsRouteGIS.Id in (select n.id from InspektionsRouteGIS n where n.Mandant = :mandant)").SetParameter("mandant", selectedMandant).ExecuteUpdate();
            session.CreateQuery("delete InspektionsRouteGIS c where c.Mandant = :mandant").SetParameter("mandant", selectedMandant).ExecuteUpdate();

            session.CreateQuery("delete AchsenReferenz c where c.ReferenzGruppe.Id in (select n.ReferenzGruppe.id from ZustandsabschnittGIS n where n.StrassenabschnittGIS.Id in (select s.id from StrassenabschnittGIS s where s.Mandant = :mandant))").SetParameter("mandant", selectedMandant).ExecuteUpdate();
            session.CreateQuery("delete ZustandsabschnittGIS c where c.StrassenabschnittGIS.Id in (select n.id from StrassenabschnittGIS n where n.Mandant = :mandant)").SetParameter("mandant", selectedMandant).ExecuteUpdate();

            session.CreateQuery("delete AchsenReferenz c where c.ReferenzGruppe.Id in (select n.ReferenzGruppe.id from StrassenabschnittGIS n where n.Mandant = :mandant)").SetParameter("mandant", selectedMandant).ExecuteUpdate();
            session.CreateQuery("delete StrassenabschnittGIS c where c.Mandant = :mandant").SetParameter("mandant", selectedMandant).ExecuteUpdate();

            session.CreateQuery("delete AchsenReferenz c where c.ReferenzGruppe.Id in (select n.ReferenzGruppe.id from MassnahmenvorschlagTeilsystemeGIS n where n.Mandant = :mandant)").SetParameter("mandant", selectedMandant).ExecuteUpdate();
            session.CreateQuery("delete MassnahmenvorschlagTeilsystemeGIS c where c.Mandant = :mandant").SetParameter("mandant", selectedMandant).ExecuteUpdate();

            session.CreateSQLQuery("delete from ADD_BETSYSRM_MSG WHERE BST_BST_RMG_NOR_ID in (SELECT RMG_ID FROM ADD_REALMASSGIS_MSG WHERE RMG_RMG_MAN_NOR_ID = :mandant)").SetGuid("mandant", selectedMandant.Id).ExecuteUpdate();
            session.CreateQuery("delete AchsenReferenz c where c.ReferenzGruppe.Id in (select n.ReferenzGruppe.id from RealisierteMassnahmeGIS n where n.Mandant = :mandant)").SetParameter("mandant", selectedMandant).ExecuteUpdate();
            session.CreateQuery("delete RealisierteMassnahmeGIS c where c.Mandant = :mandant").SetParameter("mandant", selectedMandant).ExecuteUpdate();

            session.CreateSQLQuery("delete from ADD_BETSYS_MSG WHERE BST_BST_KMG_NOR_ID in (SELECT KMG_ID FROM ADD_KOORMASSGIS_MSG WHERE KMG_KMG_MAN_NOR_ID = :mandant)").SetGuid("mandant", selectedMandant.Id).ExecuteUpdate();
            session.CreateQuery("delete AchsenReferenz c where c.ReferenzGruppe.Id in (select n.ReferenzGruppe.id from KoordinierteMassnahmeGIS n where n.Mandant = :mandant)").SetParameter("mandant", selectedMandant).ExecuteUpdate();
            session.CreateQuery("delete KoordinierteMassnahmeGIS c where c.Mandant = :mandant").SetParameter("mandant", selectedMandant).ExecuteUpdate();
            session.CreateQuery(@"delete ReferenzGruppe c where 
                    not exists (select n.Id from KoordinierteMassnahmeGIS n where n.ReferenzGruppe.Id = c.Id) and
                    not exists (select n.Id from ZustandsabschnittGIS n where n.ReferenzGruppe.Id = c.Id) and
                    not exists (select n.Id from StrassenabschnittGIS n where n.ReferenzGruppe.Id = c.Id) and
                    not exists (select n.Id from MassnahmenvorschlagTeilsystemeGIS n where n.ReferenzGruppe.Id = c.Id) and
                    not exists (select n.Id from RealisierteMassnahmeGIS n where n.ReferenzGruppe.Id = c.Id) and
                    not exists (select n.Id from AchsenReferenz n where n.ReferenzGruppe.Id = c.Id)
                    ").ExecuteUpdate();

            session.Query<BenchmarkingDataDetail>().Where(n => n.BenchmarkingData.Mandant == selectedMandant).ForEach(session.Delete);

            session.Query<BenchmarkingData>().Where(n => n.Mandant == selectedMandant).ForEach(session.Delete);

            session.CreateQuery("delete Sektor c where c.AchsenSegment.Id in (select n.id from AchsenSegment n where n.Mandant = :mandant)").SetParameter("mandant", selectedMandant).ExecuteUpdate();
            session.CreateQuery("delete AchsenSegment c where c.Mandant = :mandant").SetParameter("mandant", selectedMandant).ExecuteUpdate();
            session.CreateQuery("delete Achse c where c.Mandant = :mandant").SetParameter("mandant", selectedMandant).ExecuteUpdate();
            session.CreateQuery("delete AchsenUpdateLog c where c.Mandant = :mandant").SetParameter("mandant", selectedMandant).ExecuteUpdate();

            session.Query<AchsenUpdateConflict>().Where(n => n.Mandant == selectedMandant).ForEach(session.Delete);

            session.Query<KenngroessenFruehererJahre>().Where(n => n.Mandant == selectedMandant).ForEach(session.Delete);

            session.Query<WiederbeschaffungswertKatalog>().Where(n => n.Mandant == selectedMandant).ForEach(session.Delete);
            session.Query<MassnahmenvorschlagKatalog>().Where(n => n.Mandant == selectedMandant).ForEach(session.Delete);
            
            session.Query<ErfassungsPeriod>().Where(n => n.Mandant == selectedMandant).ForEach(session.Delete);


            session.Query<MandantDetails>().Where(n => n.Mandant == selectedMandant).ForEach(session.Delete);
            session.Query<MandantLogo>().Where(n => n.Mandant == selectedMandant).ForEach(session.Delete);
            session.Query<AchsenLock>().Where(n => n.Mandant == selectedMandant).ForEach(session.Delete);
            session.Query<Mandant>().Where(n => n.Id == selectedMandant.Id).ForEach(session.Delete);

            session.Flush();
        }

        public static void GenerateTabellarischData(ISession session, ErfassungsPeriod currentPeriod)
        {
            GenerateStrassenabschnitt(session, currentPeriod);
            GenerateRealisierteMassnahmen(session, currentPeriod);
        }

        private static void GenerateRealisierteMassnahmen(ISession session, ErfassungsPeriod currentPeriod)
        {
            var belastungskategories = session.Query<Belastungskategorie>().ToList();

            var realisierteMassnahmenBuilder = Builder<RealisierteMassnahme>.CreateListOfSize(RealisierteMassnahmeCount)
                .All()
                .With(d => d.Mandant = currentPeriod.Mandant)
                .With(d => d.ErfassungsPeriod = currentPeriod)
                .With(d => d.Belastungskategorie = Pick<Belastungskategorie>.RandomItemFrom(belastungskategories));

            foreach (var realisierteMassnahme in realisierteMassnahmenBuilder.Build())
                session.Save(realisierteMassnahme);
        }

        private static void GenerateStrassenabschnitt(ISession session, ErfassungsPeriod currentPeriod)
        {
            var belastungskategories = session.Query<Belastungskategorie>().ToList();
            var strassenabschnittBuilder = Builder<Strassenabschnitt>.CreateListOfSize(StrassenabschnittCount)
                .All()
                .With(s => s.Belastungskategorie = Pick<Belastungskategorie>.RandomItemFrom(belastungskategories))
                .With(s => s.Mandant = currentPeriod.Mandant)
                .With(s => s.ErfassungsPeriod = currentPeriod);

            var zustandsabschnitten = Builder<Zustandsabschnitt>.CreateListOfSize(StrassenabschnittCount * 2)
                .All()
                .With(z => z.Erfassungsmodus = ZustandsErfassungsmodus.Manuel)
                .With(z => z.Zustandsindex = Pick<decimal>.RandomItemFrom(new List<decimal>{0.0m, 0.25m, 0.5m, 0.75m, 1, 1.5m, 2, 2.5m, 3, 3.5m, 4, 4.5m, 5}))
                .Build();

            var index = 0;
            var pageSize = 2;
            foreach (var strassenabschnitt in strassenabschnittBuilder.Build())
            {
                session.Save(strassenabschnitt);
                foreach (var zustandsabschnitt in zustandsabschnitten.Skip(index*pageSize).Take(pageSize))
                {
                    zustandsabschnitt.Strassenabschnitt = strassenabschnitt;
                    strassenabschnitt.Zustandsabschnitten.Add(zustandsabschnitt);
                    session.Save(zustandsabschnitt);
                }
                index++;
            }
        }

        public static void GenerateGISData(ISession session, ErfassungsPeriod currentPeriod)
        {
            var achsensegmente = session.Query<AchsenSegment>().ToList();
            achsensegmente = achsensegmente.Where(r => r.ErfassungsPeriod == currentPeriod)/*.Take(StrassenabschnittGISCount)*/.ToList();
            if (achsensegmente.Count >0)
            {
                var strassenabschnitte = GenerateStrassenabschnittGIS(session, currentPeriod, achsensegmente);
                GenerateInspektionsrouten(session, currentPeriod, strassenabschnitte);

                //Note: Commetned out temporell wegen Performance test
                GenerateMassnahmenTeilsystemeGIS(session, currentPeriod);
                GenerateKoordinierteMassnahmeGIS(session, currentPeriod);
                GenerateRealisierteMassnahmeGIS(session, currentPeriod);
            }
        }

        public static IList<StrassenabschnittGIS> GenerateStrassenabschnittGIS(ISession session, ErfassungsPeriod currentPeriod, IList<AchsenSegment> achsensegmente)
        {
            var belastungskategories = session.Query<Belastungskategorie>().ToList();
            var Massnahmen = session.Query<MassnahmenvorschlagKatalog>().Where(mvk => mvk.ErfassungsPeriod.Id == currentPeriod.Id).ToList();
            Random rnd = new Random();

            int achsensegmente_count = achsensegmente.Count;
            var generator = new RandomGenerator();
            var strassenabschnittBuilder = Builder<StrassenabschnittGIS>.CreateListOfSize(achsensegmente_count)
                .All()
                .With(s => s.Belastungskategorie = Pick<Belastungskategorie>.RandomItemFrom(belastungskategories))
                .With(s => s.Mandant = currentPeriod.Mandant)
                .With(s => s.ErfassungsPeriod = currentPeriod)
                .With(s => s.Strassenname = generator.Phrase(20))
                .With(s => s.IsLocked = false)
                .With(s => s.Trottoir = Pick<TrottoirTyp>.RandomItemFrom(Enum.GetValues(typeof(TrottoirTyp)).Cast<TrottoirTyp>().ToList()));

            var strassenabschnitte = strassenabschnittBuilder.Build();
            var pageSize = 1;
            var index = 0;

            foreach (var strassenabschnitt in strassenabschnitte)
            {
                strassenabschnitt.Shape = achsensegmente[index].Shape;
                IGeometry shape = strassenabschnitt.Shape;
                foreach (Coordinate coord in shape.Coordinates)
                {
                    coord.Z = double.NaN;
                }
                
                switch (strassenabschnitt.Trottoir)
                {
                    
                    case TrottoirTyp.Links:
                        strassenabschnitt.BreiteTrottoirLinks = (decimal)rnd.NextDouble() + 1;
                        break;
                    case TrottoirTyp.Rechts:
                        strassenabschnitt.BreiteTrottoirRechts = (decimal)rnd.NextDouble() + 1;
                        break;
                    case TrottoirTyp.BeideSeiten:
                        strassenabschnitt.BreiteTrottoirLinks = (decimal)rnd.NextDouble() + 1;
                        strassenabschnitt.BreiteTrottoirRechts = (decimal)rnd.NextDouble() + 1;
                        break;
                    case TrottoirTyp.NochNichtErfasst:
                    case TrottoirTyp.KeinTrottoir:                        
                    default:
                        break;
                }

                LengthIndexedLine indexedLine = new LengthIndexedLine(shape);
                strassenabschnitt.Laenge = Convert.ToDecimal(Math.Round(indexedLine.EndIndex,1));
                strassenabschnitt.ReferenzGruppe = new ReferenzGruppe();
                strassenabschnitt.ReferenzGruppe.StrassenabschnittGISList.Add(strassenabschnitt);

                AchsenReferenz achsenreferenz = new AchsenReferenz();
                achsenreferenz.ReferenzGruppe = strassenabschnitt.ReferenzGruppe;
                achsenreferenz.AchsenSegment = achsensegmente[index];
                achsenreferenz.Shape = achsensegmente[index].Shape;
                achsenreferenz.ReferenzGruppe = strassenabschnitt.ReferenzGruppe;
                strassenabschnitt.ReferenzGruppe.AddAchsenReferenz(achsenreferenz);

                

                double linelength = indexedLine.EndIndex;
                for (int i = 0; i < pageSize; i++)
                {
                    ZustandsabschnittGIS zustandsabschnitt = new ZustandsabschnittGIS();
                    zustandsabschnitt.StrassenabschnittGIS = strassenabschnitt;
                    zustandsabschnitt.ReferenzGruppe = new ReferenzGruppe();
                    zustandsabschnitt.ReferenzGruppe.ZustandsabschnittGISList.Add(zustandsabschnitt);
                    AchsenReferenz achsenreferenzzustandsabschnitt = new AchsenReferenz();
                    achsenreferenzzustandsabschnitt.ReferenzGruppe = zustandsabschnitt.ReferenzGruppe;
                    achsenreferenzzustandsabschnitt.AchsenSegment = achsensegmente[index];
                    IGeometry subline = indexedLine.ExtractLine(linelength / pageSize * i, linelength / pageSize * (i + 1));
                    achsenreferenzzustandsabschnitt.Shape = subline;
                    zustandsabschnitt.Shape = subline;
                    zustandsabschnitt.Laenge = decimal.Parse(new LengthIndexedLine(subline).EndIndex.ToString());
                    zustandsabschnitt.Zustandsindex = Pick<decimal>.RandomItemFrom(new List<decimal> { 0, 1, 2, 3, 4, 5 });
                    zustandsabschnitt.Erfassungsmodus = ZustandsErfassungsmodus.Manuel;
                    achsenreferenzzustandsabschnitt.ReferenzGruppe = zustandsabschnitt.ReferenzGruppe;
                    zustandsabschnitt.ReferenzGruppe.AddAchsenReferenz(achsenreferenzzustandsabschnitt);
                    zustandsabschnitt.Aufnahmedatum = DateTime.Now.Date;
                    zustandsabschnitt.Aufnahmeteam = generator.Phrase(20);
                    zustandsabschnitt.Bemerkung = generator.Phrase(40);
                    zustandsabschnitt.BezeichnungVon = strassenabschnitt.BezeichnungVon + generator.Phrase(10);
                    zustandsabschnitt.BezeichnungBis = strassenabschnitt.BezeichnungBis + generator.Phrase(10);
                    strassenabschnitt.Zustandsabschnitten.Add(zustandsabschnitt);
                    zustandsabschnitt.MassnahmenvorschlagFahrbahn = Pick<MassnahmenvorschlagKatalog>.RandomItemFrom(Massnahmen.Where(mvk => mvk.KatalogTyp == MassnahmenvorschlagKatalogTyp.Fahrbahn && mvk.Belastungskategorie.Id == strassenabschnitt.Belastungskategorie.Id).ToList());
                    zustandsabschnitt.DringlichkeitFahrbahn = Pick<DringlichkeitTyp>.RandomItemFrom(Enum.GetValues(typeof(DringlichkeitTyp)).Cast<DringlichkeitTyp>().ToList());
                    switch (strassenabschnitt.Trottoir)
                    {

                        case TrottoirTyp.Links:
                            zustandsabschnitt.MassnahmenvorschlagTrottoirLinks = Pick<MassnahmenvorschlagKatalog>.RandomItemFrom(Massnahmen.Where(mvk => mvk.KatalogTyp == MassnahmenvorschlagKatalogTyp.Trottoir && mvk.Belastungskategorie.Id == strassenabschnitt.Belastungskategorie.Id).ToList());
                            zustandsabschnitt.ZustandsindexTrottoirLinks = Pick<ZustandsindexTyp>.RandomItemFrom(Enum.GetValues(typeof(ZustandsindexTyp)).Cast<ZustandsindexTyp>().ToList());
                            zustandsabschnitt.DringlichkeitTrottoirLinks = Pick<DringlichkeitTyp>.RandomItemFrom(Enum.GetValues(typeof(DringlichkeitTyp)).Cast<DringlichkeitTyp>().ToList());
                            break;
                        case TrottoirTyp.Rechts:
                            zustandsabschnitt.MassnahmenvorschlagTrottoirRechts = Pick<MassnahmenvorschlagKatalog>.RandomItemFrom(Massnahmen.Where(mvk => mvk.KatalogTyp == MassnahmenvorschlagKatalogTyp.Trottoir && mvk.Belastungskategorie.Id == strassenabschnitt.Belastungskategorie.Id).ToList());
                            zustandsabschnitt.ZustandsindexTrottoirRechts = Pick<ZustandsindexTyp>.RandomItemFrom(Enum.GetValues(typeof(ZustandsindexTyp)).Cast<ZustandsindexTyp>().ToList());
                            zustandsabschnitt.DringlichkeitTrottoirRechts = Pick<DringlichkeitTyp>.RandomItemFrom(Enum.GetValues(typeof(DringlichkeitTyp)).Cast<DringlichkeitTyp>().ToList());
                            break;
                        case TrottoirTyp.BeideSeiten:
                            zustandsabschnitt.MassnahmenvorschlagTrottoirLinks = Pick<MassnahmenvorschlagKatalog>.RandomItemFrom(Massnahmen.Where(mvk => mvk.KatalogTyp == MassnahmenvorschlagKatalogTyp.Trottoir && mvk.Belastungskategorie.Id == strassenabschnitt.Belastungskategorie.Id).ToList());
                            zustandsabschnitt.MassnahmenvorschlagTrottoirRechts = Pick<MassnahmenvorschlagKatalog>.RandomItemFrom(Massnahmen.Where(mvk => mvk.KatalogTyp == MassnahmenvorschlagKatalogTyp.Trottoir && mvk.Belastungskategorie.Id == strassenabschnitt.Belastungskategorie.Id).ToList());
                            zustandsabschnitt.ZustandsindexTrottoirLinks = Pick<ZustandsindexTyp>.RandomItemFrom(Enum.GetValues(typeof(ZustandsindexTyp)).Cast<ZustandsindexTyp>().ToList());
                            zustandsabschnitt.ZustandsindexTrottoirRechts = Pick<ZustandsindexTyp>.RandomItemFrom(Enum.GetValues(typeof(ZustandsindexTyp)).Cast<ZustandsindexTyp>().ToList());
                            zustandsabschnitt.DringlichkeitTrottoirLinks = Pick<DringlichkeitTyp>.RandomItemFrom(Enum.GetValues(typeof(DringlichkeitTyp)).Cast<DringlichkeitTyp>().ToList());
                            zustandsabschnitt.DringlichkeitTrottoirRechts = Pick<DringlichkeitTyp>.RandomItemFrom(Enum.GetValues(typeof(DringlichkeitTyp)).Cast<DringlichkeitTyp>().ToList());
                            break;
                        case TrottoirTyp.NochNichtErfasst:
                        case TrottoirTyp.KeinTrottoir:
                        default:
                            break;
                    }
                    session.Save(zustandsabschnitt);
                }
                session.Save(strassenabschnitt);
                index++;
            }
            var test = strassenabschnitte.Where(s => s.IsLocked == true);
            return strassenabschnitte;

        }

        public static void GenerateInspektionsrouten(ISession session, ErfassungsPeriod currentPeriod, IList<StrassenabschnittGIS> strassen) 
        {
            var strassenabschnitte = strassen;
            var inspektionsroutebuilder = Builder<InspektionsRouteGIS>.CreateListOfSize(InspektionasroutenGISCount)
               .All()
               .With(ir => ir.Mandant = currentPeriod.Mandant)
               .With(ir => ir.ErfassungsPeriod = currentPeriod);
            var inspektionsrouten = inspektionsroutebuilder.Build();
            int strabsperInspektionsroute = (int)(Math.Floor((double)(strassenabschnitte.Count / inspektionsrouten.Count / 2)));
            int saIndex = 1;
            foreach (InspektionsRouteGIS inspektionsRoute in inspektionsrouten)
            {
                IList<InspektionsRtStrAbschnitte> irabsList = new List<InspektionsRtStrAbschnitte>();
                IGeometry shape = null;
                for (int i = 0; i < strabsperInspektionsroute; i++)
                {
                    InspektionsRtStrAbschnitte irabs = new InspektionsRtStrAbschnitte();
                    irabs.StrassenabschnittGIS = strassenabschnitte[saIndex];
                    irabs.InspektionsRouteGIS = inspektionsRoute;
                    irabs.Reihenfolge = i;
                    irabsList.Add(irabs);

                    if (shape != null)
                    {
                        shape = shape.Union(strassenabschnitte[saIndex].Shape);
                    }
                    else
                    {
                        shape = strassenabschnitte[saIndex].Shape;
                    }
                    saIndex++;
                    session.Save(irabs);
                }
                inspektionsRoute.InspektionsRtStrAbschnitteList = irabsList;
                inspektionsRoute.Shape = shape;

                session.Save(inspektionsRoute);
            }
        }

        public static void GenerateMassnahmenTeilsystemeGIS(ISession session, ErfassungsPeriod currentPeriod)
        {
            var achsensegmente = session.Query<AchsenSegment>();
            
            var achsensegmentelist = achsensegmente.Where(r => r.ErfassungsPeriod == currentPeriod).Take(MassnahmenTeilsystemeGISCount).ToList();
            int achsensegmente_count = achsensegmentelist.Count;
            var generator = new RandomGenerator();
            var massnahmenvorschlagBuilder = Builder<MassnahmenvorschlagTeilsystemeGIS>.CreateListOfSize(achsensegmente_count)
                .All()
                .With(s => s.Mandant = currentPeriod.Mandant)
                .With(s => s.Beschreibung = generator.Phrase(20))
                .With(s => s.Projektname = generator.Phrase(20))
                .With(s => s.Teilsystem = Pick<TeilsystemTyp>.RandomItemFrom(Enum.GetValues(typeof(TeilsystemTyp)).Cast<TeilsystemTyp>().ToList()))
                .With(s=> s.Status = Pick<StatusTyp>.RandomItemFrom(Enum.GetValues(typeof(StatusTyp)).Cast<StatusTyp>().ToList()));

            var massnahmenTeilsysteme = massnahmenvorschlagBuilder.Build();
            var index = 0;
            
            foreach (var massnahmenTeilsystem in massnahmenTeilsysteme)
            {
                massnahmenTeilsystem.Shape = achsensegmentelist[index].Shape;
                massnahmenTeilsystem.ReferenzGruppe = new ReferenzGruppe();
                massnahmenTeilsystem.ReferenzGruppe.Erfassungsperiod = currentPeriod;
                massnahmenTeilsystem.ReferenzGruppe.Mandant = currentPeriod.Mandant;
                massnahmenTeilsystem.ReferenzGruppe.MassnahmenvorschlagTeilsystemeGISList.Add(massnahmenTeilsystem);
                var ar = new AchsenReferenz();
                ar.Mandandt = currentPeriod.Mandant;
                ar.Shape = achsensegmentelist[index].Shape;
                ar.AchsenSegment = achsensegmentelist[index];
                massnahmenTeilsystem.ReferenzGruppe.AchsenReferenzen.Add(ar);

                session.Save(ar);
                session.Save(massnahmenTeilsystem.ReferenzGruppe);
                session.Save(massnahmenTeilsystem);
                index++;
            }
        }

        public static void GenerateKoordinierteMassnahmeGIS(ISession session, ErfassungsPeriod currentPeriod)
        {
            var achsensegmente = session.Query<AchsenSegment>().ToList();
            achsensegmente = achsensegmente.Where(r => r.ErfassungsPeriod == currentPeriod).Take(KoordinierteMassnahmeGISCount).ToList();
            int achsensegmente_count = achsensegmente.Count;
            var generator = new RandomGenerator();
            var massnahmenvorschlagBuilder = Builder<KoordinierteMassnahmeGIS>.CreateListOfSize(achsensegmente_count)
                .All()
                .With(s => s.Mandant = currentPeriod.Mandant)
                .With(s => s.Beschreibung = generator.Phrase(20))
                .With(s => s.Projektname = generator.Phrase(20))
                .With(s => s.Status = Pick<StatusTyp>.RandomItemFrom(Enum.GetValues(typeof(StatusTyp)).Cast<StatusTyp>().ToList()));

            var massnahmenKoordiniert = massnahmenvorschlagBuilder.Build();
            var index = 0;

            foreach (var massnahmeKoordiniert in massnahmenKoordiniert)
            {
                massnahmeKoordiniert.Shape = achsensegmente[index].Shape;
                massnahmeKoordiniert.ReferenzGruppe = new ReferenzGruppe();
                massnahmeKoordiniert.ReferenzGruppe.Erfassungsperiod = currentPeriod;
                massnahmeKoordiniert.ReferenzGruppe.Mandant = currentPeriod.Mandant;
                massnahmeKoordiniert.ReferenzGruppe.KoordinierteMassnahmeGISList.Add(massnahmeKoordiniert);
                var ar = new AchsenReferenz();
                ar.Mandandt = currentPeriod.Mandant;
                ar.Shape = achsensegmente[index].Shape;
                ar.AchsenSegment = achsensegmente[index];
                ar.ReferenzGruppe = massnahmeKoordiniert.ReferenzGruppe;
                massnahmeKoordiniert.ReferenzGruppe.AchsenReferenzen.Add(ar);
                massnahmeKoordiniert.Shape = achsensegmente[index].Shape;
                session.Save(massnahmeKoordiniert.ReferenzGruppe);
                session.Save(massnahmeKoordiniert);
                index++;
            }
        }

        public static void GenerateRealisierteMassnahmeGIS(ISession session, ErfassungsPeriod currentPeriod)
        {
            var achsensegmente = session.Query<AchsenSegment>().ToList();
            achsensegmente = achsensegmente.Where(r => r.ErfassungsPeriod == currentPeriod).Take(RealisierteMassnahmeGISCount).ToList();
            int achsensegmente_count = achsensegmente.Count;
            var generator = new RandomGenerator();
            var massnahmenvorschlagBuilder = Builder<RealisierteMassnahmeGIS>.CreateListOfSize(achsensegmente_count)
                .All()
                .With(s => s.Mandant = currentPeriod.Mandant)
                .With(s => s.Beschreibung = generator.Phrase(20))
                .With(s => s.Projektname = generator.Phrase(20));

            var realisierteMassnahmenList = massnahmenvorschlagBuilder.Build();
            var index = 0;

            foreach (var realisierteMassnahme in realisierteMassnahmenList)
            {
                realisierteMassnahme.Shape = achsensegmente[index].Shape;
                realisierteMassnahme.ReferenzGruppe = new ReferenzGruppe();
                realisierteMassnahme.ReferenzGruppe.Erfassungsperiod = currentPeriod;
                realisierteMassnahme.ReferenzGruppe.Mandant = currentPeriod.Mandant;
                realisierteMassnahme.ReferenzGruppe.RealisierteMassnahmeGISList.Add(realisierteMassnahme);
                var ar = new AchsenReferenz();
                ar.Mandandt = currentPeriod.Mandant;
                ar.Shape = achsensegmente[index].Shape;
                ar.AchsenSegment = achsensegmente[index];
                ar.ReferenzGruppe = realisierteMassnahme.ReferenzGruppe;
                realisierteMassnahme.ReferenzGruppe.AchsenReferenzen.Add(ar);
                realisierteMassnahme.Shape = achsensegmente[index].Shape;
                session.Save(realisierteMassnahme);
                index++;
            }
        }

        public static void GenerateSummarichDaten(ISession session, ErfassungsPeriod currentPeriod)
        {
            var netzSummarisch = session.Query<NetzSummarisch>().Single(m => m.ErfassungsPeriod == currentPeriod);
            netzSummarisch.MittleresErhebungsJahr = DateTime.Now;
            netzSummarisch.NetzSummarischDetails.ToList().ForEach(n => netzSummarisch.NetzSummarischDetails.Remove(n));
            session.Flush();
            var belastungskategories = session.Query<Belastungskategorie>().ToList();
            var randomItemPicker = new RandomItemPicker<Belastungskategorie>(belastungskategories, new UniqueRandomGenerator());
            var detailBuilder = Builder<NetzSummarischDetail>.CreateListOfSize(belastungskategories.Count)
                .All()
                .With(d => d.NetzSummarisch = netzSummarisch)
                .With(d => d.Belastungskategorie = randomItemPicker.Pick());

            foreach (var netzSummarischDetail in detailBuilder.Build())
            {
                netzSummarisch.NetzSummarischDetails.Add(netzSummarischDetail);
                session.Save(netzSummarischDetail);
            }

            var realisierteMassnahmenBuilder = Builder<RealisierteMassnahmeSummarsich>.CreateListOfSize(RealisierteMassnahmeSummarsichCount)
                .All()
                .With(d => d.Mandant = currentPeriod.Mandant)
                .With(d => d.ErfassungsPeriod = currentPeriod)
                .With(d => d.Belastungskategorie = Pick<Belastungskategorie>.RandomItemFrom(belastungskategories));

            foreach (var realisierteMassnahmeSummarsich in realisierteMassnahmenBuilder.Build())
                session.Save(realisierteMassnahmeSummarsich);
        }
    }
}
