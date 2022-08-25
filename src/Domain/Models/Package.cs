namespace Domain.Models;

public record Package(
    string Barcode,
    DeliveryPointType DeliveryPointType,
    int Desi)
{
    public int Id { get; init; }

    public PackageState State { get; internal set; } = PackageState.Created;

    /// <summary>
    /// The parent sack if exists
    /// </summary>
    public Sack? Sack { get; internal set; }

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

        if (Sack != null && deliveryPoint == DeliveryPointType.Branch)
        {
            throw new ArgumentException($"A Package {Barcode} without a sack can't be unloaded to {deliveryPoint}");
        }

        if (Sack == null && deliveryPoint == DeliveryPointType.TransferCentre)
        {
            throw new ArgumentException($"A Package {Barcode} without a sack can't be unloaded to {deliveryPoint}");
        }

        State = PackageState.Unloaded;
    }
}