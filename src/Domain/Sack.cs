using System.Collections.ObjectModel;

namespace Domain;
public record Sack(
    string Barcode,
    DeliveryPointType DeliveryPointType)
{
    private List<Package> packages = new List<Package>();

    ReadOnlyCollection<Package> Packages => packages.AsReadOnly();
}
