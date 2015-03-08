using Fync.Client.Hash;
using Fync.Client.Node;
using Fync.Client.Traverser;
using Fync.Client.Visitors;
using Fync.Client.Web;
using Fync.Common;
using Fync.Common.Models;

namespace Fync.Client
{
    class SyncEngine : ISyncEngine
    {
        private readonly IHttpClient _httpClient;
        private readonly IClientConfiguration _clientConfiguration;
        private RootNode _rootNode;
        private readonly IHasher _hasher;
        private readonly IHashCache _cachedHash;

        public SyncEngine(IHttpClient httpClient, IClientConfiguration clientConfiguration, IHasher hasher, IHashCache cachedHash)
        {
            _httpClient = httpClient;
            _clientConfiguration = clientConfiguration;
            _hasher = hasher;
            _cachedHash = cachedHash;
        }

        public async void Start()
        {
            var root = await _httpClient.GetAsync<FolderWithChildren>("Folder");

            _rootNode = new RootNode(_clientConfiguration.FyncDirectory, root);

            //create existing folders
            {
                var traverser = new FolderTraverser<FolderNode>(_rootNode, new CreateExistingSubFoldersVisitor());
                await traverser.Traverse();
            }

            //create new folders
            {
                var traverser = new FolderTraverser<FolderNode>(_rootNode, new CreateNewSubFoldersVisitor());
                await traverser.Traverse();
            }

            //Sync existing folders
            {
                var traverser = new FolderTraverser<FolderNode>(_rootNode, new SyncFolderVisitor(_httpClient));
                await traverser.Traverse();
            }

            //Sync new folders
            {
                var traverser = new FolderTraverser<NewFolderNode>(_rootNode, new SyncNewFolderVisitor(_httpClient, _clientConfiguration));
                await traverser.Traverse();
            }

            //create existing files
            {
                var traverser = new FolderTraverser<FolderNode>(_rootNode, new CreateExistingFilesVisitor(_httpClient));
                await traverser.Traverse();
            }

            //create new files
            {
                var traverser = new FolderTraverser<FolderNode>(_rootNode, new CreateNewFilesVisitor());
                await traverser.Traverse();
            }

            //Sync existing files
            {
                var traverser = new FileTraverser<FileNode>(_rootNode, new SyncExistingFilesVisitor(_hasher, _httpClient, _clientConfiguration, _cachedHash));
                await traverser.Traverse();
            }

            //Sync new files
            {
                var traverser = new FileTraverser<NewFileNode>(_rootNode, new SyncNewFilesVisitor(_hasher, _httpClient));
                await traverser.Traverse();
            }
        }
    }
}
