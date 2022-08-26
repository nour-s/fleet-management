using Domain.Exceptions;

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
            throw new DomainException($"Sack {Barcode} can't be unloaded to {deliveryPoint} because it is not the destination");
        }

        if (deliveryPoint == DeliveryPointType.Branch)
        {
            throw new DomainException($"Sack {Barcode} can't be unloaded to {deliveryPoint} because Branch doesn't accept it.");
        }

        foreach (var package in _packages)
        {
            package.Unload(deliveryPoint);
        }

        State = SackState.Unloaded;
    }

    internal void CheckIfAllShipmentsAreUnloaded()
    {
        if (_packages.Any() && _packages.All(x => x.State == PackageState.Unloaded))
        {
            State = SackState.Unloaded;
        }
    }
}
