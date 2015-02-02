using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Fync.Client.HelperServices
{
    internal interface IFileHelper
    {
        IList<string> GetFiles(string directory = null);
        string GetFileName(string filePath);
        bool Exists(string filepath);
        DateTime LastModified(string filePath);
        Task SaveToDiskAsync(Stream fileStream, FileInfo destination);
        Stream GetStream(string filePath);
    }
}