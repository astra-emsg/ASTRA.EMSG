using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Models.Administration
{
    public class UserInfoModel : IModel
    {
        public string CurrentUserName { get; set; }
        public string CurrentMandantName { get; set; }
        public string CurrentRollen { get; set; }
    }
}
