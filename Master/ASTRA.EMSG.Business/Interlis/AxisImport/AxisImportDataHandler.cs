using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities.GIS;
using NHibernate;
using ASTRA.EMSG.Business.Entities;
using System.Diagnostics;
using ASTRA.EMSG.Business.Interlis.Parser;
using ASTRA.EMSG.Business.Interlis.AxisImport.TransactionHandling;

namespace ASTRA.EMSG.Business.Interlis.AxisImport
{
    public enum AxisImportPass
    {
        Achsen,
        Segmente,
        Sektoren
    }

    public abstract class AxisImportDataHandler : IAxisReaderDataHandler
    {
        private readonly ITransactionCommitter transactionCommitter;

        private readonly IAxisImportMonitor AxisImportMonitor;

        protected readonly int ImpNr;

        protected readonly AxisImportPass Pass;

        private int Counter=0;

        protected AxisImportStatistics statistics = new AxisImportStatistics();

        public AxisImportDataHandler(ITransactionCommitter transactionCommitter, IAxisImportMonitor axisImportMonitor, int impNr, AxisImportPass pass)
        {
            this.transactionCommitter = transactionCommitter;
            this.AxisImportMonitor = axisImportMonitor;

            transactionCommitter.ForceNext();

            this.ImpNr = impNr;
            this.Pass = pass;

            CheckCancelled();
        }

        public void EmitXMLFragment(string xml) { }

        public abstract void ReceivedAxis(IImportedAchse axis);

        public abstract void ReceivedAxissegment(IImportedSegment axisSegment);

        public abstract void ReceivedSector(IImportedSektor axisSektor);

        protected void HavePersistedRecord()
        {
            CheckCancelled();

            Counter++;

            if (transactionCommitter.Next())
            {
                PrintProgress();
            }
        }

        protected ISession CurrentSession
        {
            get
            {
                return transactionCommitter.Session;
            }
        }

        private void PrintProgress()
        {
            AxisImportMonitor.WriteLog("Progress: " + StatisticsAsString());

            Debug.WriteLine(StatisticsAsString());
        }

        public void Finished()
        {
            transactionCommitter.ForceNext();
            PrintProgress();
        }

        public AxisImportStatistics Statistics { get { return statistics; } }

        public string StatisticsAsString()
        {
            return statistics.ToString();
        }

        private void CheckCancelled()
        {
            if (AxisImportMonitor.IsCancelled()) throw new AxisImportCancelledException("Axis Import Cancelled!");
        }


        ///// utils
        protected void ReceivedItem(IImportedItem item)
        {
            if (item.Operation == AxisImportOperation.INSERT) // insert?
            {
                IKopie kopie = null;
                Num num = null;
                switch (Pass)
                {
                    case AxisImportPass.Achsen:
                         kopie = new KopieAchse();
                         CloneAxisAttributesWithoutId(kopie as KopieAchse, item as IImportedAchse);
                         num = statistics.NumAxis;
                         break;
                    case AxisImportPass.Segmente:
                         kopie = new KopieAchsenSegment();
                         CloneSegmentAttributesWithoutId(kopie as KopieAchsenSegment, item as IImportedSegment);
                         num = statistics.NumSegment;
                        break;
                    case AxisImportPass.Sektoren:
                        kopie = new KopieSektor();
                        CloneSektorAttributesWithoutId(kopie as KopieSektor, item as IImportedSektor);
                        num = statistics.NumSector;
                        break;
                    default:
                        return;
                }
                kopie.Id = item.Id;
                
                kopie.ImpNr = ImpNr;
                CurrentSession.Persist(kopie);

                num.Inserts++;
            }
            else  // update or delete
            {
                IKopie kopie = null;
                Num num = null;
                
                switch (Pass)
                {
                    case AxisImportPass.Achsen:
                        kopie = CurrentSession.Get<KopieAchse>(item.Id);
                        if (kopie == null)
                        {
                            throw new AxisImportException("Axis " + item.Id + " does not exist in database");
                        }
                        CloneAxisAttributesWithoutId(kopie as KopieAchse, item as IImportedAchse);
                        num = statistics.NumAxis;
                        break;
                    case AxisImportPass.Segmente:
                        kopie = CurrentSession.Get<KopieAchsenSegment>(item.Id);
                        if (kopie == null)
                        {
                            throw new AxisImportException("Segment " + item.Id + " does not exist in database");
                        }
                        CloneSegmentAttributesWithoutId(kopie as KopieAchsenSegment, item as IImportedSegment);
                        num = statistics.NumSegment;
                        break;
                    case AxisImportPass.Sektoren:
                        kopie = CurrentSession.Get<KopieSektor>(item.Id);
                        if (kopie == null)
                        {
                            throw new AxisImportException("Sektor " + item.Id + " does not exist in database");
                        }
                        CloneSektorAttributesWithoutId(kopie as KopieSektor, item as IImportedSektor);
                        num = statistics.NumSector;
                        break;
                    default:
                        return;
                }
                
                kopie.ImpNr = ImpNr;

                if (item.Operation == AxisImportOperation.UPDATE)
                {
                    num.Updates++;
                }
                else
                {
                    num.Deletes++;
                }

            }
            HavePersistedRecord();
        }
        protected void CloneAxisAttributesWithoutId(KopieAchse targetItem, IImportedAchse importedAxis)
        {
            targetItem.Name = importedAxis.Name;
            targetItem.Operation = importedAxis.Operation;
            targetItem.Owner = importedAxis.Owner;
            targetItem.VersionValidFrom = importedAxis.Version;
        }

        protected void CloneSegmentAttributesWithoutId(KopieAchsenSegment targetItem, IImportedSegment importedSegment)
        {
            targetItem.AchsenId = importedSegment.AchsenId;
            targetItem.Name = importedSegment.Name;
            targetItem.Sequence = importedSegment.Sequence;
            targetItem.Operation = importedSegment.Operation;
            targetItem.Shape = importedSegment.Shape;
        }

        protected void CloneSektorAttributesWithoutId(KopieSektor targetItem, IImportedSektor importedSektor)
        {
            targetItem.Km = importedSektor.Km;
            targetItem.Name = importedSektor.SectorName;
            targetItem.SectorLength = importedSektor.SectorLength;
            targetItem.Sequence = importedSektor.Sequence;
            targetItem.MarkerGeom = importedSektor.MarkerGeom;
            targetItem.SegmentId = importedSektor.SegmentId;
            targetItem.Operation = importedSektor.Operation;
        }
    }
}
