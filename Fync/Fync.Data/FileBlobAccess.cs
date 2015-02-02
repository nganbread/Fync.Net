using System;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Fync.Data
{
    //http://justazure.com/azure-blob-storage-part-4-uploading-large-blobs/
    internal class FileBlobAccess : IFileBlobAccess
    {
        private readonly CloudBlobContainer _blobContainer;

        public FileBlobAccess(Func<string, CloudBlobContainer> blobContainerFactory)
        {
            _blobContainer = blobContainerFactory(CloudBlobContainerName.File);
        }

        public ICloudBlob UploadFile(Stream stream)
        {
            stream.Position = 0;
            var blobName = Guid.NewGuid().ToString();
            var blob = _blobContainer.GetBlockBlobReference(blobName);
            blob.UploadFromStream(stream);
            return blob;
        }

        public ICloudBlob GetBlob(string name)
        {
            return _blobContainer.GetBlobReferenceFromServer(name);
        }
    }
}
