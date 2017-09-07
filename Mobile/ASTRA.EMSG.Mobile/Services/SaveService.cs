using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ASTRA.EMSG.Common.Mobile.Utils;
using Ionic.Zip;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Mobile.Services
{
    public interface ISaveService
    {

        void Save(string saveFilepath);
        void Save(string packagefolderpath, string saveFilePath);
        void SaveCurrentPackage();
    }
    public class SaveService : ISaveService
    {
        private readonly IImportService importService;
        private readonly IClientConfigurationProvider clientConfigurationProvider;
        private readonly IPackageService packageService;
        
        public SaveService(IImportService importService, IClientConfigurationProvider clientConfigurationProvider, IPackageService packageService)
        {
            this.clientConfigurationProvider = clientConfigurationProvider;
            this.importService = importService;
            this.packageService = packageService;
        }

        public void Save(string saveFilepath)
        {
            Save(clientConfigurationProvider.PackageFolderPath, saveFilepath);
        }
        private void copyTemporaryToPackagefolder(string packagefolderpath, string temporaryFolder)
        {
            string[] allfilePaths = Directory.GetFiles(temporaryFolder, "*",
                                              SearchOption.AllDirectories);

            var filePathServerPackageDescriptor = allfilePaths.Where(p => Path.GetFileName(p) == FileNameConstants.ServerPackageDescriptorFileName);
            var filePathsShapes = allfilePaths.Where(p => Path.GetExtension(p) == ".shp");
            var filePathsdbf = allfilePaths.Where(p => Path.GetExtension(p) == ".dbf");
            var filePathsshx = allfilePaths.Where(p => Path.GetExtension(p) == ".shx");
            var filePathsDTO = allfilePaths.Where(p => Path.GetExtension(p) == ".model");
            var filePathsTiffs = allfilePaths.Where(p => Path.GetExtension(p) == ".tif");
            var filePathsLegend = allfilePaths.Where(p => Path.GetExtension(p) == ".legend");

            var filePaths = filePathServerPackageDescriptor.Concat(filePathsShapes).Concat(filePathsDTO).Concat(filePathsdbf).Concat(filePathsshx).Concat(filePathsTiffs).Concat(filePathsLegend);

            var resourceAndHelpFolders = Directory.GetDirectories(temporaryFolder);

            foreach (string file in filePaths)
            {
                FileHelper.CopyFile(new FileInfo(file), new DirectoryInfo(packagefolderpath));
            }
            foreach (string folder in resourceAndHelpFolders)
            {
                FileHelper.CopyAll(new DirectoryInfo(folder), new DirectoryInfo(Path.Combine(packagefolderpath, Path.GetDirectoryName(folder))));
            }

        }
        public void SaveCurrentPackage()
        {
            var currentTemporaryFolder  = clientConfigurationProvider.CurrentTemporaryFolder;
            var packagefolderpath = clientConfigurationProvider.PackageFolderPath;
            if (!string.IsNullOrEmpty(currentTemporaryFolder))
            {
                //Check Package Descriptor, are there Inspektionsroutes?
                //If yes save the Working Dir (TempFolder) to the PackageFolder
                //If not, delete the Package Folder (All Inspektionsrouten are already exported)
                var descriptor = packageService.GetSeverpackageDescriptor(Directory.GetFiles(currentTemporaryFolder, FileNameConstants.ServerPackageDescriptorFileName,
                                                  SearchOption.AllDirectories).SingleOrDefault());

                if (descriptor.Inspektionsrouten.Any())
                {
                    copyTemporaryToPackagefolder(packagefolderpath, currentTemporaryFolder);
                }
                else
                {
                    FileHelper.TryDeleteFiles(new DirectoryInfo(clientConfigurationProvider.PackageFolderPath));
                }                
            }
        }
        public void Save(string packagefolderpath, string saveFilePath)
        {
            var temporaryFolder=clientConfigurationProvider.CurrentTemporaryFolder;
            if ( !string.IsNullOrEmpty(temporaryFolder))
            {
                copyTemporaryToPackagefolder(packagefolderpath, temporaryFolder);
                var resourceAndHelpPackageFolders = Directory.GetDirectories(packagefolderpath);

                string[] fileNames = Directory.GetFiles(packagefolderpath, "*", SearchOption.TopDirectoryOnly);
                using (var zipFile = new ZipFile())
                {

                    zipFile.AddFiles(fileNames, false, string.Empty);
                    foreach (string folder in resourceAndHelpPackageFolders)
                    {
                        //Path.GetFileName gets the folder name to preserve the structure in the zipfile
                        zipFile.AddDirectory(folder, Path.GetFileName(folder));
                       
                    }


                    zipFile.Save(saveFilePath);
                };
            }
        }
    }
}
