using System;
using System.ComponentModel.DataAnnotations;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Models.Strassennamen
{
    [Serializable]
    public class ZustandsabschnittModel : ZustandsabschnittModelBase
    {
        public Guid Strassenabschnitt { get; set; }

        public override Guid StrassenabschnittBaseId { get { return Strassenabschnitt; } }

        public int? Sreassenabschnittsnummer { get; set; }

        public decimal StrasseLaenge { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = FormatStrings.ShortDecimalFormat, NullDisplayText = "")]
        public decimal Zustandsindex { get; set; }

        public ZustandsErfassungsmodus Erfassungsmodus { get; set; }
    }
}
