namespace ASTRA.EMSG.Business.Services.Security
{
    public interface IUserIdentityProvider
    {
        string UserName { get; }
        bool IsAuthenticated { get; }
    }
}