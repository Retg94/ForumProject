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
        public static User GetUsernameById(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                var user = connection.QueryFirst<User>($"SELECT * FROM user WHERE user_id={id}");
                return user;
            }
        }


    }
}
