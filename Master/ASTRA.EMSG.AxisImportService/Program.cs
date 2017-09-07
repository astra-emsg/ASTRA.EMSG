using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace ASTRA.EMSG.AxisImportService
{
    static class Program
    {

        public static string[] __helpLines = new string[]
        {
            "-------------------------",
            "AxisImportService",
            "-------------------------",
            "",
            "kurze Hilfe, interaktiv (via CMD):",
            "AxisImportService.exe --install  \tinstalliert den Windows Dienst",
            "AxisImportService.exe --uninstall\t...deinstalliert den Dienst",
            "AxisImportService.exe --interactive\t startet den Dienst im Applikationsmodus",
            "",
            "weitere Parameter für --install und --uninstall:",
            "user:USERNAME",
            "password:PASSWORD",
            "servicename:SERVICENAME",
            "displayname:DISPLAYNAME",
            "description:DESCRIPTION",
            "",
            "",
            "WICHTIG: zum Installieren werden ADMINISTRATOR-RECHTE benötigt!"
        };
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args.Contains("--install"))
                {
                    AxisImportInstaller.Install(args);
                }
                else if (args.Contains("--uninstall"))
                {
                    AxisImportInstaller.UnInstall(args);
                }
                else if (args.Contains("--interactive"))
                {
                    AxisImportService xTest = new AxisImportService();
                    try
                    {
                        xTest.Start();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex + "\n" + ex.StackTrace);
                        System.Environment.Exit(1);
                    }
                    Console.WriteLine("Press any key to stop...");
                    Console.ReadLine();
                    Console.WriteLine("shutting down...");
                    xTest.Stop();
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White;
                    foreach (string s in __helpLines)
                        Console.WriteLine(s);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }



            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
			    { 
				    new AxisImportService() 
			    };
                ServiceBase.Run(ServicesToRun);
            }

        }
    }
}
