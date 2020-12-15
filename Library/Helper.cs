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
            if (!Regex.IsMatch(input, @"^[a-zA-Z0-9]+$")) // Only accepts letters, numbers and spaces.
                isOkay = false;
            if (String.IsNullOrWhiteSpace(input))
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
