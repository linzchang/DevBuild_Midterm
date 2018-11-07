using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using PointOfSale;
using System.Collections.Generic;

namespace POSTests
{
    [TestClass]
    public class Test1
    {
        [TestMethod]
        public void GetLineTotal_MultiplyPriceByQuantity_True()
        {
            //Assign
            Product product = new Product();
            product.Price = 4.5M;
            product.Quantity = 3;

            //Act
            decimal target = (decimal)4.5 * 3M;
            decimal actual = Product.GetLineTotal(product);

            //Assert
            Assert.AreEqual(target, actual);
        }

        [TestMethod]
        public void GetSubtotal_AddsUpLineTotalForItemsInList_True()
        {
            Product product1 = new Product();
            product1.Price = 2.5M;
            product1.Quantity = 2;
            Product product2 = new Product();
            product2.Price = 2.5M;
            product2.Quantity = 1;
            List<Product> list = new List<Product>(){product1, product2};

            decimal target = (decimal)(2.5 * 2) + (decimal)(2.5 * 1);
            decimal actual = Product.GetSubtotal(list);

            Assert.AreEqual(target, actual);
        }

        [TestMethod]
        public void GetSalesTax_MultipliesPriceByTaxForAllItems_True()
        {
            Product product1 = new Product();
            product1.Price = 2.5M;
            Product product2 = new Product();
            product2.Price = 2.5M;
            List<Product> list = new List<Product>() { product1, product2 };
            decimal Subtotal = Product.GetSubtotal(list);

            decimal target = ((decimal)2.5 + (decimal)2.5) * Product.SalesTax;
            decimal actual = Product.GetSalesTax(Subtotal);

            Assert.AreEqual(target, actual);
        }
    }
}
