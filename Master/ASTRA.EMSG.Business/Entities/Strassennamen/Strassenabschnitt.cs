using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Mapping;
using System.Linq;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Entities.Strassennamen
{
    public interface IExternalIdHolder : IIdHolder
    {
        string ExternalId { get; set; }
    }

    [TableShortName("STT")]
    public class Strassenabschnitt : StrassenabschnittBase
    {
        public Strassenabschnitt()
        {
            Zustandsabschnitten = new List<Zustandsabschnitt>();
        }

        public virtual string ExternalId { get; set; }
        

        public override bool ShouldCheckBelagChange { get { return Zustandsabschnitten.Any(); } }

        public virtual IList<Zustandsabschnitt> Zustandsabschnitten { get; set; }
    }
}
