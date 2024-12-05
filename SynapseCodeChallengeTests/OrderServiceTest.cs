using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;

namespace SynapseCodeChallengeTests;

public class OrderServiceTests
{
    private IOrderService orderService;
    private Mock<ILogger> _loggerMock;

    [SetUp]
    public void Setup()
    {
        var apiUrls = new Mock<ApiUrls>();
        orderService = new OrderService(apiUrls.Object);

    }

    [Test]
    public void CheckOrderStatusItemDelivered()
    {
    }

}
