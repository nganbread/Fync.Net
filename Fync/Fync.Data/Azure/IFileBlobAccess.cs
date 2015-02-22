using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Fync.Data
{
    public interface IFileBlobAccess
    {
        ICloudBlob UploadFile(Stream stream);
        ICloudBlob GetBlob(string name);
    }
}