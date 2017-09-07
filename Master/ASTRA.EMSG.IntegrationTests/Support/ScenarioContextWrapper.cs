using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using TechTalk.SpecFlow;

namespace ASTRA.EMSG.IntegrationTests.Support
{
    public static class ScenarioContextWrapper
    {
        public static NHibernateReadWriteTransactionScope CurrentScope
        {
            get { return (NHibernateReadWriteTransactionScope) ScenarioContext.Current["CurrentScope"]; }
            set { ScenarioContext.Current["CurrentScope"] = value; }
        }
    }
}