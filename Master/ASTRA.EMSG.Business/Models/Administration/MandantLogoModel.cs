using System;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Models.Administration
{
    [Serializable]
    public class MandantLogoModel : Model
    {
        public byte[] Logo { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }
}