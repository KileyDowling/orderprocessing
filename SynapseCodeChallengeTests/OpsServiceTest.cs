using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;

namespace SynapseCodeChallengeTests;

public class OpsServiceTest
{
    private Order CurrentOrder;

    private IOrderProcessingService OPService;
    private Mock<ILogger>? _loggerMock;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger>();
        var opsService = new Mock<OrderService>();
        OPService = new OrderProcessingService(_loggerMock.Object, opsService.Object);

        CurrentOrder = new Order() { OrderId = "abc123", Items = [] };

        for (int i = 0; i <= 5; i++)
        {
            CurrentOrder.Items.Add(new Item()
            {
                ItemId = $"{i}",
                Description = $"item ${i}",
                Status = i % 2 == 0 ? "Delivered" : "Shipped",
                DeliveryNotification = 0
            });
        }
    }

    [Test]
    public void CheckOrderStatusItemDelivered()
    {
        var result = OPService.IsItemDelivered(CurrentOrder.Items[0]);
        Assert.That(result, Is.True);
    }

    [Test]
    public void CheckDeliveryNotiifcation()
    {
        OPService.IncrementDeliveryNotification(CurrentOrder.Items[1]);
        var deliveryNotif = CurrentOrder.Items[0].DeliveryNotification;
        Assert.That(deliveryNotif, Is.EqualTo(1));
    }
}
