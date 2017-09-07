using System;

namespace ASTRA.EMSG.Business.Services.Common
{
    public interface IHttpRequestService : IStoreService
    {
        Guid? LastErrorTrackId { get; set; }
        Exception LastException { get; set; }
    }
}