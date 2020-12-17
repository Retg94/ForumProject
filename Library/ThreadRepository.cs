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
                var threads = connection.Query<Thread>("SELECT thread_id, thread_name, thread_text, username AS createdBy FROM thread JOIN user ON thread.user_id = user.user_id;");
                return threads.ToList();
            }
        }
        public static Thread GetThreadById(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                var thread = connection.QueryFirst<Thread>($"SELECT thread_id, thread_name, thread_text, username AS createdBy FROM thread JOIN user ON thread.user_id = user.user_id WHERE thread_id={id}");
                return thread;   
            }
        }
        public static void CreateNewThread(Thread thread)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                var sql = "INSERT INTO thread (thread_name, thread_text, user_id)" + " VALUES(@thread_name, @thread_text, @user_id)";
                connection.Execute(sql, thread);
            }
        }
    }
}
