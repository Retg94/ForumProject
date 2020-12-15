using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public class Post
    {
        public int PostId { get; }
        public string PostText { get; set; }
        public int UserId { get; }
        public int ThreadId { get; }
    }
}
