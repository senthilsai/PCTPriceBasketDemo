using System;
using System.Collections.Generic;
using System.Text;
using Business.Models;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class ShoppingBasketService : IShoppingBasketService
    {
        ILogger<ShoppingBasketService> _logger;
        public ShoppingBasketService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ShoppingBasketService>();
        }
        public ShoppingBasket CreateShoppingBasket(string[] items,List<Product>products)
        {
            _logger.Log(LogLevel.Information, "Creating Shopping Basket");
            ShoppingBasket basket = new ShoppingBasket()
            {
                ShoppingItems = new List<ShoppingItem>()
            };
            try
            {
                foreach (string shoppingItemName in items)
                {
                    //Check if item exists in catalogue
                    Product product = products.Find(x => (x.ProductName.ToLower() == shoppingItemName.ToLower()));
                    if (product != null)
                    {
                        // if already in cart increment quantity
                        ShoppingItem item = basket.ShoppingItems.Find(x => (x.PurchasedProduct.ProductID == product.ProductID));
                        if (item != null)
                        {
                            item.Quantity++;
                        }
                        else
                        {
                            //Add the item to the basket
                            basket.ShoppingItems.Add(new ShoppingItem()
                            {
                                PurchasedProduct = product,
                                Quantity = 1

                            });
                        }

                    }
                    else
                    {
                        _logger.Log(LogLevel.Error,$"The following product was not found {shoppingItemName}");
                        throw new ArgumentException();
                    }

                }
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, $"Exception Creating Shopping basket {ex.Message}");
                throw;
            }
            return basket;
        }
        public void CalculateSubTotal(ShoppingBasket basket)
        {
            _logger.Log(LogLevel.Information, "Calculating Subtotals");

            decimal subTotal = 0;
            try
            {
                foreach (ShoppingItem item in basket.ShoppingItems)
                {
                    //Add the  amounts without discount
                    item.SubtotalWithoutDiscount = item.PurchasedProduct.UnitPrice * item.Quantity;
                    subTotal += item.SubtotalWithoutDiscount;

                }
                basket.SubTotal = subTotal;
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Exception Calculating SubTotals");
                throw;
            }

        }
        public void CalculateDiscounts(ShoppingBasket basket,List<Discount>discounts)
        {
            decimal discountSubTotal = 0;
            _logger.Log(LogLevel.Information, "Calculating discounts");

            try
            {

                foreach (ShoppingItem item in basket.ShoppingItems)
                {
                    Discount availableDiscount = discounts.Find(x => x.DiscountedProductID == item.PurchasedProduct.ProductID);
                    if (availableDiscount != null)
                    {
                        // Find if we have purchased an eligible item and if so how many
                        ShoppingItem eligibleItem = basket.ShoppingItems.Find(x => x.PurchasedProduct.ProductID == availableDiscount.EligibleProductID);
                        int purchasedQuantity = eligibleItem.Quantity;
                        int discountCount = purchasedQuantity / availableDiscount.EligibleQuantity;

                        decimal singleProductDiscount = (availableDiscount.DiscountPercent * item.PurchasedProduct.UnitPrice) / 100;
                        item.DiscountAmount = singleProductDiscount * discountCount;
                        string discountString = item.PurchasedProduct.ProductName + " " + availableDiscount.DiscountDescription;
                        item.DiscountDescription = discountString;
                        discountSubTotal += item.DiscountAmount;
                    }

                }
                basket.DiscountSubTotal = discountSubTotal;
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Exception Calculating Discounts");
                throw;
            }
        }
        public string DisplayShoppingBasket(ShoppingBasket basket)
        {
            
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append($"SubTotal : {FormatPoundsAndPence(basket.SubTotal)}\n\n");
                sb.Append("Special Offers\n==============\n");

                foreach (ShoppingItem item in basket.ShoppingItems)
                {
                    if (item.DiscountAmount > 0)
                    {
                        sb.Append($"{item.DiscountDescription} : ");
                        sb.Append($"-{FormatPoundsAndPence(item.DiscountAmount)}\n");
                    }
                }
                if (basket.DiscountSubTotal == 0)
                {
                    sb.Append("(no offers available)\n");
                }
                sb.Append($"\nTotal : {FormatPoundsAndPence(basket.SubTotal - basket.DiscountSubTotal)}\n");
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Exception Displaying Shopping Basket");
                throw;
            }
            return sb.ToString();
        }
        public string DisplayPurchasedItems(ShoppingBasket basket)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                // Write Header
                sb.Append("Your Shopping Basket\n");
                sb.Append("====================\n\n");
                sb.Append($"{"Item",-20}{"Quantity",-20}{"Unit Price",-20}");
                sb.Append($"{"Total Price"}\n");
                sb.Append($"{"====",-20}{"========",-20}{"==========",-20}");
                sb.Append($"{"==========="}\n\n");
                foreach (ShoppingItem item in basket.ShoppingItems)
                {
                    sb.Append($"{item.PurchasedProduct.ProductName,-20}");
                    sb.Append($"{item.Quantity.ToString(),-20}");
                    sb.Append($"{item.PurchasedProduct.UnitPrice.ToString("C2"),-20}");
                    sb.Append($"{item.SubtotalWithoutDiscount.ToString("C2")}\n\n");
                }
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Exception Displaying Purchased Items");
                throw;
            }
            
            return sb.ToString();
        }
        private string FormatPoundsAndPence(decimal amount)
        {
            try
            {
                if (amount < 1)
                {
                    return ($"{(amount * 100):F0} p");
                }
                else
                {
                    return ($"{amount:C2}");
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Exception Formatting Pounds And Pence");
                throw;
            }
        }
    }
}
