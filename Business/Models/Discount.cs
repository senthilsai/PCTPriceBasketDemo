using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class Discount
    {
        /// <summary>
        /// Product for which discount is applicable
        /// </summary>
        public int DiscountedProductID { get; set; }
        /// <summary>
        /// Product that needs to be purchased to avail discount
        /// </summary>
        public int EligibleProductID { get; set; }
        /// <summary>
        /// Quantity that needs to be purchased to avail discount
        /// </summary>
        public int EligibleQuantity { get; set; }
        /// <summary>
        /// Percentage discount (50 is 50% discount)
        /// </summary>
        public decimal DiscountPercent { get; set; }
        /// <summary>
        /// Description of discount
        /// </summary>
        public string DiscountDescription { get; set; }

    }
}
