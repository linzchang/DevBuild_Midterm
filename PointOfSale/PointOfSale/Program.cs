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

            Console.Title = "Pink Moon Cafe";
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Welcome to Pink Moon Cafe!\n");
            Console.ResetColor();
            DisplayProductList(MenuItems, userPurchases);
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
            using (StreamReader reader = new StreamReader(fileName))
            {
                string line = "";
                reader.ReadLine();
                while ((line = reader.ReadLine()) != null)
                {
                    Product product = new Product();
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

        public static void DisplayProductList(Dictionary<int, Product> MenuItems, List<Product> userPurchases)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.BackgroundColor = ConsoleColor.White;
            GetList(MenuItems, userPurchases, Category.Tea);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.BackgroundColor = ConsoleColor.White;
            GetList(MenuItems, userPurchases, Category.Latte);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.BackgroundColor = ConsoleColor.White;
            GetList(MenuItems, userPurchases, Category.Coffee);
            Console.ResetColor();
            StartPurchase(ref MenuItems, userPurchases);
        }

        public static void GetList(Dictionary<int, Product> MenuItems, List<Product> userPurchases, Category category)
        {
            Console.WriteLine($"Available {category}s:");
            Console.WriteLine("{0, -10:0} {1, -30:0} {2, -50:0} {3, -20:0}", "Item", "Name", "Description", "Price");
            Console.WriteLine("{0, -10:0} {1, -30:0} {2, -50:0} {3, -20:0}", "====", "====", "===========", "=====");
            foreach (KeyValuePair<int, Product> item in MenuItems)
            {
                if (item.Value.Category == category)
                {
                    Console.WriteLine("{0, -10:0} {1, -30:0} {2, -50:0} {3, -20:C}", item.Key, item.Value.Name, item.Value.Description, item.Value.Price);
                }
            }
            Console.WriteLine(" ");
        }

        public static void StartPurchase(ref Dictionary<int, Product> MenuItems, List<Product> userPurchases)
        {
            while (true)
            {
                int key = Input.GetNumber("\nWhich item would you like to purchase?\nPlease enter the item number shown on the left. ", MenuItems);
                foreach (KeyValuePair<int, Product> item in MenuItems)
                {
                    if (item.Key == key)
                    {
                        Product product = item.Value;
                        Console.WriteLine($"\nYou selected {product.Name}.");
                        int quantity = Input.GetNumber($"How many {product.Name} drinks would you like to buy? ");
                        product.Quantity = quantity;
                        decimal lineTotal = Product.GetLineTotal(product);
                        Console.WriteLine($"{quantity} {product.Name} drink(s) will cost {lineTotal:C}.");
                        userPurchases.Add(product);

                        string answer = Input.GetString("\nWould you like to complete this purchase or buy more items?\n" +
                            "Type C to Complete purchase or B to Buy another item.");
                        if (answer.ToUpper() == "C" || answer.ToUpper() == "COMPLETE")
                        {
                            Console.Clear();
                            CompletePurchase(MenuItems, userPurchases);
                        }
                        else if (answer.ToUpper() == "B" || answer.ToUpper() == "BUY")
                        {
                            Console.Clear();
                            DisplayProductList(MenuItems, userPurchases);
                            //continue;
                        }
                        else
                        {
                            answer = Input.GetString("That's not valid, please select C to complete purchase or B buy more items.");
                            continue;
                        }
                    }
                }
            }
        }

        public static void CompletePurchase(Dictionary<int, Product> MenuItems, List<Product> userPurchases)
        {
            decimal subtotal = Product.GetSubtotal(userPurchases);
            decimal tax = Product.GetSalesTax(subtotal);
            decimal grandTotal = subtotal + tax;

            Console.WriteLine("You selected the following items:");
            foreach (var item in userPurchases)
            {
                Console.WriteLine($"{item.Quantity} {item.Name}(s) for {item.Price:C} each");
            }
            Console.WriteLine($"\nSubtotal: {subtotal:C}\nTax: {tax:C}\nGrand Total: {grandTotal:C}\n");

            string payment = Input.GetString("How would you like to pay?\n1. Cash\n2. Credit\n3. Check\n");
            while (true)
            {
                switch (payment.ToLower())
                {
                    case "1":
                    case "cash":
                        decimal change = Payment.Cash(grandTotal);
                        GetReceipt(MenuItems, subtotal, tax, grandTotal, PayType.Cash, userPurchases, change);
                        break;
                    case "2":
                    case "credit":
                        string cardNumber = Payment.Credit();
                        GetReceipt(MenuItems, subtotal, tax, grandTotal, PayType.Credit, userPurchases, cardNumber);
                        break;
                    case "3":
                    case "check":
                        string checkNumber = Payment.Check();
                        GetReceipt(MenuItems, subtotal, tax, grandTotal, PayType.Check, userPurchases, checkNumber);
                        break;
                    default:
                        payment = Input.GetString("Sorry, that's not valid.  Please try again.\nPlease choose 1 for Cash, 2 for Credit or 3 for Check.");
                        continue;
                }
            }
        }

        public static void GetReceipt(Dictionary<int, Product> MenuItems, decimal subtotal, decimal tax, decimal total, PayType payType, List<Product> userPurchases, string number)
        {
            Console.WriteLine("\nThank you for your payment! Here is your receipt:\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("______________________________________");
            Console.WriteLine("\n           PINK MOON CAFE\n");
            foreach (var item in userPurchases)
            {
                Console.WriteLine("{0, -5:0} {1, -20:0} {2, -10:C}", item.Quantity, item.Name, item.Price);
            }
            Console.WriteLine($"\nSubtotal: {subtotal:C}");
            Console.WriteLine($"Tax: {tax:C}");
            Console.WriteLine($"Total: {total:C}\n");
            Console.WriteLine($"Payment type: {payType}");
            if (payType == PayType.Credit)
            {
                Console.WriteLine($"Credit card: {number}");
            }
            else
            {
                Console.WriteLine($"Check number: {number}");
            }
            Console.WriteLine("______________________________________");
            Console.ResetColor();
            NewOrderOrQuit(MenuItems, userPurchases);
        }

        public static void GetReceipt(Dictionary<int, Product> MenuItems, decimal subtotal, decimal tax, decimal total, PayType payType, List<Product> userPurchases, decimal change)
        {
            
            Console.WriteLine("\nThank you for your payment! Here is your receipt:\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("______________________________________");
            Console.WriteLine("\n           PINK MOON CAFE\n");
            foreach (var item in userPurchases)
            {
                Console.WriteLine("{0, -5:0} {1, -20:0} {2, -10:C}", item.Quantity, item.Name, item.Price);
            }
            Console.WriteLine($"\nSubtotal: {subtotal:C}");
            Console.WriteLine($"Tax: {tax:C}");
            Console.WriteLine($"Total: {total:C}\n");
            Console.WriteLine($"Payment type: {payType}");
            Console.WriteLine($"Change: {Math.Abs(change):C}\n");
            Console.WriteLine("______________________________________");
            Console.ResetColor();
            NewOrderOrQuit(MenuItems, userPurchases); 
        }

        public static void NewOrderOrQuit(Dictionary<int, Product> MenuItems, List<Product> userPurchases)
        {
            string answer = Input.GetString("\nWould you like to start over and place a new order or quit?\nType N for New Order or Q to Quit.");
            while (true)
            {
                switch(answer.ToUpper())
                {
                    case "N":
                    case "NEW ORDER":
                        userPurchases.Clear();
                        Console.Clear();
                        DisplayProductList(MenuItems, userPurchases);
                        break;
                    case "Q":
                    case "QUIT":
                        Environment.Exit(0);
                        break;
                    default:
                        answer = Input.GetString("Sorry, that's not valid.  Please press N for a New Order or Q to quit.");
                        continue;
                }
            }
        }

    }
}