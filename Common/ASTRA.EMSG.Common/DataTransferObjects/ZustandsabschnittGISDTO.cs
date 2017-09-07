using System;
using System.Runtime.Serialization;
using ASTRA.EMSG.Common.Enums;
using GeoAPI.Geometries;

namespace ASTRA.EMSG.Common.DataTransferObjects
{
    [Serializable]
    public class ZustandsabschnittGISDTO : DataTransferObject, IReferenzGruppeDTOHolder
    {
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }
        
        public decimal? Laenge { get; set; }
        
        public decimal? FlaecheFahrbahn { get; set; }
        public decimal? FlaceheTrottoirLinks { get; set; }
        public decimal? FlaceheTrottoirRechts { get; set; }

        public DateTime? Aufnahmedatum { get; set; }
        public string Aufnahmeteam { get; set; }
        public WetterTyp Wetter { get; set; }
        public ZustandsErfassungsmodus Erfassungsmodus { get; set; }
        public string Bemerkung { get; set; }
        public Guid StrassenabschnittGIS { get; set; }
        public Guid ReferenzGruppe { get; set; }
        private ReferenzGruppeDTO referenzGruppeDTO;
        public ReferenzGruppeDTO ReferenzGruppeDTO { get { return referenzGruppeDTO; } set { ReferenzGruppe = value.Id; referenzGruppeDTO = value; } }
        public Guid StrassenabschnittBaseId { get { return StrassenabschnittGIS; } }

        public IGeometry Shape { get; set; }

        public decimal Zustandsindex { get; set; }

        public ZustandsindexTyp ZustandsindexTrottoirLinks { get; set; }
        public ZustandsindexTyp ZustandsindexTrottoirRechts { get; set; }

        public MassnahmenvorschlagDTO MassnahmenvorschlagLinks { get; set; }
        public MassnahmenvorschlagDTO MassnahmenvorschlagRechts { get; set; }

        public MassnahmenvorschlagDTO MassnahmenvorschlagFahrbahnDTO { get; set; }

        [OptionalField]
        private int? abschnittsnummer;
        public int? Abschnittsnummer
        {
            get { return abschnittsnummer; }
            set { abschnittsnummer = value; }
        }

        [OnDeserializing]
        private void SetCountryRegionDefault(StreamingContext sc)
        {
            Abschnittsnummer = null;
        }
    }
}
