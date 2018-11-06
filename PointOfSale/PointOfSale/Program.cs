using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PointOfSale
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);

            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo directory = new DirectoryInfo(currentDirectory);
            string fileName = Path.Combine(directory.FullName, "MenuItems.csv");
            Dictionary<int, Product> MenuItems = ReadMenu(fileName);

            List<Product> userPurchases = new List<Product>();

            Console.WriteLine("Welcome to Pink Moon Cafe!");
            DisplayMainMenu(MenuItems, userPurchases);
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
                    decimal price;
                    if (decimal.TryParse(values[3], out price))
                    {
                        product.Price = price;
                    }

                    products.Add(count, product);
                    count++;
                }
            }
            return products;
        }

        public static void DisplayMainMenu(Dictionary<int, Product> MenuList, List<Product> userPurchases)
        {
            string switchAnswer = Input.GetInput("Please choose from the following for a list of products:\n1. Tea\n2. Latte \n3. Coffee");
            while (true)
            {
                switch (switchAnswer)
                {
                    case "1":
                        Category tea = Category.Tea;
                        DisplayProductListByCategory(MenuList, tea, userPurchases);
                        break;
                    case "2":
                        Category latte = Category.Latte;
                        DisplayProductListByCategory(MenuList, latte, userPurchases);
                        break;
                    case "3":
                        Category coffee = Category.Coffee;
                        DisplayProductListByCategory(MenuList, coffee, userPurchases);
                        break;
                    default:
                        switchAnswer = Input.GetInput("That is not a valid selection, try again.");
                        continue;
                }

                string endProgram = Input.GetInput("Would you like to continue to the main menu?  Press Y to continue");
                if (endProgram.ToUpper() != "Y")
                {
                    break;
                }
                Console.Clear();
            }
        }

        public static void DisplayProductListByCategory(Dictionary<int, Product> MenuItems, Category choice, List<Product> userPurchases)
        {
            Console.WriteLine(" ");
            Console.WriteLine($"Available {choice}s:");
            Console.WriteLine("{0, -10:0} {1, -30:0} {2, -50:0} {3, -20:0}", "Item", "Name", "Description", "Price");
            Console.WriteLine("{0, -10:0} {1, -30:0} {2, -50:0} {3, -20:0}", "====", "====", "===========", "=====");

            foreach (KeyValuePair<int, Product> item in MenuItems)
            {
                if (item.Value.Category == choice)
                {
                    Console.WriteLine("{0, -10:0} {1, -30:0} {2, -50:0} {3, -20:C}", item.Key, item.Value.Name, item.Value.Description, item.Value.Price);
                }
            }
            Console.WriteLine(" ");
            SelectPurchaseOrMenu(MenuItems, userPurchases);
        }

        public static void SelectPurchaseOrMenu(Dictionary<int, Product> MenuItems, List<Product> userPurchases)
        {
            Console.WriteLine("Would you like to purchase an item or return to the Main Menu to view a list of item categories?");
            string answer = Input.GetInput("Type P for Purchase or M for Menu");
            while (true)
            {
                switch (answer.ToUpper())
                {
                    case "P":
                        MakePurchase(MenuItems, userPurchases);
                        break;
                    case "M":
                        Console.Clear();
                        DisplayMainMenu(MenuItems, userPurchases);
                        break;
                    default:
                        answer = Input.GetInput("That is not a valid selection, please try again.");
                        continue;
                }
            }
        }

        public static void MakePurchase(Dictionary<int, Product> MenuItems, List<Product> userPurchases)
        {
            int key = Input.GetProductKey("Which item would you like to purchase?\nPlease enter the item number shown on the left.", MenuItems);
            foreach (KeyValuePair<int, Product> item in MenuItems)
            {
                if (item.Key == key)
                {
                    Product product = item.Value;
                    Console.WriteLine($"You selected {product.Name}.");
                    int quantity = Input.GetNumber($"How many {product.Name} drinks would you like to buy?");
                    product.Quantity = quantity;
                    decimal lineTotal = Product.GetLineTotal(product);
                    Console.WriteLine($"{quantity} {product.Name} drinks will cost {lineTotal:C}.");
                    userPurchases.Add(product);
                    string answer = Input.GetInput("Would you like to complete this purchase or return to the Main Menu to purchase more items?\n" +
                        "Type C to complete purchase or M to return to Main Menu");
                    while (true)
                    {
                        if (answer.ToUpper() == "C")
                        {
                            CompletePurchase(userPurchases);
                        }
                        else if (answer.ToUpper() == "M")
                        {
                            Console.Clear();
                            DisplayMainMenu(MenuItems, userPurchases);
                        }
                        else
                        {
                            answer = Input.GetInput("That's not valid, please select C to complete purchase or M for Main Menu.");
                            continue;
                        }
                    } 
                }  
            }
        }

        public static void CompletePurchase(List<Product> userPurchases)
        {
            decimal subtotal = Product.GetSubtotal(userPurchases);
            decimal tax = Product.GetSalesTax(subtotal);
            decimal grandTotal = subtotal + tax;

            Console.WriteLine("\nYou selected the following items:");
            foreach (var item in userPurchases)
            {
                Console.WriteLine($"{item.Quantity} {item.Name}(s) for {item.Price:C} each");
            }
            Console.WriteLine($"\nSubtotal: {subtotal:C}\nTax: {tax:C}\nGrand Total: {grandTotal:C}\n");

            string payment = Input.GetInput("How would you like to pay?\n1. Cash\n2. Credit\n3. Check\n");
            while (true)
            {
                switch (payment)
                {
                    case "1":
                        Payment.Cash(grandTotal);
                        break;
                    case "2":
                        Console.WriteLine("You selected Credit");
                        break;
                    case "3":
                        Payment.Check();
                        break;
                    default:
                        payment = Input.GetInput("Sorry, that's not valid.  Please try again.\nPlease choose 1 for Cash, 2 for Credit or 3 for Check.");
                        continue;
                }
            }
        }

    }
}