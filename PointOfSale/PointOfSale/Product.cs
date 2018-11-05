using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale
{
    class Product
    {
        public string Name { get; set; }
        public Category Category { get; set; }
        public SubCategory SubCategory { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }

    public enum Category
    {
        Beverage,
        Food
    }

    public enum SubCategory
    {
        Latte,
        Tea,
        Coffee,
        Sweet,
        Savory,
        IcedTea
    }
}
