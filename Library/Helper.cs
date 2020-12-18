using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Library
{
    public static class Helper
    {
        public static bool VerifyStringOnlyLettersAndNumbers(string input)
        {
            bool isOkay = true;
            if (input.Length < 4 || input.Length > 20)
                isOkay = false;
            if (!Regex.IsMatch(input, @"^[a-zA-Z0-9]+$"))
                isOkay = false;
            if (String.IsNullOrWhiteSpace(input))
                isOkay = false;

            return isOkay;
        }
        public static string ReturnOkayString(string input, int lower, int upper)
        {
            while(input.Length < lower || input.Length > upper)
            {
                Console.WriteLine($"Invalid input. Your text must be between {lower} and {upper} characters. ");
                input = Console.ReadLine();
            }

            return input;
        }
        public static bool VerifyIntBetween(int lower, int upper, int input)
        {
            bool isOkay = true;
            if (input < lower)
                isOkay = false;
            if (input > upper)
                isOkay = false;
            return isOkay;
        }
        public static void PressAnyKeyToContinue()
        {
            Console.WriteLine("Press any key to continue..");
            Console.ReadKey();
        }
    }
}
