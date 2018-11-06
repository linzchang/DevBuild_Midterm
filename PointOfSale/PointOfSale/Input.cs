using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale
{
    class Input
    {
        public static string GetInput(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }

        public static int GetProductKey(string message, Dictionary<int, Product> MenuItems)
        {
            int number;
            Console.WriteLine(message);
            while (!int.TryParse(Console.ReadLine(), out number) || number < 1 || number > MenuItems.Count)
            {
                Console.WriteLine($"Sorry that's invalid, please try again! {message}");
            }
            return number;
        }

        public static int GetNumber(string message)
        {
            int number;
            Console.WriteLine(message);
            while (!int.TryParse(Console.ReadLine(), out number) || number < 1)
            {
                Console.WriteLine($"Sorry that's invalid, please try again! {message}");
            }
            return number;
        }

        public static ulong GetULong(string message)
        {
            ulong number;
            Console.WriteLine(message);
            while (!ulong.TryParse(Console.ReadLine(), out number) || number <= 0)
            {
                Console.WriteLine($"Sorry that's invalid, please try again! {message}");
            }
            return number;
        }

        public static decimal GetDecimal(string message)
        {
            decimal number;
            Console.WriteLine(message);
            while (!decimal.TryParse(Console.ReadLine(), out number))
            {
                Console.WriteLine($"Sorry that's invalid, please try again! {message}");
            }
            return number;
        }
    }
}
