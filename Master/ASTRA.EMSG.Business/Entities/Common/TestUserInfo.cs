using ASTRA.EMSG.Business.Entities.Mapping;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Entities.Common
{
    [TableShortName("TUI")]
    public class TestUserInfo : Entity
    {
        public const string IntegrationTestUserName = "Test";
                            
        public virtual string UserName { get; set; }
        public virtual string Mandator { get; set; }
        public virtual Rolle Rolle { get; set; }

        public override string ToString()
        {
            return string.Format("UserName: {0}, Mandator: {1}, Rolle: {2}", UserName, Mandator, Rolle);
        }
    }
}