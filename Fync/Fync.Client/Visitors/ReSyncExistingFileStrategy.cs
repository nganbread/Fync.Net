using System.Threading.Tasks;
using Fync.Client.Node.Base;
using Fync.Client.Web;
using Fync.Common;
using Fync.Common.Common;
using Fync.Common.Models;

namespace Fync.Client.Visitors
{
    internal class ReSyncExistingFileStrategy : StrategyBase<IFileNode>
    {
        private readonly IHttpClient _httpClient;
        private readonly IHasher _hasher;

        public ReSyncExistingFileStrategy(
            IHttpClient httpClient,
            IHasher hasher)
        {
            _httpClient = httpClient;
            _hasher = hasher;
        }

        public override async Task Perform(IFileNode node)
        {
            await base.Perform(node);

            var hash = _hasher.Hash(node.FileInfo);
            if (node.File.Hash == hash)
            {
                Logger.Instance.Log("\tFile hasnt changed");
                return;
            }
            var fileAlreadyExistsOnServer = await _httpClient.GetAsync<bool>("File?hash={0}", hash);

            if (fileAlreadyExistsOnServer)
            {
                Logger.Instance.Log("\tFile exists on server - PUT");
                var newSymbolicFile = new NewSymbolicFile
                {
                    Hash = hash,
                    Name = node.FileInfo.Name,
                };
                await _httpClient.PutAsync("{0}/SymbolicFile".FormatWith(node.File.FolderId), newSymbolicFile);
            }
            else
            {
                Logger.Instance.Log("\t\tFile doesnt exists on server - POST");
                await _httpClient.PostStreamAsync(node.FileInfo.OpenRead(), new NewFile { Name = node.FileInfo.Name }, "{0}/SymbolicFile", node.Parent.Folder.Id);
            }
        }
    }
}