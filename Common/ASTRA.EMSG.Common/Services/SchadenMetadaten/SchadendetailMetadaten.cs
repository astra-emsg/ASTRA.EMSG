using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Common.Services.SchadenMetadaten
{
    public class SchadendetailMetadaten
    {
        public BelagsTyp BelagsTyp { get; set; }
        public SchadengruppeTyp SchadengruppeTyp { get; set; }
        public SchadendetailTyp SchadendetailTyp { get; set; }
        public int Reihung { get; set; }
    }
}
