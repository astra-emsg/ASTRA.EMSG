using System;
using System.ComponentModel.DataAnnotations;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;
using GeoAPI.Geometries;

namespace ASTRA.EMSG.Business.Models.GIS
{
    [Serializable]
    public class MassnahmenvorschlagTeilsystemeGISModel : Model, IAbschnittGISModelBase
    {
        public string Projektname { get; set; }
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }
        public TeilsystemTyp Teilsystem { get; set; }
        public virtual string ZustaendigeOrganisation { get; set; }
        public virtual string Beschreibung { get; set; }
        public virtual DringlichkeitTyp Dringlichkeit { get; set; }
        public virtual decimal? Kosten { get; set; }
        public virtual StatusTyp Status { get; set; }

        public string FeatureGeoJSONString { get; set; }
        public IGeometry Shape { get; set; }
        public ReferenzGruppeModel ReferenzGruppeModel { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = FormatStrings.ShortDecimalFormat, NullDisplayText = "")]
        public decimal? Laenge { get; set; }
    }
}
