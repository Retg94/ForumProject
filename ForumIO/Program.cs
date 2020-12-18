using System;
using System.Collections.Generic;
using System.Linq;
using Library;

namespace ForumIO
{
    class Program
    {
        public static bool LogInOkay = false;
        public static bool Running = true;
        public static User CurrentUser = new User();
        static void Main(string[] args)
        {
            while(Running)
            {
                if (!LogInOkay)
                    LoginMenu();
                else
                    MainMenu();
            }
            Console.ReadLine();
        }
        static void LoginMenu()
        {
            Console.Clear();
            var users = UserRepository.GetUsers();
            Console.WriteLine("Write username: ");
            string tmpUsername = Console.ReadLine();
            while (!Helper.VerifyStringOnlyLettersAndNumbers(tmpUsername))
            {
                Console.WriteLine("Invalid input. You can only use letters and numbers.");
                Console.WriteLine("Write username: ");
                tmpUsername = Console.ReadLine();
            }
            Console.WriteLine("Write Password: ");
            string tmpPassword = Console.ReadLine();
            while (!Helper.VerifyStringOnlyLettersAndNumbers(tmpPassword))
            {
                Console.WriteLine("Invalid input. You can only use letters and numbers.");
                Console.WriteLine("Write password: ");
                tmpPassword = Console.ReadLine();
            }
            foreach (var user in users)
            {
                if(user.username == tmpUsername && user.password == tmpPassword)
                {
                    CurrentUser = user;
                    Console.WriteLine("Login succed. Continue to main menu...");
                    LogInOkay = true;
                }
            }
            if(!LogInOkay)
            {
                Console.WriteLine("Failed login. Try again.");
                Helper.PressAnyKeyToContinue();
            }
        }

