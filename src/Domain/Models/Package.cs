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

    public void Load(Sack sack)
    {
        Sack = sack;
        State = PackageState.LoadedInSack;
    }

    public void Unload(DeliveryPointType deliveryPoint)
    {
        State = PackageState.Unloaded;
    }
}