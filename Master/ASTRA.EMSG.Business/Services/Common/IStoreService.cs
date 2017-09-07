namespace ASTRA.EMSG.Business.Services.Common
{
    public interface IStoreService
    {
        object this[string key] { get; set; }
    }
}