using System;
using System.IO;

namespace Fync.Client
{
    public interface IClientConfiguration
    {
        Uri BaseUri { get; }
        DirectoryInfo BaseDirectory { get; }
        DirectoryInfo FyncDirectory { get; }
        string EmailAddress { get; }
        string Password { get; }
        string DatabaseFileName { get; }
    }
}