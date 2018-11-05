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
            MainMenu(MenuItems);
        }

        public static string GetInput(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
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

        public static void MainMenu(Dictionary<int, Product> MenuList)
        {
            while (true)
            {
                string switchAnswer = GetInput("Please choose from the following for a list of products:\n1. Tea\n2. Latte \n3. Coffee\n4. Iced Tea\n5. Sweet food\n6. Savory food");

                switch (switchAnswer)
                {
                    case "1":
                        Category tea = Category.Tea;
                        DisplayProductList(MenuList, tea);
                        break;
                    case "2":
                        Category latte = Category.Latte;
                        DisplayProductList(MenuList, latte);
                        break;
                    case "3":
                        Category coffee = Category.Coffee;
                        DisplayProductList(MenuList, coffee);
                        break;
                    case "4":
                        Category chilled = Category.Chilled;
                        DisplayProductList(MenuList, chilled);
                        break;
                    case "5":
                        Category sweet = Category.Sweet;
                        DisplayProductList(MenuList, sweet);
                        break;
                    case "6":
                        Category savory = Category.Savory;
                        DisplayProductList(MenuList, savory);
                        break;
                    default:
                        Console.WriteLine("That is not a valid selection, try again.");
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

        public static void DisplayProductList(Dictionary<int, Product> MenuItems, Category choice)
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
        }
    }
}
