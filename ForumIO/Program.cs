using System;
using Library;

namespace ForumIO
{
    class Program
    {
        public static bool LogInOkay = false;
        static void Main(string[] args)
        {
            while(!LogInOkay)
            {
                LoginMenu();
            }
            Console.ReadLine();
        }
        static void LoginMenu()
        {
            var Users = UserRepository.GetUsers();
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
            foreach (var user in Users)
            {
                if(user.Username == tmpUsername && user.Password == tmpPassword)
                {
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
    }
}
