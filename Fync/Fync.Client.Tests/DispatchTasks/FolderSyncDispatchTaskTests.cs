using System.Collections.Generic;
using System.IO;
using Fync.Client.Dispatcher;
using Fync.Client.DispatchTasks;
using Fync.Client.Web;
using Fync.Common.Models;
using Moq;
using NUnit.Framework;

namespace Fync.Client.Tests.DispatchTasks
{
    [TestFixture]
    public class FolderSyncDispatchTaskTests
    {
        private Mock<IHttpClient> _httpClient;
        private Mock<IDispatchFactory> _dispatchFactory;
        private Mock<IDispatcher> _dispatcher;

        [TestFixtureSetUp]
        public void Setup()
        {
            _dispatchFactory = new Mock<IDispatchFactory>();
            _dispatcher = new Mock<IDispatcher>();
            _httpClient = new Mock<IHttpClient>();
        }

        private FolderSyncDispatchTask CreateTask(DirectoryInfo localFolder, Folder parentFolder, Folder serverFolder)
        {
            return new FolderSyncDispatchTask(
                localFolder,
                parentFolder,
                serverFolder,
                _dispatchFactory.Object,
                _dispatcher.Object,
                _httpClient.Object);
        }

        [Test]
        public void GivenThereAreTwoTasks_WhenTheyHaveTheSameFolderReferences_ThenOnlyOneOfTheTasksExistsInTheSet()
        {
            var set = new HashSet<FolderSyncDispatchTask>();
            var serverFolder = new Folder();
            var parentFolder = new Folder();
            var localFolder = new DirectoryInfo("C:/");
            set.Add(CreateTask(localFolder, parentFolder, serverFolder));
            set.Add(CreateTask(localFolder, parentFolder, serverFolder));

            Assert.AreEqual(1, set.Count);
        }

        [Test]
        public void GivenThereAreTwoTasks_WhenTheyHaveTheSameFolderReferencesAndDirectoryPaths_ThenOnlyOneOfTheTasksExistsInTheSet()
        {
            var set = new HashSet<FolderSyncDispatchTask>();
            var serverFolder = new Folder();
            var parentFolder = new Folder();
            var localFolder1 = new DirectoryInfo("C:/");
            var localFolder2 = new DirectoryInfo("C:/");
            set.Add(CreateTask(localFolder1, parentFolder, serverFolder));
            set.Add(CreateTask(localFolder2, parentFolder, serverFolder));

            Assert.AreEqual(1, set.Count);
        }

        [Test]
        public void GivenThereAreTwoTasks_WhenTheyHaveTheSameFolderReferencesAndDifferentDirectoryPaths_ThenBothOfTheTasksExistsInTheSet()
        {
            var set = new HashSet<FolderSyncDispatchTask>();
            var serverFolder = new Folder();
            var parentFolder = new Folder();
            var localFolder1 = new DirectoryInfo("C:/1/");
            var localFolder2 = new DirectoryInfo("C:/2/");
            set.Add(CreateTask(localFolder1, parentFolder, serverFolder));
            set.Add(CreateTask(localFolder2, parentFolder, serverFolder));

            Assert.AreEqual(2, set.Count);
        }
    }
}
