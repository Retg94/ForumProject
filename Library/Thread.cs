using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public class Thread
    {
        public int thread_id { get; }
        public string thread_name { get; set; }
        public string thread_text { get; set; }
        public int user_id { get; }
        public List<Post> PostsInThread { get; set; }

    }
}
