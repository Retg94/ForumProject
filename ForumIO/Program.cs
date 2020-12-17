using System;
using Library;

namespace ForumIO
{
    class Program
    {
        public static bool LogInOkay = false;
        public static bool Running = true;
        public static string CurrentUser = string.Empty;
        public static int CurrentUserId;
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
                    CurrentUser = user.username;
                    CurrentUserId = user.user_id;
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
            Console.WriteLine($"Main menu --- Current user: {CurrentUser}");
            Console.WriteLine("0. Exit program");
            Console.WriteLine("1. Logout");
            Console.WriteLine("2. Show threads");
            Console.WriteLine("3. Create new thread");
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
                    ShowThreads();
                    break;
                case 3:
                    CreateNewThread();
                    break;
                default:
                    Console.WriteLine("Error");
                    Helper.PressAnyKeyToContinue();
                    break;
            }

        }
        static void ShowThreads()
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
            Helper.PressAnyKeyToContinue();


        }
        static void ShowThreadAndPosts(int id)
        {
            Console.Clear();
            var thread = ThreadRepository.GetThreadById(id);
            Console.WriteLine($"Thread name: {thread.thread_name}");
            Console.WriteLine($"Created by: {thread.createdBy}");
            Console.WriteLine($"Thread text: {thread.thread_text}");
            Console.WriteLine("---------------------------------");
            var posts = PostRepository.GetPosts(id);
            int index = 1;
            foreach(var post in posts)
            {
                Console.WriteLine($"Post number:{index}");
                Console.WriteLine($"Created by: {post.createdBy}");
                Console.WriteLine(post.post_text);
                Console.WriteLine("---------------------------------");
                index++;
            }
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
            tmpThread.user_id = CurrentUserId;
            ThreadRepository.CreateNewThread(tmpThread);
            Console.WriteLine("New thread created.");
            Helper.PressAnyKeyToContinue();

        }
    }
}
