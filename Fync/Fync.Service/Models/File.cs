using System.IO;

namespace Fync.Service.Models
{
    public class File
    {
        public Stream Stream { get; set; }
        public string FileName { get; set; }
        public long LengthInBytes { get; set; }
        public string ContentType { get; set; }
    }
}
