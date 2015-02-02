using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Fync.Client.HelperServices
{
    internal class FileHelper : IFileHelper
    {
        private readonly IClientConfiguration _clientConfiguration;
        private readonly IDirectoryHelper _directoryHelper;

        public FileHelper(IClientConfiguration clientConfiguration, IDirectoryHelper directoryHelper)
        {
            _clientConfiguration = clientConfiguration;
            _directoryHelper = directoryHelper;
        }

        public IList<string> GetFiles(string directory)
        {
            return Directory.GetFiles(directory);
        }

        public bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }

        public string GetFileName(string filePath)
        {
            return new FileInfo(filePath).Name;
        }

        public DateTime LastModified(string filePath)
        {
            return new FileInfo(filePath).LastWriteTimeUtc;
        }

        public Task SaveToDiskAsync(Stream fileStream, FileInfo destination)
        {
            return Task.Run(() =>
            {
                using (var writeStream = destination.OpenWrite())
                {
                    writeStream.Seek(0, SeekOrigin.Begin);
                    fileStream.CopyTo(writeStream);
                }
            });
        }

        public Stream GetStream(string filePath)
        {
            return File.Create(filePath);
        }

    }
}