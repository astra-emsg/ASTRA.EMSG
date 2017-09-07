using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;


namespace ASTRA.EMSG.AxisImportService
{
    [RunInstaller(true)]
    public partial class SInstaller : Installer
    {

        public static string UserName { get; set; }
        public static string Password { get; set; }
        public static string ServiceName { get; set; }
        public static string DisplayName { get; set; }
        public static string Description { get; set; }

        public SInstaller()
        {
            ServiceProcessInstaller processInstaller = null;
            if (UserName != "")
            {
                processInstaller = new ServiceProcessInstaller
                {
                    Username = UserName,
                    Password = Password
                };
            }
            else
            {
                processInstaller = new ServiceProcessInstaller
                {
                    Account = ServiceAccount.NetworkService
                };
            }
            System.ServiceProcess.ServiceInstaller serviceInstaller = new ServiceInstaller
            {
                StartType = ServiceStartMode.Automatic,
                DisplayName = DisplayName,
                ServiceName = ServiceName,
                Description = Description
                
            };
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }

    /// <summary>
    /// Installer Class, uses InstalUtil.exe (but with a different API, the behaviour is defined here).
    /// </summary>
    public class AxisImportInstaller
    {
        /// <summary>
        /// Installs the Service
        /// </summary>
        /// <param name="install"></param>
        /// <param name="args"></param>
        public static void Install(string[] args)
        {
            using (AssemblyInstaller inst = new AssemblyInstaller(typeof(Program).Assembly, new string[0]))
            {
                ParseArguments(args);
                IDictionary state = new Hashtable();
                inst.UseNewContext = true;
                try
                {
                    inst.Install(state);
                    inst.Commit(state);
                }
                catch
                {
                    try
                    {
                        inst.Rollback(state);
                    }
                    catch
                    { }
                    throw;

                }
            }
        }

        /// <summary>
        /// UnInstalls the service
        /// </summary>
        /// <param name="args"></param>
        public static void UnInstall(string[] args)
        {
            using (AssemblyInstaller inst = new AssemblyInstaller(typeof(Program).Assembly, new string[0]))
            {
                ParseArguments(args);
                IDictionary state = new Hashtable();
                inst.UseNewContext = true;
                try
                {
                    inst.Uninstall(state);
                }
                catch
                {
                    try
                    {
                        inst.Rollback(state);
                    }
                    catch
                    { }
                    throw;

                }
            }

        }

        /// <summary>
        /// Simple argument parser. This is needed because of the demo/test deployments (installed twice on the same machine).
        /// </summary>
        /// <param name="args"></param>
        private static void ParseArguments(string[] args)
        {
            SInstaller.DisplayName = "EMSGAxisImportService";
            SInstaller.ServiceName = "EMSGAxisImportService";
            SInstaller.Description = "EMSG Service to import Interlis Axis Data";


            foreach (string s in args)
            {
                string param = "";
                string value = "";
                if (s.IndexOf(":") == -1) continue;
                param = s.Substring(0, s.IndexOf(":"));
                value = s.Substring(s.IndexOf(":") + 1);
                switch (param.ToLower())
                {
                    case "displayname":
                        SInstaller.DisplayName = value;
                        break;
                    case "servicename":
                        SInstaller.ServiceName = value;
                        break;
                    case "description":
                        SInstaller.Description = value;
                        break;
                    case "user":
                        SInstaller.UserName = value;
                        break;
                    case "password":
                        SInstaller.Password = value;
                        break;
                }
            }
        }
    }
}
