using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public class User
    {
        public int UserId { get;}
        public string Username { get; set; }
        public string Password { get; set; }
        public List<Thread> ThreadsByUser { get; set; }
        public List <Post> PostsByUser { get; set; }

    }
}
