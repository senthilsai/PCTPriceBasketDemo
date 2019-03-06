using System.Collections.Generic;
using Business.Models;

namespace Business.Services
{
    public interface IDiscountService
    {
        /// <summary>
        /// Gets Path to data file
        /// </summary>
        /// <returns>Data file path</returns>
        string GetDataFilePath();

        /// <summary>
        /// Gets all the discounts from the data file
        /// </summary>
        /// <param name="dataFilePath">Path to the discounts data file</param>
        /// <returns></returns>
        List<Discount> GetAllDiscounts(string dataFilePath);
    }
}
