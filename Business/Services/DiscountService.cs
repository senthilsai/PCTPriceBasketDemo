using System;
using System.Collections.Generic;
using Business.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.IO;

namespace Business.Services
{
    public class DiscountService : IDiscountService
    {
        ILogger<DiscountService> _logger;
        public DiscountService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DiscountService>();
        }
        public List<Discount> GetAllDiscounts(string dataFilePath)
        {
            _logger.Log(LogLevel.Information, "Getting All Discounts");
            List<Discount> Discounts = new List<Discount>();
            try
            {
                //Get the Discounts from the JSON file
                using (StreamReader file = File.OpenText(dataFilePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    Discounts = (List<Discount>)serializer.Deserialize(file, typeof(List<Discount>));
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, " Error getting discounts data " + ex.Message);
                throw;
            }
            return Discounts;
        }

        public string GetDataFilePath()
        {
            //Get file path from config file
            string dataFilePath = ConfigurationManager.AppSettings["DiscountsFile"];
            if (String.IsNullOrEmpty(dataFilePath))
            {
                _logger.Log(LogLevel.Error, "Configuration entry missing for DiscountsFile");
                throw new Exception("Discount File Configuration missing or invalid");
            }
            return dataFilePath;
        }
    }
}
