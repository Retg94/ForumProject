using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;

namespace Library
{
    public static class UserRepository
    {
        private const string _connectionString = "Data Source=.\\forum2.0.db";

        public static List<User> GetUsers()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                var users = connection.Query<User>("SELECT * FROM user");
                return users.ToList();
            }
        }
        public static void CreateNewUser(User user)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                var sql = "INSERT INTO user (username, password)" + " VALUES(@username, @password)";
                connection.Execute(sql, user);
            }
        }

    }
}
