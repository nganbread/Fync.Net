using Fync.Data;

namespace Fync.Service
{
    internal class FileService : IFileService
    {
        private readonly IFileTableAccess _fileTableAccess;

        public FileService(IFileTableAccess fileTableAccess)
        {
            _fileTableAccess = fileTableAccess;
        }

        public bool FileExists(string hash)
        {
            return _fileTableAccess.FileExists(hash);
        }
    }
}