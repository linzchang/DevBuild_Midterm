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
        public static decimal Cash(decimal grandTotal)
        {
            decimal userPayment = Input.GetDecimal("\nPlease enter the amount of cash for payment (in decimal format): ");
            decimal amountOwed = grandTotal - userPayment;
            decimal change = 0;

            while (amountOwed > 0)
            {
                userPayment = Input.GetDecimal($"Sorry, you still owe {amountOwed:C}.  Please enter the amount of cash for payment: ");
                if (userPayment > amountOwed)
                {
                    change = userPayment - amountOwed;
                    Console.WriteLine($"Your change is {Math.Abs(change):C}");
                    amountOwed -= userPayment;
                    return change;
                }
                amountOwed -= userPayment;
            }

            if (userPayment > grandTotal)
            {
                change = grandTotal - userPayment;
                Console.WriteLine($"Your change is {Math.Abs(change):C}");
                return change;
            }
            return change;
        }

        public static string Credit()
        {
            string cardNumber = GetInputHideKeyStroke("\nPlease enter the 16 digit credit card number: ", @"^\d{16}$", 
                "Sorry, that's invalid. You must enter a 16 digit number.");
            string expirationDate = Input.GetString("Please enter the expiration date in MM/YYYY format: ");
            string errorMessage = $"Sorry, that's not valid.  Please enter the date in MM/YYYY format. " +
                    "Cards issued before 2000 will be rejected.";
            if (!expirationDate.Contains("/"))
            {
                expirationDate = Input.GetString(errorMessage);
            }
            while (true)
            {
                string[] dateParts = expirationDate.Split('/');

                if (!Regex.IsMatch(dateParts[0], @"^(0[1-9]{1}|1[0-2]{1})$") || !Regex.IsMatch(dateParts[1], @"^20[0-9]{2}$"))
                {
                    expirationDate = Input.GetString(errorMessage);
                    continue;
                }
                break;
            }
            string cvv = GetNumberUsingRegex("Please enter the 3-4 digit CVV: ", @"^\d{3,4}$", "Sorry, that's not valid.  Please enter a 3-4 digit CVV number.");
            Console.WriteLine($"\nYour payment has been processed.\nCredit card ending in {cardNumber.Substring(cardNumber.Length-4)} with " +
                $"expiration {expirationDate} and CVV {cvv}.");
            return cardNumber.Substring(cardNumber.Length - 4);
        }

        public static string Check()
        {
            string checkNumber = GetNumberUsingRegex("\nPlease enter the 3-4 digit check number: ", @"^\d{3,4}$", 
                "Sorry, that's not a valid check number.\nPlease enter a 4 digit number:");
            string routingNumber = GetNumberUsingRegex("Please enter the 9 digit routing number: ", @"^\d{9}$", 
                "Sorry, that's not a valid routing number.\nPlease enter a 9 digit number:");
            string accountNumber = GetInputHideKeyStroke("Please enter the 10-12 digit account number: ", @"^\d{10,12}$", 
                "That's not a valid account number.\nPlease enter a valid account number, up to 12 digits.");
            Console.WriteLine($"\nYour payment has been processed.\nCheck number {checkNumber} with routing number {routingNumber} " +
                $"and account number ending with {accountNumber.Substring(accountNumber.Length - 4)}.");
            return checkNumber;
        }

        public static string GetInputHideKeyStroke(string message, string regex, string error)
        {
            while (true)
            {
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.Black;
                string input = Console.ReadLine();
                if (Regex.IsMatch(input, regex))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    return input;
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(error);
                continue;
            }

        }

        public static string GetNumberUsingRegex(string message, string regex, string invalid)
        {
            string number = Input.GetString(message);
            while (true)
            {
                if (Regex.IsMatch(number, regex))
                {
                    return number;
                }
                else
                {
                    number = Input.GetString(invalid);
                }
            }
        }

    }

    public enum PayType
    {
        Cash,
        Credit,
        Check
    }
}
