
public class ApiUrls {
    public string UpdateApiUrl { get; set; }
    public string OrdersApiUrl { get; set; }
    public string AlertApiUrl { get; set; }
}


public class OrderService : IOrderService
{
    private ApiUrls apiUrls {get; set;}

    public OrderService(ApiUrls _apiUrls)
    {
        apiUrls = _apiUrls;
    }

    public async Task<HttpResponseMessage> GetOrdersAsync()
    {
        using HttpClient httpClient = new();
        return await httpClient.GetAsync(apiUrls.OrdersApiUrl);
    }

    public async Task<HttpResponseMessage>  UpdateOrder(StringContent content)
    {
        using HttpClient httpClient = new();
        return await httpClient.PostAsync(apiUrls.UpdateApiUrl, content);
    }

       public async Task<HttpResponseMessage> SendAlert(StringContent content)
    {
        using HttpClient httpClient = new();
        return await httpClient.PostAsync(apiUrls.AlertApiUrl, content);
    }
}
