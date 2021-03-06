﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Fync.Client.Hash;
using Fync.Common;

namespace Fync.Client.Windows.Services
{
    internal class LocalDatabaseHashCache : IHashCache
    {
        private readonly SQLiteConnection _connection;
        private const string SelectHashSql = "SELECT FilePath FROM Hashes WHERE Hash = '{0}';";
        private const string InsertHashSql = "DELETE FROM Hashes WHERE FilePath = '{1}';" +
                                             "INSERT INTO Hashes VALUES ('{0}', '{1}');";
        private const string RemoveHashSql = "DELETE FROM Hashes WHERE Hash = '{0}' AND FilePath = '{1}');'";
        private const string CreateHashTableSql = "CREATE TABLE IF NOT EXISTS Hashes (Hash varchar(64), FilePath TEXT);";
        private const string CreateHashIndexSql = "CREATE INDEX IF NOT EXISTS index_hash ON Hashes(Hash);";


        public LocalDatabaseHashCache(IClientConfiguration clientConfiguration)
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

        public void Dispose()
        {
            _connection.Close();
        }
    }
}