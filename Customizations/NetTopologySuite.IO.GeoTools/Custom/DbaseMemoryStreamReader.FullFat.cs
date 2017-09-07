using System;
using System.IO;
using NetTopologySuite.Utilities;

namespace NetTopologySuite.IO
{
    public partial class DbaseMemoryStreamReader
    {
        private partial class DbaseFileEnumerator 
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DbaseFileEnumerator"/> class.
            /// </summary>
            /// <param name="parent"></param>
            public DbaseFileEnumerator(DbaseMemoryStreamReader parent)
            {
                _parent = parent;
                
                //FileStream stream = new FileStream(parent._filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                //_dbfStream = new BinaryReader(stream, PlatformUtilityEx.GetDefaultEncoding());
                _dbfStream = new BinaryReader(_parent._inputStream, PlatformUtilityEx.GetDefaultEncoding());
                ReadHeader();
            }


            #region IDisposable Members

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, 
            /// or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                _dbfStream.Close();
            }

            #endregion
        }

        /// <summary>
        /// Initializes a new instance of the DbaseMemoryStreamReader class.
        /// </summary>
        /// <param name="filename"></param>
        public DbaseMemoryStreamReader(Stream inputStream)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException();
            }

            _inputStream = inputStream;

        }


        /// <summary>
        /// Gets the header information for the dbase file.
        /// </summary>
        /// <returns>DbaseFileHeader contain header and field information.</returns>
        public DbaseFileHeader GetHeader()
        {
            if (_header == null)
            {
                //FileStream stream = new FileStream(_filename, FileMode.Open, FileAccess.Read);
                BinaryReader dbfStream = new BinaryReader(_inputStream, PlatformUtilityEx.GetDefaultEncoding());

                _header = new DbaseFileHeader();
                // read the header
                _header.ReadHeader(dbfStream);


                //TODO: SEEK?
                _inputStream.Seek(0, SeekOrigin.Begin);

                //dbfStream.Close();
                //stream.Close();

            }
            return _header;
        }
    }
}
