using System;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using FluentNHibernate.Mapping;
using NHibernate.Spatial.Type;
using ASTRA.EMSG.Business.Utils;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class ZustandsabschnittGISMapping : ClassMapBase<ZustandsabschnittGIS>
    {
        public ZustandsabschnittGISMapping()
        {
            MapTo(za => za.Zustandsindex);
            MapTo(s => s.BezeichnungVon, "BezVon");
            MapTo(s => s.BezeichnungBis, "BezBis");
            MapTo(za => za.Erfassungsmodus);
            MapTo(za => za.Laenge);
            MapTo(za => za.Abschnittsnummer);
            MapTo(za => za.Aufnahmedatum);
            MapTo(za => za.Aufnahmeteam);
            MapTo(za => za.Wetter);
            MapToLongText(za => za.Bemerkung);
            MapTo(za => za.ZustandsindexTrottoirLinks, "ZSTINDTRL");
            MapTo(za => za.ZustandsindexTrottoirRechts, "ZSTINDTRR");

            MapTo(mf => mf.KostenMassnahmenvorschlagFahrbahn, prefix: "Fb", entityTypeOverride: typeof(ZustandsabschnittGIS), columnName: "FbKosten");
            MapTo(mf => mf.DringlichkeitFahrbahn, prefix: "Fb", entityTypeOverride: typeof(ZustandsabschnittGIS));
            ReferencesTo(mf => mf.MassnahmenvorschlagFahrbahn, "Fb", typeof(ZustandsabschnittGIS));

            MapTo(mf => mf.KostenMassnahmenvorschlagTrottoirRechts, prefix: "TrR", entityTypeOverride: typeof(ZustandsabschnittGIS), columnName: "TrRKosten");
            MapTo(mf => mf.DringlichkeitTrottoirRechts, prefix: "TrR", entityTypeOverride: typeof(ZustandsabschnittGIS));
            ReferencesTo(mf => mf.MassnahmenvorschlagTrottoirRechts, "TrR", typeof(ZustandsabschnittGIS));

            MapTo(mf => mf.KostenMassnahmenvorschlagTrottoirLinks, prefix: "TrL", entityTypeOverride: typeof(ZustandsabschnittGIS), columnName: "TrLKosten");
            MapTo(mf => mf.DringlichkeitTrottoirLinks, prefix: "TrL", entityTypeOverride: typeof(ZustandsabschnittGIS));
            ReferencesTo(mf => mf.MassnahmenvorschlagTrottoirLinks, "TrL", typeof(ZustandsabschnittGIS));

            HasMany(za => za.Schadengruppen).KeyColumn("SCG_SCG_ZSG_ID").ForeignKeyConstraintName("CFK_ZSG_SCG_NOR").Cascade.All();
            HasMany(za => za.Schadendetails).KeyColumn("SCD_SCD_ZSG_ID").ForeignKeyConstraintName("CFK_ZSG_SCD_NOR").Cascade.All();

            MapTo(za => za.Shape);

            ReferencesTo(za => za.StrassenabschnittGIS);
            ReferencesTo(za => za.ReferenzGruppe).Cascade.All();
            ReferencesTo(me => me.CopiedFrom);
        }

        protected override string TableName
        {
            get { return "ZstGIS"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}