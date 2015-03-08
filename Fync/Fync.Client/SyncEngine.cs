using System.Threading.Tasks;
using Fync.Client.Node;
using Fync.Client.Traverser;
using Fync.Client.Visitors;
using Fync.Client.Web;
using Fync.Common.Models;

namespace Fync.Client
{
    class SyncEngine : ISyncEngine
    {
        private readonly IHttpClient _httpClient;
        private readonly IClientConfiguration _clientConfiguration;
        private RootNode _rootNode;
        private readonly FolderTraverser<FolderNode, CreateExistingSubFoldersVisitor> _createExistingSubFoldersVisitor;
        private readonly FolderTraverser<FolderNode, CreateNewSubFoldersVisitor> _createNewSubFoldersVisitor;
        private readonly FolderTraverser<FolderNode, SyncFolderVisitor> _syncFolderVisitor;
        private readonly FolderTraverser<NewFolderNode, SyncNewFolderVisitor> _syncNewFolderVisitor;
        private readonly FolderTraverser<FolderNode, CreateExistingFilesVisitor> _createExistingFilesVisitor;
        private readonly FolderTraverser<FolderNode, CreateNewFilesVisitor> _createNewFilesVisitor;
        private readonly FileTraverser<FileNode, SyncExistingFilesVisitor> _syncExistingFilesVisitor;
        private readonly FileTraverser<NewFileNode, SyncNewFilesVisitor> _syncNewFilesVisitor;

        public SyncEngine(
            IHttpClient httpClient, 
            IClientConfiguration clientConfiguration, 
            FolderTraverser<FolderNode, CreateExistingSubFoldersVisitor> createExistingSubFoldersVisitor,
            FolderTraverser<FolderNode, CreateNewSubFoldersVisitor> createNewSubFoldersVisitor,
            FolderTraverser<FolderNode, SyncFolderVisitor> syncFolderVisitor,
            FolderTraverser<NewFolderNode, SyncNewFolderVisitor> syncNewFolderVisitor,
            FolderTraverser<FolderNode, CreateExistingFilesVisitor> createExistingFilesVisitor,
            FolderTraverser<FolderNode, CreateNewFilesVisitor> createNewFilesVisitor,
            FileTraverser<FileNode, SyncExistingFilesVisitor> syncExistingFilesVisitor,
            FileTraverser<NewFileNode, SyncNewFilesVisitor> syncNewFilesVisitor)
        {
            _httpClient = httpClient;
            _clientConfiguration = clientConfiguration;
            _createExistingSubFoldersVisitor = createExistingSubFoldersVisitor;
            _createNewSubFoldersVisitor = createNewSubFoldersVisitor;
            _syncFolderVisitor = syncFolderVisitor;
            _syncNewFolderVisitor = syncNewFolderVisitor;
            _createExistingFilesVisitor = createExistingFilesVisitor;
            _createNewFilesVisitor = createNewFilesVisitor;
            _syncExistingFilesVisitor = syncExistingFilesVisitor;
            _syncNewFilesVisitor = syncNewFilesVisitor;
        }

        public async Task Start()
        {
            var root = await _httpClient.GetAsync<FolderWithChildren>("Folder");

            _rootNode = new RootNode(_clientConfiguration.FyncDirectory, root);

            await _createExistingSubFoldersVisitor.Traverse(_rootNode);
            await _createNewSubFoldersVisitor.Traverse(_rootNode);
            await _syncFolderVisitor.Traverse(_rootNode);
            await _syncNewFolderVisitor.Traverse(_rootNode);
            await _createExistingFilesVisitor.Traverse(_rootNode);
            await _createNewFilesVisitor.Traverse(_rootNode);
            await _syncExistingFilesVisitor.Traverse(_rootNode);
            await _syncNewFilesVisitor.Traverse(_rootNode);
        }
    }
}
