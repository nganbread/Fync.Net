using System;
using System.Linq;
using System.Web.Http;
using Fync.Common;
using Fync.Data;
using Fync.Data.Models;

namespace Fync.Api.Controllers
{
    public class FolderController : ApiController
    {
        private readonly Func<IContext> _context;

        public FolderController(Func<IContext> context)
        {
            _context = context;
        }

        public Folder Get()
        {
            //using (var context = _context())
            //{
            //    var folderAbc = new Folder { Name = "Abc", LastModified = DateTime.UtcNow };
            //    var folderAb = new Folder { Name = "Ab", LastModified = DateTime.UtcNow };
            //    var folderAa = new Folder { Name = "Aa", LastModified = DateTime.UtcNow };
            //    var folderA = new Folder { Name = "A", LastModified = DateTime.UtcNow };

            //    folderAb.SubFolders.Add(folderAbc);
            //    folderA.SubFolders.Add(folderAa, folderAb);

            //    context.Folders.Add(folderA);
            //    context.SaveChanges();
            //}

            using (var context = _context())
            {
                var root = context.Folders.Single(x => x.Name == "A");

                var many = root.SubFolders.SelectMany(x => x.SubFolders.SelectMany(y => y.SubFolders)).ToList();

                return context.GetTree(root);
            }
        }
    }
}
