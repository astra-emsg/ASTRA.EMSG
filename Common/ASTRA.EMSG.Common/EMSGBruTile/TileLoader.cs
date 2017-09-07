using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BruTile;
using GeoAPI.Geometries;
using System.IO;
using Microsoft.Win32;
using System.Threading;
using Common.Logging;

namespace ASTRA.EMSG.Common.EMSGBruTile
{
    public class TileLoader
    {
        private ITileSource tileSource;
        public TileLoader(ITileSource source)
        {
            this.tileSource = source;
        }
        public TiledLayerInfo GetTiles(ILayerConfig layer, IGeometry bbox, string path, bool useCache, string format, double buffer = 0, int limit = 0, int maxThreads = 6, bool download = true)
        {
            Uri uri = new Uri(layer.Url);
            Extent extent = new Extent(bbox.EnvelopeInternal.MinX - buffer, bbox.EnvelopeInternal.MinY - buffer, bbox.EnvelopeInternal.MaxX + buffer, bbox.EnvelopeInternal.MaxY + buffer);
            IList<TileInfo> infos = new List<TileInfo>();
            List<ASTRA.EMSG.Common.EMSGBruTile.TileMatrix> matrixset = new List<ASTRA.EMSG.Common.EMSGBruTile.TileMatrix>();

            //sort by scale (low resolution first) and break if tiles go over limit (limit <= 0 means no limit)
            var orderedres = tileSource.Schema.Resolutions.OrderByDescending(r => r.Value.ScaleDenominator);
            int count = 0;
            foreach (var res in orderedres)
            {
                var tileinfos = tileSource.Schema.GetTilesInView(extent, res.Key);
                count += tileinfos.Count();
                if (limit > 0 && count > limit)
                {
                    break;
                }
                infos = infos.Union(tileinfos).ToList();
                ASTRA.EMSG.Common.EMSGBruTile.TileMatrix matrix = new ASTRA.EMSG.Common.EMSGBruTile.TileMatrix();
                matrix.Identifier = res.Value.Id;
                matrix.Left = res.Value.Left;
                matrix.ScaleDenominator = res.Value.ScaleDenominator;
                matrix.TileHeight = res.Value.TileHeight;
                matrix.TileWidth = res.Value.TileWidth;
                matrix.Top = res.Value.Top;
                matrixset.Add(matrix);
            }

            string extension = GetDefaultExtension(format);

            //Create Directory Structure
            foreach (TileInfo info in infos)
            {
                string filepath = Path.Combine(path, string.Format(@"{0}\{1}\{2}\{3}\", uri.Host, layer.Name, info.Index.Level, info.Index.Row));
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
            }

            List<string> filePaths = new List<string>();
            List<LoadError> errors = new List<LoadError>();
            if (download)
            {
                int listSize = (int)Math.Ceiling((decimal)(infos.Count() / maxThreads));
                var chunks = Chunk(infos, listSize);
                List<Thread> threads = new List<Thread>();
                List<LoadWorker> workers = new List<LoadWorker>();
                foreach (IEnumerable<TileInfo> chunk in chunks)
                {
                    LoadWorker worker = new LoadWorker(chunk, tileSource, path, useCache, layer.Name, uri.Host, extension);
                    workers.Add(worker);
                    Thread thread = new Thread(worker.DoWork);
                    threads.Add(thread);
                    thread.Start();
                }
                foreach (Thread thread in threads)
                {
                    thread.Join();
                }
                foreach (LoadWorker worker in workers)
                {
                    filePaths.AddRange(worker.FilePaths);
                    errors.AddRange(worker.Errors);
                }
            }
            int errorCount = errors.Count();
            if (errorCount > 0)
            {
                throw new Exception(String.Format("{0} error{1} while downloading {2}", errorCount,errorCount>1?"s":string.Empty , layer.Name));
            }

            TiledLayerInfo result = new TiledLayerInfo();
            result.Name = layer.Name;
            result.IsDefaultVisible = layer.IsDefaultVisible;
            result.Container = layer.LayerContainer;
            result.Order = layer.Order;
            result.AbsoluteFilePaths = filePaths.ToArray();
            result.RelativePath = String.Format(@"{0}\{1}\", uri.Host, layer.Name);
            result.BasePath = path;
            result.SourceUrl = result.RelativePath + @"{TileMatrix}\{TileRow}\{TileCol}" + extension;
            result.MatrixSet = matrixset;
            List<XMLKeyValuePair<string, string>> localization = new List<XMLKeyValuePair<string, string>>();
            foreach (var kvp in layer.LocalizationDict)
            {
                localization.Add(new XMLKeyValuePair<string, string>(kvp.Key, kvp.Value));
            }
            result.Localization = localization;
            return result;
        }

        public static string GetDefaultExtension(string mimeType)
        {
            string result;
            RegistryKey key;
            object value;

            key = Registry.ClassesRoot.OpenSubKey(@"MIME\Database\Content Type\" + mimeType, false);
            value = key != null ? key.GetValue("Extension", null) : null;
            result = value != null ? value.ToString() : string.Empty;

            return result;
        }

        private IEnumerable<IEnumerable<T>> Chunk<T>(IEnumerable<T> source, int chunksize)
        {
            while (source.Any())
            {
                yield return source.Take(chunksize);
                source = source.Skip(chunksize);
            }
        }
    }

    public class LoadError
    {
        public LoadError(Exception ex, TileInfo info)
        {
            this.Exception = Exception;
            this.TileInfo = info;
        }
        public Exception Exception { get; private set; }
        public TileInfo TileInfo { get; private set; }
    }

    public class LoadWorker
    {
        public List<string> FilePaths { get; private set; }
        public List<LoadError> Errors { get; private set; }
        private IEnumerable<TileInfo> infos;
        private ITileSource source;
        private string path;
        private bool useCache;
        private string layername;
        private string hostname;
        private string extension;

        public LoadWorker(IEnumerable<TileInfo> infos, ITileSource source, string path, bool useCache, string layername, string hostname, string extension)
        {
            this.infos = infos;
            this.path = path;
            this.useCache = useCache;
            this.layername = layername;
            this.hostname = hostname;
            this.extension = extension;
            this.source = source;
            this.FilePaths = new List<string>();
            this.Errors = new List<LoadError>();
        }

        public void DoWork()
        {
            foreach (TileInfo info in infos)
            {
                try
                {
                    string filename = string.Format("{0}{1}", info.Index.Col, extension);

                    string filepath = Path.Combine(path, string.Format(@"{0}\{1}\{2}\{3}\", hostname, layername, info.Index.Level, info.Index.Row));
                    string fullfilename = Path.Combine(filepath, filename);
                    if (!Directory.Exists(filepath))
                    {
                        Directory.CreateDirectory(filepath);
                    }
                    if (!File.Exists(fullfilename) || !useCache)
                    {
                        try
                        {
                            Byte[] bytes = source.Provider.GetTile(info);
                            System.IO.FileStream stream = new System.IO.FileStream(fullfilename, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                            stream.Write(bytes, 0, bytes.Length);
                            stream.Close();
                        }
                        catch (System.IO.IOException ex)
                        {
                            //if the file is currently donwloaded by a different user it should be available for this package as well 
                            ILog log = LogManager.GetLogger("ApplicationLogger");
                            log.Warn(string.Format("Tile on Path {0} is is being used by another process", fullfilename), ex);
                        }
                    }
                    FilePaths.Add(fullfilename);
                }
                catch (Exception ex)
                {
                    ILog log = LogManager.GetLogger("ApplicationLogger");
                    log.Error(string.Format("Unknown Error while downloading Tile: Level: {0}; Row: {1}; Column: {2} of Layer: {3}",info.Index.Level, info.Index.Row, info.Index.Col, this.layername), ex);
                    this.Errors.Add(new LoadError(ex, info));                   
                }
            }
        }
    }
}
