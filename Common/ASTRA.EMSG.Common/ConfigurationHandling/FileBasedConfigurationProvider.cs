using System;
using System.Configuration;

namespace ASTRA.EMSG.Common.ConfigurationHandling
{
    public abstract class FileBasedConfigurationProvider
    {
        protected bool ReadBoolValue(string settingName, bool defaultValue)
        {
            bool value;
            if (!bool.TryParse(ConfigurationManager.AppSettings[settingName], out value))
                value = defaultValue;

            return value;
        }

        protected int ReadIntValue(string settingName, int defaultValue)
        {
            int value;
            if (!int.TryParse(ConfigurationManager.AppSettings[settingName], out value))
                value = defaultValue;

            return value;
        }

        protected long ReadLongValue(string settingName, long defaultValue)
        {
            long value;
            if (!long.TryParse(ConfigurationManager.AppSettings[settingName], out value))
                value = defaultValue;

            return value;
        }

        protected double ReadDoubleValue(string settingName, int defaultValue)
        {
            double value;
            if (!double.TryParse(ConfigurationManager.AppSettings[settingName], out value))
                value = defaultValue;

            return value;
        }

        protected short ReadShortValue(string settingName, short defaultValue)
        {
            short value;
            if (!short.TryParse(ConfigurationManager.AppSettings[settingName], out value))
                value = defaultValue;

            return value;
        }

        protected TEnum ReadEnumValue<TEnum>(string settingName, TEnum defaultValue) where TEnum : struct
        {
            TEnum value;
            if (!Enum.TryParse(ConfigurationManager.AppSettings[settingName], true, out value))
                value = defaultValue;

            return value;
        }

        protected string ReadStringValue(string settingName, string defaultValue)
        {
            var value = ConfigurationManager.AppSettings[settingName];
            if (string.IsNullOrEmpty(value))
                value = defaultValue;

            return value;
        }

        protected string ReadConnectionString(string connectionStringName)
        {
            try
            {
                return ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            }
            catch (Exception)
            {
                //if not found by the connectionStringName, than take "default" (for tests)
                return ConfigurationManager.ConnectionStrings["Development"].ConnectionString;
            }
        }
    }
}
