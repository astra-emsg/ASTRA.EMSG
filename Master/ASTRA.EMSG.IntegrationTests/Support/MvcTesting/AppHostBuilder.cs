using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Master.Logging;
using Microsoft.Build.Evaluation;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using MvcIntegrationTestFramework.Hosting;

namespace ASTRA.EMSG.IntegrationTests.Support.MvcTesting
{
    public static class AppHostBuilder
    {
        private static readonly string testOutPutDir = TestDeploymentHelper.GetTestOutputDir();

        public static AppHost AppHost
        {
            get
            {
                if (appHost == null)
                {
                    BuildHost();
                }
                return appHost;
            }
        }


        public static string AppPhysicalDirectory { get; private set; }

        private static bool hasDeployed;
        private static AppHost appHost;

        public static void DeployApp()
        {
            AppPhysicalDirectory = Path.Combine(testOutPutDir, "Web");
            DeployMVCApplication(AppPhysicalDirectory);
            hasDeployed = true;
        }

        public static void BuildHost()
        {
            if (!hasDeployed)
                DeployApp();

            appHost = new AppHost(AppPhysicalDirectory);
        }

        private static void DeployMVCApplication(string mvcProjectPath)
        {
            //Publishing
            PublishWebProject(mvcProjectPath);

            //Copy additional Dll -s
            foreach (var file in Directory.GetFiles(testOutPutDir))
            {
                string targetFile = Path.Combine(testOutPutDir, "Web", "Bin", Path.GetFileName(file));
                if(!File.Exists(targetFile))
                    File.Copy(file, targetFile);

                RemoveReadOnlyRecoursive(new FileInfo(targetFile));
            }
            
            //Copy Web.config and Global.asax
            string webconfigPath = Path.Combine(mvcProjectPath, "web.config");

            //Handle Web.config
            var webconfig = XDocument.Load(webconfigPath);
            var appSettings = webconfig.Descendants("appSettings").FirstOrDefault();
            if (appSettings != null)
            {
                var dictionary = new Dictionary<string, string>
                {
                    { "Environment", ApplicationEnvironment.SpecFlow.ToString() },
                    { "UsePrecompiledViews", "true" },
                    { "SecurityCacheTimeout" ,"0"},
                    { "owin:AutomaticAppStartup", "false" }
                };
                foreach (var setting in dictionary)
                {
                    var environment = appSettings.Descendants("add").Where(n => n.Attribute("key").Value == setting.Key).SingleOrDefault();
                    if (environment != null)
                        environment.Attribute("value").Value = setting.Value;
                }
            }

            var connectionStrings = webconfig.Descendants("connectionStrings").FirstOrDefault();
            if (connectionStrings != null)
            {
                var dictionary = new Dictionary<string, string>
                {
                    { "Development", new TestConfigProvider().ConnectionString },
                };
                foreach (var setting in dictionary)
                {
                    var environment = connectionStrings.Descendants("add").Where(n => n.Attribute("name").Value == setting.Key).SingleOrDefault();
                    if (environment != null)
                        environment.Attribute("connectionString").Value = setting.Value;
                }
            }
            var customErrors = webconfig.Descendants("customErrors").FirstOrDefault();
            if (customErrors != null)
                customErrors.Attribute("mode").Value = "Off";

            webconfig.Save(webconfigPath);
        }

        private static void RemoveReadOnlyRecoursive(FileSystemInfo fileSystemInfo)
        {
            fileSystemInfo.Attributes = FileAttributes.Normal;
            var di = fileSystemInfo as DirectoryInfo;

            if (di != null)
            {
                foreach (var dirInfo in di.GetFileSystemInfos())
                {
                    RemoveReadOnlyRecoursive(dirInfo);
                }
            }
        }

        private static void PublishWebProject(string mvcProjectPath)
        {
            string webProjectFile = Path.Combine(TestDeploymentHelper.GetTestOutputDir(), ConfigurationManager.AppSettings["WebProjectFile"]);

            if (!File.Exists(webProjectFile))
                throw new Exception(string.Format("Web project file {0} not found!", webProjectFile));

            var project = new Project(webProjectFile, null, null, new ProjectCollection());

            string msBuildPath =
                Environment.ExpandEnvironmentVariables(string.Format(@"%WinDir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"));

            //Console.WriteLine("Invoke MsBuild from {0}", msBuildPath);

            ProcessStartInfo psi = new ProcessStartInfo(msBuildPath,
                                                        string.Format(
                                                            "{0} /t:PublishToFileSystem \"/p:PublishDestination={1}\"",
                                                            project.FullPath, mvcProjectPath));
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            var p = Process.Start(psi);

            string readToEnd = p.StandardOutput.ReadToEnd();

            p.WaitForExit();

            if (p.ExitCode > 0)
            {
                var trimmedOutput = readToEnd.Substring(Math.Max(0, readToEnd.Length - 1000));
                Console.WriteLine(trimmedOutput);
                var tempFileName = Path.GetTempFileName();
                File.WriteAllText(tempFileName, readToEnd);
                throw new Exception(string.Format("Build failed, see details in {0}: " + trimmedOutput, tempFileName));
            }
        }

        private static void CopyToBin(string mvcProjectPath, string dll)
        {
            string file = Path.Combine(testOutPutDir, dll);
            string destFileName = Path.Combine(mvcProjectPath, "bin", Path.GetFileName(file));
            CopyIfNewer(file, destFileName);
        }

        private static void CopyIfNewer(string file, string destFileName)
        {
            if (!File.Exists(destFileName) || File.GetCreationTimeUtc(destFileName) != File.GetCreationTimeUtc(file))
                File.Copy(file, destFileName, true);
        }

        private static void EnsureDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
        }
    }
}