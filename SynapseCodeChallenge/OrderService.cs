
public class ApiUrls {
    public string UpdateApiUrl { get; set; }
    public string OrdersApiUrl { get; set; }
    public string AlertApiUrl { get; set; }
}


public class OrderService : IOrderService
{
    private ApiUrls apiUrls {get; set;}
    private HttpClient httpClient {get; set;}

    public OrderService(ApiUrls _apiUrls, HttpClient _httpClient)
    {
        apiUrls = _apiUrls;
        httpClient = _httpClient;
    }

    public async Task<HttpResponseMessage> GetOrdersAsync()
    {
        return await httpClient.GetAsync(apiUrls.OrdersApiUrl);
    }

    public async Task<HttpResponseMessage>  UpdateOrder(StringContent content)
    {
        return await httpClient.PostAsync(apiUrls.UpdateApiUrl, content);
    }

       public async Task<HttpResponseMessage> SendAlert(StringContent content)
    {
        return await httpClient.PostAsync(apiUrls.AlertApiUrl, content);
    }
}
