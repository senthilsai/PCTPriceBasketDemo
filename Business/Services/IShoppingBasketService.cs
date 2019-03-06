using System;
using System.Collections.Generic;
using System.Text;
using Business.Models;

namespace Business.Services
{
    public interface IShoppingBasketService
    {
        /// <summary>
        /// Creates a Shopping Basket from the list of items
        /// </summary>
        /// <param name="items">array of purchased items</param>
        /// <param name="products">List of all products in inventory</param>
        /// <returns>Shopping Basket</returns>
        ShoppingBasket CreateShoppingBasket(string[] items,List<Product> products);
        /// <summary>
        /// Calculates the Subtotal for the individual items in basket
        /// </summary>
        /// <param name="basket">Shopping bnasket with items</param>
        void CalculateSubTotal(ShoppingBasket basket);
        /// <summary>
        /// Calculates any available discounts
        /// </summary>
        /// <param name="basket">Shopping Basket</param>
        /// <param name="Discounts">All available discounts</param>
        void CalculateDiscounts(ShoppingBasket basket,List<Discount>Discounts);
        /// <summary>
        /// Displays the discounts for items purchased if applicable
        /// </summary>
        /// <param name="basket">Shopping Basket</param>
        /// <returns>text of discounts</returns>
        string DisplayShoppingBasket(ShoppingBasket basket);
        /// <summary>
        /// Displays the purchased items
        /// </summary>
        /// <param name="basket">Shopping Basket</param>
        /// <returns>text of purchased items</returns>
        string DisplayPurchasedItems(ShoppingBasket basket);



    }
}
