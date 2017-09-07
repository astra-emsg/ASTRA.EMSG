using System;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Models.Administration
{
    [Serializable]
    public class MandantDetailsModel : Model
    {
        public string MandantName { get; set; }

        public int? Einwohner { get; set; }
        public int? Siedlungsflaeche { get; set; }
        public int? Gemeindeflaeche { get; set; }
        public Guid? Gemeindetyp { get; set; }
        public int? MittlereHoehenlageSiedlungsgebiete { get; set; }
        public int? DifferenzHoehenlageSiedlungsgebiete { get; set; }
        public int? Steuerertrag { get; set; }
        public Guid? OeffentlicheVerkehrsmittel { get; set; }

        public decimal NetzLaenge { get; set; }
    }
}