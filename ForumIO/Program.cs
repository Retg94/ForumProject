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
            Console.WriteLine("Welcome to the loginmenu.");
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("0. Exit program");
            Console.WriteLine("1. Log into forum");
            Console.WriteLine("2. Create new user");
            Console.WriteLine("Write your choice:");
            bool isOkay = int.TryParse(Console.ReadLine(), out int input);
            while (!isOkay || !Helper.VerifyIntBetween(0, 2, input))
            {
                Console.WriteLine("You can only enter numbers that exists infront of the choices. Try again.");
                Console.WriteLine("Write the number infront one of the options:");
                isOkay = int.TryParse(Console.ReadLine(), out input);
            }
            switch(input)
            {
                case 0:
                    Console.WriteLine("Exiting program...");
                    Running = false;
                    break;
                case 1:
                    LogIn();
                    break;
                case 2:
                    CreateNewUser();
                    break;
                default:
                    Console.WriteLine("Error");
                    break;
            }
            Helper.PressAnyKeyToContinue();
        }
        static void CreateNewUser()
        {
            var tmpUser = new User();
            var users = UserRepository.GetUsers();
            Console.WriteLine("Write your username: ");
            string tmpUsername = Console.ReadLine();
            bool alreadyExist = false;
            int index = users.FindIndex(user => user.username == tmpUsername);
            if (index >= 0)
            {
                alreadyExist = true;
            }
            while (!Helper.VerifyStringOnlyLettersAndNumbers(tmpUsername) || alreadyExist )
            {
                if(alreadyExist)
                    Console.WriteLine("Username already exists. Try again: ");
                else
                {
                    Console.WriteLine("Invalid input. The username can only contain letters and number and must contain between 4 and 20 characters. Try again:");
                }
                alreadyExist = false;
                tmpUsername = Console.ReadLine();
                index = users.FindIndex(user => user.username == tmpUsername);
                if (index >= 0)
                {
                    alreadyExist = true;
                }
            }
            Console.WriteLine("Write your password: ");
            string tmpPassword = Console.ReadLine();
            while(!Helper.VerifyStringOnlyLettersAndNumbers(tmpPassword))
            {
                Console.WriteLine("Invalid input. The password can only contain letters and number and must contain between 4 and 20 characters. Try again:");
                tmpPassword = Console.ReadLine();
            }
            tmpUser.username = tmpUsername;
            tmpUser.password = tmpPassword;
            UserRepository.CreateNewUser(tmpUser);
            Console.WriteLine("User added.");
        }
        static void LogIn()
        {
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
                if (user.username == tmpUsername && user.password == tmpPassword)
                {
                    CurrentUser = user;
                    Console.WriteLine("Login succed. Continue to main menu...");
                    LogInOkay = true;
                }
            }
            if (!LogInOkay)
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
            bool isOkay = int.TryParse(Console.ReadLine(), out int input);
            while (!isOkay || !Helper.VerifyIntBetween(0, 3, input))
            {
                Console.WriteLine("You can only enter numbers that exists infront of the choices. Try again.");
                Console.WriteLine("Write the number infront one of the options:");
                isOkay = int.TryParse(Console.ReadLine(), out input);
            }
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
            Console.Clear();
            var threads = ThreadRepository.GetThreads();
            Console.WriteLine("Showing all threads in the forum");
            int index = 1;
            int amount = 0;
            foreach (var thread in threads)
            {
                var posts = PostRepository.GetPosts(thread);
                amount = posts.Count;
                Console.WriteLine($"[{index}]");
                Console.WriteLine($"Thread name: {thread.thread_name}");
                Console.WriteLine($"Created by: {thread.createdBy}");
                Console.WriteLine($"Amount of posts: {amount}");
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
            var tmpThread = new Thread();
            foreach(var thread in threads)
            {
                if (input == index)
                {
                    tmpThread = thread;
                    break;
                }            
                index++;
            }
            ShowThreadAndPosts(tmpThread);
            PostsChoices(tmpThread);
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
        static void ShowPosts(Thread thread)
        {
            var posts = PostRepository.GetPosts(thread);
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
        static List<Post> GetPostByCurrentUser(Thread thread)
        {
            var posts = PostRepository.GetPosts(thread);
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
        static void ShowThreadAndPosts(Thread thread)
        {
            Console.Clear();
            var tmpThread = ThreadRepository.GetThreadByThreadId(thread);
            Console.WriteLine($"Thread name: {thread.thread_name}");
            Console.WriteLine($"Created by: {thread.createdBy}");
            Console.WriteLine($"Thread text: {thread.thread_text}");
            Console.WriteLine("---------------------------------");
            ShowPosts(tmpThread);
        }
        static void ThreadChoices()
        {
            Console.Clear();
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
            Console.Clear();
            var threadsByUser = ThreadRepository.GetThreadsByUserId(CurrentUser);
            bool isEmpty = !threadsByUser.Any();
            if (!isEmpty)
            {
                ShowThreadsFromList(threadsByUser);
                Console.WriteLine("Which thread do you want to delete? Enter the number of the thread.");
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
            Console.Clear();
            var threadsByUser = ThreadRepository.GetThreadsByUserId(CurrentUser);
            bool isEmpty = !threadsByUser.Any();
            if(!isEmpty)
            {
                ShowThreadsFromList(threadsByUser);
                Console.WriteLine("Which thread do you want to edit? Enter the number of the thread.");
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
                        tmpThreadName = Helper.ReturnOkayString(tmpThreadName, 4, 25);
                        tmpThread.thread_name = tmpThreadName;
                        ThreadRepository.UpdateThreadName(tmpThread);
                        Console.WriteLine("Thread updated.");
                        break;
                    case 2:
                        Console.WriteLine("Write new thread text:");
                        string tmpThreadText = Console.ReadLine();
                        tmpThreadText = Helper.ReturnOkayString(tmpThreadText, 4, 150);
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
        static void PostsChoices(Thread thread)
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
                    CreateNewPost(thread);
                    break;
                case 2:
                    UpdatePost(thread);
                    break;
                case 3:
                    DeletePost(thread);
                    break;
                default:
                    Console.WriteLine("Error");
                    Helper.PressAnyKeyToContinue();
                    break;
            }
        }
        static void CreateNewPost(Thread thread)
        {
            Console.WriteLine("Write your post text: ");
            string tmpPostText = Console.ReadLine();
            tmpPostText = Helper.ReturnOkayString(tmpPostText, 4, 100);
            var tmpPost = new Post();
            tmpPost.post_text = tmpPostText;
            tmpPost.user_id = CurrentUser.user_id;
            tmpPost.thread_id = thread.thread_id; 
            PostRepository.CreateNewPost(tmpPost);
            Console.WriteLine("New post created.");
            Helper.PressAnyKeyToContinue();
        }
        static void UpdatePost(Thread thread)
        {
            var postsByUser = GetPostByCurrentUser(thread);
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
                tmpPostText = Helper.ReturnOkayString(tmpPostText, 4, 100);
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
        static void DeletePost(Thread thread)
        {
            var postsByUser = GetPostByCurrentUser(thread);
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
            tmpThreadName = Helper.ReturnOkayString(tmpThreadName, 4, 25);
            Console.WriteLine("Write your thread text: ");
            string tmpThreadText = Console.ReadLine();
            tmpThreadText = Helper.ReturnOkayString(tmpThreadText, 4, 150);
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
