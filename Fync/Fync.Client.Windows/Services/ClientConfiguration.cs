using System;
using System.Configuration;
using System.IO;
using Fync.Client.Extensions;

namespace Fync.Client.Windows.Services
{
    internal class ClientConfiguration : IClientConfiguration
    {
        public Uri BaseUri
        {
            get { return new Uri(ConfigurationManager.AppSettings["BaseUri"]); }
        }

        public DirectoryInfo FyncDirectory
        {
            get
            {
                var folderName = ConfigurationManager.AppSettings["DirectoryName"];
                return BaseDirectory.CreateSubdirectoryInfo(folderName);
            }
        }

        public DirectoryInfo BaseDirectory
        {
            get { return ConfigurationManager.AppSettings["BaseDirectory"].ToDirectoryInfo(); }
        }

        public string EmailAddress
        {
            get { return @"user@email.com"; }
        }

        public string Password
        {
            get { return "Password1"; }
        }

        public string DatabaseFileName
        {
            get { return @"fync.sqlite"; }
        }
    }
}