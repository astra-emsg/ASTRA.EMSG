using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Interlis.Parser;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Interlis.AxisImport.TransactionHandling;

namespace ASTRA.EMSG.Business.Interlis.AxisImport
{
    public class AxisImportDataHandlerIncr : AxisImportDataHandler
    {

        public AxisImportDataHandlerIncr(ITransactionCommitter transactionComitter, IAxisImportMonitor axisImportMonitor, int impNr, AxisImportPass pass)
            : base(transactionComitter, axisImportMonitor, impNr, pass)
        {
        }

        public override void ReceivedAxis(IImportedAchse axis)
        {
            if (Pass != AxisImportPass.Achsen) return;
            ReceivedItem(axis);
        }

        public override void ReceivedAxissegment(IImportedSegment axisSegment)
        {
            if (Pass != AxisImportPass.Segmente) return;

            ReceivedItem(axisSegment);
        }

        public override void ReceivedSector(IImportedSektor axisSektor)
        {
            if (Pass != AxisImportPass.Sektoren) return;

            ReceivedItem(axisSektor);
        }
    }
}
