using System;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Models.Strassennamen
{
    public class ZustandsabschnittImportModel : Model, IExternalIdHolder
    {
        public Guid Strassenabschnitt { get; set; }

        public string Strassenname { get; set; }
        public string StrassennameBezeichnungVon { get; set; }
        public string StrassennameBezeichnungBis { get; set; }
        public string ExternalId { get; set; }

        public int? Abschnittsnummer { get; set; }
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }
        
        public decimal Zustandsindex { get; set; }
        public ZustandsindexTyp ZustandsindexTrottoirLinks { get; set; }
        public ZustandsindexTyp ZustandsindexTrottoirRechts { get; set; }

        public DateTime Aufnahmedatum { get; set; }
        public string Aufnahmeteam { get; set; }
        public WetterTyp Wetter { get; set; }
        public string Bemerkung { get; set; }
        
        public Guid? MassnahmenvorschlagFahrbahnId { get; set; }
        public Guid? MassnahmenvorschlagTrottoirLinksId { get; set; }
        public Guid? MassnahmenvorschlagTrottoirRechtsId { get; set; }

        public DringlichkeitTyp DringlichkeitFahrbahn { get; set; }
        public DringlichkeitTyp DringlichkeitTrottoirLinks { get; set; }
        public DringlichkeitTyp DringlichkeitTrottoirRechts { get; set; }

        public decimal Laenge { get; set; }
    }
}
