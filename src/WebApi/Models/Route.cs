namespace WebApi.Models;

public class Route
{
    public int DeliveryPoint { get; set; }
    public List<Shipment> Deliveries { get; set; } = new List<Shipment>();
}
