using System.Collections.ObjectModel;

namespace Domain.Models;

public record Sack(
    string Barcode,
    DeliveryPointType DeliveryPointType)
{
    private List<Package> packages = new List<Package>();

    public ReadOnlyCollection<Package> Packages => packages.AsReadOnly();

    public SackState State { get; private set; } = SackState.Created;

    public void AddPackage(Package package)
    {
        ArgumentNullException.ThrowIfNull(package);

        packages.Add(package);
        package.Load(this);
    }

    public void Unload(DeliveryPointType deliveryPoint)
    {
        foreach (var package in packages)
        {
            package.Unload(deliveryPoint);
        }

        State = SackState.Unloaded;
    }
}
