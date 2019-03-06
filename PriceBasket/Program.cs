using System;
using Business;
using System.Collections;
using Business.Services;
using Business.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace PriceBasket
{
    class Program
    {
        static void Main(string[] args)
        {
            Run(args);
            
        }
        private static void Run(string[] args)
        {
            
            try
            {
                //Set up DI
                var serviceProvider = new ServiceCollection()
                    .AddLogging(builder => builder
                    .AddConsole()
                    .AddFilter(level => level >= LogLevel.Information))
                    .AddSingleton<IDiscountService, DiscountService>()
                    .AddSingleton<IProductService, ProductService>()
                    .AddSingleton<IShoppingBasketService, ShoppingBasketService>()
                    .BuildServiceProvider();


                var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();

                // Get all Products
                var prodService = serviceProvider.GetService<IProductService>();
                string productDataFile = prodService.GetDataFilePath();
                List<Product> Products = prodService.GetAllProducts(productDataFile);

                // Get all Discounts
                var discountService = serviceProvider.GetService<IDiscountService>();
                string discountDataFile = discountService.GetDataFilePath();
                List<Discount> Discounts = discountService.GetAllDiscounts(discountDataFile);

                // Get Shopping Basket Service
                var shoppingBasketService = serviceProvider.GetService<IShoppingBasketService>();

                //Create the basket
                ShoppingBasket basket = shoppingBasketService.CreateShoppingBasket(args, Products);

                //Calculate the totals
                shoppingBasketService.CalculateSubTotal(basket);

                //Calculate the discounts if any
                shoppingBasketService.CalculateDiscounts(basket, Discounts);

                // Write the list of  items with Quantities
                Console.WriteLine(shoppingBasketService.DisplayPurchasedItems(basket));

                //Write the discounts and totals
                Console.WriteLine(shoppingBasketService.DisplayShoppingBasket(basket));
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error generating Shopping basket " + ex.Message);
            }
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
