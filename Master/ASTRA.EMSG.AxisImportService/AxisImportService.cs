using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using ASTRA.EMSG.Business.Interlis.AxisImportScanner;
using System.Configuration;

namespace ASTRA.EMSG.AxisImportService
{
    public partial class AxisImportService : ServiceBase
    {
        private AxisImportScanner scanner = null;

        public AxisImportService()
        {
            InitializeComponent();
        }

        public void Start()
        {
            this.OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            string baseDir = ConfigurationManager.AppSettings["InterlisBaseDir"];

            if (baseDir == null)
            {
                throw new Exception("InterlisBaseDir not set in app.config");
            }

            if (!System.IO.Path.IsPathRooted(baseDir))
            {
                baseDir = System.IO.Path.Combine
                (
                    System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location),
                    System.IO.Path.GetDirectoryName(baseDir)
                );
            }
            scanner = new AxisImportScanner(baseDir);
            scanner.Start();

        }

        public new void Stop()
        {
            this.OnStop();
        }

        protected override void OnStop()
        {
            scanner.Cancel();
            scanner = null;
        }
    }
}
