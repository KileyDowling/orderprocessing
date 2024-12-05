public interface IOrderService 
{
    Task<HttpResponseMessage> GetOrdersAsync();
    Task<HttpResponseMessage> UpdateOrder(StringContent content);
   Task<HttpResponseMessage> SendAlert(StringContent content);
}