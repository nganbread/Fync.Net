using System;
using System.IO;

namespace Fync.Client.Extensions
{
    public static class DirectoryInfoExtensions
    {
        public static DirectoryInfo CreateSubdirectoryInfo(this DirectoryInfo directoryInfo, string subDirectoryName)
        {
            return new DirectoryInfo(directoryInfo.FullName.Slash(subDirectoryName));
        }

        public static FileInfo CreateFileInfo(this DirectoryInfo directoryInfo, string fileName)
        {
            return new FileInfo(directoryInfo.FullName.Slash(fileName));
        }

        public static DirectoryInfo ToDirectoryInfo(this string folderPath)
        {
            return new DirectoryInfo(folderPath);
        }

        public static string GetRelativePath(this DirectoryInfo directoryInfo, DirectoryInfo @base)
        {
            var directory = new Uri(directoryInfo.FullName);
            var baseDirectory = new Uri(@base.FullName);
            var relative = baseDirectory.MakeRelativeUri(directory);

            return Uri.UnescapeDataString(relative.ToString());
        }
    }
}
