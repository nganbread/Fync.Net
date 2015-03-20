using System.IO;
using System.Threading.Tasks;
using Fync.Client.Monitor;
using Fync.Client.Node;
using Fync.Client.Traverser;
using Fync.Client.Visitors;
using Fync.Client.Web;
using Fync.Common;
using Fync.Common.Models;

namespace Fync.Client
{
    internal class SyncEngine : ISyncEngine, IMonitor<FileInfo>, IMonitor<DirectoryInfo>
    {
        private RootNode _rootNode;
        private readonly IHttpClient _httpClient;
        private readonly IClientConfiguration _clientConfiguration;
        private readonly CreateExistingSubFoldersStrategy _createExistingSubFoldersStrategy;
        private readonly SyncNewLocalFoldersToServer _syncNewLocalFoldersToServerStrategy;
        private readonly SyncExistingFolderStrategy _syncFolderStrategy;
        private readonly CreateExistingFileStrategy _createExistingFilesStrategy;
        private readonly SyncExistingFileStrategy _syncExistingFilesStrategy;
        private readonly SyncNewLocalFilesToServerStrategy _syncNewLocalFilesToFilesStrategy;

        private readonly IFactory<FileTraverser> _fileTraverserFactory;
        private readonly IFactory<FolderTraverser> _folderTraverserFactory;
        
        private readonly IFileChangeDetector _fileChangeDetector;
        private readonly IFolderChangeDetector _folderChangeDetector;

        private readonly DeleteExistingFolderStrategy _deleteExistingFolderStrategy;
        private readonly DeleteExistingFileStrategy _deleteExistingFileStrategy;
        private readonly ReSyncExistingFileStrategy _reSyncExistingFileStrategy;
        private readonly IFactory<CreateFolderStrategy> _createNewFolderStrategyFactory;
        private readonly IFactory<FolderLocatorTraverser> _folderLocatorTraverserFactory;
        private readonly IFactory<FileLocatorTraverser> _fileLocatorTraverserfactory;

        public SyncEngine(IHttpClient httpClient, IClientConfiguration clientConfiguration, CreateExistingSubFoldersStrategy createExistingSubFoldersStrategy, SyncNewLocalFoldersToServer syncNewLocalFoldersToServerStrategy, SyncExistingFolderStrategy syncFolderStrategy, CreateExistingFileStrategy createExistingFilesStrategy, SyncExistingFileStrategy syncExistingFilesStrategy, SyncNewLocalFilesToServerStrategy syncNewLocalFilesToFilesStrategy, IFactory<FileTraverser> fileTraverserFactory, IFactory<FolderTraverser> folderTraverserFactory, IFileChangeDetector fileChangeDetector, IFolderChangeDetector folderChangeDetector, DeleteExistingFolderStrategy deleteExistingFolderStrategy, DeleteExistingFileStrategy deleteExistingFileStrategy, ReSyncExistingFileStrategy reSyncExistingFileStrategy, IFactory<CreateFolderStrategy> createNewFolderStrategyFactory, IFactory<FolderLocatorTraverser> folderLocatorTraverserFactory, IFactory<FileLocatorTraverser> fileLocatorTraverserfactory)
        {
            _httpClient = httpClient;
            _clientConfiguration = clientConfiguration;
            _createExistingSubFoldersStrategy = createExistingSubFoldersStrategy;
            _syncNewLocalFoldersToServerStrategy = syncNewLocalFoldersToServerStrategy;
            _syncFolderStrategy = syncFolderStrategy;
            _createExistingFilesStrategy = createExistingFilesStrategy;
            _syncExistingFilesStrategy = syncExistingFilesStrategy;
            _syncNewLocalFilesToFilesStrategy = syncNewLocalFilesToFilesStrategy;
            _fileTraverserFactory = fileTraverserFactory;
            _folderTraverserFactory = folderTraverserFactory;
            _fileChangeDetector = fileChangeDetector;
            _folderChangeDetector = folderChangeDetector;
            _deleteExistingFolderStrategy = deleteExistingFolderStrategy;
            _deleteExistingFileStrategy = deleteExistingFileStrategy;
            _reSyncExistingFileStrategy = reSyncExistingFileStrategy;
            _createNewFolderStrategyFactory = createNewFolderStrategyFactory;
            _folderLocatorTraverserFactory = folderLocatorTraverserFactory;
            _fileLocatorTraverserfactory = fileLocatorTraverserfactory;
        }

