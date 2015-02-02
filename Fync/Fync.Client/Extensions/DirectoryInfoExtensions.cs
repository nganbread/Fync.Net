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
    }
}
