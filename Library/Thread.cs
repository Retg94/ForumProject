using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public class Thread
    {
        public int ThreadId { get; }
        public string ThreadName { get; set; }
        public string ThreadText { get; set; }
        public int UserId { get; }
        public List<Post> PostsInThread { get; set; }

    }
}
