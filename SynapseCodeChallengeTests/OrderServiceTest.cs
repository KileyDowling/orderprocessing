using System.Net;
using Moq;
using Moq.Protected;

namespace SynapseCodeChallengeTests;

public class OrderServiceTests
{
    private IOrderService orderService;

    [SetUp]
    public void Setup()
    {
        var apiUrls = new Mock<ApiUrls>();

        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

        HttpResponseMessage httpResponseMessage = new()
        {
            Content = new StringContent(GenerateOrders())
        };

        httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponseMessage);

        var httpClient = new HttpClient(httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };

        orderService = new OrderService(apiUrls.Object, httpClient);
    }

    [Test]
    public void GetOrdersAsyncTest()
    {
        var results = orderService.GetOrdersAsync().Result;
        var orders = GenerateOrders();
        var length = orders.Length;
        Assert.That(results.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(results.Content.Headers.ContentLength, Is.EqualTo(length));
    }

    public string GenerateOrders()
    {
        return "[{'OrderId':'abc123','Items':'[{'ItemId':'1','deliveryNotification':0,'Description':'new description','Status':'Shipped'}]']";
    }

}

