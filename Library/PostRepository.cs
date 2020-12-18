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
                var posts = connection.Query<Post>($"SELECT *, username AS createdBy FROM post JOIN user ON post.user_id = user.user_id WHERE thread_id={id};");
                return posts.ToList();
            }
        }
        public static void CreateNewPost(Post post)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                var sql = "INSERT INTO post (post_text, user_id, thread_id)" + " VALUES(@post_text, @user_id, @thread_id)";
                connection.Execute(sql, post);
            }
        }
        public static void UpdatePost(Post post)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                var sql = $"UPDATE post SET post_text = @post_text WHERE post_id = @post_id";
                connection.Execute(sql, post);
            }
        }
        public static void DeletePost(Post post)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                var sql = $"DELETE FROM post WHERE post_id = @post_id";
                connection.Execute(sql, post);
            }
        }
    }
}
