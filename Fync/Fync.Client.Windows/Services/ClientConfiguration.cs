using System;
using System.Configuration;
using System.IO;

namespace Fync.Client.Windows.Services
{
    internal class ClientConfiguration : IClientConfiguration
    {
        public Uri BaseUri
        {
            get { return new Uri(ConfigurationManager.AppSettings["BaseUri"]); }
        }

        public DirectoryInfo BaseDirectory
        {
            get { return new DirectoryInfo(@"D:/Railway/"); }
        }

        public string EmailAddress
        {
            get { return @"user4@email.com"; }
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