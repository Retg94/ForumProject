using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public static class ThreadRepository
    {
        private const string _connectionString = "Data Source=.\\forum2.0.db";

        public static List<Thread> GetThreads()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                var threads = connection.Query<Thread>("SELECT * FROM thread");
                return threads.ToList();
            }
        }
        public static Thread GetThreadById(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                var thread = connection.QueryFirst<Thread>($"SELECT * FROM thread WHERE thread_id={id}");
                return thread;
            }
        }
    }
}
