using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public class Post
    {
        public int post_id { get; }
        public string post_text { get; set; }
        public int user_id { get; set; }
        public int thread_id { get; set; }
        public string createdBy { get; }
    }
}
