using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using NHibernate;
using ASTRA.EMSG.Business.Interlis.AxisImport.TransactionHandling;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Business.Entities.GIS;

namespace ASTRA.EMSG.Business.Interlis.AxisImport
{
    public class TestAxisImport : AxisImport
    {

        /// <summary>
        /// fake: import Number
        /// </summary>
        public static int impNumber = 0;

        /// <summary>
        /// test-Owner
        /// </summary>
        private readonly string owner;

        public TestAxisImport(TransactionScopeFactory transactionScopeFactory, IAxisImportMonitor axisImportMonitor, string owner = "ITEST_01")
            : base(transactionScopeFactory, axisImportMonitor)
        {
            this.owner = owner;
        }

        public TestAxisImport(TransactionScopeFactory transactionScopeFactory, string owner = "ITEST_01")
            : base(transactionScopeFactory)
        {
            this.owner = owner;
        }


        protected override void CleanAchskopieTables(ITransactionCommitter transactionComitter)
        {
            var kopieSektorTypeName = typeof(KopieSektor).Name;
            var kopieAchsensegmentTypeName = typeof(KopieAchsenSegment).Name;
            var KopieAchseTypeName = typeof(KopieAchse).Name;
            var kopieAchseIdPropertyName = ExpressionHelper.GetPropertyName<KopieAchse, Guid>(ka => ka.Id);
            var kopieAchsensegmentAchsenIdPropertyName = ExpressionHelper.GetPropertyName<KopieAchsenSegment, Guid>(kas => kas.AchsenId);
            var kopieAchsenOwnerPropertyName = ExpressionHelper.GetPropertyName<KopieAchse, string>(ka => ka.Owner);
            var kopieSektorSegmentIdPropertyName = ExpressionHelper.GetPropertyName<KopieSektor, Guid>(ks => ks.SegmentId);
            var kopieAchsensegmentIdPropertyName = ExpressionHelper.GetPropertyName<KopieAchsenSegment, Guid>(kas => kas.Id);

            
            {
                IQuery qry = transactionComitter.Session.CreateQuery(string.Format("delete from {0} sektor where sektor.{1} in (select {2} from {3} segment where segment.{4} in (select {5} from {6} achse where achse.{7} = :owner))",
                    new string[] { kopieSektorTypeName, kopieSektorSegmentIdPropertyName, kopieAchsensegmentIdPropertyName, kopieAchsensegmentTypeName, kopieAchsensegmentAchsenIdPropertyName , kopieAchseIdPropertyName, KopieAchseTypeName, kopieAchsenOwnerPropertyName}));
                qry.SetString("owner", owner);
                qry.ExecuteUpdate();
            }
            {
                IQuery qry = transactionComitter.Session.CreateQuery(string.Format("delete from {0} segment where segment.{1} in (select {2} from {3} achse where achse.{4} = :owner)",
                    new string[] { kopieAchsensegmentTypeName, kopieAchsensegmentAchsenIdPropertyName, kopieAchseIdPropertyName, KopieAchseTypeName, kopieAchsenOwnerPropertyName }));
                qry.SetString("owner", owner);
                qry.ExecuteUpdate();

            }
            {
                IQuery qry = transactionComitter.Session.CreateQuery(string.Format("delete from {0} achse where achse.{1} = :owner", 
                    new string[] { KopieAchseTypeName, kopieAchsenOwnerPropertyName }));
                qry.SetString("owner", owner);
                qry.ExecuteUpdate();
            }
            transactionComitter.ForceNext();
            
        }

        public override void ClearImportLog()
        {
        }

        protected override int GetNextImpNR(ISession session, DateTime SenderTimestamp)
        {
            impNumber++;
            return impNumber;
        }

        protected override void CreateImpLogRecord(ISession session, int impNr, string filename, DateTime SenderTimestamp)
        {
        }

        protected override void UpdateImportLogRecord(ISession session, int impNr, int progress, AxisImportStatistics statistics)
        {
        }
    }
}
