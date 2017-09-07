using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace ASTRA.EMSG.Business.Interlis.AxisImportScanner
{
    public class NextImport
    {
        public String FullFilename;
        public String FullLogFilename;
        public String FullSaveFilename;
        public DateTime SenderTimestamp;
    }


    public class ImportFolders
    {
        private readonly string importBaseDir;

        public ImportFolders(string importBaseDir)
        {
            this.importBaseDir = importBaseDir;
        }

        private void CheckExistsFolder(string folderPath)
        {
            if (!System.IO.Directory.Exists(folderPath))
            {
                throw new IOException("Required Folder " + folderPath + " does not exist");
            }
        }

        private bool HasWriteAccessToFolder(string folderPath)
        {
            try
            {
                System.Security.AccessControl.DirectorySecurity ds = Directory.GetAccessControl(folderPath);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        private void CheckHasWriteAccessToFolder(string folderPath)
        {
            if (!HasWriteAccessToFolder(folderPath))
            {
                throw new IOException("No write-access to "+ folderPath);
            }
        }

        public string GetInFolder()
        {
            return System.IO.Path.Combine(this.importBaseDir, "In");
        }

        public string GetSaveFolder()
        {
            return System.IO.Path.Combine(this.importBaseDir, "Save");
        }

        public string GetOutFolder()
        {
            return System.IO.Path.Combine(this.importBaseDir, "Out");
        }

        public void CheckFolderStructure()
        {
            CheckExistsFolder(GetInFolder());
            CheckExistsFolder(GetSaveFolder());
            CheckExistsFolder(GetOutFolder());

            CheckHasWriteAccessToFolder(GetSaveFolder());
            CheckHasWriteAccessToFolder(GetOutFolder());
        }

        // example: 36b9a4a6-5a25-4e40-b8e4-c3ebb427943b_20120301080529_Axis.xtf
        private static Regex re = new Regex(@"_(?<Year>[0-9][0-9][0-9][0-9])(?<Month>[0-9][0-9])(?<Day>[0-9][0-9])(?<Hour>[0-9][0-9])(?<Minute>[0-9][0-9])(?<Second>[0-9][0-9])_Axis", RegexOptions.Singleline);

        public static DateTime? ParseSenderDate(string file)
        {
            string fileName = System.IO.Path.GetFileNameWithoutExtension(file);

            MatchCollection mCol = re.Matches(fileName); 

            if (mCol.Count != 1 || mCol[0].Groups.Count != 7)
            {
                return null;
            }

            int year = Int32.Parse(mCol[0].Groups["Year"].Value);
            int month = Int32.Parse(mCol[0].Groups["Month"].Value);
            int day = Int32.Parse(mCol[0].Groups["Day"].Value);
            int hour = Int32.Parse(mCol[0].Groups["Hour"].Value);
            int minute = Int32.Parse(mCol[0].Groups["Minute"].Value);
            int second = Int32.Parse(mCol[0].Groups["Second"].Value);

            return new DateTime(year, month, day, hour, minute, second);
        }


        /// <summary>
        /// This Function delivers the first (means: oldest) VALID file in the list (if checkFileValidity is true).
        /// This flag can be changed to false (or removed) if it is assured that only valid files are deployed in the import folder.
        /// </summary>
        /// <param name="files">List of strings with the filenames</param>
        /// <param name="checkFileValidity">bool. if set to true (default) this will perform a simple validity check.</param>
        /// <returns>string with the oldest file.</returns>
        public static string GetOldestFile(List<string> files)
        {
            SortedDictionary<DateTime?, string> sortedFiles = new System.Collections.Generic.SortedDictionary<DateTime?, string>();

            foreach (string file in files)
            {
                DateTime? date = ParseSenderDate(file);
                if (date == null)
                    continue;
                if (Parser.AxisReader2.CheckFileIsValid(file))
                    sortedFiles.Add(date, file);

            }

            return sortedFiles.Count > 0 ? sortedFiles.First().Value : null;
        }

        /// <summary>
        /// check if there are files to be processed in the In-Folder
        /// </summary>
        /// <returns>null, if there are none</returns>
        public NextImport GetNextImport()
        {
            CheckFolderStructure();

            List<string> files = System.IO.Directory.GetFiles(GetInFolder(), "*.xtf").ToList();

            string filename = GetOldestFile(files);
            if (filename == null)
            {
                return null;
            }

            NextImport nextImport = new NextImport();
            //nextImport.FullFilename = System.IO.Path.Combine(this.importBaseDir, filename);
            nextImport.FullFilename = filename;
            nextImport.FullLogFilename = System.IO.Path.Combine(GetOutFolder(), "" + System.IO.Path.GetFileNameWithoutExtension(filename) + ".log");
            nextImport.FullSaveFilename = System.IO.Path.Combine(GetSaveFolder(), "" + System.IO.Path.GetFileName(filename));
            // once again, but cheaper than moving it around.
            nextImport.SenderTimestamp = ParseSenderDate(filename).Value;
            return nextImport;
        }
    }
}
