namespace Fync.Client.HelperServices
{
    internal interface IDirectoryHelper
    {
        string[] GetDirectories(string currentDirectory);
        string FolderName(string currentDirectory);
        bool Exists(string folderPath);
    }
}