using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fync.Client.Extensions;
using Fync.Client.Node.Base;
using Fync.Client.Traverser;
using Fync.Client.Visitors;
using Fync.Common;

namespace Fync.Client
{
    internal class FolderLocatorTraverser : ITraverser
    {
        private readonly DirectoryInfo _target;
        private readonly IStrategy<IFolderNode>[] _strategies;

        public FolderLocatorTraverser(DirectoryInfo target, IStrategy<IFolderNode> strategy)
        {
            _target = target;
            _strategies = new[] {strategy};
        }

        public FolderLocatorTraverser(DirectoryInfo target, params IStrategy<IFolderNode>[] strategies)
        {
            _target = target;
            _strategies = strategies;
        }

        public async Task Traverse(IFolderNode root)
        {
            if (root.DirectoryInfo.IsSameAs(_target))
            {
                foreach (var strategy in _strategies)
                {
                    await strategy.Perform(root);
                }
                return;
            }

            root
                .SubFolders
                .Where(x => _target.StartsWith(x.DirectoryInfo))
                .ForEach(x => Traverse(x));
        }
    }
}