using System;
using System.IO;

namespace Fync.Client
{
    internal class ClientConfiguration : IClientConfiguration
    {
        public Uri BaseUri
        {
            get { return new Uri(@"http://fync.azurewebsites.net/api/"); }
        }

        public DirectoryInfo BaseDirectory
        {
            get { return new DirectoryInfo(@"D:/Fync2/"); }
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