namespace Domain.Models;

public record Package(
    string Barcode,
    DeliveryPointType DeliveryPointType,
    int Desi)
{

}