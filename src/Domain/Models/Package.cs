namespace Domain.Models;

public record Package(
    string Barcode,
    DeliveryPointType DeliveryPointType,
    int Desi)
{
    public PackageState State { get; private set; } = PackageState.Created;

    /// <summary>
    /// The parent sack if exists
    /// </summary>
    public Sack? Sack { get; private set; }

    public void Load()
    {
        State = Sack != null ? PackageState.LoadedInSack : PackageState.Loaded;
    }

    public void Unload(DeliveryPointType deliveryPoint)
    {
        if (DeliveryPointType != deliveryPoint)
        {
            throw new ArgumentException($"Package {Barcode} can't be unloaded to {deliveryPoint}");
        }

        State = PackageState.Unloaded;
    }
}