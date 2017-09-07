using System;
using ASTRA.EMSG.Business.Entities.Common;

namespace ASTRA.EMSG.Business.Services.EntityServices.Common
{
    public class EreignisLogOverviewParameter
    {
        public EreignisTyp? EreignisTyp { get; set; }
        public string Benutzer { get; set; }
        public string Mandant { get; set; }
        public DateTime? ZeitVon { get; set; }
        public DateTime? ZeitBis { get; set; }
    }
}