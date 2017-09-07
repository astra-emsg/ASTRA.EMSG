using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using ASTRA.EMSG.Business.Interlis.AxisImport;
using System.IO;
using System.IO.Compression;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.AchsenUpdate;
using NHibernate;

namespace ASTRA.EMSG.Tests.Common.Utils
{
    public class StammdatenFromAxisImport
    {
        public static void CreateTestData(TransactionScopeFactory transactionScopeFactory)
        {
            {
                //String gzfilename = @"InterlisFiles\20111219110540_Axis_CH_Kt_Full_TestData_351-353-362-363-663_2011_09_01.xml.gz";
                String gzfilename = @"InterlisFiles\20111219110540_Axis_CH_Kt_Full_TestData_353-358-663_2011_09_01.xml.gz";

                String filename = gzfilename.Replace(".gz", "");

                AxisDataUtils.GUnZipFile(gzfilename, filename);

                // axis import

                AxisImport axisImport = new AxisImport(transactionScopeFactory);

                axisImport.ClearImportLog();
                axisImport.RunImport(filename, DateTime.Now);

                // axis update

            }
        }

        public static void DoIncrementalAxisImport(TransactionScopeFactory transactionScopeFactory)
        {
            AxisImport axisImport = new AxisImport(transactionScopeFactory);
            axisImport.RunImport(@"InterlisFiles\20111219110541_Axis_CH_Kt_IncrFake_TestData_353-358-663_2011_09_01.xml", DateTime.Now);
        }

        private static void DoAxisUpdate(TransactionScopeFactory transactionScopeFactory, string mandantName)
        {
            using (var transactionScope = transactionScopeFactory.CreateReadWrite())
            {
                Mandant testMandant = transactionScope.Session.QueryOver<Mandant>().Where(o => o.MandantName == mandantName).SingleOrDefault();
                ErfassungsPeriod erfPeriod = transactionScope.Session.QueryOver<ErfassungsPeriod>().Where(o => o.Mandant == testMandant).OrderBy(o => o.Erfassungsjahr).Desc.List().First();

                //AxisDataUtils.ClearAxisDataForMandant(transactionScope.Session, testMandant.Id);

                AchsenAutoUpdate axisUpdate = new AchsenAutoUpdate(transactionScope.Session, testMandant, erfPeriod, testMandant.OwnerId);

                axisUpdate.Start();

                transactionScope.Commit();
            }

        }
    }
}
