using System;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using FluentNHibernate.Mapping;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class ZustandsabschnittMapping : ClassMapBase<Zustandsabschnitt>
    {
        public ZustandsabschnittMapping()
        {
            MapTo(za => za.Zustandsindex, "ZstInd");
            MapTo(za => za.Erfassungsmodus);
            MapTo(za => za.BezeichnungVon, "BezVon");
            MapTo(za => za.BezeichnungBis, "BezBis");
            MapTo(za => za.ExternalId);
            MapTo(za => za.Laenge);
            MapTo(za => za.Abschnittsnummer);
            MapTo(za => za.Aufnahmedatum, "AufnDat");
            MapTo(za => za.Aufnahmeteam, "AufnTeam");
            MapTo(za => za.Wetter);
            MapToLongText(za => za.Bemerkung);
            MapTo(za => za.ZustandsindexTrottoirLinks, "ZstIndTrL");
            MapTo(za => za.ZustandsindexTrottoirRechts, "ZstIndTrR");

            MapTo(mf => mf.KostenMassnahmenvorschlagFahrbahn, prefix: "Fb", entityTypeOverride: typeof(Zustandsabschnitt), columnName: "FbKosten");
            MapTo(mf => mf.DringlichkeitFahrbahn, prefix: "Fb", entityTypeOverride: typeof(Zustandsabschnitt));
            ReferencesTo(mf => mf.MassnahmenvorschlagFahrbahn, "Fb", typeof(Zustandsabschnitt));

            MapTo(mf => mf.KostenMassnahmenvorschlagTrottoirRechts, prefix: "TrR", entityTypeOverride: typeof(Zustandsabschnitt), columnName: "TrRKosten");
            MapTo(mf => mf.DringlichkeitTrottoirRechts, prefix: "TrR", entityTypeOverride: typeof(Zustandsabschnitt));
            ReferencesTo(mf => mf.MassnahmenvorschlagTrottoirRechts, "TrR", typeof(Zustandsabschnitt));

            MapTo(mf => mf.KostenMassnahmenvorschlagTrottoirLinks, prefix: "TrL", entityTypeOverride: typeof(Zustandsabschnitt), columnName: "TrLKosten");
            MapTo(mf => mf.DringlichkeitTrottoirLinks, prefix: "TrL", entityTypeOverride: typeof(Zustandsabschnitt));
            ReferencesTo(mf => mf.MassnahmenvorschlagTrottoirLinks, "TrL", typeof(Zustandsabschnitt));
            
            ReferencesTo(za => za.Strassenabschnitt);
            HasMany(za => za.Schadengruppen).KeyColumn("SCG_SCG_ZST_ID").ForeignKeyConstraintName("CFK_ZST_SCG_NOR").Cascade.All();
            HasMany(za => za.Schadendetails).KeyColumn("SCD_SCD_ZST_ID").ForeignKeyConstraintName("CFK_ZST_SCD_NOR").Cascade.All();
        }

        protected override string TableName
        {
            get { return "ZstTAB"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}
