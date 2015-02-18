using System.Collections.Generic;
using System.IO;
using Fync.Client.DataBase;
using Fync.Client.Dispatcher;
using Fync.Client.DispatchTasks;
using Fync.Client.HelperServices;
using Fync.Client.Web;
using Fync.Common;
using Fync.Common.Models;
using Moq;
using NUnit.Framework;

namespace Fync.Client.Tests.DispatchTasks
{
    [TestFixture]
    public class FileSyncDispatchTaskTests
    {
        private Mock<IHttpClient> _httpClient;
        private Mock<IFileHelper> _fileHelper;
        private Mock<IHasher> _hasher;
        private Mock<IHashCache> _localDataBase;
        private Mock<IDispatcher> _dispatcher;

        [TestFixtureSetUp]
        public void Setup()
        {
            _httpClient = new Mock<IHttpClient>();
            _fileHelper = new Mock<IFileHelper>();
            _hasher = new Mock<IHasher>();
            _localDataBase = new Mock<IHashCache>();
            _dispatcher = new Mock<IDispatcher>();
        }

        private FileSyncDispatchTask CreateTask(Folder parentFolder, FileInfo localFile, SymbolicFile serverFile)
        {
            return new FileSyncDispatchTask(
                parentFolder,
                localFile,
                serverFile,
                _fileHelper.Object,
                _httpClient.Object,
                _hasher.Object,
                _localDataBase.Object,
                _dispatcher.Object);
        }

        [Test]
        public void GivenThereAreTwoTasks_WhenTheyHaveTheSameFolderAndFileReferences_ThenOnlyOneExistsInASet()
        {
            var set = new HashSet<FileSyncDispatchTask>();
            var parentFolder = new Folder();
            var localFile = new FileInfo("C:/file.txt");
            var serverFile = new SymbolicFile();

            set.Add(CreateTask(parentFolder, localFile, serverFile));
            set.Add(CreateTask(parentFolder, localFile, serverFile));

            Assert.AreEqual(1, set.Count);
        }

        [Test]
        public void GivenThereAreTwoTasks_WhenTheyHaveTheSameFolderReferencessAndFileInfo_ThenOnlyOneExistsInASet()
        {
            var set = new HashSet<FileSyncDispatchTask>();
            var parentFolder = new Folder();
            var localFile1 = new FileInfo("C:/file.txt");
            var localFile2 = new FileInfo("C:/file.txt");
            var serverFile = new SymbolicFile();
            set.Add(CreateTask(parentFolder, localFile1, serverFile));
            set.Add(CreateTask(parentFolder, localFile2, serverFile));

            Assert.AreEqual(1, set.Count);
        }

        [Test]
        public void GivenThereAreTwoTasks_WhenTheyHaveTheSameFolderReferencessAndDifferentFileInfo_ThenOnlyTwoExistsInASet()
        {
            var set = new HashSet<FileSyncDispatchTask>();
            var parentFolder = new Folder();
            var localFile1 = new FileInfo("C:/file1.txt");
            var localFile2 = new FileInfo("C:/file2.txt");
            var serverFile = new SymbolicFile();
            set.Add(CreateTask(parentFolder, localFile1, serverFile));
            set.Add(CreateTask(parentFolder, localFile2, serverFile));

            Assert.AreEqual(2, set.Count);
        }
    }
}
