using System;
using System.Data.Entity;
using Fync.Common;
using Fync.Common.Libraries;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using TinyIoC;

namespace Fync.Data
{
    public static class Registrations
    {
        public static void Register(TinyIoCContainer container)
        {
            container.Register<Context>().AsPerRequestSingleton();
            container.Register<IContext>((c, overloads) => c.Resolve<Context>());
            container.Register<DbContext>((c, overloads) => c.Resolve<Context>());

            container.Register<IFileTableAccess, FileTableAccess>();
            container.Register<ISymbolicFileTableAccess, SymbolicFileTableAccess>();
            container.Register<IDeletedSymbolicFileTableAccess, DeletedSymbolicFileTableAccess>();
            container.Register<IFileBlobAccess, FileBlobAccess>();

            RegisterAzureStorage(container);
        }

        private static void RegisterAzureStorage(TinyIoCContainer container)
        {
            //TODO: cache all of these as singletons
            container.Register<CloudStorageAccount>(CreateCloudStorageAccount);
            container.Register<CloudBlobContainer>((c, parameters) => CreateCloudBlobContainer(c, CloudBlobContainerName.File), CloudBlobContainerName.File);
            //container.Register<CloudTable>((c, parameters) => CreateCloudTableClient(c, CloudTableName.SymbolicFile), CloudTableName.SymbolicFile);
            //container.Register<CloudTable>((c, parameters) => CreateCloudTableClient(c, CloudTableName.DeletedSymbolicFile), CloudTableName.DeletedSymbolicFile);
            //container.Register<CloudTable>((c, parameters) => CreateCloudTableClient(c, CloudTableName.File), CloudTableName.File);

            container.Register<Func<string, CloudTable>>((c, parameters) => (tableName => CreateOrGetTable(tableName, container)));
            //container.Register<Func<string, string, CloudTable>>((c, parameters) => ((prefix, tableName) => CreateOrGetTable("{0}_{1}".FormatWith(prefix, tableName), container)));
        }

        private static CloudTable CreateOrGetTable(string tableName, TinyIoCContainer container)
        {
            if (container.CanResolve<CloudTable>(tableName))
            {
                return container.Resolve<CloudTable>(tableName);
            }
            
            var storageAccount = container.Resolve<CloudStorageAccount>();
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(tableName);
            table.CreateIfNotExists();

            container.Register<CloudTable>(table, tableName);
            return table;
        }

        //private static CloudTable CreateCloudTableClient(TinyIoCContainer container, string tableName)
        //{
        //    var storageAccount = container.Resolve<CloudStorageAccount>();
        //    var tableClient = storageAccount.CreateCloudTableClient();
        //    var table = tableClient.GetTableReference(tableName);
        //    table.CreateIfNotExists();

        //    return table;
        //}

        private static CloudStorageAccount CreateCloudStorageAccount(TinyIoCContainer container, NamedParameterOverloads parameters)
        {
            var cloudStorageConnectionString = container.Resolve<IConfiguration>().CloudStorageConnectionString;
            var cloudStorageAccount = CloudStorageAccount.Parse(cloudStorageConnectionString);
            return cloudStorageAccount;
        }

        private static CloudBlobContainer CreateCloudBlobContainer(TinyIoCContainer container, string containerName)
        {
            var storageAccount = container.Resolve<CloudStorageAccount>();
            var blobClient = storageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = blobClient.GetContainerReference(containerName.ToLower());
            cloudBlobContainer.CreateIfNotExists();      
      
            return cloudBlobContainer;
        }
    }
}