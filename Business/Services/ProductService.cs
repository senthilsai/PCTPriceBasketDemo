using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Business.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Configuration;
namespace Business.Services
{
    public class ProductService : IProductService
    {
        ILogger<ProductService> _logger;
        public ProductService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ProductService>();
        }
        public List<Product> GetAllProducts(string dataFilePath)
        {
            _logger.Log(LogLevel.Information, "Getting All Products");

            List<Product> Products = new List<Product>();
            try
            {
                using (StreamReader file = File.OpenText(dataFilePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    Products = (List<Product>)serializer.Deserialize(file, typeof(List<Product>));
                }
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, ex.Message + " Product File error");
                throw;
            }
            return Products;
        }

        public string GetDataFilePath()
        {
            string dataFilePath = ConfigurationManager.AppSettings["ProductsFile"];
            if (String.IsNullOrEmpty(dataFilePath))
            {
                _logger.Log(LogLevel.Error, "Configuration entry missing for ProductsFile");
                throw new Exception("Missing Configuration entry");
            }
            return dataFilePath;
        }
    }
}
