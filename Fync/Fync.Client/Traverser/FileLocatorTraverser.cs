using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fync.Client.Extensions;
using Fync.Client.Node.Base;
using Fync.Client.Visitors;
using Fync.Common;

namespace Fync.Client.Traverser
{
    internal class FileLocatorTraverser : ITraverser
    {
        private readonly FileInfo _target;
        private readonly IStrategy<IFileNode> _strategy;

        public FileLocatorTraverser(FileInfo target, IStrategy<IFileNode> strategy)
        {
            _target = target;
            _strategy = strategy;
        }

        public async Task Traverse(IFolderNode root)
        {
            if (root.DirectoryInfo.IsSameAs(_target.Directory))
            {
                foreach (var file in root.Files.Where(x => x.FileInfo.IsSameAs(_target)))
                {
                    await _strategy.Perform(file);
                }
            }
            else
            {
                root
                    .SubFolders
                    .Where(x => _target.Directory.StartsWith(x.DirectoryInfo))
                    .ForEach(x => Traverse(x));
            }
        }
    }
}