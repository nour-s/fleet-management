using System.Collections.ObjectModel;

namespace Domain.Models;

public record Sack(
    string Barcode,
    DeliveryPointType DeliveryPointType)
{
    public int Id { get; init; }

    private List<Package> _packages = new List<Package>();

    public IEnumerable<Package> Packages => _packages;

    public SackState State { get; internal set; } = SackState.Created;

    public void AddPackage(Package package)
    {
        ArgumentNullException.ThrowIfNull(package);
        _packages.Add(package);
    }

    public void Load()
    {
        State = SackState.Loaded;
        foreach (var package in _packages)
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

        foreach (var package in _packages)
        {
            package.Unload(deliveryPoint);
        }

        State = SackState.Unloaded;
    }
}
