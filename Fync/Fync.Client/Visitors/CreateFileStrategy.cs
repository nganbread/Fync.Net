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
    internal class CreateFileStrategy : StrategyBase<IFolderNode>
    {
        private readonly FileInfo _target;
        private readonly IHasher _hasher;
        private readonly IHttpClient _httpClient;

        public CreateFileStrategy(FileInfo target, IHasher hasher, IHttpClient httpClient)
        {
            _target = target;
            _hasher = hasher;
            _httpClient = httpClient;
        }

        public override async Task Perform(IFolderNode node)
        {
            await base.Perform(node);
            
            //Do stuff
            var hash = _hasher.Hash(_target);
            var fileAlreadyExistsOnServer = await _httpClient.GetAsync<bool>("File?hash={0}", hash);
            SymbolicFile file;
            if (fileAlreadyExistsOnServer)
            {
                var newSymbolicFile = new NewSymbolicFile
                {
                    Hash = hash,
                    Name = _target.Name
                };
                file = await _httpClient.PutAsync<SymbolicFile>("{0}/SymbolicFile".FormatWith(node.Folder.Id), newSymbolicFile);
            }
            else
            {
                file = await _httpClient.PostStreamAsync<SymbolicFile>(_target.OpenRead(), new NewFile { Name = _target.Name }, "{0}/SymbolicFile", node.Folder.Id);
            }

            var existing = node.Files.SingleOrDefault(x => x.FileInfo.IsSameAs(_target));
            if (existing != null)
            {
                existing.File = file;
            }
            else
            {
                node.Files.Add(new FileNode(file, node, _target));
            }
        }
    }
}