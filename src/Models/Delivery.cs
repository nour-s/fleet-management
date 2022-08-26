using System.Text.Json.Serialization;

namespace WebApi.Models;

public class Delivery
{
    [JsonPropertyName("vehicle")]
    public string VehicleId { get; set; } = "";

    [JsonPropertyName("route")]
    public List<Route> Routes { get; set; } = new List<Route>();
}
