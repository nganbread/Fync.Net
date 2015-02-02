using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;
using Fync.Common;

namespace Fync.Client.DataBase
{
    internal class LocalDatabase : ILocalDatabase
    {
        private readonly SQLiteConnection _connection;
        private const string SelectHashSql = "SELECT FilePath FROM Hashes WHERE Hash = '{0}';";
        private const string InsertHashSql = "DELETE FROM Hashes WHERE FilePath = '{1}';" +
                                             "INSERT INTO Hashes VALUES ('{0}', '{1}');";
        private const string RemoveHashSql = "DELETE FROM Hashes WHERE Hash = '{0}' AND FilePath = '{1}');'";
        private const string CreateHashTableSql = "CREATE TABLE IF NOT EXISTS Hashes (Hash varchar(64), FilePath TEXT);";
        private const string CreateHashIndexSql = "CREATE INDEX IF NOT EXISTS index_hash ON Hashes(Hash);";


        public LocalDatabase(IClientConfiguration clientConfiguration)
        {
            var databaseFileName = clientConfiguration.DatabaseFileName;
            SQLiteConnection.CreateFile(databaseFileName);
            
            _connection = new SQLiteConnection(@"Data Source={0}".FormatWith(databaseFileName));
            _connection.Open();

            Initialize();
        }

        private void Initialize()
        {
            new SQLiteCommand(CreateHashTableSql, _connection).ExecuteNonQuery();
            new SQLiteCommand(CreateHashIndexSql, _connection).ExecuteNonQuery();
        }

        public IList<FileInfo> FilePathsOfCachedHash(string hash)
        {
            var reader = new SQLiteCommand(SelectHashSql.FormatWith(hash), _connection).ExecuteReader();

            var filePaths = new List<FileInfo>();
            while (reader.Read())
            {
                filePaths.Add(new FileInfo(Convert.ToString(reader["FilePath"])));
            }

            return filePaths;
        }
        
        public void InsertHash(string hash, string filePath)
        {
            new SQLiteCommand(InsertHashSql.FormatWith(hash, filePath), _connection).ExecuteNonQuery();
        }

        public void RemoveHash(string hash, string filePath)
        {
            new SQLiteCommand(RemoveHashSql.FormatWith(hash, filePath), _connection).ExecuteNonQuery();
        }

        public Task InsertHashAsync(string hash, string filePath)
        {
            return Task.Run(() => InsertHash(hash, filePath));
        }

        public Task RemoveHashAsync(string hash, string filePath)
        {
            return Task.Run(() => RemoveHash(hash, filePath));
        }

        public Task<IList<FileInfo>> FilePathsOfCachedHashAsync(string hash)
        {
            return Task.Run(() => FilePathsOfCachedHash(hash));
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}