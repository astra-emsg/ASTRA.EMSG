using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Entities.Summarisch;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.IntegrationTests.Support;
using NHibernate.Linq;

namespace ASTRA.EMSG.IntegrationTests.Common
{
    public static class TestDataHelpers
    {
        public static NetzSummarisch GetNetzSummarisch(ErfassungsPeriod erfassungsPeriod)
        {
            return new NetzSummarisch
                {
                    ErfassungsPeriod = erfassungsPeriod,
                    Mandant = erfassungsPeriod.Mandant,
                    MittleresErhebungsJahr = DateTime.Parse("2012.01.01"),
                };
        }

        public static NetzSummarischDetail GetNetzSummarischDetail(ErfassungsPeriod erfassungsPeriod, Belastungskategorie belastungskategorie, int fahrbahnflaeche, int fahrbahnlaenge, decimal mittlererZustand)
        {
            return new NetzSummarischDetail
                {
                    Belastungskategorie = belastungskategorie,
                    ErfassungsPeriod = erfassungsPeriod,
                    Mandant = erfassungsPeriod.Mandant,
                    Fahrbahnflaeche = fahrbahnflaeche,
                    Fahrbahnlaenge = fahrbahnlaenge,
                    MittlererZustand = mittlererZustand,
                    NetzSummarisch = GetNetzSummarisch(erfassungsPeriod)
                };
        }

        public static StrassenabschnittGIS GetStrassenabschnittGIS(ErfassungsPeriod erfassungsPeriod, string strassenname, Belastungskategorie belastungskategorie, EigentuemerTyp eigentuemerTyp)
        {
            return new StrassenabschnittGIS
                       {
                           ErfassungsPeriod = erfassungsPeriod,
                           Mandant = erfassungsPeriod.Mandant,
                           Belag = BelagsTyp.Asphalt,
                           Strassenname = strassenname,
                           BezeichnungVon = strassenname.GetStrassennameBezeichnungVon(),
                           BezeichnungBis = strassenname.GetStrassennameBezeichnungBis(),
                           Belastungskategorie = belastungskategorie,
                           BreiteFahrbahn = 20,
                           BreiteTrottoirLinks = 5,
                           BreiteTrottoirRechts = 10,
                           IsLocked = false,
                           Laenge = 100,
                           Ortsbezeichnung = "Ortsbezeichnung",
                           Strasseneigentuemer = eigentuemerTyp,
                           Trottoir = TrottoirTyp.BeideSeiten
                       };
        }

        public static Strassenabschnitt GetStrassenabschnitt(ErfassungsPeriod erfassungsPeriod, string strassenname, Belastungskategorie belastungskategorie, EigentuemerTyp eigentuemerTyp)
        {
            return new Strassenabschnitt
                       {
                           ErfassungsPeriod = erfassungsPeriod,
                           Mandant = erfassungsPeriod.Mandant,
                           Belag = BelagsTyp.Asphalt,
                           Strassenname = strassenname,
                           BezeichnungVon = strassenname.GetStrassennameBezeichnungVon(),
                           BezeichnungBis = strassenname.GetStrassennameBezeichnungBis(),
                           Belastungskategorie = belastungskategorie,
                           BreiteFahrbahn = 20,
                           BreiteTrottoirLinks = 5,
                           BreiteTrottoirRechts = 10,
                           Laenge = 100,
                           Ortsbezeichnung = "Ortsbezeichnung",
                           Strasseneigentuemer = eigentuemerTyp,
                           Trottoir = TrottoirTyp.BeideSeiten
                       };
        }

        public static ZustandsabschnittGIS GetZustandsabschnittGIS(StrassenabschnittGIS strassenabschnittGIS, decimal zustandsindex = 1.0m)
        {
            return new ZustandsabschnittGIS
                       {
                           Aufnahmedatum = new DateTime(2010, 10, 10),
                           Aufnahmeteam = "Aufnahmeteam",
                           Laenge = strassenabschnittGIS.Laenge,
                           Bemerkung = "Bemerkung",
                           BezeichnungVon = strassenabschnittGIS.BezeichnungVon.GetZustandsabschnittBezeichnungVon(),
                           BezeichnungBis = strassenabschnittGIS.BezeichnungBis.GetZustandsabschnittBezeichnungBis(),
                           Erfassungsmodus = ZustandsErfassungsmodus.Manuel,
                           StrassenabschnittGIS = strassenabschnittGIS,
                           Wetter = WetterTyp.KeinRegen,
                           Zustandsindex = zustandsindex,
                           ZustandsindexTrottoirLinks = ZustandsindexTyp.Mittel,
                           ZustandsindexTrottoirRechts = ZustandsindexTyp.Mittel
                       };
        }

