using System;
using System.IO;
using System.Linq;
using ASTRA.EMSG.Common.Utils;
using ASTRA.EMSG.Mobile.Installer.Packaging;
using Mono.Cecil;
using Mono.Collections.Generic;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Mobile.InstallerCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            string applicationPath = @"..\..\..\ASTRA.EMSG.Mobile\Bin\Debug";
            string manualInstallerPath = @"..\..\..\ASTRA.EMSG.Mobile.Installer\Bin\Debug";

            if(args.Length >= 2)
            {
                applicationPath = args[0];
                manualInstallerPath = args[1];
            }

            string installerDropLocation = "Installer";

            string installerExeName = "ASTRA.EMSG.Mobile.Installer.exe";
            string packageResourceName = "ASTRA.EMSG.Mobile.Installer.InstallerPackage.Application.pkg";
            string applicationPkg = "Application.pkg";
            string installerExe = "ASTRA.EMSG.Mobile.exe";

            if (Directory.Exists(installerDropLocation))
                Directory.Delete(installerDropLocation, true);

            Directory.CreateDirectory(installerDropLocation);

            var germanResx = Path.Combine(installerDropLocation, FileNameConstants.GermanMobileLocalizationFileName);
            var italianResx = Path.Combine(installerDropLocation, FileNameConstants.ItalyMobileLocalizationFileName);
            var frenchResx = Path.Combine(installerDropLocation, FileNameConstants.FrenchMobileLocalizationFileName);

            File.Copy(Path.Combine(applicationPath, "Resources", FileNameConstants.GermanMobileLocalizationFileName), germanResx, true);
            File.Copy(Path.Combine(applicationPath, "Resources", FileNameConstants.ItalyMobileLocalizationFileName), italianResx, true);
            File.Copy(Path.Combine(applicationPath, "Resources", FileNameConstants.FrenchMobileLocalizationFileName), frenchResx, true);

            //Creating Package
            var packaging = new Packaging();
            string packageFilePath = Path.Combine(installerDropLocation, applicationPkg);
            ReturnResult returnResult = packaging.PackageFolder(applicationPath, packageFilePath, true);
            if (!returnResult.Success)
            {
                Console.WriteLine(returnResult.Message);
                Console.ReadLine();
            }

            //Injecting Package to the Installer
            string installerExePath = Path.Combine(installerDropLocation, installerExeName);
            File.Copy(Path.Combine(manualInstallerPath, installerExeName), installerExePath, true);

            byte[] bytes;
            using (var fs = new FileStream(packageFilePath, FileMode.Open))
            {
                bytes = fs.ReadAllByte();
            }

            AssemblyDefinition asdDefinition = AssemblyDefinition.ReadAssembly(installerExePath);
            EmbeddedResource embeddedResource = new EmbeddedResource(packageResourceName, ManifestResourceAttributes.Public, bytes);

            Collection<Resource> resources = asdDefinition.MainModule.Resources;
            Resource pkg = resources.Single(r => r.Name == packageResourceName);
            resources.Remove(pkg);
            resources.Add(embeddedResource);

            asdDefinition.Write(Path.Combine(installerDropLocation, installerExe));

            CopyDirectory(Path.Combine(applicationPath, "Help"), Path.Combine(installerDropLocation, "Help"));
            CopyDirectory(Path.Combine(applicationPath, "Mobile"), Path.Combine(installerDropLocation, "Mobile"));
        }

        private static void CopyDirectory(string sourceDirectory, string targetDirectory)
        {
            var directories = Directory.GetDirectories(sourceDirectory, "*", SearchOption.AllDirectories);
            foreach (string dirPath in directories)
            {
                var destinationDirectoryName = dirPath.Replace(sourceDirectory, targetDirectory);
                if (!Directory.Exists(destinationDirectoryName))
                    Directory.CreateDirectory(destinationDirectoryName);
            }

            var files = Directory.GetFiles(sourceDirectory, "*.*", SearchOption.AllDirectories);
            foreach (string newPath in files)
            {
                var destFileName = newPath.Replace(sourceDirectory, targetDirectory);
                File.Copy(newPath, destFileName, true);
            }
        }
    }
}
