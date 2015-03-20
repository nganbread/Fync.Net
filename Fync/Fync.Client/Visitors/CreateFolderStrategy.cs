using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fync.Client.Extensions;
using Fync.Client.Node.Base;
using Fync.Client.Web;
using Fync.Common;
using Fync.Common.Models;

namespace Fync.Client.Visitors
{
    internal class CreateFolderStrategy : StrategyBase<IFolderNode>
    {
        private readonly DirectoryInfo _target;
        private readonly IHttpClient _httpClient;

        public CreateFolderStrategy(DirectoryInfo target, IHttpClient httpClient)
        {
            _target = target;
            _httpClient = httpClient;
        }

        public override async Task Perform(IFolderNode parent)
        {
            await base.Perform(parent);

            //create folder
            var newFolder = new NewFolder
            {
                Name = _target.Name,
            };
            var folder = await _httpClient.PostAsync<FolderWithChildren>("{0}/Folder".FormatWith(parent.Folder.Id), newFolder);

            var existing = parent.SubFolders.SingleOrDefault(x => x.DirectoryInfo.IsSameAs(_target));
            if (existing != null)
            {
                existing.Folder = folder;
            }
            else
            {
                parent.SubFolders.Add(new FolderNode(folder, parent, _target));
            }
        }
    }
}