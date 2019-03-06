using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class ShoppingBasket
    {
        public List<ShoppingItem> ShoppingItems { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountSubTotal { get; set; }
    }
}
