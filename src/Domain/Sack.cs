using System.Collections.ObjectModel;

namespace Domain;

public record Sack(
    string Barcode,
    DeliveryPointType DeliveryPointType)
{
    private List<Package> packages = new List<Package>();

    public ReadOnlyCollection<Package> Packages => packages.AsReadOnly();

    public void AddPackage(Package package)
    {
        packages.Add(package);
    }
}
