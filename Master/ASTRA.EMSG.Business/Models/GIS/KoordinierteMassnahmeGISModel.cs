using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;
using GeoAPI.Geometries;

namespace ASTRA.EMSG.Business.Models.GIS
{
    [Serializable]
    public class KoordinierteMassnahmeGISModel : Model, IAbschnittGISModelBase
    {
        public KoordinierteMassnahmeGISModel()
        {
            BeteiligteSysteme = new  List<TeilsystemTyp>();
        }

        public string Projektname { get; set; }
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = FormatStrings.ShortDecimalFormat, NullDisplayText = "")]
        public decimal? Laenge { get; set; }
        [DisplayFormat(DataFormatString = FormatStrings.LongDecimalFormat, NullDisplayText = "")]
        public decimal? BreiteFahrbahn { get; set; }
        [DisplayFormat(DataFormatString = FormatStrings.LongDecimalFormat, NullDisplayText = "")]
        public decimal? BreiteTrottoirLinks { get; set; }
        [DisplayFormat(DataFormatString = FormatStrings.LongDecimalFormat, NullDisplayText = "")]
        public decimal? BreiteTrottoirRechts { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal FlaecheFahrbahn { get; set;  }
        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaecheTrottoirLinks { get; set; }
        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaecheTrottoirRechts { get; set; }

        public Guid? MassnahmenvorschlagFahrbahn { get; set; }

        public List<TeilsystemTyp> BeteiligteSysteme { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? KostenGesamtprojekt { get; set; }
        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? KostenFahrbahn { get; set; }
        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? KostenTrottoirLinks { get; set; }
        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? KostenTrottoirRechts { get; set; }

        public decimal SumKosten { get { return (KostenFahrbahn ?? 0) + (KostenTrottoirLinks ?? 0) + (KostenTrottoirRechts ?? 0); } }

        public string Beschreibung { get; set; }
        public DateTime? AusfuehrungsAnfang { get; set; }
        public DateTime? AusfuehrungsEnde { get; set; }

        public string LeitendeOrganisation { get; set; }
        public StatusTyp Status { get; set; }

        public string FeatureGeoJSONString { get; set; }
        public IGeometry Shape { get; set; }
        public ReferenzGruppeModel ReferenzGruppeModel { get; set; }
    }
}
