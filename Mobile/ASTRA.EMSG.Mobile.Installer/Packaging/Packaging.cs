using System;
using System.IO;
using System.IO.Packaging;
using System.Linq;

namespace ASTRA.EMSG.Mobile.Installer.Packaging
{
    /// <summary>
    /// Utility class for Zipping and Unzipping files and folders into XPS Packages
    /// This class uses XML Paper Specification and adds [Content_Types].xml to the package
    /// </summary>
    public class Packaging
    {
        #region Events
        private delegate void AsyncStartPackageFolder(string folderName, string compressedFileName, bool overrideExisting);
        private delegate void AsyncStartPackageFile(string fileName, string compressedFileName, bool overrideExisting);
 
        /// <summary>
        /// Add a folder along with its subfolders to a Package Asynchronously
        /// </summary>
        /// <param name="folderName">The folder to add</param>
        /// <param name="compressedFileName">The package to create</param>
        /// <param name="overrideExisting">Override exsisitng files</param>
        public void PackageFolderAsync(string folderName, string compressedFileName, bool overrideExisting)
        {
            AsyncStartPackageFolder methodCall = new AsyncStartPackageFolder(StartPackageFolder);
            methodCall.BeginInvoke(folderName,
                                   compressedFileName,
                                   overrideExisting,
                                   new AsyncCallback(RunZipComplete),
                                   compressedFileName);
        }
 
        /// <summary>
        /// Compress a file into a ZIP archive as the container store Asynchronously
        /// </summary>
        /// <param name="fileName">The file to compress</param>
        /// <param name="compressedFileName">The archive file</param>
        /// <param name="overrideExisting">override existing file</param>
        public void PackageFileAsync(string fileName, string compressedFileName, bool overrideExisting)
        {
            AsyncStartPackageFolder methodCall = new AsyncStartPackageFolder(StartPackageFile);
            methodCall.BeginInvoke(fileName,
                                   compressedFileName,
                                   overrideExisting,
                                   new AsyncCallback(RunZipComplete),
                                   compressedFileName);
        }
 
        private void StartPackageFolder(string folderName, string compressedFileName, bool overrideExisting)
        {
            PackageFolder(folderName, compressedFileName, overrideExisting);
        }
 
        private void StartPackageFile(string fileName, string compressedFileName, bool overrideExisting)
        {
            PackageFile(fileName, compressedFileName, overrideExisting);
        }
 
        private void RunZipComplete(IAsyncResult result)
        {
 
        }
 
        public event ZipEventHandler Zip;
 
        protected virtual void OnZip(ZipEventArgs e)
        {
            if (Zip != null)
                Zip(this, e);
        }
 
        public event ZipEndEventHandler ZipEnd;
 
        protected virtual void OnZipEnd(ZipEndEventArgs e)
        {
            if (ZipEnd != null)
                ZipEnd(this, e);
        }
        #endregion
 
        private const string PackageRelationshipType = @"http://schemas.microsoft.com/opc/2006/sample/document";
        private const string ResourceRelationshipType = @"http://schemas.microsoft.com/opc/2006/sample/required-resource";
 