        public async Task Start()
        {
            var root = await _httpClient.GetAsync<FolderWithChildren>("Folder");

            _rootNode = new RootNode(_clientConfiguration.FyncDirectory, root);

            {
                var traverser = _folderTraverserFactory.Manufacture(new {strategy = _createExistingSubFoldersStrategy});
                await traverser.Traverse(_rootNode);
            }

            {
                var traverser = _folderTraverserFactory.Manufacture(new { strategy = _syncFolderStrategy });
                await traverser.Traverse(_rootNode);
            }
            
            {
                var traverser = _folderTraverserFactory.Manufacture(new {strategy = _syncNewLocalFoldersToServerStrategy});
                await traverser.Traverse(_rootNode);
            }

            {
                var traverser = _folderTraverserFactory.Manufacture(new {strategy = _createExistingFilesStrategy});
                await traverser.Traverse(_rootNode);
            }

            {
                var traverser = _fileTraverserFactory.Manufacture(new {strategy = _syncExistingFilesStrategy});
                await traverser.Traverse(_rootNode);
            }

            {
                var traverser = _folderTraverserFactory.Manufacture(new { strategy = _syncNewLocalFilesToFilesStrategy });
                await traverser.Traverse(_rootNode);
            }

            _folderChangeDetector.Monitor(this);
            _fileChangeDetector.Monitor(this);
        }

        public async Task Create(FileInfo target)
        {
            //TODO: optimize just to create single file
            var traverser = _folderLocatorTraverserFactory.Manufacture(new
            {
                target = target.Directory,
                strategy = _syncNewLocalFilesToFilesStrategy
            });
            await traverser.Traverse(_rootNode);
        }

        public async Task Rename(FileInfo old, FileInfo @new)
        {
            await Delete(old);
            var traverser = _folderLocatorTraverserFactory.Manufacture(new
            {
                target = @new.Directory,
                strategy = _syncNewLocalFilesToFilesStrategy
            });
            await traverser.Traverse(_rootNode);
        }

        public async Task Update(FileInfo target)
        {
            {
                var traverser = _folderLocatorTraverserFactory.Manufacture(new
                {
                    target = target.Directory,
                    strategy = _syncNewLocalFilesToFilesStrategy
                });
                await traverser.Traverse(_rootNode);
            }
            {
                var traverser = _fileLocatorTraverserfactory.Manufacture(new
                {
                    target = target,
                    strategy = _reSyncExistingFileStrategy
                });

                await traverser.Traverse(_rootNode);
            }
        }

        public async Task Delete(FileInfo target)
        {
            var traverser = _fileLocatorTraverserfactory.Manufacture(new
            {
                target = target,
                strategy = _deleteExistingFileStrategy
            });

            await traverser.Traverse(_rootNode);
        }

        public async Task Create(DirectoryInfo target)
        {
            var strategy = _createNewFolderStrategyFactory.Manufacture(new
            {
                target = target
            });

            var traverser = _folderLocatorTraverserFactory.Manufacture(new
            {
                target = target.Parent,
                strategy = strategy
            });

            await traverser.Traverse(_rootNode);
        }

        public async Task Rename(DirectoryInfo old, DirectoryInfo @new)
        {
            await Delete(old);
            await Create(@new);
        }

        public async Task Update(DirectoryInfo target)
        {
            //folders cant really be updated
        }

        public async Task Delete(DirectoryInfo target)
        {
            var traverser = _folderLocatorTraverserFactory.Manufacture(new
            {
                target = target, 
                strategy = _deleteExistingFolderStrategy
            });
            await traverser.Traverse(_rootNode);
        }
    }
}
