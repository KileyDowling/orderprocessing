using Newtonsoft.Json.Linq;

public interface IOrderProcessingService
{
    Task<List<Order>> FetchMedicalEquipmentOrders();
    Order ProcessOrder(Order order);

    bool IsItemDelivered(Item item);

    void SendAlertMessage(Item item, string orderId);

    void IncrementDeliveryNotification(Item item);
    Task SendAlertAndUpdateOrder(Order order);
}