        /// <summary>
        /// Add a folder along with its subfolders to a Package
        /// </summary>
        /// <param name="folderName">The folder to add</param>
        /// <param name="compressedFileName">The package to create</param>
        /// <param name="overrideExisting">Override exsisitng files</param>
        /// <returns>ReturnResult</returns>
        public ReturnResult PackageFolder(string folderName, string compressedFileName, bool overrideExisting)
        {
            if (folderName.EndsWith(@"\"))
                folderName = folderName.Remove(folderName.Length - 1);
            ReturnResult result = new ReturnResult();
            if (!Directory.Exists(folderName))
            {
                result.Message = "Source folder doesnt exist in" + folderName;
                return result;
            }
 
            if (!overrideExisting && File.Exists(compressedFileName))
            {
                result.Message = "Destination file " + compressedFileName + " cannot be overwritten";
                return result;
            }
            try
            {
                using (Package package = Package.Open(compressedFileName, FileMode.Create))
                {
                    var fileList = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);
                    ZipEventArgs zipArgs = new ZipEventArgs() { TotalFiles = fileList.Count(), FileNumber = 0, BytesZipped = 0, TotalBytes = 0 };
                    foreach (string fileName in fileList)
                    {
                        zipArgs.FileNumber++;
                        if (Path.GetFileName(fileName).IndexOfAny(Path.GetInvalidFileNameChars()) > -1)
                            continue;
                        //The path in the package is all of the subfolders after folderName
                        string pathInPackage;
                        pathInPackage = Path.GetDirectoryName(fileName).Replace(folderName, string.Empty) + "/" + Path.GetFileName(fileName);
 
                        Uri partUriDocument = PackUriHelper.CreatePartUri(new Uri(pathInPackage, UriKind.Relative));
                        PackagePart packagePartDocument = package.CreatePart(partUriDocument, System.Net.Mime.MediaTypeNames.Text.Xml, CompressionOption.Maximum);
                        using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                        {
                            fileStream.CopyTo(packagePartDocument.GetStream());
                            zipArgs.BytesZipped += fileStream.Length;
                            OnZip(zipArgs);
                        }
                        package.CreateRelationship(packagePartDocument.Uri, TargetMode.Internal, PackageRelationshipType);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error zipping folder " + folderName, e);
            }
            OnZipEnd(new ZipEndEventArgs());
            result.Success = true;
            result.Message = "OK";
            return result;
        }
 
        /// <summary>
        /// Compress a file into a ZIP archive as the container store
        /// </summary>
        /// <param name="fileName">The file to compress</param>
        /// <param name="compressedFileName">The archive file</param>
        /// <param name="overrideExisting">override existing file</param>
        /// <returns>ReturnResult</returns>
        public ReturnResult PackageFile(string fileName, string compressedFileName, bool overrideExisting)
        {
            ReturnResult result = new ReturnResult();
 
            if (!File.Exists(fileName))
            {
                result.Message = "Source file doesnt exist in" + fileName;
                return result;
            }
 
            if (!overrideExisting && File.Exists(compressedFileName))
            {
                result.Message = "Destination file " + compressedFileName + " cannot be overwritten";
                return result;
            }
 
            try
            {
                Uri partUriDocument = PackUriHelper.CreatePartUri(new Uri(Path.GetFileName(fileName), UriKind.Relative));
 
                using (Package package = Package.Open(compressedFileName, FileMode.Create))
                {
                    PackagePart packagePartDocument = package.CreatePart(partUriDocument, System.Net.Mime.MediaTypeNames.Text.Xml, CompressionOption.Maximum);
                    using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    {
                        ZipEventArgs zipArgs = new ZipEventArgs() { TotalFiles = 1, FileNumber = 1, TotalBytes = new FileInfo(fileName).Length, BytesZipped = 0 };
                        OnZip(zipArgs);
                        fileStream.CopyTo(packagePartDocument.GetStream());
                        zipArgs.BytesZipped += fileStream.Length;
                        OnZip(zipArgs);
                    }
                    package.CreateRelationship(packagePartDocument.Uri, TargetMode.Internal, PackageRelationshipType);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error zipping file " + fileName, e);
            }
            OnZipEnd(new ZipEndEventArgs());
            result.Success = true;
            result.Message = "OK";
            return result;
        }
 
        /// <summary>
        /// Extract a container Zip. NOTE: container must be created as Open Packaging Conventions (OPC) specification.
        /// Delete [Content_Types].xml when done
        /// </summary>
        /// <param name="folderName">The folder to extract the package to</param>
        /// <param name="compressedFileName">The package file</param>
        /// <param name="overrideExisting">override existing files</param>
        /// <returns>ReturnResult</returns>
        public ReturnResult UncompressFile(string folderName, string compressedFileName, bool overrideExisting)
        {
            return UncompressFile(folderName, compressedFileName, overrideExisting, true);
        }
 
        /// <summary>
        /// Extract a container Zip. NOTE: container must be created as Open Packaging Conventions (OPC) specification
        /// </summary>
        /// <param name="folderName">The folder to extract the package to</param>
        /// <param name="compressedFileName">The package file</param>
        /// <param name="overrideExisting">override existing files</param>
        /// <param name="removeDescFile">Delete [Content_Types].xml when done</param>
        /// <returns>ReturnResult</returns>
        public ReturnResult UncompressFile(string folderName, string compressedFileName, bool overrideExisting, bool removeDescFile)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                if (!File.Exists(compressedFileName))
                {
                    result.Success = false;
                    result.Message = "Compressed File not found";
                    return result;
                }
 
                DirectoryInfo directoryInfo = new DirectoryInfo(folderName);
                if (!directoryInfo.Exists)
                    directoryInfo.Create();
 
                using (Package package = Package.Open(compressedFileName, FileMode.Open, FileAccess.Read))
                {
                    PackagePart documentPart = null;
                    PackagePart resourcePart = null;
 
                    Uri uriDocumentTarget = null;
                    foreach (PackageRelationship relationship in package.GetRelationshipsByType(PackageRelationshipType))
                    {
                        uriDocumentTarget = PackUriHelper.ResolvePartUri(new Uri("/", UriKind.Relative), relationship.TargetUri);
                        documentPart = package.GetPart(uriDocumentTarget);
                        ExtractPart(documentPart, folderName, overrideExisting);
                    }
                    if (documentPart != null)
                    {
 
                        Uri uriResourceTarget = null;
                        foreach (PackageRelationship relationship in documentPart.GetRelationshipsByType(ResourceRelationshipType))
                        {
                            uriResourceTarget = PackUriHelper.ResolvePartUri(documentPart.Uri, relationship.TargetUri);
                            resourcePart = package.GetPart(uriResourceTarget);
                            ExtractPart(resourcePart, folderName, overrideExisting);
                        }
                    }
                }
 
                if (removeDescFile && File.Exists(folderName + "\\[Content_Types].xml"))
                    File.Delete(folderName + "\\[Content_Types].xml");
            }
            catch (Exception e)
            {
                throw new Exception("Error unzipping file " + compressedFileName, e);
            }
            OnZipEnd(new ZipEndEventArgs());
            result.Success = true;
            result.Message = "OK";
            return result;
        }
 
        private void ExtractPart(PackagePart packagePart, string targetDirectory, bool overrideExisting)
        {
            string stringPart = targetDirectory + Uri.UnescapeDataString(packagePart.Uri.ToString()).Replace('\\', '/');
 
            if (!Directory.Exists(Path.GetDirectoryName(stringPart)))
                Directory.CreateDirectory(Path.GetDirectoryName(stringPart));
 
            if (!overrideExisting && File.Exists(stringPart))
                return;
            using (FileStream fileStream = new FileStream(stringPart, FileMode.Create))
            {
                packagePart.GetStream().CopyTo(fileStream);
            }
        }
    }
}