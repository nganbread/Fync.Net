using System.Collections.Generic;
using System.Threading.Tasks;
using Fync.Client.Node.Base;
using Fync.Client.Visitors;

namespace Fync.Client.Traverser
{
    internal class FolderTraverser : ITraverser 
    {
        private readonly IList<IStrategy<IFolderNode>> _strategies;

        public FolderTraverser(IStrategy<IFolderNode> strategy)
        {
            _strategies = new[] {strategy};
        }

        public FolderTraverser(params IStrategy<IFolderNode>[] strategies)
        {
            _strategies = strategies;
        }

        public async Task Traverse(IFolderNode node)
        {
            foreach (var strategy in _strategies)
            {
                await strategy.Perform(node);
            }

            foreach (var childNode in node.SubFolders)
            {
                await Traverse(childNode);
            }
        }
    }
}