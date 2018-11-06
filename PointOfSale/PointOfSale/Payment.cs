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
            ulong cardNumber = GetCardNumber("Please enter the credit card number:");
            //get expiration
            //get CVV

            Console.WriteLine("Thank you for your payment!");
            Console.ReadLine();
        }

        public static void Check()
        {
            string routingNumber = GetRoutingNumber();
            ulong accountNumber = GetAccountNumber();
            string account = accountNumber.ToString();
            Console.WriteLine($"You entered {routingNumber} for the routing number and the account number ends with {account.Substring(account.Length - 4)}.");
            Console.WriteLine("Thank you for your payment!");
        }

        public static string GetRoutingNumber()
        {
            string routingNumber = Input.GetInput("Please enter the routing number:");
            while (true)
            {
                if (Regex.IsMatch(routingNumber, @"^[0-9]{1,9}$") && !Regex.IsMatch(routingNumber, @"[A-Z]$"))
                {
                    return routingNumber;
                }
                else
                {
                    routingNumber = Input.GetInput("Sorry, that's not a valid routing number.\nPlease enter a 9 digit number:");
                }
            }
        }

        public static ulong GetAccountNumber()
        {
            ulong accountNumber = Input.GetULong("Please enter the account number:");
            while (true)
            {
                if (Regex.IsMatch(accountNumber.ToString(), @"^[0-9]{1,12}$") && !Regex.IsMatch(accountNumber.ToString(), @"[A-Z]$"))
                {
                    return accountNumber;
                }
                else
                {
                    accountNumber = Input.GetULong("That's not a valid account number.\nPlease enter a valid account number, up to 12 digits.");
                }
            }
        }

        public static ulong GetCardNumber(string message)
        {
            ulong number;

            while (true)
            {
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.Black;
                string input = Console.ReadLine();
                //if (!ulong.TryParse(input, out number) && Regex.IsMatch(input, @"\d{16}"))
                if (Regex.IsMatch(input, @"\d{16}"))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    number = ulong.Parse(input);
                    return number;
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Sorry, that's invalid. You must enter a 16 digit number.");
                continue;
            }
            
        }
    }
}