        static void MainMenu()
        {
            Console.Clear();
            Console.WriteLine($"Main menu --- Current user: {CurrentUser.username}");
            Console.WriteLine("0. Exit program");
            Console.WriteLine("1. Logout");
            Console.WriteLine("2. Show threads");
            Console.WriteLine("3. Handle your threads");
            int input = int.Parse(Console.ReadLine());
            switch(input)
            {
                case 0:
                    Console.WriteLine("Exit program");
                    Helper.PressAnyKeyToContinue();
                    Running = false;
                    break;
                case 1:
                    Console.WriteLine("Logging out..");
                    LogInOkay = false;
                    Helper.PressAnyKeyToContinue();
                    break;
                case 2:
                    ShowAllThreads();
                    break;
                case 3:
                    ThreadChoices();
                    break;
                default:
                    Console.WriteLine("Error");
                    Helper.PressAnyKeyToContinue();
                    break;
            }
        }
        static void ShowAllThreads()
        {
            var threads = ThreadRepository.GetThreads();
            Console.WriteLine("Showing all threads in the forum");
            int index = 1;
            foreach (var thread in threads)
            {              
                Console.WriteLine($"[{index}]");
                Console.WriteLine($"Thread name: {thread.thread_name}");
                Console.WriteLine($"Created by: {thread.createdBy}");
                Console.WriteLine($"Thread text: {thread.thread_text}");
                Console.WriteLine("---------------------------------");
                index++;
            }
            Console.WriteLine("Write the number infront of the thread you want to enter:");
            bool isOkay = int.TryParse(Console.ReadLine(), out int input);
            while(!isOkay || !Helper.VerifyIntBetween(1,threads.Count, input))
            {
                Console.WriteLine("You can only enter numbers that exists infront of threads. Tty again.");
                Console.WriteLine("Write the number infront of the thread you want to enter:");
                isOkay = int.TryParse(Console.ReadLine(), out input);
            }
            index = 1;
            int tmpThreadId = 0;
            foreach(var thread in threads)
            {
                if (input == index)
                {
                    tmpThreadId = thread.thread_id;
                    break;
                }            
                index++;
            }
            ShowThreadAndPosts(tmpThreadId);
            PostsChoices(tmpThreadId);
        }
        static void ShowThreadsFromList(List<Thread> threads)
        {
            int index = 1;
            foreach (var thread in threads)
            {
                Console.WriteLine($"[{index}]");
                Console.WriteLine($"Thread name: {thread.thread_name}");
                Console.WriteLine($"Created by: {thread.createdBy}");
                Console.WriteLine($"Thread text: {thread.thread_text}");
                Console.WriteLine("---------------------------------");
                index++;
            }
        }
        static void ShowPosts(int threadId)
        {
            var posts = PostRepository.GetPosts(threadId);
            int index = 1;
            foreach (var post in posts)
            {
                Console.WriteLine($"Post number:{index}");
                Console.WriteLine($"Created by: {post.createdBy}");
                Console.WriteLine(post.post_text);
                Console.WriteLine("---------------------------------");
                index++;
            }
        }
        static List<Post> GetPostByCurrentUser(int threadId)
        {
            var posts = PostRepository.GetPosts(threadId);
            var postsByUser = new List<Post>();
            foreach(var post in posts)
            {
                if (post.user_id == CurrentUser.user_id)
                {
                    postsByUser.Add(post);
                }
            }
            return postsByUser;
        }
        static void ShowThreadAndPosts(int id)
        {
            Console.Clear();
            var thread = ThreadRepository.GetThreadByThreadId(id);
            Console.WriteLine($"Thread name: {thread.thread_name}");
            Console.WriteLine($"Created by: {thread.createdBy}");
            Console.WriteLine($"Thread text: {thread.thread_text}");
            Console.WriteLine("---------------------------------");
            ShowPosts(id);
        }
        static void ThreadChoices()
        {
            Console.WriteLine("What do you want to do now?");
            Console.WriteLine("0. Get back to mainmenu.");
            Console.WriteLine("1. Create new thread.");
            Console.WriteLine("2. Update one of your current threads.");
            Console.WriteLine("3. Delete one of your current threads.");
            Console.WriteLine("Write your choice:");
            bool isOkay = int.TryParse(Console.ReadLine(), out int input);
            while (!isOkay || !Helper.VerifyIntBetween(0, 3, input))
            {
                Console.WriteLine("You can only enter numbers that exists infront of the choices. Try again.");
                Console.WriteLine("Write the number infront one of the options:");
                isOkay = int.TryParse(Console.ReadLine(), out input);
            }
            switch (input)
            {
                case 0:
                    Console.WriteLine("Getting back to mainmenu..");
                    Helper.PressAnyKeyToContinue();
                    break;
                case 1:
                    CreateNewThread();
                    break;
                case 2:
                    UpdateThread();
                    break;
                case 3:
                    DeleteThread();
                    break;
                default:
                    Console.WriteLine("Error");
                    Helper.PressAnyKeyToContinue();
                    break;
            }
        }
        static void DeleteThread()
        {
            var threadsByUser = ThreadRepository.GetThreadsByUserId(CurrentUser.user_id);
            bool isEmpty = !threadsByUser.Any();
            if (!isEmpty)
            {
                ShowThreadsFromList(threadsByUser);
                Console.WriteLine("Which thread do you want to delete?");
                bool isOkay = int.TryParse(Console.ReadLine(), out int input);
                while (!isOkay || !Helper.VerifyIntBetween(1, threadsByUser.Count, input))
                {
                    Console.WriteLine("You can only enter numbers that exists in the threadnumbers. Try again.");
                    Console.WriteLine("Write the number of the thread you want to enter:");
                    isOkay = int.TryParse(Console.ReadLine(), out input);
                }
                int index = 1;
                var tmpThread = new Thread();
                foreach (var thread in threadsByUser)
                {
                    if (index == input)
                    {
                        tmpThread = thread;
                    }
                    index++;
                }
                ThreadRepository.DeleteThread(tmpThread);
                Console.WriteLine("Thread deleted.");
            }
            else
            {
                Console.WriteLine("You don't have any threads.");
            }
            Helper.PressAnyKeyToContinue();
        }
        static void UpdateThread()
        {
            var threadsByUser = ThreadRepository.GetThreadsByUserId(CurrentUser.user_id);
            bool isEmpty = !threadsByUser.Any();
            if(!isEmpty)
            {
                ShowThreadsFromList(threadsByUser);
                Console.WriteLine("Which thread do you want to edit?");
                bool isOkay = int.TryParse(Console.ReadLine(), out int input);
                while (!isOkay || !Helper.VerifyIntBetween(1, threadsByUser.Count, input))
                {
                    Console.WriteLine("You can only enter numbers that exists in the threadnumbers. Try again.");
                    Console.WriteLine("Write the number of the thread you want to enter:");
                    isOkay = int.TryParse(Console.ReadLine(), out input);
                }
                int index = 1;
                var tmpThread = new Thread();
                foreach(var thread in threadsByUser)
                {
                    if(index==input)
                    {
                        tmpThread = thread;
                    }
                    index++;
                }
                Console.WriteLine("What do you want to edit?");
                Console.WriteLine("0. Get back to menu");
                Console.WriteLine("1. Update thread name");
                Console.WriteLine("2. Update thread text");
                Console.WriteLine("Write your choice");
                isOkay = int.TryParse(Console.ReadLine(), out input);
                while (!isOkay || !Helper.VerifyIntBetween(0, 2, input))
                {
                    Console.WriteLine("You can only enter numbers that exists in the choices. Try again.");
                    Console.WriteLine("Write the number of the choice you want to enter:");
                    isOkay = int.TryParse(Console.ReadLine(), out input);
                }
                switch (input)
                {
                    case 0:
                        Console.WriteLine("Getting back to menu");
                        break;
                    case 1:
                        Console.WriteLine("Write new thread name:");
                        string tmpThreadName = Console.ReadLine();
                        tmpThread.thread_name = tmpThreadName;
                        ThreadRepository.UpdateThreadName(tmpThread);
                        Console.WriteLine("Thread updated.");
                        break;
                    case 2:
                        Console.WriteLine("Write new thread text:");
                        string tmpThreadText = Console.ReadLine();
                        tmpThread.thread_text = tmpThreadText;
                        ThreadRepository.UpdateThreadText(tmpThread);
                        Console.WriteLine("Thread updated.");
                        break;
                    default:
                        Console.WriteLine("Error");
                        break;
                }
            }
            else
            {
                Console.WriteLine("You don't have any threads.");
            }
            Helper.PressAnyKeyToContinue();
        }
        static void PostsChoices(int threadId)
        {
            Console.WriteLine("What do you want to do now?");
            Console.WriteLine("0. Get back to mainmenu.");
            Console.WriteLine("1. Create new post.");
            Console.WriteLine("2. Update one of your current posts.");
            Console.WriteLine("3. Delete one of your current posts.");
            Console.WriteLine("Write your choice:");
            bool isOkay = int.TryParse(Console.ReadLine(), out int input);
            while (!isOkay || !Helper.VerifyIntBetween(0, 3, input))
            {
                Console.WriteLine("You can only enter numbers that exists infront of threads. Try again.");
                Console.WriteLine("Write the number infront of the thread you want to enter:");
                isOkay = int.TryParse(Console.ReadLine(), out input);
            }
            switch (input)
            {
                case 0:
                    Console.WriteLine("Getting back to mainmenu..");
                    Helper.PressAnyKeyToContinue();
                    break;
                case 1:
                    CreateNewPost(threadId);
                    break;
                case 2:
                    UpdatePost(threadId);
                    break;
                case 3:
                    DeletePost(threadId);
                    break;
                default:
                    Console.WriteLine("Error");
                    Helper.PressAnyKeyToContinue();
                    break;
            }
        }
        static void CreateNewPost(int threadId)
        {
            Console.WriteLine("Write your post text: ");
            string tmpPostText = Console.ReadLine();
            var tmpPost = new Post();
            tmpPost.post_text = tmpPostText;
            tmpPost.user_id = CurrentUser.user_id;
            tmpPost.thread_id = threadId; 
            PostRepository.CreateNewPost(tmpPost);
            Console.WriteLine("New post created.");
            Helper.PressAnyKeyToContinue();
        }
        static void UpdatePost(int threadId)
        {
            var postsByUser = GetPostByCurrentUser(threadId);
            bool isEmpty = !postsByUser.Any();
            if (!isEmpty)
            {
                int index = 1;
                foreach(var post in postsByUser)
                {
                    Console.WriteLine($"Post number:{index}");
                    Console.WriteLine($"Created by: {post.createdBy}");
                    Console.WriteLine(post.post_text);
                    Console.WriteLine("---------------------------------");
                    index++;
                }
                var tmpPost = new Post();
                Console.WriteLine("These are your posts in this thread. Write the number of the post you want to update.");
                bool isOkay = int.TryParse(Console.ReadLine(), out int input);
                while (!isOkay || !Helper.VerifyIntBetween(1, postsByUser.Count, input))
                {
                    Console.WriteLine("You can only enter numbers that exists infront of threads. Try again.");
                    Console.WriteLine("Write the post number of the post you want to update:");
                    isOkay = int.TryParse(Console.ReadLine(), out input);
                }
                Console.WriteLine("Write the new post text:");
                string tmpPostText = Console.ReadLine();
                index = 1;
                foreach(var post in postsByUser)
                {
                    if (index == input)
                    {
                        post.post_text = tmpPostText;
                        tmpPost = post;
                    }
                }
                PostRepository.UpdatePost(tmpPost);
                Console.WriteLine("Post updated.");
            }
            else
            {
                Console.WriteLine("You don't have any posts in this thread.");
            }
            Helper.PressAnyKeyToContinue();
        }
        static void DeletePost(int threadId)
        {
            var postsByUser = GetPostByCurrentUser(threadId);
            bool isEmpty = !postsByUser.Any();
            if(!isEmpty)
            {
                int index = 1;
                foreach (var post in postsByUser)
                {
                    Console.WriteLine($"Post number:{index}");
                    Console.WriteLine($"Created by: {post.createdBy}");
                    Console.WriteLine(post.post_text);
                    Console.WriteLine("---------------------------------");
                    index++;
                }
                var tmpPost = new Post();
                Console.WriteLine("These are your posts in this thread. Write the number of the post you want to delete.");
                bool isOkay = int.TryParse(Console.ReadLine(), out int input);
                while (!isOkay || !Helper.VerifyIntBetween(1, postsByUser.Count, input))
                {
                    Console.WriteLine("You can only enter numbers that exists infront of threads. Try again.");
                    Console.WriteLine("Write the post number of the post you want to delete:");
                    isOkay = int.TryParse(Console.ReadLine(), out input);
                }
                index = 1;
                foreach (var post in postsByUser)
                {
                    if (index == input)
                        tmpPost = post;
                    index++;
                }
                PostRepository.DeletePost(tmpPost);
                Console.WriteLine("Post deleted.");
            }
            else
            {
                Console.WriteLine("You don't have any posts in this thread.");
            }
            Helper.PressAnyKeyToContinue();
        }
        static void CreateNewThread()
        {
            Console.WriteLine("Write the name of the thread: ");
            string tmpThreadName = Console.ReadLine();
            Console.WriteLine("Write your thread text: ");
            string tmpThreadText = Console.ReadLine();
            var tmpThread = new Thread();
            tmpThread.thread_name = tmpThreadName;
            tmpThread.thread_text = tmpThreadText;
            tmpThread.user_id = CurrentUser.user_id;
            ThreadRepository.CreateNewThread(tmpThread);
            Console.WriteLine("New thread created.");
            Helper.PressAnyKeyToContinue();
        }
    }
}
