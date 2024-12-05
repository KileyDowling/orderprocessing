using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class OrderProcessingService : IOrderProcessingService
{
    public ILogger Logger { get; set; }

    private IOrderService OrderService { get; set; }

    public OrderProcessingService(ILogger logger, IOrderService orderService)
    {
        Logger = logger;
        OrderService = orderService;
    }


    public async Task<List<Order>> FetchMedicalEquipmentOrders()
    {
        try
        {
            var response = await OrderService.GetOrdersAsync();
            if (response.IsSuccessStatusCode)
            {
                var ordersData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Order>>(ordersData) ?? [];
            }

            else
            {
                Logger.LogError("Failed to fetch orders from API.");
            }
        }
        catch (Exception ex)
        {
            Logger.LogError($"{ex.Message}", ex);
        }
        return [];

    }

    public Order ProcessOrder(Order order)
    {
        foreach (var item in order.Items)
        {
            if (IsItemDelivered(item))
            {
                SendAlertMessage(item, order.OrderId);
                IncrementDeliveryNotification(item);
            }
        }

        return order;
    }

    public bool IsItemDelivered(Item item)
    {
        return item.Status.Equals("Delivered", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Delivery alert
    /// </summary>
    /// <param name="orderId">The order id for the alert</param>
    /// <param name="item">The item for the alert</param>
    public async void SendAlertMessage(Item item, string orderId)
    {
        var alertData = new
        {
            Message = $"Alert for delivered item: Order {orderId}, Item: {item.Description}, " +
                      $"Delivery Notifications: {item.DeliveryNotification}"
        };
        var content = new StringContent(JObject.FromObject(alertData).ToString(), System.Text.Encoding.UTF8, "application/json");

        try
        {
            var response = await OrderService.SendAlert(content);

            if (response.IsSuccessStatusCode)
            {
                Logger.LogInformation($"Alert sent for delivered item: {item.Description}");
            }
            else
            {
                Logger.LogError($"Failed to send alert for delivered item: {item.Description}");
            }
        }
        catch (Exception ex)
        {
            Logger.LogError($"{ex.Message}", ex);
        }
    }

    public void IncrementDeliveryNotification(Item item)
    {
        item.DeliveryNotification = item.DeliveryNotification += 1;
    }

    public async Task SendAlertAndUpdateOrder(Order currentOrder)
    {
        var order = JsonConvert.SerializeObject(currentOrder);

        var content = new StringContent(order.ToString(), System.Text.Encoding.UTF8, "application/json");
        try
        {
            var response = await OrderService.UpdateOrder(content);

            if (response.IsSuccessStatusCode)
            {
                Logger.LogInformation($"Updated order sent for processing: OrderId {currentOrder.OrderId}");
            }
            else
            {
                Logger.LogError($"Failed to send updated order for processing: OrderId {currentOrder.OrderId}");
            }
        }
        catch (Exception ex)
        {
            Logger.LogError($"{ex.Message}", ex);
        }
    }
}