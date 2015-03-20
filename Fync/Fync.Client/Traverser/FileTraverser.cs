using System.Threading.Tasks;
using Fync.Client.Node.Base;
using Fync.Client.Visitors;

namespace Fync.Client.Traverser
{
    internal class FileTraverser : ITraverser
    {
        private readonly IStrategy<IFileNode>[] _stategies;

        public FileTraverser(IStrategy<IFileNode> strategy)
        {
            _stategies = new[] {strategy};
        }

        public FileTraverser(IStrategy<IFileNode>[] strategies)
        {
            _stategies = strategies;
        }

        public async Task Traverse(IFolderNode folderNode)
        {
            foreach (var node in folderNode.Files)
            {
                var closuredNode = node;
                foreach (var strategy in _stategies)
                {
                    await strategy.Perform(closuredNode);
                }
            }

            foreach (var folder in folderNode.SubFolders)
            {
                await Traverse(folder);
            }
        }
    }
}