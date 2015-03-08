using System;
using System.IO;

namespace Fync.Client.Extensions
{
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

        public static string GetRelativePath(this FileInfo directoryInfo, DirectoryInfo @base)
        {
            var directory = new Uri(directoryInfo.FullName);
            var baseDirectory = new Uri(@base.FullName);
            var relative = baseDirectory.MakeRelativeUri(directory);

            return Uri.UnescapeDataString(relative.ToString());
        }

        public static void SaveToDisk(this FileInfo destination, Stream fileStream)
        {
            using (var writeStream = destination.OpenWrite())
            {
                writeStream.Seek(0, SeekOrigin.Begin);
                fileStream.CopyTo(writeStream);
            }
        }
    }
}