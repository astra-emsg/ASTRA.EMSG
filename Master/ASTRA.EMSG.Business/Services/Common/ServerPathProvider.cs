using System.Web;

namespace ASTRA.EMSG.Business.Services.Common
{
    public interface IServerPathProvider : IService
    {
        string MapPath(string path);
    }

    public class ServerPathProvider : IServerPathProvider
    {
        public string MapPath(string path)
        {
            return HttpContext.Current.Server.MapPath(path);
        }
    }
}