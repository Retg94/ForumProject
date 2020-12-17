using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public static class PostRepository
    {
        private const string _connectionString = "Data Source=.\\forum2.0.db";

        public static List<Post> GetPosts(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                var posts = connection.Query<Post>($"SELECT post_id, post_text, username AS createdBy FROM post JOIN user ON post.user_id = user.user_id WHERE thread_id={id};");
                return posts.ToList();
            }

        }
    }
}
