namespace Fync.Common
{
    public interface IConfiguration
    {
        string CloudStorageConnectionString { get; }
        string RootFolderName { get; }
    }
}