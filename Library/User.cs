using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public class User
    {
        public int user_id { get;}
        public string username { get; set; }
        public string password { get; set; }
        public List<Thread> ThreadsByUser { get; set; }
        public List <Post> PostsByUser { get; set; }

    }
}
