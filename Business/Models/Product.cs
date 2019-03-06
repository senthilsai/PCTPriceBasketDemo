using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class Product
    {
        /// <summary>
        /// Unique Id for Product
        /// </summary>
        public int ProductID { get; set; }
        /// <summary>
        /// Name of Product
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Unit Price
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// Unit (for display)
        /// </summary>
        public string SingleUnit { get; set; }
        /// <summary>
        /// Multiple Units (for display)
        /// </summary>
        public string MultipleUnits { get; set; }
    }
}
