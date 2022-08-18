namespace Domain.Models;

public record Package(
    string Barcode,
    DeliveryPointType DeliveryPointType,
    int Desi)
{
    public PackageState State { get; private set; } = PackageState.Created;

    public void Unload(DeliveryPointType deliveryPoint)
    {
        State = PackageState.Unloaded;
    }
}