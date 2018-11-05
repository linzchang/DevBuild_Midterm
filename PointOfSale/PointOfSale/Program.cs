using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale
{
    class Program
    {
        static void Main(string[] args)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo directory = new DirectoryInfo(currentDirectory);
            string fileName = Path.Combine(directory.FullName, "MenuItems.csv");
            Dictionary<int, Product> MenuItems = ReadMenu(fileName);
            Console.WriteLine("Welcome to Pink Moon Cafe!");
            DisplayMainMenu(MenuItems);
        }

        public static string GetInput(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }

        public static int GetProductKey(string message)
        {
            int number;
            Console.WriteLine(message);
            while (!int.TryParse(Console.ReadLine(), out number) || number < 1 || number > 18)
            {
                Console.WriteLine($"Sorry that's invalid, please try again! {message}");
            }
            return number;
        }

        public static string ReadFile(string fileName)
        {
            using (var reader = new StreamReader(fileName))
            {
                return reader.ReadToEnd();
            }
        }

        public static Dictionary<int, Product> ReadMenu(string fileName)
        {
            Dictionary<int, Product> products = new Dictionary<int, Product>();
            int count = 1;
            //var MenuItems = new List<Product>();
            using (var reader = new StreamReader(fileName))
            {
                string line = "";
                reader.ReadLine();
                while ((line = reader.ReadLine()) != null)
                {
                    var product = new Product();
                    string[] values = line.Split(',');

                    product.Name = values[0];
                    Category category;
                    if (Enum.TryParse(values[1], out category))
                    {
                        product.Category = category;
                    }
                    product.Description = values[2];
                    double price;
                    if (double.TryParse(values[3], out price))
                    {
                        product.Price = price;
                    }

                    products.Add(count, product);
                    count++;
                }
            }
            return products;
        }

        public static void DisplayMainMenu(Dictionary<int, Product> MenuList)
        {
            string switchAnswer = GetInput("Please choose from the following for a list of products:\n1. Tea\n2. Latte \n3. Coffee");
            while (true)
            {
                switch (switchAnswer)
                {
                    case "1":
                        Category tea = Category.Tea;
                        DisplayProductListByCategory(MenuList, tea);
                        break;
                    case "2":
                        Category latte = Category.Latte;
                        DisplayProductListByCategory(MenuList, latte);
                        break;
                    case "3":
                        Category coffee = Category.Coffee;
                        DisplayProductListByCategory(MenuList, coffee);
                        break;
                    default:
                        switchAnswer = GetInput("That is not a valid selection, try again.");
                        continue;
                }

                string endProgram = GetInput("Would you like to continue to the main menu?  Press Y to continue");
                if (endProgram.ToUpper() != "Y")
                {
                    break;
                }
                Console.Clear();
            }
        }

        public static void DisplayProductListByCategory(Dictionary<int, Product> MenuItems, Category choice)
        {
            Console.WriteLine(" ");
            Console.WriteLine($"Available {choice}s:");
            Console.WriteLine("{0, -10:0} {1, -20:0} {2, -50:0} {3, -20:0}", "Item", "Name", "Description", "Price");
            Console.WriteLine("{0, -10:0} {1, -20:0} {2, -50:0} {3, -20:0}", "====", "====", "===========", "=====");

            foreach (KeyValuePair<int, Product> item in MenuItems)
            {
                if (item.Value.Category == choice)
                {
                    Console.WriteLine("{0, -10:0} {1, -20:0} {2, -50:0} {3, -20:C}", item.Key, item.Value.Name, item.Value.Description, item.Value.Price);
                }
            }
            Console.WriteLine(" ");
            SelectPurchaseOrMenu(MenuItems);
        }

        public static void SelectPurchaseOrMenu(Dictionary<int, Product> MenuItems)
        {
            Console.WriteLine("Would you like to purchase an item or return to the Main Menu to view a list of item categories?");
            string answer = GetInput("Type P for Purchase or M for Menu");
            while (true)
            {
                switch (answer.ToUpper())
                {
                    case "P":
                        MakePurchase(MenuItems);
                        break;
                    case "M":
                        Console.Clear();
                        DisplayMainMenu(MenuItems);
                        break;
                    default:
                        answer = GetInput("That is not a valid selection, please try again.");
                        continue;
                }
            }
        }

        public static void MakePurchase(Dictionary<int, Product> MenuItems)
        {
            int key = GetProductKey("Which item would you like to purchase?\nPlease enter the item number shown on the left.");
            foreach (KeyValuePair<int, Product> item in MenuItems)
            {
                if (item.Key == key)
                {
                    Product product = item.Value;
                    Console.WriteLine($"You selected {product.Name}.");
                }
            }
        }
    }
}