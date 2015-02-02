using System;
using System.IO;

namespace Fync.Client
{
    internal interface IClientConfiguration
    {
        Uri BaseUri { get; }
        DirectoryInfo BaseDirectory { get; }
        string EmailAddress { get; }
        string Password { get; }
        string DatabaseFileName { get; }
    }
}