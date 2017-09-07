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
    public class AxisImportDataHandlerFull : AxisImportDataHandler
    {

        public AxisImportDataHandlerFull(ITransactionCommitter transactionComitter, IAxisImportMonitor axisImportMonitor, int impNr, AxisImportPass pass)
            : base(transactionComitter, axisImportMonitor, impNr, pass)
        {
        }

        public override void ReceivedAxis(IImportedAchse axis)
        {
            if (Pass != AxisImportPass.Achsen) return;

            //if (axis.Operation != AxisImportOperation.INSERT) throw new AxisImportException("invalid operation for axis id="+ axis.Id);
            ReceivedItem(axis);
        }

        public override void ReceivedAxissegment(IImportedSegment axisSegment)
        {
            if (Pass != AxisImportPass.Segmente) return;

            //if (axisSegment.Operation != AxisImportOperation.INSERT) throw new AxisImportException("invalid operation for segment id=" + axisSegment.Id);

            ReceivedItem(axisSegment);
        }

        public override void ReceivedSector(IImportedSektor axisSektor)
        {
            if (Pass != AxisImportPass.Sektoren) return;

            //if (axisSektor.Operation != AxisImportOperation.INSERT) throw new AxisImportException("invalid operation for sector id=" + axisSektor.Id);

            ReceivedItem(axisSektor);
        }
    }
}
