using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class ShoppingItem
    {
        /// <summary>
        /// Product purchased
        /// </summary>
        public Product PurchasedProduct { get; set; }
        /// <summary>
        /// Total Units
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Price without discount
        /// </summary>
        public decimal SubtotalWithoutDiscount { get; set; }
        /// <summary>
        /// Discount Amount
        /// </summary>
        public decimal DiscountAmount { get; set; } = 0;
        public string DiscountDescription { get; set; }
        
    }
}
