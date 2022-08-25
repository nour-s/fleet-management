using System.Collections.ObjectModel;

namespace Domain.Models;

public record Sack(
    string Barcode,
    DeliveryPointType DeliveryPointType)
{
    public int Id { get; init; }

    private List<Package> packages = new List<Package>();

    public ReadOnlyCollection<Package> Packages => packages.AsReadOnly();

    public SackState State { get; internal set; } = SackState.Created;

    public void AddPackage(Package package)
    {
        ArgumentNullException.ThrowIfNull(package);
        packages.Add(package);
    }

    public void Load()
    {
        State = SackState.Loaded;
        foreach (var package in packages)
        {
            package.Load();
        }
    }

    public void Unload(DeliveryPointType deliveryPoint)
    {
        if (DeliveryPointType != deliveryPoint)
        {
            throw new ArgumentException($"Sack {Barcode} can't be unloaded to {deliveryPoint}");
        }

        foreach (var package in packages)
        {
            package.Unload(deliveryPoint);
        }

        State = SackState.Unloaded;
    }
}
