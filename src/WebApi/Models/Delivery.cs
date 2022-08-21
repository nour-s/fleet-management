namespace WebApi.Models;

public class Delivery
{
    public string VehicleId { get; set; } = "";
    public List<Route> Routes { get; set; } = new List<Route>();
}
