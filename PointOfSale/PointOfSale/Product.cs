using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale
{
    public class Product
    {
        public string Name { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 1;
        //Set Sales Tax to 6% based on current rate in Michigan
        public const decimal SalesTax = .06M;

        public static decimal GetLineTotal(Product product)
        {
            decimal lineTotal;
            lineTotal = product.Price * product.Quantity;
            return lineTotal;
        }

        public static decimal GetSubtotal(List<Product> purchasedItems)
        {
            decimal Subtotal = 0;
            foreach (var item in purchasedItems)
            {
                Subtotal += Product.GetLineTotal(item);
            }
            return Subtotal;
        }

        public static decimal GetSalesTax(decimal Subtotal)
        {
            return Subtotal * SalesTax;
        }
    }

    public enum Category
    {
        Latte,
        Tea,
        Coffee,
        Sweet,
        Savory
    }
}
