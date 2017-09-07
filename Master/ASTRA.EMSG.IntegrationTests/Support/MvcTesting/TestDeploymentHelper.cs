using System;
using System.IO;
using System.Reflection;

namespace ASTRA.EMSG.IntegrationTests.Support.MvcTesting
{
    public static class TestDeploymentHelper
    {
        public static string GetTestOutputDir()
        {
            return Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
        }
    }
}
