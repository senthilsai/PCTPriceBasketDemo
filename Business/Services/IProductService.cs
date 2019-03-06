using System.Collections.Generic;
using Business.Models;

namespace Business.Services
{
    public interface IProductService
    {
        /// <summary>
        /// Returns the path to the data file  with Products from the config
        /// </summary>
        /// <returns></returns>
        string GetDataFilePath();
        /// <summary>
        /// Gets all the products from the data file and returns a list
        /// </summary>
        /// <param name="dataFilePath">Path to the data file</param>
        /// <returns></returns>
        List<Product> GetAllProducts(string dataFilePath);
    }
}
