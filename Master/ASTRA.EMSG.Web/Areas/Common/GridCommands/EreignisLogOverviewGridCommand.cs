using System;
using ASTRA.EMSG.Business.Entities.Common;
using Kendo.Mvc;

namespace ASTRA.EMSG.Web.Areas.Common.GridCommands
{
    public class EreignisLogOverviewGridCommand : GridCommand
    {
        public EreignisTyp? EreignisTyp { get; set; }
        public string Benutzer { get; set; }
        public string Mandant { get; set; }
        public DateTime? ZeitVon { get; set; }
        public DateTime? ZeitBis { get; set; }
    }
}