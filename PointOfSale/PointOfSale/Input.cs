using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale
{
    class Input
    {
        public static string GetString(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }

        public static int GetNumber(string message, Dictionary<int, Product> MenuItems)
        {
            int number;
            Console.Write(message);
            while (!int.TryParse(Console.ReadLine(), out number) || number < 1 || number > MenuItems.Count)
            {
                Console.WriteLine($"Sorry, that's not a valid option.  Please enter an item number.");
            }
            return number;
        }

        public static int GetNumber(string message)
        {
            int number;
            Console.Write(message);
            while (!int.TryParse(Console.ReadLine(), out number) || number < 1)
            {
                Console.WriteLine($"Sorry that's invalid, please enter a number greater than 0.");
            }
            return number;
        }

        public static decimal GetDecimal(string message)
        {
            decimal number;
            Console.Write(message);
            while (!decimal.TryParse(Console.ReadLine(), out number) || number < 0)
            {
                Console.WriteLine($"Sorry that's invalid, please enter a positive number.");
            }
            return number;
        }
    }
}
