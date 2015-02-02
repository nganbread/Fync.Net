using System;
using System.IO;

namespace Fync.Client.HelperServices
{
    internal class DirectoryHelper : IDirectoryHelper
    {
        public string[] GetDirectories(string currentDirectory)
        {
            try
            {
                var directories = Directory.GetDirectories(currentDirectory);
                return directories;
            }
            catch (UnauthorizedAccessException)
            {
                return new string[0];
            }
        }

        public string FolderName(string currentDirectory)
        {
            return new DirectoryInfo(currentDirectory).Name;
        }

        public bool Exists(string folderPath)
        {
            return Directory.Exists(folderPath);
        }
    }
}