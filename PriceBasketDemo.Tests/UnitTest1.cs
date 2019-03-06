using Microsoft.VisualStudio.TestTools.UnitTesting;
using Business.Models;
using Business.Services;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Console;
using System;
using System.IO;

namespace PriceBasketDemo.Tests
{
    [TestClass]
    public class UnitTest1
    {
        

        List<Product> Products = new List<Product>();
        List<Discount> Discounts = new List<Discount>();
        IShoppingBasketService basketService;
            
        IProductService productService;
        IDiscountService discountService;


        [TestInitialize]
        public void TestInitialize()
        {
            var serviceProvider = new ServiceCollection()
            .AddLogging( builder => builder
            .AddConsole()
            .AddFilter(level =>level >= LogLevel.Information))
            .AddSingleton<IDiscountService, DiscountService>()
            .AddSingleton<IProductService, ProductService>()
            .AddSingleton<IShoppingBasketService, ShoppingBasketService>()
            .BuildServiceProvider();

            //configure console logging

            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();



            productService = serviceProvider.GetService<IProductService>();
            Products = productService.GetAllProducts(@"data\Products.json");
            discountService = serviceProvider.GetService<IDiscountService>();
            Discounts = discountService.GetAllDiscounts(@"data\Discounts.json");
            basketService =serviceProvider.GetService<IShoppingBasketService>();
        }
        [DataTestMethod]
        [DataRow(new string[] {"Apples"})]
        [ExpectedException(typeof(FileNotFoundException))]
        public void MissingProductFile(string[] items)
        {
            productService.GetAllProducts(@"Nosuchfile.json");

        }
        
        [DataTestMethod]
        [DataRow(new string[] { "Apples", "Oranges" })]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidProduct(string[] items)
        {
            ShoppingBasket basket = basketService.CreateShoppingBasket(items, Products);
        }

        [DataTestMethod]
        [DataRow(new string[] { "Apples", "Apples", "Bread", "Soup" }, 3)]
        [DataRow(new string[] { "Apples", "Bread" }, 2)]
        public void TestCreateBasket(string[] items, int expected)
        {

            ShoppingBasket basket = basketService.CreateShoppingBasket(items, Products);
            Assert.AreEqual(expected, basket.ShoppingItems.Count);


        }
        [DataTestMethod]
        [DataRow(new string[] { "Apples", "Apples", "Bread", "Soup" }, "3.45")]
        [DataRow(new string[] { "Apples", "Milk", "Milk", "Bread", "Soup" }, "5.05")]
        public void TestBasketSubTotalWithoutDiscount(string[] items, string expected)
        {
            ShoppingBasket basket = basketService.CreateShoppingBasket(items, Products);
            basketService.CalculateSubTotal(basket);
            Assert.AreEqual(expected, basket.SubTotal.ToString("F2"));
        }
        [DataTestMethod]
        [DataRow(new string[] { "Apples" }, "0.10")]
        [DataRow(new string[] { "Apples", "Apples" }, "0.20")]

        public void TestDiscountAmounts(string[] items, string expected)
        {
            ShoppingBasket basket = basketService.CreateShoppingBasket(items, Products);
            basketService.CalculateSubTotal(basket);
            basketService.CalculateDiscounts(basket, Discounts);
            Assert.AreEqual(expected, basket.DiscountSubTotal.ToString("F2"));
        }
        [DataTestMethod]
        [DataRow(new string[] { "Soup", "Bread", "Milk" }, "0.00")]
        public void TestNoDiscountForBreadIfOneCanOfSoupWasPurchased(string[] items, string expected)
        {
            ShoppingBasket basket = basketService.CreateShoppingBasket(items, Products);
            basketService.CalculateSubTotal(basket);
            basketService.CalculateDiscounts(basket, Discounts);
            Assert.AreEqual(expected, basket.DiscountSubTotal.ToString("F2"));
        }
        [DataTestMethod]
        [DataRow(new string[] { "Soup", "Soup", "Bread", "Milk" }, "0.40")]
        [DataRow(new string[] { "Soup", "Soup", "Bread", "Bread" }, "0.40")]
        [DataRow(new string[] { "Soup", "Soup", "Soup", "Soup", "Bread", "Bread" }, "0.80")]
        [DataRow(new string[] { "Soup", "Soup", "Soup", "Soup", "Bread", "Bread", "Bread" }, "0.80")]
        public void TestDiscountForBreadIfTwoCanOfSoupWasPurchased(string[] items, string expected)
        {
            ShoppingBasket basket = basketService.CreateShoppingBasket(items, Products);
            basketService.CalculateSubTotal(basket);
            basketService.CalculateDiscounts(basket, Discounts);
            Assert.AreEqual(expected, basket.DiscountSubTotal.ToString("F2"));
        }
        [TestMethod]
        [DataRow(new string[] { "Soup","Bread"},"no offers available")]
        [DataRow(new string[] { "Soup", "Soup","Bread" }, "2 Tins of Soup")]
        [DataRow(new string[] { "Apples"}, "10% off")]
        public void TestDisplayForOffers(string[] items,string expected)
        {
            ShoppingBasket basket = basketService.CreateShoppingBasket(items, Products);
            basketService.CalculateSubTotal(basket);
            basketService.CalculateDiscounts(basket, Discounts);
            string displayString = basketService.DisplayShoppingBasket(basket);
            Assert.IsTrue(displayString.Contains(expected));
        }
        [TestMethod]
        [DataRow(new string[] { "Apples"},"no offers available")]
        public void EmptyDiscounts(string[] items,string expected)
        {
            ShoppingBasket basket = basketService.CreateShoppingBasket(items, Products);
            basketService.CalculateSubTotal(basket);
            List<Discount> emptyDiscount = new List<Discount>();
            basketService.CalculateDiscounts(basket, emptyDiscount);
            string displayString = basketService.DisplayShoppingBasket(basket);
            Assert.IsTrue(displayString.Contains(expected));

        }
    }
}
