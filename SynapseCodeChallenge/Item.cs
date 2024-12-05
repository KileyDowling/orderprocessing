using Newtonsoft.Json;

public class Item
{
    public string ItemId { get; set; }

    [JsonProperty(PropertyName ="deliveryNotification")]
    public int DeliveryNotification { get; set; }
    public string Description { get; set; }
    public string Status {get; set;}

}