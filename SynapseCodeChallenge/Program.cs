using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Synapse.OrdersExample
{
    /// <summary>
    /// I Get a list of orders from the API
    /// I check if the order is in a delviered state, If yes then send a delivery alert and add one to deliveryNotification
    /// I then update the order.   
    /// </summary>
    class Program
    {

        static void Main(string[] args)
        {
            var logger = CreateLogger();

            logger.LogInformation("Start of App");

            var ops = CreateProcessingService(logger);

            var medicalEquipmentOrders = ops.FetchMedicalEquipmentOrders().GetAwaiter().GetResult();
            foreach (var order in medicalEquipmentOrders)
            {
                var updatedOrder = ops.ProcessOrder(order);
                ops.SendAlertAndUpdateOrder(updatedOrder).GetAwaiter().GetResult();
            }

            logger.LogInformation("Results sent to relevant APIs.");
        }

        static ILogger CreateLogger()
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                    .AddConsole();
            });
            ILogger logger = loggerFactory.CreateLogger<Program>();
            return logger;
        }
    
        static IOrderProcessingService CreateProcessingService(ILogger logger){
            
            var apiUrls = new ApiUrls()
            {
                UpdateApiUrl = "https://update-api.com/update",
                OrdersApiUrl = "https://orders-api.com/orders",
                AlertApiUrl = "https://alert-api.com/alerts"
            };
            var httpClient = new HttpClient();
            var orderService = new OrderService(apiUrls, httpClient);

            return new OrderProcessingService(logger, orderService);
        }
    }
}