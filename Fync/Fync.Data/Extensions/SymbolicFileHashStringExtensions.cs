namespace Fync.Data.Extensions
{
    internal static class SymbolicFileHashStringExtensions
    {
        private const int PartitionKeyLength = 5;

        public static string FileHashPartitionKey(this string fileHash)
        {
            return fileHash.Substring(0, PartitionKeyLength);
        }

        public static string FileHashRowKey(this string fileHash)
        {
            return fileHash.Substring(PartitionKeyLength);
        }
    }
}