        public static Zustandsabschnitt GetZustandsabschnitt(Strassenabschnitt strassenabschnitt, decimal zustandsindex = 0.0m)
        {
            return new Zustandsabschnitt
                       {
                           Aufnahmedatum = new DateTime(2010, 10, 10),
                           Aufnahmeteam = "Aufnahmeteam",
                           Laenge = strassenabschnitt.Laenge,
                           Bemerkung = "Bemerkung",
                           BezeichnungVon = strassenabschnitt.BezeichnungVon.GetZustandsabschnittBezeichnungVon(),
                           BezeichnungBis = strassenabschnitt.BezeichnungBis.GetZustandsabschnittBezeichnungBis(),
                           Erfassungsmodus = ZustandsErfassungsmodus.Manuel,
                           Strassenabschnitt = strassenabschnitt,
                           Wetter = WetterTyp.KeinRegen,
                           Zustandsindex = zustandsindex,
                           ZustandsindexTrottoirLinks = ZustandsindexTyp.Mittel,
                           ZustandsindexTrottoirRechts = ZustandsindexTyp.Mittel
                       };
        }

        public static InspektionsRouteGIS GetInspektionsRouteGIS(ErfassungsPeriod erfassungsPeriod, string bezeichnung, DateTime? inInspektionBis, StrassenabschnittGIS strassenabschnittGISOne)
        {
            var inspektionsRouteGIS = new InspektionsRouteGIS
                                          {
                                              Bezeichnung = bezeichnung,
                                              Bemerkungen = "Bemerkungen", 
                                              Beschreibung = "Beschreibung",
                                              ErfassungsPeriod = erfassungsPeriod, 
                                              InInspektionBei = bezeichnung.GetInInspektionBei(), 
                                              InInspektionBis = inInspektionBis, 
                                              Mandant = erfassungsPeriod.Mandant
                                          };

            inspektionsRouteGIS.StatusverlaufList = new List<InspektionsRouteStatusverlauf>
                                                        {
                                                            new InspektionsRouteStatusverlauf
                                                                {
                                                                    Status = InspektionsRouteStatus.NeuErstellt,
                                                                    InspektionsRouteGIS = inspektionsRouteGIS,
                                                                    Datum = DateTime.Now
                                                                }
                                                        };

            inspektionsRouteGIS.InspektionsRtStrAbschnitteList = new List<InspektionsRtStrAbschnitte>
                                                                     {
                                                                         new InspektionsRtStrAbschnitte
                                                                             {
                                                                                 InspektionsRouteGIS = inspektionsRouteGIS,
                                                                                 StrassenabschnittGIS = strassenabschnittGISOne
                                                                             }
                                                                     };

            return inspektionsRouteGIS;
        }

        public static KoordinierteMassnahmeGIS GetKoordinierteMassnahmeGIS(ErfassungsPeriod erfassungsPeriod, string projektName, StatusTyp status, DateTime? ausfuehrungsAnfangdateTime)
        {
            return new KoordinierteMassnahmeGIS
            {
                Projektname = projektName,
                Mandant = erfassungsPeriod.Mandant,
                Status = status,
                AusfuehrungsAnfang = ausfuehrungsAnfangdateTime,
            };
        }

        public static RealisierteMassnahmeGIS GetRealisierteMassnahmeGIS(ErfassungsPeriod erfassungsPeriod, string projektName, string leitendeOrganisation, string beschreibung)
        {
            return new RealisierteMassnahmeGIS
            {
                Projektname = projektName,
                Beschreibung = beschreibung,
                Mandant = erfassungsPeriod.Mandant,
                ErfassungsPeriod = erfassungsPeriod,
                LeitendeOrganisation = leitendeOrganisation,
            };
        }

        public static RealisierteMassnahme GetRealisierteMassnahme(ErfassungsPeriod erfassungsPeriod, string projektName, string beschreibung)
        {
            return new RealisierteMassnahme
            {
                Projektname = projektName,
                Beschreibung = beschreibung,
                Mandant = erfassungsPeriod.Mandant,
                ErfassungsPeriod = erfassungsPeriod,
            };
        }

        public static RealisierteMassnahmeSummarsich GetRealisierteMassnahmeSummarisch(ErfassungsPeriod erfassungsPeriod, string projektName, string beschreibung)
        {
            return new RealisierteMassnahmeSummarsich
            {
                Projektname = projektName,
                Beschreibung = beschreibung,
                Mandant = erfassungsPeriod.Mandant,
                ErfassungsPeriod = erfassungsPeriod,
            };
        }

        public static MassnahmenvorschlagTeilsystemeGIS GetMassnahmenvorschlagTeilsystemeGIS(ErfassungsPeriod erfassungsPeriod, string projektName, StatusTyp status, DringlichkeitTyp dringlichkeit, TeilsystemTyp teilsystem)
        {
            return new MassnahmenvorschlagTeilsystemeGIS
            {
                Projektname = projektName,
                Mandant = erfassungsPeriod.Mandant,
                Status = status,
                Dringlichkeit = dringlichkeit,
                Teilsystem = teilsystem
            };
        }

        public static Belastungskategorie GetBelastungskategorie(NHibernateTestScope scope, string typ)
        {
            return scope.Session.Query<Belastungskategorie>().Single(m => m.Typ == typ);
        }
    }
}