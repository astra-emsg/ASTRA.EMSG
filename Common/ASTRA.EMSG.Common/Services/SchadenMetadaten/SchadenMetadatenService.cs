using System;
using System.Collections.Generic;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Common.Services.SchadenMetadaten
{
    public interface ISchadenMetadatenService
    {
        List<SchadengruppeMetadaten> GetSchadengruppeMetadaten(BelagsTyp belagsTyp);
    }

    public class SchadenMetadatenService : ISchadenMetadatenService
    {
        private readonly List<SchadengruppeMetadaten> asphaltSchadengruppeMetadaten;
        private readonly List<SchadengruppeMetadaten> betonSchadengruppeMetadaten;

        public SchadenMetadatenService()
        {
            asphaltSchadengruppeMetadaten = new List<SchadengruppeMetadaten>
            {
                new SchadengruppeMetadaten{BelagsTyp = BelagsTyp.Asphalt, Gewicht = 2, Reihung = 1, SchadengruppeTyp = SchadengruppeTyp.Oberflaechenglaette}
                    .AddSchadendetail(SchadendetailTyp.Polieren)
                    .AddSchadendetail(SchadendetailTyp.Schwitzen),
                new SchadengruppeMetadaten{BelagsTyp = BelagsTyp.Asphalt, Gewicht = 2, Reihung = 2, SchadengruppeTyp = SchadengruppeTyp.Belagschaeden}
                    .AddSchadendetail(SchadendetailTyp.Abrieb)
                    .AddSchadendetail(SchadendetailTyp.AusmagerungAbsanden)
                    .AddSchadendetail(SchadendetailTyp.Kornausbrueche)
                    .AddSchadendetail(SchadendetailTyp.Abloesungen)
                    .AddSchadendetail(SchadendetailTyp.Schlagloecher)
                    .AddSchadendetail(SchadendetailTyp.OffeneNaehte)
                    .AddSchadendetail(SchadendetailTyp.Querrisse)
                    .AddSchadendetail(SchadendetailTyp.WildeRisse),
                new SchadengruppeMetadaten{BelagsTyp = BelagsTyp.Asphalt, Gewicht = 2, Reihung = 3, SchadengruppeTyp = SchadengruppeTyp.Belagsverformungen}
                    .AddSchadendetail(SchadendetailTyp.Spurrinnen)
                    .AddSchadendetail(SchadendetailTyp.Aufwoelbungen)
                    .AddSchadendetail(SchadendetailTyp.Wellblechverformungen)
                    .AddSchadendetail(SchadendetailTyp.Schubverformungen),
                new SchadengruppeMetadaten{BelagsTyp = BelagsTyp.Asphalt, Gewicht = 3, Reihung = 4, SchadengruppeTyp = SchadengruppeTyp.StrukturelleSchaeden}
                    .AddSchadendetail(SchadendetailTyp.AnrisseVonSetzungen)
                    .AddSchadendetail(SchadendetailTyp.SetzungenEinsenkungen)
                    .AddSchadendetail(SchadendetailTyp.AbgedrueckteRaeder)
                    .AddSchadendetail(SchadendetailTyp.Frosthebungen)
                    .AddSchadendetail(SchadendetailTyp.Laengsrisse)
                    .AddSchadendetail(SchadendetailTyp.Netzrisse)
                    .AddSchadendetail(SchadendetailTyp.Belagsrandrisse),
                new SchadengruppeMetadaten{BelagsTyp = BelagsTyp.Asphalt, Gewicht = 1, Reihung = 5, SchadengruppeTyp = SchadengruppeTyp.Flicke}
                    .AddSchadendetail(SchadendetailTyp.Flicke),
            };

            betonSchadengruppeMetadaten = new List<SchadengruppeMetadaten>
            {
                new SchadengruppeMetadaten{BelagsTyp = BelagsTyp.Beton, Gewicht = 1, Reihung = 1, SchadengruppeTyp = SchadengruppeTyp.Oberflaechenglaette}
                    .AddSchadendetail(SchadendetailTyp.Polieren),
                new SchadengruppeMetadaten{BelagsTyp = BelagsTyp.Beton, Gewicht = 2, Reihung = 2, SchadengruppeTyp = SchadengruppeTyp.Materialverluste}
                    .AddSchadendetail(SchadendetailTyp.Abrieb)
                    .AddSchadendetail(SchadendetailTyp.Abblaetterung)
                    .AddSchadendetail(SchadendetailTyp.Abplatzungen),
                new SchadengruppeMetadaten{BelagsTyp = BelagsTyp.Beton, Gewicht = 1, Reihung = 3, SchadengruppeTyp = SchadengruppeTyp.FugenUndKantenschaeden}
                    .AddSchadendetail(SchadendetailTyp.KantenschaedenAbsplitterung)
                    .AddSchadendetail(SchadendetailTyp.FehlenderOderSproederFugenverguss),
                new SchadengruppeMetadaten{BelagsTyp = BelagsTyp.Beton, Gewicht = 3, Reihung = 4, SchadengruppeTyp = SchadengruppeTyp.Vertikalverschiebung}
                    .AddSchadendetail(SchadendetailTyp.SetzungenFrosthebungen)
                    .AddSchadendetail(SchadendetailTyp.Stufenbildung)
                    .AddSchadendetail(SchadendetailTyp.Pumpen)
                    .AddSchadendetail(SchadendetailTyp.BlowUp),
                new SchadengruppeMetadaten{BelagsTyp = BelagsTyp.Beton, Gewicht = 2, Reihung = 5, SchadengruppeTyp = SchadengruppeTyp.RisseBrueche}
                    .AddSchadendetail(SchadendetailTyp.Risse)
                    .AddSchadendetail(SchadendetailTyp.ZerstoertePlatten),
                new SchadengruppeMetadaten{BelagsTyp = BelagsTyp.Beton, Gewicht = 1, Reihung = 6, SchadengruppeTyp = SchadengruppeTyp.Flicke}
                    .AddSchadendetail(SchadendetailTyp.Flicke),
            };
        }

        public List<SchadengruppeMetadaten> GetSchadengruppeMetadaten(BelagsTyp belagsTyp)
        {
            switch (belagsTyp)
            {
                case BelagsTyp.Asphalt:
                    return asphaltSchadengruppeMetadaten;
                case BelagsTyp.Beton:
                    return betonSchadengruppeMetadaten;
                default:
                    throw new ArgumentOutOfRangeException("belagsTyp");
            }
        }
    }
}
