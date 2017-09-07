using System;

namespace ASTRA.EMSG.Mobile.Installer.Packaging
{
    /// <summary>
    /// A Zip event
    /// </summary>
    public class ZipEventArgs : EventArgs
    {
        /// <summary>
        /// Total bytes to zip
        /// </summary>
        public long TotalBytes { get; set; }
        /// <summary>
        /// Bytes zipped
        /// </summary>
        public long BytesZipped { get; set; }
        /// <summary>
        /// Total files to zip
        /// </summary>
        public int TotalFiles { get; set; }
        /// <summary>
        /// Fileld Zipped
        /// </summary>
        public int FileNumber { get; set; }
    }
}