using System.Configuration;

namespace Fync.Common
{
    internal class Configuration : IConfiguration
    {
        public string CloudStorageConnectionString
        {
            get { return GetStringFromConfig("CloudStorageConnectionString"); }
        }

        private string GetStringFromConfig(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
