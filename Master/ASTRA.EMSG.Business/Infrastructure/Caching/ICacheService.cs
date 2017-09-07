namespace ASTRA.EMSG.Business.Infrastructure.Caching
{
    public interface ICacheService
    {
        object this[string key, int timeoutInSeconds] { set; }
        object this[string key] { get; set; }
        void Reset(string key);
    }
}