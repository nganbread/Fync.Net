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

    public static class FileInfoExtensions
    {
        public static bool CanRead(this FileInfo fileInfo)
        {
            FileStream stream = null;

            try
            {
                stream = fileInfo.OpenRead();
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return false;
        }

        public static bool CanWrite(this FileInfo fileInfo)
        {
            FileStream stream = null;

            try
            {
                stream = fileInfo.OpenWrite();
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return false;
        }
    }
}
