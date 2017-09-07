using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace ASTRA.EMSG.Common.Utils
{
    public static class PackageVersioner
    {
        private const string clientPackageVersion = "2.00";
        private const string serverPackageVersion = "2.01";
        
        public static string GetClientPackageVersion()
        {
            return clientPackageVersion;
        }
        
        public static string GetServerPackageVersion()
        {
            return serverPackageVersion;
        }

        public static bool CheckClientPackageVersion(string version)
        {
            return version == clientPackageVersion;
        }

        public static double CheckServerPackageVersion(string version)
        {
            return double.Parse(version, CultureInfo.InvariantCulture.NumberFormat) - double.Parse(serverPackageVersion, CultureInfo.InvariantCulture.NumberFormat);
        }
    }
}
