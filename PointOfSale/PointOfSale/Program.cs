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
            List<Product> MenuItems = ReadMenu(fileName);
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

        public static List<Product> ReadMenu(string fileName)
        {
            var MenuItems = new List<Product>();
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
                    SubCategory subCategory;
                    if (Enum.TryParse(values[2], out subCategory))
                    {
                        product.SubCategory = subCategory;
                    }
                    product.Description = values[3];
                    double price;
                    if (double.TryParse(values[4], out price))
                    {
                        product.Price = price;
                    }

                    MenuItems.Add(product);
                }
            }
            return MenuItems;
        }

        public static void MainMenu(List<Product> MenuList)
        {
            while (true)
            {
                string switchAnswer = GetInput("Please choose from the following for a list of products:\n1. Tea\n2. Latte \n3. Coffee\n4. Iced Tea\n5. Sweet food\n6. Savory food");

                switch (switchAnswer)
                {
                    case "1":
                        SubCategory tea = SubCategory.Tea;
                        DisplayProductList(MenuList, tea);
                        break;
                    case "2":
                        SubCategory latte = SubCategory.Latte;
                        DisplayProductList(MenuList, latte);
                        break;
                    case "3":
                        SubCategory coffee = SubCategory.Coffee;
                        DisplayProductList(MenuList, coffee);
                        break;
                    case "4":
                        SubCategory icedTea = SubCategory.IcedTea;
                        DisplayProductList(MenuList, icedTea);
                        break;
                    case "5":
                        SubCategory sweet = SubCategory.Sweet;
                        DisplayProductList(MenuList, sweet);
                        break;
                    case "6":
                        SubCategory savory = SubCategory.Savory;
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

        public static void DisplayProductList(List<Product> MenuItems, SubCategory choice)
        {
            Console.WriteLine($"\nAvailable {choice}s:\n");
            Console.WriteLine("{0, -20:0} {1, -50:0} {2, -40:0}", "Name", "Description", "Price");
            Console.WriteLine("{0, -20:0} {0, -50:0} {0, -40:0}", "========");
            foreach (Product item in MenuItems)
            {
                if (item.SubCategory == choice)
                {
                    Console.WriteLine("{0, -20:0} {1, -50:0} {2, -40:C}", item.Name, item.Description,  item.Price);
                }
            }
            Console.WriteLine(" ");
        }
    }
}
