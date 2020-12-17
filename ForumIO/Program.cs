using System;
using Library;

namespace ForumIO
{
    class Program
    {
        public static bool LogInOkay = false;
        public static bool Running = true;
        public static string CurrentUser = string.Empty;
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
                var tmpUsername = UserRepository.GetUsernameById(thread.user_id);
                Console.WriteLine($"[{index}]");
                Console.WriteLine($"Thread name: {thread.thread_name}");
                Console.WriteLine($"Created by: {tmpUsername.username}");
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
            ShowThreadAndPosts(input);
            Helper.PressAnyKeyToContinue();


        }
        static void ShowThreadAndPosts(int id)
        {
            Console.Clear();
            var thread = ThreadRepository.GetThreadById(id);
            var tmpUsername = UserRepository.GetUsernameById(thread.user_id);
            Console.WriteLine($"Thread name: {thread.thread_name}");
            Console.WriteLine($"Created by: {tmpUsername.username}");
            Console.WriteLine($"Thread text: {thread.thread_text}");
            Console.WriteLine("---------------------------------");
            var posts = PostRepository.GetPosts(id);
            int index = 1;
            foreach(var post in posts)
            {
                var tmpUsernameForPosts = UserRepository.GetUsernameById(post.user_id);
                Console.WriteLine($"Post number:{index}");
                Console.WriteLine($"Created by: {tmpUsernameForPosts.username}");
                Console.WriteLine(post.post_text);
                Console.WriteLine("---------------------------------");
                index++;
            }

        }
    }
}
