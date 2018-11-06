using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PointOfSale
{
    class Payment
    {
        public static void Cash(decimal grandTotal)
        {
            decimal userPayment = Input.GetDecimal("Please enter the amount of cash for payment (in decimal format):");
            decimal amountOwed = grandTotal - userPayment;

            if (userPayment > grandTotal)
            {
                decimal change = grandTotal - userPayment;
                Console.WriteLine($"Your change is {Math.Abs(change):C}");
            }

            while (amountOwed > 0)
            {
                userPayment = Input.GetDecimal($"Sorry, you still owe {amountOwed:C}.  Please enter the amount of cash for payment:");
                amountOwed -= userPayment;
            }

            Console.WriteLine("Thank you for your payment!");
        }

        public static void Credit()
        {
            string cardNumber = GetCardNumber("Please enter the credit card number:");
            //get expiration
            //get CVV

            Console.WriteLine("Thank you for your payment!");
            Console.ReadLine();
        }

        public static void Check()
        {
            string checkNumber = GetNumberUsingRegex("Please enter the check number:", @"^\d{4}$", 
                "Sorry, that's not a valid check number.\nPlease enter a 4 digit number:");
            string routingNumber = GetNumberUsingRegex("Please enter the routing number:", @"^\d{9}$", 
                "Sorry, that's not a valid routing number.\nPlease enter a 9 digit number:");
            string accountNumber = GetNumberUsingRegex("Please enter the account number:", @"^\d{1,12}$", 
                "That's not a valid account number.\nPlease enter a valid account number, up to 12 digits.");
            Console.WriteLine($"You entered check number {checkNumber} with routing number {routingNumber} " +
                $"and account number ending with {accountNumber.Substring(accountNumber.Length - 4)}.");
            Console.WriteLine("Thank you for your payment!");
        }

        public static string GetCardNumber(string message)
        {
            while (true)
            {
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.Black;
                string input = Console.ReadLine();
                if (Regex.IsMatch(input, @"^\d{16}$"))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    return input;
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Sorry, that's invalid. You must enter a 16 digit number.");
                continue;
            }

        }

        public static string GetNumberUsingRegex(string message, string regex, string invalid)
        {
            string number = Input.GetInput(message);
            while (true)
            {
                if (Regex.IsMatch(number, regex))
                {
                    return number;
                }
                else
                {
                    number = Input.GetInput(invalid);
                }
            }
        }

    }
}
