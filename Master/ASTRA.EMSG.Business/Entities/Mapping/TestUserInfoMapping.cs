using ASTRA.EMSG.Business.Entities.Common;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class TestUserInfoMapping : ClassMapBase<TestUserInfo>
    {
        public TestUserInfoMapping()
        {
            MapTo(b => b.UserName);
            MapTo(b => b.Mandator);
            MapTo(b => b.Rolle);
        }

        protected override string TableName
        {
            get { return "TestUser"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.AdministrationParameter; }
        }
    }
}