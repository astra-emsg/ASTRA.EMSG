using System;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using FluentNHibernate.Mapping;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class RealisierteMassnahmeMapping : ClassMapBase<RealisierteMassnahme>
    {
        public RealisierteMassnahmeMapping()
        {
            MapTo(rm => rm.Projektname);
            MapTo(za => za.BezeichnungVon, "BezVon");
            MapTo(za => za.BezeichnungBis, "BezBis");
            MapTo(za => za.Laenge);
            MapTo(s => s.BreiteFahrbahn, "BreiteFb");
            MapTo(s => s.BreiteTrottoirLinks, "BreiteTrL");
            MapTo(s => s.BreiteTrottoirRechts, "BreiteTrR");

            ReferencesTo(za => za.MassnahmenvorschlagFahrbahn);
            ReferencesTo(za => za.MassnahmenvorschlagTrottoir, "Tr");
            ReferencesTo(za => za.Belastungskategorie);

            MapToLongText(rm => rm.Beschreibung, "Beschr");
            MapTo(za => za.KostenFahrbahn, "FbKosten");
            MapTo(za => za.KostenTrottoirRechts, "TrRKosten");
            MapTo(za => za.KostenTrottoirLinks, "TrlKosten");
            MapTo(za => za.Strasseneigentuemer, "Eigentuemer");

            ReferencesTo(za => za.ErfassungsPeriod);
            ReferencesTo(za => za.Mandant);
        }

        protected override string TableName
        {
            get { return "RelMassTab"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}