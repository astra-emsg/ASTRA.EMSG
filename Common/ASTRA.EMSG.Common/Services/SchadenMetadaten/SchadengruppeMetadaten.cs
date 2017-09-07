using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Common.Services.SchadenMetadaten
{
    public class SchadengruppeMetadaten
    {
        public SchadengruppeMetadaten()
        {
            Schadendetails = new List<SchadendetailMetadaten>();
        }

        public BelagsTyp BelagsTyp { get; set; }
        public SchadengruppeTyp SchadengruppeTyp { get; set; }
        public int Gewicht { get; set; }
        public int Reihung { get; set; }

        public List<SchadendetailMetadaten> Schadendetails { get; set; }

        public SchadengruppeMetadaten AddSchadendetail(SchadendetailTyp schadendetailTyp)
        {
            Schadendetails.Add(new SchadendetailMetadaten
                                   {
                                       BelagsTyp = BelagsTyp, 
                                       SchadengruppeTyp = SchadengruppeTyp, 
                                       SchadendetailTyp = schadendetailTyp, 
                                       Reihung = (Schadendetails.Max(sd => (int?)sd.Reihung) ?? 0) + 1
                                   });
            return this;
        }
    }
}
