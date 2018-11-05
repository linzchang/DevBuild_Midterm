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
            var fileName = Path.Combine(directory.FullName, "MenuItems.csv");
            var fileContents = ReadSoccerResults(fileName);


        }

        public static string ReadFile(string fileName)
        {
            using (var reader = new StreamReader(fileName))
            {
                return reader.ReadToEnd();
            }
        }

        public static List<Product> ReadSoccerResults(string fileName)
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
    }
}
